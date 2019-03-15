using CMS.Setup.Installers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace CMS.Setup.Screens
{
    public partial class InstallWizardForm : Form
    {

        #region Properties and Fields

        private Stack<InstallWizardStep> _forwardSteps = new Stack<InstallWizardStep>();        // steps for Next
        private Stack<InstallWizardStep> _backwardSteps = new Stack<InstallWizardStep>();       // steps for Back
        private InstallWizardStep _currentStep = null;                                          // currently shown step

        public OperationManifest SetupManifest = null;

        #endregion


        public InstallWizardForm()
        {
            InitializeComponent();
        }

        private void lblFormMoveDragTarget_MouseDown(object sender, MouseEventArgs e)
        {
            UI.EnableBorderlessFormMove(this.Handle, e);
        }

        private void InstallWizardForm_Shown(object sender, EventArgs e)
        {
            tbWorkingDirectory.Text = Application.StartupPath;
#if DEBUG
            tbWorkingDirectory.Visible = true;
#else
            tbWorkingDirectory.Visible = false;
#endif

            #region Initialize Wizard Steps

            _forwardSteps = new Stack<InstallWizardStep>();

            // The collection is a STACK... so remember to push steps in the opposite order to how we want them to show up!!!

            _forwardSteps.Push(
                new InstallWizardStep()
                {
                    StepName = "FinishScreen",
                    Title = "Setup complete",
                    Description = "Setup has finished making changes to your system. Review the logs below and click Finish when done.",
                    StepScreen = new FinishScreen()
                }
            );

            _forwardSteps.Push(
                new InstallWizardStep()
                {
                    StepName = "SetupProgress",
                    Title = "Setup progress",
                    Description = "Follow the progress of the setup program's actions in the log below.",
                    StepScreen = new ProgressScreen()
                }
            );

            InstallWizardStep iisStep = new InstallWizardStep()
            {
                StepName = "ConfigureIIS",
                Title = "Configure IIS",
                Description = "Since you opted to install a component that requires to be installed into IIS, select configuration for these components here. Required components in IIS will be installed into Windows automatically.",
                StepScreen = new IISConnectionScreen()
            };
            iisStep.RequiredIfComponentsSelectedForInstall.Add("ControlCenter");
            iisStep.RequiredIfComponentsSelectedForInstall.Add("APIService");

            _forwardSteps.Push(
                iisStep
            );

            InstallWizardStep databaseStep = new InstallWizardStep()
            {
                StepName = "ConnectToDatabase",
                Title = "Connect to database",
                Description = "Provide the details to be able to connect to the database engine to deploy the Farm database (ConfigDB).",
                StepScreen = new DatabaseConnectionScreen()
            };
            databaseStep.RequiredIfComponentsSelectedForInstall.Add("Corkscrew_ConfigDB");
            databaseStep.RequiredIfComponentsSelectedForInstall.Add("APIService");
            databaseStep.RequiredIfComponentsSelectedForInstall.Add("ControlCenter");
            databaseStep.RequiredIfComponentsSelectedForInstall.Add("Drive");
            databaseStep.RequiredIfComponentsSelectedForInstall.Add("Explorer");
            databaseStep.RequiredIfComponentsSelectedForInstall.Add("WorkflowService");

            _forwardSteps.Push(
                databaseStep
            );

            _forwardSteps.Push(
                new InstallWizardStep()
                {
                    StepName = "SelectComponents",
                    Title = "Select components to install, repair and uninstall",
                    Description = "Select the components of Corkscrew to be installed, uninstalled or repaired.",
                    StepScreen = new SelectComponentsScreen()
                }
            );

            _forwardSteps.Push(
                new InstallWizardStep()
                {
                    StepName = "SelectInstallFolder",
                    Title = "Select installation folder",
                    Description = "Below, select the folder where you want to install the components you selected.",
                    StepScreen = new SelectInstallFolderScreen()
                }
            );

            _forwardSteps.Push(
                new InstallWizardStep()
                {
                    StepName = "Welcome",
                    Title = "Welcome",
                    Description = "This is the installation wizard for the Aquarius Corkscrew Content Management System.",
                    StepScreen = new WelcomeScreen()
                }
            );
#endregion

#region Determine and load components

            SetupManifest = new OperationManifest()
            {
                InstallBaseDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Aquarius Operating Systems", "CMS"),
                CabExtractDirectoryRoot = Path.Combine(Application.StartupPath, "_layout")
            };

#endregion

            GoForward();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (UI.ShowMessage(this, "This will exit the installer. Are you sure this is what you wish to do?", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                ExitApplication();
            }
        }

        private void btnStepBack_Click(object sender, EventArgs e)
        {
            GoBackward();
        }

        private void btnStepForward_Click(object sender, EventArgs e)
        {
            GoForward();
        }

        private void tbWorkingDirectory_DoubleClick(object sender, EventArgs e)
        {
            Process.Start("explorer.exe", tbWorkingDirectory.Text);
        }

        private void SetScreen()
        {
            if (_currentStep == null)
            {
                UI.ShowMessage(this, "Invalid screen to be shown!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                ExitApplication();
            }

            UseWaitCursor = true;
            this.Cursor = Cursors.WaitCursor;

            lblStepTitle.Text = _currentStep.Title;
            lblStepDescription.Text = _currentStep.Description;

            panelStepControls.Controls.Clear();
            panelStepControls.Controls.Add(_currentStep.StepScreen);
            _currentStep.StepScreen.InitializeUI();                         // reloads controls on that screen

            if (_forwardSteps.Count == 0)
            {
                btnStepForward.Text = "&Finish";
            }
            else
            {
                btnStepForward.Text = "&Next >>";
            }

            btnStepBack.Enabled = (_backwardSteps.Count > 0);

            UseWaitCursor = false;
            this.Cursor = Cursors.Default;
        }

        private void GoForward()
        {
            if (btnStepForward.Text == "&Finish")
            {
                ExitApplication();
                return;
            }

            if (_forwardSteps.Peek().StepScreen is ProgressScreen)
            {
                if (SetupManifest.Installers.Where(c => (c.ActionToExecute != ActionTypeEnum.Undefined)).Count() == 0)
                {
                    UI.ShowMessage(this, "You have not selected any components to install. If you wish to exit without installing anything, please click on the Cancel button.");
                    return;
                }
            }

            if (_currentStep != null)
            {
                _backwardSteps.Push(_currentStep);
            }

            _currentStep = _forwardSteps.Pop();
            if (_currentStep.RequiredIfComponentsSelectedForInstall.Count > 0)
            {
                bool requiredToShow = false;

                foreach (string componentName in _currentStep.RequiredIfComponentsSelectedForInstall)
                {
                    foreach (ComponentInstaller installer in SetupManifest.Installers)
                    {
                        if (installer.ComponentName.Equals(componentName, StringComparison.InvariantCultureIgnoreCase))
                        {
                            if ((installer.ActionToExecute == ActionTypeEnum.Install) || (installer.ActionToExecute == ActionTypeEnum.Repair))
                            {
                                requiredToShow = true;
                                break;
                            }
                        }
                    }

                    if (requiredToShow)
                    {
                        break;
                    }
                }

                if (!requiredToShow)
                {
                    GoForward();
                    return;
                }
            }

            SetScreen();
        }

        private void GoBackward()
        {
            _forwardSteps.Push(_currentStep);
            _currentStep = _backwardSteps.Pop();

            if (_currentStep.RequiredIfComponentsSelectedForInstall.Count > 0)
            {
                bool requiredToShow = false;

                foreach (string componentName in _currentStep.RequiredIfComponentsSelectedForInstall)
                {
                    foreach (ComponentInstaller installer in SetupManifest.Installers)
                    {
                        if (installer.ComponentName.Equals(componentName, StringComparison.InvariantCultureIgnoreCase))
                        {
                            if ((installer.ActionToExecute == ActionTypeEnum.Install) || (installer.ActionToExecute == ActionTypeEnum.Repair))
                            {
                                requiredToShow = true;
                                break;
                            }
                        }
                    }

                    if (requiredToShow)
                    {
                        break;
                    }
                }

                if (!requiredToShow)
                {
                    GoBackward();
                    return;
                }
            }

            SetScreen();
        }

        private void ExitApplication()
        {
            this.Close();
            Application.Exit();
        }


        private class InstallWizardStep
        {
#region Properties

            /// <summary>
            /// An internal name of this step
            /// </summary>
            public string StepName
            {
                get;
                set;
            }

            /// <summary>
            /// Title to be shown on the UI
            /// </summary>
            public string Title
            {
                get;
                set;
            }

            /// <summary>
            /// Description to be shown on the UI
            /// </summary>
            public string Description
            {
                get;
                set;
            }

            /// <summary>
            /// List of names of components (ComponentInstaller.ComponentName) of the components that 
            /// require this step to be shown.
            /// </summary>
            public List<string> RequiredIfComponentsSelectedForInstall
            {
                get
                {
                    if (_required == null)
                    {
                        _required = new List<string>();
                    }

                    return _required;
                }
            }
            private List<string> _required = null;

            /// <summary>
            /// The screen shown for this step
            /// </summary>
            public ScreenTemplate StepScreen
            {
                get;
                set;
            }

#endregion
        }
    }
}
