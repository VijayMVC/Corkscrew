DELIMITER ;;
CREATE PROCEDURE `GetQueuedMailItemById`(
    IN	p_MailId	varchar(64)
)
BEGIN

	select `Id`, `From`, `Recipient`, `InternalCopyTo`, `Subject`, `ContentHtml` 
		from `MailSendQueue` 
	where (`Id` = p_MailId) ;
    
END ;;
DELIMITER ;