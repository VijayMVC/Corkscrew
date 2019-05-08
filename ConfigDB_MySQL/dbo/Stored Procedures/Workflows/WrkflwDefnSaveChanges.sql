DELIMITER ;;
CREATE PROCEDURE `WrkflwDefnSaveChanges`(
	IN p_Id							varchar(64),
	IN p_Name						varchar(255),
    IN p_Description				varchar(1024),
	IN p_DefaultAssociationData		longtext,
	IN p_allow_manual_start			bit,
	IN p_start_on_create			bit,
	IN p_start_on_modify			bit,
	IN p_is_enabled					bit,
	IN p_ModifiedBy					varchar(64),
	IN p_Modified					datetime
)
BEGIN

	declare exit handler for sqlexception
    begin
		rollback;
	end;
    
    if ((select count(*) from `WorkflowDefinitions` where ((`Id` <> p_Id) and (`Name` = p_Name))) > 0) then 
		if ((select count(*) from `WorkflowDefinitions` where (`Name` = p_Name)) > 0) then 
			call raise_error;
        end if;
    end if;
    
    set p_Description = if((p_Description = ''), null, p_Description);
    set p_DefaultAssociationData = if((p_DefaultAssociationData = ''), null, p_DefaultAssociationData);
    
    start transaction;
    
		update `WorkflowAssociations` as WA 
			inner join `WorkflowDefinitions` WD on (WD.`Id` = WA.`WorkflowDefinitionId`) 
		set 
			WA.`is_enabled` = 0,
            WA.`prevent_new_instances` = 1,
            WA.`Modified` = p_Modified,
            WA.`ModifiedBy` = p_ModifiedBy 
		where ((WD.`is_enabled` = 1) and (p_is_enabled = 0) and (WD.`Id` = p_Id));
        
        update `WorkflowInstances` as WI
			inner join `WorkflowDefinitions` WD on (WD.`Id` = WI.`WorkflowDefinitionId`)
		set 
			WI.`CurrentState` = 2,
            WI.`CompletedReason` = 5,
            WI.`ErrorMessage` = 'Workflow definition was disabled.',
            WI.`Modified` = now(),
            WI.`ModifiedBy` = `SystemUserGuid`() 
		where ((WD.`is_enabled` = 1) and (p_is_enabled = 0) AND (WD.`Id` = p_Id));
        
        update `WorkflowDefinitions` 
        set 
			`Name` = p_Name,
            `Description` = p_Description,
            `DefaultAssociationData` = p_DefaultAssociationData,
            `allow_manual_start` = p_allow_manual_start,
            `start_on_create` = p_start_on_create,
            `start_on_modify` = p_start_on_modify,
            `is_enabled` = p_is_enabled,
            `Modified` = p_Modified,
            `ModifiedBy` = p_ModifiedBy 
		where (`Id` = p_Id);
    
    commit;
        
END ;;
DELIMITER ;