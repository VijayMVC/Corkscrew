

DECLARE @isf_systemGuid		uniqueidentifier = '99999999-0000-0043-4f52-4b5343524557',
		@isf_timeStamp		datetime	= GETDATE()
		

IF (NOT EXISTS(SELECT 1 FROM [FileSystem] WITH (NOLOCK) WHERE ([Id] = @isf_systemGuid))) 
BEGIN
	
	INSERT INTO [FileSystem] 
		( [Id], [SiteId], [Filename], [FilenameExtension], [DirectoryName],  
				[Created], [CreatedBy], [Modified], [ModifiedBy], [LastAccessed], [LastAccessedBy], 
					[is_directory], [is_readonly], [is_archive], [is_system], 
						[ContentStream] )
		VALUES 
		(
			@isf_systemGuid,	-- Id 
				dbo.GUIDDEFAULT(),	-- Site Id
				'/',			-- Filename
				NULL,			-- Extension
				NULL,				-- Parent directory (we would prefer it be NULL, but column is NOT NULL)
					@isf_timeStamp, @isf_systemGuid, @isf_timeStamp, @isf_systemGuid, @isf_timeStamp, @isf_systemGuid, 
						1,		-- directory
						1,		-- readonly
						1,		-- archive
						1,		-- system
						NULL	-- data (valid only for files)
		);

END

