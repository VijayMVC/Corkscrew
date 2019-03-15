using Corkscrew.SDK.tools;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;

namespace Corkscrew.SDK.providers.database
{

    /// <summary>
    /// Class implementing the SqlServer database provider
    /// </summary>
    public class CSSqlServerProvider : IDisposable, ICSDatabaseProvider
    {
        private SqlConnection _sqlConnection = null;

        /// <summary>
        /// Starting character of the literal used to quote identifiers
        /// </summary>
        public string QuoteStartChar { get { return "["; } }

        /// <summary>
        /// Ending character of the literal used to quote identifiers
        /// </summary>
        public string QuoteEndChar { get { return "]"; } }

        /// <summary>
        /// Provides access to the underlying connection so that the ODMs can 
        /// perform functions not directly supported by this provider class.
        /// </summary>
        public DbConnection Connection { get { return _sqlConnection; } }

        /// <summary>
        /// Returns if the provider has suffered an error
        /// </summary>
        public bool HasError { get; private set; }

        /// <summary>
        /// Name of the database engine's primary database. 
        /// </summary>
        public string PrimaryDatabaseName { get { return "master"; } }

        /// <summary>
        /// Name of the database engine's type 
        /// </summary>
        public string ProviderName { get { return "mssql"; } }

        #region Constructors

        /// <summary>
        /// Public constructor, initializes with the connection string
        /// </summary>
        /// <param name="connectionString">Connection string to initialize with</param>
        public CSSqlServerProvider(string connectionString)
        {
            try
            {
                _sqlConnection = new SqlConnection(connectionString);
                HasError = false;
            }
            catch
            {
                // we should ideally throw here, but creates other problems in our Factory
                HasError = true;
            }

        }

        /// <summary>
        /// NOP constructor
        /// </summary>
        public CSSqlServerProvider()
        {
            // do nothing
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns if the given table exists
        /// </summary>
        /// <param name="tableName">Name of the table to check</param>
        /// <returns>True if table exists</returns>
        public bool TableExists(string tableName)
        {
            using (SqlConnection cn = new SqlConnection(_sqlConnection.ConnectionString))
            using (SqlCommand cmd = new SqlCommand("select name from sys.tables where (name='" + tableName + "')", cn))
            {
                SqlDataReader reader = null;

                try
                {
                    string result = null;

                    cn.Open();
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        result = reader.GetString(0);
                    }

                    if (!string.IsNullOrEmpty(result))
                    {
                        return true;
                    }
                }
                catch { }
                finally
                {
                    reader.Close();
                    cn.Close();
                }
            }

            return false;
        }

        /// <summary>
        /// Returns all the databases from the given connection string
        /// </summary>
        /// <param name="connectionString">Connection string</param>
        /// <returns>List of database names, empty list if not retrieved.</returns>
        public List<string> GetAllDatabases(string connectionString = null)
        {
            List<string> list = new List<string>();

            using (SqlConnection cn = new SqlConnection(((!string.IsNullOrEmpty(connectionString)) ? connectionString : _sqlConnection.ConnectionString)))
            {
                SqlCommand cmd = null;
                SqlDataReader reader = null;

                try
                {
                    cn.Open();

                    cmd = new SqlCommand("select name from master.sys.databases", cn);
                    reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        list.Add(reader.GetString(0));
                    }
                }
                catch { }
                finally
                {
                    reader.Close();
                    cmd.Dispose();
                    cn.Close();
                }
            }

            return list;
        }


