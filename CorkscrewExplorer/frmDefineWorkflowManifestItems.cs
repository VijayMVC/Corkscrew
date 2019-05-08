using Corkscrew.SDK.tools;
using Corkscrew.SDK.workflow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Corkscrew.Explorer
{
    public partial class frmDefineWorkflowManifestItems : Form
    {

        public CSWorkflowManifest Manifest
        {
            get;
            set;
        }


        public frmDefineWorkflowManifestItems()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(Object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmDefineWorkflowManifestItems_Shown(Object sender, EventArgs e)
        {
            ReloadManifestItems();
        }

        private void btnRemoveManifestItem_Click(Object sender, EventArgs e)
        {
            IReadOnlyList<CSWorkflowManifestItem> items = Manifest.GetItems();
            foreach (ListViewItem lvItem in lvManifestFiles.SelectedItems)
            {
                Guid itemId = Utility.SafeConvertToGuid(lvItem.Tag);
                if (itemId != Guid.Empty)
                {
                    CSWorkflowManifestItem item = items.Where(i => i.Id.Equals(itemId)).FirstOrDefault();
                    if (item != default(CSWorkflowManifestItem))
                    {
                        Manifest.RemoveItem(item);
                    }
                }
            }

            ReloadManifestItems();
        }

        private void btnEditManifestItem_Click(Object sender, EventArgs e)
        {
            if (lvManifestFiles.SelectedIndices.Count > 0)
            {
                ListViewItem lvItem = lvManifestFiles.SelectedItems[0];     // multi-select is OFF
                Guid itemId = Utility.SafeConvertToGuid(lvItem.Tag);
                if (itemId != Guid.Empty)
                {
                    CSWorkflowManifestItem item = Manifest.GetItems().Where(i => i.Id.Equals(itemId)).FirstOrDefault();
                    if (item != default(CSWorkflowManifestItem))
                    {
                        using (frmCreateEditManifestItem frm = new frmCreateEditManifestItem())
                        {
                            frm.Manifest = this.Manifest;
                            frm.ManifestItem = item;

                            frm.ShowDialog(this);

                            ReloadManifestItems();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Could not find manifest item.");
                        ReloadManifestItems();
                    }
                }
            }
        }

        private void btnAddManifestFile_Click(Object sender, EventArgs e)
        {
            using (frmCreateEditManifestItem frm = new frmCreateEditManifestItem())
            {
                frm.Manifest = Manifest;
                frm.ManifestItem = null;
                frm.ShowDialog(this);
            }

            ReloadManifestItems();
        }

        private void lvManifestFiles_SelectedIndexChanged(Object sender, EventArgs e)
        {
            btnEditManifestItem.Enabled = (lvManifestFiles.SelectedIndices.Count > 0);
            btnRemoveManifestItem.Enabled = (lvManifestFiles.SelectedIndices.Count > 0);
        }

        private void ReloadManifestItems()
        {
            if (lvManifestFiles.Columns.Count == 0)
            {
                lvManifestFiles.Columns.Add("Filename", 80, HorizontalAlignment.Left);
                lvManifestFiles.Columns.Add("Type", 80, HorizontalAlignment.Left);
                lvManifestFiles.Columns.Add("Last modified", 80, HorizontalAlignment.Right);
            }

            lvManifestFiles.Items.Clear();

            if (Manifest != null)
            {
                foreach (CSWorkflowManifestItem item in Manifest.GetItems())
                {
                    ListViewItem lvItem = new ListViewItem(item.FilenameWithExtension);
                    lvItem.Tag = item.Id;
                    lvItem.SubItems.Add(Enum.GetName(typeof(WorkflowManifestItemTypeEnum), item.ItemType));
                    lvItem.SubItems.Add(item.Modified.ToString("MMM dd, yyyy HH:mm:ss"));

                    lvManifestFiles.Items.Add(lvItem);
                }
            }

            btnRemoveManifestItem.Enabled = false;
            btnEditManifestItem.Enabled = false;
        }

        private void frmDefineWorkflowManifestItems_MouseDown(object sender, MouseEventArgs e)
        {
            UI.EnableBorderlessFormMove(this.Handle, e);
        }

        private void btnFormClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
