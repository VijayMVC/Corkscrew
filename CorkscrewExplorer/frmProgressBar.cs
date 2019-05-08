using System.Windows.Forms;

namespace Corkscrew.Explorer
{
    public partial class frmProgressBar : Form
    {

        public string Status
        {
            get { return lblItemName.Text; }
            set
            {
                lblItemName.Text = value;
                Application.DoEvents();
            }
        }

        public int Progress
        {
            get { return pbProgress.Value; }
            set
            {
                int corrected = value;

                if (corrected < 0)
                {
                    corrected = 0;
                }

                if (corrected > 100)
                {
                    corrected = 100;
                }

                pbProgress.Value = corrected;
                Application.DoEvents();
            }
        }


        public frmProgressBar()
        {
            InitializeComponent();
        }
    }
}
