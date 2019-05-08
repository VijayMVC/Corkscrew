CREATE PROCEDURE [GetColDefsAll]
AS
BEGIN

	SELECT [Id], [SiteId], [Name], [Type], [MaxLength], [Nullable], [Created], [CreatedBy] 
		FROM [TabularDataColDefs] WITH (NOLOCK);

END