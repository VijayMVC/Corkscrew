DELIMITER ;;
CREATE PROCEDURE `UserGroupDeleteById`(
	IN p_GroupId			varchar(64)
)
BEGIN

	declare exit handler for sqlexception
    begin
		rollback;
	end;

	start transaction;

	delete from `Permissions` 
		where (`PrincipalId` = p_GroupId);
        
	delete from `UserGroupMembers` 
		where (`GroupId` = p_GroupId);

	delete from `UserGroups` 
		where (`Id` = p_GroupId);
	
    commit;
        
END ;;
DELIMITER ;