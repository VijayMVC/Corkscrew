CREATE PROCEDURE [GetQueuedMail] 
	@Count		int		= 25		/* get 25 emails at a time */
AS
BEGIN

	/*
		This sproc is used by the mail sending handler to pull email to send.

	*/

	/* we're using WITH TIES to take care of rows with the same Created timestamp */
	SELECT TOP(@Count) WITH TIES
		[Id], [From], [Recipient], [InternalCopyTo], [Subject], [ContentHtml] 
	FROM [MailSendQueue] WITH (NOLOCK) 
		WHERE ([WasSent] = 0) 
			ORDER BY [Created] ASC;

	RETURN @@ROWCOUNT;
	
END