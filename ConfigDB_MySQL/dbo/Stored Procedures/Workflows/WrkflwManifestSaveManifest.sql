DELIMITER ;;
CREATE PROCEDURE `WrkflwManifestSaveManifest`(
	IN p_ManifestId						varchar(64),
	IN p_WorkflowEngineName				char(4),
	IN p_OutputAssemblyName				nvarchar(255),
	IN p_WorkflowClassName				nvarchar(1024),

	IN p_buildAssemblyTitle				varchar(255),
	IN p_buildAssemblyDescription		varchar(255),
	IN p_buildAssemblyCompany			varchar(255),
	IN p_buildAssemblyProduct			varchar(255),
	IN p_buildAssemblyCopyright			varchar(255),
	IN p_buildAssemblyTrademark			varchar(255),
	IN p_buildAssemblyVersion			varchar(16),
	IN p_buildAssemblyFileVersion		varchar(16),

	IN p_always_compile					bit,
	IN p_cache_compile_results			bit,

	IN p_ModifiedBy						varchar(64),
	IN p_Modified						datetime
)
BEGIN

	declare exit handler for sqlexception
    begin
		rollback;
	end;
    
    if ((select count(*) from `WorkflowDefinitions` where (`Id` = p_WorkflowDefinitionId)) = 0) then
		call raise_error;
    end if;
    
    if ((select count(*) from `WorkflowManifests` where (`Id` = p_WorkflowManifestId)) = 0) then
		call raise_error;
    end if;
    
    if ((nvl(p_WorkflowEngineName, '') = '') or (nvl(p_OutputAssemblyName, '') = '') or (nvl(p_WorkflowClassName, '') = '')) then 
		call raise_error;
    end if;
    
    UPDATE `WorkflowManifests` 
		SET
			`WorkflowEngine` = p_WorkflowEngineName, 
			`OutputAssemblyName` = p_OutputAssemblyName, 
			`WorkflowClassName` = p_WorkflowClassName, 
			`always_compile` = p_always_compile,
			`cache_compile_results` = p_cache_compile_results,
			`build_assembly_title` = p_buildAssemblyTitle, 
			`build_assembly_description` = p_buildAssemblyDescription, 
			`build_assembly_company` = p_buildAssemblyCompany, 
			`build_assembly_product` = p_buildAssemblyProduct, 
			`build_assembly_copyright` = p_buildAssemblyCopyright, 
			`build_assembly_trademark` = p_buildAssemblyTrademark, 
			`build_assembly_version` = p_buildAssemblyVersion, 
			`build_assembly_fileversion` = p_buildAssemblyFileVersion, 
			`last_compiled_datetime` = 
										CASE 
											WHEN (p_is_compilation_result = 1) THEN p_RightNow 
											ELSE `last_compiled_datetime` 
										END,
			`Modified` = p_Modified, 
			`ModifiedBy` = p_ModifiedBy
	WHERE (`Id` = p_ManifestId);
        
END ;;
DELIMITER ;