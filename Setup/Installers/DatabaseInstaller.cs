using Corkscrew.SDK.providers.database;
using Corkscrew.SDK.tools;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;

namespace CMS.Setup.Installers
{
    /// <summary>
    /// Installer class for databases
    /// </summary>
    public class DatabaseInstaller : WindowsInstaller
    {

        // order of running scripts
        private Dictionary<string, string> _databaseScriptsInstallOrder = new Dictionary<string, string>()
        {
            { "Roles", "ROLE" },
            { "Schemas", "SCHEMA" },
            { "Functions", "FUNCTION" },
            { "Tables", "TABLE" },
            { "Views", "VIEW" },
            { "Indexes", "INDEX" },
            { "Triggers", "TRIGGER" },
            { "Stored Procedures", "PROCEDURE" },
            { "Jobs", "JOB" }
        };

        private ICSDatabaseProvider _providerMasterDatabase = null;
        private ICSDatabaseProvider _providerConfigDB = null;

        #region Properties

        /// <summary>
        /// Configuration settings for one database for the installer
        /// </summary>
        public DatabaseSettings DatabaseConfiguration
        {
            get
            {
                if (_databaseConfig == null)
                {
                    _databaseConfig = DatabaseSettings.GetSQLServerDefaultSettings();
                }

                return _databaseConfig;
            }
            set
            {
                if ((value != null) && (value.ProviderType != DatabaseProviderEnum.Microsoft_SQL_Server))
                {
                    throw new ArgumentException("ProviderType is not set or is set to a database other than Microsoft SQL Server. Not implemented!");
                }

                _databaseConfig = value;
            }
        }
        private DatabaseSettings _databaseConfig = null;

        /// <summary>
        /// Top directory hosting scripts for the database operations
        /// </summary>
        public string DatabaseScriptsRootDirectory
        {
            get;
            set;
        }

        /// <summary>
        /// Folder that contain any patch scripts. This folder is expected to be FLAT without any subdirectories. 
        /// Files in this folder must be named with the same name as the original CREATE file.
        /// </summary>
        public string DatabasePatchScriptsPath
        {
            get;
            set;
        }

        /// <summary>
        /// Name of the database to be installed
        /// </summary>
        public string DatabaseName
        {
            get;
            set;
        }

        /// <summary>
        /// Identifier start quote character
        /// </summary>
        public string IdentifierQuoteCharacterStart
        {
            get
            {
                if (string.IsNullOrEmpty(_quoteStart))
                {
                    _quoteStart = DatabaseConfiguration.GetEnvironmentStartQuoteCharacter();
                }
                return _quoteStart;
            }
        }
        private string _quoteStart = null;

        /// <summary>
        /// Identifier end quote character
        /// </summary>
        public string IdentifierQuoteCharacterEnd
        {
            get
            {
                if (string.IsNullOrEmpty(_quoteEnd))
                {
                    _quoteEnd = DatabaseConfiguration.GetEnvironmentEndQuoteCharacter();
                }
                return _quoteEnd;
            }
        }
        private string _quoteEnd = null;


