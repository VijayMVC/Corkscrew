DELIMITER ;;
CREATE PROCEDURE `SiteGetByDnsName`(
	IN p_DnsName		varchar(255)
)
BEGIN

	select distinct 
		s.`Id`, s.`Name`, s.`Description`, 
			s.`ContentDBServerName`, s.`ContentDBName`, s.`QuotaBytes`,
				s.`Created`, s.`CreatedBy`, s.`Modified`, s.`ModifiedBy` 
	from `sites` s 
		inner join `DnsSites` ds on (ds.`SiteId` = s.`Id`) 
	where (ds.`DnsName` = p_DnsName);
        
END ;;
DELIMITER ;