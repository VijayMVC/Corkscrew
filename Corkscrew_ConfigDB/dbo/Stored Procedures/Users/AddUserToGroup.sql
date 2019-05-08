CREATE PROCEDURE [AddUserToGroup]
	@UserId			uniqueidentifier,
	@GroupId		uniqueidentifier,
	@IsWinADMap		bit
AS
BEGIN

	IF ((SELECT COUNT(*) FROM [UserGroupMembers] WITH (NOLOCK) WHERE (([UserId] = @UserId) AND ([GroupId] = @GroupId))) = 0) 
	BEGIN
		INSERT INTO [UserGroupMembers] 
			( [UserId], [GroupId], [IsWinADMembership] ) 
			VALUES 
			(
				@UserId,
				@GroupId,
				@IsWinADMap
			)
	END

END