CREATE PROCEDURE [WrkflwDefnCreateWorkflow]
	@Id									uniqueidentifier				,
	@Name								nvarchar(255)					,
	@Description						nvarchar(1024)					= NULL,

	@DefaultAssociationData				nvarchar(max)					= NULL,

	@allow_manual_start					bit								,
	@start_on_create					bit								,
	@start_on_modify					bit								,
	@is_enabled							bit								,

	@CreatedBy							uniqueidentifier,
	@Created							datetime
AS
BEGIN

	IF (EXISTS (SELECT 1 FROM [WorkflowDefinitions] WITH (NOLOCK) WHERE (([Id] = @Id) OR ([Name] = @Name))))
	BEGIN
		RAISERROR ( N'A workflow of that name or Id already exists.', 16, 1);
		RETURN -1;
	END

	IF (ISNULL(@Description, '') = '') 
	BEGIN
		SET @Description = NULL 
	END

	IF (ISNULL(@DefaultAssociationData, '') = '')
	BEGIN
		SET @DefaultAssociationData = NULL 
	END

	INSERT INTO [WorkflowDefinitions] 
		( 
			[Id], [Name], [Description], 
				[DefaultAssociationData], 
					[allow_manual_start], [start_on_create], [start_on_modify], [is_enabled],  
						[allow_event_farm_created], [allow_event_farm_modified], [allow_event_farm_deleted],
							[allow_event_site_created], [allow_event_site_modified], [allow_event_site_deleted], 
								[allow_event_directory_created], [allow_event_directory_modified], [allow_event_directory_deleted], 
									[allow_event_file_created], [allow_event_file_modified], [allow_event_file_deleted], 				
										[allow_event_catch_bubbledevents], 
											[Created], [CreatedBy], [Modified], [ModifiedBy]
		)
		VALUES 
		(
			@Id, @Name, @Description, 
				@DefaultAssociationData, 
					@allow_manual_start, @start_on_create, @start_on_modify, @is_enabled, 
						0, 0, 0, 
							0, 0, 0, 
								0, 0, 0, 
									0, 0, 0, 
										0,
											@Created, @CreatedBy, @Created, @CreatedBy
		)

	RETURN 0;
END