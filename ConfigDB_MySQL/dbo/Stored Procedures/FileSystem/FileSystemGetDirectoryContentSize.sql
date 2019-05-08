DELIMITER ;;
CREATE PROCEDURE `FileSystemGetDirectoryContentSize`(
	IN p_SiteId				varchar(64), 
    IN p_DirectoryName		longtext,
    IN p_UserId				varchar(64)
)
BEGIN
	
    update `FileSystem` 
		set 
			`LastAccessed` = NOW(),
            `LastAccessedBy` = p_UserId 
	where ((DirectoryName = p_DirectoryName) and (SiteId = p_SiteId));
    
    select sum(nvl(ContentSize, 0)) 
	from FileSystem 
    where ((DirectoryName like concat(p_DirectoryName, '%')) and (SiteId = p_SiteId) and (is_directory = 0));
    
END ;;
DELIMITER ;