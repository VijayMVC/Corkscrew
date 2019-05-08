CREATE TABLE [SMTPConfiguration]
(
	[ServerDNS]						nvarchar(1024)			NOT NULL,
	[ServerPort]					int						NOT NULL		DEFAULT(25),
	[SSLRequired]					bit						NOT NULL		DEFAULT(0),
	[Username]						nvarchar(255)			NOT NULL,
	[PasswordEncrypted]				nvarchar(1024)			NOT NULL,
	[DefaultSender]					nvarchar(255)			NOT NULL,

	[Created]						datetime				NOT NULL		DEFAULT(GETDATE()),
	[CreatedBy]						uniqueidentifier		NOT NULL		DEFAULT(dbo.GUIDDEFAULT()),
	[LastModified]					datetime				NOT NULL		DEFAULT(GETDATE()),
	[LastModifiedBy]				uniqueidentifier		NOT NULL		DEFAULT(dbo.GUIDDEFAULT())
)
