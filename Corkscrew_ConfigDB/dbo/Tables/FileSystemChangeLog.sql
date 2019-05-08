CREATE TABLE [FileSystemChangeLog]
(
	[Id]					UNIQUEIDENTIFIER			NOT NULL,
	[FileSystemId]			UNIQUEIDENTIFIER			NOT NULL,
	
	[SiteId]				UNIQUEIDENTIFIER			NULL,
	[IsProcessed]			BIT							NOT NULL		DEFAULT(0),

	[ChangeType]			CHAR(2)						NOT NULL,
		/*
			
			[0] = { 'I' -> Insert, 'U' -> Update, 'D' -> Delete }
			[1] = { only for 'U' -> 'P' : Property Update, 'C' : Content Update }

		*/

	[ChangeTimeStamp]		DATETIME					NOT NULL,
	[ChangedBy]				UNIQUEIDENTIFIER			NOT NULL,

	
	
	[PreviousData]			VARBINARY(MAX)				NULL,
		/* 
		This will contain the entire previous contents of the file if the content was modified, else NULL 
		i.e., will be populated only if ChangeType = 'UC'
		*/

	
	/*

		These properties will contain the previous values for respective FileSystem columns.
		Populated only if ChangeType = 'UP'
		Unchanged properties will be NULL

	*/
	[PreviousFilename]			nvarchar(255)			null, 
	[PreviousFilenameExtension]	nvarchar(255)			null,
	[PreviousDirectoryName]		nvarchar(max)			null,
	[PreviousCreated]			datetime				null,
	[PreviousCreatedBy]			uniqueidentifier		null,
	[PreviousModified]			datetime				null,
	[PreviousModifiedBy]		uniqueidentifier		null,

	[was_directory]				bit						null, 
	[was_readonly]				bit						null, 
	[was_archive]				bit						null, 
	[was_system]				bit						null,

	CONSTRAINT				[PK_FileSystemChangeLog]	PRIMARY KEY		([Id])
)
