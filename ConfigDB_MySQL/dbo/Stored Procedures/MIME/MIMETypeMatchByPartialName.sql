DELIMITER ;;
CREATE PROCEDURE `MIMETypeMatchByPartialName`(
	IN	p_PartialMimeType		varchar(255)
)
BEGIN

	select `FileExtension`, `KnownMimeType` from `MIMETypes` 
		WHERE (`KnownMimeType` like concat(p_PartialMimeType, '%'));
    
END ;;
DELIMITER ;