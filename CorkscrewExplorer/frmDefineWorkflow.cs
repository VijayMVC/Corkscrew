

using Corkscrew.SDK.objects;
using Corkscrew.SDK.tools;
using Corkscrew.SDK.workflow;
using System;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace Corkscrew.Explorer
{
    public partial class frmDefineWorkflow : Form
    {

        /// <summary>
        /// The farm
        /// </summary>
        public CSFarm Farm
        {
            get;
            set;
        }

        /// <summary>
        /// The workflow definition
        /// </summary>
        public CSWorkflowDefinition WorkflowDefinition
        {
            get;
            set;
        }


        public frmDefineWorkflow()
        {
            InitializeComponent();
        }

        private void frmDefineWorkflow_Shown(object sender, EventArgs e)
        {
            if (WorkflowDefinition != null)
            {
                tbId.Text = WorkflowDefinition.Id.ToString("D");
                tbName.Text = WorkflowDefinition.Name;
                tbDescription.Text = WorkflowDefinition.Description;
                tbDefaultAssociationData.Text = WorkflowDefinition.DefaultAssociationData;

                cbAllowStartOnCreate.Checked = WorkflowDefinition.StartOnCreate;
                cbAllowStartOnModify.Checked = WorkflowDefinition.StartOnModify;
                cbEnabled.Checked = WorkflowDefinition.IsEnabled;

                chkTriggersAllowBubbledEvents.Checked = WorkflowDefinition.AllowProcessingBubbledTriggers;

                chkTriggerFarmCreated.Checked = WorkflowDefinition.IsTriggerRegistered(WorkflowTriggerEventNamesEnum.farm_created);
                chkTriggerFarmModified.Checked = WorkflowDefinition.IsTriggerRegistered(WorkflowTriggerEventNamesEnum.farm_modified);
                chkTriggerFarmDeleted.Checked = WorkflowDefinition.IsTriggerRegistered(WorkflowTriggerEventNamesEnum.farm_deleted);
                chkTriggerSiteCreated.Checked = WorkflowDefinition.IsTriggerRegistered(WorkflowTriggerEventNamesEnum.site_created);
                chkTriggerSiteModified.Checked = WorkflowDefinition.IsTriggerRegistered(WorkflowTriggerEventNamesEnum.site_modified);
                chkTriggerSiteDeleted.Checked = WorkflowDefinition.IsTriggerRegistered(WorkflowTriggerEventNamesEnum.site_deleted);
                chkTriggerDirectoryCreated.Checked = WorkflowDefinition.IsTriggerRegistered(WorkflowTriggerEventNamesEnum.directory_created);
                chkTriggerDirectoryModified.Checked = WorkflowDefinition.IsTriggerRegistered(WorkflowTriggerEventNamesEnum.directory_modified);
                chkTriggerDirectoryDeleted.Checked = WorkflowDefinition.IsTriggerRegistered(WorkflowTriggerEventNamesEnum.directory_deleted);
                chkTriggerFileCreated.Checked = WorkflowDefinition.IsTriggerRegistered(WorkflowTriggerEventNamesEnum.file_created);
                chkTriggerFileModified.Checked = WorkflowDefinition.IsTriggerRegistered(WorkflowTriggerEventNamesEnum.file_modified);
                chkTriggerFileDeleted.Checked = WorkflowDefinition.IsTriggerRegistered(WorkflowTriggerEventNamesEnum.file_deleted);

                lblCreated.Text = WorkflowDefinition.Created.ToString("MMM dd, yyyy HH:mm:ss");
                lblCreatedBy.Text = WorkflowDefinition.CreatedBy.Username;
                lblModified.Text = WorkflowDefinition.Modified.ToString("MMM dd, yyyy HH:mm:ss");
                lblModifiedBy.Text = WorkflowDefinition.ModifiedBy.Username;
            }
            else
            {
                tbId.Text = Guid.NewGuid().ToString("D");
                cbAllowStartOnCreate.Checked = true;
                cbAllowStartOnModify.Checked = true;
                cbEnabled.Checked = true;

                lblCreated.Text = DateTime.Now.ToString("MMM dd, yyyy HH:mm:ss");
                lblCreatedBy.Text = Farm.AuthenticatedUser.Username;
                lblModified.Text = lblCreated.Text;
                lblModifiedBy.Text = lblCreatedBy.Text;
            }

            tbName.Focus();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!cbAllowStartOnCreate.Checked && !cbAllowStartOnModify.Checked)
            {
                MessageBox.Show("No start method specified for the workflow definition. Select one or more options.");
                cbAllowStartOnCreate.Focus();
                return;
            }

            if ((!cbEnabled.Checked) && (MessageBox.Show("You have not checked the \"Workflow is enabled\" checkbox. Workflow will be DISABLED. Is this what you want to do?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No))
            {
                cbEnabled.Focus();
                return;
            }

            if (string.IsNullOrEmpty(tbName.Text))
            {
                MessageBox.Show("Workflow name must be specified.");
                tbName.Focus();
                return;
            }

            if (WorkflowDefinition == null)
            {
                WorkflowDefinition = Farm.AllWorkflowDefinitions.Add(tbName.Text, tbDescription.Text, tbDefaultAssociationData.Text, cbAllowStartOnCreate.Checked, cbAllowStartOnModify.Checked, false);

                if (WorkflowDefinition == null)
                {
                    MessageBox.Show("Could not create manifest.");
                    return;
                }

                tbId.Text = WorkflowDefinition.Id.ToString("D");
            }
            else
            {
                WorkflowDefinition.Name = tbName.Text;
                WorkflowDefinition.Description = tbDescription.Text;
                WorkflowDefinition.DefaultAssociationData = tbDefaultAssociationData.Text;

                WorkflowDefinition.AllowManualStart = false;
                WorkflowDefinition.StartOnCreate = cbAllowStartOnCreate.Checked;
                WorkflowDefinition.StartOnModify = cbAllowStartOnModify.Checked;
            }

            if (!cbEnabled.Checked)
            {
                WorkflowDefinition.Disable();
            }

            WorkflowDefinition.AllowProcessingBubbledTriggers = chkTriggersAllowBubbledEvents.Checked;
            WorkflowDefinition.Save();

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
                            if (!WorkflowDefinition.IsTriggerRegistered(trigger))
                            {
                                WorkflowDefinition.RegisterTrigger(trigger);
                            }
                        }
                        else
                        {
                            if (WorkflowDefinition.IsTriggerRegistered(trigger))
                            {
                                WorkflowDefinition.DeregisterTrigger(trigger);
                            }
                        }
                    }
                }
            }

            lblCreated.Text = WorkflowDefinition.Created.ToString("MMM dd, yyyy HH:mm:ss");
            lblCreatedBy.Text = WorkflowDefinition.CreatedBy.LongformDisplayName;
            lblModified.Text = WorkflowDefinition.Modified.ToString("MMM dd, yyyy HH:mm:ss");
            lblModifiedBy.Text = WorkflowDefinition.ModifiedBy.LongformDisplayName;
        }

        private void btnDefineManifest_Click(Object sender, EventArgs e)
        {
            if (this.WorkflowDefinition == null)
            {
                MessageBox.Show("Save the manifest before clicking this button.");
                return;
            }

            if (this.WorkflowDefinition.GetManifest() == null)
            {
                this.WorkflowDefinition.CreateManifest(WorkflowEngineEnum.CS1C, WorkflowDefinition.Name.RemoveNonAlphanumericCharacters() + ".dll", "Workflow1.cs", false, true);
            }

            using (frmDefineWorkflowSetManifest frm = new frmDefineWorkflowSetManifest())
            {
                frm.WorkflowDefinition = this.WorkflowDefinition;
                frm.ShowDialog(this);
            }
        }

        private void btnInstallFromManifest_Click(Object sender, EventArgs e)
        {
            string manifestXmlPath = null;
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.InitialDirectory = Application.StartupPath;
                ofd.AddExtension = true;
                ofd.CheckFileExists = true;
                ofd.CheckPathExists = true;
                ofd.DefaultExt = ".xml";
                ofd.DereferenceLinks = true;
                ofd.Filter = "Install Manifest Files (*.xml)|*.xml|All Files (*.*)|*.*";
                ofd.FilterIndex = 0;
                ofd.Multiselect = false;
                ofd.ShowReadOnly = true;
                ofd.SupportMultiDottedExtensions = true;
                ofd.Title = "Select manifest file to import from...";
                ofd.ValidateNames = true;

                if (ofd.ShowDialog(this) == DialogResult.Cancel)
                {
                    return;
                }

                manifestXmlPath = ofd.SafeFileName;
            }

            if (!File.Exists(manifestXmlPath))
            {
                MessageBox.Show("File does not exist at: " + manifestXmlPath);
            }

            using (frmProgressBar progress = new frmProgressBar())
            {
                progress.Progress = 10;
                progress.Status = "Loading manifest...";

                XmlDocument cfg = new XmlDocument();
                cfg.Load(manifestXmlPath);

                foreach (XmlElement definitionElement in cfg.DocumentElement.GetElementsByTagName("WorkflowDefinition"))
                {
                    string workflowDefinitionName = null;
                    CSWorkflowDefinition definition = null;

                    try
                    {
                        workflowDefinitionName = definitionElement.Attributes["name"].Value;
                        progress.Progress += 10;
                        progress.Status = "Installing workflow: " + workflowDefinitionName;

                        definition = Farm.AllWorkflowDefinitions.Find(workflowDefinitionName);
                        if (definition != null)
                        {
                            if (MessageBox.Show("Workflow definition [" + workflowDefinitionName + "] already exists. Do you wish to drop it before continuing [Y/n]?", "Install Workflow", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                            {
                                continue;
                            }

                            progress.Progress += 10;
                            progress.Status = "Deleting definition for workflow: " + workflowDefinitionName;
                            definition.Delete();
                        }

                        definition = Farm.AllWorkflowDefinitions.Add
                        (
                            workflowDefinitionName,
                            Utility.SafeString(definitionElement.Attributes["description"].Value, ""),
                            Utility.SafeString(definitionElement.Attributes["defaultAssociationData"].Value, ""),
                            Utility.SafeConvertToBool(definitionElement.Attributes["startOnCreateNewItem"].Value),
                            Utility.SafeConvertToBool(definitionElement.Attributes["startOnModifyItem"].Value),
                            Utility.SafeConvertToBool(definitionElement.Attributes["allowStartWorkflowManually"].Value)
                        );

                        progress.Progress += 10;
                        progress.Status = "Installing workflow: " + workflowDefinitionName + " - Created.";

                        if (definition == null)
                        {
                            MessageBox.Show("Workflow definition [" + workflowDefinitionName + "] could not be created.");
                            continue;
                        }

                        XmlNodeList eventsElements = definitionElement.GetElementsByTagName("Events");
                        if (eventsElements.Count > 0)
                        {
                            XmlNode eventsElement = eventsElements[0];
                            foreach (XmlNode eventItemNode in eventsElement.ChildNodes)
                            {
                                WorkflowTriggerEventNamesEnum triggerName = WorkflowTriggerEventNamesEnum.None;
                                if (Enum.TryParse<WorkflowTriggerEventNamesEnum>(eventItemNode.Attributes["name"].Value, out triggerName))
                                {
                                    definition.RegisterTrigger(triggerName);
                                }
                            }
                        }

                        XmlNodeList manifestElements = definitionElement.GetElementsByTagName("Manifest");
                        if (manifestElements.Count == 0)
                        {
                            MessageBox.Show("Workflow manifest entry for [" + workflowDefinitionName + "] not found. Please fix and try again.");
                            definition.Delete();
                            continue;
                        }

                        XmlNode manifestElement = manifestElements[0];

                        progress.Progress += 10;
                        progress.Status = "Creating workflow manifest...";

                        CSWorkflowManifest manifest = definition.CreateManifest
                        (
                            (WorkflowEngineEnum)Enum.Parse(typeof(WorkflowEngineEnum), manifestElement.Attributes["engine"].Value),
                            Utility.SafeString(manifestElement.Attributes["assemblyName"].Value, Guid.NewGuid().ToString("n") + ".dll"),
                            Utility.SafeString(manifestElement.Attributes["className"].Value, "CorkscrewWorkflow"),
                            Utility.SafeConvertToBool(manifestElement.Attributes["alwaysCompile"].Value),
                            Utility.SafeConvertToBool(manifestElement.Attributes["cacheCompileResults"].Value)
                        );

                        if (manifest == null)
                        {
                            MessageBox.Show("Could not create manifest for [" + workflowDefinitionName + "].");
                            definition.Delete();
                            continue;
                        }

                        foreach (XmlNode manifestNodeChildElement in manifestElement.ChildNodes)
                        {
                            if (manifestNodeChildElement.Name.Equals("Build"))
                            {
                                manifest.BuildAssemblyCompany = Utility.SafeString(manifestNodeChildElement.Attributes["company"].Value, null);
                                manifest.BuildAssemblyCopyright = Utility.SafeString(manifestNodeChildElement.Attributes["copyright"].Value, null);
                                manifest.BuildAssemblyDescription = Utility.SafeString(manifestNodeChildElement.Attributes["description"].Value, null);
                                manifest.BuildAssemblyFileVersion = new Version(Utility.SafeString(manifestNodeChildElement.Attributes["fileversion"].Value, "1.0.0.0"));
                                manifest.BuildAssemblyProduct = Utility.SafeString(manifestNodeChildElement.Attributes["product"].Value, null);
                                manifest.BuildAssemblyTitle = Utility.SafeString(manifestNodeChildElement.Attributes["name"].Value, null);
                                manifest.BuildAssemblyTrademark = Utility.SafeString(manifestNodeChildElement.Attributes["trademark"].Value, null);
                                manifest.BuildAssemblyVersion = new Version(Utility.SafeString(manifestNodeChildElement.Attributes["version"].Value, "1.0.0.0"));
                            }

                            if (manifestNodeChildElement.Name.Equals("ManifestItems"))
                            {
                                foreach (XmlNode manifestItemElement in manifestNodeChildElement.ChildNodes)
                                {
                                    if (manifestItemElement.Name.Equals("Item"))
                                    {
                                        string fileName = manifestItemElement.Attributes["name"].Value;
                                        string itemResourceFilePath = fileName;                         // this could be an absolute path
                                        WorkflowManifestItemTypeEnum itemType = WorkflowManifestItemTypeEnum.Unknown;
                                        if (!Enum.TryParse<WorkflowManifestItemTypeEnum>(manifestItemElement.Attributes["type"].Value, out itemType))
                                        {
                                            MessageBox.Show("Manifest item " + fileName + " has an invalid type configured. Skipping this item. You may add this item manually through another tool.");
                                            continue;
                                        }

                                        if (!File.Exists(itemResourceFilePath))
                                        {
                                            itemResourceFilePath = Path.Combine(Path.GetDirectoryName(manifestXmlPath), fileName);
                                            if (!File.Exists(itemResourceFilePath))
                                            {
                                                // primary assembly is allowed to not exist.. it will be compiled.
                                                if (itemType != WorkflowManifestItemTypeEnum.PrimaryAssembly)
                                                {
                                                    MessageBox.Show("Manifest file item for [" + workflowDefinitionName + "] with name [" + fileName + "] was not found. Please fix and try again.");
                                                    definition.Delete();    // will also clear the manifest and all items created thus far

                                                    continue;
                                                }
                                            }
                                        }

                                        Console.WriteLine("Creating workflow manifest item... " + fileName);
                                        byte[] buffer = null;

                                        if (File.Exists(itemResourceFilePath))
                                        {
                                            buffer = File.ReadAllBytes(itemResourceFilePath);
                                        }

                                        manifest.AddItem(
                                            Path.GetFileNameWithoutExtension(fileName),
                                            Path.GetExtension(fileName),
                                            (WorkflowManifestItemTypeEnum)Enum.Parse(typeof(WorkflowManifestItemTypeEnum), manifestItemElement.Attributes["type"].Value),
                                            Utility.SafeConvertToBool(manifestItemElement.Attributes["requiredForExecution"].Value),
                                            buffer,
                                            Utility.SafeString(manifestItemElement.Attributes["buildRelativeFolder"].Value, null),
                                            Utility.SafeString(manifestItemElement.Attributes["runtimeRelativeFolder"].Value, null)
                                        );
                                    }
                                }
                            }
                        }

                    }
                    catch
                    {
                        if (definition != null)
                        {
                            definition.Delete();
                        }
                    }

                    progress.Progress += 10;
                    progress.Status = "Workflow definition [" + workflowDefinitionName + "] created.";
                }
            }

            MessageBox.Show("One or more workflow definitions were installed. Close this window to refresh the Explorer view.");
        }

        private void frmDefineWorkflow_MouseDown(object sender, MouseEventArgs e)
        {
            UI.EnableBorderlessFormMove(this.Handle, e);
        }

        private void btnFormClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lblLogoText_MouseDown(object sender, MouseEventArgs e)
        {
            UI.EnableBorderlessFormMove(this.Handle, e);
        }
    }
}
