CREATE PROCEDURE [FileSystemGetDataById]
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
			[ContentSize], [ContentStream] 
		FROM [FileSystem] WITH (NOLOCK) 
	WHERE (([Id] = @Id) AND ([is_directory] = 0))

END