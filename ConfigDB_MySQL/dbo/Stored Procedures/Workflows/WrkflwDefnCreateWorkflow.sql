DELIMITER ;;
CREATE PROCEDURE `WrkflwDefnCreateWorkflow`(
	IN p_Id							varchar(64),
	IN p_Name						varchar(255),
    IN p_Description				varchar(1024),
	IN p_DefaultAssociationData		longtext,
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
    
    if ((select count(*) from `WorkflowDefinitions` where (`Id` = p_Id)) > 0) then 
		call raise_error;
    end if;

	INSERT INTO `WorkflowDefinitions` 
		( 
			`Id`, `Name`, `Description`, 
				`DefaultAssociationData`, 
					`allow_manual_start`, `start_on_create`, `start_on_modify`, `is_enabled`,  
						`allow_event_farm_created`, `allow_event_farm_modified`, `allow_event_farm_deleted`,
							`allow_event_site_created`, `allow_event_site_modified`, `allow_event_site_deleted`, 
								`allow_event_directory_created`, `allow_event_directory_modified`, `allow_event_directory_deleted`, 
									`allow_event_file_created`, `allow_event_file_modified`, `allow_event_file_deleted`, 				
										`allow_event_catch_bubbledevents`, 
											`Created`, `CreatedBy`, `Modified`, `ModifiedBy`
		)
		VALUES 
		(
			p_Id, p_Name, p_Description, 
				p_DefaultAssociationData, 
					p_allow_manual_start, p_start_on_create, p_start_on_modify, p_is_enabled, 
						0, 0, 0, 
							0, 0, 0, 
								0, 0, 0, 
									0, 0, 0, 
										0,
											p_Created, p_CreatedBy, p_Created, p_CreatedBy
		);
        
END ;;
DELIMITER ;