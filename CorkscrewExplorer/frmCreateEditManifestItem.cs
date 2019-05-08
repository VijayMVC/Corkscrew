using Corkscrew.SDK.tools;
using Corkscrew.SDK.workflow;
using System;
using System.IO;
using System.Windows.Forms;

namespace Corkscrew.Explorer
{
    public partial class frmCreateEditManifestItem : Form
    {

        /// <summary>
        /// The manifest
        /// </summary>
        public CSWorkflowManifest Manifest
        {
            get;
            set;
        }

        /// <summary>
        /// The manifest item
        /// </summary>
        public CSWorkflowManifestItem ManifestItem
        {
            get;
            set;
        }


        public frmCreateEditManifestItem()
        {
            InitializeComponent();
        }

        private void btnSelectUploadFile_Click(object sender, EventArgs e)
        {
            if (ofdSelectFile.ShowDialog() == DialogResult.OK)
            {
                lblSelectedFilePath.Text = ofdSelectFile.SafeFileName;
                tbFilename.Text = Path.GetFileNameWithoutExtension(ofdSelectFile.SafeFileName);
                tbFilenameExtension.Text = Path.GetExtension(ofdSelectFile.SafeFileName);

                // guess the item type given the extension
                WorkflowManifestItemTypeEnum itemType = WorkflowManifestItemTypeEnum.CustomDataFile;
                switch (tbFilenameExtension.Text)
                {
                    case ".dll":
                        itemType = WorkflowManifestItemTypeEnum.DependencyAssembly;
                        break;

                    case ".cs":
                    case ".vb":
                        itemType = WorkflowManifestItemTypeEnum.SourceCodeFile;
                        break;

                    case ".xaml":
                    case ".xamlx":
                    case ".xoml":
                        itemType = WorkflowManifestItemTypeEnum.XamlFile;
                        break;

                    case ".config":
                    case ".ini":
                    case ".inf":
                    case ".settings":
                        itemType = WorkflowManifestItemTypeEnum.ConfigurationFile;
                        break;

                    case ".jpg":
                    case ".png":
                    case ".ico":
                    case ".mp3":
                    case ".mp4":
                    case ".wav":
                    case ".mpeg":
                        itemType = WorkflowManifestItemTypeEnum.MediaResourceFile;
                        break;

                    case ".css":
                        itemType = WorkflowManifestItemTypeEnum.Stylesheet;
                        break;

                    case ".res":
                    case ".resx":
                    case ".resource":
                    case ".resources":
                        itemType = WorkflowManifestItemTypeEnum.ResourceFile;
                        break;
                }

                cbItemType.SelectedItem = cbItemType.Items[cbItemType.FindString(Enum.GetName(typeof(WorkflowManifestItemTypeEnum), itemType))];
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            if (cbItemType.SelectedItem == null)
            {
                MessageBox.Show("Item type must be specified");
                cbItemType.Focus();
                return;
            }

            WorkflowManifestItemTypeEnum itemType = (WorkflowManifestItemTypeEnum)Enum.Parse(typeof(WorkflowManifestItemTypeEnum), Utility.SafeString(cbItemType.SelectedItem, "Unknown"));
            if (itemType == WorkflowManifestItemTypeEnum.Unknown)
            {
                MessageBox.Show("Item type must be specified");
                cbItemType.Focus();
                return;
            }

            if (ManifestItem == null)
            {
                if ((string.IsNullOrEmpty(lblSelectedFilePath.Text)) && (itemType != WorkflowManifestItemTypeEnum.PrimaryAssembly))
                {
                    MessageBox.Show("A manifest file must be selected for this item type");
                    btnSelectUploadFile.Focus();
                    return;
                }
            }
            else
            {
                if ((string.IsNullOrEmpty(tbFilename.Text)) || (string.IsNullOrEmpty(tbFilenameExtension.Text)))
                {
                    MessageBox.Show("Filename and Filename extension cannot both be empty");
                    tbFilename.Focus();
                    return;
                }
            }

            if (!cbRequiredForRun.Checked)
            {
                tbRuntimeRelativeFolder.Text = "";
            }

            if ((!string.IsNullOrEmpty(tbBuildRelativePath.Text)) && (Path.IsPathRooted(tbBuildRelativePath.Text)))
            {
                MessageBox.Show("Cannot start build path with a \"/\". Remove the slash and continue.");
                tbBuildRelativePath.Focus();
                return;
            }

            if ((!string.IsNullOrEmpty(tbRuntimeRelativeFolder.Text)) && (Path.IsPathRooted(tbRuntimeRelativeFolder.Text)))
            {
                MessageBox.Show("Cannot start runtime path with a \"/\". Remove the slash and continue.");
                tbRuntimeRelativeFolder.Focus();
                return;
            }

            byte[] data = null;
            if (!string.IsNullOrEmpty(lblSelectedFilePath.Text))
            {
                data = File.ReadAllBytes(lblSelectedFilePath.Text);
            }
            else
            {
                if (ManifestItem != null)
                {
                    data = ManifestItem.FileContent;
                }
            }


            if (ManifestItem == null)
            {
                ManifestItem = Manifest.AddItem(tbFilename.Text, tbFilenameExtension.Text, itemType, cbRequiredForRun.Checked, data, tbBuildRelativePath.Text, tbRuntimeRelativeFolder.Text);
                tbId.Text = ManifestItem.Id.ToString("D");
            }
            else
            {
                ManifestItem.Filename = tbFilename.Text;
                ManifestItem.FilenameExtension = tbFilenameExtension.Text;
                ManifestItem.ItemType = itemType;
                ManifestItem.BuildtimeRelativeFolder = tbBuildRelativePath.Text;
                ManifestItem.RuntimeRelativeFolder = tbRuntimeRelativeFolder.Text;
                ManifestItem.FileContent = data;

                ManifestItem.Save();
            }

            lblCreated.Text = ManifestItem.Created.ToString("MMM dd, yyyy HH:mm:ss");
            lblCreatedBy.Text = ManifestItem.CreatedBy.Username;
            lblModified.Text = ManifestItem.Modified.ToString("MMM dd, yyyy HH:mm:ss");
            lblModifiedBy.Text = ManifestItem.ModifiedBy.Username;
        }

        private void frmCreateEditManifestItem_Shown(object sender, EventArgs e)
        {
            if (ManifestItem != null)
            {
                tbId.Text = ManifestItem.Id.ToString("D");
                tbFilename.Text = ManifestItem.Filename;
                tbFilenameExtension.Text = ManifestItem.FilenameExtension;
                cbItemType.SelectedItem = cbItemType.Items[cbItemType.FindString(Enum.GetName(typeof(WorkflowManifestItemTypeEnum), ManifestItem.ItemType))];
                tbBuildRelativePath.Text = ManifestItem.BuildtimeRelativeFolder;
                cbRequiredForRun.Checked = ManifestItem.RequiredForExecution;
                tbRuntimeRelativeFolder.Text = ManifestItem.RuntimeRelativeFolder;

                lblCreated.Text = ManifestItem.Created.ToString("MMM dd, yyyy HH:mm:ss");
                lblCreatedBy.Text = ManifestItem.CreatedBy.Username;
                lblModified.Text = ManifestItem.Modified.ToString("MMM dd, yyyy HH:mm:ss");
                lblModifiedBy.Text = ManifestItem.ModifiedBy.Username;

                if (ManifestItem.FileContentSize > 0)
                {
                    btnDownloadFile.Enabled = true;
                }
            }
            else
            {
                tbBuildRelativePath.Text = "/";

                tbId.Text = "(to be generated)";
                lblCreated.Text = DateTime.Now.ToString("MMM dd, yyyy HH:mm:ss");
                lblCreatedBy.Text = Manifest.WorkflowDefinition.Farm.AuthenticatedUser.Username;
                lblModified.Text = lblCreated.Text;
                lblModifiedBy.Text = lblCreatedBy.Text;

                btnDownloadFile.Enabled = false;
            }

            tbFilename.Focus();
        }

        private void cbRequiredForRun_CheckedChanged(object sender, EventArgs e)
        {
            tbRuntimeRelativeFolder.Enabled = cbRequiredForRun.Checked;
        }

        private void btnDownloadFile_Click(object sender, EventArgs e)
        {
            if ((ManifestItem == null) || (ManifestItem.FileContentSize == 0))
            {
                MessageBox.Show("Cannot download file. No manifest item exists or content is empty.");
            }

            sfdDownloadName.FileName = ManifestItem.FilenameWithExtension;
            if (sfdDownloadName.ShowDialog(this) == DialogResult.OK)
            {
                File.WriteAllBytes(sfdDownloadName.FileName, ManifestItem.FileContent);
            }
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
