CREATE TABLE [WorkflowDefinitions]
(
	[Id]									uniqueidentifier				NOT NULL,
	[Name]									nvarchar(255)					NOT NULL,
	[Description]							nvarchar(1024)					NULL,

	[DefaultAssociationData]				nvarchar(max)					NULL,

	[allow_manual_start]					bit								NOT NULL,
	[start_on_create]						bit								NOT NULL,
	[start_on_modify]						bit								NOT NULL,
	[is_enabled]							bit								NOT NULL,

	[allow_event_farm_created]				bit								NOT NULL	DEFAULT(0),
	[allow_event_farm_modified]				bit								NOT NULL	DEFAULT(0),
	[allow_event_farm_deleted]				bit								NOT NULL	DEFAULT(0),

	[allow_event_site_created]				bit								NOT NULL	DEFAULT(0),
	[allow_event_site_modified]				bit								NOT NULL	DEFAULT(0),
	[allow_event_site_deleted]				bit								NOT NULL	DEFAULT(0),

	[allow_event_directory_created]			bit								NOT NULL	DEFAULT(0),
	[allow_event_directory_modified]		bit								NOT NULL	DEFAULT(0),
	[allow_event_directory_deleted]			bit								NOT NULL	DEFAULT(0),

	[allow_event_file_created]				bit								NOT NULL	DEFAULT(0),
	[allow_event_file_modified]				bit								NOT NULL	DEFAULT(0),
	[allow_event_file_deleted]				bit								NOT NULL	DEFAULT(0),

	[allow_event_catch_bubbledevents]		bit								NOT NULL	DEFAULT(0),

	[all_events_switch]						as								(
																				[allow_event_farm_created] & [allow_event_farm_modified] & [allow_event_farm_deleted] &
																					[allow_event_site_created] & [allow_event_site_modified] & [allow_event_site_deleted] & 
																						[allow_event_directory_created] & [allow_event_directory_modified] & [allow_event_directory_deleted] & 
																							[allow_event_file_created] & [allow_event_file_modified] & [allow_event_file_deleted] 
																			),


	[Created]								datetime						NOT NULL,
	[CreatedBy]								uniqueidentifier				NOT NULL,
	[Modified]								datetime						NOT NULL,
	[ModifiedBy]							uniqueidentifier				NOT NULL,

	constraint								[PK_WorkflowDefinitions]		primary key ( [Id], [Name] )
)
