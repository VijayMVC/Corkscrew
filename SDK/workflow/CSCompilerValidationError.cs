namespace Corkscrew.SDK.workflow
{

    /// <summary>
    /// Validates the workflow definition
    /// </summary>
    public class CSCompilerValidationError
    {

        /// <summary>
        /// Name of the operation that raised this error
        /// </summary>
        public string OperationName { get; set; }

        /// <summary>
        /// A numeric code for the error
        /// </summary>
        public int ErrorCode { get; }

        /// <summary>
        /// The error message
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Whether this is a "warning" level error. If false, then it is a fatal error
        /// </summary>
        public bool IsWarning { get; }

        /// <summary>
        /// Constructor for the error object
        /// </summary>
        /// <param name="message">The error message</param>
        /// <param name="code">A numeric code for the error</param>
        /// <param name="isWarning">Whether this is a "warning" level error. If false, then it is a fatal error</param>
        /// <param name="operationName">Name of the operation that raised this error</param>
        public CSCompilerValidationError(string message, int code, bool isWarning, string operationName)
        {
            OperationName = operationName;
            ErrorCode = code;
            IsWarning = isWarning;
            OperationName = operationName;
        }

    }
}
