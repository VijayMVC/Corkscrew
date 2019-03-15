using CMS.Setup.Installers;
using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace CMS.Setup.Screens
{
    public partial class SelectInstallFolderScreen : ScreenTemplate
    {

        private InstallWizardForm _parent = null;


        public SelectInstallFolderScreen()
        {
            InitializeComponent();
        }

        private void tbInstallFolder_DoubleClick(object sender, EventArgs e)
        {
            // show dialog
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                fbd.Description = "Select the folder where Corkscrew components are to be installed";
                fbd.RootFolder = Environment.SpecialFolder.MyComputer;
                fbd.SelectedPath = _parent.SetupManifest.InstallBaseDirectory;
                fbd.ShowNewFolderButton = true;
                
                if (fbd.ShowDialog() != DialogResult.Cancel)
                {
                    tbInstallFolder.Text = fbd.SelectedPath;
                    _parent.SetupManifest.InstallBaseDirectory = fbd.SelectedPath;
                }
            }
        }

        public override void InitializeUI()
        {
            _parent = (InstallWizardForm)this.ParentForm;
            tbInstallFolder.Text = _parent.SetupManifest.InstallBaseDirectory;

            if (_parent.SetupManifest.Installers.Where(ci => (ci.OriginalStatus == Installers.LastActionState.Installed)).Count() > 0)
            {
                string previousPath = RegistryKeyAction.GetPreviousInstallationPath();
                if (string.IsNullOrEmpty(previousPath))
                {
                    previousPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Aquarius Operating Systems", "CMS");
                }
                tbInstallFolder.Text = previousPath;
                tbInstallFolder.Enabled = false;
            }
            else
            {
                tbInstallFolder.Enabled = true;
            }
        }

        private void SelectInstallFolderScreen_Load(object sender, EventArgs e)
        {
            
        }
    }
}
