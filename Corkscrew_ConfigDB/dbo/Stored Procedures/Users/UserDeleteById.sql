CREATE PROCEDURE [UserDeleteById]
	@Id		uniqueidentifier
AS
BEGIN
	BEGIN TRAN

	BEGIN TRY

		DELETE FROM [Permissions] 
			WHERE ([PrincipalId] = @Id)

		--- Delete user
		DELETE FROM [Users] 
			WHERE ([Id] = @Id)

		COMMIT TRAN

	END TRY
	BEGIN CATCH
		ROLLBACK TRAN
	END CATCH
END
