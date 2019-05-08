namespace Corkscrew.Explorer
{
    partial class frmConnectToFarm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmConnectToFarm));
            this.rbOptionUseUIDPWDAuth = new System.Windows.Forms.RadioButton();
            this.rbOptionUseIntegratedAuthentication = new System.Windows.Forms.RadioButton();
            this.tbPassword = new System.Windows.Forms.TextBox();
            this.tbUsername = new System.Windows.Forms.TextBox();
            this.tbServerAddress = new System.Windows.Forms.TextBox();
            this.cmbDatabaseEngine = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbConnectionString = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnFormClose = new System.Windows.Forms.Button();
            this.lblLogoText = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // rbOptionUseUIDPWDAuth
            // 
            this.rbOptionUseUIDPWDAuth.AutoSize = true;
            this.rbOptionUseUIDPWDAuth.Checked = true;
            this.rbOptionUseUIDPWDAuth.Location = new System.Drawing.Point(49, 223);
            this.rbOptionUseUIDPWDAuth.Name = "rbOptionUseUIDPWDAuth";
            this.rbOptionUseUIDPWDAuth.Size = new System.Drawing.Size(292, 17);
            this.rbOptionUseUIDPWDAuth.TabIndex = 61;
            this.rbOptionUseUIDPWDAuth.TabStop = true;
            this.rbOptionUseUIDPWDAuth.Text = "Use a database username and password to authenticate";
            this.rbOptionUseUIDPWDAuth.UseVisualStyleBackColor = true;
            this.rbOptionUseUIDPWDAuth.CheckedChanged += new System.EventHandler(this.rbOptionUseUIDPWDAuth_CheckedChanged);
            // 
            // rbOptionUseIntegratedAuthentication
            // 
            this.rbOptionUseIntegratedAuthentication.AutoSize = true;
            this.rbOptionUseIntegratedAuthentication.Location = new System.Drawing.Point(49, 200);
            this.rbOptionUseIntegratedAuthentication.Name = "rbOptionUseIntegratedAuthentication";
            this.rbOptionUseIntegratedAuthentication.Size = new System.Drawing.Size(164, 17);
            this.rbOptionUseIntegratedAuthentication.TabIndex = 60;
            this.rbOptionUseIntegratedAuthentication.Text = "Use integrated authentication";
            this.rbOptionUseIntegratedAuthentication.UseVisualStyleBackColor = true;
            this.rbOptionUseIntegratedAuthentication.CheckedChanged += new System.EventHandler(this.rbOptionUseIntegratedAuthentication_CheckedChanged);
            // 
            // tbPassword
            // 
            this.tbPassword.Location = new System.Drawing.Point(219, 284);
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.PasswordChar = '*';
            this.tbPassword.Size = new System.Drawing.Size(205, 20);
            this.tbPassword.TabIndex = 59;
            this.tbPassword.UseSystemPasswordChar = true;
            this.tbPassword.TextChanged += new System.EventHandler(this.tbPassword_TextChanged);
            // 
            // tbUsername
            // 
            this.tbUsername.Location = new System.Drawing.Point(220, 254);
            this.tbUsername.Name = "tbUsername";
            this.tbUsername.Size = new System.Drawing.Size(204, 20);
            this.tbUsername.TabIndex = 58;
            this.tbUsername.TextChanged += new System.EventHandler(this.tbUsername_TextChanged);
            // 
            // tbServerAddress
            // 
            this.tbServerAddress.Location = new System.Drawing.Point(219, 161);
            this.tbServerAddress.Name = "tbServerAddress";
            this.tbServerAddress.Size = new System.Drawing.Size(412, 20);
            this.tbServerAddress.TabIndex = 57;
            this.tbServerAddress.TextChanged += new System.EventHandler(this.tbServerAddress_TextChanged);
            // 
            // cmbDatabaseEngine
            // 
            this.cmbDatabaseEngine.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDatabaseEngine.FormattingEnabled = true;
            this.cmbDatabaseEngine.Items.AddRange(new object[] {
            "(Select a database engine)",
            "Microsoft SQL Server",
            "MySQL",
            "InnoDB"});
            this.cmbDatabaseEngine.Location = new System.Drawing.Point(219, 133);
            this.cmbDatabaseEngine.Name = "cmbDatabaseEngine";
            this.cmbDatabaseEngine.Size = new System.Drawing.Size(412, 21);
            this.cmbDatabaseEngine.TabIndex = 56;
            this.cmbDatabaseEngine.SelectedIndexChanged += new System.EventHandler(this.cmbDatabaseEngine_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(69, 284);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(131, 13);
            this.label6.TabIndex = 55;
            this.label6.Text = "Database Login Password";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(69, 254);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(133, 13);
            this.label4.TabIndex = 54;
            this.label4.Text = "Database Login Username";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(46, 161);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(78, 13);
            this.label3.TabIndex = 53;
            this.label3.Text = "Server address";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(46, 133);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(111, 13);
            this.label2.TabIndex = 52;
            this.label2.Text = "Database engine type";
            // 
            // tbConnectionString
            // 
            this.tbConnectionString.Location = new System.Drawing.Point(49, 348);
            this.tbConnectionString.MaxLength = 255;
            this.tbConnectionString.Name = "tbConnectionString";
            this.tbConnectionString.ReadOnly = true;
            this.tbConnectionString.Size = new System.Drawing.Size(582, 20);
            this.tbConnectionString.TabIndex = 51;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(46, 332);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(89, 13);
            this.label5.TabIndex = 50;
            this.label5.Text = "Connection string";
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Location = new System.Drawing.Point(459, 387);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 63;
            this.btnCancel.Text = "&Close";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Location = new System.Drawing.Point(540, 387);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(91, 23);
            this.btnSave.TabIndex = 62;
            this.btnSave.Text = "C&onnect";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnFormClose
            // 
            this.btnFormClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFormClose.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.btnFormClose.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.ButtonFace;
            this.btnFormClose.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ButtonShadow;
            this.btnFormClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFormClose.Font = new System.Drawing.Font("Microsoft Himalaya", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFormClose.Location = new System.Drawing.Point(631, 1);
            this.btnFormClose.Name = "btnFormClose";
            this.btnFormClose.Size = new System.Drawing.Size(60, 47);
            this.btnFormClose.TabIndex = 65;
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
            this.lblLogoText.Location = new System.Drawing.Point(0, 0);
            this.lblLogoText.Name = "lblLogoText";
            this.lblLogoText.Size = new System.Drawing.Size(631, 107);
            this.lblLogoText.TabIndex = 64;
            this.lblLogoText.Text = "Connect to Farm";
            this.lblLogoText.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            this.lblLogoText.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmConnectToFarm_MouseDown);
            // 
            // frmConnectToFarm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(692, 435);
            this.Controls.Add(this.btnFormClose);
            this.Controls.Add(this.lblLogoText);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.rbOptionUseUIDPWDAuth);
            this.Controls.Add(this.rbOptionUseIntegratedAuthentication);
            this.Controls.Add(this.tbPassword);
            this.Controls.Add(this.tbUsername);
            this.Controls.Add(this.tbServerAddress);
            this.Controls.Add(this.cmbDatabaseEngine);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbConnectionString);
            this.Controls.Add(this.label5);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmConnectToFarm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Connect to a Farm";
            this.Shown += new System.EventHandler(this.frmConnectToFarm_Shown);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmConnectToFarm_MouseDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton rbOptionUseUIDPWDAuth;
        private System.Windows.Forms.RadioButton rbOptionUseIntegratedAuthentication;
        private System.Windows.Forms.TextBox tbPassword;
        private System.Windows.Forms.TextBox tbUsername;
        private System.Windows.Forms.TextBox tbServerAddress;
        private System.Windows.Forms.ComboBox cmbDatabaseEngine;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbConnectionString;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnFormClose;
        private System.Windows.Forms.Label lblLogoText;
    }
}