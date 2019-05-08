CREATE PROCEDURE [MIMETypeGetForExtension]
	@FileExtension		nvarchar(24)
AS
	SELECT [FileExtension], [KnownMimeType] 
		FROM [MIMETypes] WITH (NOLOCK) 
	WHERE ([FileExtension] = @FileExtension) 