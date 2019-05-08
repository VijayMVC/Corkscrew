DELIMITER ;;
CREATE PROCEDURE `MIMETypeGetForExtension`(
	IN	p_FileExtension		varchar(24)
)
BEGIN

	select `FileExtension`, `KnownMimeType` from `MIMETypes` 
		WHERE (`FileExtension` = p_FileExtension);
    
END ;;
DELIMITER ;