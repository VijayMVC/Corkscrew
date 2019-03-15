using System;

namespace CMS.Setup.Installers
{
    /// <summary>
    /// Provides settings for a single database connection
    /// </summary>
    public class DatabaseSettings
    {

        #region Properties

        /// <summary>
        /// Type of database
        /// </summary>
        public DatabaseProviderEnum ProviderType
        {
            get;
            set;
        } = DatabaseProviderEnum.NotSelected;

        /// <summary>
        /// Server hostname or IP address
        /// </summary>
        public string DatabaseServerAddress
        {
            get;
            set;
        } = ".";

        /// <summary>
        /// Database server port, if different from default port for the provider
        /// </summary>
        public int DatabaseServerPort
        {
            get;
            set;
        } = 0;      // interpreted as "default"

        /// <summary>
        /// Name of the database to connect to
        /// </summary>
        public string DatabaseName
        {
            get;
            set;
        } = string.Empty;

        /// <summary>
        /// If set, connection will use integrated authentication. Otherwise a username/password login is used.
        /// </summary>
        public bool IsUsingWindowsIntegratedAuthentication
        {
            get;
            set;
        } = false;

        /// <summary>
        /// Username for a database user login
        /// </summary>
        public string Username
        {
            get;
            set;
        } = "sa";

        /// <summary>
        /// Password for database user login
        /// </summary>
        public string Password
        {
            get;
            set;
        } = string.Empty;

        /// <summary>
        /// Any other options to append to the connection string
        /// </summary>
        public string ExtraOptions
        {
            get;
            set;
        } = string.Empty;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor, blank
        /// </summary>
        public DatabaseSettings() { }

        /// <summary>
        /// Gets DatabaseSettings instance populate for default access to a SQL Server
        /// </summary>
        /// <returns>DatabaseSettings object populated</returns>
        public static DatabaseSettings GetSQLServerDefaultSettings()
        {
            return new DatabaseSettings()
            {
                ProviderType = DatabaseProviderEnum.Microsoft_SQL_Server,
                DatabaseServerAddress = "(local)",
                DatabaseServerPort = 1433,
                DatabaseName = "Corkscrew_ConfigDB",
                IsUsingWindowsIntegratedAuthentication = true,
                Username = string.Empty,
                Password = string.Empty
            };
        }

