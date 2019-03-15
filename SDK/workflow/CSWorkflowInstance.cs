using Corkscrew.SDK.diagnostics;
using Corkscrew.SDK.exceptions;
using Corkscrew.SDK.objects;
using Corkscrew.SDK.odm;
using Corkscrew.SDK.security;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;

namespace Corkscrew.SDK.workflow
{

    /// <summary>
    /// A single instance of a workflow
    /// </summary>
    public class CSWorkflowInstance : IDisposable
    {

        private CSUser _credential = null;                                                          // credential we run workflows under. Gets set to SystemUser in constructor
        private object _sendEventLock = new object();                                               // lock object. used in SendEvent() to queue events
        private AutoResetEvent _workflowEventWaiterAutoResetEvent = new AutoResetEvent(false);      // ARE used in RunHandler() for event thread to signal completion
        private bool _isSendEventRunning = false;                                                   // used in SendEvent() to tell us another SendEvent() is in progress, and queue the new event
        private bool _isSendEventDrainRunning = false;                                              // used in finally{} of SendEvent() to prevent multiple drains

        // both of these are only used within the SendEvent() func
        private Queue<CSWorkflowEventTypesEnum> _eventQueue = new Queue<CSWorkflowEventTypesEnum>();    // queue to hold events from overlapping, SendEvent()
        private Exception _eventQueueException = null;      // the first exception message in the queue will terminate the workflow, so we only need to cache ONE.

        /// <summary>
        /// The number of milliseconds the workflow event handler will timeout in. If the handler does not 
        /// complete in this time-limit, it is aborted and the workflow instance will be terminated as failed.
        /// </summary>
        public static int EVENTHANDLER_RUNTIME_LIMIT_MILLISECONDS = (5 * 60 * 1000);                // 5 minutes in milliseconds, after this the event handler is terminated.

        #region Properties

        /// <summary>
        /// Guid of this instance of the workflow (for tracing events, logs, etc)
        /// </summary>
        public Guid Id
        {
            get;
            internal set;
        } = Guid.Empty;


        /// <summary>
        /// Guid for this instance of the execution. On each reload from persistence layer, 
        /// this will be changed. This allows each hydration of the instance to be tracked seperately.
        /// </summary>
        public Guid RunInstanceCorrelationId
        {
            get
            {
                if (_correlationId == Guid.Empty)
                {
                    _correlationId = Guid.NewGuid();
                }

                return _correlationId;
            }
            set
            {
                _correlationId = value;
            }
        }
        private Guid _correlationId = Guid.Empty;

        /// <summary>
        /// Guid used to perform persistence for this instance. 
        /// This will change on each App.Run()
        /// </summary>
        internal Guid WorkflowPeristenceId
        {
            get;
            set;
        } = Guid.Empty;


        /// <summary>
        /// The association for this instance
        /// </summary>
        public CSWorkflowAssociation Association
        {
            get;
            internal set;
        } = null;

        /// <summary>
        /// Serialized custom data. 
        /// Format is managed by workflow through instantiation or step-related forms.
        /// </summary>
        public string InstanceInformation
        {
            get;
            internal set;
        } = null;

        /// <summary>
        /// Associated farm, will always return the current farm.
        /// </summary>
        public CSFarm Farm
        {
            get { return CSFarm.Open(_credential); }
        }

        /// <summary>
        /// Associated Site 
        /// (will be NULL only for Farm level workflows)
        /// </summary>
        public CSSite Site
        {
            get;
            internal set;
        } = null;

        /// <summary>
        /// Associated entity. 
        /// Will never be NULL.
        /// </summary>
        public IWorkflowInstantiable InstantiableEntity
        {
            get;
            internal set;
        }

        /// <summary>
        /// The current state of this workflow
        /// </summary>
        public CSWorkflowEventTypesEnum CurrentState
        {
            get;
            internal set;
        } = CSWorkflowEventTypesEnum.Undefined;

        /// <summary>
        /// If CurrentState is Completed, then this would give the completion reason.
        /// </summary>
        public CSWorkflowEventCompletionTypesEnum CompletedReason
        {
            get;
            internal set;
        } = CSWorkflowEventCompletionTypesEnum.Undefined;

