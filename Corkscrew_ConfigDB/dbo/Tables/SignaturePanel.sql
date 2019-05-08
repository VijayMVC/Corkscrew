CREATE TABLE [SignaturePanel]
(
	[Id]					uniqueidentifier			NOT NULL,
	[PanelType]				int							NOT NULL,
	[IsTimelimited]			bit							NOT NULL,
	[Deadline]				datetime					NOT NULL,

	[ResponsesInProgress]	bit							NOT NULL,
	[CurrentState]			int							NOT NULL,

	[Created]				datetime					not null,
	[CreatedBy]				uniqueidentifier			not null,
	[Modified]				datetime					not null,
	[ModifiedBy]			uniqueidentifier			not null

	constraint				[PK_SignaturePanel]			primary key ( [Id] )
)
