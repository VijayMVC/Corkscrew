﻿CREATE PROCEDURE [WrkflwAssnGetForDirectory]
	@SiteId			uniqueidentifier,
	@DirectoryId	uniqueidentifier
AS
BEGIN

	DECLARE @EmptyGuid	uniqueidentifier = dbo.GUIDDEFAULT()

	SELECT 
		[Id], [WorkflowDefinitionId], [Name], 
				[AssociationData],  
					[FarmId], [SiteId], [AssociatedContainerId], 
						[allow_manual_start], [start_on_create], [start_on_modify], [is_enabled], [prevent_new_instances], 
							[subscribe_event_farm_created], [subscribe_event_farm_modified], [subscribe_event_farm_deleted],
								[subscribe_event_site_created], [subscribe_event_site_modified], [subscribe_event_site_deleted], 
									[subscribe_event_directory_created], [subscribe_event_directory_modified], [subscribe_event_directory_deleted], 
										[subscribe_event_file_created], [subscribe_event_file_modified], [subscribe_event_file_deleted], 				
											[subscribe_event_catch_bubbledevents], 
												[Created], [CreatedBy], [Modified], [ModifiedBy]
	FROM [WorkflowAssociations] WITH (NOLOCK) 
	WHERE (
		(([SiteId] IS NOT NULL) AND ([SiteId] = @SiteId)) 
		AND ([AssociatedContainerId] = @DirectoryId)
	)
	
	RETURN @@ROWCOUNT;

END