        /// <summary>
        /// The full error message (including stack trace, etc) if the CompletedReason is one of the error states
        /// </summary>
        public string ErrorMessage
        {
            get
            {

                if ((_errorMessage == null) && (_lastException != null))
                {
                    _errorMessage =
                        string.Format(
                            "One or more errors occurred in the workflow. Workflow association Id: {0}, instance run Id: {1}, workflow state before exception: {2}.\n{3}",
                            Association.Id.ToString("D"),
                            RunInstanceCorrelationId.ToString("D"),
                            Enum.GetName(typeof(CSWorkflowEventTypesEnum), CurrentState),
                            CSExceptionHelper.GetExceptionRollup(_lastException)                                  // extension method handles NULLs
                        );
                }

                return _errorMessage;
            }
            internal set
            {
                _errorMessage = value;
            }
        }
        private string _errorMessage = null;

        /// <summary>
        /// The last exception raised, if CompletedReason is one of the error states
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations")]
        public Exception LastException
        {
            get
            {
                if ((_lastException == null) && (_errorMessage != null))
                {
                    _lastException = new Exception(_errorMessage);
                }

                return _lastException;
            }
            internal set
            {
                _lastException = value;
            }
        }
        private Exception _lastException = null;

        /// <summary>
        /// Returns if the workflow is in one of the states that allows state to be updated.
        /// </summary>
        public bool CanChangeState
        {
            get
            {
                return (
                    (CurrentState == CSWorkflowEventTypesEnum.Undefined)
                    || (CurrentState == CSWorkflowEventTypesEnum.Started)
                    || (CurrentState == CSWorkflowEventTypesEnum.Continued)
                    || (CurrentState == CSWorkflowEventTypesEnum.Paused)
                );
            }
        }

        /// <summary>
        /// Gets/sets a TextWriter that handles tracing calls through WriteTrace()
        /// </summary>
        public TextWriter TraceWriter
        {
            get { return _traceWriter; }
            set { _traceWriter = value; }
        }
        private TextWriter _traceWriter = null;

        /// <summary>
        /// Get/set if this instance is currently loaded in a runtime
        /// </summary>
        public bool IsLoadedInRuntime
        {
            get;
            set;
        }

        #endregion

        #region Event System

        /// <summary>
        /// Collection of events
        /// </summary>
        protected EventHandlerList Events
        {
            get
            {
                lock (_sendEventLock)
                {
                    if (_events == null)
                    {
                        _events = new EventHandlerList();
                    }
                }

                return _events;
            }
        }
        private EventHandlerList _events = null;

        /// <summary>
        /// Retrieves the eventhandler matching the given key under lock.
        /// </summary>
        /// <param name="eventHandlerKey">The key for the event handler to fetch</param>
        /// <returns>Event handler delegate</returns>
        private WorkflowEventHandler GetEventHandlerSafe(object eventHandlerKey)
        {
            WorkflowEventHandler handler = null;

            lock (_sendEventLock)
            {
                handler = (WorkflowEventHandler)Events[eventHandlerKey];
            }

            return handler;
        }

        #region Event Handlers
        /// <summary>
        /// Fires after the workflow has been set up and execution has been started. The workflow must perform 
        /// its "First Run" activity for the specific associated item within this event.
        /// </summary>
        public event WorkflowEventHandler WorkflowStarted
        {
            add { lock (_sendEventLock) { Events.AddHandler(workflowStartedEventHander, value); } }
            remove { lock (_sendEventLock) { Events.RemoveHandler(workflowStartedEventHander, value); } }
        }
        private object workflowStartedEventHander = new object();

        /// <summary>
        /// Fires when the workflow is paused to wait for some time period or event.
        /// </summary>
        public event WorkflowEventHandler WorkflowPaused
        {
            add { lock (_sendEventLock) { Events.AddHandler(workflowPausedEventHandler, value); } }
            remove { lock (_sendEventLock) { Events.RemoveHandler(workflowPausedEventHandler, value); } }
        }
        private object workflowPausedEventHandler = new object();

