DELIMITER ;;
CREATE PROCEDURE `MIMETypesGetAll`(
)
BEGIN

	select `FileExtension`, `KnownMimeType` from `MimeTypes`;
    
END ;;
DELIMITER ;