DELIMITER ;;
CREATE PROCEDURE `WrkflwAssnDeleteByAssociationId`(
	IN p_WorkflowAssociationId		varchar(64)
)
BEGIN

	declare v_EmptyGuid varchar(64);

	declare exit handler for sqlexception
    begin
		rollback;
	end;
    
    start transaction;
    
    update `WorkflowInstances` 
    set 
		`CurrentState` = 2,
        `CompletedReason` = 5,
        `ErrorMessage` = 'Workflow association was deleted',
        `Modified` = now(),
        `ModifiedBy` = `SystemUserGuid`()
	where (`WorkflowAssociationId` = p_WorkflowAssociationId);
    
    delete from `WorkflowAssociations` where (`Id` = p_WorkflowAssociationId);
    
    commit;
        
END ;;
DELIMITER ;