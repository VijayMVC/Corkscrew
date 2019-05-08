DELIMITER ;;
CREATE PROCEDURE `WrkflwManifestDeleteManifest`(
	IN p_WorkflowManifestId						varchar(64)
)
BEGIN

	declare	v_RightNow	datetime;

	declare exit handler for sqlexception
    begin
		rollback;
	end;
    
    start transaction;
    
		update `WorkflowInstances` as WI
			inner join `WorkflowManifests` WM on (WM.`WorkflowDefinitionId` = WI.`WorkflowDefinitionId`)
		set 
			WI.`CurrentState` = 2,
            WI.`CompletedReason` = 5,
            WI.`ErrorMessage` = 'Workflow manifest was deleted.',
            WI.`Modified` = now(),
            WI.`ModifiedBy` = `SystemUserGuid`() 
		where (WM.`Id` = p_WorkflowManifestId);
    
		update `WorkflowDefinitions` as WD 
			inner join `WorkflowManifests` WM on (WM.`WorkflowDefinitionId` = WD.`Id`) 
		set 
			WD.`Modified` = now(),
            WD.`ModifiedBy` = `SystemUserGuid`() 
		where (WM.`Id` = p_WorkflowManifestId);
        
        delete from `WorkflowManifestItems` where (`WorkflowManifestId` = p_WorkflowManifestId);
        delete from `WorkflowManifests` where (`Id` = p_WorkflowManifestId);
    
    commit;
        
END ;;
DELIMITER ;