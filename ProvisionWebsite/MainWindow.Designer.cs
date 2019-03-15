namespace Corkscrew.Tools.ProvisionWebsite
{
    partial class MainWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.lblLogoText = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.Log = new System.Windows.Forms.TextBox();
            this.ConfigDBServerName = new System.Windows.Forms.TextBox();
            this.DBUsername = new System.Windows.Forms.TextBox();
            this.DBPassword = new System.Windows.Forms.TextBox();
            this.CorkscrewSite = new System.Windows.Forms.ComboBox();
            this.IISIPAddress = new System.Windows.Forms.ComboBox();
            this.IISPort = new System.Windows.Forms.TextBox();
            this.IISHostname = new System.Windows.Forms.TextBox();
            this.IISSSLCertificateName = new System.Windows.Forms.Label();
            this.PickSSLCertificateButton = new System.Windows.Forms.Button();
            this.ProvisionButton = new System.Windows.Forms.Button();
            this.CanceButton = new System.Windows.Forms.Button();
            this.DatabaseLoginButton = new System.Windows.Forms.Button();
            this.UseIntegratedAuth = new System.Windows.Forms.CheckBox();
            this.label13 = new System.Windows.Forms.Label();
            this.UseSystemUser = new System.Windows.Forms.CheckBox();
            this.CSPassword = new System.Windows.Forms.TextBox();
            this.CSUsername = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.IISWebAppDiskFolderPath = new System.Windows.Forms.Label();
            this.SelectWebApplicationFolderButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblLogoText
            // 
            this.lblLogoText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblLogoText.Font = new System.Drawing.Font("Microsoft Himalaya", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLogoText.Image = ((System.Drawing.Image)(resources.GetObject("lblLogoText.Image")));
            this.lblLogoText.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.lblLogoText.Location = new System.Drawing.Point(0, -3);
            this.lblLogoText.Name = "lblLogoText";
            this.lblLogoText.Size = new System.Drawing.Size(921, 115);
            this.lblLogoText.TabIndex = 4;
            this.lblLogoText.Text = "Provision Website";
            this.lblLogoText.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            this.lblLogoText.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lblLogoText_MouseDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 126);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(327, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Name or IP address of server containing Farm\'s ConfigDB database:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(129, 150);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(126, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Database authentication:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(511, 155);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Username:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(709, 155);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Password:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 242);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(328, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Select or create the Corkscrew site hosting content for your website:";
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(13, 268);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(890, 2);
            this.label6.TabIndex = 10;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(13, 283);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(158, 13);
            this.label7.TabIndex = 11;
            this.label7.Text = "IIS website bindings information:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(278, 302);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(60, 13);
            this.label8.TabIndex = 12;
            this.label8.Text = "IP address:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(309, 326);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(29, 13);
            this.label9.TabIndex = 13;
            this.label9.Text = "Port:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(280, 350);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(58, 13);
            this.label10.TabIndex = 14;
            this.label10.Text = "Hostname:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(259, 374);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(79, 13);
            this.label11.TabIndex = 15;
            this.label11.Text = "SSL certificate:";
            // 
            // label12
            // 
            this.label12.BackColor = System.Drawing.Color.White;
            this.label12.Location = new System.Drawing.Point(10, 436);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(890, 2);
            this.label12.TabIndex = 16;
            // 
            // Log
            // 
            this.Log.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this.Log.ForeColor = System.Drawing.Color.White;
            this.Log.Location = new System.Drawing.Point(16, 498);
            this.Log.Multiline = true;
            this.Log.Name = "Log";
            this.Log.ReadOnly = true;
            this.Log.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.Log.Size = new System.Drawing.Size(885, 75);
            this.Log.TabIndex = 16;
            this.Log.WordWrap = false;
            // 
            // ConfigDBServerName
            // 
            this.ConfigDBServerName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this.ConfigDBServerName.ForeColor = System.Drawing.Color.White;
            this.ConfigDBServerName.Location = new System.Drawing.Point(357, 126);
            this.ConfigDBServerName.Name = "ConfigDBServerName";
            this.ConfigDBServerName.Size = new System.Drawing.Size(544, 20);
            this.ConfigDBServerName.TabIndex = 0;
            // 
            // DBUsername
            // 
            this.DBUsername.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this.DBUsername.Enabled = false;
            this.DBUsername.ForeColor = System.Drawing.Color.White;
            this.DBUsername.Location = new System.Drawing.Point(575, 152);
            this.DBUsername.Name = "DBUsername";
            this.DBUsername.Size = new System.Drawing.Size(128, 20);
            this.DBUsername.TabIndex = 2;
            // 
            // DBPassword
            // 
            this.DBPassword.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this.DBPassword.Enabled = false;
            this.DBPassword.ForeColor = System.Drawing.Color.White;
            this.DBPassword.Location = new System.Drawing.Point(771, 152);
            this.DBPassword.Name = "DBPassword";
            this.DBPassword.PasswordChar = '*';
            this.DBPassword.Size = new System.Drawing.Size(129, 20);
            this.DBPassword.TabIndex = 3;
            this.DBPassword.UseSystemPasswordChar = true;
            // 
            // CorkscrewSite
            // 
            this.CorkscrewSite.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this.CorkscrewSite.ForeColor = System.Drawing.Color.White;
            this.CorkscrewSite.FormattingEnabled = true;
            this.CorkscrewSite.Location = new System.Drawing.Point(354, 242);
            this.CorkscrewSite.Name = "CorkscrewSite";
            this.CorkscrewSite.Size = new System.Drawing.Size(544, 21);
            this.CorkscrewSite.TabIndex = 8;
            this.CorkscrewSite.DropDown += new System.EventHandler(this.CorkscrewSite_DropDown);
            this.CorkscrewSite.SelectedIndexChanged += new System.EventHandler(this.CorkscrewSite_SelectedIndexChanged);
            // 
            // IISIPAddress
            // 
            this.IISIPAddress.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this.IISIPAddress.ForeColor = System.Drawing.Color.White;
            this.IISIPAddress.FormattingEnabled = true;
            this.IISIPAddress.Location = new System.Drawing.Point(355, 299);
            this.IISIPAddress.Name = "IISIPAddress";
            this.IISIPAddress.Size = new System.Drawing.Size(544, 21);
            this.IISIPAddress.TabIndex = 9;
            // 
            // IISPort
            // 
            this.IISPort.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this.IISPort.ForeColor = System.Drawing.Color.White;
            this.IISPort.Location = new System.Drawing.Point(355, 324);
            this.IISPort.Name = "IISPort";
            this.IISPort.Size = new System.Drawing.Size(143, 20);
            this.IISPort.TabIndex = 10;
            this.IISPort.Text = "80";
            // 
            // IISHostname
            // 
            this.IISHostname.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this.IISHostname.ForeColor = System.Drawing.Color.White;
            this.IISHostname.Location = new System.Drawing.Point(355, 350);
            this.IISHostname.Name = "IISHostname";
            this.IISHostname.Size = new System.Drawing.Size(544, 20);
            this.IISHostname.TabIndex = 11;
            // 
            // IISSSLCertificateName
            // 
            this.IISSSLCertificateName.Location = new System.Drawing.Point(355, 374);
            this.IISSSLCertificateName.Name = "IISSSLCertificateName";
            this.IISSSLCertificateName.Size = new System.Drawing.Size(505, 23);
            this.IISSSLCertificateName.TabIndex = 25;
            this.IISSSLCertificateName.Text = "(No SSL)";
            // 
            // PickSSLCertificateButton
            // 
            this.PickSSLCertificateButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.PickSSLCertificateButton.Location = new System.Drawing.Point(865, 374);
            this.PickSSLCertificateButton.Name = "PickSSLCertificateButton";
            this.PickSSLCertificateButton.Size = new System.Drawing.Size(33, 23);
            this.PickSSLCertificateButton.TabIndex = 12;
            this.PickSSLCertificateButton.Text = "...";
            this.PickSSLCertificateButton.UseVisualStyleBackColor = true;
            this.PickSSLCertificateButton.Click += new System.EventHandler(this.PickSSLCertificateButton_Click);
            // 
            // ProvisionButton
            // 
            this.ProvisionButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ProvisionButton.Location = new System.Drawing.Point(797, 452);
            this.ProvisionButton.Name = "ProvisionButton";
            this.ProvisionButton.Size = new System.Drawing.Size(101, 26);
            this.ProvisionButton.TabIndex = 14;
            this.ProvisionButton.Text = "&Provision";
            this.ProvisionButton.UseVisualStyleBackColor = true;
            this.ProvisionButton.Click += new System.EventHandler(this.ProvisionButton_Click);
            // 
            // CanceButton
            // 
            this.CanceButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CanceButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CanceButton.Location = new System.Drawing.Point(16, 452);
            this.CanceButton.Name = "CanceButton";
            this.CanceButton.Size = new System.Drawing.Size(101, 26);
            this.CanceButton.TabIndex = 15;
            this.CanceButton.Text = "C&ancel";
            this.CanceButton.UseVisualStyleBackColor = true;
            this.CanceButton.Click += new System.EventHandler(this.CanceButton_Click);
            // 
            // DatabaseLoginButton
            // 
            this.DatabaseLoginButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.DatabaseLoginButton.Location = new System.Drawing.Point(355, 207);
            this.DatabaseLoginButton.Name = "DatabaseLoginButton";
            this.DatabaseLoginButton.Size = new System.Drawing.Size(106, 23);
            this.DatabaseLoginButton.TabIndex = 7;
            this.DatabaseLoginButton.Text = "&Get sites...";
            this.DatabaseLoginButton.UseVisualStyleBackColor = true;
            this.DatabaseLoginButton.Click += new System.EventHandler(this.DatabaseLoginButton_Click);
            // 
            // UseIntegratedAuth
            // 
            this.UseIntegratedAuth.AutoSize = true;
            this.UseIntegratedAuth.Location = new System.Drawing.Point(356, 152);
            this.UseIntegratedAuth.Name = "UseIntegratedAuth";
            this.UseIntegratedAuth.Size = new System.Drawing.Size(144, 17);
            this.UseIntegratedAuth.TabIndex = 1;
            this.UseIntegratedAuth.Text = "Integrated authentication";
            this.UseIntegratedAuth.UseVisualStyleBackColor = true;
            this.UseIntegratedAuth.CheckedChanged += new System.EventHandler(this.UseIntegratedAuth_CheckedChanged);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(129, 177);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(130, 13);
            this.label13.TabIndex = 31;
            this.label13.Text = "Corkscrew authentication:";
            // 
            // UseSystemUser
            // 
            this.UseSystemUser.AutoSize = true;
            this.UseSystemUser.Location = new System.Drawing.Point(356, 176);
            this.UseSystemUser.Name = "UseSystemUser";
            this.UseSystemUser.Size = new System.Drawing.Size(85, 17);
            this.UseSystemUser.TabIndex = 4;
            this.UseSystemUser.Text = "System User";
            this.UseSystemUser.UseVisualStyleBackColor = true;
            this.UseSystemUser.CheckedChanged += new System.EventHandler(this.UseSystemUser_CheckedChanged);
            // 
            // CSPassword
            // 
            this.CSPassword.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this.CSPassword.Enabled = false;
            this.CSPassword.ForeColor = System.Drawing.Color.White;
            this.CSPassword.Location = new System.Drawing.Point(772, 178);
            this.CSPassword.Name = "CSPassword";
            this.CSPassword.PasswordChar = '*';
            this.CSPassword.Size = new System.Drawing.Size(129, 20);
            this.CSPassword.TabIndex = 6;
            this.CSPassword.UseSystemPasswordChar = true;
            // 
            // CSUsername
            // 
            this.CSUsername.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this.CSUsername.Enabled = false;
            this.CSUsername.ForeColor = System.Drawing.Color.White;
            this.CSUsername.Location = new System.Drawing.Point(576, 178);
            this.CSUsername.Name = "CSUsername";
            this.CSUsername.Size = new System.Drawing.Size(128, 20);
            this.CSUsername.TabIndex = 5;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(710, 181);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(56, 13);
            this.label14.TabIndex = 34;
            this.label14.Text = "Password:";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(512, 181);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(58, 13);
            this.label15.TabIndex = 33;
            this.label15.Text = "Username:";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(202, 399);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(138, 13);
            this.label16.TabIndex = 37;
            this.label16.Text = "Web application disk folder:";
            // 
            // IISWebAppDiskFolderPath
            // 
            this.IISWebAppDiskFolderPath.Location = new System.Drawing.Point(355, 399);
            this.IISWebAppDiskFolderPath.Name = "IISWebAppDiskFolderPath";
            this.IISWebAppDiskFolderPath.Size = new System.Drawing.Size(505, 23);
            this.IISWebAppDiskFolderPath.TabIndex = 38;
            this.IISWebAppDiskFolderPath.Text = "(Not selected)";
            // 
            // SelectWebApplicationFolderButton
            // 
            this.SelectWebApplicationFolderButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SelectWebApplicationFolderButton.Location = new System.Drawing.Point(865, 399);
            this.SelectWebApplicationFolderButton.Name = "SelectWebApplicationFolderButton";
            this.SelectWebApplicationFolderButton.Size = new System.Drawing.Size(33, 23);
            this.SelectWebApplicationFolderButton.TabIndex = 13;
            this.SelectWebApplicationFolderButton.Text = "...";
            this.SelectWebApplicationFolderButton.UseVisualStyleBackColor = true;
            this.SelectWebApplicationFolderButton.Click += new System.EventHandler(this.SelectWebApplicationFolderButton_Click);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this.ClientSize = new System.Drawing.Size(920, 590);
            this.Controls.Add(this.SelectWebApplicationFolderButton);
            this.Controls.Add(this.IISWebAppDiskFolderPath);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.CSPassword);
            this.Controls.Add(this.CSUsername);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.UseSystemUser);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.UseIntegratedAuth);
            this.Controls.Add(this.DatabaseLoginButton);
            this.Controls.Add(this.CanceButton);
            this.Controls.Add(this.ProvisionButton);
            this.Controls.Add(this.PickSSLCertificateButton);
            this.Controls.Add(this.IISSSLCertificateName);
            this.Controls.Add(this.IISHostname);
            this.Controls.Add(this.IISPort);
            this.Controls.Add(this.IISIPAddress);
            this.Controls.Add(this.CorkscrewSite);
            this.Controls.Add(this.DBPassword);
            this.Controls.Add(this.DBUsername);
            this.Controls.Add(this.ConfigDBServerName);
            this.Controls.Add(this.Log);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblLogoText);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "MainWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Provision Website (a Corkscrew Tool)";
            this.Shown += new System.EventHandler(this.MainWindow_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblLogoText;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox Log;
        private System.Windows.Forms.TextBox ConfigDBServerName;
        private System.Windows.Forms.TextBox DBUsername;
        private System.Windows.Forms.TextBox DBPassword;
        private System.Windows.Forms.ComboBox CorkscrewSite;
        private System.Windows.Forms.ComboBox IISIPAddress;
        private System.Windows.Forms.TextBox IISPort;
        private System.Windows.Forms.TextBox IISHostname;
        private System.Windows.Forms.Label IISSSLCertificateName;
        private System.Windows.Forms.Button PickSSLCertificateButton;
        private System.Windows.Forms.Button ProvisionButton;
        private System.Windows.Forms.Button CanceButton;
        private System.Windows.Forms.Button DatabaseLoginButton;
        private System.Windows.Forms.CheckBox UseIntegratedAuth;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.CheckBox UseSystemUser;
        private System.Windows.Forms.TextBox CSPassword;
        private System.Windows.Forms.TextBox CSUsername;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label IISWebAppDiskFolderPath;
        private System.Windows.Forms.Button SelectWebApplicationFolderButton;
    }
}

