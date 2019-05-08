DELIMITER ;;
CREATE PROCEDURE `DropColDef` 
(
	IN p_Id				varchar(64)
)
BEGIN
	
    delete from `TabularDataColDefs` 
		where (`Id` = p_Id);
    
END ;;
DELIMITER ;