CREATE PROCEDURE [GetQueuedMailItemById]
	@MailId			uniqueidentifier
AS
BEGIN

	SELECT 
		[Id],  
			[From], [Recipient], [InternalCopyTo], [Subject], [ContentHtml], 
				[WasSent], 
					[Created], [CreatedBy], [LastModified], [LastModifiedBy]
	FROM [MailSendQueue] WITH (NOLOCK) 
	WHERE 
	([Id] = @MailId);

	RETURN @@ROWCOUNT;

END