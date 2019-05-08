CREATE PROCEDURE [GetSMTPConfiguration]
AS
BEGIN

	SELECT 
			[ServerDNS], [ServerPort], [SSLRequired], [Username], [PasswordEncrypted], [DefaultSender]
		FROM [SMTPConfiguration] WITH (NOLOCK);

	RETURN 1;

END