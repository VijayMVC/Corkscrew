DELIMITER ;;
CREATE PROCEDURE `PermissionsGetByResourceUri`(
	IN	p_CorkscrewResourceUri		longtext
)
BEGIN

	select 
		`PrincipalId`, `CorkscrewUri`, `IsRead`, `IsContribute`, `IsFullControl`, `IsChildAccess` 
    from `Permissions` 
    where (
		(`CorkscrewUri` = p_CorkscrewResourceUri) 
    );
    
END ;;
DELIMITER ;