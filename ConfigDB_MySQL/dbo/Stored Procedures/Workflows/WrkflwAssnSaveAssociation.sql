DELIMITER ;;
CREATE PROCEDURE `WrkflwAssnSaveAssociation`(
	IN p_Id							varchar(64),
	IN p_Name						varchar(255),
	IN p_AssociationData			longtext,
	IN p_allow_manual_start			bit,
	IN p_start_on_create			bit,
	IN p_start_on_modify			bit,
	IN p_is_enabled					bit,
    IN p_prevent_new_instances		bit,
	IN p_ModifiedBy					varchar(64),
	IN p_Modified					datetime
)
BEGIN

	declare exit handler for sqlexception
    begin
		rollback;
	end;
    
    if ((select count(*) from `WorkflowAssociations` where (`Id` = p_Id)) = 0) then 
		call raise_error;
    end if;

	start transaction;
    
    if (((p_prevent_new_instances & p_is_enabled) = 1) or (p_is_enabled = 0)) then 
		update `WorkflowInstances` 
		set 
			`CurrentState` = 2,
            `CompletedReason` = 5,
            `ErrorMessage` = 'Workflow association was disabled.',
            `Modified` = now(),
            `ModifiedBy` = `SystemUserGuid`() 
		where (`WorkflowAssociationId` = p_Id);
    end if;
    
    update `WorkflowAssociations` 
    set 
		`Name` = p_Name,
        `AssociationData` = p_AssociationData,
        `allow_manual_start` = p_allow_manual_start,
        `start_on_create` = p_start_on_create, 
		`start_on_modify` = p_start_on_modify, 
		`prevent_new_instances` = p_prevent_new_instances, 
		`is_enabled` = p_is_enabled, 
		`Modified` = p_Modified, 
		`ModifiedBy` = p_ModifiedBy 
	where (`Id` = p_Id);
    
    commit;
        
END ;;
DELIMITER ;