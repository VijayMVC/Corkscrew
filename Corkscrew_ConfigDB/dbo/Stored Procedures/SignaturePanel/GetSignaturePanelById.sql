CREATE PROCEDURE [GetSignaturePanelById]
	@Id			uniqueidentifier
AS
BEGIN

	SELECT 
		[Id], [PanelType], [IsTimelimited], [Deadline], [CurrentState], [ResponsesInProgress],
			[Created], [CreatedBy], [Modified], [ModifiedBy] 
	FROM [SignaturePanel] WITH (NOLOCK) 
	WHERE (
		[Id] = @Id
	)

	RETURN @@ROWCOUNT;

END