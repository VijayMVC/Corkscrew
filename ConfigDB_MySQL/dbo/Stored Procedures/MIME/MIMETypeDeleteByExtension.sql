DELIMITER ;;
CREATE PROCEDURE `MIMETypeDeleteByExtension`(
	IN	p_FileExtension		varchar(24)
)
BEGIN

	DELETE FROM `MIMETypes` 
		WHERE (`FileExtension` = p_FileExtension);
    
END ;;
DELIMITER ;