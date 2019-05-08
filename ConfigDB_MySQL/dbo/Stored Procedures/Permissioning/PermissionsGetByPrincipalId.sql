DELIMITER ;;
CREATE PROCEDURE `PermissionsGetByPrincipalId`(
	IN	p_PrincipalId	varchar(64)
)
BEGIN

	select 
		`PrincipalId`, `CorkscrewUri`, `IsRead`, `IsContribute`, `IsFullControl`, `IsChildAccess` 
    from `Permissions` 
    where (
		(`PrincipalId` = p_PrincipalId)
    );
    
END ;;
DELIMITER ;