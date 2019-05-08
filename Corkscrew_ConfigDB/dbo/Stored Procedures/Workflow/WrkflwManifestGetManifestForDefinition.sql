CREATE PROCEDURE [WrkflwManifestGetManifestForDefinition]
	@WorkflowDefinitionId			uniqueidentifier
AS
BEGIN

	SELECT 
		[Id], [WorkflowDefinitionId], [WorkflowEngine], 
			[OutputAssemblyName], [WorkflowClassName], 
				[build_assembly_title], [build_assembly_description], [build_assembly_company], [build_assembly_product], [build_assembly_copyright], [build_assembly_trademark], [build_assembly_version], [build_assembly_fileversion], 
					[always_compile], [cache_compile_results], [last_compiled_datetime], 
						[Created], [CreatedBy], [Modified], [ModifiedBy] 
	FROM [WorkflowManifests] WITH (NOLOCK) 
	WHERE (
		[WorkflowDefinitionId] = @WorkflowDefinitionId
	)

	RETURN @@ROWCOUNT;

END