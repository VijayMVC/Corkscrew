DELIMITER ;;
CREATE PROCEDURE `GetColDef` 
(
	IN p_Id				varchar(64)
)
BEGIN
	
    select `Id`, `Name`, `Type`, `MaxLength`, `Nullable`, `MinValue`, `MaxValue`  
		from `TabularDataColDefs` 
	where (`Id` = p_Id);
    
END ;;
DELIMITER ;