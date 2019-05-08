CREATE PROCEDURE [PermissionsTestAdministrator]
	@SiteId			uniqueidentifier,	--- Will be NULL if we are checking for global admin
	@PrincipalId	uniqueidentifier	--- should never be NULL, validated below
AS
BEGIN 

	DECLARE	@IsAdmin	bit							= 0, 
			@emptyGuid	uniqueidentifier			= dbo.GUIDDEFAULT(),
			@CorkscrewResourceUri	nvarchar(max)	= 'corkscrew://'

	IF (ISNULL(@PrincipalId, @emptyGuid) = @emptyGuid) 
	BEGIN
		RAISERROR (N'PrincipalId cannot be Null or Empty.', 16, 1);
		RETURN;
	END

	IF (@PrincipalId = CAST('99999999-0000-0043-4f52-4b5343524557' as uniqueidentifier))
	BEGIN
		SELECT 
			@SiteId = @emptyGuid, 
			@PrincipalId = @PrincipalId, 
			@IsAdmin = 1
	END
	ELSE 
		BEGIN
			--- change SiteId to NULL if it is set to EmptyGuid
			SET @CorkscrewResourceUri = 'corkscrew://';
			IF (ISNULL(@SiteId, @emptyGuid) <> @emptyGuid)
			BEGIN
				SET @CorkscrewResourceUri = CONCAT('corkscrew://', LOWER(CAST(@SiteId as varchar(50))), '/');
			END

			-- check for admin. Only 2 actual conditions are: 
			--	[1] IsFullControl = 1
			--  [2] ObjectId = NULL (otherwise is an Object Admin) 

			SELECT @IsAdmin = 1 
				FROM [Permissions] WITH (NOLOCK) 
					WHERE (([PrincipalId] = @PrincipalId) AND ([CorkscrewUri] = @CorkscrewResourceUri) AND ([IsFullControl] = 1))

			SET @IsAdmin = ISNULL(@IsAdmin, 0) 
		END

	SELECT @SiteId As 'SiteId', @PrincipalId As 'PrincipalId', @IsAdmin As 'IsAdmin'

END