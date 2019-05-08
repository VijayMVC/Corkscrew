DELIMITER ;;
CREATE PROCEDURE `SiteGetById`(
	IN p_Id		varchar(64)
)
BEGIN

	select 
		`Id`, `Name`, `Description`, 
			`ContentDBServerName`, `ContentDBName`, `QuotaBytes`,
				`Created`, `CreatedBy`, `Modified`, `ModifiedBy` 
	from `Sites` 
	where (`Id` = p_Id);
        
END ;;
DELIMITER ;