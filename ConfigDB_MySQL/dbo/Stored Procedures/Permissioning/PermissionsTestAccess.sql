DELIMITER ;;
CREATE PROCEDURE `PermissionsTestAccess`(
	IN	p_CorkscrewResourceUri		longtext,
    IN  p_PrincipalId				varchar(64)
)
BEGIN

	declare	v_AccessRead 		bit;
    declare v_AccessContribute 	bit;
    declare v_AccessFullControl	bit;
    declare	v_AccessSpecial		bit;
    declare	v_SystemUser		varchar(64);
    declare	v_Found				bit;
    declare v_EmptyGuid			varchar(64);
    
    set v_AccessRead = 0;
    set v_AccessContribute = 0;
    set v_AccessFullControl = 0;
    set v_AccessSpecial = 0;
    set v_Found = 0;
    set v_SystemUser = `SystemUserGuid`();
    set v_EmptyGuid = `GuidDefault`();
    
	IF (p_PrincipalId = v_emptyGuid) THEN 
		SET p_PrincipalId = NULL;
	END IF;
    
    if (p_PrincipalId in (v_SystemUser, '99999999-0100-0043-4f52-4b5343524557')) then 
		set v_AccessRead = 1;
        set v_AccessSpecial = 0;
        set v_Found = 1;
        
        case p_PrincipalId 
        
			when v_SystemUser then 
				set v_AccessContribute = 1;
                set v_AccessFullControl = 1;
                
			else 
				set v_AccessContribute = 0;
                set v_AccessFullControl = 0;
                
        end case;
	else 
		select 
			`IsRead`, `IsChildAccess`, `IsContribute`, `IsFullControl`, 1 
		from `Permissions` 
		where (
			(`CorkscrewUri` = p_CorkscrewResourceUri) 
			and (`PrincipalId` = p_PrincipalId) 
		) 
        into v_AccessRead, v_AccessSpecial, v_AccessContribute, v_AccessFullControl, v_Found;
	end if;
    
    select 
		CAST(ifnull(v_AccessRead, 0) as unsigned) as 'IsRead', 
        CAST(ifnull(v_AccessContribute, 0) as unsigned) as 'IsContribute', 
        CAST(ifnull(v_AccessFullControl, 0) as unsigned) as 'IsFullControl', 
        CAST(ifnull(v_AccessSpecial, 0) as unsigned) as 'IsChildAccess', 
        p_CorkscrewResourceUri as 'CorkscrewResourceUri', 
        CAST(p_PrincipalId as char(64)) as 'PrincipalId',
        CAST(v_Found as unsigned) as 'PermissionsFound'
    ;
    
END ;;
DELIMITER ;