namespace CMS.Setup.Screens
{
    partial class SelectInstallFolderScreen
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SelectInstallFolderScreen));
            this.label1 = new System.Windows.Forms.Label();
            this.tbInstallFolder = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(3, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(350, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Select the folder where Corkscrew (or its components) are to be installed:";
            // 
            // tbInstallFolder
            // 
            this.tbInstallFolder.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbInstallFolder.Location = new System.Drawing.Point(6, 31);
            this.tbInstallFolder.Name = "tbInstallFolder";
            this.tbInstallFolder.ReadOnly = true;
            this.tbInstallFolder.Size = new System.Drawing.Size(741, 20);
            this.tbInstallFolder.TabIndex = 1;
            this.tbInstallFolder.WordWrap = false;
            this.tbInstallFolder.DoubleClick += new System.EventHandler(this.tbInstallFolder_DoubleClick);
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(3, 68);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(745, 410);
            this.label2.TabIndex = 2;
            this.label2.Text = resources.GetString("label2.Text");
            // 
            // SelectInstallFolderScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbInstallFolder);
            this.Controls.Add(this.label1);
            this.Name = "SelectInstallFolderScreen";
            this.Load += new System.EventHandler(this.SelectInstallFolderScreen_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbInstallFolder;
        private System.Windows.Forms.Label label2;
    }
}
