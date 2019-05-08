DELIMITER ;;
CREATE PROCEDURE `GetSMTPConfiguration`(
)
BEGIN

	select `ServerDNS`, `ServerPort`, `SSLRequired`, `Username`, `PasswordEncrypted`, `DefautSender` 
		from `SMTPConfiguration` ;
    
END ;;
DELIMITER ;