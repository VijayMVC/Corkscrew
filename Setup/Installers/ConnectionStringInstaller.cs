using System.Collections.Generic;
using System.Configuration;
using System.Web.Configuration;

namespace CMS.Setup.Installers
{

    /// <summary>
    /// Installs connection strings into the config files
    /// </summary>
    public class ConnectionStringInstaller : WindowsInstaller
    {

        #region Properties

        /// <summary>
        /// Full path to the .config file to edit the connection strings in
        /// </summary>
        public string ConfigFilePath
        {
            get;
            set;
        }

        /// <summary>
        /// Database settings
        /// </summary>
        public DatabaseSettings DatabaseConfiguration
        {
            get;
            set;
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configPath">Full path to the .config file to edit the connection strings in</param>
        /// <param name="databaseSettings">Connection string value for ConfigDB</param>
        /// <param name="siteDb">Connection string value for SiteDB</param>
        public ConnectionStringInstaller(string configPath, DatabaseSettings databaseSettings)
            : base()
        {
            ConfigFilePath = configPath;
            DatabaseConfiguration = databaseSettings;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Installs the configuration settings
        /// </summary>
        /// <returns>True if settings were installed</returns>
        public override bool Install()
        {
            ExeConfigurationFileMap map = new ExeConfigurationFileMap()
            {
                ExeConfigFilename = ConfigFilePath
            };

            Configuration cfg = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);

            // take the chance to perform additional changes...
            if (ConfigFilePath.EndsWith("web.config"))
            {
                CompilationSection compileSection = (CompilationSection)cfg.GetSection("system.web/compilation");
                if (compileSection != null)
                {
                    if (!string.IsNullOrEmpty(compileSection.AssemblyPostProcessorType))
                    {
                        compileSection.AssemblyPostProcessorType = "";
                    }
                }

                cfg.AppSettings.Settings.Remove("Microsoft.VisualStudio.Enterprise.AspNetHelper.VsInstrLocation");

                // contains assembly binding to AspNetHelper
                cfg.Sections.Remove("runtime");
            }

            ConnectionStringSettingsCollection strings = cfg.ConnectionStrings.ConnectionStrings;
            cfg.ConnectionStrings.ConnectionStrings.Clear();

            // add the connection strings
            cfg.ConnectionStrings.ConnectionStrings.EmitClear = true;
            cfg.ConnectionStrings.ConnectionStrings.Add(new ConnectionStringSettings("configdb", DatabaseConfiguration.GetConnectionStringForDatabase("Corkscrew_ConfigDB"), "System.Data.SqlClient"));
            cfg.ConnectionStrings.ConnectionStrings.Add(new ConnectionStringSettings("sitedb", DatabaseConfiguration.GetConnectionStringForDatabase("$(DBName)"), "System.Data.SqlClient"));

            try
            {
                cfg.Save(ConfigurationSaveMode.Modified);

                UndoCommands.Push(() =>
                {
                    ExeConfigurationFileMap undoCfgMap = new ExeConfigurationFileMap()
                    {
                        ExeConfigFilename = ConfigFilePath
                    };

                    Configuration undoCfg = ConfigurationManager.OpenMappedExeConfiguration(undoCfgMap, ConfigurationUserLevel.None);
                    undoCfg.ConnectionStrings.ConnectionStrings.Clear();

                    foreach (ConnectionStringSettings cn in strings)
                    {
                        cfg.ConnectionStrings.ConnectionStrings.Add(new ConnectionStringSettings(cn.Name, cn.ConnectionString));
                    }

                    cfg.Save(ConfigurationSaveMode.Modified);
                });
            }
            catch
            {
                return false;
            }
            finally
            {
                cfg = null;
            }

            return true;
        }

        /// <summary>
        /// Repairs the configuration settings
        /// </summary>
        /// <returns>True if settings were repaired</returns>
        public override bool Repair()
        {
            return Install();
        }

        public override void Uninstall()
        {
            // do nothing
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
            foreach(string variable in environment.Keys)
            {
                ConfigFilePath = ConfigFilePath.Replace(variable, environment[variable]);
            }
        }

        #endregion
    }
}
