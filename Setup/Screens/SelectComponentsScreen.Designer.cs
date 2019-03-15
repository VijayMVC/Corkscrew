namespace CMS.Setup.Screens
{
    partial class SelectComponentsScreen
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
            this.clbComponentsForInstall = new System.Windows.Forms.CheckedListBox();
            this.clbComponentsForRepair = new System.Windows.Forms.CheckedListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.clbComponentsForUninstall = new System.Windows.Forms.CheckedListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.lblComponentDescription = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(4, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(143, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Select components to Install:";
            // 
            // clbComponentsForInstall
            // 
            this.clbComponentsForInstall.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.clbComponentsForInstall.FormattingEnabled = true;
            this.clbComponentsForInstall.Location = new System.Drawing.Point(7, 21);
            this.clbComponentsForInstall.Name = "clbComponentsForInstall";
            this.clbComponentsForInstall.Size = new System.Drawing.Size(248, 120);
            this.clbComponentsForInstall.TabIndex = 1;
            this.clbComponentsForInstall.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.ComponentListBoxes_ItemCheck);
            this.clbComponentsForInstall.SelectedIndexChanged += new System.EventHandler(this.ComponentListBoxes_SelectedIndexChanged);
            // 
            // clbComponentsForRepair
            // 
            this.clbComponentsForRepair.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.clbComponentsForRepair.FormattingEnabled = true;
            this.clbComponentsForRepair.Location = new System.Drawing.Point(7, 175);
            this.clbComponentsForRepair.Name = "clbComponentsForRepair";
            this.clbComponentsForRepair.Size = new System.Drawing.Size(248, 120);
            this.clbComponentsForRepair.TabIndex = 3;
            this.clbComponentsForRepair.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.ComponentListBoxes_ItemCheck);
            this.clbComponentsForRepair.SelectedIndexChanged += new System.EventHandler(this.ComponentListBoxes_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(4, 158);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(147, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Select components to Repair:";
            // 
            // clbComponentsForUninstall
            // 
            this.clbComponentsForUninstall.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.clbComponentsForUninstall.FormattingEnabled = true;
            this.clbComponentsForUninstall.Location = new System.Drawing.Point(7, 329);
            this.clbComponentsForUninstall.Name = "clbComponentsForUninstall";
            this.clbComponentsForUninstall.Size = new System.Drawing.Size(248, 120);
            this.clbComponentsForUninstall.TabIndex = 5;
            this.clbComponentsForUninstall.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.ComponentListBoxes_ItemCheck);
            this.clbComponentsForUninstall.SelectedIndexChanged += new System.EventHandler(this.ComponentListBoxes_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(4, 312);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(156, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Select components to Uninstall:";
            // 
            // lblComponentDescription
            // 
            this.lblComponentDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblComponentDescription.ForeColor = System.Drawing.Color.White;
            this.lblComponentDescription.Location = new System.Drawing.Point(271, 20);
            this.lblComponentDescription.Name = "lblComponentDescription";
            this.lblComponentDescription.Size = new System.Drawing.Size(458, 433);
            this.lblComponentDescription.TabIndex = 6;
            this.lblComponentDescription.Text = "Click on a component to display its description here.";
            // 
            // SelectComponentsScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblComponentDescription);
            this.Controls.Add(this.clbComponentsForUninstall);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.clbComponentsForRepair);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.clbComponentsForInstall);
            this.Controls.Add(this.label1);
            this.Name = "SelectComponentsScreen";
            this.Load += new System.EventHandler(this.SelectComponentsScreen_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckedListBox clbComponentsForInstall;
        private System.Windows.Forms.CheckedListBox clbComponentsForRepair;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckedListBox clbComponentsForUninstall;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblComponentDescription;
    }
}
