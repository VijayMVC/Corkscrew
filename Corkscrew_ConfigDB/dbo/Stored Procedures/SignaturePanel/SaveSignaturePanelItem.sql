CREATE PROCEDURE [SaveSignaturePanelItem]
	@Id					uniqueidentifier,
	@PanelId			uniqueidentifier,
	@RespondentId		uniqueidentifier,
	@IsDecider			bit,
	@IsTieBreaker		bit,
	@IsMandatory		bit,
	@Response			int,
	@Comment			nvarchar(1024),
	@SentToResponder	bit,
	@ModifiedBy			uniqueidentifier, 
	@Modified			datetime
AS
BEGIN

	DECLARE @RightNow	datetime = GETDATE()

	IF (NOT EXISTS (SELECT 1 FROM [SignaturePanelItem] WITH (NOLOCK) WHERE ([Id] = @Id)))
	BEGIN
		INSERT INTO [SignaturePanelItem] 
			(
				[Id], [PanelId], [RespondentId], [IsFinalDecider], [IsTieBreaker], [IsMandatory], 
					[Response], [Comment], [RespondedOn], [SentToResponder],
						[Created], [CreatedBy], [Modified], [ModifiedBy]
			)
			VALUES 
			(
				@Id, @PanelId, @RespondentId, @IsDecider, @IsTieBreaker, @IsMandatory,
					@Response, 
						CASE @Response 
							WHEN 0 THEN NULL 
							ELSE @Comment 
						END,
							CASE @Response 
								WHEN 0 THEN NULL 
								ELSE @RightNow 
							END, 
								@SentToResponder,
						@Modified, @ModifiedBy, @Modified, @ModifiedBy
			)
	END
	ELSE 
		BEGIN
			UPDATE [SignaturePanelItem]
			SET 
				[Response] = @Response,
				[Comment] = CASE @Response 
								WHEN 0 THEN [Comment]
								ELSE @Comment 
							END,
				[RespondedOn] = CASE @Response 
									WHEN 0 THEN [RespondedOn]
									ELSE @RightNow 
								END,
				[SentToResponder] = @SentToResponder,
				[Modified] = @Modified,
				[ModifiedBy] = @ModifiedBy
			WHERE ([Id] = @Id)
		END

	RETURN 0;

END