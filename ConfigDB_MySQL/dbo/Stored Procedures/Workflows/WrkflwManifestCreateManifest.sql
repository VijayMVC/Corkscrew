DELIMITER ;;
CREATE PROCEDURE `WrkflwManifestCreateManifest`(
	IN p_ManifestId						varchar(64),
	IN p_WorkflowDefinitionId			varchar(64),
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

	IN p_CreatedBy						varchar(64),
	IN p_Created						datetime
)
BEGIN

	declare	v_RightNow	datetime;

	declare exit handler for sqlexception
    begin
		rollback;
	end;
    
    if ((select count(*) from `WorkflowDefinitions` where (`Id` = p_WorkflowDefinitionId)) = 0) then
		call raise_error;
    end if;
    
    if ((select count(*) from `WorkflowManifests` where (`Id` = p_WorkflowManifestId)) > 0) then
		call raise_error;
    end if;
    
    if ((nvl(p_WorkflowEngineName, '') = '') or (nvl(p_OutputAssemblyName, '') = '') or (nvl(p_WorkflowClassName, '') = '')) then 
		call raise_error;
    end if;
    
    INSERT INTO `WorkflowManifests` 
		( 
			`Id`, `WorkflowDefinitionId`, `WorkflowEngine`, 
				`OutputAssemblyName`, `WorkflowClassName`, 
					`build_assembly_title`, `build_assembly_description`, `build_assembly_company`, `build_assembly_product`, `build_assembly_copyright`, `build_assembly_trademark`, `build_assembly_version`, `build_assembly_fileversion`, 
						`always_compile`, `cache_compile_results`, `last_compiled_datetime`, 
							`Created`, `CreatedBy`, `Modified`, `ModifiedBy` 
		) 
		VALUES
		(
			p_ManifestId, p_WorkflowDefinitionId, p_WorkflowEngineName, 
				p_OutputAssemblyName, p_WorkflowClassName, 
					p_buildAssemblyTitle, p_buildAssemblyDescription, p_buildAssemblyCompany, p_buildAssemblyProduct, p_buildAssemblyCopyright, p_buildAssemblyTrademark, p_buildAssemblyVersion, p_buildAssemblyFileVersion, 
						p_always_compile, p_cache_compile_results, NULL, 
							p_Created, p_CreatedBy, p_Created, p_CreatedBy
		);
        
END ;;
DELIMITER ;