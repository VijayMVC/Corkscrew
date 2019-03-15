using Corkscrew.SDK.objects;
using Corkscrew.SDK.providers.database;
using Corkscrew.SDK.tools;
using System;
using System.Collections.Generic;
using System.Data;

namespace Corkscrew.SDK.odm
{
    /// <summary>
    /// Base class for all ODM classes
    /// </summary>
    internal class OdmBase
	{

        public static string DATABASE_CONFIGDB = "configdb";
        public static string DATABASE_SITEDB = "sitedb";

        public static string ERROR_NO_DATA_FOUND = "No data available";

        private bool _dataProviderIsEnabled = false;

		/// <summary>
		/// Data Provider, connected to our regular database
		/// </summary>
		protected ICSDatabaseProvider DataProvider { get; set; }

        /// <summary>
        /// Returns a correlation Id for the set of operations for this database session
        /// </summary>
        protected Guid CorrelationId
        {
            get
            {
                if (_correlationId == Guid.Empty)
                {
                    _correlationId = Guid.NewGuid();
                }

                return _correlationId;
            }
        }
        private Guid _correlationId = Guid.Empty;
        
        /// <summary>
        /// The last DatabaseActionResult. If the Error flag is set, an entry is automatically created 
        /// in the JournalLog table.
        /// </summary>
        protected DatabaseActionResult LastResult
        {
            get { return _lastResult; }
            private set
            {
                _lastResult = value;
            }
        }
        private DatabaseActionResult _lastResult = default(DatabaseActionResult);



		/// <summary>
		/// Protected constructor, only accessible to base class.
		/// We do not want external classes accidentally reinitializing the data provider!
		/// </summary>
        /// <param name="site">The CSSite to attach to. If NULL, attaches to ConfigDB.</param>
        protected OdmBase(CSSite site = null)
        {
            string selector = DATABASE_CONFIGDB;
            string server = null;
            string database = null;

            if ((site != null) && (! site.IsConfigSite))
            {
                selector = DATABASE_SITEDB;
                server = site.ContentDatabaseServerName;
                database = site.ContentDatabaseName;
            }

            DataProvider = CSDatabaseProviderFactory.GetProvider(selector, server, database);
            if ((DataProvider != null) && (DataProvider.TryConnect(DataProvider.Connection.ConnectionString)))
            {
                _dataProviderIsEnabled = true;
            }
        }

		/// <summary>
		/// Get data by executing a stored procedure
		/// </summary>
		/// <param name="sprocName">Stored procedure name</param>
		/// <param name="parameters">Parameters for stored procedure</param>
		/// <returns>DataSet object with data</returns>
		protected DataSet GetData(string sprocName, Dictionary<string, object> parameters = null)
		{
            if (! _dataProviderIsEnabled)
            {
                return null;
            }

			DatabaseActionResult result =
				DataProvider.ExecuteStoredProcedure
				(
					sprocName,
					parameters
				);

            result.ModuleName = sprocName;
            LastResult = result;
			if ((result.Error) || (! HasData(result.ResultDataSet)))
			{
				return null;
			}

			return result.ResultDataSet;
		}

        /// <summary>
        /// Get binary (byte array) data by executing a stored procedure
        /// </summary>
        /// <param name="sprocName">Stored procedure name</param>
        /// <param name="parameters">Parameters for stored procedure</param>
        /// <returns>Byte array with data</returns>
        protected byte[] GetBinaryData(string sprocName, Dictionary<string, object> parameters = null)
        {
            if (!_dataProviderIsEnabled)
            {
                return new byte[0];
            }

            return DataProvider.GetBinaryContent
            (
                sprocName,
                parameters,
                0,
                1
            );
        }


		/// <summary>
		/// Saves data by executing a stored procedure
		/// </summary>
		/// <param name="sprocName">Stored procedure name</param>
		/// <param name="parameters">Parameters for stored procedure</param>
		/// <returns>True if commit was successful.</returns>
		protected bool CommitChanges(string sprocName, Dictionary<string, object> parameters)
		{
            if (!_dataProviderIsEnabled)
            {
                return false;
            }

            DatabaseActionResult result = 
				DataProvider.ExecuteStoredProcedure
				(
					sprocName,
					parameters
				);

            result.ModuleName = sprocName;
            LastResult = result;

            if (result.Error)
			{
				return false;
			}

            return true;
        }

        /// <summary>
        /// Checks the given dataset to see if it has any data. This also checks if the dataset is NULL.
        /// </summary>
        /// <param name="dataset">Dataset to check</param>
        /// <returns>True if dataset is non-null and has atleast one table with atleast one row.</returns>
        protected bool HasData(DataSet dataset)
        {
            bool result = false;

            if (dataset != null)
            {
                if ((dataset.Tables != null) && (dataset.Tables.Count >= 1))
                {
                    foreach (DataTable table in dataset.Tables)
                    {
                        if (table.Rows.Count > 0)
                        {
                            result = true;
                            break;
                        }
                    }
                }
            }

            return result;
        }

        protected string GetTypeFromEntityRegistry(Guid id)
        {
            DataSet ds = GetData
            (
                "GetTypeFromEntityRegistry",
                new Dictionary<string, object>()
                {
                    { "EntityId", id.ToString("d") }
                }
            );

            if (HasData(ds))
            {
                return Utility.SafeString(ds.Tables[0].Rows[0]["EntityClass"]);
            }

            return null;
        }
	}
}
