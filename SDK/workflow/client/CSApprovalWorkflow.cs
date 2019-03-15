using Corkscrew.SDK.security;
using Corkscrew.SDK.tools;
using System;

namespace Corkscrew.SDK.workflow
{

    /// <summary>
    /// This is an example workflow simulating an approval scenario
    /// </summary>
    public class CSApprovalWorkflow : CSWorkflow
    {

        private CSSignaturePanel approvalPanel = null;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="instance">Workflow instance</param>
        public CSApprovalWorkflow(CSWorkflowInstance instance)
            : base(instance)
        {
        }

        /// <summary>
        /// Workflow starte event handler
        /// </summary>
        /// <param name="sender">Workflow instance</param>
        /// <param name="e">Workflow event arguments</param>
        protected override void OnStarted(CSWorkflowInstance sender, CSWorkflowEventArgs e)
        {
            base.OnStarted(sender, e);
            base.Context.Instance.WriteTrace("ApprovalWorkflow: Started");

            approvalPanel = new CSSignaturePanel(SignaturePanelTypeEnum.AtleastOneApproves, base.Context.Credential);
            approvalPanel.AddPanelMember(CSUser.CreateAnonymousUser(), false, false, false);
            approvalPanel.AddPanelMember(CSUser.CreateSystemUser(), false, false, false);
            approvalPanel.Start();

            base.Context.Instance.WriteTrace("ApprovalWorkflow: Sent for responses");

            Pause(approvalPanel.Id.ToString());
        }

        /// <summary>
        /// Workflow continued event handler
        /// </summary>
        /// <param name="sender">Workflow instance</param>
        /// <param name="e">Workflow event arguments</param>
        protected override void OnContinued(CSWorkflowInstance sender, CSWorkflowEventArgs e)
        {
            base.OnContinued(sender, e);
            base.Context.Instance.WriteTrace("ApprovalWorkflow: Continued");

            Guid id = Utility.SafeConvertToGuid(Context.Instance.InstanceInformation);
            if (id.Equals(Guid.Empty))
            {
                MarkErrored("Could not retrieve panel Id from persistence.");
                return;
            }

            approvalPanel = CSSignaturePanel.Get(id, Context.Credential);

            // instead of this loop to hardcode responses, a real-world scenario will evaluate the response from 
            // another source like a database table, email, file, web-page response, etc.
            foreach(CSSignatureItem item in approvalPanel.Members)
            {
                item.RegisterResponse(SignatureItemStateEnum.Approved, "Approved");
            }

            if ((approvalPanel.State == SignaturePanelStateEnum.Approved) || (approvalPanel.State == SignaturePanelStateEnum.Rejected))
            {
                approvalPanel.Terminate();
            }

            base.Context.Instance.WriteTrace("ApprovalWorkflow: Approvals completed.");

            Complete(CSWorkflowEventCompletionTypesEnum.Successful);
            base.Context.Instance.WriteTrace("ApprovalWorkflow: Completed");
        }

        /// <summary>
        /// Workflow errored event handler
        /// </summary>
        /// <param name="sender">Workflow instance</param>
        /// <param name="e">Workflow event arguments</param>
        protected override void OnError(CSWorkflowInstance sender, CSWorkflowEventArgs e)
        {
            base.OnError(sender, e);
            base.Context.Instance.WriteTrace("ApprovalWorkflow: Errored");
        }

    }
}
