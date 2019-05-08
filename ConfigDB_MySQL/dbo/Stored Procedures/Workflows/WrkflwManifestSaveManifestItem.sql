DELIMITER ;;
CREATE PROCEDURE `WrkflwManifestSaveManifestItem`(
	p_ManifestItemId					varchar(64),
	p_Filename							nvarchar(255),
	p_FilenameExtension					nvarchar(255),
	p_ItemType							int,
	p_BuildRelativeFolder				nvarchar(1024),
	p_RuntimeFolder						nvarchar(1024),
	p_RequiredForExecution				bit, 
	p_ContentStream						longblob,
	p_ModifiedBy						varchar(64),
	p_Modified							datetime
)
BEGIN

	declare exit handler for sqlexception
    begin
		rollback;
	end;
    
    if ((select count(*) from `WorkflowManifestItems` where (`Id` = p_ManifestItemId)) = 0) then
		call raise_error;
    end if;
    
    if ((select count(*) from `WorkflowDefinitions` where (`Id` = p_WorkflowDefinitionId)) = 0) then
		call raise_error;
    end if;
    
    if ((select count(*) from `WorkflowManifests` where (`Id` = p_WorkflowManifestId)) = 0) then
		call raise_error;
    end if;
    
    if ((nvl(p_Filename, '') = '') and (nvl(p_FilenameExtension, '') = '')) then 
		call raise_error;
    end if;
    
    if ((p_ItemType < 1) or (p_ItemType > 9)) then 
		call raise_error;
	end if;
    
    UPDATE `WorkflowManifestItems` 
	SET 
		`Filename` = p_Filename, 
		`FilenameExtension` = p_FilenameExtension, 
		`ItemType` = p_ItemType, 
		`build_relative_folder` = p_BuildRelativeFolder, 
		`runtime_folder` = p_RuntimeFolder, 
		`required_for_execution` = p_RequiredForExecution, 
		`ContentStream` = p_ContentStream, 
		`Modified` = p_Modified, 
		`ModifiedBy` = p_ModifiedBy 
	WHERE (`Id` = p_ManifestItemId);
        
END ;;
DELIMITER ;