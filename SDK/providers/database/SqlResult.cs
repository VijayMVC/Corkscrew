using System;
using System.Collections.Generic;
using System.Data;

namespace Corkscrew.SDK.providers.database
{
    /// <summary>
    /// [struct] Rich result from database operations
    /// </summary>
    public struct DatabaseActionResult
    {
        /// <summary>
        /// Value returned by remote call (eg: Stored Procedure or Function return)
        /// </summary>
        public int ReturnValue;

        /// <summary>
        /// Values of [out] parameters of called stored procedures
        /// </summary>
        public Dictionary<string, object> OutParameters;

        /// <summary>
        /// Dataset returned by SELECT queries or Stored Procedures or Table-valued functions
        /// </summary>
        public DataSet ResultDataSet;

        /// <summary>
        /// If [true], indicates an error occured
        /// </summary>
        public bool Error;

        /// <summary>
        /// If [Error] is [true], will contain the error message
        /// </summary>
        public string ErrorMessage;

        /// <summary>
        /// The exception object if an exception was thrown
        /// </summary>
        public Exception RichException;

        /// <summary>
        /// Name of the module that was executed
        /// </summary>
        public string ModuleName;
    }
}
