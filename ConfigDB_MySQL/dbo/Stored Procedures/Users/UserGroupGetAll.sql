DELIMITER ;;
CREATE PROCEDURE `UserGroupGetAll`(
)
BEGIN

	select 
		`Id`, `Username`, `DisplayName`, `EmailAddress`, `IsWinADGroup` 
	from `UserGroups`;
        
END ;;
DELIMITER ;