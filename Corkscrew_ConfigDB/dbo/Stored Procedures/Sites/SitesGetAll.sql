CREATE PROCEDURE [SitesGetAll]
AS
	SELECT 
			[Id], [Name], [Description], [Created], [CreatedBy], [Modified], [ModifiedBy], 
				[ContentDBServerName], [ContentDBName], [QuotaBytes]
	FROM [Sites] WITH (NOLOCK) 
