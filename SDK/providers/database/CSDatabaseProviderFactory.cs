using Corkscrew.SDK.tools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Corkscrew.SDK.providers.database
{

    /// <summary>
    /// This class determines the backend provider class from the list of loaded or attached assemblies and provides the correct one based on the connection string. 
    /// The CSDatabaseProviderFactory class is what helps Corkscrew support multiple (database) backend engines to store its data.
    /// </summary>
    public static class CSDatabaseProviderFactory
    {

        /// <summary>
        /// The preferred provider. This is the one ConfigDB was opened with.
        /// </summary>
        private static Type preferredProvider = null;

        /// <summary>
        /// Gets set to true if something goes wrong. No more providers will be fetched.
        /// Call Discover() to reset.
        /// </summary>
        private static bool isErrorState = false;

        /// <summary>
        /// Storage cache for instantiated providers. Perf optimization.
        /// </summary>
        private static Hashtable instantiatedProviders = new Hashtable();

        /// <summary>
        /// Returns the PrimaryDatabaseName property for the preferred provider. 
        /// This name is designed to be the name of the primary database on the database engine on that server.
        /// </summary>
        /// <returns>Name of the primary database</returns>
        public static string GetPreferredProviderPrimaryDatabaseName()
        {
            // if discovery has not run yet, run it.
            if ((preferredProvider == null) && (!isErrorState))
            {
                Discover();
            }

            if ((preferredProvider == null) && (isErrorState))
            {
                throw new InvalidOperationException("Error instantiating backend provider.");
            }

            if (instantiatedProviders.ContainsKey("configdb"))
            {
                ICSDatabaseProvider provider = (ICSDatabaseProvider)instantiatedProviders["configdb"];
                return provider.PrimaryDatabaseName;
            }

            return null;
        }

        /// <summary>
        /// Will discover what type of connections we have configured 
        /// and will set up the provider preference.
        /// </summary>
        /// <param name="onlyIfConnectionSucceeds">If set, also tries to establish a connection to the target of the connection string. Provider is accepted only if the connection attempt succeeds.</param>
        /// <exception cref="Exception">If the "configdb" connection string could not be found or was emoty or no providers were found to match it</exception>
        public static void Discover(bool onlyIfConnectionSucceeds = false)
        {
            isErrorState = false;
            preferredProvider = null;
            instantiatedProviders.Clear();

            try
            {
                string connectionString = GetConnectionString("configdb");
                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new Exception("Connection string cannot be null or empty.");
                }

                Type IIprovider = typeof(ICSDatabaseProvider);

                IEnumerable<Type> implementedProviders = AppDomain.CurrentDomain.GetAssemblies()
                                                            .SelectMany(a => a.GetTypes())
                                                                .Where(a => (IIprovider.IsAssignableFrom(a) && (!a.IsInterface)));
                foreach (Type t in implementedProviders)
                {
                    if (t.GetMethod("TryConnect") != null)
                    {
                        try
                        {
                            ICSDatabaseProvider prov = (ICSDatabaseProvider)Activator.CreateInstance(t, connectionString);
                            if ((prov != null) && (!prov.HasError))
                            {
                                if ((! onlyIfConnectionSucceeds) || (onlyIfConnectionSucceeds && prov.TryConnect(connectionString)))
                                {
                                    preferredProvider = t;
                                    instantiatedProviders.Add("configdb", prov);
                                    break;
                                }
                            }
                        }
                        catch
                        {
                            // exception may be due to connection string containing engine parameters not supported by this engine
                            // eat
                        }
                    }
                }

                /*
                 *  We could have preferredProvider == null if:
                 *      (a) the connection string points to an OOBE-unsupported database engine
                 *      (b) none of the assemblies of the app have a "driver" for (a) 
                 *      (c) there is an error in the connection string (typically wrong username/password or some other parameter)
                 *      (d) the target database is down
                 */

                if (preferredProvider == null)
                {
                    throw new Exception("Unable to find a feasible database provider. Check if you have the right assemblies loaded, the correct connection string to ConfigDB and that the target database server and databases are online.");
                }
            }
            catch
            {
                isErrorState = true;
                throw;
            }
        }

        /// <summary>
        /// Returns the appropriate provider for the given connection
        /// </summary>
        /// <param name="selector">The connection string name ("configdb" selects the configuration database connection string)</param>
        /// <param name="server">The server to connect to</param>
        /// <param name="database">Name of the database we will be connecting to (required if [selector] is not "configdb")</param>
        /// <returns>The database provider.</returns>
        /// <exception cref="InvalidOperationException">If the factory is in errored state or the preferred provider is not set</exception>
        public static ICSDatabaseProvider GetProvider(string selector = "configdb", string server = null, string database = null)
        {
            // if discovery has not run yet, run it.
            if ((preferredProvider == null) && (!isErrorState))
            {
                Discover(true);
            }

            if ((preferredProvider == null) && (isErrorState))
            {
                throw new InvalidOperationException("Error instantiating backend provider.");
            }

            if (instantiatedProviders.ContainsKey(selector))
            {
                return (ICSDatabaseProvider)instantiatedProviders[selector];
            }
            server = server.SafeString(-1, false, false, onEmpty: "(local)");

            string connectionString = GetConnectionString(selector);
            if (string.IsNullOrEmpty(connectionString))
            {
                // fall back to configdb, we support everything there :-)
                selector = "configdb";
                connectionString = GetConnectionString(selector);
            }

            if (! string.IsNullOrEmpty(server))
            {
                connectionString = connectionString.Replace("$(DBServer)", server);
            }

            if (! string.IsNullOrEmpty(database))
            {
                connectionString = connectionString.Replace("$(DBName)", database);
            }

            ICSDatabaseProvider prov = (ICSDatabaseProvider)Activator.CreateInstance(preferredProvider, connectionString);
            return prov;
        }

        /// <summary>
        /// Returns the first database engine provider that works with the given connection string
        /// </summary>
        /// <param name="connectionString">Connection string to use</param>
        /// <param name="onlyIfConnectionSucceeds">If set, will first try to establish a connection. If not set, will not test connection.</param>
        /// <returns>Provider or NULL</returns>
        public static ICSDatabaseProvider GetProviderByConnectionString(string connectionString, bool onlyIfConnectionSucceeds = false)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                return null;
            }

            ICSDatabaseProvider provider = null;

            try
            {
                Type IIprovider = typeof(ICSDatabaseProvider);

                IEnumerable<Type> implementedProviders = AppDomain.CurrentDomain.GetAssemblies()
                                                            .SelectMany(a => a.GetTypes())
                                                                .Where(a => (IIprovider.IsAssignableFrom(a) && (!a.IsInterface)));
                foreach (Type t in implementedProviders)
                {
                    if (t.GetMethod("TryConnect") != null)
                    {
                        try
                        {
                            ICSDatabaseProvider prov = (ICSDatabaseProvider)Activator.CreateInstance(t, connectionString);
                            if ((prov != null) && (! prov.HasError))
                            {
                                if (
                                        (! onlyIfConnectionSucceeds)
                                        || (onlyIfConnectionSucceeds && (prov.TryConnect(connectionString)))
                                )
                                {
                                    provider = prov;
                                    break;
                                }
                            }
                        }
                        catch
                        {
                            // exception may be due to connection string containing engine parameters not supported by this engine
                            // eat
                        }
                    }
                }
            }
            catch
            {
            }

            return provider;
        }

        internal static string GetConnectionString(string connectionName)
        {
            string connectionString = null;

            if ((ConfigurationManager.ConnectionStrings != null) && (ConfigurationManager.ConnectionStrings[connectionName] != null))
            {
                connectionString = ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;
            }

            // if we have a preferred provider, check if ConfigDB has the connection string
            if ((preferredProvider != null) && (string.IsNullOrEmpty(connectionString)))
            {
                connectionString = (new Corkscrew.SDK.odm.OdmConfiguration()).GetConfiguration("configuration/connectionStrings/@" + connectionName);
            }

            return connectionString;
        }

    }
}