        /// <summary>
        /// Fires after the workflow has been resumed after a paused state. The workflow must perform 
        /// its continuation activity for the specific associated item within this event.
        /// </summary>
        public event WorkflowEventHandler WorkflowContinued
        {
            add { lock (_sendEventLock) { Events.AddHandler(workflowContinuedEventHandler, value); } }
            remove { lock (_sendEventLock) { Events.RemoveHandler(workflowContinuedEventHandler, value); } }
        }

        private object workflowContinuedEventHandler = new object();

        /// <summary>
        /// Fires after completion of the workflow. No state changes of the workflow is permitted. 
        /// </summary>
        public event WorkflowEventHandler WorkflowCompleted
        {
            add { lock (_sendEventLock) { Events.AddHandler(workflowCompletedEventHandler, value); } }
            remove { lock (_sendEventLock) { Events.RemoveHandler(workflowCompletedEventHandler, value); } }
        }
        private object workflowCompletedEventHandler = new object();

        /// <summary>
        /// Fires when there is an unhandled exception in the workflow code and it bubbles up into the Corkscrew engine.
        /// </summary>
        public event WorkflowEventHandler WorkflowErrored
        {
            add { lock (_sendEventLock) { Events.AddHandler(workflowErroredEventHandler, value); } }
            remove { lock (_sendEventLock) { Events.RemoveHandler(workflowErroredEventHandler, value); } }
        }
        private object workflowErroredEventHandler = new object();

        #endregion


