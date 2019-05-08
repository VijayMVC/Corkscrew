DELIMITER ;;
CREATE PROCEDURE `SitesGetAll`(
)
BEGIN

	select 
		`Id`, `Name`, `Description`, 
			`ContentDBServerName`, `ContentDBName`, `QuotaBytes`,
				`Created`, `CreatedBy`, `Modified`, `ModifiedBy` 
	from `Sites`;
        
END ;;
DELIMITER ;