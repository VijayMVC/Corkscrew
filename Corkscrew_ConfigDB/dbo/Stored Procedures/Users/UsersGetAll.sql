CREATE PROCEDURE [UsersGetAll]
AS
	SELECT [Id], [Username], [SecretHash], [DisplayName], [EmailAddress], [IsWinADUser]  
		FROM [Users] WITH (NOLOCK)
