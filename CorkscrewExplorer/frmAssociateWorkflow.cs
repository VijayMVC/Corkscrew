using Corkscrew.SDK.constants;
using Corkscrew.SDK.objects;
using Corkscrew.SDK.workflow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Corkscrew.Explorer
{
    public partial class frmAssociateWorkflow : Form
    {

        private Dictionary<Guid, string> definitions = null;

        public CSWorkflowDefinition WorkflowDefinition
        {
            get;
            set;
        }

        public CSWorkflowAssociation WorkflowAssociation
        {
            get;
            set;
        }

        public CSFarm Farm
        {
            get;
            set;
        }

        public object WorkflowTargetObject
        {
            get;
            set;
        }

        public ScopeEnum WorkflowScope
        {
            get;
            set;
        }

        public frmAssociateWorkflow()
        {
            InitializeComponent();
        }

        private void frmAssociateWorkflow_Shown(object sender, EventArgs e)
        {
            definitions = new Dictionary<Guid, string>();
            definitions.Add(Guid.Empty, "(Select a Workflow)");
            foreach(CSWorkflowDefinition def in Farm.AllWorkflowDefinitions)
            {
                definitions.Add(def.Id, string.Format("{0} (Id: {1})", def.Name, def.Id.ToString("d")));
            }
            cmbWorkflowDefinition.DataSource = definitions.Values.ToList();
            cmbWorkflowDefinition.SelectedItem = cmbWorkflowDefinition.Items[cmbWorkflowDefinition.FindString("(Select a Workflow)")];

            switch (WorkflowScope)
            {
                case ScopeEnum.Farm:
                    tbAssociationScopeUrl.Text = CSPath.CmsPathPrefix;
                    break;

                case ScopeEnum.Site:
                    tbAssociationScopeUrl.Text = CSPath.GetFullPath((CSSite)WorkflowTargetObject, null);
                    break;

                case ScopeEnum.Directory:
                    tbAssociationScopeUrl.Text = ((CSFileSystemEntryDirectory)WorkflowTargetObject).FullPath;
                    break;

                case ScopeEnum.Invalid:
                    // this is a special invalid scope we are setting in frmMainWindow
                    // to tell us that the user invoked this by clicking on the workflow definition.
                    tbAssociationScopeUrl.ReadOnly = false;

                    foreach (Guid key in definitions.Keys)
                    {
                        if (key == WorkflowDefinition.Id)
                        {
                            cmbWorkflowDefinition.SelectedItem = cmbWorkflowDefinition.Items[cmbWorkflowDefinition.FindString(definitions[key])];
                            break;
                        }
                    }
                    break;
            }

            if (WorkflowAssociation != null)
            {
                tbId.Text = WorkflowAssociation.Id.ToString("D");

                foreach(Guid key in definitions.Keys)
                {
                    if (key == WorkflowAssociation.WorkflowDefinition.Id)
                    {
                        cmbWorkflowDefinition.SelectedItem = cmbWorkflowDefinition.Items[cmbWorkflowDefinition.FindString(definitions[key])];
                        break;
                    }
                }

                tbAssociationName.Text = WorkflowAssociation.Name;

                chkTriggerFarmCreated.Checked = WorkflowAssociation.IsEventSubscribed(WorkflowTriggerEventNamesEnum.farm_created);
                chkTriggerFarmModified.Checked = WorkflowAssociation.IsEventSubscribed(WorkflowTriggerEventNamesEnum.farm_modified);
                chkTriggerFarmDeleted.Checked = WorkflowAssociation.IsEventSubscribed(WorkflowTriggerEventNamesEnum.farm_deleted);
                chkTriggerSiteCreated.Checked = WorkflowAssociation.IsEventSubscribed(WorkflowTriggerEventNamesEnum.site_created);
                chkTriggerSiteModified.Checked = WorkflowAssociation.IsEventSubscribed(WorkflowTriggerEventNamesEnum.site_modified);
                chkTriggerSiteDeleted.Checked = WorkflowAssociation.IsEventSubscribed(WorkflowTriggerEventNamesEnum.site_deleted);
                chkTriggerDirectoryCreated.Checked = WorkflowAssociation.IsEventSubscribed(WorkflowTriggerEventNamesEnum.directory_created);
                chkTriggerDirectoryModified.Checked = WorkflowAssociation.IsEventSubscribed(WorkflowTriggerEventNamesEnum.directory_modified);
                chkTriggerDirectoryDeleted.Checked = WorkflowAssociation.IsEventSubscribed(WorkflowTriggerEventNamesEnum.directory_deleted);
                chkTriggerFileCreated.Checked = WorkflowAssociation.IsEventSubscribed(WorkflowTriggerEventNamesEnum.file_created);
                chkTriggerFileModified.Checked = WorkflowAssociation.IsEventSubscribed(WorkflowTriggerEventNamesEnum.file_modified);
                chkTriggerFileDeleted.Checked = WorkflowAssociation.IsEventSubscribed(WorkflowTriggerEventNamesEnum.file_deleted);

                // once associated, definition cannot be changed
                cmbWorkflowDefinition.Enabled = false;


                lblCreated.Text = WorkflowAssociation.Created.ToString("MMM dd, yyyy HH:mm:ss");
                lblCreatedBy.Text = WorkflowAssociation.CreatedBy.Username;
                lblModified.Text = WorkflowAssociation.Modified.ToString("MMM dd, yyyy HH:mm:ss");
                lblModifiedBy.Text = WorkflowAssociation.ModifiedBy.Username;
            }
            else
            {
                tbId.Text = "(not generated)";
                chkAssociationIsEnabled.Checked = true;

                lblCreated.Text = DateTime.Now.ToString("MMM dd, yyyy HH:mm:ss");
                lblCreatedBy.Text = Farm.AuthenticatedUser.Username;
                lblModified.Text = lblCreated.Text;
                lblModifiedBy.Text = lblCreatedBy.Text;
            }

            tbAssociationName.Focus();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (cmbWorkflowDefinition.Enabled)
            {
                foreach (CSWorkflowDefinition def in Farm.AllWorkflowDefinitions)
                {
                    if (cmbWorkflowDefinition.SelectedItem.ToString() == string.Format("{0} (Id: {1})", def.Name, def.Id.ToString("d")))
                    {
                        WorkflowDefinition = def;
                        break;
                    }
                }
            }

            if (string.IsNullOrEmpty(tbAssociationName.Text))
            {
                MessageBox.Show("Association must have a name.");
                tbAssociationName.Focus();
                return;
            }

            if ((!chkAssociationIsEnabled.Checked) && (MessageBox.Show("You have not checked the \"Workflow is enabled\" checkbox. Workflow will be DISABLED. Is this what you want to do?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No))
            {
                chkAssociationIsEnabled.Focus();
                return;
            }

            if (WorkflowAssociation == null)
            {
                switch (WorkflowScope)
                {
                    case ScopeEnum.Farm:
                        WorkflowAssociation = WorkflowDefinition.CreateFarmAssociation(tbAssociationName.Text);
                        break;

                    case ScopeEnum.Site:
                        WorkflowAssociation = WorkflowDefinition.CreateSiteAssociation(tbAssociationName.Text, (CSSite)WorkflowTargetObject);
                        break;

                    case ScopeEnum.Directory:
                        WorkflowAssociation = WorkflowDefinition.CreateDirectoryAssociation(tbAssociationName.Text, (CSFileSystemEntryDirectory)WorkflowTargetObject);
                        break;
                }
            }

            WorkflowAssociation.AllowProcessingBubbledTriggers = false;
            WorkflowAssociation.Save();

            if (!chkAssociationIsEnabled.Checked)
            {
                WorkflowAssociation.Disable();
            }

            foreach (CheckBox chk in gbEventList.Controls)
            {
                WorkflowTriggerEventNamesEnum trigger = WorkflowTriggerEventNamesEnum.None;

                if (chk.Name.StartsWith("chk"))
                {
                    switch (chk.Name)
                    {
                        case "chkTriggerFarmCreated":
                            trigger = WorkflowTriggerEventNamesEnum.farm_created;
                            break;

                        case "chkTriggerFarmModified":
                            trigger = WorkflowTriggerEventNamesEnum.farm_modified;
                            break;

                        case "chkTriggerFarmDeleted":
                            trigger = WorkflowTriggerEventNamesEnum.farm_deleted;
                            break;

                        case "chkTriggerSiteCreated":
                            trigger = WorkflowTriggerEventNamesEnum.site_created;
                            break;

                        case "chkTriggerSiteModified":
                            trigger = WorkflowTriggerEventNamesEnum.site_modified;
                            break;

                        case "chkTriggerSiteDeleted":
                            trigger = WorkflowTriggerEventNamesEnum.site_deleted;
                            break;

                        case "chkTriggerDirectoryCreated":
                            trigger = WorkflowTriggerEventNamesEnum.directory_created;
                            break;

                        case "chkTriggerDirectoryModified":
                            trigger = WorkflowTriggerEventNamesEnum.directory_modified;
                            break;

                        case "chkTriggerDirectoryDeleted":
                            trigger = WorkflowTriggerEventNamesEnum.directory_deleted;
                            break;

                        case "chkTriggerFileCreated":
                            trigger = WorkflowTriggerEventNamesEnum.file_created;
                            break;

                        case "chkTriggerFileModified":
                            trigger = WorkflowTriggerEventNamesEnum.file_modified;
                            break;

                        case "chkTriggerFileDeleted":
                            trigger = WorkflowTriggerEventNamesEnum.file_deleted;
                            break;
                    }

                    if (trigger != WorkflowTriggerEventNamesEnum.None)
                    {
                        if (chk.Checked)
                        {
                            if ((!WorkflowAssociation.IsEventSubscribed(trigger)) && (chk.Enabled))
                            {
                                WorkflowAssociation.SubscribeWorkflowEvent(trigger);
                            }
                        }
                        else
                        {
                            if ((WorkflowAssociation.IsEventSubscribed(trigger)) && (chk.Enabled))
                            {
                                WorkflowAssociation.UnsubscribeWorkflowEvent(trigger);
                            }
                        }
                    }
                }
            }

            tbId.Text = WorkflowAssociation.Id.ToString("d");

            lblCreated.Text = WorkflowAssociation.Created.ToString("MMM dd, yyyy HH:mm:ss");
            lblCreatedBy.Text = WorkflowAssociation.CreatedBy.Username;
            lblModified.Text = WorkflowAssociation.Modified.ToString("MMM dd, yyyy HH:mm:ss");
            lblModifiedBy.Text = WorkflowAssociation.ModifiedBy.Username;

            UI.ShowMessage(this, "Association was saved.");
        }

        private void cmbWorkflowDefinition_SelectedIndexChanged(object sender, EventArgs e)
        {

            foreach (CSWorkflowDefinition def in Farm.AllWorkflowDefinitions)
            {
                if (cmbWorkflowDefinition.SelectedItem.ToString() == string.Format("{0} (Id: {1})", def.Name, def.Id.ToString("d")))
                {
                    chkTriggerFarmCreated.Enabled = def.IsTriggerRegistered(WorkflowTriggerEventNamesEnum.farm_created);
                    chkTriggerFarmModified.Enabled = def.IsTriggerRegistered(WorkflowTriggerEventNamesEnum.farm_modified);
                    chkTriggerFarmDeleted.Enabled = def.IsTriggerRegistered(WorkflowTriggerEventNamesEnum.farm_deleted);
                    chkTriggerSiteCreated.Enabled = def.IsTriggerRegistered(WorkflowTriggerEventNamesEnum.site_created);
                    chkTriggerSiteModified.Enabled = def.IsTriggerRegistered(WorkflowTriggerEventNamesEnum.site_modified);
                    chkTriggerSiteDeleted.Enabled = def.IsTriggerRegistered(WorkflowTriggerEventNamesEnum.site_deleted);
                    chkTriggerDirectoryCreated.Enabled = def.IsTriggerRegistered(WorkflowTriggerEventNamesEnum.directory_created);
                    chkTriggerDirectoryModified.Enabled = def.IsTriggerRegistered(WorkflowTriggerEventNamesEnum.directory_modified);
                    chkTriggerDirectoryDeleted.Enabled = def.IsTriggerRegistered(WorkflowTriggerEventNamesEnum.directory_deleted);
                    chkTriggerFileCreated.Enabled = def.IsTriggerRegistered(WorkflowTriggerEventNamesEnum.file_created);
                    chkTriggerFileModified.Enabled = def.IsTriggerRegistered(WorkflowTriggerEventNamesEnum.file_modified);
                    chkTriggerFileDeleted.Enabled = def.IsTriggerRegistered(WorkflowTriggerEventNamesEnum.file_deleted);

                    if ((tbAssociationScopeUrl.ReadOnly) && (tbAssociationScopeUrl.Text != ""))
                    {
                        ScopeEnum pathScope = ScopeEnum.Invalid;
                        PATH_INFO pathInfo = CSPath.GetPathInfo(tbAssociationScopeUrl.Text);
                        if (pathInfo.IsValid)
                        {
                            if ((pathInfo.ResourceURI != null) && (pathInfo.ResourceURI != "/"))
                            {
                                pathScope = ScopeEnum.Directory;
                            }
                            else if (pathInfo.SiteId != Guid.Empty)
                            {
                                pathScope = ScopeEnum.Site;
                            }
                            else
                            {
                                pathScope = ScopeEnum.Farm;
                            }
                        }

                        if ((pathScope != ScopeEnum.Invalid) && (! def.HasEventsForScope(pathScope)))
                        {
                            MessageBox.Show("This workflow has no events for this scope.");
                            cmbWorkflowDefinition.SelectedItem = cmbWorkflowDefinition.Items[cmbWorkflowDefinition.FindString("(Select a Workflow)")];
                        }
                    }

                    break;
                }
            }
        }

        private void btnFormClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lblLogoText_MouseMove(object sender, MouseEventArgs e)
        {
            UI.EnableBorderlessFormMove(this.Handle, e);
        }
    }
}
