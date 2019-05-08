CREATE PROCEDURE [UserGroupDeleteById]
	@GroupId		uniqueidentifier
AS
BEGIN

	BEGIN TRANSACTION 

	BEGIN TRY
		DELETE FROM [Permissions] WHERE ([PrincipalId] = @GroupId)
		DELETE FROM [UserGroupMembers] WHERE ([GroupId] = @GroupId) 
		DELETE FROM [UserGroups] WHERE ([Id] = @GroupId) 

		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION
	END CATCH

END