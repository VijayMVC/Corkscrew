CREATE TABLE [UserGroupMembers]
(
	[UserId]			uniqueidentifier		not null,
	[GroupId]			uniqueidentifier		not null,
	[IsWinADMembership]	bit						not null	default(0),

	CONSTRAINT [PK_UserGroupMembers]	PRIMARY KEY ([UserId], [GroupId])
)
