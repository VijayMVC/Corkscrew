DELIMITER ;;
CREATE PROCEDURE `RemoveUserFromGroup`(
	IN p_UserId			varchar(64), 
	IN p_GroupId		varchar(64)
)
BEGIN
	
	DELETE FROM `UserGroupMembers` WHERE ((`UserId` = p_UserId) AND (`GroupId` = p_GroupId)));
    
END ;;
DELIMITER ;