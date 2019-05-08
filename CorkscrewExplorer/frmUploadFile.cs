using Corkscrew.SDK.objects;
using Corkscrew.SDK.tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Corkscrew.Explorer
{
    public partial class frmUploadFile : Form
    {

        #region Properties

        /// <summary>
        /// Folder where the file is to be uploaded
        /// </summary>
        public CSFileSystemEntryDirectory ContainingFolder
        {
            get;
            set;
        }

        /// <summary>
        /// Uploaded files
        /// </summary>
        public CSFileSystemEntryCollection UploadedFiles
        {
            get;
            private set;
        }

        #endregion


        public frmUploadFile()
        {
            InitializeComponent();
        }

        private void frmUploadFile_Shown(object sender, EventArgs e)
        {
            if (ContainingFolder != null)
            {
                lblUploadFolderPath.Text = ContainingFolder.FullPath;
                lblSelectedFilePath.Text = "";
                cbImportFromZip.Checked = false;
                cbImportFromZip.Visible = false;
            }
        }

        private void btnSelectUploadFile_Click(object sender, EventArgs e)
        {
            if (ofdSelectFile.ShowDialog() == DialogResult.OK)
            {
                lblSelectedFilePath.Text = ofdSelectFile.SafeFileName;
                cbImportFromZip.Checked = false;
                cbImportFromZip.Visible = false;

                if (Path.GetExtension(ofdSelectFile.SafeFileName).ToLower() == ".zip")
                {
                    cbImportFromZip.Visible = true;
                    MessageBox.Show("Zip file was selected. If you wish to import the files and folders in the Zip file into the CMS, check the \"Import from Zip file\" option. Leave it unchecked if you want to treat the Zip file as a regular file instead.");
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (cbImportFromZip.Checked)
            {
                List<CSFileSystemEntry> imported = CSZipFiles.ExtractArchive(ofdSelectFile.OpenFile(), ContainingFolder);
                UploadedFiles = new CSFileSystemEntryCollection(ContainingFolder.Site, imported, true);
            }
            else
            {
                FileInfo info = new FileInfo(ofdSelectFile.FileName);
                if (info.Length > int.MaxValue)
                {
                    MessageBox.Show("Files larger than 2 GB cannot be uploaded to Corkscrew.");
                }

                byte[] buffer = new byte[info.Length];
                using (Stream stream = ofdSelectFile.OpenFile())
                {
                    stream.Read(buffer, 0, (int)info.Length);
                }

                CSFileSystemEntryFile file = ContainingFolder.CreateFile(Path.GetFileNameWithoutExtension(ofdSelectFile.SafeFileName), Path.GetExtension(ofdSelectFile.SafeFileName), buffer);
                if (file != null)
                {
                    UploadedFiles = new CSFileSystemEntryCollection(ContainingFolder.Site, new List<CSFileSystemEntry>() { file }, true);
                }
            }

            // set attributes
            if (cbHidden.Checked || cbReadonly.Checked)
            {
                foreach (CSFileSystemEntry entry in UploadedFiles)
                {
                    entry.IsHidden = cbHidden.Checked;
                    entry.IsReadonly = cbReadonly.Checked;
                    entry.Save();
                }
            }

            MessageBox.Show("Files uploaded successfully.");
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
