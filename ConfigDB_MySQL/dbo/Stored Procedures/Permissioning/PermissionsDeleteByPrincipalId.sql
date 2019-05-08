DELIMITER ;;
CREATE PROCEDURE `PermissionDeleteByPrincipalId`(
    IN	p_PrincipalId		varchar(64)
)
BEGIN

	delete from `Permissions` 
		where (`PrincipalId` = p_PrincipalId);
    
END ;;
DELIMITER ;