DELIMITER ;;
CREATE FUNCTION `SystemUserGuid`() RETURNS varchar(64) CHARSET latin1 COLLATE latin1_general_ci
BEGIN
    RETURN '99999999-0000-0043-4f52-4b5343524557';
    
END ;;
DELIMITER ;