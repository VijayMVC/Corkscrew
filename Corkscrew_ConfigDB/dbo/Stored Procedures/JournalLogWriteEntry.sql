CREATE PROCEDURE [JournalLogWriteEntry]
	@MachineName			nvarchar(255),
	@LogType				tinyint				= 1,			-- default to Information
	@Timestamp				datetime			= NULL,			-- will be set to GETDATE()
	@Message				nvarchar(1024),
	@CorrelationId			uniqueidentifier	= NULL,
	@SiteId					uniqueidentifier	= NULL,
	@FileSystemEntryId		uniqueidentifier	= NULL,
	@UserId					uniqueidentifier	= NULL,
	@ModuleName				nvarchar(255)		= NULL,
	@StateInfo				nvarchar(4000)		= NULL,
	@ExceptionStack			nvarchar(4000)		= NULL,
	@EventClass				nvarchar(255)		= NULL,
	@EventType				nvarchar(255)		= NULL,
	@EventId				int					= 0				-- value of "0" lets it conform to the WEL (Windows Event Log) convention)
AS
BEGIN

	--- Validations
	IF (@LogType NOT IN (1, 2, 4, 5, 6))
	BEGIN
		SET @LogType = 1
	END

	SET @Timestamp = ISNULL(@Timestamp, GETDATE())

	IF (@EventId < 0)
	BEGIN 
		SET @EventId = 0
	END

	DECLARE @emptyGuid	uniqueidentifier = dbo.GUIDDEFAULT()
	
	IF (@CorrelationId = @emptyGuid)
	BEGIN
		SET @CorrelationId = NULL
	END

	IF (@SiteId = @emptyGuid)
	BEGIN
		SET @SiteId = NULL
	END

	IF (@FileSystemEntryId = @emptyGuid)
	BEGIN
		SET @FileSystemEntryId = NULL
	END

	IF (@UserId = @emptyGuid)
	BEGIN
		SET @UserId = NULL
	END


	--- Now insert
	INSERT INTO [JournalLog] 
		(	[MachineName], [LogType], [Timestamp], 
				[Message],
					[CorrelationId],
						[SiteId], [FileSystemEntryId], [UserId],
							[ModuleName], [StateInfo], [ExceptionStack],
								[EventClass], [EventType], [EventId]	)
	VALUES
	(
		@MachineName, @LogType, @Timestamp, 
				@Message,
					@CorrelationId,
						@SiteId, @FileSystemEntryId, @UserId,
							@ModuleName, @StateInfo, @ExceptionStack,
								@EventClass, @EventType, @EventId
	)
	
END