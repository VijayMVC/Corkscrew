CREATE PROCEDURE [UpdateMailTry]
	@MailId			uniqueidentifier,
	@AttemptResult	bit
AS
BEGIN

	UPDATE [MailSendQueue] 
		SET 
			[WasSent] = @AttemptResult,
			[LastModified] = GETDATE(),
			[LastModifiedBy] = dbo.SYSTEMUSERGUID()			/* always SYSTEM */
	WHERE ([Id] = @MailId)

END