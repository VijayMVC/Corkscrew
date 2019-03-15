using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Corkscrew.SDK.workflow
{

    /// <summary>
    /// Provides a mechanism to manage allowed / subscribed events. 
    /// This class is used by CSWorkflowDefinition and CSWorkflowAssociation
    /// </summary>
    internal class CSWorkflowTriggerEvents : IDisposable
    {

        #region Event Names

        public static string EVENTNAME_FARM_CREATED = "farm_created";
        public static string EVENTNAME_FARM_MODIFIED = "farm_modified";
        public static string EVENTNAME_FARM_DELETED = "farm_deleted";

        public static string EVENTNAME_SITE_CREATED = "site_created";
        public static string EVENTNAME_SITE_MODIFIED = "site_modified";
        public static string EVENTNAME_SITE_DELETED = "site_deleted";

        public static string EVENTNAME_DIRECTORY_CREATED = "directory_created";
        public static string EVENTNAME_DIRECTORY_MODIFIED = "directory_modified";
        public static string EVENTNAME_DIRECTORY_DELETED = "directory_deleted";

        public static string EVENTNAME_FILE_CREATED = "file_created";
        public static string EVENTNAME_FILE_MODIFIED = "file_modified";
        public static string EVENTNAME_FILE_DELETED = "file_deleted";

        private List<string> _eventNames = new List<string>()
        {
            EVENTNAME_FARM_CREATED,
            EVENTNAME_FARM_MODIFIED,
            EVENTNAME_FARM_DELETED,
            EVENTNAME_SITE_CREATED,
            EVENTNAME_SITE_MODIFIED,
            EVENTNAME_SITE_DELETED,
            EVENTNAME_DIRECTORY_CREATED,
            EVENTNAME_DIRECTORY_MODIFIED,
            EVENTNAME_DIRECTORY_DELETED,
            EVENTNAME_FILE_CREATED,
            EVENTNAME_FILE_MODIFIED,
            EVENTNAME_FILE_DELETED
        };

        #endregion

        // backing dictionary. Key is name of event
        private Dictionary<string, bool> _workflowEvents = null;

        #region Events
        /// <summary>
        /// Signals that a new workflow event has been registered and requires to be persisted to the backend.
        /// </summary>
        public event WorkflowTriggerRegistered EventRegistered
        {
            add
            {
                lock (_eventListLock)
                {
                    Events.AddHandler(eventRegisteredHandler, value);
                }
            }
            remove
            {
                lock (_eventListLock)
                {
                    Events.RemoveHandler(eventRegisteredHandler, value);
                }
            }
        }
        private object eventRegisteredHandler = null;

        /// <summary>
        /// Signals that a workflow event has been de-registered and requires to be un-persisted to the backend.
        /// </summary>
        public event WorkflowTriggerDeregistered EventDeregistered
        {
            add
            {
                lock (_eventListLock)
                {
                    Events.AddHandler(eventDeRegisteredHandler, value);
                }
            }
            remove
            {
                lock (_eventListLock)
                {
                    Events.RemoveHandler(eventDeRegisteredHandler, value);
                }
            }
        }
        private object eventDeRegisteredHandler = null;

        /// <summary>
        /// Collection of events
        /// </summary>
        protected EventHandlerList Events
        {
            get
            {
                lock (_eventListLock)
                {
                    if (_eventHandlers == null)
                    {
                        _eventHandlers = new EventHandlerList();
                    }
                }

                return _eventHandlers;
            }
        }
        private EventHandlerList _eventHandlers = null;
        private object _eventListLock = null;
        #endregion

        #region Methods

        /// <summary>
        /// Returns if the given event is registered
        /// </summary>
        /// <param name="eventName">Name of the event. If not set, checks if the collection has any events at all</param>
        /// <returns>True if event is registered</returns>
        public bool Has(string eventName = null)
        {
            if (string.IsNullOrEmpty(eventName))
            {
                return ((_workflowEvents != null) && (_workflowEvents.Count > 0));
            }

            ThrowIfEventNameIsInvalid(eventName);

            if ((_workflowEvents.ContainsKey(eventName)) && (_workflowEvents[eventName]))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns if the given event is registered
        /// </summary>
        /// <param name="eventName">Name of the event. If not set, checks if the collection has any events at all</param>
        /// <returns>True if event is registered</returns>
        public bool Has(WorkflowTriggerEventNamesEnum eventName = WorkflowTriggerEventNamesEnum.None)
        {
            if (eventName == WorkflowTriggerEventNamesEnum.None)
            {
                return ((_workflowEvents != null) && (_workflowEvents.Count > 0));
            }

            return Has(Enum.GetName(typeof(WorkflowTriggerEventNamesEnum), eventName));
        }

        /// <summary>
        /// Register the given event
        /// </summary>
        /// <param name="eventName">Name of the event</param>
        /// <param name="silently">If set, does not raise the event-registered event</param>
        public void Add(string eventName, bool silently = false)
        {
            ThrowIfEventNameIsInvalid(eventName);

            if (_workflowEvents.ContainsKey(eventName))
            {
                if (! _workflowEvents[eventName])
                {
                    _workflowEvents[eventName] = true;
                }
            }
            else
            {
                _workflowEvents.Add(eventName, true);
            }

            if (!silently)
            {
                lock (_eventListLock)
                {
                    if (_eventHandlers[eventRegisteredHandler] != null)
                    {
                        foreach (WorkflowTriggerRegistered handler in _eventHandlers[eventRegisteredHandler].GetInvocationList())
                        {
                            handler(eventName);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Register the given event
        /// </summary>
        /// <param name="eventName">Name of the event</param>
        /// <param name="silently">If set, does not raise the event-registered event</param>
        public void Add(WorkflowTriggerEventNamesEnum eventName, bool silently = false)
        {
            Add(Enum.GetName(typeof(WorkflowTriggerEventNamesEnum), eventName), silently);
        }

        /// <summary>
        /// Deregister the given event
        /// </summary>
        /// <param name="eventName">Name of the event</param>
        /// <param name="silently">If set, does not raise the event-deregistered event</param>
        public void Remove(string eventName, bool silently = false)
        {
            ThrowIfEventNameIsInvalid(eventName);

            if (_workflowEvents.ContainsKey(eventName))
            {
                _workflowEvents.Remove(eventName);
            }

            if (!silently)
            {
                lock (_eventListLock)
                {
                    if (_eventHandlers[eventDeRegisteredHandler] != null)
                    {
                        foreach (WorkflowTriggerDeregistered handler in _eventHandlers[eventDeRegisteredHandler].GetInvocationList())
                        {
                            handler(eventName);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Deregister the given event
        /// </summary>
        /// <param name="eventName">Name of the event</param>
        /// <param name="silently">If set, does not raise the event-deregistered event</param>
        public void Remove(WorkflowTriggerEventNamesEnum eventName, bool silently = false)
        {
            Remove(Enum.GetName(typeof(WorkflowTriggerEventNamesEnum), eventName), silently);
        }

        /// <summary>
        /// Validates that the given name of the event is a valid identifier. 
        /// If it is not a valid eventname, an ArgumentException is thrown.
        /// </summary>
        /// <param name="eventName">Name of event.</param>
        private void ThrowIfEventNameIsInvalid(string eventName)
        {
            if (string.IsNullOrEmpty(eventName) || (! _eventNames.Contains(eventName)))
            {
                throw new ArgumentException("Name of event is not recognized.");
            }
        }

        #endregion

        public CSWorkflowTriggerEvents()
        {
            _workflowEvents = new Dictionary<string, bool>();
            _eventHandlers = new EventHandlerList();
            _eventListLock = new object();
            eventDeRegisteredHandler = new object();
            eventRegisteredHandler = new object();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    lock (_eventListLock)
                    {
                        if (_eventHandlers[eventRegisteredHandler] != null)
                        {
                            foreach(Delegate d in _eventHandlers[eventRegisteredHandler].GetInvocationList())
                            {
                                Events.RemoveHandler(eventRegisteredHandler, d);
                            }
                        }

                        if (_eventHandlers[eventDeRegisteredHandler] != null)
                        {
                            foreach (Delegate d in _eventHandlers[eventDeRegisteredHandler].GetInvocationList())
                            {
                                Events.RemoveHandler(eventDeRegisteredHandler, d);
                            }
                        }

                        _eventHandlers.Dispose();
                    }
                }

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            Dispose(true);
        }
        #endregion

    }

    /// <summary>
    /// Names for workflow trigger events as an Enum.
    /// </summary>
    public enum WorkflowTriggerEventNamesEnum
    {
        /// <summary>
        /// Default value, no event. Is an invalid value to set. 
        /// When used in a "check" operation, this evaluates to "Any".
        /// </summary>
        None = 0,

        /// <summary>
        /// Farm created. 
        /// This event is not raised at this time since farm is already provisioned at install time!
        /// </summary>
        farm_created,

        /// <summary>
        /// Farm modified. 
        /// Raised on any configuration changes or creation/modification/deletion of sites within farm. 
        /// </summary>
        farm_modified,

        /// <summary>
        /// Farm deleted. 
        /// This event is not raised at this time since there is no place for this workflow to run after the farm is gone.
        /// </summary>
        farm_deleted,

        /// <summary>
        /// Site created.
        /// </summary>
        site_created,

        /// <summary>
        /// Site modified. 
        /// Triggered only on site configuration (Name, DNS, database) changes.
        /// </summary>
        site_modified,

        /// <summary>
        /// Site deleted.
        /// </summary>
        site_deleted,

        /// <summary>
        /// Directory created.
        /// Bubbles to site_modified.
        /// </summary>
        directory_created,

        /// <summary>
        /// Directory modified. 
        /// Triggered when directory properties are changed or files are added/removed in the directory. 
        /// Bubbles to site_modified.
        /// </summary>
        directory_modified,

        /// <summary>
        /// Directory deleted.
        /// Bubbles to site_modified.
        /// </summary>
        directory_deleted,

        /// <summary>
        /// File created.
        /// Bubbles to directory_modified.
        /// </summary>
        file_created,

        /// <summary>
        /// File modified.
        /// Bubbles to directory_modified.
        /// </summary>
        file_modified,

        /// <summary>
        /// File deleted. 
        /// Bubbles to directory_modified.
        /// </summary>
        file_deleted
    }
}
