CREATE PROCEDURE [UserSave]
	@Id				uniqueidentifier, 
	@Username		nvarchar(255),
	@SecretHash		char(64)			= null,
	@DisplayName	nvarchar(255), 
	@EmailAddress	nvarchar(512)		= null,
	@IsWinADUser	bit					= 0
AS 
BEGIN

	IF (NOT EXISTS (SELECT 1 FROM [Users] WITH (NOLOCK) WHERE (([Id] = @Id) OR ([Username] = @Username))))
	BEGIN
		INSERT INTO [Users] 
			( [Id], [Username], [SecretHash], [DisplayName], [EmailAddress], [IsWinADUser] ) 
			VALUES 
			(
				@Id,
				@Username,
				@SecretHash,
				@DisplayName,
				@EmailAddress, 
				@IsWinADUser
			)
	END
	ELSE 
		BEGIN 
			UPDATE [Users] 
				SET 
					[SecretHash] = 
						CASE ([IsWinADUser]) 
							WHEN 1 THEN '' 
							ELSE ISNULL(@SecretHash, [SecretHash]) 
						END,
					[DisplayName] = @DisplayName, 
					[EmailAddress] = @EmailAddress 
			WHERE ([Id] = @Id)
		END

END