CREATE PROCEDURE [FileSystemGetDirectoryContentSize]
	@SiteId					uniqueidentifier,
	@DirectoryName			nvarchar(max), 
	@UserId					uniqueidentifier
AS
BEGIN

	UPDATE [FileSystem] 
		SET 
			[LastAccessed] = GETDATE(),
			[LastAccessedBy] = @UserId 
	WHERE (([DirectoryName] = @DirectoryName) AND ([SiteId] = @SiteId))
	
	SELECT SUM(ISNULL([ContentSize], 0)) 
		FROM [FileSystem]  
	WHERE (
		([SiteId] = @SiteId)
		AND ([DirectoryName] LIKE @DirectoryName + '%')
		AND ([is_directory] = 0)
	)

END