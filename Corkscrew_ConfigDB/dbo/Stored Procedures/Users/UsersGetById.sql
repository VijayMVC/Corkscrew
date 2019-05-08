CREATE PROCEDURE [UsersGetById]
	@Id		uniqueidentifier 
AS
	SELECT [Id], [Username], [SecretHash], [DisplayName], [EmailAddress], [IsWinADUser] 
		FROM [Users] WITH (NOLOCK) 
	WHERE ([Id] = @Id)