        /// <summary>
        /// If set, workflow elements are not installed
        /// </summary>
        public bool SkipWorkflowElements
        {
            get;
            set;
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="settings">Database configuration settings</param>
        /// <remarks>NOTE: Only SQL Server related operations have been coded. If provider is set to any other provider, an exception will be thrown!</remarks>
        public DatabaseInstaller(DatabaseSettings settings)
            : base()
        {
            if (settings != null)
            {
                if (settings.ProviderType != DatabaseProviderEnum.Microsoft_SQL_Server)
                {
                    throw new ArgumentException("Database provider is not set or is set to a database other than Microsoft SQL Server. Not implemented!");
                }

                DatabaseConfiguration = settings;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns if the target database server is the local system
        /// </summary>
        /// <returns>True if database server is local machine</returns>
        public bool IsLocalSystem()
        {
            string serverAddress = DatabaseConfiguration.DatabaseServerAddress;
            int startPos = 0, endPos = serverAddress.Length;
            bool isLocal = true;

            if (serverAddress.StartsWith("tcp:", StringComparison.InvariantCultureIgnoreCase))
            {
                serverAddress = serverAddress.Substring(4);
            }

            startPos = serverAddress.IndexOf(":");
            if (startPos > 0)
            {
                serverAddress = serverAddress.Substring(0, startPos);
            }

            startPos = serverAddress.IndexOf(",");
            if (startPos > 0)
            {
                serverAddress = serverAddress.Substring(0, startPos);
            }

            // now what we have is a hostname or IP address. Resolve it
            if ((serverAddress == ".") || (serverAddress == "(local)"))
            {
                // no doubt
                return true;
            }

            try
            {
                IPAddress[] remoteAddresses = Dns.GetHostAddresses(serverAddress);

                IPAddress[] localAddresses = Dns.GetHostAddresses(Dns.GetHostName());
                if (remoteAddresses.Any(ip => ((!IPAddress.IsLoopback(ip)) && (!localAddresses.Contains(ip)))))
                {
                    isLocal = false;
                }
            }
            catch (Exception ex)
            {
                // invalid host?
                if (ex.Message.Contains("No such host is known"))
                {
                    throw new Exception("DatabaseProvider server address could not be resolved. " + DatabaseConfiguration.DatabaseServerAddress);
                }
            }

            return isLocal;
        }

        /// <summary>
        /// Installs the database
        /// </summary>
        /// <returns>True if installation succeeded</returns>
        public override bool Install()
        {
            // reset state
            LastStatus = LastActionState.NotExecuted;

            try
            {
                _providerMasterDatabase = CSDatabaseProviderFactory.GetProviderByConnectionString(DatabaseConfiguration.GetConnectionStringForDatabase("master"));
                _providerConfigDB = CSDatabaseProviderFactory.GetProviderByConnectionString(DatabaseConfiguration.GetConnectionStringForDatabase("Corkscrew_ConfigDB"));

                if (! CreateDatabase())
                {
                    // will be caught and handled below
                    throw new Exception();
                }

                foreach (string component in _databaseScriptsInstallOrder.Keys)
                {
                    if (! RunScriptsInFolder(component))
                    {
                        // will be caught and handled below
                        throw new Exception();
                    }
                }

                RunPostDeploymentScripts();

                LastStatus = LastActionState.Installed;
            }
            catch
            {
                LastStatus = LastActionState.InstallFailed;
                return false;
            }
            finally
            {
                _providerConfigDB.Connection.Close();
                _providerMasterDatabase.Connection.Close();

                _providerConfigDB = null;
                _providerMasterDatabase = null;
            }

            return true;
        }

        /// <summary>
        /// Repairs the database
        /// </summary>
        /// <returns>True if repair succeeded</returns>
        public override bool Repair()
        {
            return Install();
        }

        /// <summary>
        /// Uninstalls the database
        /// </summary>
        public override void Uninstall()
        {
            // reset state
            LastStatus = LastActionState.NotExecuted;

            _providerMasterDatabase = CSDatabaseProviderFactory.GetProviderByConnectionString(DatabaseConfiguration.GetConnectionStringForDatabase("master"));
            _providerConfigDB = CSDatabaseProviderFactory.GetProviderByConnectionString(DatabaseConfiguration.GetConnectionStringForDatabase("Corkscrew_ConfigDB"));

            if (! DatabaseExists())
            {
                LastStatus = LastActionState.Uninstalled;
                return;
            }

            try
            {
                OnProgressChanged(1, "Dropping [" + DatabaseConfiguration.DatabaseName + "] database...");
                _providerMasterDatabase.ExecuteNonQueryStatement(string.Format("ALTER DATABASE {0} SET SINGLE_USER WITH ROLLBACK IMMEDIATE;", EnsureQuotedIdentifier(DatabaseConfiguration.DatabaseName)));    // closes existing connections
                _providerMasterDatabase.ExecuteNonQueryStatement(string.Format("DROP DATABASE {0}", EnsureQuotedIdentifier(DatabaseConfiguration.DatabaseName)));
                OnProgressChanged(0, "[Success]");

                if (IsJobSystemEnabled())
                {
                    // drop the SQL job
                    OnProgressChanged(1, "Dropping SQL Server Agent Job [DatabaseCleanupJob]...");

                    _providerMasterDatabase.Connection.Close();
                    _providerMasterDatabase = CSDatabaseProviderFactory.GetProviderByConnectionString(DatabaseConfiguration.GetConnectionStringForDatabase("msdb"));

                    _providerMasterDatabase.ExecuteStoredProcedure(
                        "sp_delete_job",
                        new Dictionary<string, object>()
                        {
                        { "@job_name", "ConfigDB_DatabaseCleanupJob" }
                        }
                    );
                    OnProgressChanged(0, "[Success]");
                }

                LastStatus = LastActionState.Uninstalled;
            }
            catch
            {
                LastStatus = LastActionState.UninstallFailed;
            }
            finally
            {
                _providerConfigDB.Connection.Close();
                _providerMasterDatabase.Connection.Close();

                _providerConfigDB = null;
                _providerMasterDatabase = null;
            }
        }

        private bool IsJobSystemEnabled()
        {
            string dbValue = _providerMasterDatabase.GetSingleValue("select [status] from sys.dm_server_services where servicename like N'SQL Server Agent%'");
            if ((! string.IsNullOrEmpty(dbValue)) && (dbValue.Length == 1))
            {
                // if return was non-NULL and was any 1-digit number then Agent is installed (it may not be running, but that is fine).
                if (! _databaseScriptsInstallOrder.ContainsKey("Jobs"))
                {
                    _databaseScriptsInstallOrder.Add("Jobs", "JOB");
                }
                return true;
            }

            if (_databaseScriptsInstallOrder.ContainsKey("Jobs"))
            {
                _databaseScriptsInstallOrder.Remove("Jobs");
            }

            return false;
        }

        private bool DatabaseExists()
        {
            List<string> existingDatabaseNames = _providerMasterDatabase.GetAllDatabases(null);
            foreach(string dbName in existingDatabaseNames)
            {
                if (dbName.Equals(DatabaseConfiguration.DatabaseName, StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        private bool CreateDatabase()
        {
            if (!DatabaseExists())
            {
                OnProgressChanged(1, "Creating database [" + DatabaseConfiguration.DatabaseName + "]... ");
                string createCommand = string.Format("CREATE DATABASE {0};", EnsureQuotedIdentifier(DatabaseConfiguration.DatabaseName));
                DatabaseActionResult result = _providerMasterDatabase.ExecuteNonQueryStatement(createCommand);
                if (result.Error)
                {
                    if ((result.RichException is SqlException) && (((SqlException)result.RichException).Number == -2))
                    {
                        // timeout. Check if DB exists
                        // this case happens with SQL Azure
                        if (!DatabaseExists())
                        {
                            OnProgressChanged(0, "[Failed]: Server took too long to create database. " + result.ErrorMessage);
                            return false;
                        }
                    }
                    else
                    {
                        OnProgressChanged(0, "[Failed]: " + result.ErrorMessage);
                        return false;
                    }
                }

                UndoCommands.Push(
                    () =>
                    {
                        _providerMasterDatabase.ExecuteNonQueryStatement(string.Format("DROP DATABASE {0};", EnsureQuotedIdentifier(DatabaseConfiguration.DatabaseName)));
                    }
                );

                OnProgressChanged(0, "[Success]");
            }
            else
            {
                OnProgressChanged(1, "Database " + DatabaseConfiguration.DatabaseName + " already exists.");

                // disable ChangeGuard trigger
                OnProgressChanged(1, "Disabling database trigger [ChangeGuard]...");
                _providerConfigDB.ExecuteNonQueryStatement("IF (OBJECTPROPERTY(OBJECT_ID('ChangeGuard'), 'IsTrigger') = 1) DISABLE TRIGGER [ChangeGuard] ON DATABASE;");
                OnProgressChanged(0, "[Success]");
            }

            return true;
        }

        private void DropDatabase()
        {
            _providerMasterDatabase.ExecuteNonQueryStatement(string.Format("DROP DATABASE {0};", EnsureQuotedIdentifier(DatabaseConfiguration.DatabaseName)));
        }

        private bool RunScriptsInFolder(string componentPathName)
        {
            string fullPathName = Path.Combine(DatabaseScriptsRootDirectory, "dbo", componentPathName);
            if (! Directory.Exists(fullPathName))
            {
                return true;    // no such component to install
            }

            string componentTypeName = _databaseScriptsInstallOrder[componentPathName];        // "tables" -> "table"

            // use AllDirectories because some category folder will contain subfolders
            foreach (string scriptFilename in Directory.GetFiles(fullPathName, "*.sql", SearchOption.AllDirectories))
            {
                string objectName = Path.GetFileNameWithoutExtension(scriptFilename);
                if (SkipWorkflowElements && (objectName.Contains("Workflow") || objectName.Contains("Wrkflw")))
                {
                    continue;
                }

                OnProgressChanged(1, "Installing " + componentTypeName + " [" + objectName + "]...");

                string scriptText = File.ReadAllText(scriptFilename);
                if (! string.IsNullOrEmpty(scriptText))
                {
                    DatabaseActionResult result = _providerConfigDB.ExecuteNonQueryStatement(scriptText);
                    if (result.Error)
                    {
                        if ((result.ErrorMessage.Contains("exists")) || (result.ErrorMessage.Contains("is already an object")))
                        {
                            OnProgressChanged(0, "[Object already exists, attempting to patch]");
                            string patchFileName = Path.Combine(Utility.SafeString(DatabasePatchScriptsPath, string.Empty), componentPathName, objectName + ".sql");
                            if ((! string.IsNullOrEmpty(DatabasePatchScriptsPath)) && (File.Exists(patchFileName)))
                            {
                                string patchScript = File.ReadAllText(patchFileName);
                                if (! string.IsNullOrEmpty(patchScript))
                                {
                                    result = _providerConfigDB.ExecuteNonQueryStatement(scriptText);
                                    if (result.Error)
                                    {
                                        OnProgressChanged(0, "[Patching failed]");
                                    }
                                    else
                                    {
                                        OnProgressChanged(0, "[Patched]");
                                    }
                                }
                            }
                            else
                            {
                                // functions, triggers and sprocs can be altered
                                if ((componentTypeName == "FUNCTION") || (componentTypeName == "TRIGGER") || (componentTypeName == "PROCEDURE"))
                                {
                                    OnProgressChanged(0, "[Object already exists, attempting to auto-patch]");
                                    scriptText = scriptText.SafeString(expectStart: "ALTER", removeAtStart: "CREATE");
                                    result = _providerConfigDB.ExecuteNonQueryStatement(scriptText);
                                    if (result.Error)
                                    {
                                        OnProgressChanged(0, "[Patching failed]");
                                    }
                                    else
                                    {
                                        OnProgressChanged(0, "[Patched]");
                                    }
                                }
                                else
                                {
                                    OnProgressChanged(0, "[Object already exists, no patch file or auto-patch found]");
                                }
                            }
                        }
                        else
                        {
                            OnProgressChanged(0, "[Failed]: " + result.ErrorMessage);
                        }
                    }
                    else
                    {
                        UndoCommands.Push(
                            () =>
                            {
                                _providerConfigDB.ExecuteNonQueryStatement(string.Format("DROP {0} {1}", componentTypeName, EnsureQuotedIdentifier(objectName)));
                            }
                        );

                        OnProgressChanged(0, "[Success]");
                    }
                }
            }

            return true;
        }

        public void RunPostDeploymentScripts()
        {
            string scriptsPath = Path.Combine(DatabaseScriptsRootDirectory, "Scripts", "Deployment");

            // though the below functions return success/failure we dont care for the return values

            InstallDatabaseCleanupJob(Path.Combine(scriptsPath, "InstallDatabaseCleanupJob.sql"));
            InstallDefaultMIMETypes(Path.Combine(scriptsPath, "InstallDefaultMIMETypes.sql"));
            InstallRootFolder(Path.Combine(scriptsPath, "InstallSystemFolders.sql"));
            InstallChangeGuardTrigger();
        }

        public bool InstallDatabaseCleanupJob(string scriptFilePath)
        {
            if (! IsJobSystemEnabled())
            {
                OnProgressChanged(1, "Skipping installation of SQL Job [DatabaseCleanupJob]... SQL Server Agent is not installed.");
                return true;
            }

            if (!File.Exists(scriptFilePath))
            {
                return true;
            }

            string scriptText = File.ReadAllText(scriptFilePath);
            if (! string.IsNullOrEmpty(scriptText))
            {
                // remove all "GO"s as our ExecNonQuery hates it.
                scriptText = string.Join(Environment.NewLine, scriptText.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList().Except(new List<string> { "GO" }));

                OnProgressChanged(1, "Installing SQL Job [DatabaseCleanupJob]...");
                DatabaseActionResult result = _providerMasterDatabase.ExecuteNonQueryStatement(scriptText);
                if (result.Error)
                {
                    OnProgressChanged(0, "[Failed] : " + result.ErrorMessage);
                    return false;
                }
                else
                {
                    UndoCommands.Push(
                        () =>
                        {
                            _providerMasterDatabase.ExecuteNonQueryStatement(
                                "DECLARE @jobId uniqueidentifier;" +
                                "SELECT @jobId = job_id FROM msdb.dbo.sysjobs WHERE (name = N'ConfigDB_DatabaseCleanupJob');" +
                                "IF (@jobId IS NOT NULL) msdb.dbo.sp_delete_job @job_id=@jobId;"
                            );
                        }
                    );

                    OnProgressChanged(0, "[Success] : Ensure SQL Server Agent is enabled and turned ON for SQL Jobs to execute.");
                }
            }

            return true;
        }

        public bool InstallDefaultMIMETypes(string scriptFilePath)
        {
            if (! File.Exists(scriptFilePath))
            {
                return true;
            }

            string scriptText = File.ReadAllText(scriptFilePath);
            if (! string.IsNullOrEmpty(scriptText))
            {
                // remove "GO"s
                scriptText = string.Join(Environment.NewLine, scriptText.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList().Except(new List<string> { "GO" }));

                // pad the script between a transaction...
                scriptText = "BEGIN TRANSACTION;" + Environment.NewLine + 
                                "BEGIN TRY " + Environment.NewLine +
                                    scriptText +
                                    ";COMMIT TRANSACTION; " + Environment.NewLine +
                                "END TRY " + Environment.NewLine +
                                "BEGIN CATCH " + Environment.NewLine +
                                    "ROLLBACK TRANSACTION; " + Environment.NewLine +
                                "END CATCH";

                OnProgressChanged(1, "Installing data [Default MIME Types]...");
                DatabaseActionResult result = _providerConfigDB.ExecuteNonQueryStatement(scriptText);
                if (result.Error)
                {
                    // no undo command -- inserts were wrapped in a transaction
                    OnProgressChanged(0, "[Failed, auto-rolledback]: " + result.ErrorMessage);
                    return false;   
                }
                else
                {
                    OnProgressChanged(0, "[Success]");
                }
            }

            return true;
        }

        public bool InstallRootFolder(string scriptFilePath)
        {
            string scriptText = null;
            DatabaseActionResult result;
            string rootFolderGuid = "99999999-0000-0043-4f52-4b5343524557";   // Guid is fixed.

            if (File.Exists(scriptFilePath))
            {
                scriptText = File.ReadAllText(scriptFilePath);

                // remove "GO"s
                scriptText = string.Join(Environment.NewLine, scriptText.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList().Except(new List<string> { "GO" }));

            }
            else
            {
                // this *has* to happen for a usable system!
                scriptText = string.Format("IF (NOT EXISTS(SELECT 1 FROM [FileSystem] WITH (NOLOCK) WHERE ([Id]='{0}'))) " + Environment.NewLine + 
                                "BEGIN " + Environment.NewLine +
                                    "INSERT INTO[FileSystem] ([Id],[SiteId],[Filename],[FilenameExtension],[DirectoryName], [Created],[CreatedBy],[Modified],[ModifiedBy],[LastAccessed],[LastAccessedBy],[is_directory],[is_readonly],[is_archive],[is_system],[ContentStream]) " + Environment.NewLine +
                                        "VALUES " + Environment.NewLine +
                                        "('{0}','00000000-0000-0000-0000-000000000000','/',NULL,NULL,'{1}','{0}','{1}','{0}','{1}','{0}',1,1,1,1,NULL);" + Environment.NewLine + 
                                "END", 
                                rootFolderGuid, 
                                DateTime.Now.ToString("MMM dd, yyyy HH:mm:ss")
                             );
            }


            OnProgressChanged(1, "Installing filesystem...");
            result = _providerConfigDB.ExecuteNonQueryStatement(scriptText);
            if (result.Error)
            {
                OnProgressChanged(0, "[Failed]: " + result.ErrorMessage);
            }
            else
            {
                OnProgressChanged(0, "[Success]");

                UndoCommands.Push(
                    () =>
                    {
                        _providerConfigDB.ExecuteNonQueryStatement(string.Format("DELETE FROM [FileSystem] WHERE ([Id]='{0}');", rootFolderGuid));
                    }
                );
            }

            return true;
        }

        public void InstallChangeGuardTrigger()
        {
            OnProgressChanged(1, "Installing ChangeGuard trigger (if it does not exist already)... ");
            DatabaseActionResult result = _providerConfigDB.ExecuteNonQueryStatement(
                "IF ((SELECT COUNT(*) FROM sys.triggers WHERE ([name] = 'ChangeGuard')) = 0) " + 
                "  BEGIN " +
                "      EXEC dbo.sp_executesql @statement = N'CREATE TRIGGER [ChangeGuard] ON DATABASE " + 
                "        FOR DDL_FUNCTION_EVENTS, DDL_PROCEDURE_EVENTS, DDL_TABLE_EVENTS " + 
                "      AS " +
                "      BEGIN " + 
                "          PRINT N''ChangeGuard is ENABLED. Attempted DDL is rolled back. To disable, run DISABLE TRIGGER [ChangeGuard] ON DATABASE;'' " + 
                "          ROLLBACK; " + 
                "      END'" + 
                " END"
            );

            if (result.Error)
            {
                OnProgressChanged(0, "[Failed]: " + result.ErrorMessage);
            }
            else
            {
                OnProgressChanged(0, "[Success]");
            }

            OnProgressChanged(1, "Enabling ChangeGuard trigger...");
            result = _providerConfigDB.ExecuteNonQueryStatement(
                        "IF (OBJECTPROPERTY(OBJECT_ID('ChangeGuard'), 'IsTrigger') = 1) " + 
                        "  BEGIN " +
                        "     ENABLE TRIGGER [ChangeGuard] ON DATABASE; " +
                        "  END"
                     );

            if (result.Error)
            {
                OnProgressChanged(0, "[Failed]: " + result.ErrorMessage);
            }
            else
            {
                OnProgressChanged(0, "[Success]");
            }
        }

        private string EnsureQuotedIdentifier(string value)
        {
            return value.SafeString(expectStart: IdentifierQuoteCharacterStart, expectEnd: IdentifierQuoteCharacterEnd);
        }

        /// <summary>
        /// Resolve all path properties in the installer
        /// </summary>
        /// <param name="environment">Key is special variable name, Value is value of that variable</param>
        /// <remarks>Variables are: 
        ///  $(InstallSource) - will be Application.StartupPath + "_layout" (absolute path)
        ///  $(InstallTargetRoot) - OperationManifest.InstallBaseDirectory (absolute path)
        ///  $(ComponentSource) - the particular path within $(InstallSource) where the files for this component are located (absolute path)
        ///  $(ComponentDestination) - ComponentInstaller.InstallFolderName (relative path)
        ///  $(ComponentName) - name of the current component from the manifest Xml
        /// </remarks>
        public override void ResolvePathProperties(Dictionary<string, string> environment)
        {
            foreach (string variable in environment.Keys)
            {
                DatabaseScriptsRootDirectory = DatabaseScriptsRootDirectory.Replace(variable, environment[variable]);
                DatabasePatchScriptsPath = DatabasePatchScriptsPath.Replace(variable, environment[variable]);
            }
        }

        #endregion

    }
}
