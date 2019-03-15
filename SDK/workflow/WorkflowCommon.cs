namespace Corkscrew.SDK.workflow
{

    /// <summary>
    /// Events raised by Corkscrew Workflow
    /// </summary>
    public enum CSWorkflowEventTypesEnum
    {

        /// <summary>
        /// Unknown or not set
        /// </summary>
        Undefined = 0,

        /// <summary>
        /// Workflow has started execution (initial fire, not a continuation)
        /// </summary>
        Started,

        /// <summary>
        /// Workflow execution is paused to wait for some event or time.
        /// </summary>
        Paused,

        /// <summary>
        /// Workflow continues from previous (Paused) stage
        /// </summary>
        Continued,

        /// <summary>
        /// Workflow has completed in one of the completion states
        /// </summary>
        Completed,

        /// <summary>
        /// The workflow system encountered an unhandled exception and will terminate
        /// </summary>
        Errored
    }
    
    /// <summary>
    /// Types of completion (end) states of a Corkscrew Workflow. 
    /// These values apply when setting the event type to Completed.
    /// </summary>
    public enum CSWorkflowEventCompletionTypesEnum
    {

        /// <summary>
        /// Unknown or not set (or not completed)
        /// </summary>
        Undefined = 0,

        /// <summary>
        /// Successfully completed
        /// </summary>
        Successful,

        /// <summary>
        /// Workflow failed on attempting to (initial) start it. 
        /// </summary>
        ErrorOnStart,

        /// <summary>
        /// Workflow suffered a catastrophic failure while running
        /// </summary>
        ErrorProcessing,

        /// <summary>
        /// Workflow was terminated by the caller or the user
        /// </summary>
        TerminatedByUser,

        /// <summary>
        /// Workflow was internally aborted (through the workflow's own code, not user action)
        /// </summary>
        Aborted

    }
    
    /* 
        Workflow event delegates 
    */

    /// <summary>
    /// Event handler for the Corkscrew Workflow events
    /// </summary>
    /// <param name="sender">The workflow instance that is raising the event</param>
    /// <param name="e">The workflow event args. Set e.Terminate to True to terminate the workflow on returning from this handler</param>
    public delegate void WorkflowEventHandler(CSWorkflowInstance sender, CSWorkflowEventArgs e);

    /// <summary>
    /// Signals that a new workflow trigger has been registered and requires to be persisted to the backend.
    /// </summary>
    /// <param name="triggerName">Name of the trigger registered</param>
    internal delegate void WorkflowTriggerRegistered(string triggerName);

    /// <summary>
    /// Signals that a workflow trigger has been de-registered and requires to be un-persisted to the backend.
    /// </summary>
    /// <param name="triggerName">Name of the trigger de-registered</param>
    internal delegate void WorkflowTriggerDeregistered(string triggerName);

}
