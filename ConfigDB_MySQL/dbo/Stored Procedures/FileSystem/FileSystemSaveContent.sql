DELIMITER ;;
CREATE PROCEDURE `FileSystemSaveContent`(
	IN p_Id				varchar(64), 
	IN p_Modified		datetime, 
	IN p_ModifiedBy		varchar(64), 
    IN p_Content		longblob
)
BEGIN
	
    DECLARE v_ContentStream		longblob;
    DECLARE v_rowCount			bigint;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION 
	BEGIN
		rollback;
	END;

	if ((SELECT count(*) from `filesystem` where ((`Id` = p_Id) AND (`SiteId` = p_SiteId) and (`is_directory` = 0))) = 1) then 

		start transaction;

			SELECT `ContentStream` from `filesystem` where ((`Id` = p_Id) AND (`SiteId` = p_SiteId) and (`is_directory` = 0)) into v_ContentStream;
					
			INSERT INTO `FileSystemChangeLog` 
				( `Id`, `FileSystemId`, `SiteId`, `IsProcessed`, `ChangeType`, `ChangeTimeStamp`, `ChangedBy`, 
					`PreviousData`, `PreviousFilename`, `PreviousFilenameExtension`, `PreviousDirectoryName`,  
						`PreviousCreated`, `PreviousCreatedBy`, `PreviousModified`, `PreviousModifiedBy`,
							`was_directory`, `was_readonly`, `was_archive`, `was_system` )
			VALUES
			(
				UUID(), p_Id, p_SiteId, 0, 'UC', p_Modified, p_ModifiedBy, 
					v_ContentStream, p_Filename, p_FileExtension, p_DirectoryName, 
						p_Created, p_CreatedBy, p_Modified, p_ModifiedBy, 
							p_is_directory, p_is_readonly, p_is_archive, p_is_system
							
			);
			
			UPDATE `FileSystem` 
				SET 
					`ContentStream`		= p_Content,
					`Modified`			= p_Modified, 
					`ModifiedBy`		= p_ModifiedBy, 
					`LastAccessed`		= p_Modified, 
					`LastAccessedBy`	= p_ModifiedBy
			WHERE (
				(`Id` = p_Id) 
				and (`is_directory` = 0)
			);
			
			SET v_ContentStream = null;
            
            if ((select count(*) from information_schema.tables where ((`TABLE_SCHEMA` = 'Corkscrew_ConfigDB') and (`TABLE_NAME`='WorkflowInstances'))) > 0) then 
				INSERT INTO `WorkflowInstances` 
					(
						`Id`, `WorkflowAssociationId`, `WorkflowDefinitionId`, 
							`AssociationData`, 
								`FarmId`, `SiteId`, `DirectoryId`, `FileId`, 
									`CurrentState`, `CompletedReason`, `ErrorMessage`, `SqlPersistenceId`, `IsLoadedInRuntime`, `InstanceStartEvent`, 
										`Created`, `CreatedBy`, `Modified`, `ModifiedBy`
					)
					SELECT
						v_WorkflowInstanceId, `Id`, `WorkflowDefinitionId`, 
							`AssociationData`, 
								`FarmId`, p_Id, NULL, NULL, 
									0, 0, NULL, NULL, 0, if ((p_is_directory = 1), 'directory_modified', 'file_modified'),
										p_Modified, p_ModifiedBy, p_Modified, p_ModifiedBy
					FROM `WorkflowAssociations`  
					WHERE (
						(
							(
								(`is_directory_scope` = 1) 
								AND (
										((p_is_directory = 1) AND (`subscribe_event_directory_modified` = 1)) 
										OR ((p_is_directory = 0) AND (`subscribe_event_file_modified` = 1))
								)
							)
							OR (
									(`is_site_scope` = 1) 
									AND (
											((p_is_directory = 1) AND (`subscribe_event_directory_modified` = 0) AND (`subscribe_event_catch_bubbledevents` = 1)) 
											OR ((p_is_directory = 0) AND (`subscribe_event_file_modified` = 0) AND (`subscribe_event_catch_bubbledevents` = 1))
									)
							)
						) 
						AND (`start_on_modify` = 1)
						AND (`is_enabled` = 1) 
						AND (`prevent_new_instances` = 0)
					);
					
				
			
				SELECT ROW_COUNT() INTO v_rowCount;

				IF (v_rowCount > 0) THEN
					if ((select count(*) from information_schema.tables where ((`TABLE_SCHEMA` = 'Corkscrew_ConfigDB') and (`TABLE_NAME`='WorkflowHistory'))) > 0) then 
						INSERT INTO `WorkflowHistory` 
						(
							`Id`, `WorkflowAssociationId`, `WorkflowInstanceId`, 
								`AssociationData`, `State`, `CompletedReason`, `ErrorMessage`, 
									`Created`, `CreatedBy`
						)
						SELECT
							NEWID(), `WorkflowAssociationId`, `Id`, 
								`AssociationData`, `CurrentState`, `CompletedReason`, `ErrorMessage`,
									`Modified`, `ModifiedBy`
						FROM `WorkflowInstances`  
						WHERE (`Id` = p_WorkflowInstanceId);
					END IF;
				END IF;
			END IF;

		commit;

    end if;
    
END ;;
DELIMITER ;