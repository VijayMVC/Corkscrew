using System;
using System.Diagnostics;
using System.Security.Permissions;
using Corkscrew.SDK.constants;
using Corkscrew.SDK.objects;
using Corkscrew.SDK.security;

namespace Corkscrew.SDK.diagnostics
{

    /// <summary>
    /// Class helps in writing of logging and tracing messages. Messages are always written to the JournalLog table in the 
    /// ConfigDB database and may optionally be written to the Windows Event Log.
    /// </summary>
    public static class CSDiagnostics
    {

        /// <summary>
        /// Flag indicating if the Windows Event Log event source exists and can be used.
        /// </summary>
        private static bool eventSourceExists = false;

        /// <summary>
        /// Create an entry in the logging infrastructure (Windows Event Log and database)
        /// </summary>
        /// <param name="CorrelationId">The correlation Guid (typically used by workflows)</param>
        /// <param name="type">EventLogEntryType of entry. Only Error and Information are used.</param>
        /// <param name="moduleName">Name of the module calling the function.</param>
        /// <param name="message">The error message to log.</param>
        /// <param name="isTracing">If true, this is a tracing message (debug/info)</param>
        /// <param name="error">If this is an exception being logged, the Exception object (used only if type == Error).</param>
        /// <param name="site">The CSSite associated with this event. NULL if global.</param>
        /// <param name="fileSystemEntry">The CSFileSystemEntry associated to this event. NULL if non-file related.</param>
        /// <param name="user">The CSUser attached to this event. NULL if no user.</param>
        /// <param name="writeToWindowsEventLog">If set, writes event to Windows Event Log as well. Otherwise writes only to the database log table.</param>
        public static void CreateLogEntry(Guid CorrelationId, EventLogEntryType type, string moduleName, string message,
            bool isTracing = false, Exception error = null, CSSite site = null, CSFileSystemEntry fileSystemEntry = null, CSUser user = null, 
                bool writeToWindowsEventLog = false)
        {

            string exceptionMessage = string.Empty;
            if ((type == EventLogEntryType.Error) && (error != null))
            {
                exceptionMessage = string.Format("{0}\n\nException: {1}", message, error.Message);
                while (error.InnerException != null)
                {
                    exceptionMessage = string.Format("{0}\n\nInner Exception: {1}", exceptionMessage, error.InnerException.ToString());
                    error = error.InnerException;
                }
            }

            CSDiagnosticsEntry journalEntry = new CSDiagnosticsEntry(CorrelationId)
            {
                MachineName = Environment.MachineName,
                Message = message,
                EntryType = ((type == EventLogEntryType.Information) ? LoggingConstants.LogEntryTypeEnum.Information : LoggingConstants.LogEntryTypeEnum.Error),
                Timestamp = DateTime.Now,

                SiteId = ((site == null) ? Guid.Empty : site.Id),
                FileSystemEntryId = ((fileSystemEntry == null) ? Guid.Empty : fileSystemEntry.Id),
                UserId = ((user == null) ? Guid.Empty : user.Id),

                ModuleName = moduleName,
                ExceptionStack = exceptionMessage,

                EventId = ((type == EventLogEntryType.Error) ? LoggingConstants.LOG_EVENTID_ERROR : LoggingConstants.LOG_EVENTID_ERROR)
            };

            // add the trace flag if applicable
            if (isTracing)
            {
                journalEntry.EntryType |= LoggingConstants.LogEntryTypeEnum.Trace;
            }

            // save it to ConfigDB.JournalLog table
            journalEntry.Save();


            // ------------ now add it to the Windows Event Log -----------
            if (writeToWindowsEventLog && eventSourceExists)
            {
                // change message to empty string to help with concats below.
                if (message == null)
                {
                    message = string.Empty;
                }


                message = exceptionMessage;
                EventLog.WriteEntry
                (
                    LoggingConstants.GetEventSourceName(moduleName),
                    message,
                    type,
                    ((type == EventLogEntryType.Error) ? LoggingConstants.LOG_EVENTID_ERROR : LoggingConstants.LOG_EVENTID_ERROR)
                );
            }
        }


        /// <summary>
        /// Create an EventLog source. 
        /// This method requires the calling code to run under a Local Administrator account on the particular machine.
        /// </summary>
        /// <param name="eventSourceName">Name of the event source to create</param>
        /// <param name="eventLogName">Name of the event log to create it in (default: Application)</param>
        /// <returns>True if log was created, false if something failed.</returns>
        [PrincipalPermission(SecurityAction.Demand, Role = @"BUILTIN\Administrators")]
        public static bool CreateEventSourceIfNotExists(string eventSourceName, string eventLogName = "Application")
        {
            try
            {
                eventSourceExists = EventLog.SourceExists(eventSourceName);
                if (! eventSourceExists)
                {
                    EventLog.CreateEventSource(eventSourceName, eventLogName);
                    eventSourceExists = true;
                }
            }
            catch { }

            return eventSourceExists;
        }

        /// <summary>
        /// Constructor for static class. Creates the Windows Event Log event source if it does not already exist.
        /// </summary>
        static CSDiagnostics()
        {
            try
            {
                // this call will fail if this code is not running in elevated mode.
                CreateEventSourceIfNotExists(LoggingConstants.GetEventSourceName(null));
            }
            catch
            {
                // eat the exception
            }
        }
    }
}
