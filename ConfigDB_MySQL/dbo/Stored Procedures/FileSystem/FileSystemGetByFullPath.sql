DELIMITER ;;
CREATE PROCEDURE `FileSystemGetByFullPath`(
	IN p_SiteId				varchar(64), 
    IN p_FullPath			longtext,
    IN p_UserId				varchar(64)
)
BEGIN

	set p_FullPath = LOWER(p_FullPath);
	
    update `FileSystem` 
		set 
			`LastAccessed` = NOW(),
            `LastAccessedBy` = p_UserId 
	where ((`FullPath` = p_FullPath) and (`SiteId` = p_SiteId)) ;
    
    select `Id`, `SiteId`, Filename, FilenameExtension, DirectoryName, FullPath, ContentSize, 
		Created, CreatedBy, Modified, ModifiedBy, LastAccessed, LastAccessedBy, 
			is_directory, is_readonly, is_archive, is_system 
	from FileSystem 
    where (
		((FullPath = p_FullPath) or (concat(FullPath, '/') = p_FullPath))
        and (SiteId = p_SiteId) 
    );
    
END ;;
DELIMITER ;