DELIMITER ;;
CREATE PROCEDURE `SiteGetAllDnsNames`(
	IN p_SiteId		varchar(64)
)
BEGIN

	select `DnsName` from `DNSSites` 
    where (`SiteId` = p_SiteId);
        
END ;;
DELIMITER ;