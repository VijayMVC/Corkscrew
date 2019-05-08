CREATE PROCEDURE [PermissionsGetByPrincipalId]
	@PrincipalId	uniqueidentifier
AS
	SELECT 
			[PrincipalId], [CorkscrewUri], 
				[IsRead], [IsContribute], [IsFullControl], [IsChildAccess] 
		FROM [Permissions] WITH (NOLOCK) 
	WHERE ([PrincipalId] = @PrincipalId)
