CREATE TABLE [Users]
(
	[Id]			uniqueidentifier		not null,
	[Username]		nvarchar(255)			not null,
	[SecretHash]	char(64)				not null,
	[DisplayName]	nvarchar(255)			not null,
	[EmailAddress]	nvarchar(512)			null,
	[IsWinADUser]	bit						not null	default(0),

	CONSTRAINT [PK_Users_Id]	PRIMARY KEY ([Id], [Username], [SecretHash])
)
