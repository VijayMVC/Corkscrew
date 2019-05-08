DELIMITER ;;
CREATE PROCEDURE `WrkflwDefnDeleteById`(
	IN p_Id							varchar(64)
)
BEGIN

	declare exit handler for sqlexception
    begin
		rollback;
	end;
    
	start transaction;
    
    delete from `WorkflowManifestItems` where (`WorkflowDefinitionId` = p_Id);
    delete from `WorkflowManifests` where (`WorkflowDefinitionId` = p_Id);
    
    update `WorkflowInstances` 
    set 
		`CurrentState` = 2,
        `CompletedReason` = 5,
        `ErrorMessage` = 'Workflow definition was deleted',
        `Modified` = now(),
        `ModifiedBy` = `SystemUserGuid`() 
	where (`WorkflowDefinitionId` = p_Id);
    
    delete from `WorkflowAssociations` where (`WorkflowDefinitionId` = p_Id);
    delete from `WorkflowDefinitions` where (`Id` = p_Id);
    
    commit;
        
END ;;
DELIMITER ;