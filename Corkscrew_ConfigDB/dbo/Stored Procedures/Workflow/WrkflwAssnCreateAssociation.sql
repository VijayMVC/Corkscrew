CREATE PROCEDURE [WrkflwAssnCreateAssociation]
	@Id							uniqueidentifier,
	@WorkflowDefinitionId		uniqueidentifier,
	@Name						nvarchar(255),
	@AssociationData			nvarchar(max)		= NULL,
	@FarmId						uniqueidentifier,
	@SiteId						uniqueidentifier	= NULL,
	@DirectoryId				uniqueidentifier	= NULL,
	@allow_manual_start			bit,
	@start_on_create			bit,
	@start_on_modify			bit,
	@is_enabled					bit,
	@CreatedBy					uniqueidentifier,
	@Created					datetime
AS
BEGIN

	DECLARE	@EmptyGuid	uniqueidentifier	= dbo.GUIDDEFAULT()

	IF (EXISTS (SELECT 1 FROM [WorkflowAssociations] WITH (NOLOCK) WHERE ([Id] = @Id)))
	BEGIN
		RAISERROR ( N'Workflow with that Id already exists.', 16, 1);
		RETURN -1;
	END	

	--- ensure one workflow is associated only ONCE per item
	IF (EXISTS (SELECT 1 FROM [WorkflowAssociations] WITH (NOLOCK) WHERE 
		(
			([WorkflowDefinitionId] = @WorkflowDefinitionId) 
			AND (ISNULL([SiteId], @EmptyGuid) = ISNULL(@SiteId, @EmptyGuid)) 
			AND (ISNULL([AssociatedContainerId], @EmptyGuid) = ISNULL(@DirectoryId, @EmptyGuid)) 
			AND (ISNULL([FarmId], @EmptyGuid) = ISNULL(@FarmId, @EmptyGuid)) 
		)
	)) 
	BEGIN
		RAISERROR ( N'Workflow is already associated with target.', 16, 1);
		RETURN -1;
	END

	INSERT INTO [WorkflowAssociations] 
		( 
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
		) 
		VALUES 
		(
			@Id, @WorkflowDefinitionId, @Name, 
				@AssociationData,  
					@FarmId, @SiteId, @DirectoryId, 
							@allow_manual_start, @start_on_create, @start_on_modify, @is_enabled, 0,  
								0, 0, 0, 
									0, 0, 0,
										0, 0, 0,
											0, 0, 0,
												0,
													@Created, @CreatedBy, @Created, @CreatedBy
		)

	RETURN 0;

END
