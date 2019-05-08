DELIMITER ;;
CREATE PROCEDURE `UserGroupGetByName`(
	IN p_Username			varchar(255)
)
BEGIN

	select 
		`Id`, `Username`, `DisplayName`, `EmailAddress`, `IsWinADGroup` 
	from `UserGroups` 
    where (`Username` = p_Username);
        
END ;;
DELIMITER ;