        /// <summary>
        /// Gets DatabaseSettings instance populate for default access to a My SQL Server
        /// </summary>
        /// <param name="username">Username to set</param>
        /// <param name="password">Password to set</param>
        /// <returns>DatabaseSettings object populated</returns>
        public static DatabaseSettings GetMySQLServerDefaultSettings(string username = "root", string password = "p@ssw0rd!")
        {
            return new DatabaseSettings()
            {
                ProviderType = DatabaseProviderEnum.MySQL_OR_MariaDB_Server,
                DatabaseServerAddress = "localhost",
                DatabaseServerPort = 3306,
                DatabaseName = "Corkscrew_ConfigDB",
                IsUsingWindowsIntegratedAuthentication = false,
                Username = username,
                Password = password
            };
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns if the port is the default port for the database engine
        /// </summary>
        /// <returns>True or false</returns>
        public bool IsDefaultPortForProvider()
        {
            return (
                ((ProviderType == DatabaseProviderEnum.Microsoft_SQL_Server) && (DatabaseServerPort == 1433)) 
                || ((ProviderType == DatabaseProviderEnum.MySQL_OR_MariaDB_Server) && (DatabaseServerPort == 3306))
            );
        }

        /// <summary>
        /// Returns the string for the Datasource element in the connection string
        /// </summary>
        /// <returns>String containing the correct datasource value</returns>
        public string GetDataSourceString()
        {
            if (ProviderType == DatabaseProviderEnum.NotSelected)
            {
                throw new ArgumentException("ProviderType is not set");
            }

            if (string.IsNullOrEmpty(DatabaseServerAddress))
            {
                throw new ArgumentException("DatabaseServerAddress is not set");
            }

            return (
                (ProviderType == DatabaseProviderEnum.Microsoft_SQL_Server) ? 
                    DatabaseServerAddress + ((! IsDefaultPortForProvider()) ? DatabaseServerPort.ToString() : "") 
                    : DatabaseServerAddress + ";Port=" + DatabaseServerPort.ToString()
            );
        }

        /// <summary>
        /// Returns the connection string to the primary database of the selected provider
        /// </summary>
        /// <returns>Connection string</returns>
        public string GetPrimaryDatabaseConnectionString()
        {
            if (ProviderType == DatabaseProviderEnum.NotSelected)
            {
                throw new ArgumentException("ProviderType is not set");
            }

            return GetConnectionStringForDatabase(((ProviderType == DatabaseProviderEnum.Microsoft_SQL_Server) ? "master" : "mysql"));
        }

        /// <summary>
        /// Returns the connection string for the provided database name
        /// </summary>
        /// <param name="databaseName">Database name</param>
        /// <returns>Connection string</returns>
        public string GetConnectionStringForDatabase(string databaseName)
        {
            string result = string.Empty;

            if (IsUsingWindowsIntegratedAuthentication)
            {
                switch (ProviderType)
                {
                    case DatabaseProviderEnum.Microsoft_SQL_Server:
                        result = string.Format(
                                    "Data Source={0};Initial Catalog={1};Trusted_Connection=yes;{2}",
                                    DatabaseServerAddress,
                                    databaseName,
                                    ExtraOptions
                                 );
                        break;

                    case DatabaseProviderEnum.MySQL_OR_MariaDB_Server:
                        result = string.Format(
                                    "Server={0};Database={1};Trusted_Connection=yes;{2}",
                                    DatabaseServerAddress?.Replace(":", ";Port="),
                                    databaseName,
                                    ExtraOptions
                                 );
                        break;
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(ExtraOptions))
                {
                    if (!ExtraOptions.Contains("Persist Security Info=true;"))
                    {
                        if (!ExtraOptions.EndsWith(";"))
                        {
                            ExtraOptions += ";";
                        }

                        ExtraOptions += "Persist Security Info=true;";
                    }
                }
                else
                {
                    ExtraOptions = "Persist Security Info=true;";
                }

                switch (ProviderType)
                {
                    case DatabaseProviderEnum.Microsoft_SQL_Server:
                        result = string.Format(
                                    "Data Source={0};Initial Catalog={1};User Id={2};Password={3};{4}",
                                    DatabaseServerAddress,
                                    databaseName,
                                    Username,
                                    Password,
                                    ExtraOptions
                                 );
                        break;

                    case DatabaseProviderEnum.MySQL_OR_MariaDB_Server:
                        result = string.Format(
                                    "Server={0};Database={1};Uid={2};Pwd={3};{4}",
                                    DatabaseServerAddress?.Replace(":", ";Port="),
                                    databaseName,
                                    Username,
                                    Password,
                                    ExtraOptions
                                 );
                        break;
                }
            }

            return result;
        }

        /// <summary>
        /// Returns the completed connection string as per current values
        /// </summary>
        /// <returns>Connection string</returns>
        public override string ToString()
        {
            string result = string.Empty;

            if (IsUsingWindowsIntegratedAuthentication)
            {
                switch (ProviderType)
                {
                    case DatabaseProviderEnum.Microsoft_SQL_Server:
                        result = string.Format(
                                    "Data Source={0};Initial Catalog={1};Trusted_Connection=yes;{2}",
                                    DatabaseServerAddress,
                                    DatabaseName,
                                    ExtraOptions
                                 );
                        break;

                    case DatabaseProviderEnum.MySQL_OR_MariaDB_Server:
                        result = string.Format(
                                    "Server={0};Database={1};Trusted_Connection=yes;{2}",
                                    DatabaseServerAddress?.Replace(":", ";Port="),
                                    DatabaseName,
                                    ExtraOptions
                                 );
                        break;
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(ExtraOptions))
                {
                    if (!ExtraOptions.Contains("Persist Security Info=true;"))
                    {
                        if (!ExtraOptions.EndsWith(";"))
                        {
                            ExtraOptions += ";";
                        }

                        ExtraOptions += "Persist Security Info=true;";
                    }
                }
                else
                {
                    ExtraOptions = "Persist Security Info=true;";
                }

                switch (ProviderType)
                {
                    case DatabaseProviderEnum.Microsoft_SQL_Server:
                        result = string.Format(
                                    "Data Source={0};Initial Catalog={1};User Id={2};Password={3};{4}",
                                    DatabaseServerAddress,
                                    DatabaseName,
                                    Username,
                                    Password,
                                    ExtraOptions
                                 );
                        break;

                    case DatabaseProviderEnum.MySQL_OR_MariaDB_Server:
                        result = string.Format(
                                    "Server={0};Database={1};Uid={2};Pwd={3};{4}",
                                    DatabaseServerAddress?.Replace(":", ";Port="),
                                    DatabaseName,
                                    Username,
                                    Password,
                                    ExtraOptions
                                 );
                        break;
                }
            }

            return result;
        }

        /// <summary>
        /// Returns a cloned instance of DatabaseSettings
        /// </summary>
        /// <returns>Cloned database settings</returns>
        public DatabaseSettings Clone()
        {
            return new DatabaseSettings()
            {
                ProviderType = this.ProviderType,
                DatabaseName = this.DatabaseName,
                DatabaseServerAddress = this.DatabaseServerAddress,
                DatabaseServerPort = this.DatabaseServerPort,
                ExtraOptions = this.ExtraOptions,
                IsUsingWindowsIntegratedAuthentication = this.IsUsingWindowsIntegratedAuthentication,
                Username = this.Username,
                Password = this.Password
            };
        }

        /// <summary>
        /// Get the identifier start quote character
        /// </summary>
        /// <returns>Character as per provider type</returns>
        public string GetEnvironmentStartQuoteCharacter()
        {
            string character = "[";

            if (ProviderType == DatabaseProviderEnum.MySQL_OR_MariaDB_Server)
            {
                character = "`";
            }

            return character;
        }


        /// <summary>
        /// Get the identifier end quote character
        /// </summary>
        /// <returns>Character as per provider type</returns>
        public string GetEnvironmentEndQuoteCharacter()
        {
            string character = "]";

            if (ProviderType == DatabaseProviderEnum.MySQL_OR_MariaDB_Server)
            {
                character = "`";
            }

            return character;
        }

        #endregion
    }

    /// <summary>
    /// Enumeration of database providers
    /// </summary>
    public enum DatabaseProviderEnum
    {
        /// <summary>
        /// No provider has been selected
        /// </summary>
        NotSelected = 0,

        /// <summary>
        /// Microsoft SQL Server
        /// </summary>
        Microsoft_SQL_Server,

        /// <summary>
        /// MySQL or MariaDB server
        /// </summary>
        MySQL_OR_MariaDB_Server
    }
}
