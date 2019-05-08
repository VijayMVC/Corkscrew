CREATE TABLE [FileSystem]
(
	[Id]					uniqueidentifier			rowguidcol			not null,
	
	[SiteId]				uniqueidentifier								not null,
	
	[Filename]				nvarchar(255)									not null, 
	[FilenameExtension]		nvarchar(255)									null,
	[DirectoryName]			nvarchar(max)									null,

	[Created]				datetime										not null, 
	[CreatedBy]				uniqueidentifier								not null, 
	[Modified]				datetime										not null, 
	[ModifiedBy]			uniqueidentifier								not null, 
	[LastAccessed]			datetime										not null,
	[LastAccessedBy]		uniqueidentifier								not null,

	[is_directory]			bit												not null, 
	[is_readonly]			bit												not null, 
	[is_archive]			bit												not null, 
	[is_system]				bit												not null,
	
	[ContentStream]			varbinary(max)									null, 

	[FullPath]				AS		LOWER(CONCAT(
											ISNULL(
												[DirectoryName]
												, CONCAT(
													'corkscrew://'
													, CAST(
														ISNULL(
															[SiteId]
															, CAST('00000000-0000-0000-0000-000000000000' as uniqueidentifier)
														) 
														as nvarchar(max)
													)
												)
											)
											, CASE [DirectoryName] 
												WHEN NULL THEN '/' 
												ELSE 
													CASE SUBSTRING([DirectoryName], LEN([DirectoryName]), 1) 
														WHEN '/' THEN '' 
														ELSE '/' 
													END 
											END
											, REPLACE(
												CONCAT(
													COALESCE([Filename], '')
													, COALESCE([FilenameExtension], '')
												)
												, '/', '')
											))	
														persisted			not null,

	[ContentSize]			AS		ISNULL(DATALENGTH([ContentStream]), 0)
														persisted			not null,

	constraint				[PK_FileSystem_Id]			primary key			([Id])
)

