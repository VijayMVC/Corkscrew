DELIMITER ;;
CREATE PROCEDURE `WrkflwDefnGetEvents`(
	IN p_WorkflowDefinitionId		varchar(64)
)
BEGIN

	SELECT 
		`allow_event_farm_created`, `allow_event_farm_modified`, `allow_event_farm_deleted`,
			`allow_event_site_created`, `allow_event_site_modified`, `allow_event_site_deleted`, 
				`allow_event_directory_created`, `allow_event_directory_modified`, `allow_event_directory_deleted`, 
					`allow_event_file_created`, `allow_event_file_modified`, `allow_event_file_deleted`, 				
						`allow_event_catch_bubbledevents`
	FROM `WorkflowDefinitions` 
	WHERE (
		(`Id` = p_WorkflowDefinitionId)
	);
        
END ;;
DELIMITER ;