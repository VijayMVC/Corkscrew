CREATE PROCEDURE [GetColDefById]
	@Id			uniqueidentifier
AS
BEGIN

	SELECT [Id], [SiteId], [Name], [Type], [MaxLength], [Nullable], [Created], [CreatedBy] 
		FROM [TabularDataColDefs] WITH (NOLOCK) 
	WHERE ([Id] = @Id);

END