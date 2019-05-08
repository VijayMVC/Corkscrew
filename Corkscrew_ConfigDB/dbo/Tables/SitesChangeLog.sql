CREATE TABLE [SitesChangeLog]
(
	[Id]					bigint						not null	identity(1, 1),
	[SiteId]				uniqueidentifier			not null, 

	[IsLocked]				BIT							NOT NULL		DEFAULT(0),
	[IsProcessed]			BIT							NOT NULL		DEFAULT(0),

	[ChangeType]			CHAR(1)						NOT NULL,
		/*
			
			{ 'I' -> Insert, 'U' -> Update, 'D' -> Delete }

		*/

	[ChangeTimeStamp]		DATETIME					NOT NULL,
	[ChangedBy]				UNIQUEIDENTIFIER			NOT NULL,

    /* Previous values */
	[PrevName]				nvarchar(255)				null,
	[PrevDescription]		nvarchar(512)				null,
	[PrevDBServer]			nvarchar(255)				null,
	[PrevDBName]			nvarchar(255)				null,
	[PrevQuotaBytes]		bigint						null,
	[PrevModified]			datetime					null,
	[PrevModifiedBy]		uniqueidentifier			null,

	CONSTRAINT [PK_SitesChangeLog_Id] PRIMARY KEY ([Id])
)
