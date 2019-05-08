using Corkscrew.SDK.objects;
using Corkscrew.SDK.tools;
using Corkscrew.SDK.workflow;
using System;
using System.Web;
using System.Web.UI.WebControls;

namespace Corkscrew.ControlCenter.workflows
{
    public partial class WorkflowDefinitions : System.Web.UI.Page
    {

        private CSFarm farm = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            farm = CSFarm.Open(WebHelpers.GetSessionUser(HttpContext.Current));

            lvWorkflowDefinitions.ItemEditing += lvWorkflowDefinitions_ItemEditing;
            lvWorkflowDefinitions.ItemDeleting += lvWorkflowDefinitions_ItemDeleting;

            if (! IsPostBack)
            {
                lvWorkflowDefinitions.DataSource = farm.AllWorkflowDefinitions;
                lvWorkflowDefinitions.DataBind();
            }
            
        }

        private void lvWorkflowDefinitions_ItemDeleting(object sender, ListViewDeleteEventArgs e)
        {
            HiddenField hf = (HiddenField)lvWorkflowDefinitions.Items[e.ItemIndex].FindControl("hiddenRowItemId");
            Guid defId = Utility.SafeConvertToGuid(hf.Value);
            CSWorkflowDefinition def = farm.AllWorkflowDefinitions.Find(defId);
            if (def != null)
            {
                def.Delete();
            }

            Response.Redirect(Request.Url.ToString());
        }

        private void lvWorkflowDefinitions_ItemEditing(object sender, ListViewEditEventArgs e)
        {
            HiddenField hf = (HiddenField)lvWorkflowDefinitions.Items[e.NewEditIndex].FindControl("hiddenRowItemId");
            Response.Redirect("/workflows/DefineWorkflow/Step1.aspx?DefinitionId=" + hf.Value);
        }

        protected void cbRowItemEnabled_CheckedChanged(object sender, EventArgs e)
        {

            CheckBox cb = (CheckBox)sender;
            ListViewDataItem dataItem = (ListViewDataItem)cb.NamingContainer;
            Guid Id = Utility.SafeConvertToGuid(lvWorkflowDefinitions.DataKeys[dataItem.DisplayIndex].Value);

            CSWorkflowDefinition def = farm.AllWorkflowDefinitions.Find(Id);
            if (def != null)
            {
                if ((cb.Checked) && (!def.IsEnabled))
                {
                    def.Enable();
                }
                else if ((!cb.Checked) && (def.IsEnabled))
                {
                    def.Disable();
                }
            }

            Response.Redirect("/workflows/WorkflowDefinitions.aspx");   // refresh
        }

        protected void CreateWorkflowDefinitionButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("/workflows/DefineWorkflow/Step1.aspx");
        }
    }
}