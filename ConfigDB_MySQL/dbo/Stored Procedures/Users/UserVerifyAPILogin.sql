DELIMITER ;;
CREATE PROCEDURE `UserVerifyAPILogin`(
    IN p_APIToken		char(64),
    IN p_RemoteAddress	varchar(255)
)
BEGIN

	select usr.`Id`, usr.`Username`, usr.`SecretHash`, usr.`DisplayName`, usr.`EmailAddress`, usr.`IsWinADUser`
        from `Users` usr 
			inner join `APILogins` api on (api.`UserId` = usr.`Id`)
	where ((api.`APIToken` = p_APIToken) and (api.`RemoteAddress` = p_RemoteAddress));
        
END ;;
DELIMITER ;