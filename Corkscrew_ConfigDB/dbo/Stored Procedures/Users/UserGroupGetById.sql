CREATE PROCEDURE [UserGroupGetById]
	@GroupId		uniqueidentifier
AS
BEGIN

	SELECT [Id], [Username], [DisplayName], [EmailAddress], [IsWinADGroup]
		FROM [UserGroups] WITH (NOLOCK) 
	WHERE ([Id] = @GroupId)

END