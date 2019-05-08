DELIMITER ;;
CREATE PROCEDURE `SiteDeleteById`(
	IN p_SiteId			varchar(64), 
    IN p_DeletedById	varchar(64)
)
BEGIN

    declare	v_rowCount				bigint;
    declare	v_RightNow				datetime;
    declare v_WorkflowInstanceId	varchar(64);
    
    SET v_RightNow = now();
    SET v_WorkflowInstanceId = uuid();

	insert into `SitesChangeLog` 
			( `SiteId`, `IsLocked`, `IsProcessed`, `ChangeType`, `ChangeTimeStamp`, `ChangedBy`, 
				`PrevName`, `PrevDescription`, `PrevDBServer`, `PrevDBName`, `PrevQuotaBytes`, `PrevModified`, `PrevModifiedBy` ) 
			SELECT 
				p_SiteId, 0, 0, 'D', now(), p_DeletedById,
				`Name`, `Description`, `ContentDBServerName`, `ContentDBName`, `QuotaBytes`, `Modified`, `ModifiedBy`
			FROM `Sites` WHERE 
			(
				`Id` = p_SiteId
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
						(`subscribe_event_site_deleted` = 1) 
						OR ((`subscribe_event_site_deleted` = 0) AND (`subscribe_event_catch_bubbledevents` = 1))
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

	delete from `Sites` 
		where (`Id` = p_SiteId);
        
END ;;
DELIMITER ;