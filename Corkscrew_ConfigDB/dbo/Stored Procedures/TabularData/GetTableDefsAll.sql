CREATE PROCEDURE [GetTableDefsAll] 
	@SiteId		uniqueidentifier
AS
BEGIN

	SELECT [Id], [SiteId], [Name], [UniqueName], [FriendlyName], [Description], [Created], [CreatedBy], [Modified], [ModifiedBy] 
		FROM [TabularDataTableDefs] WITH (NOLOCK) 
	WHERE ([SiteId] = @SiteId);

END