        ////// WARNING! This method should _NEVER_ throw exceptions unless it _HAS_ to !!!
        /// <summary>
        /// Sends the event to the workflow. If an event is already executing, will queue the event and return.
        /// </summary>
        /// <param name="eventName">The event to send</param>
        /// <param name="exception">If this is an exception event, then set this value</param>
        private void SendEvent(CSWorkflowEventTypesEnum eventName, Exception exception = null)
        {

            // if a SendEvent is already running (outer event, any handlers we run may fire child SendEvents when states change), 
            // queue the new event so that we can run them all after we are done with the outer event
            lock (_sendEventLock)
            {
                if (_isSendEventRunning)
                {
                    // queue an errored event only if we dont have one already waiting
                    if ((eventName == CSWorkflowEventTypesEnum.Errored) && (!_eventQueue.ToArray().Contains(CSWorkflowEventTypesEnum.Errored)))
                    {
                        _eventQueue.Enqueue(eventName);

                        // queue only the first exception we get told about
                        if (exception != null)
                        {
                            _eventQueueException = exception;
                        }
                    }
                    else
                    {
                        _eventQueue.Enqueue(eventName);
                    }

                    return;
                }

                // this is reset in the finally{} below
                _isSendEventRunning = true;
            }

            // if this is an exception...
            if (eventName == CSWorkflowEventTypesEnum.Errored)
            {
                LastException = exception;
                ErrorMessage =
                    string.Format(
                        "One or more errors occurred in the workflow. Workflow association Id: {0}, instance run Id: {1}, workflow state before exception: {2}.\n{3}",
                        Association.Id.ToString("D"),
                        RunInstanceCorrelationId.ToString("D"),
                        Enum.GetName(typeof(CSWorkflowEventTypesEnum), CurrentState),
                        CSExceptionHelper.GetExceptionRollup(exception)                                          // extension method handles NULLs
                    );
            }

            // ensure all state information is uptodate
            RefreshLatestState();
            if ((!CanChangeState) || (!IsCorrectStateTransition(eventName)))
            {
                // we dont run the event as it not the right event at this time
                _isSendEventRunning = false;
                return;
            }

            // to locate the event delegates later
            object eventHandlerKey = null;

            try
            {
                switch (eventName)
                {
                    case CSWorkflowEventTypesEnum.Started:
                        RunInstanceCorrelationId = Guid.NewGuid();                  // do this only in Started and Continued
                        eventHandlerKey = workflowStartedEventHander;
                        UpdateStateInternal(CSWorkflowEventTypesEnum.Started);
                        break;

                    case CSWorkflowEventTypesEnum.Paused:
                        UpdateStateInternal(CSWorkflowEventTypesEnum.Paused);
                        eventHandlerKey = workflowPausedEventHandler;
                        break;

                    case CSWorkflowEventTypesEnum.Continued:
                        RunInstanceCorrelationId = Guid.NewGuid();                  // do this only in Started and Continued
                        eventHandlerKey = workflowContinuedEventHandler;
                        UpdateStateInternal(CSWorkflowEventTypesEnum.Continued);
                        break;

                    case CSWorkflowEventTypesEnum.Completed:
                        UpdateStateInternal
                        (
                            CSWorkflowEventTypesEnum.Completed,
                            ((CompletedReason == CSWorkflowEventCompletionTypesEnum.Undefined) ? CSWorkflowEventCompletionTypesEnum.Successful : CompletedReason)
                        );
                        eventHandlerKey = workflowCompletedEventHandler;
                        break;

                    case CSWorkflowEventTypesEnum.Errored:
                        UpdateStateInternal
                        (
                            CSWorkflowEventTypesEnum.Errored,
                            ((CurrentState <= CSWorkflowEventTypesEnum.Started) ? CSWorkflowEventCompletionTypesEnum.ErrorOnStart : CSWorkflowEventCompletionTypesEnum.ErrorProcessing)
                        );
                        eventHandlerKey = workflowErroredEventHandler;
                        break;
                }

                // IsLoadedInRuntime is only set if we are actually running in some (c#) appdomain. 
                // it will be False if the instance is sitting in the backend for example with someone running State change DML statements.

                if (IsLoadedInRuntime && (eventHandlerKey != null))
                {
                    WorkflowEventHandler handler = null;

                    handler = GetEventHandlerSafe(eventHandlerKey);
                    if (handler != null)
                    {
                        try
                        {
                            RunHandler(handler);

                            // if we are still here, we continue running the workflow
                            // shortcut by checking if RunHandler queued any events (may have marked paused/completed/errored) 
                            // we dont want to have to run unexpected event handlers here!

                            if (_eventQueue.Count == 0)
                            {
                                // no events are queued, go ahead and pause the workflow
                                if (CanChangeState && (CurrentState != CSWorkflowEventTypesEnum.Paused))
                                {
                                    UpdateStateInternal(CSWorkflowEventTypesEnum.Paused);
                                    handler = GetEventHandlerSafe(workflowPausedEventHandler);
                                    if (handler != null)
                                    {
                                        RunHandler(handler);
                                    }
                                }
                            }
                        }
                        catch (CSWorkflowException)
                        {
                            // thrown from RunHandler() to indicate we should abort the workflow
                            UpdateStateInternal
                            (
                                ((CurrentState != CSWorkflowEventTypesEnum.Errored) ? CSWorkflowEventTypesEnum.Completed : CurrentState),
                                CSWorkflowEventCompletionTypesEnum.TerminatedByUser
                            );
                        }
                        catch (Exception ex)
                        {
                            // this is any other exception from inside the handler itself or the threading or anything else
                            // if workflow is in abortable state, abort it.
                            if (CanChangeState)
                            {
                                LastException = ex;
                                UpdateStateInternal
                                (
                                    CSWorkflowEventTypesEnum.Errored,
                                    ((CurrentState == CSWorkflowEventTypesEnum.Started) ? CSWorkflowEventCompletionTypesEnum.ErrorOnStart : CSWorkflowEventCompletionTypesEnum.ErrorProcessing)
                                );
                            }
                        }
                    }
                }
            }
            catch
            {
                // eat exceptions
            }
            finally
            {
                lock (_sendEventLock)
                {
                    _isSendEventRunning = false;
                }

                // drain pending events
                if (!_isSendEventDrainRunning)
                {
                    _isSendEventDrainRunning = true;

                    while ((_eventQueue.Count > 0) && (CanChangeState))
                    {
                        CSWorkflowEventTypesEnum eventname = _eventQueue.Dequeue();
                        Exception ex = null;
                        if ((eventname == CSWorkflowEventTypesEnum.Errored) && (_eventQueueException != null))
                        {
                            ex = _eventQueueException;
                            _eventQueueException = null;
                        }

                        SendEvent(eventname, ex);
                        if ((eventname == CSWorkflowEventTypesEnum.Errored) || (eventname == CSWorkflowEventTypesEnum.Completed))
                        {
                            // dont process any more events after an Errored or Completed event
                            break;
                        }
                    }

                    _isSendEventDrainRunning = false;
                }
            }
        }

