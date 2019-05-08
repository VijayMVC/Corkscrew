DELIMITER ;;
CREATE PROCEDURE `DnsSitesSave`(
	IN p_DnsName	varchar(255),
    IN p_SiteId		varchar(64)
)
BEGIN

	if ((select count(*) from `DnsSites` where ((`DnsName` = p_DnsName) and (`SiteId` <> p_SiteId))) = 1) then 
		call raise_error;
	else 
		insert into `dnssites` 
			( `DnsName`, `SiteId` ) 
			values 
			(
				p_DnsName,
				p_SiteId
			);
    end if;
        
END ;;
DELIMITER ;