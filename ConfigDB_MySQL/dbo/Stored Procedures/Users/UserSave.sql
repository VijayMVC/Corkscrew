DELIMITER ;;
CREATE PROCEDURE `UserSave`(
	IN p_Id				varchar(64),
    IN p_Username		varchar(255),
    IN p_SecretHash		char(64),
    IN p_DisplayName	varchar(255),
    IN p_EmailAddress	varchar(512), 
    IN p_IsWinADUser	bit
)
BEGIN

	declare v_UserId	varchar(64);
    declare v_test		varchar(64);
    
    set v_test = uuid();
    
    select `Id` from `Users` where ((`Id` = p_Id) or (`Username` = p_Username)) into v_UserId;
    
    if (ifnull(v_UserId, v_test) = v_test) then 
		insert into `Users` 
			( `Id`, `Username`, `SecretHash`, `DisplayName`, `EmailAddress`, `IsWinADUser` ) 
            values 
            (
				p_Id, p_Username, p_SecretHash, p_DisplayName, p_EmailAddress, p_IsWinADUser
            );
	else 
		update `Users` 
			set 
				`SecretHash` = ifnull(p_SecretHash, `SecretHash`),
                `DisplayName` = p_DisplayName, 
                `EmailAddress` = p_EmailAddress 
		where (`Id` = p_Id);
    end if;
        
END ;;
DELIMITER ;