        /// <summary>
        /// Updates the state in the backend
        /// </summary>
        /// <param name="eventName">New state</param>
        /// <param name="completedName">If completed, the type of completion</param>
        private void UpdateStateInternal(CSWorkflowEventTypesEnum eventName, CSWorkflowEventCompletionTypesEnum completedName = CSWorkflowEventCompletionTypesEnum.Undefined)
        {
            CurrentState = eventName;

            // dont set CompletedReason unless eventname is Completed
            if (eventName == CSWorkflowEventTypesEnum.Completed)
            {
                CompletedReason = completedName;
            }

            (new OdmWorkflow(Association.WorkflowDefinition.Farm, Association.Site)).UpdateInstanceState(this);
        }

        // checks if requested state transition is legal
        // we can only go from some states to some other states.
        private bool IsCorrectStateTransition(CSWorkflowEventTypesEnum nextState)
        {
            bool result = false;

            // can go to Errored from any state
            if (nextState == CSWorkflowEventTypesEnum.Errored)
            {
                return true;
            }

            switch (CurrentState)
            {
                case CSWorkflowEventTypesEnum.Undefined:
                    // can go to: Started, Completed
                    if ((nextState == CSWorkflowEventTypesEnum.Started) || (nextState == CSWorkflowEventTypesEnum.Completed))
                    {
                        result = true;
                    }
                    break;

                case CSWorkflowEventTypesEnum.Started:
                case CSWorkflowEventTypesEnum.Continued:
                    // Can go to: Paused, Completed
                    if ((nextState == CSWorkflowEventTypesEnum.Paused) || (nextState == CSWorkflowEventTypesEnum.Completed))
                    {
                        result = true;
                    }
                    break;

                case CSWorkflowEventTypesEnum.Paused:
                    // a paused task cannot directly complete
                    if (nextState == CSWorkflowEventTypesEnum.Continued)
                    {
                        result = true;
                    }
                    break;

                    /* any other state to any other state is illegal */
            }

            return result;
        }

        // will throw an exception to cause workflow to terminate within SendEvent
        private void RunHandler(WorkflowEventHandler handler)
        {
            bool abortWorkflow = false, handlerThrewException = false;
            Exception handlerException = null;

            Delegate[] handlerInvocationList = handler?.GetInvocationList();
            if ((handlerInvocationList == null) || (handlerInvocationList.Length == 0))
            {
                // no delegates to run
                return;
            }

            Thread handlerThread = new Thread
            (
                () =>
                {
                    try
                    {
                        CSWorkflowEventArgs e = new CSWorkflowEventArgs(this);

                        foreach (WorkflowEventHandler handlerDelegate in handlerInvocationList)
                        {
                            try
                            {
                                handlerDelegate(this, e);
                                if (e.Terminate)
                                {
                                    abortWorkflow = true;
                                    break;                          // dont run any further delegates
                                }
                            }
                            catch (Exception ex)
                            {
                                handlerThrewException = true;
                                handlerException = ex;
                                break;                              // dont run any further delegates
                            }
                        }
                    }
                    catch (ThreadAbortException)
                    {
                        // without this, the TAE will bubble out to the app level and crash everything :-(
                        Thread.ResetAbort();
                    }
                    finally
                    {
                        // signal completion
                        _workflowEventWaiterAutoResetEvent.Set();
                    }
                }
            )
            {
                IsBackground = true                          // not visible in Task Manager
            };

            handlerThread.Start();

            if (!_workflowEventWaiterAutoResetEvent.WaitOne(EVENTHANDLER_RUNTIME_LIMIT_MILLISECONDS))
            {
                // timeout
                handlerThread.Abort();

                // wait for Abort() to do its work, finally block in thread sets the event.
                _workflowEventWaiterAutoResetEvent.WaitOne();
            }


            /*
             * If you're watching this in Debugger, you may see the below statements execute in parallel with the final statements 
             * inside the thread above. This is correct.
             */

            if (handlerThrewException)
            {
                throw new Exception("Workflow event handler threw an exception.", handlerException);
            }

            if (abortWorkflow)
            {
                throw new CSWorkflowException("Workflow event handler set abort flag to terminate workflow.");
            }
        }

