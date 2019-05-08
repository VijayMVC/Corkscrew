CREATE PROCEDURE [PermissionsDeleteByPrincipalId]
	@PrincipalId	uniqueidentifier
AS
	DELETE FROM [Permissions] 
		WHERE ([PrincipalId] = @PrincipalId)
