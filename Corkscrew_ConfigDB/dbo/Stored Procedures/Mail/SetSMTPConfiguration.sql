CREATE PROCEDURE [SetSMTPConfiguration]
	@ServerDns			nvarchar(1024),
	@ServerPort			int				= 25,
	@SSLRequired		bit				= 0,
	@Username			nvarchar(255),
	@PasswordEnc		nvarchar(1024),
	@DefaultSender		nvarchar(255),
	@CreatingUserId		uniqueidentifier
AS
BEGIN

	--- Row is inserted by deployment post script, so we only update

	UPDATE [SMTPConfiguration] 
		SET 
			[ServerDNS] = @ServerDns, 
			[ServerPort] = @ServerPort,
			[SSLRequired] = @SSLRequired,
			[Username] = @Username,
			[PasswordEncrypted] = @PasswordEnc,
			[DefaultSender] = @DefaultSender, 
			[LastModified] = GETDATE(),
			[LastModifiedBy] = @CreatingUserId 

	RETURN 0;

END