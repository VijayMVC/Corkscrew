CREATE PROCEDURE [WrkflwAssnGetEvents]
	@WorkflowAssociationId		uniqueidentifier
AS
BEGIN

	SELECT 
		[subscribe_event_farm_created], [subscribe_event_farm_modified], [subscribe_event_farm_deleted],
			[subscribe_event_site_created], [subscribe_event_site_modified], [subscribe_event_site_deleted], 
				[subscribe_event_directory_created], [subscribe_event_directory_modified], [subscribe_event_directory_deleted], 
					[subscribe_event_file_created], [subscribe_event_file_modified], [subscribe_event_file_deleted], 				
						[subscribe_event_catch_bubbledevents]
	FROM [WorkflowAssociations] WITH (NOLOCK) 
	WHERE (
		[Id] = @WorkflowAssociationId
	)

	RETURN @@ROWCOUNT;

END