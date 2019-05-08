CREATE PROCEDURE [DeleteSignaturePanelItemById]
	@Id		uniqueidentifier
AS
BEGIN

	DELETE FROM [SignaturePanelItem] 
		WHERE (
			[Id] = @Id
		)

	RETURN 0;

END