        #endregion

        #region Constructors

        // internal constructor
        internal CSWorkflowInstance()
        {
            Id = Guid.NewGuid();
            RunInstanceCorrelationId = Guid.Empty;
            WorkflowPeristenceId = Guid.Empty;
            ErrorMessage = null;
            _credential = CSUser.CreateSystemUser();            // create and cache it.

            Association = null;
            InstanceInformation = null;
            Site = null;
            InstantiableEntity = null;

            CompletedReason = CSWorkflowEventCompletionTypesEnum.Undefined;
            CurrentState = CSWorkflowEventTypesEnum.Undefined;
            ErrorMessage = null;
            _events = null;

            TraceWriter = null;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get the history for this instance
        /// </summary>
        /// <returns>Workflow history chain (linked list)</returns>
        public CSWorkflowHistoryChain GetHistory()
        {
            return (new OdmWorkflow(Association.WorkflowDefinition.Farm, Association.Site)).GetHistoryByInstance(this);
        }

        /// <summary>
        /// Write tracing information to the TraceWriter
        /// </summary>
        /// <param name="message">Message to write. This can also be a format string</param>
        /// <param name="data">If [message] is a format string, these are the token replacement values.</param>
        public void WriteTrace(string message, params object[] data)
        {
            string finalMessage = message;

            // If the caller used the params as a String.Format(), make the message.
            if ((!string.IsNullOrEmpty(message)) && (data != null) && (data.Length > 0))
            {
                finalMessage = string.Format(message, data);
            }

            finalMessage = "(Instantiated Entity Id: " + InstantiableEntity.Id.ToString("d") + ")" + finalMessage;

            if (TraceWriter != null)
            {
                TraceWriter.WriteLine(finalMessage);
            }

            CSDiagnostics.CreateLogEntry(
                RunInstanceCorrelationId,
                System.Diagnostics.EventLogEntryType.Information,
                "CSWorkflowInstance",
                finalMessage,
                true,
                null,
                Site,
                null,
                null,
                false               // only to JournalLog table -- we should probably have a property or parameter later on to decide this.
            );

            finalMessage = null;
        }

        /// <summary>
        /// Sends the Start event to the workflow
        /// </summary>
        public void Start()
        {
            if ((CanChangeState) && (IsCorrectStateTransition(CSWorkflowEventTypesEnum.Started)))
            {
                SendEvent(CSWorkflowEventTypesEnum.Started);
            }
        }

        /// <summary>
        /// Sends the Continue event to the workflow
        /// </summary>
        public void Continue()
        {
            if ((CanChangeState) && (IsCorrectStateTransition(CSWorkflowEventTypesEnum.Continued)))
            {
                SendEvent(CSWorkflowEventTypesEnum.Continued);
            }
        }

        /// <summary>
        /// Pauses the workflow instance.
        /// </summary>
        /// <param name="persistenceInformation">Serialized information to persist. This is passed into the Continued event when workflow resumes.</param>
        public void Pause(string persistenceInformation)
        {
            if ((CanChangeState) && (IsCorrectStateTransition(CSWorkflowEventTypesEnum.Paused)))
            {
                InstanceInformation = persistenceInformation;
                SendEvent(CSWorkflowEventTypesEnum.Paused);
            }
        }

        /// <summary>
        /// Mark the workflow as completed.
        /// </summary>
        /// <param name="typeOfCompletion">Type of completion to set.</param>
        public void Complete(CSWorkflowEventCompletionTypesEnum typeOfCompletion = CSWorkflowEventCompletionTypesEnum.Successful)
        {
            if ((CanChangeState) && (IsCorrectStateTransition(CSWorkflowEventTypesEnum.Completed)))
            {
                CompletedReason = typeOfCompletion;
                SendEvent(CSWorkflowEventTypesEnum.Completed);
            }
        }

        /// <summary>
        /// Marks the workflow as aborted. 
        /// Instead of calling this method, callers may also set the [terminate] out parameter to TRUE from 
        /// any of the Workflow event handlers.
        /// </summary>
        /// <seealso cref="CSWorkflowEventArgs.Terminate"/>
        public void Abort()
        {
            if ((CanChangeState) && (IsCorrectStateTransition(CSWorkflowEventTypesEnum.Completed)))
            {
                CompletedReason = CSWorkflowEventCompletionTypesEnum.Aborted;
                SendEvent(CSWorkflowEventTypesEnum.Completed);
            }
        }

        /// <summary>
        /// Mark the workflow as errored
        /// </summary>
        /// <param name="message">Optional error message to include</param>
        public void MarkErrored(string message = null)
        {
            // no need to check state/transition. Can error from anywhere!
            SendEvent(CSWorkflowEventTypesEnum.Errored, new Exception(message));
        }

        /// <summary>
        /// Mark the workflow as errored
        /// </summary>
        /// <param name="exception">Optional exception to include</param>
        public void MarkErrored(Exception exception = null)
        {
            // no need to check state/transition. Can error from anywhere!
            SendEvent(CSWorkflowEventTypesEnum.Errored, exception);
        }

        /// <summary>
        /// Refreshes the latest state information from the persistence store
        /// </summary>
        public void RefreshLatestState()
        {
            CSWorkflowInstance instance = (new OdmWorkflow(Association.WorkflowDefinition.Farm, Association.Site)).GetInstanceById(Id, Association);

            // Don't refresh any other state variable here!!!
            CompletedReason = instance.CompletedReason;
            CurrentState = instance.CurrentState;
            ErrorMessage = instance.ErrorMessage;

            instance = null;
        }

        /// <summary>
        /// Copy the given filesystem item to the given destination directory. The destination can be on another site.
        /// </summary>
        /// <param name="source">The source filesystem item</param>
        /// <param name="destinationDirectory">The directory to copy to</param>
        /// <param name="preserveMetadata">If false, the Created/Modified/LastAccessed timestamps and credentials are modified to current values</param>
        /// <returns>The copied new filesytem item</returns>
        public CSFileSystemEntry CopyItem(CSFileSystemEntry source, CSFileSystemEntryDirectory destinationDirectory, bool preserveMetadata)
        {
            CSFileSystemEntry newEntry = new CSFileSystemEntry(source, true)
            {
                Site = destinationDirectory.Site,
                ParentDirectory = destinationDirectory
            };

            if (! preserveMetadata)
            {
                DateTime newTimestamps = DateTime.Now;
                newEntry.Created = newTimestamps;
                newEntry.Modified = newTimestamps;
                newEntry.LastAccessed = newTimestamps;

                newEntry.CreatedBy = source.AuthenticatedUser;
                newEntry.ModifiedBy = source.AuthenticatedUser;
                newEntry.LastAccessedBy = source.AuthenticatedUser;
            }

            newEntry.Save(false);

            if (! newEntry.IsFolder)
            {
                // copy content
                CSFileSystemEntryFile sourceFile = new CSFileSystemEntryFile(source);
                if (sourceFile.Open(FileAccess.Read))
                {
                    byte[] buffer = new byte[sourceFile.Size];
                    if (sourceFile.Read(buffer, 0, sourceFile.Size) > 0)
                    {
                        CSFileSystemEntryFile newEntryFile = new CSFileSystemEntryFile(newEntry);
                        if (newEntryFile.Open(FileAccess.Write))
                        {
                            newEntryFile.Write(buffer, 0, buffer.Length);
                            newEntryFile.Close();
                        }
                    }

                    sourceFile.Close();
                }
            }

            return newEntry;
        }

        #endregion

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        /// <summary>
        /// Dispose the instance
        /// </summary>
        /// <param name="disposing">True if disposing</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (_traceWriter != null) _traceWriter.Dispose();
                    if (_sendEventLock != null) _sendEventLock = null;
                    if (_workflowEventWaiterAutoResetEvent != null) _workflowEventWaiterAutoResetEvent.Dispose();
                    if (InstantiableEntity != null) InstantiableEntity = null;
                    if (_events != null) _events.Dispose();
                    if (Site != null) Site.Dispose();
                }

