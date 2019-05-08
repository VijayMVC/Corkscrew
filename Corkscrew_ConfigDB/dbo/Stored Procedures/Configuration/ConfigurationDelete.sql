CREATE PROCEDURE [ConfigurationDelete]
	@Name		varchar(255)
AS
	DELETE FROM [Configuration] 
		WHERE ([Name] = @Name)
