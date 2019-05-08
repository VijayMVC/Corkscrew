CREATE INDEX [IDX_FileSystem]
	ON [dbo].[FileSystem]
	([Id])
	INCLUDE ([DirectoryName], [FullPath])