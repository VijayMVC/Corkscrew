DELIMITER ;;
CREATE PROCEDURE `SiteSave`(
	IN p_Id					varchar(64),
    IN p_Name				varchar(255),
    IN p_Description		varchar(512),
    IN p_Created			datetime,
    IN p_CreatedBy			varchar(64),
    IN p_Modified			datetime,
    IN p_ModifiedBy			varchar(64),
    IN p_ContentDBServer	varchar(255),
    IN p_ContentDBName		varchar(255),
    IN p_QuotaBytes			long
)
BEGIN

	declare v_RootFolderId			varchar(64);
    declare	v_rowCount				bigint;
    declare	v_RightNow				datetime;
    declare v_WorkflowInstanceId	varchar(64);

	declare exit handler for sqlexception
    begin
		rollback;
	end;

	/* 
		we cannot create sitedb tables in mysql yet -- cannot copy sprocs. So force db to be configdb server! 
		Remove the below line if we fix this in the future.
	*/
	SET p_ContentDBName = 'Corkscrew_ConfigDB';
    
    SET v_RightNow = now();
    SET v_WorkflowInstanceId = uuid();
    
	if ((select count(*) from `Sites` where (`Id` = p_Id)) = 0) then 
        
			SET v_RootFolderId = UUID();

			start transaction;

			insert into `Sites` 
				( `Id`, `Name`, `Description`, 
					`ContentDBServerName`, `ContentDBName`, `QuotaBytes`,
						`Created`, `CreatedBy`, `Modified`, `ModifiedBy`) 
				values 
                (
					p_Id, p_Name, p_Description, 
						p_ContentDBServer, p_ContentDBName, p_QuotaBytes, 
							p_Created, p_CreatedBy, p_Modified, p_ModifiedBy
                );
        
			INSERT INTO `FileSystem` 
				( `Id`, `SiteId`, `Filename`, `FilenameExtension`, `DirectoryName`, 
					`Created`, `CreatedBy`, `Modified`, `ModifiedBy`, `LastAccessed`, `LastAccessedBy`, 
						`is_directory`, `is_readonly`, `is_archive`, `is_system`,
							`ContentStream` )
			VALUES 
			(
				v_RootFolderId, p_Id, '/', null, null, 
					p_Created, p_CreatedBy, p_Modified, p_ModifiedBy, p_Modified, p_ModifiedBy, 
						1, 0, 0, 0, null
			);
            
            INSERT INTO `SitesChangeLog` 
				( `Id`, `SiteId`, `IsLocked`, `IsProcessed`, `ChangeType`, `ChangeTimeStamp`, `ChangedBy`, 
					`PrevName`, `PrevDescription`, `PrevDBServer`, `PrevDBName`, `PrevQuotaBytes`, `PrevModified`, `PrevModifiedBy` ) 
				VALUES 
                (
					UUID(), p_Id, 0, 0, 'I', p_Created, p_CreatedBy, 
						NULL, NULL, NULL, NULL, NULL, NULL, NULL  
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
									0, 0, NULL, NULL, 0, 'site_created', 
										v_RightNow, p_Modified, v_RightNow, p_Modified
					FROM `WorkflowAssociations` 
					WHERE (
						(`is_farm_scope` = 1) 
						AND (`start_on_create` = 1)
						AND (
								(`subscribe_event_site_created` = 1) 
								OR ((`subscribe_event_site_created` = 0) AND (`subscribe_event_catch_bubbledevents` = 1))
						) 
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

			commit;
				
			/*

			SET @CreateSiteDBSql = CONCAT('CREATE DATABASE `', p_ContentDBName, '` CHARACTER SET latin1 COLLATE latin1_general_ci;');
			PREPARE DSQL1 FROM @CreateSiteDBSql;
			EXECUTE DSQL1;
			DEALLOCATE PREPARE DSQL1;

			SET @CreateSiteDBSql = CONCAT('CREATE TABLE `', p_ContentDBName, '`.`FileSystem` LIKE `Corkscrew_ConfigDB`.`FileSystem`; ');
			PREPARE DSQL2 FROM @CreateSiteDBSql;
			EXECUTE DSQL2;
			DEALLOCATE PREPARE DSQL2;
			
			SET @CreateSiteDBSql = CONCAT('CREATE TABLE `', p_ContentDBName, '`.`FileSystemChangeLog` LIKE `Corkscrew_ConfigDB`.`FileSystemChangeLog`; ');
			PREPARE DSQL3 FROM @CreateSiteDBSql;
			EXECUTE DSQL3;
			DEALLOCATE PREPARE DSQL3;

			SET @CreateSiteDBSql = CONCAT(
				'INSERT INTO `', p_ContentDBName, '`.`FileSystem` ',
				'( `Id`, `SiteId`, `Filename`, `FilenameExtension`, `DirectoryName`, `Created`, `CreatedBy`, `Modified`, `ModifiedBy`, `LastAccessed`, `LastAccessedBy`, `is_directory`, `is_readonly`, `is_archive`, `is_system`, `ContentStream` ) ', 
				' SELECT `Id`, `SiteId`, `Filename`, `FilenameExtension`, `DirectoryName`, `Created`, `CreatedBy`, `Modified`, `ModifiedBy`, `LastAccessed`, `LastAccessedBy`, `is_directory`, `is_readonly`, `is_archive`, `is_system`, `ContentStream` ', 
				' FROM `Corkscrew_ConfigDB`.`FileSystem` WHERE (`Id`=''', v_RootFolderId, ''');'
			);
			PREPARE DSQL4 FROM @CreateSiteDBSql;
			EXECUTE DSQL4;
			DEALLOCATE PREPARE DSQL4;

				TODO: How do you copy Stored Procedures from ConfigDB -> new site db ?? This was a good solution, but doesn't work ---
				Error: Cannot combine write-locking of system tables with other tables and lock types!

					SELECT @@SESSION.sql_mode session INTO @sql_mode;
					SET sql_mode = '';

					CREATE TABLE fssproc LIKE mysql.proc;

					INSERT INTO fssproc (db, name, type, specific_name, language, sql_data_access, is_deterministic, security_type, param_list, returns, body, definer, created, modified, sql_mode, comment, 
						character_set_client, collation_connection, db_collation, body_utf8) 
						SELECT  
							p_ContentDBName, name, type, specific_name, language, sql_data_access, is_deterministic, security_type, param_list, returns, body, definer, created, modified, sql_mode, comment, 
								character_set_client, collation_connection, db_collation, body_utf8 
						FROM mysql.proc WHERE ((db = 'Corkscrew_ConfigDB') and (name like 'FileSystem%'));

					INSERT INTO mysql.proc (db, name, type, specific_name, language, sql_data_access, is_deterministic, security_type, param_list, returns, body, definer, created, modified, sql_mode, comment, 
						character_set_client, collation_connection, db_collation, body_utf8) 
						SELECT  
							p_ContentDBName, name, type, specific_name, language, sql_data_access, is_deterministic, security_type, param_list, returns, body, definer, created, modified, sql_mode, comment, 
								character_set_client, collation_connection, db_collation, body_utf8 
						FROM fssproc WHERE ((db = 'Corkscrew_ConfigDB') and (name like 'FileSystem%'));

					DROP TABLE fssproc;

					SET sql_mode = @sql_mode;

			*/

		

    else 

		start transaction;

		INSERT INTO `SitesChangeLog` 
			( `Id`, `SiteId`, `IsLocked`, `IsProcessed`, `ChangeType`, `ChangeTimeStamp`, `ChangedBy`, 
				`PrevName`, `PrevDescription`, `PrevDBServer`, `PrevDBName`, `PrevQuotaBytes`, `PrevModified`, `PrevModifiedBy` ) 
			SELECT 
				UUID(), p_Id, 0, 0, 'U', p_Modified, p_ModifiedBy, 
					`Name`, `Description`, `ContentDBServerName`, `ContentDBName`, `QuotaBytes`, `Modified`, `ModifiedBy` 
			FROM `Sites` 
            where (`Id` = p_Id);
            
		update `Sites` 
			set 
				`Name` = p_Name, 
                `Description` = p_Description, 
                `QuotaBytes` = p_QuotaBytes,
                `ContentDBServerName` = p_ContentDBServerName,
                `ContentDBName` = p_ContentDBName, 
                `Modified` = p_Modified,
                `ModifiedBy` = p_ModifiedBy 
		where (`Id` = p_Id);
        
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
					@WorkflowInstanceId, `Id`, `WorkflowDefinitionId`, 
						`AssociationData`, 
							`FarmId`, @Id, NULL, NULL, 
								0, 0, NULL, NULL, 0, 'site_created', 
									@Modified, @ModifiedBy, @Modified, @ModifiedBy
				FROM `WorkflowAssociations` 
				WHERE (
					(`is_farm_scope` = 1) 
					AND (`start_on_create` = 1)
					AND (
							(`subscribe_event_site_modified` = 1) 
							OR ((`subscribe_event_site_modified` = 0) AND (`subscribe_event_catch_bubbledevents` = 1))
					) 
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
					WHERE (`Id` = @WorkflowInstanceId);
				END IF;
			END IF;
        end if;

		commit;

    end if;
	            
	
        
END ;;
DELIMITER ;