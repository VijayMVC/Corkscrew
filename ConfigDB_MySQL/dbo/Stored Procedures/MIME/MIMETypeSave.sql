DELIMITER ;;
CREATE PROCEDURE `MIMETypeSave`(
	IN  p_FileExtension	varchar(24),
	IN	p_MimeType		varchar(255)
)
BEGIN

	if ((select count(*) from `MimeTypes` where ((`FileExtension` = p_FileExtension) and (`KnownMimeType` <> p_MimeType))) = 1) then 
		update `MimeTypes` 
			set 
				`KnownMimeType` = p_MimeType 
		where (`FileExtension` = p_FileExtension);
	else 
		insert into `MimeTypes` 
			( `FileExtension`, `KnownMimeType` ) 
			values 
			(
				p_FileExtension, p_MimeType
			);
    end if;
    
END ;;
DELIMITER ;