CREATE PROCEDURE [WrkflwManifestSaveManifestItem]
	@ManifestItemId						uniqueidentifier,
	@Filename							nvarchar(255),
	@FilenameExtension					nvarchar(255),
	@ItemType							int,
	@BuildRelativeFolder				nvarchar(1024) = NULL,
	@RuntimeFolder						nvarchar(1024) = NULL,
	@RequiredForExecution				bit,
	@ContentStream						varbinary(max), 
	@ModifiedBy							uniqueidentifier,
	@Modified							datetime
AS
BEGIN

	IF (NOT EXISTS (SELECT 1 FROM [WorkflowManifestItems] WITH (NOLOCK) WHERE ([Id] = @ManifestItemId)))
	BEGIN
		RAISERROR ( N'Workflow manifest item with that Id does not exist.', 16, 1);
		RETURN -1;
	END

	IF (NOT EXISTS (SELECT 1 FROM [WorkflowDefinitions] WD WITH (NOLOCK) INNER JOIN [WorkflowManifestItems] WMI WITH (NOLOCK) ON (WMI.[WorkflowDefinitionId] = WD.[Id]) WHERE ((WMI.[Id] = @ManifestItemId) AND (WD.[is_enabled] = 1))))
	BEGIN
		RAISERROR ( N'Workflow definition does not exist or was either disabled or deleted.', 16, 1);
		RETURN -1;
	END

	IF (NOT EXISTS (SELECT 1 FROM [WorkflowManifests] WM WITH (NOLOCK) INNER JOIN [WorkflowManifestItems] WMI WITH (NOLOCK) ON (WMI.[WorkflowManifestId] = WM.[Id]) WHERE (WMI.[Id] = @ManifestItemId)))
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

	UPDATE [WorkflowManifestItems] 
	SET 
		[Filename] = @Filename, 
		[FilenameExtension] = @FilenameExtension, 
		[ItemType] = @ItemType, 
		[build_relative_folder] = @BuildRelativeFolder, 
		[runtime_folder] = @RuntimeFolder, 
		[required_for_execution] = @RequiredForExecution, 
		[ContentStream] = @ContentStream, 
		[Modified] = @Modified, 
		[ModifiedBy] = @ModifiedBy 
	WHERE ([Id] = @ManifestItemId)

	RETURN 0

END
