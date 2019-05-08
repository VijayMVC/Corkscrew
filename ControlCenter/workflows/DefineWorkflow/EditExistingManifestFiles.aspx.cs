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
    public partial class EditExistingManifestFiles : System.Web.UI.Page
    {
        private Guid defId = Guid.Empty;
        private CSFarm farm = null;
        private CSWorkflowDefinition def = null;
        private CSWorkflowManifest defManifest = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            farm = CSFarm.Open(WebHelpers.GetSessionUser(HttpContext.Current));

            if (string.IsNullOrEmpty(Request.QueryString["DefinitionId"]))
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
                Response.Redirect("/workflows/Step2.aspx?DefinitionId=" + defId);
            }

            lvExistingManifestItems.ItemEditing += lvExistingManifestItems_ItemEditing;
            lvExistingManifestItems.ItemUpdating += lvExistingManifestItems_ItemUpdating;
            lvExistingManifestItems.ItemCanceling += lvExistingManifestItems_ItemCanceling;
            lvExistingManifestItems.ItemDeleting += lvExistingManifestItems_ItemDeleting;
            lvExistingManifestItems.ItemDataBound += lvExistingManifestItems_ItemDataBound;

            if (!IsPostBack)
            {
                BindView();
            }

        }

        private void lvExistingManifestItems_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                if (e.Item.DataItemIndex == lvExistingManifestItems.EditIndex)
                {
                    CSWorkflowManifestItem manifestItem = (CSWorkflowManifestItem)e.Item.DataItem;
                    if (manifestItem == null)
                    {
                        return;
                    }

                    DropDownList list = (DropDownList)e.Item.FindControl("rowItemType");
                    if (list == null)
                    {
                        return;
                    }

                    list.Items.FindByValue(Enum.GetName(typeof(WorkflowManifestItemTypeEnum), manifestItem.ItemType)).Selected = true;
                }
                else
                {
                    if (lvExistingManifestItems.EditIndex != -1)
                    {
                        e.Item.Visible = false;
                    }
                }
            }
        }

        private void lvExistingManifestItems_ItemDeleting(object sender, ListViewDeleteEventArgs e)
        {
            HiddenField rowIdField = (HiddenField)lvExistingManifestItems.Items[e.ItemIndex].FindControl("hiddenRowManifestItemId");
            if (rowIdField == null)
            {
                BindView();
                return;
            }

            CSWorkflowManifestItem manifestItem = GetItemById(defManifest.GetItems(), Utility.SafeConvertToGuid(rowIdField.Value));
            if (manifestItem == null)
            {
                BindView();
                return;
            }

            defManifest.RemoveItem(manifestItem);
            BindView();
        }

        private void lvExistingManifestItems_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {
            HiddenField rowIdField = (HiddenField)lvExistingManifestItems.Items[e.ItemIndex].FindControl("hiddenRowManifestItemId");
            if (rowIdField == null)
            {
                BindView();
                return;
            }

            CSWorkflowManifestItem manifestItem = GetItemById(defManifest.GetItems(), Utility.SafeConvertToGuid(rowIdField.Value));
            if (manifestItem == null)
            {
                BindView();
                return;
            }

            TextBox fileName = (TextBox)lvExistingManifestItems.Items[e.ItemIndex].FindControl("rowFilename");
            TextBox extension = (TextBox)lvExistingManifestItems.Items[e.ItemIndex].FindControl("rowFilenameExtension");
            DropDownList list = (DropDownList)lvExistingManifestItems.Items[e.ItemIndex].FindControl("rowItemType");
            WorkflowManifestItemTypeEnum type = (WorkflowManifestItemTypeEnum)Enum.Parse(typeof(WorkflowManifestItemTypeEnum), list.SelectedItem.Value);
            CheckBox reqExec = (CheckBox)lvExistingManifestItems.Items[e.ItemIndex].FindControl("rowRequiredForExec");
            TextBox buildFolder = (TextBox)lvExistingManifestItems.Items[e.ItemIndex].FindControl("rowBuildFolder");
            TextBox runFolder = (TextBox)lvExistingManifestItems.Items[e.ItemIndex].FindControl("rowRuntimeFolder");

            manifestItem.Filename = fileName.Text;
            manifestItem.FilenameExtension = extension.Text;
            manifestItem.ItemType = type;
            manifestItem.RequiredForExecution = reqExec.Checked;
            manifestItem.BuildtimeRelativeFolder = buildFolder.Text;
            manifestItem.RuntimeRelativeFolder = runFolder.Text;
            manifestItem.Save();

            lvExistingManifestItems.EditIndex = -1;
            BindView();
        }

        protected void ClearManifestItemsButton_Click(object sender, EventArgs e)
        {
            foreach(CSWorkflowManifestItem item in defManifest.GetItems())
            {
                defManifest.RemoveItem(item);
            }

            BindView();
        }

        protected void UploadZipButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("/workflows/DefineWorkflow/Step3.aspx?DefinitionId=" + def.Id.ToString("d"));
        }

        private void lvExistingManifestItems_ItemCanceling(object sender, ListViewCancelEventArgs e)
        {
            lvExistingManifestItems.EditIndex = -1;
            BindView();
        }

        private void lvExistingManifestItems_ItemEditing(object sender, ListViewEditEventArgs e)
        {
            lvExistingManifestItems.EditIndex = e.NewEditIndex;
            BindView();
        }


        private CSWorkflowManifestItem GetItemById(IEnumerable<CSWorkflowManifestItem> items, Guid id)
        {
            CSWorkflowManifestItem result = null;

            foreach(CSWorkflowManifestItem t in items)
            {
                if (t.Id.Equals(id))
                {
                    result = t;
                    break;
                }
            }

            return result;
        }

        private void BindView()
        {
            lvExistingManifestItems.DataSource = defManifest.GetItems();
            lvExistingManifestItems.DataBind();
        }
    }
}