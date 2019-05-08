DELIMITER ;;
CREATE PROCEDURE `PermissionsSetChildAccess`(
	IN	p_CorkscrewResourceUri		longtext,
    IN  p_PrincipalId				varchar(64),
    IN	p_ChildAccess				bit
)
BEGIN

	declare	v_ExistingChildAccess	bit;
    set v_ExistingChildAccess = 0;
    
	select 
		`IsChildAccess` 
    from `permissions` 
    where (
		(`PrincipalId` = p_PrincipalId) 
        and (`CorkscrewUri` = p_CorkscrewResourceUri)
    ) 
    into v_ExistingChildAccess;
    
    if ((v_ExistingChildAccess is null) and (p_ChildAccess = 1)) then 
		insert into `Permissions` 
			( `PrincipalId`, `CorkscrewUri`, 
				`IsRead`, `IsContribute`, `IsFullControl`, `IsChildAccess` ) 
			values 
			(
				p_PrincipalId, p_CorkscrewResourceUri, 
					0, 0, 0, p_ChildAccess
			);
	else 
		if ((v_ExistingChildAccess = 1) and (p_ChildAccess = 0)) then 
			delete from `Permissions` 
				where (
					(`PrincipalId` = p_PrincipalId) 
					and (`CorkscrewUri` = p_CorkscrewResourceUri)
				);
        end if;
	end if;
    
END ;;
DELIMITER ;