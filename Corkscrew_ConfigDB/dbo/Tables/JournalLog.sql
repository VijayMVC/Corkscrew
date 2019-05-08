CREATE TABLE [JournalLog]
(
	[Id]						bigint				not null				IDENTITY(1, 1),
	[MachineName]				nvarchar(255)		not null,

	/*
		Potential values for LogType :-

		1 - Information
		2 - Error
		4 - Trace

	*/
	[LogType]					tinyint				not null,
	[Timestamp]					datetime			not null,

	[Message]					nvarchar(1024)		not null,

	/* 
		
		Items without a CorrelationId are system events, those with them are Request-related. 
		NOTE: Not implemented at this time (this NOTE will go away when it is)

	*/
	[CorrelationId]				uniqueidentifier	null,

	[SiteId]					uniqueidentifier	null,
	[FileSystemEntryId]			uniqueidentifier	null,
	[UserId]					uniqueidentifier	null,

	[ModuleName]				nvarchar(255)		null,
	[StateInfo]					nvarchar(4000)		null,
	[ExceptionStack]			nvarchar(4000)		null,

	[EventClass]				nvarchar(255)		null,
	[EventType]					nvarchar(255)		null,
	[EventId]					int					null,
	
	CONSTRAINT					[PK_JournalLog]		PRIMARY KEY				( [Id] )
)
