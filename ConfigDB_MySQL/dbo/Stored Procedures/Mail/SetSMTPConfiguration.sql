DELIMITER ;;
CREATE PROCEDURE `SetSMTPConfiguration`(
	IN	p_ServerDns			varchar(1024),
    IN	p_ServerPort		int,
    IN	p_SSLRequired		bit,
    IN 	p_Username			varchar(255),
    IN  p_PasswordEnc		varchar(1024),
    IN  p_DefaultSender		varchar(255),
    IN  p_CreatingUserId	varchar(64)
)
BEGIN

	UPDATE `SMTPConfiguration` 
		SET 
			`ServerDNS` = p_ServerDns, 
			`ServerPort` = p_ServerPort,
			`SSLRequired` = p_SSLRequired,
			`Username` = p_Username,
			`PasswordEncrypted` = p_PasswordEnc,
			`DefaultSender` = p_DefaultSender, 
			`LastModified` = now(),
			`LastModifiedBy` = p_CreatingUserId 
	;
    
END ;;
DELIMITER ;