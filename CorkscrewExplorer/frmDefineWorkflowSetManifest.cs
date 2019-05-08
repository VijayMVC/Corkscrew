using Corkscrew.SDK.tools;
using Corkscrew.SDK.workflow;
using System;
using System.Windows.Forms;

namespace Corkscrew.Explorer
{
    public partial class frmDefineWorkflowSetManifest : Form
    {

        /// <summary>
        /// The workflow definition
        /// </summary>
        public CSWorkflowDefinition WorkflowDefinition
        {
            get { return _definition; }
            set
            {
                _definition = value;
                if (_definition == null)
                {
                    Manifest = null;
                }
                else
                {
                    Manifest = _definition.GetManifest();
                    if (Manifest == null)
                    {
                        Manifest = _definition.CreateManifest(WorkflowEngineEnum.CS1C, _definition.Name.RemoveNonAlphanumericCharacters() + ".dll", "Workflow1.cs", false, true);
                    }
                }
            }
        }
        private CSWorkflowDefinition _definition = null;

        private CSWorkflowManifest Manifest
        {
            get;
            set;
        }

        public frmDefineWorkflowSetManifest()
        {
            InitializeComponent();
        }

        private void frmDefineWorkflowSetManifest_Shown(Object sender, EventArgs e)
        {
            lblCompiled.Text = Manifest.LastCompiled.ToString("MMM dd, yyyy HH:mm:ss");
            rbAlwaysCompile.Checked = Manifest.AlwaysCompile;
            rbAlwaysCache.Checked = Manifest.CacheCompileResults;
            tbWorkflowClassName.Text = Manifest.WorkflowClassName;


            string engineFriendlyName = "Corkscrew Coded Workflow";
            switch (Manifest.WorkflowEngine)
            {
                case WorkflowEngineEnum.CS1C: engineFriendlyName = "Corkscrew Coded Workflow"; break;
                case WorkflowEngineEnum.WF4C: engineFriendlyName = "Workflow v4.0 Coded Workflow"; break;
                case WorkflowEngineEnum.WF4X: engineFriendlyName = "Workflow v4.0 Xaml Workflow"; break;
                case WorkflowEngineEnum.WF3C: engineFriendlyName = "Workflow v3.0 Coded Workflow"; break;
                case WorkflowEngineEnum.WF3X: engineFriendlyName = "Workflow v3.0 Xaml Workflow"; break;
            }
            cbWorkflowEngine.SelectedItem = cbWorkflowEngine.Items[cbWorkflowEngine.FindString(engineFriendlyName)];

            tbBuildAssemblyFilename.Text = Manifest.OutputAssemblyName;

            tbBuildProductName.Text = Manifest.BuildAssemblyProduct;
            tbBuildProductTitle.Text = Manifest.BuildAssemblyTitle;
            tbBuildProductDescription.Text = Manifest.BuildAssemblyDescription;
            tbBuildProductCompany.Text = Manifest.BuildAssemblyCompany;
            tbBuildProductCopyright.Text = Manifest.BuildAssemblyCopyright;
            tbBuildProductTrademark.Text = Manifest.BuildAssemblyTrademark;
            tbBuildProductVersion.Text = Manifest.BuildAssemblyVersion.ToString(4);
            tbBuildProductFileVersion.Text = Manifest.BuildAssemblyFileVersion.ToString(4);

        }

        private void btnCancel_Click(Object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(Object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbWorkflowClassName.Text))
            {
                MessageBox.Show("Workflow classname must be specified.");
                tbWorkflowClassName.Focus();
                return;
            }

            if (string.IsNullOrEmpty(tbBuildAssemblyFilename.Text))
            {
                MessageBox.Show("Output assembly filename must be specified.");
                tbBuildAssemblyFilename.Focus();
                return;
            }

            Version test = null;
            if (!Version.TryParse(tbBuildProductVersion.Text, out test))
            {
                MessageBox.Show("Build version is not valid.");
                tbBuildProductVersion.Focus();
                return;
            }
            if (!Version.TryParse(tbBuildProductFileVersion.Text, out test))
            {
                MessageBox.Show("Build file-version is not valid.");
                tbBuildProductFileVersion.Focus();
                return;
            }

            WorkflowEngineEnum engine = WorkflowEngineEnum.CS1C;
            switch (cbWorkflowEngine.SelectedItem.ToString())
            {
                case "Workflow v4.0 Coded Workflow": engine = WorkflowEngineEnum.WF4C; break;
                case "Workflow v4.0 Xaml Workflow": engine = WorkflowEngineEnum.WF4X; break;
                case "Workflow v3.0 Coded Workflow": engine = WorkflowEngineEnum.WF3C; break;
                case "Workflow v3.0 Xaml Workflow": engine = WorkflowEngineEnum.WF3X; break;

                default:
                    engine = WorkflowEngineEnum.CS1C;
                    break;
            }

            Manifest.WorkflowEngine = engine;
            Manifest.OutputAssemblyName = tbBuildAssemblyFilename.Text;
            Manifest.WorkflowClassName = tbWorkflowClassName.Text;
            Manifest.AlwaysCompile = rbAlwaysCompile.Checked;
            Manifest.CacheCompileResults = rbAlwaysCache.Checked;

            Manifest.BuildAssemblyProduct = tbBuildProductName.Text;
            Manifest.BuildAssemblyTitle = tbBuildProductTitle.Text;
            Manifest.BuildAssemblyDescription = tbBuildProductDescription.Text;
            Manifest.BuildAssemblyCompany = tbBuildProductCompany.Text;
            Manifest.BuildAssemblyCopyright = tbBuildProductCopyright.Text;
            Manifest.BuildAssemblyTrademark = tbBuildProductTrademark.Text;
            Manifest.BuildAssemblyVersion = Version.Parse(tbBuildProductVersion.Text);
            Manifest.BuildAssemblyFileVersion = Version.Parse(tbBuildProductFileVersion.Text);

            Manifest.Save();
        }

        private void btnDefineManifestItems_Click(Object sender, EventArgs e)
        {
            using (frmDefineWorkflowManifestItems frm = new frmDefineWorkflowManifestItems())
            {
                frm.Manifest = this.Manifest;
                frm.ShowDialog(this);
            }
        }

        private void frmDefineWorkflowSetManifest_MouseDown(object sender, MouseEventArgs e)
        {
            UI.EnableBorderlessFormMove(this.Handle, e);
        }

        private void btnFormClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
