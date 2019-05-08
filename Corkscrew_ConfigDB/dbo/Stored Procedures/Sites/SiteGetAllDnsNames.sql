CREATE PROCEDURE [SiteGetAllDnsNames]
	@SiteId		uniqueidentifier
AS
	SELECT [DnsName] 
		FROM [DnsSites] WITH (NOLOCK) 
	WHERE ([SiteId] = @SiteId) 
