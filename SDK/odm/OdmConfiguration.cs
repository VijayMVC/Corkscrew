using System.Collections.Generic;
using System.Data;
using Corkscrew.SDK.tools;

namespace Corkscrew.SDK.odm
{

    /// <summary>
    /// Object/Data mapper for Configuration
    /// </summary>
    internal class OdmConfiguration : OdmBase
    {

        /// <summary>
        /// Constructor
        /// </summary>
        public OdmConfiguration() : base() { }

        /// <summary>
        /// Returns if workflows are enabled for the farm
        /// </summary>
        /// <returns>True if workflows are enabled, false if not.</returns>
        public bool CheckIfWorkflowsAreEnabled()
        {
            // this is a simple check, we test if the ConfigDB contains our workflow related tables. If not, we dont have workflows :)
            bool result = 
                base.DataProvider.TableExists("WorkflowDefinitions") 
                && base.DataProvider.TableExists("WorkflowAssociations")
                && base.DataProvider.TableExists("WorkflowHistory")
                && base.DataProvider.TableExists("WorkflowInstances")
                && base.DataProvider.TableExists("WorkflowManifests")
                && base.DataProvider.TableExists("WorkflowManifestItems")
                ;

            return result;
        }

        /// <summary>
        /// Save the configuration value
        /// </summary>
        /// <param name="name">Name of the configuration</param>
        /// <param name="value">Value of the configuration</param>
        public bool Save(string name, string value)
        {
            return base.CommitChanges
            (
                "ConfigurationSave",
                new Dictionary<string, object>()
                {
                    { "@Name", name }, 
                    { "@Value", value }
                }
            );
        }

        /// <summary>
        /// Deletes a configuration setting by its name
        /// </summary>
        /// <param name="name">Name of the configuration setting to delete</param>
        public bool Delete(string name)
        {
            return base.CommitChanges
            (
                "ConfigurationDelete",
                new Dictionary<string, object>()
                {
                    { "@Name", name }
                }
            );
        }

        /// <summary>
        /// Get all the configuration information
        /// </summary>
        /// <returns>Dictionary of configuration information</returns>
        public Dictionary<string, string> GetAll()
        {
            DataSet ds = base.GetData
                (
                    "ConfigurationGetAll",
                    null
                );

            Dictionary<string, string> configuration = new Dictionary<string, string>();
            if (base.HasData(ds))
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    string name = Utility.SafeString(row["Name"]);
                    if (! configuration.ContainsKey(name))
                    {
                        configuration.Add(name, Utility.SafeString(row["Value"]));
                    }
                }
            }

            return configuration;
        }

        /// <summary>
        /// Gets a single configuration value
        /// </summary>
        /// <param name="name">Name of the configuration value to retrieve</param>
        /// <returns>String</returns>
        public string GetConfiguration(string name)
        {
            DataSet ds = base.GetData
                (
                    "ConfigurationGetByName",
                    new Dictionary<string, object>()
                    {
                        { "@Name", name }
                    }
                );

            if (! base.HasData(ds))
            {
                return null;
            }

            return Utility.SafeString(ds.Tables[0].Rows[0]["Value"]);
        }

    }
}
