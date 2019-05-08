DELIMITER ;;
CREATE PROCEDURE `ConfigurationGetAll`()
BEGIN
	
    select `Name`, `Value` from `Configuration`;
    
END ;;
DELIMITER ;