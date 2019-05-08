CREATE PROCEDURE [GetTypeFromEntityRegistry]
	@EntityId		uniqueidentifier
AS
BEGIN

	SELECT [EntityId], [EntityClass] 
		FROM [EntityRegistry] WITH (NOLOCK) 
	WHERE (
		[EntityId] = @EntityId
	)

END