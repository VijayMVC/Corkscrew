CREATE PROCEDURE [GetUsersNotInGroupByGroupId]
	@GroupId		uniqueidentifier
AS
BEGIN

	SELECT USR.[Id], USR.[Username], USR.[SecretHash], USR.[DisplayName], USR.[EmailAddress], USR.[IsWinADUser], UGM.[IsWinADMembership]  
		FROM [Users] USR WITH (NOLOCK) 
			LEFT JOIN [UserGroupMembers] UGM WITH (NOLOCK) 
				ON (UGM.[UserId] = USR.[Id]) 
	WHERE (
		(UGM.[GroupId] = @GroupId) 
		AND (UGM.[UserId] IS NULL)
	) 

END