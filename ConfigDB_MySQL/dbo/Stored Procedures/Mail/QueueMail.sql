DELIMITER ;;
CREATE PROCEDURE `QueueMail`(
	IN	p_Id					varchar(64),
    IN	p_From					varchar(255),
    IN	p_Recipient				varchar(255),
    IN	p_InternalCC			varchar(255),
    IN	p_Subject				varchar(512),
    IN	p_ContentHtml			longtext,
    IN	p_CreatingUserId		varchar(64),
    IN	p_WasSent				bit
    
)
BEGIN

	DECLARE	v_RightNow		datetime;
    DECLARE	v_ZeroGuid		varchar(64);
    DECLARE	v_EmptyDate		datetime;
    
    SET	v_RightNow = now();
    SET v_ZeroGuid = `GuidDefault`();
    SET v_EmptyDate = `DateTimeDefault`();
    
    if (p_From is null) then
    
		select `DefaultSender` from `smtpconfiguration` into p_From;
        set p_From = ifnull(p_From, 'system@corkscrewcms.com');
    
    end if;
    
    insert into `MailSendQueue` 
		( `Id`, `From`, `Recipient`, `InternalCopyTo`, `Subject`, `ContentHtml`, 
			`WasSent`, `Created`, `CreatedBy`, `LastModified`, `LastModifiedBy` ) 
		values 
        (
			p_Id, p_From, p_Recipient, p_InternalCC, p_Subject, p_ContentHtml, 
				p_WasSent, v_RightNow, p_CreatingUserId, v_RightNow, p_CreatingUserId
        );
    
END ;;
DELIMITER ;