CREATE TABLE [APILogins]
(
	[Id]				uniqueidentifier		NOT NULL,
	[UserId]			uniqueidentifier		NOT NULL,
	[RemoteAddress]		varchar(255)			NOT NULL,
	[APIToken]			char(64)				NOT NULL,

	[Created]			datetime				NOT NULL,


	constraint			[PK_APILogins]			primary key ([Id])
)
