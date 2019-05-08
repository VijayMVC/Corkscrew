DELIMITER ;;
CREATE PROCEDURE `SiteGetHistoryBySiteId`(
	IN p_Id					varchar(64)
)
BEGIN

	SELECT 
		`Id`, `SiteId`, `IsLocked`, `IsProcessed`, `ChangeType`, `ChangeTimeStamp`, `ChangedBy`, 
			`PrevName`, `PrevDescription`, `PrevDBServer`, `PrevDBName`, `PrevQuotaBytes`, `PrevModified`, `PrevModifiedBy`
	FROM `SitesChangeLog` 
	WHERE (`SiteId` = p_Id);
    
END ;;
DELIMITER ;