namespace Corkscrew.Explorer
{
    partial class frmImportUsersFromAD
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmImportUsersFromAD));
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tbUsername = new System.Windows.Forms.TextBox();
            this.tbPassword = new System.Windows.Forms.TextBox();
            this.tbDomainName = new System.Windows.Forms.TextBox();
            this.btnLoginToAD = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.cbDoNotImportDisabledUsers = new System.Windows.Forms.CheckBox();
            this.lvUsersToImport = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnImportUsers = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.cbDoNotImportUsersWithoutEmailAddress = new System.Windows.Forms.CheckBox();
            this.cbSelectAll = new System.Windows.Forms.CheckBox();
            this.btnFormClose = new System.Windows.Forms.Button();
            this.lblLogoText = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 127);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(402, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Enter credentials that has permission to query the Active Directory domain for us" +
    "ers..";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(23, 149);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Username";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(23, 176);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Password";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(22, 206);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(72, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Domain name";
            // 
            // tbUsername
            // 
            this.tbUsername.Location = new System.Drawing.Point(99, 149);
            this.tbUsername.Name = "tbUsername";
            this.tbUsername.Size = new System.Drawing.Size(196, 20);
            this.tbUsername.TabIndex = 10;
            // 
            // tbPassword
            // 
            this.tbPassword.Location = new System.Drawing.Point(99, 176);
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.PasswordChar = '*';
            this.tbPassword.Size = new System.Drawing.Size(196, 20);
            this.tbPassword.TabIndex = 11;
            this.tbPassword.UseSystemPasswordChar = true;
            // 
            // tbDomainName
            // 
            this.tbDomainName.Location = new System.Drawing.Point(100, 206);
            this.tbDomainName.Name = "tbDomainName";
            this.tbDomainName.Size = new System.Drawing.Size(195, 20);
            this.tbDomainName.TabIndex = 12;
            // 
            // btnLoginToAD
            // 
            this.btnLoginToAD.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLoginToAD.Location = new System.Drawing.Point(312, 206);
            this.btnLoginToAD.Name = "btnLoginToAD";
            this.btnLoginToAD.Size = new System.Drawing.Size(102, 23);
            this.btnLoginToAD.TabIndex = 13;
            this.btnLoginToAD.Text = "&Login";
            this.btnLoginToAD.UseVisualStyleBackColor = true;
            this.btnLoginToAD.Click += new System.EventHandler(this.btnLoginToAD_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 245);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(243, 13);
            this.label6.TabIndex = 14;
            this.label6.Text = "Select the users to be imported from the list below.";
            // 
            // cbDoNotImportDisabledUsers
            // 
            this.cbDoNotImportDisabledUsers.AutoSize = true;
            this.cbDoNotImportDisabledUsers.Checked = true;
            this.cbDoNotImportDisabledUsers.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbDoNotImportDisabledUsers.Location = new System.Drawing.Point(438, 149);
            this.cbDoNotImportDisabledUsers.Name = "cbDoNotImportDisabledUsers";
            this.cbDoNotImportDisabledUsers.Size = new System.Drawing.Size(165, 17);
            this.cbDoNotImportDisabledUsers.TabIndex = 15;
            this.cbDoNotImportDisabledUsers.Text = "Do not import [disabled] users";
            this.cbDoNotImportDisabledUsers.UseVisualStyleBackColor = true;
            // 
            // lvUsersToImport
            // 
            this.lvUsersToImport.CheckBoxes = true;
            this.lvUsersToImport.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader6,
            this.columnHeader7});
            this.lvUsersToImport.FullRowSelect = true;
            this.lvUsersToImport.Location = new System.Drawing.Point(15, 270);
            this.lvUsersToImport.Name = "lvUsersToImport";
            this.lvUsersToImport.ShowGroups = false;
            this.lvUsersToImport.Size = new System.Drawing.Size(659, 272);
            this.lvUsersToImport.TabIndex = 16;
            this.lvUsersToImport.UseCompatibleStateImageBehavior = false;
            this.lvUsersToImport.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Username";
            this.columnHeader1.Width = 141;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "First name";
            this.columnHeader2.Width = 125;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Last name";
            this.columnHeader3.Width = 115;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Email address";
            this.columnHeader6.Width = 138;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "Enabled ?";
            // 
            // btnImportUsers
            // 
            this.btnImportUsers.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnImportUsers.Location = new System.Drawing.Point(599, 548);
            this.btnImportUsers.Name = "btnImportUsers";
            this.btnImportUsers.Size = new System.Drawing.Size(75, 23);
            this.btnImportUsers.TabIndex = 17;
            this.btnImportUsers.Text = "Import";
            this.btnImportUsers.UseVisualStyleBackColor = true;
            this.btnImportUsers.Click += new System.EventHandler(this.btnImportUsers_Click);
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Location = new System.Drawing.Point(15, 542);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 18;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // cbDoNotImportUsersWithoutEmailAddress
            // 
            this.cbDoNotImportUsersWithoutEmailAddress.AutoSize = true;
            this.cbDoNotImportUsersWithoutEmailAddress.Checked = true;
            this.cbDoNotImportUsersWithoutEmailAddress.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbDoNotImportUsersWithoutEmailAddress.Location = new System.Drawing.Point(438, 172);
            this.cbDoNotImportUsersWithoutEmailAddress.Name = "cbDoNotImportUsersWithoutEmailAddress";
            this.cbDoNotImportUsersWithoutEmailAddress.Size = new System.Drawing.Size(236, 17);
            this.cbDoNotImportUsersWithoutEmailAddress.TabIndex = 19;
            this.cbDoNotImportUsersWithoutEmailAddress.Text = "Do not import users without an email address";
            this.cbDoNotImportUsersWithoutEmailAddress.UseVisualStyleBackColor = true;
            // 
            // cbSelectAll
            // 
            this.cbSelectAll.AutoSize = true;
            this.cbSelectAll.Location = new System.Drawing.Point(604, 244);
            this.cbSelectAll.Name = "cbSelectAll";
            this.cbSelectAll.Size = new System.Drawing.Size(70, 17);
            this.cbSelectAll.TabIndex = 20;
            this.cbSelectAll.Text = "Select All";
            this.cbSelectAll.UseVisualStyleBackColor = true;
            this.cbSelectAll.CheckedChanged += new System.EventHandler(this.cbSelectAll_CheckedChanged);
            // 
            // btnFormClose
            // 
            this.btnFormClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFormClose.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.btnFormClose.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.ButtonFace;
            this.btnFormClose.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ButtonShadow;
            this.btnFormClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFormClose.Font = new System.Drawing.Font("Microsoft Himalaya", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFormClose.Location = new System.Drawing.Point(629, 0);
            this.btnFormClose.Name = "btnFormClose";
            this.btnFormClose.Size = new System.Drawing.Size(60, 47);
            this.btnFormClose.TabIndex = 79;
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
            this.lblLogoText.Location = new System.Drawing.Point(3, 2);
            this.lblLogoText.Name = "lblLogoText";
            this.lblLogoText.Size = new System.Drawing.Size(620, 107);
            this.lblLogoText.TabIndex = 78;
            this.lblLogoText.Text = "Import from AD";
            this.lblLogoText.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            this.lblLogoText.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmImportUsersFromAD_MouseDown);
            // 
            // frmImportUsersFromAD
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(689, 587);
            this.ControlBox = false;
            this.Controls.Add(this.btnFormClose);
            this.Controls.Add(this.lblLogoText);
            this.Controls.Add(this.cbSelectAll);
            this.Controls.Add(this.cbDoNotImportUsersWithoutEmailAddress);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnImportUsers);
            this.Controls.Add(this.lvUsersToImport);
            this.Controls.Add(this.cbDoNotImportDisabledUsers);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btnLoginToAD);
            this.Controls.Add(this.tbDomainName);
            this.Controls.Add(this.tbPassword);
            this.Controls.Add(this.tbUsername);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmImportUsersFromAD";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Shown += new System.EventHandler(this.frmImportUsersFromAD_Shown);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmImportUsersFromAD_MouseDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbUsername;
        private System.Windows.Forms.TextBox tbPassword;
        private System.Windows.Forms.TextBox tbDomainName;
        private System.Windows.Forms.Button btnLoginToAD;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox cbDoNotImportDisabledUsers;
        private System.Windows.Forms.ListView lvUsersToImport;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.Button btnImportUsers;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.CheckBox cbDoNotImportUsersWithoutEmailAddress;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.CheckBox cbSelectAll;
        private System.Windows.Forms.Button btnFormClose;
        private System.Windows.Forms.Label lblLogoText;
    }
}