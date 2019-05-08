DELIMITER ;;
CREATE PROCEDURE `FileSystemGetDataById`(
	IN p_Id					varchar(64), 
    IN p_UserId				varchar(64)
)
BEGIN
	
    update `FileSystem` 
		set 
			`LastAccessed` = NOW(),
            `LastAccessedBy` = p_UserId 
	where (`Id` = p_Id);
    
    select ContentSize, ContentStream
	from FileSystem 
    where (
		(Id = p_Id) 
        and (is_directory = 0)
    );
    
END ;;
DELIMITER ;