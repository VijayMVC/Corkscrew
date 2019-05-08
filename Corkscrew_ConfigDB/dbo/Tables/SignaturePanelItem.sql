CREATE TABLE [SignaturePanelItem]
(
	[Id]					uniqueidentifier			NOT NULL,
	[PanelId]				uniqueidentifier			NOT NULL,
	[RespondentId]			uniqueidentifier			NOT NULL,
	[IsFinalDecider]		bit							NOT NULL,
	[IsTieBreaker]			bit							NOT NULL,
	[IsMandatory]			bit							NOT NULL,

	[Response]				int							NOT NULL,
	[Comment]				nvarchar(1024)				NULL,
	[RespondedOn]			datetime					NULL,

	[SentToResponder]		bit							NOT NULL,

	[Created]				datetime					not null,
	[CreatedBy]				uniqueidentifier			not null,
	[Modified]				datetime					not null,
	[ModifiedBy]			uniqueidentifier			not null

	constraint				[PK_SignaturePanelItem]		primary key ( [PanelId], [Id], [RespondentId] )
)
