DELIMITER ;;
CREATE PROCEDURE `WrkflwAssnGetForFarm`(
	IN p_FarmId			varchar(64)
)
BEGIN

	SELECT 
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
	FROM `WorkflowAssociations` 
	WHERE (
		(`FarmId` = p_FarmId)
		and (`SiteId` is null) 
        and (`DirectoryId` is null)
	);
        
END ;;
DELIMITER ;