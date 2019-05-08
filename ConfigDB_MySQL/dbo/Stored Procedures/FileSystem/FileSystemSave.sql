DELIMITER ;;
CREATE PROCEDURE `FileSystemSave`(
	IN p_Id				varchar(64), 
	IN p_SiteId			varchar(64),
	IN p_Filename		varchar(255),
	IN p_FileExtension	varchar(255), 
	IN p_DirectoryName	longtext,
	IN p_Created		datetime, 
	IN p_CreatedBy		varchar(64), 
	IN p_Modified		datetime, 
	IN p_ModifiedBy		varchar(64), 
	IN p_is_directory	bit, 
	IN p_is_readonly	bit,
	IN p_is_archive		bit,
	IN p_is_system		bit, 
	IN p_DoNotUpdate	bit
)
BEGIN
	
	DECLARE v_RootFolder		longtext;
    DECLARE v_parentIsSystem	bit;
	DECLARE v_parentNodeId		varchar(64);
	DECLARE v_EmptyGuid			varchar(64);
    DECLARE v_ContentStream		longblob;
    DECLARE v_WorkflowInstanceId	varchar(64);
    DECLARE	v_rowCount			bigint;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION 
	BEGIN
		rollback;
	END;
        
    SET v_RootFolder = CONCAT('corkscrew://', p_SiteId, '/');
	SET v_parentIsSystem = 0;
    SET v_parentNodeId = null;
    SET v_EmptyGuid = `GuidDefault`();
    SET v_WorkflowInstanceId = uuid();

	IF ((SELECT count(*) FROM `FileSystem` WHERE (`FullPath` = v_RootFolder)) = 0) THEN
		call raise_error;
	END IF;

	SET p_DirectoryName = IFNULL(p_DirectoryName, v_RootFolder);
    
    IF ((SELECT count(*) FROM `FileSystem` WHERE (`FullPath` = p_DirectoryName)) = 0) THEN 
		call raise_error;
    END IF;

	IF (IFNULL(p_Filename, '') = '') THEN 
		SET p_Filename = null;
	END IF;
    
	IF (IFNULL(p_FileExtension, '') = '') THEN 
		SET p_FileExtension = null;
	END IF;
    
    START transaction;

	IF ((SELECT count(*) FROM `FileSystem` WHERE (`Id` = p_Id)) = 0) THEN 
		INSERT INTO `FileSystem` 
			( `Id`, `SiteId`, `Filename`, `FilenameExtension`, `DirectoryName`, 
				`Created`, `CreatedBy`, `Modified`, `ModifiedBy`, `LastAccessed`, `LastAccessedBy`, 
					`is_directory`, `is_readonly`, `is_archive`, `is_system`,
						`ContentStream` )
		VALUES 
		(
			p_Id, p_SiteId, p_Filename, p_FileExtension, p_DirectoryName, 
				p_Created, p_CreatedBy, p_Modified, p_ModifiedBy, p_Modified, p_ModifiedBy, 
					p_is_directory, p_is_readonly, p_is_archive, p_is_system, 
							/* `ContentStream` */
							CASE 
								WHEN p_is_directory = 1
									THEN NULL 
								ELSE 
									null
							END
		);
        
        INSERT INTO `FileSystemChangeLog` 
			( `Id`, `FileSystemId`, `SiteId`, `IsProcessed`, `ChangeType`, `ChangeTimeStamp`, `ChangedBy`, 
				`PreviousData`, `PreviousFilename`, `PreviousFilenameExtension`, `PreviousDirectoryName`,  
					`PreviousCreated`, `PreviousCreatedBy`, `PreviousModified`, `PreviousModifiedBy`,
						`was_directory`, `was_readonly`, `was_archive`, `was_system` )
		VALUES
		(
			UUID(), p_Id, p_SiteId, 0, 'I', p_Modified, p_ModifiedBy, 
				null, p_Filename, p_FileExtension, p_DirectoryName, 
					p_Created, p_CreatedBy, p_Modified, p_ModifiedBy, 
						p_is_directory, p_is_readonly, p_is_archive, p_is_system
						
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
								`FarmId`, p_Id, NULL, NULL, 
									0, 0, NULL, NULL, 0, if ((p_is_directory = 1), 'directory_created', 'file_created'),
										v_RightNow, p_Modified, v_RightNow, p_Modified
					FROM `WorkflowAssociations` 
					WHERE (
							(
								(
									(`is_directory_scope` = 1) 
									AND (
											((p_is_directory = 1) AND (`subscribe_event_directory_created` = 1)) 
											OR ((p_is_directory = 0) AND (`subscribe_event_file_created` = 1))
									)
								)
								OR (
										(`is_site_scope` = 1) 
										AND (
												((p_is_directory = 1) AND (`subscribe_event_directory_created` = 0) AND (`subscribe_event_catch_bubbledevents` = 1)) 
												OR ((p_is_directory = 0) AND (`subscribe_event_file_created` = 0) AND (`subscribe_event_catch_bubbledevents` = 1))
										)
								)
							)
							AND (`start_on_create` = 1) 
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
							uuid(), `WorkflowAssociationId`, `Id`, 
								`AssociationData`, `CurrentState`, `CompletedReason`, `ErrorMessage`,
									`Modified`, `ModifiedBy`
						FROM `WorkflowInstances` 
						WHERE (`Id` = v_WorkflowInstanceId);
					END IF;
				END IF;
			end if;
                            
	ELSE IF (p_DoNotUpdate = 0) THEN
            UPDATE `FileSystem` 
				SET 
					`Filename`			= p_Filename, 
					`FilenameExtension`	= p_FileExtension,
					`DirectoryName`		= p_DirectoryName, 
					`Modified`			= p_Modified, 
					`ModifiedBy`		= p_ModifiedBy, 
					`LastAccessed`		= p_Modified, 
					`LastAccessedBy`	= p_ModifiedBy, 
					`is_readonly`		= p_is_readonly, 
					`is_archive`		= p_is_archive
					/* 
						We should never change the is_directory and is_system flags
					*/
			WHERE (
				(`Id` = p_Id) 
				AND (`SiteId` = p_SiteId)
			);
            
            SELECT `ContentStream` from `filesystem` where ((`Id` = p_Id) AND (`SiteId` = p_SiteId)) into v_ContentStream;
            
            INSERT INTO `FileSystemChangeLog` 
				( `Id`, `FileSystemId`, `SiteId`, `IsProcessed`, `ChangeType`, `ChangeTimeStamp`, `ChangedBy`, 
					`PreviousData`, `PreviousFilename`, `PreviousFilenameExtension`, `PreviousDirectoryName`,  
						`PreviousCreated`, `PreviousCreatedBy`, `PreviousModified`, `PreviousModifiedBy`,
							`was_directory`, `was_readonly`, `was_archive`, `was_system` )
			VALUES
			(
				UUID(), p_Id, p_SiteId, 0, 'UP', p_Modified, p_ModifiedBy, 
					v_ContentStream, p_Filename, p_FileExtension, p_DirectoryName, 
						p_Created, p_CreatedBy, p_Modified, p_ModifiedBy, 
							p_is_directory, p_is_readonly, p_is_archive, p_is_system
							
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

		END IF;
	END IF;
    
    COMMIT;
    
END ;;
DELIMITER ;