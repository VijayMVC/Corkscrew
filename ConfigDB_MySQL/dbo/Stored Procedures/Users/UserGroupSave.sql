DELIMITER ;;
CREATE PROCEDURE `UserGroupSave`(
	IN p_Id				varchar(64),
    IN p_Username		varchar(255),
    IN p_DisplayName	varchar(255),
    IN p_EmailAddress	varchar(512), 
    IN p_IsWinADGroup	bit
)
BEGIN

    if ((SELECT COUNT(*) FROM `UserGroups` WHERE ((`Id` = p_Id) or (`Username` = p_Username))) = 0) then 
		insert into `UserGroups` 
			( `Id`, `Username`, `DisplayName`, `EmailAddress`, `IsWinADGroup` ) 
            values 
            (
				p_Id, p_Username, p_DisplayName, p_EmailAddress, p_IsWinADGroup
            );
	else 
		update `UserGroups` 
			set 
                `DisplayName` = p_DisplayName, 
                `EmailAddress` = p_EmailAddress 
		where (`Id` = p_Id);
    end if;
        
END ;;
DELIMITER ;