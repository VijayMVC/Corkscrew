CREATE PROCEDURE [WrkflwManifestCreateManifest]
	@ManifestId						uniqueidentifier,
	@WorkflowDefinitionId			uniqueidentifier,
	@WorkflowEngineName				char(4)				= 'CS1C',
	@OutputAssemblyName				nvarchar(255),
	@WorkflowClassName				nvarchar(1024),

	@buildAssemblyTitle				nvarchar(255)		= NULL,
	@buildAssemblyDescription		nvarchar(255)		= NULL,
	@buildAssemblyCompany			nvarchar(255)		= NULL,
	@buildAssemblyProduct			nvarchar(255)		= NULL,
	@buildAssemblyCopyright			nvarchar(255)		= NULL,
	@buildAssemblyTrademark			nvarchar(255)		= NULL,
	@buildAssemblyVersion			nvarchar(16)		= '1.0.0.0',
	@buildAssemblyFileVersion		nvarchar(16)		= '1.0.0.0',

	@always_compile					bit					= 0,
	@cache_compile_results			bit					= 1,

	@CreatedBy						uniqueidentifier,
	@Created						datetime
AS
BEGIN

	IF (EXISTS (SELECT 1 FROM [WorkflowManifests] WITH (NOLOCK) WHERE ([Id] = @ManifestId)))
	BEGIN
		RAISERROR ( N'Workflow manifest with that Id already exists.', 16, 1);
		RETURN -1;
	END

	IF (NOT EXISTS (SELECT 1 FROM [WorkflowDefinitions] WITH (NOLOCK) WHERE (([Id] = @WorkflowDefinitionId) AND ([is_enabled] = 1))))
	BEGIN
		RAISERROR ( N'Workflow definition does not exist or was either disabled or deleted.', 16, 1);
		RETURN -1;
	END

	--- Only one manifest per definition!
	IF (EXISTS (SELECT 1 FROM [WorkflowManifests] WITH (NOLOCK) WHERE ([WorkflowDefinitionId] = @WorkflowDefinitionId)))
	BEGIN
		RAISERROR ( N'Workflow manifest for that workflow definition already exists.', 16, 1);
		RETURN -1;
	END

	IF ((ISNULL(@WorkflowEngineName, '') = '') OR (ISNULL(@OutputAssemblyName, '') = '') OR (ISNULL(@WorkflowClassName, '') = '')) 
	BEGIN
		RAISERROR ( N'Workflow engine, output assembly and class names are mandatory.', 16, 1);
		RETURN -1;
	END

	INSERT INTO [WorkflowManifests] 
		( 
			[Id], [WorkflowDefinitionId], [WorkflowEngine], 
				[OutputAssemblyName], [WorkflowClassName], 
					[build_assembly_title], [build_assembly_description], [build_assembly_company], [build_assembly_product], [build_assembly_copyright], [build_assembly_trademark], [build_assembly_version], [build_assembly_fileversion], 
						[always_compile], [cache_compile_results], [last_compiled_datetime], 
							[Created], [CreatedBy], [Modified], [ModifiedBy] 
		) 
		VALUES
		(
			@ManifestId, @WorkflowDefinitionId, @WorkflowEngineName, 
				@OutputAssemblyName, @WorkflowClassName, 
					@buildAssemblyTitle, @buildAssemblyDescription, @buildAssemblyCompany, @buildAssemblyProduct, @buildAssemblyCopyright, @buildAssemblyTrademark, @buildAssemblyVersion, @buildAssemblyFileVersion, 
						@always_compile, @cache_compile_results, NULL, 
							@Created, @CreatedBy, @Created, @CreatedBy
		)

	RETURN 0;

END