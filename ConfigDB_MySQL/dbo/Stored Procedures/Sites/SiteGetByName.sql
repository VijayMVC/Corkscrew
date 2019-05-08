DELIMITER ;;
CREATE PROCEDURE `SiteGetByName`(
	IN p_Name		varchar(255)
)
BEGIN

	select 
		`Id`, `Name`, `Description`, 
			`ContentDBServerName`, `ContentDBName`, `QuotaBytes`,
				`Created`, `CreatedBy`, `Modified`, `ModifiedBy` 
	from `Sites` 
	where (`Name` = p_Name);
        
END ;;
DELIMITER ;