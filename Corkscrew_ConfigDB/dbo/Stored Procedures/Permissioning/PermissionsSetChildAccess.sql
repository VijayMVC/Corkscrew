CREATE PROCEDURE [PermissionsSetChildAccess]
	@CorkscrewResourceUri	nvarchar(max), 
	@PrincipalId			uniqueidentifier,
	@ChildAccess			bit
AS
BEGIN 

	--- dont set child access if explicit R/C/FC is set
	--- because Object/Principal Id is a primary key, we will have only 1 row ever.

	DECLARE @ExistingChildAccess bit = null

	SELECT @ExistingChildAccess = [IsChildAccess] FROM [Permissions] WITH (NOLOCK) 
		WHERE (
				([PrincipalId] = @PrincipalId) 
			AND ([CorkscrewUri] = @CorkscrewResourceUri)
		)

	IF ((@ExistingChildAccess IS NULL) AND (@ChildAccess = 1))
	BEGIN 
		-- no row (since insert/update will always set it to 0 or 1)
		INSERT INTO [Permissions] 
		( 
			[PrincipalId], [CorkscrewUri],  
					[IsRead], [IsContribute], [IsFullControl], 
						[IsChildAccess] 
		) 
		VALUES 
		(
			@PrincipalId, @CorkscrewResourceUri, 
				0, 0, 0, 
					@ChildAccess
		)
	END
	ELSE 
		IF ((@ExistingChildAccess = 1) AND (@ChildAccess = 0))
		BEGIN

			DELETE FROM [Permissions] 
				WHERE (
						([PrincipalId] = @PrincipalId) 
					AND ([CorkscrewUri] = @CorkscrewResourceUri) 
				)

		END
	
END