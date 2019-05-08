DELIMITER ;;
CREATE PROCEDURE `UserGroupGetById`(
	IN p_GroupId			varchar(64)
)
BEGIN

	select 
		`Id`, `Username`, `DisplayName`, `EmailAddress`, `IsWinADGroup` 
	from `UserGroups` 
    where (`Id` = p_GroupId);
        
END ;;
DELIMITER ;