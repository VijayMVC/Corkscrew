DELIMITER ;;
CREATE PROCEDURE `AddUserToGroup`(
	IN p_UserId			varchar(64), 
	IN p_GroupId		varchar(64),
	IN p_IsWinADMap		bit
)
BEGIN
	
	IF ((SELECT COUNT(*) FROM `UserGroupMembers` WHERE ((`UserId` = p_UserId) AND (`GroupId` = p_GroupId))) = 0) THEN 
    
		INSERT INTO `UserGroupMembers` (`UserId`, `GroupId`, `IsWinADMembership`) 
			VALUES 
            (
				p_UserId, 
                p_GroupId, 
                p_IsWinADMap
            );
		
    END IF;
    
END ;;
DELIMITER ;