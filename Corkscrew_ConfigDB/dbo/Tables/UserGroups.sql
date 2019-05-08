CREATE TABLE [UserGroups]
(
	[Id]			uniqueidentifier		not null,
	[Username]		nvarchar(255)			not null,
	[DisplayName]	nvarchar(255)			not null,
	[EmailAddress]	nvarchar(512)			null,
	[IsWinADGroup]	bit						not null	default(0),

	CONSTRAINT [PK_UserGroups_Id]	PRIMARY KEY ([Id], [Username])
)
