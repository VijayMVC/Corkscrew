DELIMITER ;;
CREATE PROCEDURE `SiteGetHistoryById`(
	IN p_Id					varchar(64)
)
BEGIN

	SELECT 
		`Id`, `SiteId`, `IsLocked`, `IsProcessed`, `ChangeType`, `ChangeTimeStamp`, `ChangedBy`, 
			`PrevName`, `PrevDescription`, `PrevDBServer`, `PrevDBName`, `PrevQuotaBytes`, `PrevModified`, `PrevModifiedBy`
	FROM `SitesChangeLog` 
	WHERE (`Id` = p_Id);
    
END ;;
DELIMITER ;