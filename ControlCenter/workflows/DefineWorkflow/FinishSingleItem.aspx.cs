using Corkscrew.SDK.objects;
using Corkscrew.SDK.tools;
using Corkscrew.SDK.workflow;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.UI.WebControls;

namespace Corkscrew.ControlCenter.workflows.DefineWorkflow
{
    public partial class FinishSingleItem : System.Web.UI.Page
    {
        private Guid defId = Guid.Empty;
        private CSFarm farm = null;
        private CSWorkflowDefinition def = null;
        private CSWorkflowManifest defManifest = null;
        private CSWorkflowManifestItem item = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            farm = CSFarm.Open(WebHelpers.GetSessionUser(HttpContext.Current));

            if ((string.IsNullOrEmpty(Request.QueryString["DefinitionId"])) || (string.IsNullOrEmpty(Request.QueryString["ItemId"])))
            {
                Response.Redirect("/workflows/WorkflowDefinitions.aspx");
            }

            defId = Utility.SafeConvertToGuid(Request.QueryString["DefinitionId"]);
            if (defId == Guid.Empty)
            {
                Response.Redirect("/workflows/WorkflowDefinitions.aspx");
            }

            def = farm.AllWorkflowDefinitions.Find(defId);
            if (def == null)
            {
                Response.Redirect("/workflows/WorkflowDefinitions.aspx");
            }

            defManifest = def.GetManifest();
            if (defManifest == null)
            {
                Response.Redirect("/workflows/Step3.aspx?DefinitionId=" + defId);
            }

            Guid itemId = Utility.SafeConvertToGuid(Request.QueryString["ItemId"]);
            if (itemId == Guid.Empty)
            {
                Response.Redirect("/workflows/Step3.aspx?DefinitionId=" + defId);
            }

            IReadOnlyList<CSWorkflowManifestItem> items = defManifest.GetItems();
            foreach(CSWorkflowManifestItem t in items)
            {
                if (t.Id == itemId)
                {
                    item = t;
                    break;
                }
            }

            if (item == null)
            {
                Response.Redirect("/workflows/Step3.aspx?DefinitionId=" + defId);
            }

            if (! IsPostBack)
            {
                DefinitionId.Text = defId.ToString("d");
                rowFilename.Text = item.Filename;
                rowFilenameExtension.Text = item.FilenameExtension;

                if (item.ItemType != WorkflowManifestItemTypeEnum.Unknown)
                {
                    rowItemType.Items.FindByValue(Enum.GetName(typeof(WorkflowManifestItemTypeEnum), item.ItemType)).Selected = true;
                }

                rowBuildFolder.Text = item.BuildtimeRelativeFolder;
                rowRuntimeFolder.Text = item.RuntimeRelativeFolder;
                rowRequiredForExec.Checked = item.RequiredForExecution;
            }
        }

        protected void BackButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("/workflows/Step3.aspx?DefinitionId=" + defId);
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            item.Filename = rowFilename.Text;
            item.FilenameExtension = rowFilenameExtension.Text;
            item.BuildtimeRelativeFolder = rowBuildFolder.Text;
            item.RequiredForExecution = rowRequiredForExec.Checked;
            item.RuntimeRelativeFolder = (item.RequiredForExecution ? rowRuntimeFolder.Text : null);
            item.ItemType = (WorkflowManifestItemTypeEnum)Enum.Parse(typeof(WorkflowManifestItemTypeEnum), rowItemType.SelectedItem.Value);
            item.Save();

            Response.Redirect("/workflows/WorkflowDefinitions.aspx");
        }
    }
}