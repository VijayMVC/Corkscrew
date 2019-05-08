DELIMITER ;;
CREATE PROCEDURE `UpdateMailTry`(
	IN	p_MailId		varchar(64),
    IN	p_AttemptResult	bit
)
BEGIN

	UPDATE `MailSendQueue` 
		SET 
			`WasSent` = p_AttemptResult, 
			`LastModified` = now(),
			`LastModifiedBy` = `SystemUserGuid`() 
	;
    
END ;;
DELIMITER ;