CREATE PROCEDURE [SiteGetByDnsName]
	@DnsName	nvarchar(255)
AS
	SELECT 
		DISTINCT
			[Sites].[Id], [Sites].[Name], [Sites].[Description], [Sites].[Created], [Sites].[CreatedBy], [Sites].[Modified], [Sites].[ModifiedBy],
				[Sites].[ContentDBServerName], [Sites].[ContentDBName], [Sites].[QuotaBytes],   
					[DnsSites].[DnsName]  
	FROM [Sites] WITH (NOLOCK) 
		INNER JOIN [DnsSites] WITH (NOLOCK) 
			ON ([DnsSites].[SiteId] = [Sites].[Id]) 
	WHERE ([DnsSites].[DnsName] = @DnsName)
