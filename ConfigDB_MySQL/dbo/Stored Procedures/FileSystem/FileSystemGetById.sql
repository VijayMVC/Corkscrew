DELIMITER ;;
CREATE PROCEDURE `FileSystemGetById`(
	IN p_Id					varchar(64), 
    IN p_UserId				varchar(64)
)
BEGIN
	
    update `FileSystem` 
		set 
			`LastAccessed` = NOW(),
            `LastAccessedBy` = p_UserId 
	where (`Id` = p_Id);
    
    select `Id`, `SiteId`, Filename, FilenameExtension, DirectoryName, FullPath, ContentSize, 
		Created, CreatedBy, Modified, ModifiedBy, LastAccessed, LastAccessedBy, 
			is_directory, is_readonly, is_archive, is_system 
	from FileSystem 
    where (
		(Id = p_Id) 
    );
    
END ;;
DELIMITER ;