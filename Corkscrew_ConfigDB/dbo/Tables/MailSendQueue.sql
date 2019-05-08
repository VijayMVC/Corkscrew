CREATE TABLE [MailSendQueue]
(
	[Id]								uniqueidentifier	NOT NULL,

	[From]								nvarchar(255)		NOT NULL,
	[Recipient]							nvarchar(255)		NOT NULL,
	[InternalCopyTo]					nvarchar(255)		NULL,							/* if sent with "CC me" */

	[Subject]							nvarchar(512)		NOT NULL,
	[ContentHtml]						nvarchar(max)		NOT NULL,						/* Content is always Html */

	[WasSent]							bit					NOT NULL,

	[Created]							datetime			NOT NULL	DEFAULT(GETDATE()),
	[CreatedBy]							uniqueidentifier	NOT NULL	DEFAULT(dbo.GUIDDEFAULT()),
	[LastModified]						datetime			NOT NULL	DEFAULT(GETDATE()),
	[LastModifiedBy]					uniqueidentifier	NOT NULL	DEFAULT(dbo.GUIDDEFAULT()),

	constraint							[PK_MailSendQueue]	primary key ([Id])
)