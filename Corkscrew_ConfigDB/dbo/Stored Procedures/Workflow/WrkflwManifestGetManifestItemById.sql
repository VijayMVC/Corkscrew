CREATE PROCEDURE [WrkflwManifestGetManifestItemById]
	@ManifestItemId			uniqueidentifier 
AS
BEGIN 

	SELECT 
		[Id], [WorkflowDefinitionId], [WorkflowManifestId], 
			[Filename], [FilenameExtension], [ItemType], 
				[build_relative_folder], [runtime_folder], [required_for_execution], [ContentStream], 
					[Created], [CreatedBy], [Modified], [ModifiedBy] 
	FROM [WorkflowManifestItems] WITH (NOLOCK) 
	WHERE ([Id] = @ManifestItemId) 

	RETURN @@ROWCOUNT;

END