                disposedValue = true;
            }
        }

        /// <summary>
        /// Dispose the instance
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }
        #endregion

    }

    /// <summary>
    /// A collection of workflow instances
    /// </summary>
    public class CSWorkflowInstanceCollection : CSBaseCollection<CSWorkflowInstance>
    {

        internal CSWorkflowInstanceCollection() : base(true) { }

        /// <summary>
        /// Get instances for the given definition
        /// </summary>
        /// <param name="definition">The definition to fetch instances for</param>
        /// <param name="onlyRunnable">If set, returns only runnable and inprogress instances</param>
        /// <returns>Instance collection</returns>
        /// <exception cref="ArgumentNullException">If definition is null</exception>
        public static CSWorkflowInstanceCollection GetInstances(CSWorkflowDefinition definition, bool onlyRunnable = true)
        {
            if (definition == null)
            {
                throw new ArgumentNullException();
            }

            CSWorkflowInstanceCollection collection = new CSWorkflowInstanceCollection();
            collection.ImportItemsFromListHelper((new OdmWorkflow(definition.Farm)).GetInstancesForDefinition(definition, onlyRunnable));
            return collection;
        }


        /// <summary>
        /// Get instances for the given association
        /// </summary>
        /// <param name="association">Association to get instances for</param>
        /// <param name="onlyRunnable">If set, returns only runnable and inprogress instances</param>
        /// <returns>Instance collection</returns>
        /// <exception cref="ArgumentNullException">If association is null</exception>
        public static CSWorkflowInstanceCollection GetInstances(CSWorkflowAssociation association, bool onlyRunnable = true)
        {
            if (association == null)
            {
                throw new ArgumentNullException();
            }

            CSWorkflowInstanceCollection collection = new CSWorkflowInstanceCollection();
            collection.ImportItemsFromListHelper((new OdmWorkflow(association.WorkflowDefinition.Farm, association.Site)).GetInstancesForAssociation(association, onlyRunnable));
            return collection;
        }

        /// <summary>
        /// Gets all runnable workflow instances in the farm
        /// </summary>
        /// <param name="farm">The farm to get instances for</param>
        /// <returns>Instance collection</returns>
        /// <exception cref="ArgumentNullException">If farm is null or the authenticated user set in the farm is null</exception>
        public static CSWorkflowInstanceCollection GetAllRunnable(CSFarm farm)
        {
            if ((farm == null) || (farm.AuthenticatedUser == null))
            {
                throw new ArgumentNullException();
            }

            CSWorkflowInstanceCollection collection = new CSWorkflowInstanceCollection();
            collection.ImportItemsFromListHelper(new OdmWorkflow(farm).GetAllRunnableInstances());
            return collection;
        }

        /// <summary>
        /// Gets all runnable workflow instances in the site
        /// </summary>
        /// <param name="site">The site to get instances for</param>
        /// <returns>Instance collection</returns>
        /// <exception cref="ArgumentNullException">If farm is null or the authenticated user set in the farm is null</exception>
        public static CSWorkflowInstanceCollection GetAllRunnable(CSSite site)
        {
            if ((site == null) || (site.AuthenticatedUser == null))
            {
                throw new ArgumentNullException();
            }

            CSWorkflowInstanceCollection collection = new CSWorkflowInstanceCollection();
            collection.ImportItemsFromListHelper(new OdmWorkflow(site.Farm, site).GetAllRunnableInstances());
            return collection;
        }

        /// <summary>
        /// Removes an instance from the collection
        /// </summary>
        /// <param name="instance">Instance to remove</param>
        internal void Remove(CSWorkflowInstance instance)
        {
            base.RemoveInternal(instance);
        }

        private void ImportItemsFromListHelper(IEnumerable<CSWorkflowInstance> list)
        {
            foreach (CSWorkflowInstance item in list)
            {
                base.AddInternal(item, false);
            }
        }

    }
}



