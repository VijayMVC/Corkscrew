DELIMITER ;;
CREATE PROCEDURE `UsersGetById`(
	IN p_Id			varchar(64)
)
BEGIN

	select 
		`Id`, `Username`, `SecretHash`, `DisplayName`, `EmailAddress`, `IsWinADUser` 
	from `Users` 
    where (`Id` = p_Id);
        
END ;;
DELIMITER ;