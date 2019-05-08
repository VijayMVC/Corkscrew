DELIMITER ;;
CREATE PROCEDURE `FileSystemDeleteById`(
	IN p_Id				varchar(64),
    IN p_DeleteUserId	varchar(64)
)
BEGIN
	
    declare v_DeletedItemFullPath			longtext;
    declare v_IsFolder						bit;
    declare	v_WorkflowInstanceId			varchar(64);
    declare v_RightNow						datetime;
    declare	v_rowCount						bigint;
    
    
	set v_DeletedItemFullPath = null;
    set v_IsFolder = 0;
    set v_WorkflowInstanceId = uuid();
    set v_RightNow = now();
    
    if ((select count(*) from `FileSystem` where (`Id` = p_Id)) = 0) then 
		call raise_error;
	end if;
    
    select `is_directory`, `FullPath` from `filesystem` where (`Id` = p_Id) into v_IsFolder, v_DeletedItemFullPath;
    
    if (v_DeletedItemFullPath is not null) then 
    
		INSERT INTO `FileSystemChangeLog` 
			( `Id`, `FileSystemId`, `SiteId`, `IsProcessed`, `ChangeType`, `ChangeTimeStamp`, `ChangedBy`, 
					`PreviousData`, `PreviousFilename`, `PreviousFilenameExtension`, `PreviousDirectoryName`,  
						`PreviousCreated`, `PreviousCreatedBy`, `PreviousModified`, `PreviousModifiedBy`,
							`was_directory`, `was_readonly`, `was_archive`, `was_system` )
				SELECT 
					newid(), p_Id, `SiteId`, 0, 'D', v_RightNow, p_DeleteUserId, 
						`ContentStream`, `Filename`, `FilenameExtension`, `DirectoryName`, 
							`Created`, `CreatedBy`, `Modified`, `ModifiedBy`, 
								`is_directory`, `is_readonly`, `is_archive`, `is_system` 
				FROM `FileSystem` 
				WHERE (
						(
							(`FullPath` = v_DeletedItemFullPath)				
							OR (`FullPath` LIKE CONCAT(v_DeletedItemFullPath,'/%'))	
						)
				);
		
		delete from `FileSystem` 
			where (
				(`FullPath` = v_DeletedItemFullPath) 
                OR (`FullPath` like concat(v_DeletedItemFullPath, '/%'))
            );
            
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
							`FarmId`, v_Id, NULL, NULL, 
								0, 0, NULL, NULL, 0, 
									CASE v_is_directory 
										WHEN 1 THEN 'directory_deleted' 
										ELSE 'file_deleted' 
									END,
									v_RightNow, p_DeleteUserId, v_RightNow, p_DeleteUserId
				FROM `WorkflowAssociations` 
				WHERE (
					(
						(
							(`is_directory_scope` = 1) 
							AND (
									((v_is_directory = 1) AND (`subscribe_event_directory_deleted` = 1)) 
									OR ((v_is_directory = 0) AND (`subscribe_event_file_deleted` = 1))
							)
						)
						OR (
								(`is_site_scope` = 1) 
								AND (
										((v_is_directory = 1) AND (`subscribe_event_directory_deleted` = 0) AND (`subscribe_event_catch_bubbledevents` = 1)) 
										OR ((v_is_directory = 0) AND (`subscribe_event_file_deleted` = 0) AND (`subscribe_event_catch_bubbledevents` = 1))
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
					WHERE (`Id` = v_WorkflowInstanceId);
				END IF;
			END IF;
		END IF;
        
	end if;
   
    
END ;;
DELIMITER ;