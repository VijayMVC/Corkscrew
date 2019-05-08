DELIMITER ;;
CREATE PROCEDURE `PermissionSave`(
    IN  p_PrincipalId	varchar(64),
    IN	p_CorkscrewUri	longtext,
    IN  p_Read			bit,
    IN  p_Contribute	bit,
    IN  p_FullControl   bit
)
BEGIN

	declare	v_EmptyGuid  varchar(64);
    set v_EmptyGuid = `GuidDefault`();
    
    if (p_PrincipalId = v_EmptyGuid) then 
		call raise_error;
	end if;
    
    if ((p_Read = 0) and (p_Contribute = 0) and (p_FullControl = 0)) then 
		delete from `Permissions` 
			where ((`PrincipalId` = p_PrincipalId) and (`CorkscrewUri` = p_CorkscrewUri));
	else
    
		if ((select count(*) from `Permissions` where ((`PrincipalId` = p_PrincipalId) and (`CorkscrewUri` = p_CorkscrewUri))) <> 1) then 
			insert into `Permissions` 
				( `Id`, `PrincipalId`, `CorkscrewUri`, 
					`IsRead`, `IsContribute`, `IsFullControl`, `IsChildAccess` ) 
				values 
				(
					uuid(), p_PrincipalId, p_CorkscrewUri, 
						p_Read, p_Contribute, p_FullControl, 0
				);
		else 
			update `Permissions` 
				set 
					`IsRead` = p_Read,
					`IsContribute` = p_Contribute,
					`IsFullControl` = p_FullControl,
					`IsChildAccess` = if(((p_Read = 1) or (p_Contribute = 1) or (p_FullControl = 1)), 0, `IsChildAccess`)
			where ((`PrincipalId` = p_PrincipalId) and (`CorkscrewUri` = p_CorkscrewUri));
		end if;

	end if;
    
END ;;
DELIMITER ;