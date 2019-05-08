CREATE PROCEDURE [MIMETypeSave]
	@FileExtension		nvarchar(24),
	@MimeType			nvarchar(255)
AS
BEGIN

	-- Dont allow more than one mimetype to get mapped to an extension. 
	IF (EXISTS (SELECT 1 FROM [MIMETypes] WITH (NOLOCK) WHERE (([FileExtension] = @FileExtension) AND ([KnownMimeType] <> @MimeType)))) 
	BEGIN
		UPDATE [MIMETypes] 
			SET 
				[KnownMimeType] = @MimeType 
		WHERE ([FileExtension] = @FileExtension)
	END
	ELSE 
		BEGIN
			IF (NOT EXISTS (SELECT 1 FROM [MIMETypes] WITH (NOLOCK) WHERE (([FileExtension] = @FileExtension) AND ([KnownMimeType] = @MimeType)))) 
			BEGIN
				INSERT INTO [MIMETypes] 
					( [FileExtension], [KnownMimeType] ) 
				VALUES ( @FileExtension, @MimeType )
			END
		END

END