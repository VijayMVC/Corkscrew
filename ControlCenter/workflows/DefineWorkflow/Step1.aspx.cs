using Corkscrew.SDK.objects;
using Corkscrew.SDK.tools;
using Corkscrew.SDK.workflow;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;

namespace Corkscrew.ControlCenter.workflows.DefineWorkflow
{
    public partial class Step1 : System.Web.UI.Page
    {
        private Guid defId = Guid.Empty;
        private CSFarm farm = null;
        private CSWorkflowDefinition def = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            farm = CSFarm.Open(WebHelpers.GetSessionUser(HttpContext.Current));
            if (!string.IsNullOrEmpty(Request.QueryString["DefinitionId"]))
            {
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

                if (!IsPostBack)
                {
                    DefinitionId.Text = def.Id.ToString("d");
                    Name.Text = def.Name;
                    Description.Text = def.Description;
                    DefaultAssociationData.Text = def.DefaultAssociationData;
                    StartOnCreate.Checked = def.StartOnCreate;
                    StartOnModify.Checked = def.StartOnModify;
                    IsEnabled.Checked = def.IsEnabled;

                    if (def.IsTriggerRegistered(WorkflowTriggerEventNamesEnum.farm_created)) { SetEventListItem("farm_created"); }
                    if (def.IsTriggerRegistered(WorkflowTriggerEventNamesEnum.farm_modified)) { SetEventListItem("farm_modified"); }
                    if (def.IsTriggerRegistered(WorkflowTriggerEventNamesEnum.farm_deleted)) { SetEventListItem("farm_deleted"); }
                    if (def.IsTriggerRegistered(WorkflowTriggerEventNamesEnum.site_created)) { SetEventListItem("site_created"); }
                    if (def.IsTriggerRegistered(WorkflowTriggerEventNamesEnum.site_modified)) { SetEventListItem("site_modified"); }
                    if (def.IsTriggerRegistered(WorkflowTriggerEventNamesEnum.site_deleted)) { SetEventListItem("site_deleted"); }
                    if (def.IsTriggerRegistered(WorkflowTriggerEventNamesEnum.directory_created)) { SetEventListItem("directory_created"); }
                    if (def.IsTriggerRegistered(WorkflowTriggerEventNamesEnum.directory_modified)) { SetEventListItem("directory_modified"); }
                    if (def.IsTriggerRegistered(WorkflowTriggerEventNamesEnum.directory_deleted)) { SetEventListItem("directory_deleted"); }
                    if (def.IsTriggerRegistered(WorkflowTriggerEventNamesEnum.file_created)) { SetEventListItem("file_created"); }
                    if (def.IsTriggerRegistered(WorkflowTriggerEventNamesEnum.file_modified)) { SetEventListItem("file_modified"); }
                    if (def.IsTriggerRegistered(WorkflowTriggerEventNamesEnum.file_deleted)) { SetEventListItem("file_deleted"); }
                }
            }
        }

        protected void BackButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("/workflows/WorkflowDefinitions.aspx");
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            ErrorMessage.Text = "";

            if ((!StartOnCreate.Checked) && (!StartOnModify.Checked))
            {
                ErrorMessage.Text = "No start options specified. Check at least one of the two options.";
                return;
            }

            Dictionary<WorkflowTriggerEventNamesEnum, bool> selectedTriggers = new Dictionary<WorkflowTriggerEventNamesEnum, bool>();
            foreach (ListItem li in EventsList.Items)
            {
                selectedTriggers.Add((WorkflowTriggerEventNamesEnum)Enum.Parse(typeof(WorkflowTriggerEventNamesEnum), li.Value), li.Selected);
            }

            if (selectedTriggers.Count == 0)
            {
                ErrorMessage.Text = "Select at least one event to trigger on.";
                return;
            }

            if (def == null)
            {
                def = farm.AllWorkflowDefinitions.Add(Name.Text, Description.Text, DefaultAssociationData.Text, StartOnCreate.Checked, StartOnModify.Checked, false);
            }
            else
            {
                def.Name = Name.Text;
                def.Description = Description.Text;
                def.DefaultAssociationData = DefaultAssociationData.Text;
                def.StartOnCreate = StartOnCreate.Checked;
                def.StartOnModify = StartOnModify.Checked;
                def.Save();
            }

            if (def.IsEnabled && (!IsEnabled.Checked))
            {
                def.Disable();
            }
            else if ((! def.IsEnabled) && IsEnabled.Checked)
            {
                def.Enable();
            }

            // process triggers
            foreach(WorkflowTriggerEventNamesEnum trig in selectedTriggers.Keys)
            {
                SetEventTriggerRegistration(trig, selectedTriggers[trig]);
            }


            Response.Redirect("/workflows/DefineWorkflow/Step2.aspx?DefinitionId=" + def.Id.ToString("d"));
        }

        private void SetEventTriggerRegistration(WorkflowTriggerEventNamesEnum trigger, bool register)
        {
            if (def.IsTriggerRegistered(trigger))
            {
                if (! register)
                {
                    def.DeregisterTrigger(trigger);
                }
            }
            else
            {
                if (register)
                {
                    def.RegisterTrigger(trigger);
                }
            }
        }

        private void SetEventListItem(string value)
        {
            ListItem li = EventsList.Items.FindByValue(value);
            if (li != null)
            {
                li.Selected = true;
            }
        }
    }
}