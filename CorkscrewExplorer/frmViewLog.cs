using System;
using System.Data;
using System.Windows.Forms;

namespace Corkscrew.Explorer
{
    public partial class frmViewLog : Form
    {

        public DataTable LogData
        {
            get;
            set;
        }

        public frmViewLog()
        {
            InitializeComponent();
        }

        private void frmViewLog_Shown(object sender, EventArgs e)
        {
            dgvLogView.DataSource = LogData.DefaultView;
        }

        private void btnFormClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmViewLog_MouseDown(object sender, MouseEventArgs e)
        {
            UI.EnableBorderlessFormMove(this.Handle, e);
        }
    }
}
