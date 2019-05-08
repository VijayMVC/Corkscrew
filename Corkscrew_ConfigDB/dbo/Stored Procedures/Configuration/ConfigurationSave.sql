CREATE PROCEDURE [ConfigurationSave]
	@Name		varchar(255),
	@Value		varchar(4000) = null
AS
BEGIN	
	
	IF (NOT EXISTS (SELECT 1 FROM [Configuration] WHERE ([Name] = @Name)))
	BEGIN
		INSERT INTO [Configuration] ( [Name], [Value] ) 
			VALUES (@Name, @Value) 
	END
	ELSE 
		BEGIN
			UPDATE [Configuration] 
				SET 
					[Value] = @Value 
			WHERE ([Name] = @Name)
		END

END