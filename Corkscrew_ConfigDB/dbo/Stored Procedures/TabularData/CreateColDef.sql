CREATE PROCEDURE [CreateColDef]
	@Id				uniqueidentifier,
	@SiteId			uniqueidentifier,
	@Name			nvarchar(255),
    @Type			nvarchar(255),
    @MaxLength		int,
    @AllowNull		bit,
	@Created		datetime,
	@CreatedBy		uniqueidentifier
AS
BEGIN

	IF (EXISTS (SELECT 1 FROM [TabularDataColDefs] WITH (NOLOCK) WHERE (([Id] = @Id) OR (UPPER([Name]) = UPPER(@Name))))) 
	BEGIN
		RAISERROR ( N'Column name or Id already exists.', 16, 1);
		RETURN -1;
	END

	IF (@MaxLength < 0) SET @MaxLength = -1;

	INSERT INTO [TabularDataColDefs] 
		( [Id], [SiteId], [Name], [Type], [MaxLength], [Nullable], [Created], [CreatedBy] ) 
		VALUES 
		(
			@Id, @SiteId, @Name, @Type, @MaxLength, @AllowNull, @Created, @CreatedBy
		);

END