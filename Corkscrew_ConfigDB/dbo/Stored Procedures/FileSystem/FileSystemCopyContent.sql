CREATE PROCEDURE [FileSystemCopyContent]
	@SourceId		uniqueidentifier,
	@DestinationId	uniqueidentifier,
	@Modified		datetime,
	@ModifiedBy		uniqueidentifier
AS
BEGIN

	--- Ensure destination exists
	IF (NOT EXISTS (SELECT 1 FROM [FileSystem] WHERE ([Id] = @DestinationId)))
	BEGIN
		RAISERROR ( N'Cannot copy content. Destination does not exist.', 16, 1);
		RETURN -1;
	END

	--- Ensure source exists
	IF (NOT EXISTS (SELECT 1 FROM [FileSystem] WHERE ([Id] = @SourceId)))
	BEGIN
		RAISERROR ( N'Cannot copy content. Source does not exist.', 16, 1);
		RETURN -1;
	END


	BEGIN TRANSACTION

	BEGIN TRY

		UPDATE [FileSystem] 
		SET 
			[ContentStream] = SRC.ContentStream, 
			[Modified] = @Modified, 
			[ModifiedBy] = @ModifiedBy,
			[LastAccessed] = @Modified,
			[LastAccessedBy] = @ModifiedBy
			FROM 
		(
			SELECT [ContentStream] FROM [FileSystem] 
				WHERE ([Id] = @SourceId)
		) SRC 
		WHERE ([Id] = @DestinationId)

		UPDATE [FileSystem] 
		SET 
			[LastAccessed] = @Modified,
			[LastAccessedBy] = @ModifiedBy 
		WHERE ([Id] = @SourceId)

		COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION;
		RETURN -1;
	END CATCH

	RETURN 0;
END