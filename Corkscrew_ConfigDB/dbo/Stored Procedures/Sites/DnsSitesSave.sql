CREATE PROCEDURE [DnsSitesSave]
	@DnsName	nvarchar(255),
	@SiteId		uniqueidentifier
AS
BEGIN

	/*

		We do not allow one DNS name to be mapped to multiple sites

		But we do allow one site to have multiple dns mappings

	*/


	-- prevent multiple maps to a single DNS Name
	IF (EXISTS(SELECT 1 FROM [DnsSites] WITH (NOLOCK) WHERE (([DnsName] = @DnsName) AND ([SiteId] <> @SiteId))))
	BEGIN
		RAISERROR (
			N'DNS hostname already mapped to another Site.', 
			16,
			1
		)
	END
	ELSE 
		BEGIN 
			INSERT INTO [DnsSites] 
					( [DnsName], [SiteId] ) 
				VALUES 
					( @DnsName, @SiteId ) 
		END

END