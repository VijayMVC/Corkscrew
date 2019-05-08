CREATE PROCEDURE [WrkflwDefnGetAll]
AS
BEGIN
	SELECT 
		[Id], [Name], [Description], 
				[DefaultAssociationData], 
					[allow_manual_start], [start_on_create], [start_on_modify], [is_enabled],  
						[allow_event_farm_created], [allow_event_farm_modified], [allow_event_farm_deleted],
							[allow_event_site_created], [allow_event_site_modified], [allow_event_site_deleted], 
								[allow_event_directory_created], [allow_event_directory_modified], [allow_event_directory_deleted], 
									[allow_event_file_created], [allow_event_file_modified], [allow_event_file_deleted], 				
										[allow_event_catch_bubbledevents], 
											[Created], [CreatedBy], [Modified], [ModifiedBy]
	FROM [WorkflowDefinitions] WITH (NOLOCK) 

	RETURN @@ROWCOUNT;
END