CREATE PROCEDURE [FileSystemGetByFullPath] 
	@SiteId			uniqueidentifier,
	@FullPath		nvarchar(max), 
	@UserId			uniqueidentifier
AS
BEGIN

	SET @FullPath = LOWER(@FullPath)

	UPDATE [FileSystem] 
		SET 
			[LastAccessed] = GETDATE(),
			[LastAccessedBy] = @UserId 
		WHERE (([SiteId] = @SiteId) AND (([FullPath] = @FullPath) OR ([FullPath] + '/' = @FullPath)))

	SELECT 
			[Id], [SiteId], [Filename], [FilenameExtension], [DirectoryName], [FullPath], [ContentSize], 
				[Created], [CreatedBy], [Modified], [ModifiedBy], [LastAccessed], [LastAccessedBy], 
					[is_directory], [is_readonly], [is_archive], [is_system] 
		FROM [FileSystem] WITH (NOLOCK) 
	WHERE (([SiteId] = @SiteId) AND (([FullPath] = @FullPath) OR ([FullPath] + '/' = @FullPath)))

END