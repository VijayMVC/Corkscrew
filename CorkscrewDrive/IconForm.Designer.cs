namespace Corkscrew.Drive
{
    partial class IconForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IconForm));
            this.NotificationAreaIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.cmsOptions = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiPauseSync = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiResumeSync = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiExit = new System.Windows.Forms.ToolStripMenuItem();
            this.tbLog = new System.Windows.Forms.TextBox();
            this.lblLogoText = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.cmsOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // NotificationAreaIcon
            // 
            this.NotificationAreaIcon.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.NotificationAreaIcon.BalloonTipText = resources.GetString("NotificationAreaIcon.BalloonTipText");
            this.NotificationAreaIcon.BalloonTipTitle = "Corkscrew Drive";
            this.NotificationAreaIcon.ContextMenuStrip = this.cmsOptions;
            this.NotificationAreaIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("NotificationAreaIcon.Icon")));
            this.NotificationAreaIcon.Text = "Corkscrew Drive";
            this.NotificationAreaIcon.Visible = true;
            // 
            // cmsOptions
            // 
            this.cmsOptions.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiAbout,
            this.tsmiSettings,
            this.toolStripMenuItem1,
            this.tsmiPauseSync,
            this.tsmiResumeSync,
            this.toolStripMenuItem2,
            this.tsmiExit});
            this.cmsOptions.Name = "cmsOptions";
            this.cmsOptions.Size = new System.Drawing.Size(144, 126);
            // 
            // tsmiAbout
            // 
            this.tsmiAbout.Name = "tsmiAbout";
            this.tsmiAbout.Size = new System.Drawing.Size(143, 22);
            this.tsmiAbout.Text = "&About";
            this.tsmiAbout.Click += new System.EventHandler(this.tsmiAbout_Click);
            // 
            // tsmiSettings
            // 
            this.tsmiSettings.Name = "tsmiSettings";
            this.tsmiSettings.Size = new System.Drawing.Size(143, 22);
            this.tsmiSettings.Text = "&Settings";
            this.tsmiSettings.Click += new System.EventHandler(this.tsmiSettings_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(140, 6);
            // 
            // tsmiPauseSync
            // 
            this.tsmiPauseSync.Name = "tsmiPauseSync";
            this.tsmiPauseSync.Size = new System.Drawing.Size(143, 22);
            this.tsmiPauseSync.Text = "P&ause sync";
            this.tsmiPauseSync.Click += new System.EventHandler(this.tsmiPauseSync_Click);
            // 
            // tsmiResumeSync
            // 
            this.tsmiResumeSync.Name = "tsmiResumeSync";
            this.tsmiResumeSync.Size = new System.Drawing.Size(143, 22);
            this.tsmiResumeSync.Text = "R&esume sync";
            this.tsmiResumeSync.Click += new System.EventHandler(this.tsmiResumeSync_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(140, 6);
            // 
            // tsmiExit
            // 
            this.tsmiExit.Name = "tsmiExit";
            this.tsmiExit.Size = new System.Drawing.Size(143, 22);
            this.tsmiExit.Text = "E&xit";
            this.tsmiExit.Click += new System.EventHandler(this.tsmiExit_Click);
            // 
            // tbLog
            // 
            this.tbLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbLog.Location = new System.Drawing.Point(10, 140);
            this.tbLog.Multiline = true;
            this.tbLog.Name = "tbLog";
            this.tbLog.ReadOnly = true;
            this.tbLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbLog.Size = new System.Drawing.Size(504, 105);
            this.tbLog.TabIndex = 1;
            // 
            // lblLogoText
            // 
            this.lblLogoText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblLogoText.Font = new System.Drawing.Font("Microsoft Himalaya", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLogoText.Image = ((System.Drawing.Image)(resources.GetObject("lblLogoText.Image")));
            this.lblLogoText.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.lblLogoText.Location = new System.Drawing.Point(-4, -1);
            this.lblLogoText.Name = "lblLogoText";
            this.lblLogoText.Size = new System.Drawing.Size(528, 107);
            this.lblLogoText.TabIndex = 79;
            this.lblLogoText.Text = "Drive";
            this.lblLogoText.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            this.lblLogoText.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lblLogoText_MouseDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 124);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 13);
            this.label1.TabIndex = 80;
            this.label1.Text = "Activity log:";
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Location = new System.Drawing.Point(439, 109);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 81;
            this.btnClose.Text = "&Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // IconForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this.ClientSize = new System.Drawing.Size(526, 257);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblLogoText);
            this.Controls.Add(this.tbLog);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "IconForm";
            this.Text = "Aquarius Corkscrew Drive";
            this.Shown += new System.EventHandler(this.IconForm_Shown);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lblLogoText_MouseDown);
            this.cmsOptions.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.NotifyIcon NotificationAreaIcon;
        private System.Windows.Forms.ContextMenuStrip cmsOptions;
        private System.Windows.Forms.ToolStripMenuItem tsmiAbout;
        private System.Windows.Forms.ToolStripMenuItem tsmiSettings;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem tsmiPauseSync;
        private System.Windows.Forms.ToolStripMenuItem tsmiResumeSync;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem tsmiExit;
        private System.Windows.Forms.TextBox tbLog;
        private System.Windows.Forms.Label lblLogoText;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnClose;
    }
}