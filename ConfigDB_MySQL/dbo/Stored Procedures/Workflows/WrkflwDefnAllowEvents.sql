DELIMITER ;;
CREATE PROCEDURE `WrkflwDefnAllowEvents`(
	p_WorkflowDefinitionId					varchar(64),
	
	p_allow_event_farm_created				bit,
	p_allow_event_farm_modified				bit,
	p_allow_event_farm_deleted				bit,
	
	p_allow_event_site_created				bit,
	p_allow_event_site_modified				bit,
	p_allow_event_site_deleted				bit,
	
	p_allow_event_directory_created			bit,
	p_allow_event_directory_modified		bit,
	p_allow_event_directory_deleted			bit,
	
	p_allow_event_file_created				bit,
	p_allow_event_file_modified				bit,
	p_allow_event_file_deleted				bit,
	
	p_allow_event_catch_bubbledevents		bit,

	p_ModifiedBy							varchar(64),
	p_Modified								datetime
)
BEGIN

	DECLARE v_RightNow				datetime;
	DECLARE v_SystemUserId			varchar(64);

	declare exit handler for sqlexception
    begin
		rollback;
	end;
    
    set v_RightNow = now();
    set v_SystemUserId = `SystemUserGuid`();
    
    start transaction;
    
    UPDATE `WorkflowInstances` as WI 
    INNER JOIN `WorkflowDefinitions` WD ON (WD.`Id` = WI.`WorkflowDefinitionId`)
		SET 
			`CurrentState` = 2,	
			`CompletedReason` = 5,
			`ErrorMessage` = 'Workflow event has been disabled at the association.',
			`Modified` = v_RightNow, 
			`ModifiedBy` = v_SystemUserId 
		WHERE (
			(WD.`Id` = p_WorkflowDefinitionId)
			AND (WD.`CurrentState` IN (0, 1, 3, 4)) 
			AND (
			
				   ((WI.`InstanceStartEvent` = 'farm_created') AND (p_allow_event_farm_created = 0))
				OR ((WI.`InstanceStartEvent` = 'farm_modified') AND (p_allow_event_farm_modified = 0))
				OR ((WI.`InstanceStartEvent` = 'farm_deleted') AND (p_allow_event_farm_deleted = 0)) 

				OR ((WI.`InstanceStartEvent` = 'site_created') AND (p_allow_event_site_created = 0))
				OR ((WI.`InstanceStartEvent` = 'site_modified') AND (p_allow_event_site_modified = 0))
				OR ((WI.`InstanceStartEvent` = 'site_deleted') AND (p_allow_event_site_deleted = 0)) 

				OR ((WI.`InstanceStartEvent` = 'directory_created') AND (p_allow_event_directory_created = 0))
				OR ((WI.`InstanceStartEvent` = 'directory_modified') AND (p_allow_event_directory_modified = 0))
				OR ((WI.`InstanceStartEvent` = 'directory_deleted') AND (p_allow_event_directory_deleted = 0)) 

				OR ((WI.`InstanceStartEvent` = 'file_created') AND (p_allow_event_file_created = 0))
				OR ((WI.`InstanceStartEvent` = 'file_modified') AND (p_allow_event_file_modified = 0))
				OR ((WI.`InstanceStartEvent` = 'file_deleted') AND (p_allow_event_file_deleted = 0)) 

			)
		);
        
	UPDATE `WorkflowDefinitions` 
		SET 

			`allow_event_farm_created`				= p_allow_event_farm_created,
			`allow_event_farm_modified`				= p_allow_event_farm_modified,
			`allow_event_farm_deleted`				= p_allow_event_farm_deleted,
		
			`allow_event_site_created`				= p_allow_event_site_created,
			`allow_event_site_modified`				= p_allow_event_site_modified,
			`allow_event_site_deleted`				= p_allow_event_site_deleted,
		
			`allow_event_directory_created`			= p_allow_event_directory_created,
			`allow_event_directory_modified`		= p_allow_event_directory_modified,
			`allow_event_directory_deleted`			= p_allow_event_directory_deleted,
		
			`allow_event_file_created`				= p_allow_event_file_created,
			`allow_event_file_modified`				= p_allow_event_file_modified,
			`allow_event_file_deleted`				= p_allow_event_file_deleted,		
		
			`allow_event_catch_bubbledevents`		= p_allow_event_catch_bubbledevents,

			`Modified`									= p_Modified, 
			`ModifiedBy`								= p_ModifiedBy

		WHERE (`Id` = p_WorkflowDefinitionId);
    
    commit;
        
END ;;
DELIMITER ;