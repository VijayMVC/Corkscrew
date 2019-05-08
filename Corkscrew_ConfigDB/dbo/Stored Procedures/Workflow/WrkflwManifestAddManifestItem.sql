CREATE PROCEDURE [WrkflwManifestAddManifestItem]
	@ManifestItemId						uniqueidentifier,
	@WorkflowDefinitionId				uniqueidentifier,
	@WorkflowManifestId					uniqueidentifier,
	@Filename							nvarchar(255),
	@FilenameExtension					nvarchar(255),
	@ItemType							int,
	@BuildRelativeFolder				nvarchar(1024) = NULL,
	@RuntimeFolder						nvarchar(1024) = NULL,
	@RequiredForExecution				bit, 
	@ContentStream						varbinary(max),
	@CreatedBy							uniqueidentifier,
	@Created							datetime
AS
BEGIN

	IF (EXISTS (SELECT 1 FROM [WorkflowManifestItems] WITH (NOLOCK) WHERE ([Id] = @ManifestItemId)))
	BEGIN
		RAISERROR ( N'Workflow manifest item with that Id already exists.', 16, 1);
		RETURN -1;
	END

	IF (NOT EXISTS (SELECT 1 FROM [WorkflowDefinitions] WITH (NOLOCK) WHERE (([Id] = @WorkflowDefinitionId) AND ([is_enabled] = 1))))
	BEGIN
		RAISERROR ( N'Workflow definition does not exist or was either disabled or deleted.', 16, 1);
		RETURN -1;
	END

	IF (NOT EXISTS (SELECT 1 FROM [WorkflowManifests] WITH (NOLOCK) WHERE ([Id] = @WorkflowManifestId)))
	BEGIN
		RAISERROR ( N'Workflow manifest does not exist.', 16, 1);
		RETURN -1;
	END 

	IF ((ISNULL(@Filename, '') = '') AND (ISNULL(@FilenameExtension, '') = '')) 
	BEGIN
		RAISERROR ( N'Either filename or filename extension must be provided.', 16, 1);
		RETURN -1;
	END

	IF ((@ItemType < 1) OR (@ItemType > 9)) 
	BEGIN 
		RAISERROR ( N'ItemType must be between 1 and 9.', 16, 1);
		RETURN -1;
	END 

	IF (@ContentStream IS NULL)
	BEGIN 
		SET @ContentStream = 0x 
	END

	INSERT INTO [WorkflowManifestItems] 
		(
			[Id], [WorkflowDefinitionId], [WorkflowManifestId], 
				[Filename], [FilenameExtension], [ItemType], 
					[build_relative_folder], [runtime_folder], [required_for_execution], [ContentStream], 
						[Created], [CreatedBy], [Modified], [ModifiedBy] 
		)
		VALUES 
		(
			@ManifestItemId, @WorkflowDefinitionId, @WorkflowManifestId, 
				@Filename, @FilenameExtension, @ItemType, 
					@BuildRelativeFolder, @RuntimeFolder, @RequiredForExecution, @ContentStream, 
						@Created, @CreatedBy, @Created, @CreatedBy
		)

	RETURN 0

END
