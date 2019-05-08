namespace Corkscrew.workflow.WindowsService
{
    partial class ProjectInstaller
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
            this.spiWorkflowServiceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.siWorkflowServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // spiWorkflowServiceProcessInstaller
            // 
            this.spiWorkflowServiceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.NetworkService;
            this.spiWorkflowServiceProcessInstaller.Password = null;
            this.spiWorkflowServiceProcessInstaller.Username = null;
            // 
            // siWorkflowServiceInstaller
            // 
            this.siWorkflowServiceInstaller.DelayedAutoStart = true;
            this.siWorkflowServiceInstaller.Description = "Host and executive program that controls and runs the Corkscrew CMS workflow inst" +
    "ances. If this service is stopped or disabled, workflow instances may start but " +
    "will not execute.";
            this.siWorkflowServiceInstaller.DisplayName = "Aquarius Corkscrew Workflow Service";
            this.siWorkflowServiceInstaller.ServiceName = "CorkscrewWorkflowService";
            this.siWorkflowServiceInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.spiWorkflowServiceProcessInstaller,
            this.siWorkflowServiceInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller spiWorkflowServiceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller siWorkflowServiceInstaller;
    }
}