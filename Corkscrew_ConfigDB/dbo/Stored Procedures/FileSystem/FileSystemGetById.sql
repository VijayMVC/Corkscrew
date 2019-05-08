CREATE PROCEDURE [FileSystemGetById]
	@Id			uniqueidentifier, 
	@UserId		uniqueidentifier
AS
BEGIN

	UPDATE [FileSystem] 
		SET 
			[LastAccessed] = GETDATE(),
			[LastAccessedBy] = @UserId 
	WHERE ([Id] = @Id)

	SELECT 
			[Id], [SiteId], [Filename], [FilenameExtension], [DirectoryName], [FullPath], [ContentSize], 
				[Created], [CreatedBy], [Modified], [ModifiedBy], [LastAccessed], [LastAccessedBy], 
					[is_directory], [is_readonly], [is_archive], [is_system]
		FROM [FileSystem] WITH (NOLOCK) 
	WHERE ([Id] = @Id)

END