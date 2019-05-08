namespace Corkscrew.Explorer
{
    partial class frmDefineWorkflowManifestItems
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDefineWorkflowManifestItems));
            this.btnEditManifestItem = new System.Windows.Forms.Button();
            this.lvManifestFiles = new System.Windows.Forms.ListView();
            this.btnRemoveManifestItem = new System.Windows.Forms.Button();
            this.btnAddManifestFile = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnFormClose = new System.Windows.Forms.Button();
            this.lblLogoText = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnEditManifestItem
            // 
            this.btnEditManifestItem.Enabled = false;
            this.btnEditManifestItem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEditManifestItem.Location = new System.Drawing.Point(449, 350);
            this.btnEditManifestItem.Name = "btnEditManifestItem";
            this.btnEditManifestItem.Size = new System.Drawing.Size(75, 23);
            this.btnEditManifestItem.TabIndex = 2;
            this.btnEditManifestItem.Text = "Edit Item";
            this.btnEditManifestItem.UseVisualStyleBackColor = true;
            this.btnEditManifestItem.Click += new System.EventHandler(this.btnEditManifestItem_Click);
            // 
            // lvManifestFiles
            // 
            this.lvManifestFiles.FullRowSelect = true;
            this.lvManifestFiles.Location = new System.Drawing.Point(35, 154);
            this.lvManifestFiles.MultiSelect = false;
            this.lvManifestFiles.Name = "lvManifestFiles";
            this.lvManifestFiles.Size = new System.Drawing.Size(570, 190);
            this.lvManifestFiles.TabIndex = 0;
            this.lvManifestFiles.UseCompatibleStateImageBehavior = false;
            this.lvManifestFiles.View = System.Windows.Forms.View.Details;
            this.lvManifestFiles.SelectedIndexChanged += new System.EventHandler(this.lvManifestFiles_SelectedIndexChanged);
            // 
            // btnRemoveManifestItem
            // 
            this.btnRemoveManifestItem.Enabled = false;
            this.btnRemoveManifestItem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRemoveManifestItem.Location = new System.Drawing.Point(353, 350);
            this.btnRemoveManifestItem.Name = "btnRemoveManifestItem";
            this.btnRemoveManifestItem.Size = new System.Drawing.Size(90, 23);
            this.btnRemoveManifestItem.TabIndex = 3;
            this.btnRemoveManifestItem.Text = "&Remove Item";
            this.btnRemoveManifestItem.UseVisualStyleBackColor = true;
            this.btnRemoveManifestItem.Click += new System.EventHandler(this.btnRemoveManifestItem_Click);
            // 
            // btnAddManifestFile
            // 
            this.btnAddManifestFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddManifestFile.Location = new System.Drawing.Point(530, 350);
            this.btnAddManifestFile.Name = "btnAddManifestFile";
            this.btnAddManifestFile.Size = new System.Drawing.Size(75, 23);
            this.btnAddManifestFile.TabIndex = 1;
            this.btnAddManifestFile.Text = "&Add Item";
            this.btnAddManifestFile.UseVisualStyleBackColor = true;
            this.btnAddManifestFile.Click += new System.EventHandler(this.btnAddManifestFile_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(32, 117);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(573, 34);
            this.label1.TabIndex = 9;
            this.label1.Text = "Add, modify or remove items from this workflow manifest. Manifest items are nothi" +
    "ng but files that are used to compile or execute the associated workflow.";
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Location = new System.Drawing.Point(35, 350);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "&Close";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnFormClose
            // 
            this.btnFormClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFormClose.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.btnFormClose.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.ButtonFace;
            this.btnFormClose.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ButtonShadow;
            this.btnFormClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFormClose.Font = new System.Drawing.Font("Microsoft Himalaya", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFormClose.Location = new System.Drawing.Point(602, 1);
            this.btnFormClose.Name = "btnFormClose";
            this.btnFormClose.Size = new System.Drawing.Size(60, 47);
            this.btnFormClose.TabIndex = 75;
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
            this.lblLogoText.Location = new System.Drawing.Point(-1, -3);
            this.lblLogoText.Name = "lblLogoText";
            this.lblLogoText.Size = new System.Drawing.Size(606, 107);
            this.lblLogoText.TabIndex = 74;
            this.lblLogoText.Text = "Manifest Items";
            this.lblLogoText.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            this.lblLogoText.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmDefineWorkflowManifestItems_MouseDown);
            // 
            // frmDefineWorkflowManifestItems
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this.ClientSize = new System.Drawing.Size(662, 391);
            this.ControlBox = false;
            this.Controls.Add(this.btnFormClose);
            this.Controls.Add(this.lblLogoText);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnEditManifestItem);
            this.Controls.Add(this.lvManifestFiles);
            this.Controls.Add(this.btnRemoveManifestItem);
            this.Controls.Add(this.btnAddManifestFile);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmDefineWorkflowManifestItems";
            this.ShowInTaskbar = false;
            this.Text = "Define Workflow - Manifest Items";
            this.Shown += new System.EventHandler(this.frmDefineWorkflowManifestItems_Shown);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmDefineWorkflowManifestItems_MouseDown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnEditManifestItem;
        private System.Windows.Forms.ListView lvManifestFiles;
        private System.Windows.Forms.Button btnRemoveManifestItem;
        private System.Windows.Forms.Button btnAddManifestFile;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnFormClose;
        private System.Windows.Forms.Label lblLogoText;
    }
}