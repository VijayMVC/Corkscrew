DELIMITER ;;
CREATE PROCEDURE `GetColDefUsage` 
(
	IN p_Id				varchar(64)
)
BEGIN
	
    select `Id`, `TableName`, `ColDefId`
		from `TabularDataColDefUsage` 
	where (`Id` = p_Id);
    
END ;;
DELIMITER ;