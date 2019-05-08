CREATE TABLE `FileSystem` (
  `Id` varchar(64) CHARACTER SET latin1 COLLATE latin1_general_ci NOT NULL,
  `SiteId` varchar(64) CHARACTER SET latin1 COLLATE latin1_general_ci DEFAULT NULL,
  `Filename` varchar(255) CHARACTER SET latin1 COLLATE latin1_general_ci NOT NULL,
  `FilenameExtension` varchar(255) CHARACTER SET latin1 COLLATE latin1_general_ci DEFAULT NULL,
  `DirectoryName` longtext CHARACTER SET latin1 COLLATE latin1_general_ci ,
  `Created` datetime NOT NULL,
  `CreatedBy` varchar(64) CHARACTER SET latin1 COLLATE latin1_general_ci NOT NULL,
  `Modified` datetime NOT NULL,
  `ModifiedBy` varchar(64) CHARACTER SET latin1 COLLATE latin1_general_ci NOT NULL,
  `LastAccessed` datetime NOT NULL,
  `LastAccessedBy` varchar(64) CHARACTER SET latin1 COLLATE latin1_general_ci NOT NULL,
  `is_directory` tinyint(1) NOT NULL,
  `is_readonly` tinyint(1) NOT NULL,
  `is_archive` tinyint(1) NOT NULL,
  `is_system` tinyint(1) NOT NULL,
  `ContentStream` longblob,
  `FullPath` longtext CHARACTER SET latin1 COLLATE latin1_general_ci GENERATED ALWAYS AS (
		lower(
			concat(
				  ifnull(
					`DirectoryName`, 
                    concat(
						'corkscrew://',
                        ifnull(
							`SiteId`,
                            '00000000-0000-0000-0000-000000000000'
						)
                    )
                  )
                , case 
					when isnull(`DirectoryName`) then '/' 
                    else 
						case 
							when (substring(`DirectoryName`, length(`DirectoryName`), 1) = '/') then '' 
                            else '/' 
						end 
				  end
				, replace(
					concat(
						ifnull(`Filename`, ''), 
                        ifnull(`FilenameExtension`, '')
                    ),
                    '/',
                    ''
                  )
            )
        )
  ) STORED,
  `ContentSize` bigint(20) GENERATED ALWAYS AS (length(`ContentStream`)) STORED,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Id` (`Id`),
  KEY `IDX_FileSystem` (`Id`,`SiteId`,`DirectoryName`(255),`FullPath`(255))
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_general_ci;
