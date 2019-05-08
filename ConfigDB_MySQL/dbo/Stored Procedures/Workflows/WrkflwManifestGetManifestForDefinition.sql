DELIMITER ;;
CREATE PROCEDURE `WrkflwManifestGetManifestForDefinition`(
	IN p_WorkflowDefinitionId						varchar(64)
)
BEGIN

	declare exit handler for sqlexception
    begin
		rollback;
	end;
    
    SELECT 
		`Id`, `WorkflowDefinitionId`, `WorkflowEngine`, 
			`OutputAssemblyName`, `WorkflowClassName`, 
				`build_assembly_title`, `build_assembly_description`, `build_assembly_company`, `build_assembly_product`, `build_assembly_copyright`, `build_assembly_trademark`, `build_assembly_version`, `build_assembly_fileversion`, 
					`always_compile`, `cache_compile_results`, `last_compiled_datetime`, 
						`Created`, `CreatedBy`, `Modified`, `ModifiedBy` 
	FROM `WorkflowManifests` 
	WHERE (
		`WorkflowDefinitionId` = @p_WorkflowDefinitionId
	);
        
END ;;
DELIMITER ;