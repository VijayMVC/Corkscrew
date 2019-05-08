CREATE PROCEDURE [MIMETypeMatchByPartialName]
	@PartialMimeType	nvarchar(255)
AS
	SELECT [FileExtension], [KnownMimeType]  
		FROM [MIMETypes] WITH (NOLOCK) 
	WHERE ([KnownMimeType] LIKE (@PartialMimeType + '%'))
