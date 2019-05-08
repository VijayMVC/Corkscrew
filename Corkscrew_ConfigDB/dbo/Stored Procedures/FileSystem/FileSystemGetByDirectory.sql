CREATE PROCEDURE [FileSystemGetByDirectory]
	@SiteId				uniqueidentifier,
	@DirectoryName		nvarchar(max), 
	@UserId				uniqueidentifier, 
	@ReturnFolders		bit,
	@ReturnFiles		bit
AS
BEGIN

	UPDATE [FileSystem] 
		SET 
			[LastAccessed] = GETDATE(),
			[LastAccessedBy] = @UserId 
	WHERE (([DirectoryName] = @DirectoryName) AND ([SiteId] = @SiteId))

	SELECT 
			[Id], [SiteId], [Filename], [FilenameExtension], [DirectoryName], [FullPath], [ContentSize], 
				[Created], [CreatedBy], [Modified], [ModifiedBy], [LastAccessed], [LastAccessedBy], 
					[is_directory], [is_readonly], [is_archive], [is_system]  
		FROM [FileSystem] WITH (NOLOCK) 
	WHERE (
		([DirectoryName] = @DirectoryName) 
		AND ([SiteId] = @SiteId) 
		AND (
				((@ReturnFiles = 1) AND ([is_directory] = 0)) 
				OR ((@ReturnFolders = 1) AND ([is_directory] = 1))
		) 
	)

END
