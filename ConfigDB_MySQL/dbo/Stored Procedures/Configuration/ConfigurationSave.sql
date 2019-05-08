DELIMITER ;;
CREATE PROCEDURE `ConfigurationSave`(IN p_Name varchar(255), IN p_Value varchar(4000))
BEGIN
   
    if ((select count(*) from `Configuration` where (`Name` = p_Name)) = 0) then 
		insert into `Configuration` (`Name`, `Value`) 
			values (p_Name, p_Value);
    else 
		update `Configuration` 
			set 
				`Value` = p_Value 
		where (`Name` = p_Name);
	end if;
    
END ;;
DELIMITER ;