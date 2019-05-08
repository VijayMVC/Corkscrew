CREATE PROCEDURE [WrkflwDefnAllowEvents]
	@WorkflowDefinitionId					uniqueidentifier,
	
	@allow_event_farm_created				bit = 0,
	@allow_event_farm_modified				bit = 0,
	@allow_event_farm_deleted				bit = 0,
	
	@allow_event_site_created				bit = 0,
	@allow_event_site_modified				bit = 0,
	@allow_event_site_deleted				bit = 0,
	
	@allow_event_directory_created			bit = 0,
	@allow_event_directory_modified			bit = 0,
	@allow_event_directory_deleted			bit = 0,
	
	@allow_event_file_created				bit = 0,
	@allow_event_file_modified				bit = 0,
	@allow_event_file_deleted				bit = 0,
	
	@allow_event_catch_bubbledevents		bit = 0,

	@ModifiedBy								uniqueidentifier,
	@Modified								datetime
AS
BEGIN

	DECLARE @RightNow		datetime			= GETDATE()
		,	@SystemUserId	uniqueidentifier	= dbo.SYSTEMUSERGUID()

	IF (NOT EXISTS (SELECT 1 FROM [WorkflowDefinitions] WITH (NOLOCK) WHERE ([Id] = @WorkflowDefinitionId)))
	BEGIN
		RAISERROR ( N'A workflow of that Id does not exist or is deleted.', 16, 1);
		RETURN -1;
	END

	BEGIN TRANSACTION

	BEGIN TRY

		--- if any events have been disallowed, disable the instance
		UPDATE WI 
		SET 
			[CurrentState] = 2,		--- completed
			[CompletedReason] = 5,	--- aborted
			[ErrorMessage] = N'Workflow event has been disabled at the definition.',
			[Modified] = @RightNow, 
			[ModifiedBy] = @SystemUserId 
		FROM [WorkflowInstances] WI 
		INNER JOIN [WorkflowDefinitions] WD 
			ON (WD.[Id] = WI.[WorkflowDefinitionId]) 
		WHERE (
			
			(WD.[Id] = @WorkflowDefinitionId)
			AND (WD.[is_enabled] = 1) 
			AND (WI.[CurrentState] IN (0, 1, 3, 4)) 
			AND (
			
				   ((WI.[InstanceStartEvent] = 'farm_created') AND (@allow_event_farm_created = 0))
				OR ((WI.[InstanceStartEvent] = 'farm_modified') AND (@allow_event_farm_modified = 0))
				OR ((WI.[InstanceStartEvent] = 'farm_deleted') AND (@allow_event_farm_deleted = 0)) 

				OR ((WI.[InstanceStartEvent] = 'site_created') AND (@allow_event_site_created = 0))
				OR ((WI.[InstanceStartEvent] = 'site_modified') AND (@allow_event_site_modified = 0))
				OR ((WI.[InstanceStartEvent] = 'site_deleted') AND (@allow_event_site_deleted = 0)) 

				OR ((WI.[InstanceStartEvent] = 'directory_created') AND (@allow_event_directory_created = 0))
				OR ((WI.[InstanceStartEvent] = 'directory_modified') AND (@allow_event_directory_modified = 0))
				OR ((WI.[InstanceStartEvent] = 'directory_deleted') AND (@allow_event_directory_deleted = 0)) 

				OR ((WI.[InstanceStartEvent] = 'file_created') AND (@allow_event_file_created = 0))
				OR ((WI.[InstanceStartEvent] = 'file_modified') AND (@allow_event_file_modified = 0))
				OR ((WI.[InstanceStartEvent] = 'file_deleted') AND (@allow_event_file_deleted = 0)) 

			)
		)

		--- Update the definition
		UPDATE [WorkflowDefinitions] 
		SET 

			[allow_event_farm_created]				= @allow_event_farm_created,
			[allow_event_farm_modified]				= @allow_event_farm_modified,
			[allow_event_farm_deleted]				= @allow_event_farm_deleted,
		
			[allow_event_site_created]				= @allow_event_site_created,
			[allow_event_site_modified]				= @allow_event_site_modified,
			[allow_event_site_deleted]				= @allow_event_site_deleted,
		
			[allow_event_directory_created]			= @allow_event_directory_created,
			[allow_event_directory_modified]		= @allow_event_directory_modified,
			[allow_event_directory_deleted]			= @allow_event_directory_deleted,
		
			[allow_event_file_created]				= @allow_event_file_created,
			[allow_event_file_modified]				= @allow_event_file_modified,
			[allow_event_file_deleted]				= @allow_event_file_deleted,		
		
			[allow_event_catch_bubbledevents]		= @allow_event_catch_bubbledevents,

			[Modified]								= @Modified, 
			[ModifiedBy]							= @ModifiedBy

		WHERE ([Id] = @WorkflowDefinitionId)

		COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION;
		RETURN -1;
	END CATCH

	RETURN 0;

END