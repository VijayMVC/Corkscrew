namespace Corkscrew.SDK.workflow
{

    /// <summary>
    /// Workflow implementing apps must implement this interface or derive from CSWorkflow.
    /// </summary>
    public interface ICSWorkflow
    {

        #region Properties

        /// <summary>
        /// Workflow runtime context
        /// </summary>
        CSWorkflowRuntimeContext Context
        {
            get;
        }

        #endregion

    }

}
