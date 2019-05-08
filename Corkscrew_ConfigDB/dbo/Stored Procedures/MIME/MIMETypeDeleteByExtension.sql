CREATE PROCEDURE [MIMETypeDeleteByExtension]
	@FileExtension		nvarchar(24)
AS
	DELETE FROM [MIMETypes] 
		WHERE ([FileExtension] = @FileExtension) 
