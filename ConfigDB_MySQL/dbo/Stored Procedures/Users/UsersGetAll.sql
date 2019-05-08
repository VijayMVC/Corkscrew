DELIMITER ;;
CREATE PROCEDURE `UsersGetAll`(
)
BEGIN

	select 
		`Id`, `Username`, `SecretHash`, `DisplayName`, `EmailAddress`, `IsWinADUser` 
	from `Users`;
        
END ;;
DELIMITER ;