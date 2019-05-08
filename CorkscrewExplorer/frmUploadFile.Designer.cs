namespace Corkscrew.Explorer
{
    partial class frmUploadFile
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmUploadFile));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblSelectedFilePath = new System.Windows.Forms.Label();
            this.btnSelectUploadFile = new System.Windows.Forms.Button();
            this.cbOverwrite = new System.Windows.Forms.CheckBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblUploadFolderPath = new System.Windows.Forms.Label();
            this.cbImportFromZip = new System.Windows.Forms.CheckBox();
            this.ofdSelectFile = new System.Windows.Forms.OpenFileDialog();
            this.btnFormClose = new System.Windows.Forms.Button();
            this.lblLogoText = new System.Windows.Forms.Label();
            this.cbReadonly = new System.Windows.Forms.CheckBox();
            this.cbHidden = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(33, 126);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Uploading to folder";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(33, 149);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "File to upload";
            // 
            // lblSelectedFilePath
            // 
            this.lblSelectedFilePath.AutoEllipsis = true;
            this.lblSelectedFilePath.Location = new System.Drawing.Point(158, 144);
            this.lblSelectedFilePath.Name = "lblSelectedFilePath";
            this.lblSelectedFilePath.Size = new System.Drawing.Size(325, 18);
            this.lblSelectedFilePath.TabIndex = 4;
            this.lblSelectedFilePath.Text = "(select a file to upload)";
            // 
            // btnSelectUploadFile
            // 
            this.btnSelectUploadFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSelectUploadFile.Location = new System.Drawing.Point(489, 139);
            this.btnSelectUploadFile.Name = "btnSelectUploadFile";
            this.btnSelectUploadFile.Size = new System.Drawing.Size(75, 23);
            this.btnSelectUploadFile.TabIndex = 5;
            this.btnSelectUploadFile.Text = "&Upload";
            this.btnSelectUploadFile.UseVisualStyleBackColor = true;
            this.btnSelectUploadFile.Click += new System.EventHandler(this.btnSelectUploadFile_Click);
            // 
            // cbOverwrite
            // 
            this.cbOverwrite.AutoSize = true;
            this.cbOverwrite.Location = new System.Drawing.Point(159, 197);
            this.cbOverwrite.Name = "cbOverwrite";
            this.cbOverwrite.Size = new System.Drawing.Size(130, 17);
            this.cbOverwrite.TabIndex = 6;
            this.cbOverwrite.Text = "Overwrite existing files";
            this.cbOverwrite.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Location = new System.Drawing.Point(489, 214);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 7;
            this.btnSave.Text = "&Save File";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Location = new System.Drawing.Point(408, 214);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblUploadFolderPath
            // 
            this.lblUploadFolderPath.AutoEllipsis = true;
            this.lblUploadFolderPath.Location = new System.Drawing.Point(156, 126);
            this.lblUploadFolderPath.Name = "lblUploadFolderPath";
            this.lblUploadFolderPath.Size = new System.Drawing.Size(333, 20);
            this.lblUploadFolderPath.TabIndex = 9;
            this.lblUploadFolderPath.Text = "(not set)";
            // 
            // cbImportFromZip
            // 
            this.cbImportFromZip.AutoSize = true;
            this.cbImportFromZip.Location = new System.Drawing.Point(161, 220);
            this.cbImportFromZip.Name = "cbImportFromZip";
            this.cbImportFromZip.Size = new System.Drawing.Size(112, 17);
            this.cbImportFromZip.TabIndex = 10;
            this.cbImportFromZip.Text = "Import from Zip file";
            this.cbImportFromZip.UseVisualStyleBackColor = true;
            // 
            // ofdSelectFile
            // 
            this.ofdSelectFile.Filter = "All Files (*.*)|*.*";
            this.ofdSelectFile.ShowReadOnly = true;
            this.ofdSelectFile.SupportMultiDottedExtensions = true;
            this.ofdSelectFile.Title = "Select a file to upload to Corkscrew";
            // 
            // btnFormClose
            // 
            this.btnFormClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFormClose.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.btnFormClose.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.ButtonFace;
            this.btnFormClose.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ButtonShadow;
            this.btnFormClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFormClose.Font = new System.Drawing.Font("Microsoft Himalaya", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFormClose.Location = new System.Drawing.Point(562, 0);
            this.btnFormClose.Name = "btnFormClose";
            this.btnFormClose.Size = new System.Drawing.Size(60, 47);
            this.btnFormClose.TabIndex = 81;
            this.btnFormClose.Text = "X";
            this.btnFormClose.UseVisualStyleBackColor = true;
            this.btnFormClose.Click += new System.EventHandler(this.btnFormClose_Click);
            // 
            // lblLogoText
            // 
            this.lblLogoText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblLogoText.Font = new System.Drawing.Font("Microsoft Himalaya", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLogoText.Image = ((System.Drawing.Image)(resources.GetObject("lblLogoText.Image")));
            this.lblLogoText.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.lblLogoText.Location = new System.Drawing.Point(-1, 0);
            this.lblLogoText.Name = "lblLogoText";
            this.lblLogoText.Size = new System.Drawing.Size(565, 107);
            this.lblLogoText.TabIndex = 80;
            this.lblLogoText.Text = "File Upload";
            this.lblLogoText.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            this.lblLogoText.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lblLogoText_MouseDown);
            // 
            // cbReadonly
            // 
            this.cbReadonly.AutoSize = true;
            this.cbReadonly.Location = new System.Drawing.Point(159, 174);
            this.cbReadonly.Name = "cbReadonly";
            this.cbReadonly.Size = new System.Drawing.Size(71, 17);
            this.cbReadonly.TabIndex = 0;
            this.cbReadonly.Text = "Readonly";
            this.cbReadonly.UseVisualStyleBackColor = true;
            // 
            // cbHidden
            // 
            this.cbHidden.AutoSize = true;
            this.cbHidden.Location = new System.Drawing.Point(236, 174);
            this.cbHidden.Name = "cbHidden";
            this.cbHidden.Size = new System.Drawing.Size(60, 17);
            this.cbHidden.TabIndex = 1;
            this.cbHidden.Text = "Hidden";
            this.cbHidden.UseVisualStyleBackColor = true;
            // 
            // frmUploadFile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(620, 257);
            this.Controls.Add(this.cbHidden);
            this.Controls.Add(this.btnFormClose);
            this.Controls.Add(this.cbReadonly);
            this.Controls.Add(this.lblLogoText);
            this.Controls.Add(this.cbImportFromZip);
            this.Controls.Add(this.lblUploadFolderPath);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.cbOverwrite);
            this.Controls.Add(this.btnSelectUploadFile);
            this.Controls.Add(this.lblSelectedFilePath);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmUploadFile";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Upload File";
            this.Shown += new System.EventHandler(this.frmUploadFile_Shown);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lblLogoText_MouseDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblSelectedFilePath;
        private System.Windows.Forms.Button btnSelectUploadFile;
        private System.Windows.Forms.CheckBox cbOverwrite;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblUploadFolderPath;
        private System.Windows.Forms.CheckBox cbImportFromZip;
        private System.Windows.Forms.OpenFileDialog ofdSelectFile;
        private System.Windows.Forms.Button btnFormClose;
        private System.Windows.Forms.Label lblLogoText;
        private System.Windows.Forms.CheckBox cbReadonly;
        private System.Windows.Forms.CheckBox cbHidden;
    }
}