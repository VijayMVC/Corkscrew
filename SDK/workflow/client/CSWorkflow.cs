using Corkscrew.SDK.exceptions;
using System;

namespace Corkscrew.SDK.workflow
{
    /// <summary>
    /// Workflow implementing apps must either implement ICSWorkflow or inherit from this class.
    /// </summary>
    public abstract class CSWorkflow : ICSWorkflow
    {
        
        #region Properties

        /// <summary>
        /// The workflow runtime context. This object gives access to all the information 
        /// related to this run of the workflow.
        /// </summary>
        public CSWorkflowRuntimeContext Context
        {
            get;
            private set;
        }

        #endregion

        #region Constructors

        private CSWorkflow()
        {
            // do nothing. 
            // private to prevent code from calling the default constructor.
        }


        /// <summary>
        /// Base constructor. Call this to setup the event handlers
        /// </summary>
        /// <param name="instance">The workflow instance this workflow is running</param>
        public CSWorkflow(CSWorkflowInstance instance)
            : this()
        {
            if (instance == null)
            {
                throw new ArgumentNullException();
            }

            // hook up event handlers
            instance.WorkflowStarted += new WorkflowEventHandler(OnStarted);
            instance.WorkflowPaused += new WorkflowEventHandler(OnPaused);
            instance.WorkflowContinued += new WorkflowEventHandler(OnContinued);
            instance.WorkflowCompleted += new WorkflowEventHandler(OnCompleted);
            instance.WorkflowErrored += new WorkflowEventHandler(OnError);

            Context = CSWorkflowRuntimeContext.CreateContext(instance);
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Handles the Workflow's Started event. This event handler runs AFTER the workflow has started to run. 
        /// Put all the code to handle running of your workflow here. Call the base class handler BEFORE your code.
        /// </summary>
        /// <param name="sender">The workflow instance that is raising the event</param>
        /// <param name="e">The workflow event args. Set e.Terminate to True to terminate the workflow on returning from this handler</param>
        protected virtual void OnStarted(CSWorkflowInstance sender, CSWorkflowEventArgs e)
        {
            Context.Instance.WriteTrace("Workflow started. {0}", BuildTraceString(e.Context));

        }

        /// <summary>
        /// Handles the Workflow's Continued event. This event handler runs AFTER the workflow has continued. 
        /// Put the code to handle the running of the workflow on continuation here. Call the base class handler BEFORE your code.
        /// </summary>
        /// <param name="sender">The workflow instance that is raising the event</param>
        /// <param name="e">The workflow event args. Set e.Terminate to True to terminate the workflow on returning from this handler</param>
        protected virtual void OnContinued(CSWorkflowInstance sender, CSWorkflowEventArgs e)
        {
            Context.Instance.WriteTrace("Workflow continued. {0}", BuildTraceString(e.Context));

        }

        /// <summary>
        /// Handles the Workflow's Paused event. This event handler runs AFTER the workflow has been paused. 
        /// Put the code to handle things like state-persistence here. Call the base class handler BEFORE your code.
        /// </summary>
        /// <param name="sender">The workflow instance that is raising the event</param>
        /// <param name="e">The workflow event args. Set e.Terminate to True to terminate the workflow on returning from this handler</param>
        protected virtual void OnPaused(CSWorkflowInstance sender, CSWorkflowEventArgs e)
        {
            Context.Instance.WriteTrace("Workflow paused. {0}", BuildTraceString(e.Context));

        }

        /// <summary>
        /// Handles the Workflow's Completed event. This event handler runs AFTER the workflow has completed.  Call the base class handler BEFORE your code.
        /// You cannot perform any action that would change state of the workflow here. Put code to clean up here.
        /// </summary>
        /// <param name="sender">The workflow instance that is raising the event</param>
        /// <param name="e">The workflow event args. Set e.Terminate to True to terminate the workflow on returning from this handler</param>
        protected virtual void OnCompleted(CSWorkflowInstance sender, CSWorkflowEventArgs e)
        {
            Context.Instance.WriteTrace("Workflow completed. {0}", BuildTraceString(e.Context));
            Context.Instance.WriteTrace("Completion reason: {0}", Enum.GetName(typeof(CSWorkflowEventCompletionTypesEnum), sender.CompletedReason));
        }

        /// <summary>
        /// Handles the Workflow's Errored eventThis event handler runs AFTER the workflow has completed.  Call the base class handler BEFORE your code.
        /// You cannot perform any action that would change state of the workflow here. Put code to clean up here.
        /// </summary>
        /// <param name="sender">The workflow instance that is raising the event</param>
        /// <param name="e">The workflow event args. Set e.Terminate to True to terminate the workflow on returning from this handler</param>
        protected virtual void OnError(CSWorkflowInstance sender, CSWorkflowEventArgs e)
        {
            Context.Instance.WriteTrace("Workflow errored. {0}", BuildTraceString(e.Context));
            Context.Instance.WriteTrace("Error: {0}", CSExceptionHelper.GetExceptionRollup(sender.LastException));
        }

        #endregion

        #region Methods

        /// <summary>
        /// Builds a string that surfaces the Ids of the elements in the Workflow runtime context object 
        /// for tracing/logging purposes.
        /// </summary>
        /// <param name="context">Workflow runtime context.</param>
        /// <returns>String containing the Workflow definition, association, instance and correlation Ids and the current state of the workflow.</returns>
        protected string BuildTraceString(CSWorkflowRuntimeContext context)
        {
            return string.Format(
                "Workflow Definition Id = {0}, Workflow Association Id = {1}, Correlation Id = {2}, Instance Id = {3}, Current State = {4}",
                context.Definition.Id.ToString("D"), 
                context.AssociationId.ToString("D"),
                context.CorrelationId.ToString("D"),
                context.Instance.Id.ToString("D"),
                Enum.GetName(typeof(CSWorkflowEventTypesEnum), context.State)
            );
        }

        /// <summary>
        /// Write tracing information to the InstanceWriter
        /// </summary>
        /// <param name="message">Message to write. This can also be a format string</param>
        /// <param name="data">If [message] is a format string, these are the token replacement values.</param>
        public void WriteTrace(string message, params object[] data)
        {
            Context.Instance.WriteTrace(message, data);
        }

        /// <summary>
        /// Pauses the workflow instance.
        /// </summary>
        /// <param name="persistenceInformation">Serialized information to persist. This is passed into the Continued event when workflow resumes.</param>
        public void Pause(string persistenceInformation)
        {
            Context.Instance.Pause(persistenceInformation);
        }

        /// <summary>
        /// Mark the workflow as completed.
        /// </summary>
        /// <param name="typeOfCompletion">Type of completion to set.</param>
        public void Complete(CSWorkflowEventCompletionTypesEnum typeOfCompletion = CSWorkflowEventCompletionTypesEnum.Successful)
        {
            Context.Instance.Complete(typeOfCompletion);
        }

        /// <summary>
        /// Marks the workflow as aborted. 
        /// Instead of calling this method, callers may also set the [terminate] out parameter to TRUE from 
        /// any of the Workflow event handlers.
        /// </summary>
        public void Abort()
        {
            Context.Instance.Abort();
        }

        /// <summary>
        /// Mark the workflow as errored
        /// </summary>
        /// <param name="message">Optional error message to include</param>
        public void MarkErrored(string message = null)
        {
            Context.Instance.MarkErrored(message);
        }

        /// <summary>
        /// Mark the workflow as errored
        /// </summary>
        /// <param name="exception">Optional exception to include</param>
        public void MarkErrored(Exception exception = null)
        {
            Context.Instance.MarkErrored(exception);
        }

        #endregion
    }
}
