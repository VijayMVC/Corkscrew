CREATE PROCEDURE [UserGetByName]
	@Username	NVARCHAR(255)
AS
	SELECT [Id], [Username], [SecretHash], [DisplayName], [EmailAddress], [IsWinADUser] 
		FROM [Users] WITH (NOLOCK) 
	WHERE ([Username] = @Username)
