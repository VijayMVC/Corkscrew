CREATE PROCEDURE [UserVerifyAPILogin]
	@APIToken		CHAR(64),
	@RemoteAddress	VARCHAR(255)
AS
BEGIN

	SELECT 
		USR.[Id], USR.[Username], USR.[DisplayName], USR.[EmailAddress], USR.[SecretHash], USR.[IsWinADUser] 
	FROM [Users] USR WITH (NOLOCK) 
		INNER JOIN [APILogins] API WITH (NOLOCK) 
			ON (API.[UserId] = USR.[Id]) 
	WHERE (
		(API.[APIToken] = @APIToken) 
		AND (API.[RemoteAddress] = @RemoteAddress) 
	)

END