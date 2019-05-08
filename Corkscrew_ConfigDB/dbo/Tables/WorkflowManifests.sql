CREATE TABLE [WorkflowManifests]
(
	[Id]									uniqueidentifier				NOT NULL,
	[WorkflowDefinitionId]					uniqueidentifier				NOT NULL,

	[WorkflowEngine]						char(4)							NOT NULL,
	/*
		One of:

			WF3X - Xaml workflow, Workflow 3 (System.Workflow.Activities)
			WF3C - Coded workflow, Workflow 3 (System.Workflow.Activities)
			
			WF4X - Xaml workflow, Workflow 4 (System.Activities)
			WF4C - Coded workflow, Workflow 4 (System.Activities)
			
			CS1C - Code-only workflow, Corkscrew v1 (implements Corkscrew.SDK.workflow.ICSWorkflow)

	*/

	[OutputAssemblyName]					nvarchar(255)					NOT NULL,
	[WorkflowClassName]						nvarchar(1024)					NOT NULL,

	[build_assembly_title]					nvarchar(255)					NULL,
	[build_assembly_description]			nvarchar(255)					NULL,
	[build_assembly_company]				nvarchar(255)					NULL,
	[build_assembly_product]				nvarchar(255)					NULL,
	[build_assembly_copyright]				nvarchar(255)					NULL,
	[build_assembly_trademark]				nvarchar(255)					NULL,
	[build_assembly_version]				nvarchar(16)					NULL,
	[build_assembly_fileversion]			nvarchar(16)					NULL,

	[always_compile]						bit								NOT NULL,
	[cache_compile_results]					bit								NOT NULL,

	[last_compiled_datetime]				datetime						NULL,

	[Created]								datetime						NOT NULL,
	[CreatedBy]								uniqueidentifier				NOT NULL,
	[Modified]								datetime						NOT NULL,
	[ModifiedBy]							uniqueidentifier				NOT NULL,
	

	constraint								[PK_WorkflowManifests]			primary key ( [Id], [WorkflowDefinitionId] )

)
