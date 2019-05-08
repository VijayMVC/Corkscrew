CREATE PROCEDURE [PermissionsGetAllAdministrators]
AS
	SELECT  
		[PrincipalId], 
		[CorkscrewUri]
	FROM [Permissions] WITH (NOLOCK) 
	WHERE ( 
		([IsFullControl] = 1) 
		AND (
			([CorkscrewUri] = 'corkscrew://')	/* farm url */ 
			OR (LEN([CorkscrewUri]) = 49)		/* site url, unless someone played with data manually */
		)
	)