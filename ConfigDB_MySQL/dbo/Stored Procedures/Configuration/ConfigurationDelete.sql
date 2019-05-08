DELIMITER ;;
CREATE PROCEDURE `ConfigurationDelete`(IN p_Name varchar(255))
BEGIN
	delete from `Configuration` 
	where (`Name` = p_Name);
    
END ;;
DELIMITER ;