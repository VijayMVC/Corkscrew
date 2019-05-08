DELIMITER ;;
CREATE PROCEDURE `DnsSitesDeleteByDnsName`(
	IN p_DnsName	varchar(255)
)
BEGIN

	delete from `DnsSites` 
		where (`DnsName` = p_DnsName);
        
END ;;
DELIMITER ;