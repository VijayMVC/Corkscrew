DELIMITER ;;
CREATE PROCEDURE `PermissionsGetAllAdministrators`(
)
BEGIN

	select 
		`PrincipalId`, `CorkscrewUri`
    from `Permissions` 
    where (
		(`IsFullControl` = 1) 
        and (
			(`CorkscrewUri` = 'corkscrew://') 
            or (length(`CorkscrewUri`) = 49)
        )
    );
    
END ;;
DELIMITER ;