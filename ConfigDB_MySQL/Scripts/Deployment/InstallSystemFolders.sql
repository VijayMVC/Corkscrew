INSERT INTO `FileSystem` 
	( `Id`, `SiteId`, `Filename`, `FilenameExtension`, `DirectoryName`,  
			`Created`, `CreatedBy`, `Modified`, `ModifiedBy`, `LastAccessed`, `LastAccessedBy`, 
				`is_directory`, `is_readonly`, `is_archive`, `is_system`,
					`ContentStream` )
	SELECT * FROM (
		select  
			'99999999-0000-0043-4f52-4b5343524557' as `Id`,
			GUIDDEFAULT() as `SiteId`,
			'/' as `Filename`,
			NULL as `FilenameExtension`,
			NULL as `DirectoryName`,
			NOW() as `Created`, 
            '99999999-0000-0043-4f52-4b5343524557' as `CreatedBy`, 
            NOW() as `Modified`, 
            '99999999-0000-0043-4f52-4b5343524557' as `ModifiedBy`, 
            NOW() as `LastAccessed`, 
            '99999999-0000-0043-4f52-4b5343524557' as `LastAccessedBy`, 
			1 as `is_directory`,
			1 as `is_readonly`,
			1 as `is_archive`,
			1 as `is_system`,
			NULL as `ContentStream`
	) AS TMP
	WHERE NOT EXISTS (
		SELECT `Id` FROM `FileSystem` WHERE (`Id` = '99999999-0000-0043-4f52-4b5343524557')
	);

