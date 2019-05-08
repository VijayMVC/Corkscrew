CREATE PROCEDURE [WrkflwDefnSaveChanges]
	@Id									uniqueidentifier				,
	@Name								nvarchar(255)					,
	@Description						nvarchar(1024)					= NULL,

	@DefaultAssociationData				nvarchar(max)					= NULL,

	@allow_manual_start					bit								,
	@start_on_create					bit								,
	@start_on_modify					bit								,
	@is_enabled							bit								,

	@ModifiedBy							uniqueidentifier,
	@Modified							datetime
AS
BEGIN

	IF (NOT EXISTS (SELECT 1 FROM [WorkflowDefinitions] WITH (NOLOCK) WHERE ([Id] = @Id)))
	BEGIN
		RAISERROR ( N'A workflow of that Id does not exist or is of a different type.', 16, 1);
		RETURN -1;
	END

	IF (EXISTS (SELECT 1 FROM [WorkflowDefinitions] WITH (NOLOCK) WHERE (([Id] <> @Id) AND ([Name] = @Name)))) 
	BEGIN
		IF (EXISTS (SELECT 1 FROM [WorkflowDefinitions] WITH (NOLOCK) WHERE ([Name] = @Name)))
		BEGIN
			RAISERROR ( N'A workflow of that name already exists. Cannot change the name.', 16, 1);
			RETURN -1;
		END
	END

	IF (ISNULL(@Description, '') = '') 
	BEGIN
		SET @Description = NULL 
	END

	IF (ISNULL(@DefaultAssociationData, '') = '')
	BEGIN
		SET @DefaultAssociationData = NULL 
	END

	BEGIN TRANSACTION

	BEGIN TRY

		--- disable associations for disabled (not deleted) definitions
		UPDATE WA 
			SET 
				WA.[is_enabled] = 0, 
				WA.[prevent_new_instances] = 1,
				WA.[Modified] = @Modified, 
				WA.[ModifiedBy] = @ModifiedBy
		FROM [WorkflowAssociations] WA 
		INNER JOIN [WorkflowDefinitions] WD ON (WD.[Id] = WA.[WorkflowDefinitionId])
		WHERE (
			((WD.[is_enabled] = 1) and (@is_enabled = 0))
			AND (WD.[Id] = @Id)
		)

		--- abort the instances for the association if it was disabled
		UPDATE WI
			SET 
				WI.[CurrentState] = 2,		--- completed
				WI.[CompletedReason] = 5,	--- aborted
				WI.[ErrorMessage] = N'Workflow definition was disabled or deleted',
				WI.[Modified] = GETDATE(), 
				WI.[ModifiedBy] = dbo.SYSTEMUSERGUID() 
		FROM [WorkflowInstances] WI 
		INNER JOIN [WorkflowDefinitions] WD ON (WD.[Id] = WI.[WorkflowDefinitionId])
		WHERE (
			((WD.[is_enabled] = 1) and (@is_enabled = 0))
			AND (WD.[Id] = @Id)
		)

		UPDATE [WorkflowDefinitions] 
			SET 
				[Name] = @Name,
				[Description] = @Description, 
				[DefaultAssociationData] = @DefaultAssociationData, 
				[allow_manual_start] = @allow_manual_start,
				[start_on_create] = @start_on_create,
				[start_on_modify] = @start_on_modify,
				[is_enabled] = @is_enabled, 
				[Modified] = @Modified, 
				[ModifiedBy] = @ModifiedBy
		WHERE ([Id] = @Id) 

	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION
	END CATCH

	

	RETURN 0;

END