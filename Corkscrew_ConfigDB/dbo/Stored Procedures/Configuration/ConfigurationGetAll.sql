CREATE PROCEDURE [ConfigurationGetAll]
AS
	SELECT [Name], [Value] FROM [Configuration] WITH (NOLOCK) 
