DELIMITER ;;
CREATE PROCEDURE `WrkflwInstGetByAssociation`(
	IN p_WorkflowAssociationId		varchar(64),
    IN p_only_runnable				bit
)
BEGIN

	declare exit handler for sqlexception
    begin
		rollback;
	end;
    
    IF (p_only_runnable = 1) THEN
		SELECT 
			`Id`, `WorkflowAssociationId`, `WorkflowDefinitionId`, 
				`AssociationData`, 
					`FarmId`, `SiteId`, `DirectoryId`, `FileId`, 
						`CurrentState`, `CompletedReason`, `ErrorMessage`, `SqlPersistenceId`, `IsLoadedInRuntime`,
							`Created`, `CreatedBy`, `Modified`, `ModifiedBy` 
		FROM `WorkflowInstances` 
		WHERE (
			(`WorkflowAssociationId` = p_WorkflowAssociationId) 
			AND (`CurrentState` IN (0, 1, 3, 4))
		);
	ELSE 
		SELECT 
			`Id`, `WorkflowAssociationId`, `WorkflowDefinitionId`, 
				`AssociationData`, 
					`FarmId`, `SiteId`, `DirectoryId`, `FileId`, 
						`CurrentState`, `CompletedReason`, `ErrorMessage`, `SqlPersistenceId`, `IsLoadedInRuntime`,
							`Created`, `CreatedBy`, `Modified`, `ModifiedBy` 
		FROM `WorkflowInstances` 
		WHERE (`WorkflowAssociationId` = p_WorkflowAssociationId);
	END IF;
        
END ;;
DELIMITER ;