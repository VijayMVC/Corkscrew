CREATE PROCEDURE [FileSystemUpdateDirectoryName]
	@SiteId				uniqueidentifier,
	@OldDirectoryName	nvarchar(max),
	@NewDirectoryName	nvarchar(max),
	@Modified			datetime,
	@ModifiedBy			uniqueidentifier
AS
BEGIN

	DECLARE @DNWhereString nvarchar(max) = @OldDirectoryName

	--- Ensure destination exists
	IF (NOT EXISTS (SELECT 1 FROM [FileSystem] WHERE (([DirectoryName] = @NewDirectoryName) AND ([SiteId] = @SiteId))))
	BEGIN
		RAISERROR ( N'Cannot move items. Destination does not exist.', 16, 1);
		RETURN -1;
	END

	
	IF (RIGHT(@DNWhereString, 1) <> '/')
	BEGIN
		SET @DNWhereString = CONCAT(@DNWhereString, '/')
	END
	SET @DNWhereString = CONCAT(@DNWhereString, '%')


	IF ((RIGHT(@OldDirectoryName, 1) = '/') AND (RIGHT(@NewDirectoryName, 1) <> '/'))
	BEGIN
		SET @NewDirectoryName = CONCAT(@NewDirectoryName, '/')
	END
	ELSE IF ((RIGHT(@OldDirectoryName, 1) <> '/') AND (RIGHT(@NewDirectoryName, 1) = '/'))
		BEGIN
			SET @NewDirectoryName = LEFT(@NewDirectoryName, (LEN(@NewDirectoryName) - 1))
		END


	UPDATE [FileSystem] 
		SET 
			[DirectoryName] = REPLACE([DirectoryName], @OldDirectoryName, @NewDirectoryName),
			[Modified] = @Modified,
			[ModifiedBy] = @ModifiedBy, 
			[LastAccessed] = @Modified,
			[LastAccessedBy] = @ModifiedBy
	WHERE (([DirectoryName] LIKE @DNWhereString) AND ([SiteId] = @SiteId))

END