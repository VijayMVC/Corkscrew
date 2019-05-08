CREATE PROCEDURE [ConfigurationGetByName]
	@Name		varchar(255)
AS
	SELECT [Name], [Value] FROM [Configuration] WITH (NOLOCK) 
		WHERE ([Name] = @Name)
