using System.Collections.Generic;
using Corkscrew.SDK.objects;

namespace Corkscrew.SDK.odm
{

    /// <summary>
    /// ODM for Diagnostics functionality
    /// </summary>
    internal class OdmDiagnostics : OdmBase
    {

        /// <summary>
        /// Constructor
        /// </summary>
        public OdmDiagnostics() : base() { }

        /// <summary>
        /// Writes a diagnostics entry to the backend
        /// </summary>
        /// <param name="diagnostics">CSDiagnosticsEntry with data to be written</param>
        public void WriteEntry(CSDiagnosticsEntry diagnostics)
        {
            base.CommitChanges
            (
                "JournalLogWriteEntry",
                new Dictionary<string, object>()
                {
                    { "@MachineName", diagnostics.MachineName },
                    { "@LogType", diagnostics.EntryType },
                    { "@Timestamp", diagnostics.Timestamp },
                    { "@Message", diagnostics.Message },
                    { "@CorrelationId", diagnostics.CorrelationId },
                    { "@SiteId", diagnostics.SiteId },
                    { "@FileSystemEntryId", diagnostics.FileSystemEntryId },
                    { "@UserId", diagnostics.UserId },
                    { "@ModuleName", diagnostics.ModuleName },
                    { "@StateInfo", diagnostics.StateInfo },
                    { "@ExceptionStack", diagnostics.ExceptionStack },
                    { "@EventClass", diagnostics.EventClass },
                    { "@EventType", diagnostics.EventType },
                    { "@EventId", diagnostics.EventId }
                }
            );
        }

    }
}
