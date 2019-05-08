DELIMITER ;;
CREATE PROCEDURE `WrkflwAssnDeleteByDefinitionId`(
	IN p_WorkflowDefinitionId		varchar(64)
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
	where (`WorkflowDefinitionId` = p_WorkflowDefinitionId);
    
    delete from `WorkflowAssociations` where (`WorkflowDefinitionId` = p_WorkflowDefinitionId);
    
    commit;
        
END ;;
DELIMITER ;