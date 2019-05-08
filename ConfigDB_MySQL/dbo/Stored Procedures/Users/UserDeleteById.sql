DELIMITER ;;
CREATE PROCEDURE `UserDeleteById`(
	IN p_Id			varchar(64)
)
BEGIN

	declare exit handler for sqlexception
    begin
		rollback;
	end;

	start transaction;

	delete from `Permissions` 
		where (`PrincipalId` = p_Id);

	delete from `Users` 
		where (`Id` = p_Id);
	
    commit;
        
END ;;
DELIMITER ;