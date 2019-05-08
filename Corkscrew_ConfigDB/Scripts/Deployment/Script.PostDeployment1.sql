/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/


-- ------------------------------------------------------------------------------------

/* Check or set if we are a new install or a patch */

DECLARE @DeploymentIsFreshInstall	BIT				= 1
DECLARE @DeploymentInstallVersion	varchar(15)		= '1.0'

IF (EXISTS (SELECT 1 FROM [Configuration] WITH (NOLOCK) WHERE ([Name] = 'InstallVersion')))
BEGIN
	SET @DeploymentIsFreshInstall = 0
END

IF (@DeploymentIsFreshInstall = 1)
BEGIN
	INSERT INTO [Configuration] ( [Name], [Value] ) VALUES ('InstallVersion', @DeploymentInstallVersion )
END
ELSE 
	BEGIN 
		UPDATE [Configuration] SET [Value] = @DeploymentInstallVersion WHERE ([Name] = 'InstallVersion')
	END

	
-- ------------------------------------------------------------------------------------

/* Install default MIME Types */

PRINT N'Installing default MIME types...'
:r .\InstallDefaultMIMETypes.sql

-- ------------------------------------------------------------------------------------


/* Install system folders */

PRINT N'Installing system folders...'
:r .\InstallSystemFolders.sql

-- ------------------------------------------------------------------------------------

/* Create user and grant access to the account on the database */
PRINT N'Granting access to CorkscrewUser on $(DatabaseName) ...'

IF (NOT EXISTS(SELECT 1 FROM master.dbo.syslogins WHERE (loginname='_sqlServerUser')))
BEGIN

	/* This password MUST match what is provided in connection strings! */
	CREATE LOGIN [CorkscrewUser] WITH PASSWORD = 'JuS7@C0mpl1cat3dP@ssW)rd'
	
	EXEC sp_addsrvrolemember 'CorkscrewUser', 'sysadmin' 
END

IF (NOT EXISTS(SELECT 1 FROM sys.database_principals WHERE (name = 'CorkscrewUser'))) 
BEGIN
	
	CREATE USER [CorkscrewUser] FOR LOGIN [CorkscrewUser] 
		WITH DEFAULT_SCHEMA=[dbo];

	DECLARE @ServerVersion VARCHAR(128) = CONVERT(varchar(128), SERVERPROPERTY('productversion'));
	DECLARE @dotIndex INT = CHARINDEX('.', @ServerVersion, 1);
	SET @ServerVersion = SUBSTRING(@ServerVersion, 1, @dotIndex - 1);

	IF (CONVERT(INT, @ServerVersion) = 10) 
	BEGIN
		EXEC sp_addrolemember @rolename='db_owner', @membername='CorkscrewUser';
	END
	ELSE 
		BEGIN
			EXEC sp_executesql N'ALTER ROLE [db_owner] ADD MEMBER [CorkscrewUser]';
		END
END

/* Create first admin and grant access -- if ConfigDB is being deployed through script there is no way other than Control Center to create the first admin. */
PRINT N'Creating first user [CorkscrewAdmin] and granting access ...'
IF (NOT EXISTS(SELECT 1 FROM [Users] WITH (NOLOCK) WHERE ([Username]='CorkscrewAdmin'))) 
BEGIN
	DECLARE @UserId	uniqueidentifier;
	SET @UserId = NEWID();

	EXEC [UserSave] 
		@Id = @UserId,
		@Username = 'CorkscrewAdmin', 
		@SecretHash = '36672A906D204F54392FF0186A6474852473C7288C7F0299617EC49077D22015',		/* JuS7@C0mpl1cat3dP@ssW)rd */
		@DisplayName = 'Corkscrew Administrator',
		@EmailAddress = 'CorkscreAdmin@',
		@IsWinADUser = 0

	EXEC [PermissionSave] 
		@PrincipalId = @UserId,
		@CorkscrewUri = 'corkscrew://', 
		@Read = 0,
		@Contribute = 0,
		@FullControl = 1
END

-- ------------------------------------------------------------------------------------

/* Take a backup of the deployed database */
PRINT N'Backing up Config DB...'
DECLARE @BACKUP		SYSNAME = ''
DECLARE @BACKUPTIME	NVARCHAR(24) = ''

EXECUTE [master].dbo.xp_instance_regread 
		N'HKEY_LOCAL_MACHINE', 
		N'SOFTWARE\Microsoft\MSSQLServer\MSSQLServer', 
		N'BackupDirectory',
		@BACKUP OUTPUT

IF @@ERROR <> 0 
	BEGIN
		SELECT 
			@BACKUPTIME = REPLACE(REPLACE(REPLACE(REPLACE(convert(nvarchar, getdate(), 126), '-', ''), ':', ''), 'T', ''), '.', '')

		SET @BACKUP = CONCAT(@BACKUP, '\', '$(DatabaseName)', '_FreshInstall_', @BACKUPTIME, '.bak')

		BACKUP DATABASE [$(DatabaseName)] 
			TO DISK = @BACKUP
				WITH 
					FORMAT, 
					COMPRESSION,
					MEDIANAME = 'Corkscrew_ConfigDB_Backup', 
					NAME = 'Backup of Corkscrew ConfigDB'
	END



-- ------------------------------------------------------------------------------------
PRINT N'Installing ChangeGuard on ConfigDB...'
IF ((SELECT COUNT(*) FROM sys.triggers WHERE ([name] = 'ChangeGuard')) = 0)
BEGIN
	EXEC dbo.sp_executesql 
		@statement = N'CREATE TRIGGER [ChangeGuard]
	ON DATABASE
	FOR DDL_FUNCTION_EVENTS, DDL_PROCEDURE_EVENTS, DDL_TABLE_EVENTS
	AS
	BEGIN
		PRINT ''ChangeGuard is ENABLED. To disable, run DISABLE TRIGGER [ChangeGuard] ON DATABASE;''
		ROLLBACK;
		PRINT ''Attempted DDL is rolled back.''
	END'
END

PRINT N'Enabling ChangeGuard...'
IF ((SELECT COUNT(*) FROM sys.triggers WHERE ([name] = 'ChangeGuard')) = 1)
BEGIN
	ENABLE TRIGGER [ChangeGuard] ON DATABASE;
END

-- ------------------------------------------------------------------------------------


/* 
	Install DatabaseCleanup SQL JOB 

	Do this LAST because the script changes the database context to MSDB.

*/

PRINT N'Installing database cleanup job...'
:r .\InstallDatabaseCleanupJob.sql


-- ------------------------------------------------------------------------------------

PRINT N'Installation script completed.'