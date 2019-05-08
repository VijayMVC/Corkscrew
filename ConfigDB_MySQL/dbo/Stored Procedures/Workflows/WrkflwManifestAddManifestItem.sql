DELIMITER ;;
CREATE PROCEDURE `WrkflwManifestAddManifestItem`(
	p_ManifestItemId					varchar(64),
	p_WorkflowDefinitionId				varchar(64),
	p_WorkflowManifestId				varchar(64),
	p_Filename							nvarchar(255),
	p_FilenameExtension					nvarchar(255),
	p_ItemType							int,
	p_BuildRelativeFolder				nvarchar(1024),
	p_RuntimeFolder						nvarchar(1024),
	p_RequiredForExecution				bit, 
	p_ContentStream						longblob,
	p_CreatedBy							varchar(64),
	p_Created							datetime
)
BEGIN

	declare	v_RightNow	datetime;

	declare exit handler for sqlexception
    begin
		rollback;
	end;
    
    if ((select count(*) from `WorkflowManifestItems` where (`Id` = p_ManifestItemId)) > 0) then
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
    
    INSERT INTO `WorkflowManifestItems` 
		(
			`Id`, `WorkflowDefinitionId`, `WorkflowManifestId`, 
				`Filename`, `FilenameExtension`, `ItemType`, 
					`build_relative_folder`, `runtime_folder`, `required_for_execution`, `ContentStream`, 
						`Created`, `CreatedBy`, `Modified`, `ModifiedBy` 
		)
		VALUES 
		(
			p_ManifestItemId, p_WorkflowDefinitionId, p_WorkflowManifestId, 
				p_Filename, p_FilenameExtension, p_ItemType, 
					p_BuildRelativeFolder, p_RuntimeFolder, p_RequiredForExecution, p_ContentStream, 
						p_Created, p_CreatedBy, p_Created, p_CreatedBy
		);
        
END ;;
DELIMITER ;