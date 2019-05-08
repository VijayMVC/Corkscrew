DELIMITER ;;
CREATE PROCEDURE `UserGetByName`(
	IN p_Username			varchar(255)
)
BEGIN

	select 
		`Id`, `Username`, `SecretHash`, `DisplayName`, `EmailAddress`, `IsWinADUser` 
	from `Users` 
    where (`Username` = p_Username);
        
END ;;
DELIMITER ;