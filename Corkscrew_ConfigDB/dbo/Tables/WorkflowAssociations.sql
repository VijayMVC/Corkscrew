CREATE TABLE [WorkflowAssociations]
(
	[Id]											uniqueidentifier				NOT NULL,
	[WorkflowDefinitionId]							uniqueidentifier				NOT NULL,
	[Name]											nvarchar(255)					NOT NULL,
	
	[AssociationData]								nvarchar(max)					NULL,

	[FarmId]										uniqueidentifier				NOT NULL,
	[SiteId]										uniqueidentifier				NULL,
	[AssociatedContainerId]							uniqueidentifier				NOT NULL,

	[allow_manual_start]							bit								NOT NULL,
	[start_on_create]								bit								NOT NULL,
	[start_on_modify]								bit								NOT NULL,

	[prevent_new_instances]							bit								NOT NULL,
	[is_enabled]									bit								NOT NULL,

	[subscribe_event_farm_created]					bit								NOT NULL	DEFAULT(0),
	[subscribe_event_farm_modified]					bit								NOT NULL	DEFAULT(0),
	[subscribe_event_farm_deleted]					bit								NOT NULL	DEFAULT(0),

	[subscribe_event_site_created]					bit								NOT NULL	DEFAULT(0),
	[subscribe_event_site_modified]					bit								NOT NULL	DEFAULT(0),
	[subscribe_event_site_deleted]					bit								NOT NULL	DEFAULT(0),

	[subscribe_event_directory_created]				bit								NOT NULL	DEFAULT(0),
	[subscribe_event_directory_modified]			bit								NOT NULL	DEFAULT(0),
	[subscribe_event_directory_deleted]				bit								NOT NULL	DEFAULT(0),

	[subscribe_event_file_created]					bit								NOT NULL	DEFAULT(0),
	[subscribe_event_file_modified]					bit								NOT NULL	DEFAULT(0),
	[subscribe_event_file_deleted]					bit								NOT NULL	DEFAULT(0),

	[subscribe_event_catch_bubbledevents]			bit								NOT NULL	DEFAULT(0),

	[Created]										datetime						NOT NULL,
	[CreatedBy]										uniqueidentifier				NOT NULL,
	[Modified]										datetime						NOT NULL,
	[ModifiedBy]									uniqueidentifier				NOT NULL, 

	[is_farm_scope]									as								(
																						CASE 
																							WHEN ([SiteId] IS NULL) THEN 1 
																							ELSE 0
																						END 
																					),

	[is_site_scope]									as								(
																						CASE 
																							WHEN (([AssociatedContainerId] IS NOT NULL) AND ([AssociatedContainerId] = [SiteId])) THEN 1 
																							ELSE 0
																						END 
																					),

	[is_directory_scope]							as								(
																						CASE 
																							WHEN ([AssociatedContainerId] IS NOT NULL) THEN 1 
																							ELSE 0
																						END 
																					),

	constraint										[PK_WorkflowAssociations]		primary key ( [Id], [Name], [WorkflowDefinitionId], [is_enabled] )
)
