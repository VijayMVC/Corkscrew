CREATE PROCEDURE [PermissionsTestAccess]
	@CorkscrewResourceUri		nvarchar(max), 
	@PrincipalId				uniqueidentifier
AS
BEGIN 

	DECLARE @AccessRead			bit	= 0, 
			@AccessContribute	bit	= 0, 
			@AccessFullControl	bit	= 0,
			@AccessSpecial		bit = 0, 
			@Found				bit = 0,
			@SystemUser			uniqueidentifier = CAST('99999999-0000-0043-4f52-4b5343524557' as uniqueidentifier)

	DECLARE @emptyGuid	uniqueidentifier = dbo.GUIDDEFAULT()

	--- Our .NET layer cannot pass a NULL uid, so we send Empty Guids... Convert them to NULLs
	IF (@PrincipalId = @emptyGuid) 
		SET @PrincipalId = NULL 

	IF (@PrincipalId IN (@SystemUser, CAST('99999999-0100-0043-4f52-4b5343524557' as uniqueidentifier)))
	BEGIN
		SELECT 
			@AccessRead = 1, 
			@AccessContribute = CASE @PrincipalId WHEN @SystemUser THEN 1 ELSE 0 END, 
			@AccessFullControl = CASE @PrincipalId WHEN @SystemUser THEN 1 ELSE 0 END, 
			@AccessSpecial = 0, 
			@Found = 1
	END
	ELSE 
		BEGIN 
			--- check for direct access (prefer)
			SELECT 
					@AccessRead = [IsRead], 
					@AccessContribute = [IsContribute], 
					@AccessFullControl = [IsFullControl], 
					@AccessSpecial = [IsChildAccess], 
					@Found = 1
				FROM [Permissions] WITH (NOLOCK) 
			WHERE (([CorkscrewUri] = @CorkscrewResourceUri) AND ([PrincipalId] = @PrincipalId))
		END

	

	--- return result
	SELECT	ISNULL(@AccessRead, 0) As 'IsRead', 
			ISNULL(@AccessContribute, 0) As 'IsContribute', 
			ISNULL(@AccessFullControl, 0) As 'IsFullControl', 
			ISNULL(@AccessSpecial, 0) As 'IsChildAccess',
			@CorkscrewResourceUri As 'CorkscrewResourceUri',
			@PrincipalId As 'PrincipalId',
			@Found As 'PermissionsFound'
	

END