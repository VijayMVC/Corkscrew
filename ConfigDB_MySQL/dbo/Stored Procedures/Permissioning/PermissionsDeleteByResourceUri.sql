DELIMITER ;;
CREATE PROCEDURE `PermissionsDeleteByResourceUri`(
    IN	p_CorkscrewResourceUri		longtext
)
BEGIN

	declare v_SubTreeUri longtext;
    
    set v_SubTreeUri = p_CorkscrewResourceUri;
    
    if (substring(v_SubTreeUri, (length(v_SubTreeUri) - 1), 1) <> '/') then 
		set v_SubTreeUri = concat(v_SubTreeUri, '/');
    end if;

	delete from `Permissions` 
		where ((`CorkscrewUri` = p_CorkscrewResourceUri) and (`CorkscrewUri` like v_SubTreeUri));
    
END ;;
DELIMITER ;