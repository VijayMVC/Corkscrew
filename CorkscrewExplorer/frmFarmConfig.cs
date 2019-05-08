using Corkscrew.SDK.objects;
using Corkscrew.SDK.tools;
using System;
using System.Data;
using System.Windows.Forms;

namespace Corkscrew.Explorer
{
    public partial class frmFarmConfig : Form
    {

        private DataTable dtConfigTable = new DataTable();

        #region Properties

        /// <summary>
        /// The farm
        /// </summary>
        public CSFarm Farm
        {
            get;
            set;
        } = null;

        #endregion

        public frmFarmConfig()
        {
            InitializeComponent();
        }

        private void frmFarmConfig_Shown(object sender, EventArgs e)
        {
            InitGrid();
        }

        private void InitGrid()
        {
            if (dtConfigTable.Columns.Count > 0)
            {
                dgvConfiguration.DataSource = null;
                dtConfigTable = new DataTable();
            }

            dtConfigTable.Columns.Add("Name", typeof(string));
            dtConfigTable.Columns.Add("Value", typeof(string));
            dtConfigTable.PrimaryKey = new DataColumn[] { dtConfigTable.Columns["Name"] };
            dgvConfiguration.DataSource = dtConfigTable;

            Farm = CSFarm.Open(Farm.AuthenticatedUser);                 // create a fresh instance so that the Config is reloaded
            foreach (CSKeyValuePair pair in Farm.AllConfiguration)
            {
                AddRowToDataTable(pair.Key, pair.Value);
            }
        }

        private void AddRowToDataTable(string name, string value)
        {
            DataRow row = dtConfigTable.Rows.Find(name);
            if (row == null)
            {
                row = dtConfigTable.NewRow();
                row["Name"] = name;
                row["Value"] = value;
                dtConfigTable.Rows.Add(row);
            }
            else
            {
                row["Name"] = name;
                row["Value"] = value;
            }
        }

        private void dgvConfiguration_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            // dont allow editing "Name" column
            if ((e.ColumnIndex == 0) && (! dgvConfiguration.Rows[e.RowIndex].IsNewRow))
            {
                e.Cancel = true;
            }
        }

        private void dgvConfiguration_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                string name = Utility.SafeString(dgvConfiguration.Rows[e.RowIndex].Cells[0].Value);
                string value = Utility.SafeString(dgvConfiguration.Rows[e.RowIndex].Cells[1].Value);

                if (Farm.AllConfiguration.IndexOf(name) == -1)
                {
                    Farm.AllConfiguration.Add(name, value);
                }
                else
                {
                    Farm.AllConfiguration.Update(name, value);
                }

                InitGrid();
            }
        }

        private void dgvConfiguration_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            string name = Utility.SafeString(e.Row.Cells[0].Value);
            e.Cancel = true;                                            // not having this causes an IndexOutOfBounds exception

            Farm.AllConfiguration.Remove(name);
            InitGrid();
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
