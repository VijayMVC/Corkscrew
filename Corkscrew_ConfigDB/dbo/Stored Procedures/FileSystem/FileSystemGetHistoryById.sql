﻿CREATE PROCEDURE [FileSystemGetHistoryById]
	@Id		uniqueidentifier
AS
	SELECT 
		[Id], [FileSystemId], [SiteId], [IsProcessed], [ChangeType], [ChangeTimeStamp], [ChangedBy], 
				[PreviousData], [PreviousFilename], [PreviousFilenameExtension], [PreviousDirectoryName],  
					[PreviousCreated], [PreviousCreatedBy], [PreviousModified], [PreviousModifiedBy],
						[was_directory], [was_readonly], [was_archive], [was_system], ISNULL(DATALENGTH([PreviousData]), 0) As 'PreviousDataSize'
	FROM [FileSystemChangeLog] WITH (NOLOCK) 
	WHERE ([Id] = @Id)
