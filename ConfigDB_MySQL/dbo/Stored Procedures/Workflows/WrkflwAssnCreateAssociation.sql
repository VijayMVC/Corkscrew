DELIMITER ;;
CREATE PROCEDURE `WrkflwAssnCreateAssociation`(
	IN p_Id							varchar(64),
	IN p_WorkflowDefinitionId		varchar(64),
	IN p_Name						varchar(255),
	IN p_AssociationData			longtext,
	IN p_FarmId						varchar(64),
	IN p_SiteId						varchar(64),
	IN p_DirectoryId				varchar(64),
	IN p_allow_manual_start			bit,
	IN p_start_on_create			bit,
	IN p_start_on_modify			bit,
	IN p_is_enabled					bit,
	IN p_CreatedBy					varchar(64),
	IN p_Created					datetime
)
BEGIN

	declare v_EmptyGuid varchar(64);

	declare exit handler for sqlexception
    begin
		rollback;
	end;
    
    select `GuidDefault`() into v_EmptyGuid;
    
    set p_FarmId = if((p_FarmId = v_EmptyGuid), null, p_FarmId);
    set p_SiteId = if((p_SiteId = v_EmptyGuid), null, p_SiteId);
    set p_DirectoryId = if((p_DirectoryId = v_EmptyGuid), null, p_DirectoryId);
    
    if ((select count(*) from `WorkflowAssociations` where ((`WorkflowDefinitionId` = p_WorkflowDefinitionId) and (`SiteId` = p_SiteId) and (`DirectoryId` = p_DirectoryId) and (`FarmId` = p_FarmId))) > 0) then 
		call raise_error;
    end if;

	INSERT INTO `WorkflowAssociations` 
		( 
			`Id`, `WorkflowDefinitionId`, `Name`, 
				`AssociationData`,  
					`FarmId`, `SiteId`, `DirectoryId`, 
						`allow_manual_start`, `start_on_create`, `start_on_modify`, `is_enabled`, `prevent_new_instances`, 
							`subscribe_event_farm_created`, `subscribe_event_farm_modified`, `subscribe_event_farm_deleted`,
								`subscribe_event_site_created`, `subscribe_event_site_modified`, `subscribe_event_site_deleted`, 
									`subscribe_event_directory_created`, `subscribe_event_directory_modified`, `subscribe_event_directory_deleted`, 
										`subscribe_event_file_created`, `subscribe_event_file_modified`, `subscribe_event_file_deleted`, 				
											`subscribe_event_catch_bubbledevents`, 
												`Created`, `CreatedBy`, `Modified`, `ModifiedBy` 
		) 
		VALUES 
		(
			p_Id, p_WorkflowDefinitionId, p_Name, 
				p_AssociationData,  
					p_FarmId, p_SiteId, p_DirectoryId, 
							p_allow_manual_start, p_start_on_create, p_start_on_modify, p_is_enabled, 0,  
								0, 0, 0, 
									0, 0, 0,
										0, 0, 0,
											0, 0, 0,
												0,
													p_Created, p_CreatedBy, p_Created, p_CreatedBy
		);
        
END ;;
DELIMITER ;