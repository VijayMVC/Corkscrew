namespace CMS.Setup.Screens
{
    partial class IISConnectionScreen
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label6 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblControlsDisableAPI = new System.Windows.Forms.Label();
            this.cbIPAddressAPI = new System.Windows.Forms.ComboBox();
            this.cbSSLAPI = new System.Windows.Forms.CheckBox();
            this.tbCertificateAPI = new System.Windows.Forms.TextBox();
            this.tbHostnameAPI = new System.Windows.Forms.TextBox();
            this.tbPortAPI = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblControlsDisableCC = new System.Windows.Forms.Label();
            this.cbIPAddressCC = new System.Windows.Forms.ComboBox();
            this.cbSSLCC = new System.Windows.Forms.CheckBox();
            this.tbCertificateCC = new System.Windows.Forms.TextBox();
            this.tbHostnameCC = new System.Windows.Forms.TextBox();
            this.tbPortCC = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Black;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(0, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(750, 56);
            this.label6.TabIndex = 11;
            this.label6.Text = "IIS Web Application Settings";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblControlsDisableAPI);
            this.panel1.Controls.Add(this.cbIPAddressAPI);
            this.panel1.Controls.Add(this.cbSSLAPI);
            this.panel1.Controls.Add(this.tbCertificateAPI);
            this.panel1.Controls.Add(this.tbHostnameAPI);
            this.panel1.Controls.Add(this.tbPortAPI);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(6, 72);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(741, 168);
            this.panel1.TabIndex = 12;
            // 
            // lblControlsDisableAPI
            // 
            this.lblControlsDisableAPI.Location = new System.Drawing.Point(218, 38);
            this.lblControlsDisableAPI.Name = "lblControlsDisableAPI";
            this.lblControlsDisableAPI.Size = new System.Drawing.Size(520, 112);
            this.lblControlsDisableAPI.TabIndex = 10;
            // 
            // cbIPAddressAPI
            // 
            this.cbIPAddressAPI.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this.cbIPAddressAPI.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbIPAddressAPI.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbIPAddressAPI.ForeColor = System.Drawing.Color.White;
            this.cbIPAddressAPI.FormattingEnabled = true;
            this.cbIPAddressAPI.Location = new System.Drawing.Point(239, 44);
            this.cbIPAddressAPI.Name = "cbIPAddressAPI";
            this.cbIPAddressAPI.Size = new System.Drawing.Size(489, 21);
            this.cbIPAddressAPI.TabIndex = 11;
            this.cbIPAddressAPI.SelectedIndexChanged += new System.EventHandler(this.cbIPAddressAPI_SelectedIndexChanged);
            // 
            // cbSSLAPI
            // 
            this.cbSSLAPI.AutoSize = true;
            this.cbSSLAPI.ForeColor = System.Drawing.Color.White;
            this.cbSSLAPI.Location = new System.Drawing.Point(350, 72);
            this.cbSSLAPI.Name = "cbSSLAPI";
            this.cbSSLAPI.Size = new System.Drawing.Size(46, 17);
            this.cbSSLAPI.TabIndex = 9;
            this.cbSSLAPI.Text = "SSL";
            this.cbSSLAPI.UseVisualStyleBackColor = true;
            this.cbSSLAPI.CheckedChanged += new System.EventHandler(this.cbSSLAPI_CheckedChanged);
            // 
            // tbCertificateAPI
            // 
            this.tbCertificateAPI.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this.tbCertificateAPI.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbCertificateAPI.ForeColor = System.Drawing.Color.White;
            this.tbCertificateAPI.Location = new System.Drawing.Point(239, 122);
            this.tbCertificateAPI.Name = "tbCertificateAPI";
            this.tbCertificateAPI.Size = new System.Drawing.Size(489, 20);
            this.tbCertificateAPI.TabIndex = 8;
            this.tbCertificateAPI.DoubleClick += new System.EventHandler(this.tbCertificateAPI_DoubleClick);
            // 
            // tbHostnameAPI
            // 
            this.tbHostnameAPI.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this.tbHostnameAPI.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbHostnameAPI.ForeColor = System.Drawing.Color.White;
            this.tbHostnameAPI.Location = new System.Drawing.Point(239, 96);
            this.tbHostnameAPI.Name = "tbHostnameAPI";
            this.tbHostnameAPI.Size = new System.Drawing.Size(489, 20);
            this.tbHostnameAPI.TabIndex = 7;
            this.tbHostnameAPI.TextChanged += new System.EventHandler(this.tbHostnameAPI_TextChanged);
            // 
            // tbPortAPI
            // 
            this.tbPortAPI.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this.tbPortAPI.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbPortAPI.ForeColor = System.Drawing.Color.White;
            this.tbPortAPI.Location = new System.Drawing.Point(239, 71);
            this.tbPortAPI.MaxLength = 5;
            this.tbPortAPI.Name = "tbPortAPI";
            this.tbPortAPI.Size = new System.Drawing.Size(94, 20);
            this.tbPortAPI.TabIndex = 6;
            this.tbPortAPI.TextChanged += new System.EventHandler(this.tbPortAPI_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(38, 122);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(146, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "SSL Certificate (PFX or CER):";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(38, 96);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(58, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Hostname:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(38, 71);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Port:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(38, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "IP address:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(12, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(206, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Settings for Corkscrew API Service";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lblControlsDisableCC);
            this.panel2.Controls.Add(this.cbIPAddressCC);
            this.panel2.Controls.Add(this.cbSSLCC);
            this.panel2.Controls.Add(this.tbCertificateCC);
            this.panel2.Controls.Add(this.tbHostnameCC);
            this.panel2.Controls.Add(this.tbPortCC);
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.label8);
            this.panel2.Controls.Add(this.label9);
            this.panel2.Controls.Add(this.label10);
            this.panel2.Controls.Add(this.label11);
            this.panel2.Location = new System.Drawing.Point(6, 258);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(741, 168);
            this.panel2.TabIndex = 13;
            // 
            // lblControlsDisableCC
            // 
            this.lblControlsDisableCC.Location = new System.Drawing.Point(218, 36);
            this.lblControlsDisableCC.Name = "lblControlsDisableCC";
            this.lblControlsDisableCC.Size = new System.Drawing.Size(520, 120);
            this.lblControlsDisableCC.TabIndex = 11;
            // 
            // cbIPAddressCC
            // 
            this.cbIPAddressCC.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this.cbIPAddressCC.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbIPAddressCC.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbIPAddressCC.ForeColor = System.Drawing.Color.White;
            this.cbIPAddressCC.FormattingEnabled = true;
            this.cbIPAddressCC.Location = new System.Drawing.Point(239, 44);
            this.cbIPAddressCC.Name = "cbIPAddressCC";
            this.cbIPAddressCC.Size = new System.Drawing.Size(489, 21);
            this.cbIPAddressCC.TabIndex = 12;
            this.cbIPAddressCC.SelectedIndexChanged += new System.EventHandler(this.cbIPAddressCC_SelectedIndexChanged);
            // 
            // cbSSLCC
            // 
            this.cbSSLCC.AutoSize = true;
            this.cbSSLCC.ForeColor = System.Drawing.Color.White;
            this.cbSSLCC.Location = new System.Drawing.Point(350, 72);
            this.cbSSLCC.Name = "cbSSLCC";
            this.cbSSLCC.Size = new System.Drawing.Size(46, 17);
            this.cbSSLCC.TabIndex = 14;
            this.cbSSLCC.Text = "SSL";
            this.cbSSLCC.UseVisualStyleBackColor = true;
            this.cbSSLCC.CheckedChanged += new System.EventHandler(this.cbSSLCC_CheckedChanged);
            // 
            // tbCertificateCC
            // 
            this.tbCertificateCC.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this.tbCertificateCC.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbCertificateCC.ForeColor = System.Drawing.Color.White;
            this.tbCertificateCC.Location = new System.Drawing.Point(239, 122);
            this.tbCertificateCC.Name = "tbCertificateCC";
            this.tbCertificateCC.Size = new System.Drawing.Size(489, 20);
            this.tbCertificateCC.TabIndex = 13;
            this.tbCertificateCC.DoubleClick += new System.EventHandler(this.tbCertificateCC_DoubleClick);
            // 
            // tbHostnameCC
            // 
            this.tbHostnameCC.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this.tbHostnameCC.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbHostnameCC.ForeColor = System.Drawing.Color.White;
            this.tbHostnameCC.Location = new System.Drawing.Point(239, 96);
            this.tbHostnameCC.Name = "tbHostnameCC";
            this.tbHostnameCC.Size = new System.Drawing.Size(489, 20);
            this.tbHostnameCC.TabIndex = 12;
            this.tbHostnameCC.TextChanged += new System.EventHandler(this.tbHostnameCC_TextChanged);
            // 
            // tbPortCC
            // 
            this.tbPortCC.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this.tbPortCC.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbPortCC.ForeColor = System.Drawing.Color.White;
            this.tbPortCC.Location = new System.Drawing.Point(239, 71);
            this.tbPortCC.MaxLength = 5;
            this.tbPortCC.Name = "tbPortCC";
            this.tbPortCC.Size = new System.Drawing.Size(94, 20);
            this.tbPortCC.TabIndex = 11;
            this.tbPortCC.TextChanged += new System.EventHandler(this.tbPortCC_TextChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(38, 122);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(146, 13);
            this.label7.TabIndex = 4;
            this.label7.Text = "SSL Certificate (PFX or CER):";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.Color.White;
            this.label8.Location = new System.Drawing.Point(38, 96);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(58, 13);
            this.label8.TabIndex = 3;
            this.label8.Text = "Hostname:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ForeColor = System.Drawing.Color.White;
            this.label9.Location = new System.Drawing.Point(38, 71);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(29, 13);
            this.label9.TabIndex = 2;
            this.label9.Text = "Port:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.ForeColor = System.Drawing.Color.White;
            this.label10.Location = new System.Drawing.Point(38, 44);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(60, 13);
            this.label10.TabIndex = 1;
            this.label10.Text = "IP address:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.Color.White;
            this.label11.Location = new System.Drawing.Point(12, 11);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(270, 13);
            this.label11.TabIndex = 0;
            this.label11.Text = "Settings for Corkscrew Control Center Website";
            // 
            // IISConnectionScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label6);
            this.Name = "IISConnectionScreen";
            this.Load += new System.EventHandler(this.IISConnectionScreen_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.CheckBox cbSSLCC;
        private System.Windows.Forms.TextBox tbCertificateCC;
        private System.Windows.Forms.TextBox tbHostnameCC;
        private System.Windows.Forms.TextBox tbPortCC;
        private System.Windows.Forms.CheckBox cbSSLAPI;
        private System.Windows.Forms.TextBox tbCertificateAPI;
        private System.Windows.Forms.TextBox tbHostnameAPI;
        private System.Windows.Forms.TextBox tbPortAPI;
        private System.Windows.Forms.Label lblControlsDisableAPI;
        private System.Windows.Forms.Label lblControlsDisableCC;
        private System.Windows.Forms.ComboBox cbIPAddressAPI;
        private System.Windows.Forms.ComboBox cbIPAddressCC;
    }
}
