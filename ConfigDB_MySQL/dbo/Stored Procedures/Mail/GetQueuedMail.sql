DELIMITER ;;
CREATE PROCEDURE `GetQueuedMail`(
    IN	p_Count		int
)
BEGIN

	select `Id`, `From`, `Recipient`, `InternalCopyTo`, `Subject`, `ContentHtml` 
		from `MailSendQueue` 
	where (`WasSent` = 0) 
    order by `Created` asc 
    limit p_Count;
    
END ;;
DELIMITER ;