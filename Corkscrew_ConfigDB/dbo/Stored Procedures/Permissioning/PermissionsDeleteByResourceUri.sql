CREATE PROCEDURE [PermissionsDeleteByResourceUri]
	@CorkscrewResourceUri	nvarchar(max)
AS
BEGIN
	DECLARE @SubTreeUri nvarchar(max)

	SET @SubTreeUri = @CorkscrewResourceUri;
	IF (SUBSTRING(@SubTreeUri, (LEN(@SubTreeUri)-1), 1) <> '/') 
	BEGIN
		SET @SubTreeUri = CONCAT(@SubTreeUri, '/')
	END

	DELETE FROM [Permissions] 
		WHERE (([CorkscrewUri] = @CorkscrewResourceUri) AND ([CorkscrewUri] LIKE @SubTreeUri))
END
