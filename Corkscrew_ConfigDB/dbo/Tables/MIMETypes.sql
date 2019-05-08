CREATE TABLE [MIMETypes]
(
	[Id]					uniqueidentifier		not null		DEFAULT	NEWSEQUENTIALID(), 
	[FileExtension]			nvarchar(24)			not null, 
	[KnownMimeType]			nvarchar(255)			not null		DEFAULT	'application/octet-stream',

	constraint				[PK_ContentTypes_Id]	primary key		([Id], [FileExtension])
)
