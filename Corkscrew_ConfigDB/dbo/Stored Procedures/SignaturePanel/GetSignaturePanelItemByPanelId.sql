CREATE PROCEDURE [GetSignaturePanelItemByPanelId]
	@PanelId		uniqueidentifier
AS
BEGIN

	SELECT 
		[Id], [PanelId], [RespondentId], [IsFinalDecider], [IsTieBreaker], [IsMandatory], 
			[Response], [Comment], [RespondedOn], [SentToResponder],
				[Created], [CreatedBy], [Modified], [ModifiedBy] 
	FROM [SignaturePanelItem] WITH (NOLOCK) 
	WHERE (
		[PanelId] = @PanelId
	)

	RETURN @@ROWCOUNT;

END