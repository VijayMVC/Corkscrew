DELIMITER ;;
CREATE PROCEDURE `UserLogin`(
    IN p_Username		varchar(255),
    IN p_PasswordHash	char(64),
    IN p_APILogin		bit,
    IN p_RemoteAddress	varchar(255),
    IN p_APIToken		char(64)
)
BEGIN

	DECLARE v_UserId	varchar(64);
    SET v_UserId = NULL;
    
    IF (p_APILogin = 1) THEN 
		IF ((IFNULL(p_RemoteAddress, '') <> '') AND (IFNULL(p_APIToken, '') <> '')) THEN 
			SELECT `Id` FROM `Users` WHERE ((`Username` = p_Username) and (`SecretHash` = p_PasswordHash)) into v_UserId;
            IF (IFNULL(v_UserId, '') <> '') THEN 
				INSERT INTO `APILogins` (`Id`, `UserId`, `RemoteAddress`, `APIToken`, `Created`) 
					VALUES (
						UUID(),
                        v_UserId,
                        p_RemoteAddress,
                        p_APIToken,
                        NOW()
                    );
            END IF;
        END IF;
	ELSE
		select `Id`, `Username`, `SecretHash`, `DisplayName`, `EmailAddress`, `IsWinADUser`
			from `Users` 
		where ((`Username` = p_Username) and (`SecretHash` = p_PasswordHash));
    END IF;
        
END ;;
DELIMITER ;