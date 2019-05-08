namespace Corkscrew.Explorer
{
    partial class frmChangeGroupMembership
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmChangeGroupMembership));
            this.btnFormClose = new System.Windows.Forms.Button();
            this.lblLogoText = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblGroupname = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lbAvailableUsers = new System.Windows.Forms.ListBox();
            this.lbGroupMembers = new System.Windows.Forms.ListBox();
            this.btnAddToGroup = new System.Windows.Forms.Button();
            this.btnRemoveFromGroup = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnFormClose
            // 
            this.btnFormClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFormClose.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.btnFormClose.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.ButtonFace;
            this.btnFormClose.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ButtonShadow;
            this.btnFormClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFormClose.Font = new System.Drawing.Font("Microsoft Himalaya", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFormClose.Location = new System.Drawing.Point(763, 1);
            this.btnFormClose.Name = "btnFormClose";
            this.btnFormClose.Size = new System.Drawing.Size(60, 47);
            this.btnFormClose.TabIndex = 55;
            this.btnFormClose.Text = "X";
            this.btnFormClose.UseVisualStyleBackColor = true;
            this.btnFormClose.Click += new System.EventHandler(this.btnFormClose_Click);
            this.btnFormClose.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lblLogoText_MouseDown);
            // 
            // lblLogoText
            // 
            this.lblLogoText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblLogoText.Font = new System.Drawing.Font("Microsoft Himalaya", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLogoText.Image = ((System.Drawing.Image)(resources.GetObject("lblLogoText.Image")));
            this.lblLogoText.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.lblLogoText.Location = new System.Drawing.Point(0, -1);
            this.lblLogoText.Name = "lblLogoText";
            this.lblLogoText.Size = new System.Drawing.Size(766, 107);
            this.lblLogoText.TabIndex = 54;
            this.lblLogoText.Text = "Group Members";
            this.lblLogoText.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            this.lblLogoText.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lblLogoText_MouseDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(33, 123);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 13);
            this.label1.TabIndex = 56;
            this.label1.Text = "Group name:";
            // 
            // lblGroupname
            // 
            this.lblGroupname.AutoSize = true;
            this.lblGroupname.Location = new System.Drawing.Point(120, 123);
            this.lblGroupname.Name = "lblGroupname";
            this.lblGroupname.Size = new System.Drawing.Size(66, 13);
            this.lblGroupname.TabIndex = 57;
            this.lblGroupname.Text = "(groupname)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(33, 151);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(202, 13);
            this.label2.TabIndex = 58;
            this.label2.Text = "Users who are not members of this group:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(476, 151);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(184, 13);
            this.label3.TabIndex = 59;
            this.label3.Text = "Users who are members of this group:";
            // 
            // lbAvailableUsers
            // 
            this.lbAvailableUsers.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lbAvailableUsers.FormattingEnabled = true;
            this.lbAvailableUsers.Location = new System.Drawing.Point(36, 167);
            this.lbAvailableUsers.Name = "lbAvailableUsers";
            this.lbAvailableUsers.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbAvailableUsers.Size = new System.Drawing.Size(327, 351);
            this.lbAvailableUsers.TabIndex = 0;
            // 
            // lbGroupMembers
            // 
            this.lbGroupMembers.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lbGroupMembers.FormattingEnabled = true;
            this.lbGroupMembers.Location = new System.Drawing.Point(479, 167);
            this.lbGroupMembers.Name = "lbGroupMembers";
            this.lbGroupMembers.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbGroupMembers.Size = new System.Drawing.Size(327, 351);
            this.lbGroupMembers.TabIndex = 2;
            // 
            // btnAddToGroup
            // 
            this.btnAddToGroup.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddToGroup.Location = new System.Drawing.Point(385, 282);
            this.btnAddToGroup.Name = "btnAddToGroup";
            this.btnAddToGroup.Size = new System.Drawing.Size(75, 23);
            this.btnAddToGroup.TabIndex = 1;
            this.btnAddToGroup.Text = ">>";
            this.btnAddToGroup.UseVisualStyleBackColor = true;
            this.btnAddToGroup.Click += new System.EventHandler(this.btnAddToGroup_Click);
            // 
            // btnRemoveFromGroup
            // 
            this.btnRemoveFromGroup.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRemoveFromGroup.Location = new System.Drawing.Point(385, 322);
            this.btnRemoveFromGroup.Name = "btnRemoveFromGroup";
            this.btnRemoveFromGroup.Size = new System.Drawing.Size(75, 23);
            this.btnRemoveFromGroup.TabIndex = 3;
            this.btnRemoveFromGroup.Text = "<<";
            this.btnRemoveFromGroup.UseVisualStyleBackColor = true;
            this.btnRemoveFromGroup.Click += new System.EventHandler(this.btnRemoveFromGroup_Click);
            // 
            // frmChangeGroupMembership
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this.ClientSize = new System.Drawing.Size(824, 546);
            this.Controls.Add(this.btnRemoveFromGroup);
            this.Controls.Add(this.btnAddToGroup);
            this.Controls.Add(this.lbGroupMembers);
            this.Controls.Add(this.lbAvailableUsers);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblGroupname);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnFormClose);
            this.Controls.Add(this.lblLogoText);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmChangeGroupMembership";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "frmChangeGroupMembership";
            this.Shown += new System.EventHandler(this.frmChangeGroupMembership_Shown);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lblLogoText_MouseDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnFormClose;
        private System.Windows.Forms.Label lblLogoText;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblGroupname;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListBox lbAvailableUsers;
        private System.Windows.Forms.ListBox lbGroupMembers;
        private System.Windows.Forms.Button btnAddToGroup;
        private System.Windows.Forms.Button btnRemoveFromGroup;
    }
}