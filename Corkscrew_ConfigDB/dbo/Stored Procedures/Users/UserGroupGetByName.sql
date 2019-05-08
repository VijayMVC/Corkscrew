CREATE PROCEDURE [UserGroupGetByName]
	@Username		nvarchar(255)
AS
BEGIN

	SELECT [Id], [Username], [DisplayName], [EmailAddress], [IsWinADGroup]
		FROM [UserGroups] WITH (NOLOCK) 
	WHERE ([Username] = @Username)

END