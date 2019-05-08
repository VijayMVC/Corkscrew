DELIMITER ;;
CREATE PROCEDURE `WrkflwManifestDeleteManifestItem`(
	IN p_ManifestItemId						varchar(64)
)
BEGIN

	declare exit handler for sqlexception
    begin
		rollback;
	end;
    
    start transaction;
    
		update `WorkflowInstances` as WI
			inner join `WorkflowManifestItems` WMI on (WM.`WorkflowDefinitionId` = WI.`WorkflowDefinitionId`)
		set 
			WI.`CurrentState` = 2,
            WI.`CompletedReason` = 5,
            WI.`ErrorMessage` = 'Workflow manifest was deleted.',
            WI.`Modified` = now(),
            WI.`ModifiedBy` = `SystemUserGuid`() 
		where (
			(WMI.`Id` = p_ManifestItemId) 
            and (WMI.`ItemType` in (1,2,3,4,5))
		);
        
    
        update `WorkflowDefinitions` as WD 
			inner join `WorkflowManifests` WM on (WM.`WorkflowDefinitionId` = WD.`Id`) 
		set 
			WD.`Modified` = now(),
            WD.`ModifiedBy` = `SystemUserGuid`() 
		where (WM.`Id` = p_WorkflowManifestId);
        
        delete from `WorkflowManifestItems` where (`Id` = p_ManifestItemId);
    
    commit;
        
END ;;
DELIMITER ;