DELIMITER ;;
CREATE PROCEDURE `WrkflwInstGetById`(
	IN p_Id		varchar(64)
)
BEGIN

	declare exit handler for sqlexception
    begin
		rollback;
	end;
    
    SELECT 
		`Id`, `WorkflowAssociationId`, `WorkflowDefinitionId`, 
			`AssociationData`, 
				`FarmId`, `SiteId`, `DirectoryId`, `FileId`, 
					`CurrentState`, `CompletedReason`, `ErrorMessage`, `SqlPersistenceId`, `IsLoadedInRuntime`,
						`Created`, `CreatedBy`, `Modified`, `ModifiedBy` 
	FROM `WorkflowInstances` 
	WHERE (`Id` = p_Id);
        
END ;;
DELIMITER ;