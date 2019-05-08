CREATE PROCEDURE [QueueMail]
	@Id					uniqueidentifier,
	@From				nvarchar(255)		= NULL,
	@Recipient			nvarchar(255),
	@InternalCC			nvarchar(255),
	@Subject			nvarchar(512),
	@ContentHtml		nvarchar(max),
	@CreatingUserId		uniqueidentifier,
	@WasSent			bit					= 0
AS
BEGIN

	DECLARE @RightNow	datetime			= GETDATE()

	IF (@From IS NULL) 
	BEGIN
		SELECT @From = [DefaultSender] FROM [SMTPConfiguration] WITH (NOLOCK);
		SET @From = ISNULL(@From, 'system@corkscrewCorkscrew.com');					--- sending mail may fail if hosted on customer infra
	END

	INSERT INTO [MailSendQueue] 
		( [Id],  
			[From], [Recipient], [InternalCopyTo], [Subject], [ContentHtml], 
				[WasSent], 
					[Created], [CreatedBy], [LastModified], [LastModifiedBy] )
		VALUES 
		(
			@Id,
			@From,
			@Recipient,
			@InternalCC,
			@Subject,
			@ContentHtml,
			@WasSent,
			@RightNow,
			@CreatingUserId,
			@RightNow,
			@CreatingUserId
		);

	RETURN 0;

END