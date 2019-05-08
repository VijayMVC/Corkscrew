DELIMITER ;;
CREATE PROCEDURE `SaveSignaturePanel`(
	IN p_Id						varchar(64),
    IN p_Type					int,
    IN p_TimeLimited			bit,
    IN p_Deadline				datetime,
    IN p_State					int,
    IN p_ResponsesInProgress	bit,
    IN p_ModifiedBy				varchar(64),
    IN p_Modified				datetime
)
BEGIN

	declare v_RightNow datetime;
    set v_RightNow = now();
    
    if ((select count(*) from `SignaturePanel` where (`Id` = p_Id)) = 0) then 
		insert into `signaturepanel` 
			(
				`Id`, `PanelType`, `IsTimeLimited`, `Deadline`, `CurrentState`, `ResponsesInProgress`,
						`Created`, `CreatedBy`, `Modified`, `ModifiedBy`
            )
            values 
            (
				p_Id, p_Type, p_TimeLimited, if((p_TimeLimited = 1), p_Deadline, `DateTimeDefault`()), p_State, p_ResponsesInProgress,
						p_Modified, p_ModifiedBy, p_Modified, p_ModifiedBy
            );
	else 
		update `SignaturePanel` 
			set 
				`Deadline` = if((p_TimeLimited = 1), p_Deadline, `DateTimeDefault`()),
                `CurrentState` = p_State,
                `ResponsesInProgress` = p_ResponsesInProgress,
				`Modified` = p_Modified,
                `ModifiedBy` = p_ModifiedBy
		where (`Id` = p_Id);
    end if;
    
END ;;
DELIMITER ;