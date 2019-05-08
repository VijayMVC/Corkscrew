CREATE PROCEDURE [WrkflwManifestSaveManifest]
	@ManifestId						uniqueidentifier,
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
	@is_compilation_result			bit,

	@ModifiedBy						uniqueidentifier,
	@Modified						datetime
AS
BEGIN

	DECLARE	@RightNow				datetime			= GETDATE()

	IF (NOT EXISTS (SELECT 1 FROM [WorkflowManifests] WITH (NOLOCK) WHERE ([Id] = @ManifestId)))
	BEGIN
		RAISERROR ( N'Workflow manifest with that Id does not exists.', 16, 1);
		RETURN -1;
	END

	IF (NOT EXISTS (SELECT 1 FROM [WorkflowDefinitions] WD WITH (NOLOCK) INNER JOIN [WorkflowManifests] WM WITH (NOLOCK) ON (WM.[WorkflowDefinitionId] = WD.[Id]) WHERE ((WM.[Id] = @ManifestId) AND (WD.[is_enabled] = 1))))
	BEGIN
		RAISERROR ( N'Workflow definition does not exist or was either disabled or deleted.', 16, 1);
		RETURN -1;
	END


	IF ((ISNULL(@WorkflowEngineName, '') = '') OR (ISNULL(@OutputAssemblyName, '') = '') OR (ISNULL(@WorkflowClassName, '') = '')) 
	BEGIN
		RAISERROR ( N'Workflow engine, output assembly and class names are mandatory.', 16, 1);
		RETURN -1;
	END

	UPDATE [WorkflowManifests] 
		SET
			[WorkflowEngine] = @WorkflowEngineName, 
			[OutputAssemblyName] = @OutputAssemblyName, 
			[WorkflowClassName] = @WorkflowClassName, 
			[always_compile] = @always_compile,
			[cache_compile_results] = @cache_compile_results,
			[build_assembly_title] = @buildAssemblyTitle, 
			[build_assembly_description] = @buildAssemblyDescription, 
			[build_assembly_company] = @buildAssemblyCompany, 
			[build_assembly_product] = @buildAssemblyProduct, 
			[build_assembly_copyright] = @buildAssemblyCopyright, 
			[build_assembly_trademark] = @buildAssemblyTrademark, 
			[build_assembly_version] = @buildAssemblyVersion, 
			[build_assembly_fileversion] = @buildAssemblyFileVersion, 
			[last_compiled_datetime] = 
										CASE 
											WHEN (@is_compilation_result = 1) THEN @RightNow 
											ELSE [last_compiled_datetime] 
										END,
			[Modified] = @Modified, 
			[ModifiedBy] = @ModifiedBy
	WHERE ([Id] = @ManifestId)

	RETURN 0;

END