DELIMITER ;;
CREATE PROCEDURE `WrkflwManifestGetManifestItemForManifest`(
	IN p_WorkflowManifestId						varchar(64)
)
BEGIN

	declare exit handler for sqlexception
    begin
		rollback;
	end;
    
    SELECT 
		`Id`, `WorkflowDefinitionId`, `WorkflowManifestId`, 
			`Filename`, `FilenameExtension`, `ItemType`, 
				`build_relative_folder`, `runtime_folder`, `required_for_execution`, `ContentStream`, 
					`Created`, `CreatedBy`, `Modified`, `ModifiedBy` 
	FROM `WorkflowManifestItems` 
	WHERE (`WorkflowManifestId` = p_WorkflowManifestId);
        
END ;;
DELIMITER ;