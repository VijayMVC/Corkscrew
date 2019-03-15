namespace Corkscrew.Tools.ProvisionWebsite
{
    partial class CertificateSelectionScreen
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
            this.label3 = new System.Windows.Forms.Label();
            this.lvCertificates = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSelectCertificate = new System.Windows.Forms.Button();
            this.btnViewCertificate = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Black;
            this.label3.Font = new System.Drawing.Font("Arial Black", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(631, 51);
            this.label3.TabIndex = 12;
            this.label3.Text = "Select Certificate for SSL Binding";
            this.label3.MouseDown += new System.Windows.Forms.MouseEventHandler(this.label3_MouseDown);
            // 
            // lvCertificates
            // 
            this.lvCertificates.BackColor = System.Drawing.SystemColors.Window;
            this.lvCertificates.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lvCertificates.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.lvCertificates.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lvCertificates.FullRowSelect = true;
            this.lvCertificates.Location = new System.Drawing.Point(6, 55);
            this.lvCertificates.MultiSelect = false;
            this.lvCertificates.Name = "lvCertificates";
            this.lvCertificates.Size = new System.Drawing.Size(613, 424);
            this.lvCertificates.TabIndex = 13;
            this.lvCertificates.UseCompatibleStateImageBehavior = false;
            this.lvCertificates.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Subject name";
            this.columnHeader1.Width = 264;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Issued by";
            this.columnHeader2.Width = 165;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Expires on";
            this.columnHeader3.Width = 154;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(7, 494);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 14;
            this.btnCancel.Text = "C&ancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSelectCertificate
            // 
            this.btnSelectCertificate.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnSelectCertificate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSelectCertificate.ForeColor = System.Drawing.Color.White;
            this.btnSelectCertificate.Location = new System.Drawing.Point(542, 494);
            this.btnSelectCertificate.Name = "btnSelectCertificate";
            this.btnSelectCertificate.Size = new System.Drawing.Size(75, 23);
            this.btnSelectCertificate.TabIndex = 15;
            this.btnSelectCertificate.Text = "&Select";
            this.btnSelectCertificate.UseVisualStyleBackColor = true;
            this.btnSelectCertificate.Click += new System.EventHandler(this.btnSelectCertificate_Click);
            // 
            // btnViewCertificate
            // 
            this.btnViewCertificate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnViewCertificate.ForeColor = System.Drawing.Color.White;
            this.btnViewCertificate.Location = new System.Drawing.Point(413, 494);
            this.btnViewCertificate.Name = "btnViewCertificate";
            this.btnViewCertificate.Size = new System.Drawing.Size(123, 23);
            this.btnViewCertificate.TabIndex = 16;
            this.btnViewCertificate.Text = "&View certificate...";
            this.btnViewCertificate.UseVisualStyleBackColor = true;
            this.btnViewCertificate.Click += new System.EventHandler(this.btnViewCertificate_Click);
            // 
            // CertificateSelectionScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this.ClientSize = new System.Drawing.Size(629, 529);
            this.ControlBox = false;
            this.Controls.Add(this.btnViewCertificate);
            this.Controls.Add(this.btnSelectCertificate);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lvCertificates);
            this.Controls.Add(this.label3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "CertificateSelectionScreen";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Shown += new System.EventHandler(this.CertificateSelectionScreen_Shown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListView lvCertificates;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSelectCertificate;
        private System.Windows.Forms.Button btnViewCertificate;
    }
}