DELIMITER ;;
CREATE PROCEDURE `ConfigurationGetByName`(IN p_Name varchar(255))
BEGIN
	
    select `Name`, `Value` from `Configuration` 
    where (`Name` = p_Name);
    
END ;;
DELIMITER ;