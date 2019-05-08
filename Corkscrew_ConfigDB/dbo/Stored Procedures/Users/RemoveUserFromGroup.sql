CREATE PROCEDURE [RemoveUserFromGroup]
	@UserId			uniqueidentifier,
	@GroupId		uniqueidentifier
AS
BEGIN

	DELETE FROM [UserGroupMembers] 
			WHERE (([UserId] = @UserId) AND ([GroupId] = @GroupId))

END