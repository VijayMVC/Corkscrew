CREATE PROCEDURE [SiteGetById]
	@Id		uniqueidentifier
AS
	SELECT 
			[Id], [Name], [Description], [Created], [CreatedBy], [Modified], [ModifiedBy],
				[ContentDBServerName], [ContentDBName], [QuotaBytes]
	FROM [Sites] WITH (NOLOCK) 
	WHERE ([Id] = @Id)
