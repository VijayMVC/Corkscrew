CREATE PROCEDURE [WrkflwAssnSubscribeEvents]
	@WorkflowAssociationId						uniqueidentifier,
	
	@subscribe_event_farm_created				bit = 0,
	@subscribe_event_farm_modified				bit = 0,
	@subscribe_event_farm_deleted				bit = 0,
	
	@subscribe_event_site_created				bit = 0,
	@subscribe_event_site_modified				bit = 0,
	@subscribe_event_site_deleted				bit = 0,
	
	@subscribe_event_directory_created			bit = 0,
	@subscribe_event_directory_modified			bit = 0,
	@subscribe_event_directory_deleted			bit = 0,
	
	@subscribe_event_file_created				bit = 0,
	@subscribe_event_file_modified				bit = 0,
	@subscribe_event_file_deleted				bit = 0,
	
	@subscribe_event_catch_bubbledevents		bit = 0,

	@ModifiedBy									uniqueidentifier,
	@Modified									datetime
AS
BEGIN

	DECLARE @RightNow				datetime			= GETDATE()
		,	@SystemUserId			uniqueidentifier	= dbo.SYSTEMUSERGUID() 

	IF (NOT EXISTS (SELECT 1 FROM [WorkflowAssociations] WITH (NOLOCK) WHERE ([Id] = @WorkflowAssociationId)))
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
			[ErrorMessage] = N'Workflow event has been disabled at the association.',
			[Modified] = @RightNow, 
			[ModifiedBy] = @SystemUserId 
		FROM [WorkflowInstances] WI 
		INNER JOIN [WorkflowAssociations] WA 
			ON (WA.[Id] = WI.[WorkflowAssociationId])
		WHERE (

			(WA.[Id] = @WorkflowAssociationId)
			AND (WI.[CurrentState] IN (0, 1, 3, 4)) 
			AND (
			
				   ((WI.[InstanceStartEvent] = 'farm_created') AND (@subscribe_event_farm_created = 0))
				OR ((WI.[InstanceStartEvent] = 'farm_modified') AND (@subscribe_event_farm_modified = 0))
				OR ((WI.[InstanceStartEvent] = 'farm_deleted') AND (@subscribe_event_farm_deleted = 0)) 

				OR ((WI.[InstanceStartEvent] = 'site_created') AND (@subscribe_event_site_created = 0))
				OR ((WI.[InstanceStartEvent] = 'site_modified') AND (@subscribe_event_site_modified = 0))
				OR ((WI.[InstanceStartEvent] = 'site_deleted') AND (@subscribe_event_site_deleted = 0)) 

				OR ((WI.[InstanceStartEvent] = 'directory_created') AND (@subscribe_event_directory_created = 0))
				OR ((WI.[InstanceStartEvent] = 'directory_modified') AND (@subscribe_event_directory_modified = 0))
				OR ((WI.[InstanceStartEvent] = 'directory_deleted') AND (@subscribe_event_directory_deleted = 0)) 

				OR ((WI.[InstanceStartEvent] = 'file_created') AND (@subscribe_event_file_created = 0))
				OR ((WI.[InstanceStartEvent] = 'file_modified') AND (@subscribe_event_file_modified = 0))
				OR ((WI.[InstanceStartEvent] = 'file_deleted') AND (@subscribe_event_file_deleted = 0)) 

			)
		)

		--- update the association
		UPDATE [WorkflowAssociations] 
		SET 

			[subscribe_event_farm_created]				= @subscribe_event_farm_created,
			[subscribe_event_farm_modified]				= @subscribe_event_farm_modified,
			[subscribe_event_farm_deleted]				= @subscribe_event_farm_deleted,
		
			[subscribe_event_site_created]				= @subscribe_event_site_created,
			[subscribe_event_site_modified]				= @subscribe_event_site_modified,
			[subscribe_event_site_deleted]				= @subscribe_event_site_deleted,
		
			[subscribe_event_directory_created]			= @subscribe_event_directory_created,
			[subscribe_event_directory_modified]		= @subscribe_event_directory_modified,
			[subscribe_event_directory_deleted]			= @subscribe_event_directory_deleted,
		
			[subscribe_event_file_created]				= @subscribe_event_file_created,
			[subscribe_event_file_modified]				= @subscribe_event_file_modified,
			[subscribe_event_file_deleted]				= @subscribe_event_file_deleted,		
		
			[subscribe_event_catch_bubbledevents]		= @subscribe_event_catch_bubbledevents,

			[Modified]									= @Modified, 
			[ModifiedBy]								= @ModifiedBy

		WHERE ([Id] = @WorkflowAssociationId)

		COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION;
		RETURN -1;
	END CATCH

	

	RETURN 0;

END
