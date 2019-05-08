CREATE PROCEDURE [MIMETypesGetAll]
AS
	SELECT [FileExtension], [KnownMimeType] 
		FROM [MIMETypes] WITH (NOLOCK) 
