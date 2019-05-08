namespace Corkscrew.Explorer
{
    partial class frmItemPermissions
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmItemPermissions));
            this.tbId = new System.Windows.Forms.TextBox();
            this.tbFullPath = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbAclInheritedFullControl = new System.Windows.Forms.RadioButton();
            this.rbAclInheritedContribute = new System.Windows.Forms.RadioButton();
            this.rbAclInheritedRead = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rbAclCurrentFullControl = new System.Windows.Forms.RadioButton();
            this.rbAclCurrentContribute = new System.Windows.Forms.RadioButton();
            this.rbAclCurrentRead = new System.Windows.Forms.RadioButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.rbAclNewFullControl = new System.Windows.Forms.RadioButton();
            this.rbAclNewContribute = new System.Windows.Forms.RadioButton();
            this.rbAclNewRead = new System.Windows.Forms.RadioButton();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cbSecurityPrincipals = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnFormClose = new System.Windows.Forms.Button();
            this.lblLogoText = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbId
            // 
            this.tbId.Location = new System.Drawing.Point(172, 135);
            this.tbId.Name = "tbId";
            this.tbId.ReadOnly = true;
            this.tbId.Size = new System.Drawing.Size(416, 20);
            this.tbId.TabIndex = 26;
            // 
            // tbFullPath
            // 
            this.tbFullPath.Location = new System.Drawing.Point(172, 161);
            this.tbFullPath.Multiline = true;
            this.tbFullPath.Name = "tbFullPath";
            this.tbFullPath.ReadOnly = true;
            this.tbFullPath.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbFullPath.Size = new System.Drawing.Size(415, 30);
            this.tbFullPath.TabIndex = 25;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(32, 137);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(16, 13);
            this.label5.TabIndex = 24;
            this.label5.Text = "Id";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(32, 161);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 13);
            this.label4.TabIndex = 23;
            this.label4.Text = "Full path";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbAclInheritedFullControl);
            this.groupBox1.Controls.Add(this.rbAclInheritedContribute);
            this.groupBox1.Controls.Add(this.rbAclInheritedRead);
            this.groupBox1.Location = new System.Drawing.Point(35, 240);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(184, 109);
            this.groupBox1.TabIndex = 27;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Inherited permissions";
            // 
            // rbAclInheritedFullControl
            // 
            this.rbAclInheritedFullControl.AutoSize = true;
            this.rbAclInheritedFullControl.Enabled = false;
            this.rbAclInheritedFullControl.Location = new System.Drawing.Point(19, 68);
            this.rbAclInheritedFullControl.Name = "rbAclInheritedFullControl";
            this.rbAclInheritedFullControl.Size = new System.Drawing.Size(76, 17);
            this.rbAclInheritedFullControl.TabIndex = 2;
            this.rbAclInheritedFullControl.TabStop = true;
            this.rbAclInheritedFullControl.Text = "Full control";
            this.rbAclInheritedFullControl.UseVisualStyleBackColor = true;
            // 
            // rbAclInheritedContribute
            // 
            this.rbAclInheritedContribute.AutoSize = true;
            this.rbAclInheritedContribute.Enabled = false;
            this.rbAclInheritedContribute.Location = new System.Drawing.Point(19, 45);
            this.rbAclInheritedContribute.Name = "rbAclInheritedContribute";
            this.rbAclInheritedContribute.Size = new System.Drawing.Size(73, 17);
            this.rbAclInheritedContribute.TabIndex = 1;
            this.rbAclInheritedContribute.TabStop = true;
            this.rbAclInheritedContribute.Text = "Contribute";
            this.rbAclInheritedContribute.UseVisualStyleBackColor = true;
            // 
            // rbAclInheritedRead
            // 
            this.rbAclInheritedRead.AutoSize = true;
            this.rbAclInheritedRead.Enabled = false;
            this.rbAclInheritedRead.Location = new System.Drawing.Point(19, 22);
            this.rbAclInheritedRead.Name = "rbAclInheritedRead";
            this.rbAclInheritedRead.Size = new System.Drawing.Size(51, 17);
            this.rbAclInheritedRead.TabIndex = 0;
            this.rbAclInheritedRead.TabStop = true;
            this.rbAclInheritedRead.Text = "Read";
            this.rbAclInheritedRead.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rbAclCurrentFullControl);
            this.groupBox2.Controls.Add(this.rbAclCurrentContribute);
            this.groupBox2.Controls.Add(this.rbAclCurrentRead);
            this.groupBox2.Location = new System.Drawing.Point(225, 240);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(174, 109);
            this.groupBox2.TabIndex = 28;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Current permissions";
            // 
            // rbAclCurrentFullControl
            // 
            this.rbAclCurrentFullControl.AutoSize = true;
            this.rbAclCurrentFullControl.Enabled = false;
            this.rbAclCurrentFullControl.Location = new System.Drawing.Point(20, 68);
            this.rbAclCurrentFullControl.Name = "rbAclCurrentFullControl";
            this.rbAclCurrentFullControl.Size = new System.Drawing.Size(76, 17);
            this.rbAclCurrentFullControl.TabIndex = 5;
            this.rbAclCurrentFullControl.TabStop = true;
            this.rbAclCurrentFullControl.Text = "Full control";
            this.rbAclCurrentFullControl.UseVisualStyleBackColor = true;
            // 
            // rbAclCurrentContribute
            // 
            this.rbAclCurrentContribute.AutoSize = true;
            this.rbAclCurrentContribute.Enabled = false;
            this.rbAclCurrentContribute.Location = new System.Drawing.Point(20, 45);
            this.rbAclCurrentContribute.Name = "rbAclCurrentContribute";
            this.rbAclCurrentContribute.Size = new System.Drawing.Size(73, 17);
            this.rbAclCurrentContribute.TabIndex = 4;
            this.rbAclCurrentContribute.TabStop = true;
            this.rbAclCurrentContribute.Text = "Contribute";
            this.rbAclCurrentContribute.UseVisualStyleBackColor = true;
            // 
            // rbAclCurrentRead
            // 
            this.rbAclCurrentRead.AutoSize = true;
            this.rbAclCurrentRead.Enabled = false;
            this.rbAclCurrentRead.Location = new System.Drawing.Point(20, 22);
            this.rbAclCurrentRead.Name = "rbAclCurrentRead";
            this.rbAclCurrentRead.Size = new System.Drawing.Size(51, 17);
            this.rbAclCurrentRead.TabIndex = 3;
            this.rbAclCurrentRead.TabStop = true;
            this.rbAclCurrentRead.Text = "Read";
            this.rbAclCurrentRead.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.rbAclNewFullControl);
            this.groupBox3.Controls.Add(this.rbAclNewContribute);
            this.groupBox3.Controls.Add(this.rbAclNewRead);
            this.groupBox3.Location = new System.Drawing.Point(405, 240);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(183, 109);
            this.groupBox3.TabIndex = 29;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "New permissions";
            // 
            // rbAclNewFullControl
            // 
            this.rbAclNewFullControl.AutoSize = true;
            this.rbAclNewFullControl.Location = new System.Drawing.Point(21, 68);
            this.rbAclNewFullControl.Name = "rbAclNewFullControl";
            this.rbAclNewFullControl.Size = new System.Drawing.Size(76, 17);
            this.rbAclNewFullControl.TabIndex = 5;
            this.rbAclNewFullControl.TabStop = true;
            this.rbAclNewFullControl.Text = "Full control";
            this.rbAclNewFullControl.UseVisualStyleBackColor = true;
            // 
            // rbAclNewContribute
            // 
            this.rbAclNewContribute.AutoSize = true;
            this.rbAclNewContribute.Location = new System.Drawing.Point(21, 45);
            this.rbAclNewContribute.Name = "rbAclNewContribute";
            this.rbAclNewContribute.Size = new System.Drawing.Size(73, 17);
            this.rbAclNewContribute.TabIndex = 4;
            this.rbAclNewContribute.TabStop = true;
            this.rbAclNewContribute.Text = "Contribute";
            this.rbAclNewContribute.UseVisualStyleBackColor = true;
            // 
            // rbAclNewRead
            // 
            this.rbAclNewRead.AutoSize = true;
            this.rbAclNewRead.Location = new System.Drawing.Point(21, 22);
            this.rbAclNewRead.Name = "rbAclNewRead";
            this.rbAclNewRead.Size = new System.Drawing.Size(51, 17);
            this.rbAclNewRead.TabIndex = 3;
            this.rbAclNewRead.TabStop = true;
            this.rbAclNewRead.Text = "Read";
            this.rbAclNewRead.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Location = new System.Drawing.Point(431, 364);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 31;
            this.btnCancel.Text = "&Close";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Location = new System.Drawing.Point(512, 364);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 30;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(32, 197);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 13);
            this.label1.TabIndex = 32;
            this.label1.Text = "Security Principal";
            // 
            // cbSecurityPrincipals
            // 
            this.cbSecurityPrincipals.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSecurityPrincipals.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbSecurityPrincipals.FormattingEnabled = true;
            this.cbSecurityPrincipals.Location = new System.Drawing.Point(172, 197);
            this.cbSecurityPrincipals.Name = "cbSecurityPrincipals";
            this.cbSecurityPrincipals.Size = new System.Drawing.Size(415, 21);
            this.cbSecurityPrincipals.TabIndex = 33;
            this.cbSecurityPrincipals.SelectedIndexChanged += new System.EventHandler(this.cbSecurityPrincipals_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(32, 364);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(350, 32);
            this.label2.TabIndex = 34;
            this.label2.Text = "NOTE: Only aggregate highest permissions are shown under Inherited and Current pe" +
    "rmissions.";
            // 
            // btnFormClose
            // 
            this.btnFormClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFormClose.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.btnFormClose.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.ButtonFace;
            this.btnFormClose.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ButtonShadow;
            this.btnFormClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFormClose.Font = new System.Drawing.Font("Microsoft Himalaya", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFormClose.Location = new System.Drawing.Point(568, -2);
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
            this.lblLogoText.Location = new System.Drawing.Point(-2, -2);
            this.lblLogoText.Name = "lblLogoText";
            this.lblLogoText.Size = new System.Drawing.Size(574, 107);
            this.lblLogoText.TabIndex = 78;
            this.lblLogoText.Text = "Permissions";
            this.lblLogoText.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            this.lblLogoText.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lblLogoText_MouseDown);
            // 
            // frmItemPermissions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this.ClientSize = new System.Drawing.Size(627, 410);
            this.Controls.Add(this.btnFormClose);
            this.Controls.Add(this.lblLogoText);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cbSecurityPrincipals);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.tbId);
            this.Controls.Add(this.tbFullPath);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmItemPermissions";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Set permissions";
            this.Shown += new System.EventHandler(this.frmItemPermissions_Shown);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lblLogoText_MouseDown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbId;
        private System.Windows.Forms.TextBox tbFullPath;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton rbAclInheritedFullControl;
        private System.Windows.Forms.RadioButton rbAclInheritedContribute;
        private System.Windows.Forms.RadioButton rbAclInheritedRead;
        private System.Windows.Forms.RadioButton rbAclCurrentFullControl;
        private System.Windows.Forms.RadioButton rbAclCurrentContribute;
        private System.Windows.Forms.RadioButton rbAclCurrentRead;
        private System.Windows.Forms.RadioButton rbAclNewFullControl;
        private System.Windows.Forms.RadioButton rbAclNewContribute;
        private System.Windows.Forms.RadioButton rbAclNewRead;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbSecurityPrincipals;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnFormClose;
        private System.Windows.Forms.Label lblLogoText;
    }
}