CREATE PROCEDURE [UserGroupGetAll]
AS
BEGIN

	SELECT [Id], [Username], [DisplayName], [EmailAddress], [IsWinADGroup] FROM [UserGroups] WITH (NOLOCK);

END