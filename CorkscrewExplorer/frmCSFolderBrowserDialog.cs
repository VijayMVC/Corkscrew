using Corkscrew.SDK.objects;
using System;
using System.Windows.Forms;

namespace Corkscrew.Explorer
{
    public partial class frmCSFolderBrowserDialog : Form
    {

        public CSSite BrowseSite
        {
            get;
            set;
        }

        public string SelectedPath
        {
            get;
            set;
        }


        public frmCSFolderBrowserDialog()
        {
            InitializeComponent();
        }

        private void frmCSFolderBrowserDialog_Shown(object sender, EventArgs e)
        {
            TreeNode rootNode = new TreeNode("/");
            rootNode.ImageIndex = 0;
            rootNode.Tag = BrowseSite.RootFolder.FullPath;
            tvFolders.Nodes.Add(rootNode);

            LoadDirectory(rootNode, BrowseSite.RootFolder.FullPath);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (tvFolders.SelectedNode != null)
            {
                SelectedPath = tvFolders.SelectedNode.Tag.ToString();
            }
            else
            {
                SelectedPath = null;
                DialogResult = DialogResult.Cancel;
            }

            this.Close();
        }


        private void LoadDirectory(TreeNode parentNode, string path)
        {
            if (parentNode.Nodes.Count == 0)
            {
                CSFileSystemEntryDirectory folder = new CSFileSystemEntryDirectory(CSFileSystemEntry.GetItemInfo(BrowseSite, path));
                foreach (CSFileSystemEntryDirectory entry in folder.Directories)
                {
                    if (entry.IsHidden)
                    {
                        continue;
                    }

                    TreeNode item = new TreeNode(entry.FilenameWithExtension);
                    item.Tag = entry.FullPath;
                    item.ImageIndex = 0;

                    parentNode.Nodes.Add(item);
                }
            }
        }

        private void tvFolders_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node.Nodes.Count == 0)
            {
                LoadDirectory(e.Node, e.Node.Tag.ToString());
            }
        }

        private void btnFormClose_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void lblLogoText_MouseDown(object sender, MouseEventArgs e)
        {
            UI.EnableBorderlessFormMove(this.Handle, e);
        }

        private void tvFolders_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node != null)
            {
                lblSelectedFolderPath.Text = e.Node.Tag.ToString();
            }
        }
    }
}
