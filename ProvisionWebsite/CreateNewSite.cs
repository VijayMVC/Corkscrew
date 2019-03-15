using System.Windows.Forms;

namespace Corkscrew.Tools.ProvisionWebsite
{
    public partial class CreateNewSite : Form
    {
        public CreateNewSite()
        {
            InitializeComponent();
        }

        private void lblLogoText_MouseDown(object sender, MouseEventArgs e)
        {
            UI.EnableBorderlessMouseMove(this, e);
        }

        private void CancelButton_Click(object sender, System.EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void CreateButton_Click(object sender, System.EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }


        public string EnteredSiteName
        {
            get
            {
                return SiteName.Text;
            }
        }

        public string EnteredSiteDescription
        {
            get { return SiteDescription.Text; }
        }

    }
}
