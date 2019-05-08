CREATE PROCEDURE [SaveSignaturePanel]
	@Id						uniqueidentifier,
	@Type					int,
	@TimeLimited			bit,
	@Deadline				datetime,
	@State					int,
	@ResponsesInProgress	bit,
	@ModifiedBy				uniqueidentifier,
	@Modified				datetime
AS
BEGIN

	IF (NOT EXISTS (SELECT 1 FROM [SignaturePanel] WITH (NOLOCK) WHERE ([Id] = @Id)))
	BEGIN
		INSERT INTO [SignaturePanel] 
			(
				[Id], [PanelType], [IsTimelimited], [Deadline], [CurrentState], [ResponsesInProgress],
					[Created], [CreatedBy], [Modified], [ModifiedBy]
			)
			VALUES 
			(
				@Id, @Type, @TimeLimited, 
					CASE @TimeLimited 
						WHEN 1 THEN @Deadline 
						ELSE dbo.DATETIMEDEFAULT() 
					END, 
						@State, @ResponsesInProgress,
					@Modified, @ModifiedBy, @Modified, @ModifiedBy
			)
	END
	ELSE 
		BEGIN
			UPDATE [SignaturePanel] 
			SET 
				[Deadline] = 
					CASE @TimeLimited 
						WHEN 1 THEN @Deadline 
						ELSE dbo.DATETIMEDEFAULT() 
					END,
				[CurrentState] = @State, 
				[ResponsesInProgress] = @ResponsesInProgress,
				[Modified] = @Modified,
				[ModifiedBy] = @ModifiedBy
			WHERE ([Id] = @Id)
		END

	RETURN 0;

END