        /// <summary>
        /// Attempts to connect to the given connection string
        /// </summary>
        /// <param name="connectionString">Connection string to try</param>
        /// <returns>True if connection was successful.</returns>
        public bool TryConnect(string connectionString)
        {
            bool result = false;

            using (SqlConnection test = new SqlConnection(connectionString))
            {
                try
                {
                    test.Open();
                    result = true;
                }
                catch { return false; }
                finally
                {
                    if ((test != null) && (test.State == ConnectionState.Open))
                    {
                        test.Close();
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Execute a stored procedure and return its results
        /// </summary>
        /// <param name="procedureName">Stored procedure to run</param>
        /// <param name="inParameters">[OPTIONAL] Dictionary of IN parameters (keys are parameter names, values are param values)</param>
        /// <param name="outParameters">[OPTIONAL] Dictionary of OUT parameters. If this dictionary is NULL, then no out parameters are retrieved</param>
        /// <param name="commandTimeout">[OPTIONAL] Timeout of command. Default of 30 seconds.</param>
        /// <returns>DatabaseActionResult object with the data</returns>
        /// <exception cref="ArgumentException">If any of the outParameters are set to NULL (is not allowed since the ADO layer cannot guess the data type of such a parameter)</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public DatabaseActionResult ExecuteStoredProcedure(string procedureName, Dictionary<string, object> inParameters = null, Dictionary<string, object> outParameters = null, int commandTimeout = 30)
        {

            DatabaseActionResult result = new DatabaseActionResult()
            {
                Error = false,
                ErrorMessage = null,
                RichException = null,
                ResultDataSet = new DataSet(),
                ModuleName = procedureName
            };

            try
            {
                using (SqlConnection cn = new SqlConnection(_sqlConnection.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(procedureName, cn))
                {
                    try
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = commandTimeout;

                        if ((inParameters != null) && (inParameters.Count > 0))
                        {
                            foreach (string key in inParameters.Keys)
                            {
                                cmd.Parameters.Add(CreateParameter(key, inParameters[key], ParameterDirection.Input));
                            }
                        }

                        if ((outParameters != null) && (outParameters.Count > 0))
                        {
                            foreach (string key in outParameters.Keys)
                            {
                                if (outParameters[key] == null)
                                {
                                    throw new ArgumentException(string.Format("Value of [out] parameter [{0}] cannot be passed as [NULL] since the underlying layer cannot guess the datatype.", key));
                                }

                                cmd.Parameters.Add(CreateParameter(key, outParameters[key], ParameterDirection.Output));
                            }
                        }

                        cmd.Parameters.Add(CreateParameter("@returnValue", null, ParameterDirection.ReturnValue));

                        cn.Open();
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            try
                            {
                                sda.Fill(result.ResultDataSet);
                            }
                            catch (Exception fillException)
                            {
                                result.Error = true;
                                result.ErrorMessage = fillException.Message;
                                result.RichException = fillException;
                            }

                            if (!result.Error)
                            {
                                NameTables(result.ResultDataSet);

                                if (cmd.Parameters["@returnvalue"].Value != null)
                                {
                                    result.ReturnValue = Utility.SafeConvertToInt(cmd.Parameters["@returnvalue"].Value);
                                }
                                else
                                {
                                    result.ReturnValue = 0;
                                }

                                if (result.ReturnValue < 0)
                                {
                                    result.Error = true;
                                    result.ErrorMessage = "Stored procedure returned -1 (error condition) instead of throwing an exception.";
                                }

                                if (outParameters != null)
                                {
                                    result.OutParameters = new Dictionary<string, object>();
                                    foreach (string key in outParameters.Keys)
                                    {
                                        if (cmd.Parameters[key] != null)
                                        {
                                            result.OutParameters.Add(key, cmd.Parameters[key].Value);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception connectionException)
                    {
                        result.Error = true;
                        result.RichException = connectionException;
                        result.ErrorMessage = connectionException.Message;
                    }
                    finally
                    {
                        cn.Close();
                    }
                }

            }
            catch (Exception ex)
            {
                result.Error = true;
                result.ErrorMessage = ex.Message;
                result.RichException = ex;
            }
            finally
            {
            }

            return result;
        }

        /// <summary>
        /// Executes a stored procedure to retrieve byte[] data
        /// </summary>
        /// <param name="procedureName">Stored procedure to run</param>
        /// <param name="inParameters">[OPTIONAL] Dictionary of IN parameters (keys are parameter names, values are param values)</param>
        /// <param name="contentLengthColumnIndex">Zero-based index of the column containing the length of the content</param>
        /// <param name="contentColumnIndex">Zero-based index of the column containing the content</param>
        /// <param name="commandTimeout">[OPTIONAL] Timeout of command. Default of 30 seconds.</param>
        /// <returns>Byte array or NULL</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public byte[] GetBinaryContent(string procedureName, Dictionary<string, object> inParameters = null, int contentLengthColumnIndex = 0, int contentColumnIndex = 1, int commandTimeout = 30)
        {
            byte[] data = null;

            using (SqlConnection cn = new SqlConnection(_sqlConnection.ConnectionString))
            using (SqlCommand cmd = new SqlCommand(procedureName, cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = commandTimeout;

                if ((inParameters != null) && (inParameters.Count > 0))
                {
                    foreach (string key in inParameters.Keys)
                    {
                        cmd.Parameters.Add(CreateParameter(key, inParameters[key], ParameterDirection.Input));
                    }
                }

                try
                {
                    cn.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.SequentialAccess))
                    {
                        if (dr.Read())
                        {
                            long contentLength = dr.GetInt64(contentLengthColumnIndex);     // content length at index #0
                            if ((contentLength > 0) && (contentLength < Int32.MaxValue))
                            {
                                data = new byte[contentLength];

                                dr.GetBytes
                                (
                                    contentColumnIndex,             // content bytes sent at index #1
                                    0,                              // start at #0 byte of content
                                    data,                           // buffer byte array
                                    0,                              // insert at #0 byte of buffer array
                                    (int)contentLength              // read everything --- cast to "int" is OK. Our max size is 2GB which is int.MaxValue.
                                );
                            }
                        }
                    }
                }
                catch
                {

                }
                finally
                {
                    cn.Close();
                }
            }

            return data;
        }

        /// <summary>
        /// Execute a non-query statement
        /// </summary>
        /// <param name="statement">DML statement to execute</param>
        /// <param name="commandTimeout">[OPTIONAL] Timeout of command. Default of 30 seconds.</param>
        /// <returns>DatabaseActionResult with any errors. Note that DatabaseActionResult datasets will NOT be populated</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public DatabaseActionResult ExecuteNonQueryStatement(string statement, int commandTimeout = 30)
        {
            SqlCommand cmd = null;
            DatabaseActionResult result = new DatabaseActionResult();
            result.Error = false;

            try
            {
                _sqlConnection.Open();

                cmd = new SqlCommand(statement, _sqlConnection);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = commandTimeout;

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                result.Error = true;
                result.ErrorMessage = ex.Message;
                result.RichException = ex;
            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Dispose();
                    cmd = null;
                }

                _sqlConnection.Close();
            }

            return result;
        }

        /// <summary>
        /// Get a single value output (instead of a statement). This is similar to the ExecuteScalar statement.
        /// </summary>
        /// <param name="query">Select query to run</param>
        /// <param name="commandTimeout">[OPTIONAL] Timeout of command. Default of 30 seconds.</param>
        /// <param name="onEmpty">[OPTIONAL] value to return if nothing was returned by SQL</param>
        /// <returns>The string equivalent of what was returned.</returns>
        public string GetSingleValue(string query, int commandTimeout = 30, string onEmpty = null)
        {
            string value = onEmpty;

            DatabaseActionResult result = ExecuteSelectStatement(query, commandTimeout);
            if ((!result.Error) && (HasData(result.ResultDataSet, 1)))
            {
                value = Utility.SafeString(result.ResultDataSet.Tables[0].Rows[0][0], onEmpty);
            }

            return value;
        }

        /// <summary>
        /// Execute a Sql SELECT statement and return its results
        /// </summary>
        /// <param name="selectStatement">SELECT statement to run</param>
        /// <param name="commandTimeout">[OPTIONAL] Timeout of command. Default of 30 seconds.</param>
        /// <returns>DatabaseActionResult object with the data</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public DatabaseActionResult ExecuteSelectStatement(string selectStatement, int commandTimeout = 30)
        {

            DatabaseActionResult result = new DatabaseActionResult();
            result.Error = false;
            SqlCommand cmd = null;

            try
            {
                cmd = new SqlCommand(selectStatement, _sqlConnection);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = commandTimeout;

                _sqlConnection.Open();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                result.ResultDataSet = new DataSet();
                sda.Fill(result.ResultDataSet);

                NameTables(result.ResultDataSet);

                result.Error = false;
                result.ErrorMessage = null;
            }
            catch (Exception ex)
            {
                result.Error = true;
                result.ErrorMessage = ex.Message;
                result.RichException = ex;
            }
            finally
            {
                _sqlConnection.Close();

                if (cmd != null)
                {
                    cmd.Dispose();
                    cmd = null;
                }
            }

            return result;
        }

        #endregion

        #region IDisposable
        /// <summary>
        /// Dispose the provider
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Dispose the provider
        /// </summary>
        /// <param name="disposing">Set to true if really disposing</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_sqlConnection != null)
                {
                    if (_sqlConnection.State.HasFlag(ConnectionState.Open))
                    {
                        _sqlConnection.Close();
                    }

                    _sqlConnection.Dispose();
                }
            }
        }
        #endregion

        #region Internal Helper Methods

        /// <summary>
        /// Create a SqlParameter (to pass to ExecuteStoredProcedure)
        /// </summary>
        /// <param name="name">Name of parameter</param>
        /// <param name="value">Value to set for the parameter</param>
        /// <param name="direction">Whether input, output or bidirectional parameter</param>
        /// <returns>SqlParameter object</returns>
        private SqlParameter CreateParameter(string name, object value, ParameterDirection direction)
        {
            SqlParameter p = new SqlParameter((name.StartsWith("@") ? name : ("@" + name)), value);
            p.Direction = direction;
            return p;
        }



        /// <summary>
        /// Checks if the provided dataset has any tables and if any of those tables have at least one row.
        /// </summary>
        /// <param name="dataset">Dataset to check</param>
        /// <param name="expectNumberOfTables">[OPTIONAL] Minimum number of tables to expect. Default: 1.</param>
        /// <returns>True if the dataset is not null, has at least [expectNumberOfTables] tables and at least one of those tales has at least one datarow.</returns>
        private bool HasData(DataSet dataset, int expectNumberOfTables = 1)
        {
            bool result = false;

            if (expectNumberOfTables < 1)
            {
                return false;
            }

            if (dataset != null)
            {
                if ((dataset.Tables != null) && (dataset.Tables.Count >= expectNumberOfTables))
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

        /// <summary>
        /// Name the tables in the dataset if they do not already have a name.
        /// </summary>
        /// <param name="dataSet">The dataset with tables to rename</param>
        /// <returns>Dataset populated with the named tables</returns>
        private DataSet NameTables(DataSet dataSet)
        {
            if ((dataSet == null) || (dataSet.Tables.Count < 1))
            {
                return dataSet;
            }

            int tableIndex = 1;
            foreach (DataTable table in dataSet.Tables)
            {
                if (string.IsNullOrEmpty(table.TableName))
                {
                    table.TableName = string.Format
                                        (
                                            "Table{0}",
                                            ((dataSet.Tables.Count > 1) ? (tableIndex++).ToString() : "")
                                        );
                }
            }

            return dataSet;
        }

        #endregion
    }
}