DELIMITER ;;
CREATE PROCEDURE `SetColDefUsage` 
(
	IN p_Id				varchar(64),
    IN p_TableName		varchar(255)
)
BEGIN
	
    SET p_TableName = UCASE(p_TableName);
    
    if ((select count(*) from `TabularDataColDefUsage` where ((`ColDefId` = p_Id) and (`TableName` = p_TableName))) = 0) then 
		insert into `TabularDataColDefUsage` 
			( `ColDefId`, `TableName` ) 
            values 
            (
				p_Id, 
                p_TableName
            );
    end if;
    
END ;;
DELIMITER ;