CREATE PROCEDURE [PermissionsGetByResourceUri]
	@CorkscrewResourceUri	nvarchar(max)
AS 
	SELECT 
			[PrincipalId], [CorkscrewUri], 
				[IsRead], [IsContribute], [IsFullControl], [IsChildAccess] 
		FROM [Permissions] WITH (NOLOCK) 
	WHERE (
			([CorkscrewUri] = @CorkscrewResourceUri)
	)
