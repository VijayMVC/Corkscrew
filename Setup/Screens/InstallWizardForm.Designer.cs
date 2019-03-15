namespace CMS.Setup.Screens
{
    partial class InstallWizardForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InstallWizardForm));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lblStepTitle = new System.Windows.Forms.Label();
            this.lblStepDescription = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panelStepControls = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnStepBack = new System.Windows.Forms.Button();
            this.btnStepForward = new System.Windows.Forms.Button();
            this.tbWorkingDirectory = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pictureBox1.Image = global::CMS.Setup.Properties.Resources.corkscrew_logo_vertical_darkbg;
            this.pictureBox1.Location = new System.Drawing.Point(2, 50);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(152, 643);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // lblStepTitle
            // 
            this.lblStepTitle.Font = new System.Drawing.Font("Arial Black", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStepTitle.ForeColor = System.Drawing.Color.White;
            this.lblStepTitle.Location = new System.Drawing.Point(155, 51);
            this.lblStepTitle.Name = "lblStepTitle";
            this.lblStepTitle.Size = new System.Drawing.Size(753, 35);
            this.lblStepTitle.TabIndex = 1;
            this.lblStepTitle.Text = "Aquarius Corkscrew";
            // 
            // lblStepDescription
            // 
            this.lblStepDescription.ForeColor = System.Drawing.Color.White;
            this.lblStepDescription.Location = new System.Drawing.Point(160, 86);
            this.lblStepDescription.Name = "lblStepDescription";
            this.lblStepDescription.Size = new System.Drawing.Size(753, 52);
            this.lblStepDescription.TabIndex = 3;
            this.lblStepDescription.Text = "Corkscrew is an enterprise-grade content management system (CMS) from Aquarius Op" +
    "erating Systems.";
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(160, 144);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(753, 2);
            this.label1.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Location = new System.Drawing.Point(162, 635);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(753, 2);
            this.label2.TabIndex = 5;
            // 
            // panelStepControls
            // 
            this.panelStepControls.Location = new System.Drawing.Point(161, 150);
            this.panelStepControls.Name = "panelStepControls";
            this.panelStepControls.Size = new System.Drawing.Size(752, 482);
            this.panelStepControls.TabIndex = 6;
            // 
            // btnCancel
            // 
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(160, 644);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "C&ancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnStepBack
            // 
            this.btnStepBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStepBack.ForeColor = System.Drawing.Color.White;
            this.btnStepBack.Location = new System.Drawing.Point(757, 644);
            this.btnStepBack.Name = "btnStepBack";
            this.btnStepBack.Size = new System.Drawing.Size(75, 23);
            this.btnStepBack.TabIndex = 8;
            this.btnStepBack.Text = "<< &Back";
            this.btnStepBack.UseVisualStyleBackColor = true;
            this.btnStepBack.Click += new System.EventHandler(this.btnStepBack_Click);
            // 
            // btnStepForward
            // 
            this.btnStepForward.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStepForward.ForeColor = System.Drawing.Color.White;
            this.btnStepForward.Location = new System.Drawing.Point(838, 644);
            this.btnStepForward.Name = "btnStepForward";
            this.btnStepForward.Size = new System.Drawing.Size(75, 23);
            this.btnStepForward.TabIndex = 9;
            this.btnStepForward.Text = "&Next >>";
            this.btnStepForward.UseVisualStyleBackColor = true;
            this.btnStepForward.Click += new System.EventHandler(this.btnStepForward_Click);
            // 
            // tbWorkingDirectory
            // 
            this.tbWorkingDirectory.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this.tbWorkingDirectory.ForeColor = System.Drawing.Color.White;
            this.tbWorkingDirectory.Location = new System.Drawing.Point(160, 673);
            this.tbWorkingDirectory.Name = "tbWorkingDirectory";
            this.tbWorkingDirectory.ReadOnly = true;
            this.tbWorkingDirectory.Size = new System.Drawing.Size(755, 20);
            this.tbWorkingDirectory.TabIndex = 10;
            this.tbWorkingDirectory.Visible = false;
            this.tbWorkingDirectory.DoubleClick += new System.EventHandler(this.tbWorkingDirectory_DoubleClick);
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Black;
            this.label3.Font = new System.Drawing.Font("Arial Black", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(925, 51);
            this.label3.TabIndex = 11;
            this.label3.Text = "Aquarius Corkscrew Setup";
            this.label3.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lblFormMoveDragTarget_MouseDown);
            // 
            // InstallWizardForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this.ClientSize = new System.Drawing.Size(925, 699);
            this.ControlBox = false;
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbWorkingDirectory);
            this.Controls.Add(this.btnStepForward);
            this.Controls.Add(this.btnStepBack);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.panelStepControls);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblStepDescription);
            this.Controls.Add(this.lblStepTitle);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "InstallWizardForm";
            this.Text = "Corkscrew Setup";
            this.Shown += new System.EventHandler(this.InstallWizardForm_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lblStepTitle;
        private System.Windows.Forms.Label lblStepDescription;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panelStepControls;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnStepBack;
        private System.Windows.Forms.Button btnStepForward;
        private System.Windows.Forms.TextBox tbWorkingDirectory;
        private System.Windows.Forms.Label label3;
    }
}