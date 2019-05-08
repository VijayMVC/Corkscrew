using Corkscrew.SDK.security;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Corkscrew.Explorer
{
    public partial class frmChangeGroupMembership : Form
    {

        public CSUserGroup UserGroup
        {
            get;
            set;
        }

        public frmChangeGroupMembership()
        {
            InitializeComponent();
        }

        private void btnFormClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lblLogoText_MouseDown(object sender, MouseEventArgs e)
        {
            UI.EnableBorderlessFormMove(this.Handle, e);
        }

        private void frmChangeGroupMembership_Shown(object sender, EventArgs e)
        {
            lblGroupname.Text = UserGroup.LongformDisplayName;
            BindListboxes();
        }

        private void btnAddToGroup_Click(object sender, EventArgs e)
        {
            UseWaitCursor = true;
            Cursor = Cursors.WaitCursor;

            foreach(CSUser user in lbAvailableUsers.SelectedItems)
            {
                UserGroup.Add(user);
            }

            BindListboxes();

            UseWaitCursor = false;
            Cursor = Cursors.Default;
        }

        private void btnRemoveFromGroup_Click(object sender, EventArgs e)
        {
            UseWaitCursor = true;
            Cursor = Cursors.WaitCursor;

            foreach (CSUser user in lbGroupMembers.SelectedItems)
            {
                UserGroup.Remove(user);
            }

            BindListboxes();

            UseWaitCursor = false;
            Cursor = Cursors.Default;
        }

        private void BindListboxes()
        {
            lbAvailableUsers.Items.Clear();
            lbAvailableUsers.DisplayMember = "Username";
            lbAvailableUsers.ValueMember = "Id";
            lbAvailableUsers.DataSource = UserGroup.GetNonMembers().OrderBy(u => u.LongformDisplayName);

            lbGroupMembers.Items.Clear();
            lbGroupMembers.DisplayMember = "Username";
            lbGroupMembers.ValueMember = "Id";
            lbGroupMembers.DataSource = UserGroup.Members.OrderBy(u => u.LongformDisplayName);

            lbAvailableUsers.Focus();
        }
    }
}
