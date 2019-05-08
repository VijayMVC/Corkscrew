CREATE PROCEDURE [SiteGetByName]
	@Name		nvarchar(255)
AS
	SELECT 
			[Id], [Name], [Description], [Created], [CreatedBy], [Modified], [ModifiedBy],
				[ContentDBServerName], [ContentDBName], [QuotaBytes]    
	FROM [Sites] WITH (NOLOCK) 
	WHERE ([Name] = @Name) 
