DELIMITER ;;
CREATE PROCEDURE `SaveSignaturePanelItem`(
	IN p_Id						varchar(64),
    IN p_PanelId				varchar(64),
    IN p_RespondentId			varchar(64),
    IN p_IsDecider				bit,
    IN p_IsTieBreaker			bit,
    IN p_IsMandatory			bit,
    IN p_Response				int,
    IN p_Comment				varchar(1024),
    IN p_SentToResponder		bit,
    IN p_ModifiedBy				varchar(64),
    IN p_Modified				datetime
)
BEGIN

	declare v_RightNow datetime;
    set v_RightNow = now();
    
    if ((select count(*) from `SignaturePanelItem` where (`Id` = p_Id)) = 0) then 
		insert into `signaturepanelitem` 
			(
				`Id`, `PanelId`, `RespondentId`, `IsFinalDecider`, `IsTieBreaker`, `IsMandatory`, 
					`Response`, `Comment`, `RespondedOn`, `SentToResponder`,
						`Created`, `CreatedBy`, `Modified`, `ModifiedBy`
            )
            values 
            (
				p_Id, p_PanelId, p_RespondentId, p_IsFinalDecider, p_IsTieBreaker, p_IsMandatory, 
					p_Response, if((p_Response = 0), null, p_Comment), if ((p_Response = 0), null, v_RightNow), p_SentToResponder,
						p_Modified, p_ModifiedBy, p_Modified, p_ModifiedBy
            );
	else 
		update `SignaturePanelItem` 
			set 
				`Response` = p_Response,
                `Comment` = if((p_Response = 0), `Comment`, p_Comment), 
                `RespondedOn` = if((p_Response = 0), `RespondedOn`, v_RightNow),
                `SentToResponder` = p_SentToResponder, 
                `Modified` = p_Modified,
                `ModifiedBy` = p_ModifiedBy
		where (`Id` = p_Id);
    end if;
        
END ;;
DELIMITER ;