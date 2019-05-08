using Corkscrew.SDK.constants;
using Corkscrew.SDK.objects;
using Corkscrew.SDK.tools;
using Corkscrew.SDK.workflow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;

namespace Corkscrew.ControlCenter.workflows
{
    public partial class AssociateWorkflow : System.Web.UI.Page
    {

        private CSFarm farm = null;
        private ScopeEnum associationScope = ScopeEnum.Invalid;
        private CSSite associationSite = null;
        private CSFileSystemEntryDirectory associationDirectory = null;
        private CSWorkflowAssociationCollection workflowAssociations = null;
        private List<CSWorkflowDefinition> workflowDefinitionsForDropdownBinding = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            farm = CSFarm.Open(WebHelpers.GetSessionUser(HttpContext.Current));

            associationScope = (ScopeEnum)Enum.Parse(typeof(ScopeEnum), Utility.SafeString(Request.QueryString["Scope"], "Invalid"));
            if ((associationScope != ScopeEnum.Farm) && (associationScope != ScopeEnum.Site) && (associationScope != ScopeEnum.Directory))
            {
                ReturnToSender();
            }

            Guid objectId = Utility.SafeConvertToGuid(Request.QueryString["ObjectId"]);
            switch (associationScope)
            {
                case ScopeEnum.Site:
                    associationSite = farm.AllSites.Find(objectId);
                    if (associationSite == null)
                    {
                        ReturnToSender();
                    }
                    break;

                case ScopeEnum.Directory:
                    if (string.IsNullOrEmpty(Request.QueryString["ParentSiteId"]))
                    {
                        ReturnToSender();
                    }

                    Guid parentSiteId = Utility.SafeConvertToGuid(Request.QueryString["ParentSiteId"]);
                    CSSite parentSite = farm.AllSites.Find(parentSiteId);
                    if (parentSite == null)
                    {
                        ReturnToSender();
                    }

                    associationDirectory = parentSite.GetDirectory(objectId);
                    if (associationDirectory == null)
                    {
                        ReturnToSender();
                    }
                    break;
            }

            switch (associationScope)
            {
                case ScopeEnum.Farm:
                    workflowAssociations = farm.AllWorkflowAssociations;
                    break;

                case ScopeEnum.Site:
                    workflowAssociations = associationSite.AllWorkflowAssociations;
                    break;

                case ScopeEnum.Directory:
                    workflowAssociations = associationDirectory.AllWorkflows;
                    break;
            }

            if (workflowAssociations == null)
            {
                ReturnToSender();
            }

            workflowDefinitionsForDropdownBinding = farm.AllWorkflowDefinitions.ToList().Where(d => (d.IsEnabled && (workflowAssociations.FindByDefinition(d.Id).Count() == 0))).ToList();
            if (workflowDefinitionsForDropdownBinding.Count == 0)
            {
                lvAssociations.InsertItemPosition = InsertItemPosition.None;
            }


            lvAssociations.ItemDataBound += lvAssociations_ItemDataBound;
            lvAssociations.ItemInserting += lvAssociations_ItemInserting;
            lvAssociations.ItemEditing += lvAssociations_ItemEditing;
            lvAssociations.ItemUpdating += lvAssociations_ItemUpdating;
            lvAssociations.ItemCanceling += lvAssociations_ItemCanceling;
            lvAssociations.ItemDeleting += lvAssociations_ItemDeleting;

