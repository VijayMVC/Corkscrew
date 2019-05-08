CREATE PROCEDURE [GetUserMemberships]
	@UserId		uniqueidentifier
AS
BEGIN

	SELECT GRP.[Id], GRP.[Username], GRP.[DisplayName], GRP.[EmailAddress], GRP.[IsWinADGroup], UGM.[IsWinADMembership]  
		FROM [UserGroups] GRP WITH (NOLOCK) 
			INNER JOIN [UserGroupMembers] UGM WITH (NOLOCK) 
				ON (UGM.[GroupId] = GRP.[Id]) 
	WHERE (
		UGM.[UserId] = @UserId
	)
		

END