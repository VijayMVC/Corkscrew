CREATE PROCEDURE [UserLogin]
	@Username		NVARCHAR(255),
	@PasswordHash	CHAR(64),
	@APILogin		BIT				= 0,
	@RemoteAddress	VARCHAR(255)	= NULL,
	@APIToken		CHAR(64)		= NULL
AS
BEGIN

	DECLARE @UserId	uniqueidentifier = NULL;

	IF (@APILogin = 1) 
	BEGIN
		IF ((ISNULL(@RemoteAddress, '') = '') OR (ISNULL(@APIToken, '') = '')) 
		BEGIN
			RAISERROR ( N'APILogin is set, but RemoteAddress or APIToken is not set.', 16, 1);
			RETURN;
		END

		SELECT @UserId = [Id] FROM [Users] WITH (NOLOCK) WHERE (([Username] = @Username) AND ([SecretHash] = @PasswordHash))

		IF (@UserId IS NOT NULL) 
		BEGIN
			INSERT INTO [APILogins] 
				( [Id], [UserId], [RemoteAddress], [APIToken], [Created] ) 
				VALUES 
				(
					NEWID(), 
					@UserId, 
					@RemoteAddress, 
					@APIToken, 
					GETDATE()
				);
		END
	END

	SELECT [Id], [Username], [SecretHash], [DisplayName], [EmailAddress], [IsWinADUser] 
		FROM [Users] WITH (NOLOCK) 
	WHERE (
		([Username] = @Username) 
		AND (
			(([IsWinADUser] = 0) AND ([SecretHash] = @PasswordHash)) 
			OR ([IsWinADUser] = 1)
		)
	)

END