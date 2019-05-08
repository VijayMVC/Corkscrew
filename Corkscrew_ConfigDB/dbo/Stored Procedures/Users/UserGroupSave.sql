CREATE PROCEDURE [UserGroupSave]
	@Id				uniqueidentifier, 
	@Username		nvarchar(255),
	@DisplayName	nvarchar(255), 
	@EmailAddress	nvarchar(512)		= null,
	@IsWinADGroup	bit					= 0
AS 
BEGIN

	IF (NOT EXISTS (SELECT 1 FROM [Users] WITH (NOLOCK) WHERE (([Id] = @Id) OR ([Username] = @Username))))
	BEGIN
		INSERT INTO [UserGroups] 
			( [Id], [Username], [DisplayName], [EmailAddress], [IsWinADGroup] ) 
			VALUES 
			(
				@Id,
				@Username,
				@DisplayName,
				@EmailAddress, 
				@IsWinADGroup
			)
	END
	ELSE 
		BEGIN 
			UPDATE [UserGroups]
				SET 
					[DisplayName] = @DisplayName, 
					[EmailAddress] = @EmailAddress 
			WHERE ([Id] = @Id)
		END

END