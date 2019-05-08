DELIMITER ;;
CREATE FUNCTION `DateTimeDefault`() RETURNS datetime
BEGIN
    DECLARE dt	datetime	default null ;
    
    RETURN str_to_date('01/01/2000 00:00:00', '%m/%d/%Y %H:%i:%s');
    
END ;;
DELIMITER ;