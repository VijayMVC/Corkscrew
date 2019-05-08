DELIMITER ;;
CREATE PROCEDURE `PermissionsTestAdministrator`(
	IN	p_SiteId		varchar(64),
    IN  p_PrincipalId	varchar(64)
)
BEGIN

	declare	v_IsAdmin 				bit;
    declare	v_EmptyGuid				varchar(64);
    declare v_CorkscrewResourceUri	longtext;
    
    set v_IsAdmin = 0;
    set v_EmptyGuid = `GuidDefault`();
    
    if (p_PrincipalId = '99999999-0100-0043-4f52-4b5343524557') then 
		select 
			v_EmptyGuid as 'SiteId',
            p_PrincipalId as 'PrincipalId', 
            1 as 'IsAdmin'
		;
	else
		set v_CorkscrewResourceUri = 'corkscrew://';
        if (ifnull(v_CorkscrewResourceUri, v_EmptyGuid) = v_EmptyGuid) then 
			set v_CorkscrewResourceUri = concat('corkscrew://', lower(p_SiteId), '/');
        end if;
    
		select 
			1
		from `Permissions` 
		where (
			(`CorkscrewUri` = v_CorkscrewResourceUri)
			and (`PrincipalId` = p_PrincipalId) 
            and (`IsFullControl` = 1)
		) 
        into v_IsAdmin;
	
		select 
			p_SiteId as 'SiteId', 
            p_PrincipalId as 'PrincipalId', 
            v_IsAdmin as 'IsAdmin'
		;
        
	end if;
        
END ;;
DELIMITER ;