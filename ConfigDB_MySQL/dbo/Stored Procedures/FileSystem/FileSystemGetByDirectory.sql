DELIMITER ;;
CREATE PROCEDURE `FileSystemGetByDirectory`(
	IN p_SiteId				varchar(64), 
    IN p_DirectoryName		longtext,
    IN p_UserId				varchar(64),
    IN p_ReturnFolders		bit,
    IN p_ReturnFiles		bit
)
BEGIN
	
    update `FileSystem` 
		set 
			`LastAccessed` = NOW(),
            `LastAccessedBy` = p_UserId 
	where ((`DirectoryName` = p_DirectoryName) and (`SiteId` = p_SiteId)) ;
    
    select `Id`, `SiteId`, Filename, FilenameExtension, DirectoryName, FullPath, ContentSize, 
		Created, CreatedBy, Modified, ModifiedBy, LastAccessed, LastAccessedBy, 
			is_directory, is_readonly, is_archive, is_system 
	from FileSystem 
    where (
		(DirectoryName = p_DirectoryName) 
        and (SiteId = p_SiteId) 
        and (
			((p_ReturnFiles = 1) and (is_directory = 0)) 
            or ((p_ReturnFolders = 1) and (is_directory = 1))
        )
    );
    
END ;;
DELIMITER ;