using System.Collections.Generic;
using System.Data.Common;

namespace Corkscrew.SDK.providers.database
{

    /// <summary>
    /// Interface to be implemented by classes that wish to provide the ability to interact with a backend provider. 
    /// </summary>
    public interface ICSDatabaseProvider
    {

        /// <summary>
        /// Name of the database engine's primary database. 
        /// </summary>
        string PrimaryDatabaseName { get; }

        /// <summary>
        /// Starting character of the literal used to quote identifiers
        /// </summary>
        string QuoteStartChar { get; }

        /// <summary>
        /// Ending character of the literal used to quote identifiers
        /// </summary>
        string QuoteEndChar { get; }

        /// <summary>
        /// Name of the database engine's type 
        /// </summary>
        string ProviderName { get; }

        /// <summary>
        /// Provides access to the underlying connection so that the ODMs can 
        /// perform functions not directly supported by this provider class.
        /// </summary>
        DbConnection Connection { get; }

        /// <summary>
        /// Returns if the provider has suffered an error
        /// </summary>
        bool HasError { get; }

        /// <summary>
        /// Returns all the databases from the given connection string
        /// </summary>
        /// <param name="connectionString">Connection string</param>
        /// <returns>List of database names, empty list if not retrieved.</returns>
        List<string> GetAllDatabases(string connectionString);


        /// <summary>
        /// Execute a stored procedure and return its results
        /// </summary>
        /// <param name="procedureName">Stored procedure to run</param>
        /// <param name="inParameters">[OPTIONAL] Dictionary of IN parameters (keys are parameter names, values are param values)</param>
        /// <param name="outParameters">[OPTIONAL] Dictionary of OUT parameters. If this dictionary is NULL, then no out parameters are retrieved</param>
        /// <param name="commandTimeout">[OPTIONAL] Timeout of command. Default of 30 seconds.</param>
        /// <returns>DatabaseActionResult object with the data</returns>
        DatabaseActionResult ExecuteStoredProcedure(string procedureName, Dictionary<string, object> inParameters = null, Dictionary<string, object> outParameters = null, int commandTimeout = 30);

        /// <summary>
        /// Executes a stored procedure to retrieve byte[] data
        /// </summary>
        /// <param name="procedureName">Stored procedure to run</param>
        /// <param name="inParameters">[OPTIONAL] Dictionary of IN parameters (keys are parameter names, values are param values)</param>
        /// <param name="contentLengthColumnIndex">Zero-based index of the column containing the length of the content</param>
        /// <param name="contentColumnIndex">Zero-based index of the column containing the content</param>
        /// <param name="commandTimeout">[OPTIONAL] Timeout of command. Default of 30 seconds.</param>
        /// <returns>Byte array or NULL</returns>
        byte[] GetBinaryContent(string procedureName, Dictionary<string, object> inParameters = null, int contentLengthColumnIndex = 0, int contentColumnIndex = 1, int commandTimeout = 30);

        /// <summary>
        /// Attempts to connect to the given connection string
        /// </summary>
        /// <param name="connectionString">Connection string to try</param>
        /// <returns>True if connection was successful.</returns>
        bool TryConnect(string connectionString);

        /// <summary>
        /// Returns if the given table exists
        /// </summary>
        /// <param name="tableName">Name of the table to check</param>
        /// <returns>True if table exists</returns>
        bool TableExists(string tableName);

        /// <summary>
        /// Execute a non-query statement
        /// </summary>
        /// <param name="statement">DML statement to execute</param>
        /// <param name="commandTimeout">[OPTIONAL] Timeout of command. Default of 30 seconds.</param>
        /// <returns>DatabaseActionResult with any errors. Note that DatabaseActionResult datasets will NOT be populated</returns>
        DatabaseActionResult ExecuteNonQueryStatement(string statement, int commandTimeout = 30);

        /// <summary>
        /// Get a single value output (instead of a statement). This is similar to the ExecuteScalar statement.
        /// </summary>
        /// <param name="query">Select query to run</param>
        /// <param name="commandTimeout">[OPTIONAL] Timeout of command. Default of 30 seconds.</param>
        /// <param name="onEmpty">[OPTIONAL] value to return if nothing was returned by SQL</param>
        /// <returns>The string equivalent of what was returned.</returns>
        string GetSingleValue(string query, int commandTimeout = 30, string onEmpty = null);

        /// <summary>
        /// Execute a Sql SELECT statement and return its results
        /// </summary>
        /// <param name="selectStatement">SELECT statement to run</param>
        /// <param name="commandTimeout">[OPTIONAL] Timeout of command. Default of 30 seconds.</param>
        /// <returns>DatabaseActionResult object with the data</returns>
        DatabaseActionResult ExecuteSelectStatement(string selectStatement, int commandTimeout = 30);

    }
}