            if (!IsPostBack)
            {
                lvAssociations.DataSource = workflowAssociations;
                lvAssociations.DataBind();

                if (workflowDefinitionsForDropdownBinding.Count > 0)
                {
                    DropDownList workflowNamesList = (DropDownList)lvAssociations.InsertItem.FindControl("InsertWorkflowNamesList");
                    if (workflowNamesList != null)
                    {
                        workflowNamesList.SelectedIndexChanged += WorkflowNamesList_SelectedIndexChanged;
                        workflowNamesList.DataSource = workflowDefinitionsForDropdownBinding;
                        workflowNamesList.DataTextField = "Name";
                        workflowNamesList.DataValueField = "Id";
                        workflowNamesList.DataBind();

                        if (workflowNamesList.Items.Count == 1)
                        {
                            workflowNamesList.Items[0].Selected = true;

                            CheckBoxList eventNamesList = (CheckBoxList)lvAssociations.InsertItem.FindControl("InsertEventsList");
                            foreach(ListItem li in eventNamesList.Items)
                            {
                                WorkflowTriggerEventNamesEnum name = (WorkflowTriggerEventNamesEnum)Enum.Parse(typeof(WorkflowTriggerEventNamesEnum), li.Value);
                                if (name != WorkflowTriggerEventNamesEnum.None)
                                {
                                    bool reg = workflowDefinitionsForDropdownBinding[0].IsTriggerRegistered(name);
                                    li.Enabled = reg;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void lvAssociations_ItemDeleting(object sender, ListViewDeleteEventArgs e)
        {
            HiddenField hfId = (HiddenField)lvAssociations.Items[e.ItemIndex].FindControl("hiddenAssociationId");
            Guid associationId = Utility.SafeConvertToGuid(hfId.Value);
            CSWorkflowAssociation association = null;

            switch (associationScope)
            {
                case ScopeEnum.Farm:
                    association = farm.AllWorkflowAssociations.Find(associationId);
                    break;

                case ScopeEnum.Site:
                    association = associationSite.AllWorkflowAssociations.Find(associationId);
                    break;

                case ScopeEnum.Directory:
                    association = associationDirectory.AllWorkflows.Find(associationId);
                    break;
            }
            if (association != null)
            {
                association.Delete();
            }

            RefreshPage();
        }

        private void lvAssociations_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {
            HiddenField hfId = (HiddenField)lvAssociations.Items[e.ItemIndex].FindControl("hiddenAssociationId");
            Guid associationId = Utility.SafeConvertToGuid(hfId.Value);
            CSWorkflowAssociation association = null;

            switch (associationScope)
            {
                case ScopeEnum.Farm:
                    association = farm.AllWorkflowAssociations.Find(associationId);
                    break;

                case ScopeEnum.Site:
                    association = associationSite.AllWorkflowAssociations.Find(associationId);
                    break;

                case ScopeEnum.Directory:
                    association = associationDirectory.AllWorkflows.Find(associationId);
                    break;
            }

            if (association != null)
            {
                TextBox assnName = (TextBox)lvAssociations.Items[e.ItemIndex].FindControl("EditAssociationName");
                CheckBox onStart = (CheckBox)lvAssociations.Items[e.ItemIndex].FindControl("EditOnStartCreate");
                CheckBox onModify = (CheckBox)lvAssociations.Items[e.ItemIndex].FindControl("EditOnStartModify");
                CheckBox enabled = (CheckBox)lvAssociations.Items[e.ItemIndex].FindControl("EditIsEnabled");
                CheckBoxList eventsList = (CheckBoxList)lvAssociations.Items[e.ItemIndex].FindControl("EditEventsList");

                association.Name = assnName.Text;
                association.StartOnCreate = onStart.Checked;
                association.StartOnModify = onModify.Checked;
                association.Save();

                if (association.IsEnabled && (! enabled.Checked))
                {
                    association.Disable();
                }
                else if ((! association.IsEnabled) && enabled.Checked)
                {
                    association.Enable();
                }

                foreach (ListItem li in eventsList.Items)
                {
                    if (li.Enabled)
                    {
                        WorkflowTriggerEventNamesEnum eventName = (WorkflowTriggerEventNamesEnum)Enum.Parse(typeof(WorkflowTriggerEventNamesEnum), li.Value);
                        if (eventName != WorkflowTriggerEventNamesEnum.None)
                        {
                            bool isSubscribed = association.IsEventSubscribed(eventName);
                            if (isSubscribed && (! li.Selected))
                            {
                                association.UnsubscribeWorkflowEvent(eventName);
                            }
                            else if ((! isSubscribed) && li.Selected)
                            {
                                association.SubscribeWorkflowEvent(eventName);
                            }
                        }
                    }
                }
            }

            lvAssociations.EditIndex = -1;
            RefreshPage();
        }

        private void lvAssociations_ItemEditing(object sender, ListViewEditEventArgs e)
        {
            lvAssociations.EditIndex = e.NewEditIndex;
            lvAssociations.DataSource = workflowAssociations;
            lvAssociations.DataBind();
        }

        private void lvAssociations_ItemInserting(object sender, ListViewInsertEventArgs e)
        {
            TextBox assnName = (TextBox)e.Item.FindControl("InsertAssociationName");
            DropDownList workflowDefinitionList = (DropDownList)e.Item.FindControl("InsertWorkflowNamesList");
            CheckBox onStart = (CheckBox)e.Item.FindControl("InsertOnStartCreate");
            CheckBox onModify = (CheckBox)e.Item.FindControl("InsertOnStartModify");
            CheckBox enabled = (CheckBox)e.Item.FindControl("InsertIsEnabled");
            CheckBoxList eventsList = (CheckBoxList)e.Item.FindControl("InsertEventsList");

            Guid workflowDefinitionId = Utility.SafeConvertToGuid(workflowDefinitionList.SelectedItem.Value);
            CSWorkflowDefinition workflowDefinition = farm.AllWorkflowDefinitions.Find(workflowDefinitionId);
            if (workflowDefinition == null)
            {
                // was deleted or disabled elsewhere?
                RefreshPage();
                return;
            }

            CSWorkflowAssociation association = null;
            switch (associationScope)
            {
                case ScopeEnum.Farm:
                    association = workflowDefinition.CreateFarmAssociation(assnName.Text);
                    break;

                case ScopeEnum.Site:
                    association = workflowDefinition.CreateSiteAssociation(assnName.Text, associationSite);
                    break;

                case ScopeEnum.Directory:
                    association = workflowDefinition.CreateDirectoryAssociation(assnName.Text, associationDirectory);
                    break;
            }

            association.StartOnCreate = onStart.Checked;
            association.StartOnModify = onModify.Checked;
            association.Save();

            if (association.IsEnabled && (!enabled.Checked))
            {
                association.Disable();
            }
            else if ((!association.IsEnabled) && enabled.Checked)
            {
                association.Enable();
            }

            foreach (ListItem li in eventsList.Items)
            {
                if (li.Enabled)
                {
                    WorkflowTriggerEventNamesEnum eventName = (WorkflowTriggerEventNamesEnum)Enum.Parse(typeof(WorkflowTriggerEventNamesEnum), li.Value);
                    if (eventName != WorkflowTriggerEventNamesEnum.None)
                    {
                        bool isSubscribed = association.IsEventSubscribed(eventName);
                        if (isSubscribed && (!li.Selected))
                        {
                            association.UnsubscribeWorkflowEvent(eventName);
                        }
                        else if ((!isSubscribed) && li.Selected)
                        {
                            association.SubscribeWorkflowEvent(eventName);
                        }
                    }
                }
            }

            RefreshPage();
        }

        private void lvAssociations_ItemCanceling(object sender, ListViewCancelEventArgs e)
        {
            lvAssociations.EditIndex = -1;

            lvAssociations.DataSource = workflowAssociations;
            lvAssociations.DataBind();
        }

        private void lvAssociations_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                CSWorkflowAssociation association = (CSWorkflowAssociation)e.Item.DataItem;

                if ((lvAssociations.EditIndex != -1) && (e.Item.DataItemIndex == lvAssociations.EditIndex))
                {
                    CheckBoxList eventsList = (CheckBoxList)e.Item.FindControl("EditEventsList");
                    if (eventsList != null)
                    {
                        foreach(ListItem li in eventsList.Items)
                        {
                            WorkflowTriggerEventNamesEnum eventName = (WorkflowTriggerEventNamesEnum)Enum.Parse(typeof(WorkflowTriggerEventNamesEnum), li.Value);
                            if (eventName != WorkflowTriggerEventNamesEnum.None)
                            {
                                if (! association.WorkflowDefinition.IsTriggerRegistered(eventName))
                                {
                                    li.Enabled = false;
                                }
                                else
                                {
                                    if (association.IsEventSubscribed(eventName))
                                    {
                                        li.Selected = true;
                                    }
                                }
                            }
                        }
                    }
                }
                else if ((lvAssociations.EditIndex == -1) || (e.Item.DataItemIndex != lvAssociations.EditIndex))
                {
                    List<string> sb = new List<string>();
                    foreach (WorkflowTriggerEventNamesEnum name in Enum.GetValues(typeof(WorkflowTriggerEventNamesEnum)))
                    {
                        if ((name != WorkflowTriggerEventNamesEnum.None) && (association.IsEventSubscribed(name)))
                        {
                            sb.Add(Enum.GetName(typeof(WorkflowTriggerEventNamesEnum), name));
                        }
                    }

                    Literal lit = (Literal)e.Item.FindControl("rowItemEventsList");
                    if (lit != null)
                    {
                        lit.Text = string.Join(", ", sb);
                    }
                }
            }
        }

        // Note: This event only fires for the InsertItem.
        protected void WorkflowNamesList_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList list = (DropDownList)sender;
            CSWorkflowDefinition def = null;
            CheckBoxList eventsList = (CheckBoxList)lvAssociations.InsertItem.FindControl("InsertEventsList");  // this event only fires for InsertItem

            if (list.SelectedItem == null)
            {
                return;
            }

            def = farm.AllWorkflowDefinitions.Find(Utility.SafeConvertToGuid(list.SelectedItem.Value));


            foreach (ListItem li in eventsList.Items)
            {
                WorkflowTriggerEventNamesEnum name = (WorkflowTriggerEventNamesEnum)Enum.Parse(typeof(WorkflowTriggerEventNamesEnum), li.Value);
                if (name != WorkflowTriggerEventNamesEnum.None)
                {
                    bool reg = def.IsTriggerRegistered(name);
                    li.Enabled = reg;
                }
            }
        }

        private void ReturnToSender()
        {
            Response.Redirect((string.IsNullOrEmpty(Request.QueryString["ReturnUrl"]) ? "/" : Request.QueryString["ReturnUrl"]));
        }

        private void RefreshPage()
        {
            Response.Redirect(Request.Url.ToString());
        }

    }
}