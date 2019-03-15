namespace CMS.Setup.Screens
{
    partial class ProgressScreen
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.pbOverall = new System.Windows.Forms.ProgressBar();
            this.pbCurrent = new System.Windows.Forms.ProgressBar();
            this.lblComponentName = new System.Windows.Forms.Label();
            this.lblComponentDescription = new System.Windows.Forms.Label();
            this.tbProgressMessages = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(3, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(728, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "Overall progress";
            // 
            // label2
            // 
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(3, 91);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(115, 23);
            this.label2.TabIndex = 1;
            this.label2.Text = "Current component:";
            // 
            // pbOverall
            // 
            this.pbOverall.BackColor = System.Drawing.SystemColors.Control;
            this.pbOverall.Location = new System.Drawing.Point(6, 36);
            this.pbOverall.Name = "pbOverall";
            this.pbOverall.Size = new System.Drawing.Size(725, 23);
            this.pbOverall.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.pbOverall.TabIndex = 2;
            // 
            // pbCurrent
            // 
            this.pbCurrent.BackColor = System.Drawing.SystemColors.Control;
            this.pbCurrent.Location = new System.Drawing.Point(6, 117);
            this.pbCurrent.Name = "pbCurrent";
            this.pbCurrent.Size = new System.Drawing.Size(725, 23);
            this.pbCurrent.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.pbCurrent.TabIndex = 3;
            // 
            // lblComponentName
            // 
            this.lblComponentName.ForeColor = System.Drawing.Color.White;
            this.lblComponentName.Location = new System.Drawing.Point(115, 91);
            this.lblComponentName.Name = "lblComponentName";
            this.lblComponentName.Size = new System.Drawing.Size(616, 23);
            this.lblComponentName.TabIndex = 4;
            // 
            // lblComponentDescription
            // 
            this.lblComponentDescription.ForeColor = System.Drawing.Color.White;
            this.lblComponentDescription.Location = new System.Drawing.Point(6, 143);
            this.lblComponentDescription.Name = "lblComponentDescription";
            this.lblComponentDescription.Size = new System.Drawing.Size(725, 100);
            this.lblComponentDescription.TabIndex = 5;
            // 
            // tbProgressMessages
            // 
            this.tbProgressMessages.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this.tbProgressMessages.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbProgressMessages.ForeColor = System.Drawing.Color.White;
            this.tbProgressMessages.Location = new System.Drawing.Point(6, 256);
            this.tbProgressMessages.Multiline = true;
            this.tbProgressMessages.Name = "tbProgressMessages";
            this.tbProgressMessages.ReadOnly = true;
            this.tbProgressMessages.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbProgressMessages.Size = new System.Drawing.Size(725, 212);
            this.tbProgressMessages.TabIndex = 6;
            this.tbProgressMessages.WordWrap = false;
            this.tbProgressMessages.TextChanged += new System.EventHandler(this.tbProgressMessages_TextChanged);
            // 
            // ProgressScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tbProgressMessages);
            this.Controls.Add(this.lblComponentDescription);
            this.Controls.Add(this.pbCurrent);
            this.Controls.Add(this.pbOverall);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblComponentName);
            this.Name = "ProgressScreen";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ProgressBar pbOverall;
        private System.Windows.Forms.ProgressBar pbCurrent;
        private System.Windows.Forms.Label lblComponentName;
        private System.Windows.Forms.Label lblComponentDescription;
        private System.Windows.Forms.TextBox tbProgressMessages;
    }
}
