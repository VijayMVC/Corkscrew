CREATE TABLE [WorkflowManifestItems]
(
	[Id]									uniqueidentifier				NOT NULL,
	[WorkflowDefinitionId]					uniqueidentifier				NOT NULL,
	[WorkflowManifestId]					uniqueidentifier				NOT NULL,

	[Filename]								nvarchar(255)					NOT NULL,
	[FilenameExtension]						nvarchar(255)					NOT NULL,
	[ItemType]								int								NOT NULL,
	/*
		one of:

			1 - primary assembly
			2 - dependency assembly 
			3 - source file
			4 - xaml file
			5 - config file
			6 - media resource (images, audio, video)
			7 - stylesheet (css)
			8 - data file (xml, txt, etc)
			9 - resource file (resx, resource, etc)

	*/

	[build_relative_folder]					nvarchar(1024)					NULL,
	[runtime_folder]						nvarchar(1024)					NULL,
	
	[required_for_execution]				bit								NOT NULL, 

	[ContentStream]							varbinary(max)					NOT NULL,
	[ContentSize]							AS								ISNULL(DATALENGTH([ContentStream]), 0) 
																persisted	NOT NULL,

	[Created]								datetime						NOT NULL,
	[CreatedBy]								uniqueidentifier				NOT NULL,
	[Modified]								datetime						NOT NULL,
	[ModifiedBy]							uniqueidentifier				NOT NULL,
	

	constraint								[PK_WorkflowManifestItems]		primary key ( [Id], [WorkflowDefinitionId], [WorkflowManifestId], [ItemType] )


)
