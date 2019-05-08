CREATE PROCEDURE [DnsSitesDeleteByDnsName]
	@DnsName		nvarchar(255)
AS
	DELETE FROM [DnsSites] 
		WHERE ([DnsName] = @DnsName)