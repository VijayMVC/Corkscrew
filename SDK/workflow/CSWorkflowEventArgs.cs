using System;

namespace Corkscrew.SDK.workflow
{

    /// <summary>
    /// Event argument used by Corkscrew Workflow events
    /// </summary>
    public sealed class CSWorkflowEventArgs : EventArgs
    {

        #region Properties

        /// <summary>
        /// The workflow runtime context
        /// </summary>
        public CSWorkflowRuntimeContext Context
        {
            get;
            private set;
        } = null;

        /// <summary>
        /// Returns if the instance has suffered an exception
        /// </summary>
        public bool IsException
        {
            get
            {
                return (Context.Instance.LastException != null);
            }
        }

        /// <summary>
        /// Event consumer must set this to True to cause the workflow to terminate on return from this handler
        /// </summary>
        public bool Terminate
        {
            get;
            set;
        } = false;

        #endregion


        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="instance">Workflow instance to initialize with</param>
        public CSWorkflowEventArgs(CSWorkflowInstance instance)
            : base()
        {
            Context = CSWorkflowRuntimeContext.CreateContext(instance);
            Terminate = false;
        }

        #endregion

    }
}
