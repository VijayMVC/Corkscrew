
using System;

namespace Corkscrew.SDK.constants
{

    /// <summary>
    /// Enumeration of the location of a FileSystemEntry object.
    /// </summary>
    public enum FileSystemEntryLocationEnum
    {

        /// <summary>
        /// This value used to initialize variables of this type
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Location is in Configuration database
        /// </summary>
        ConfigDB = 1,

        /// <summary>
        /// Location is in Site database
        /// </summary>
        /// <remarks>This value is not used in the current release</remarks>
        SiteDB = 2

    }

    /// <summary>
    /// Enumeration of the scope of a value or operation. This value is used to indicate the actual scope when 
    /// a method is dealing with multiple levels in the Corkscrew object hierarchy.
    /// </summary>
    [Flags]
    public enum ScopeEnum
    {

        /// <summary>
        /// Farm
        /// </summary>
        Farm = 16,

        /// <summary>
        /// Site
        /// </summary>
        Site = 8,

        /// <summary>
        /// Specific directory
        /// </summary>
        Directory = 4,

        /// <summary>
        /// Specific File
        /// </summary>
        File = 2,

        /// <summary>
        /// A file or directory (used in functions where determining the actual type of object cannot be done).
        /// </summary>
        FileOrDirectory = 1,

        /// <summary>
        /// Invalid scope (default value)
        /// </summary>
        Invalid = 0
    }

    /// <summary>
    /// Enumeration of the type of change operation performed
    /// </summary>
    public enum ChangeTypeEnum
    {
        /// <summary>
        /// Unknown or default value
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Create or insert type of change
        /// </summary>
        Create,

        /// <summary>
        /// Modify or update type of change
        /// </summary>
        Update,

        /// <summary>
        /// Delete or drop type of change
        /// </summary>
        Delete
    }


    /// <summary>
    /// Constants and enumerations used for diagnostics (tracing and logging).
    /// </summary>
    public static class LoggingConstants
    {

        /// <summary>
        /// Name of primary event source for logging
        /// </summary>
        public static readonly string LOG_EVENTSOURCE_NAME_PRIMARY = "Aquarius Corkscrew CMS";

        /// <summary>
        /// Logging event Id for Information messages
        /// </summary>
        public static readonly int LOG_EVENTID_INFO = 0;

        /// <summary>
        /// Logging event Id for Error messages
        /// </summary>
        public static readonly int LOG_EVENTID_ERROR = 100;

        /// <summary>
        /// Enum indicating type of log entry
        /// </summary>
        [Flags]
        public enum LogEntryTypeEnum : byte
        {
            /// <summary>
            /// Information message
            /// </summary>
            Information = 1,

            /// <summary>
            /// Error message
            /// </summary>
            Error = 2,

            /// <summary>
            /// Trace or debug message (can be combined with Information or Error)
            /// </summary>
            Trace = 4
        }

        /// <summary>
        /// Returns an event source name for a given module
        /// </summary>
        /// <param name="moduleName">Name of the module</param>
        /// <returns>Event source name to be used</returns>
        public static string GetEventSourceName(string moduleName)
        {
            if (string.IsNullOrEmpty(moduleName))
            {
                return LOG_EVENTSOURCE_NAME_PRIMARY;
            }

            return string.Format("{0} - {1}", LOG_EVENTSOURCE_NAME_PRIMARY, moduleName);
        }

    }


}
