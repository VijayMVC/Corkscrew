using CMS.Setup.Installers;
using Corkscrew.SDK.tools;
using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace CMS.Setup.Screens
{
    public partial class ProgressScreen : ScreenTemplate
    {
        public ProgressScreen()
        {
            InitializeComponent();
        }

        public override void InitializeUI()
        {
            RunInstallers();
        }

        private void RunInstallers()
        {
            bool result = true, componentResult = true, wasFatalAbort = false;
            int eachComponentIncreasesOverAllBy = 0;

            pbOverall.Value = 0;
            pbCurrent.Value = 0;
            lblComponentName.Text = "Initializing...";
            lblComponentDescription.Text = "Please wait while Setup is determining the best way to do this...";
            tbProgressMessages.Text = "Initializing...";

            Application.DoEvents();

            InstallWizardForm wizard = (InstallWizardForm)ParentForm;
            wizard.SetupManifest.ResolvePaths();
            ComponentInstallerCollection componentCollection = new ComponentInstallerCollection(wizard.SetupManifest.Installers.Where(c => (c.ActionToExecute != ActionTypeEnum.Undefined)));


            // check how many are to be executed
            eachComponentIncreasesOverAllBy = (100 / componentCollection.Count());
            foreach (ComponentInstaller component in componentCollection)
            {
                componentResult = true;

                pbOverall.Value += eachComponentIncreasesOverAllBy;
                lblComponentName.Text = component.ComponentName;
                lblComponentDescription.Text = component.Description;

                tbProgressMessages.AppendText(Environment.NewLine + "----------------------------------------" + Environment.NewLine +
                                            Enum.GetName(typeof(ActionTypeEnum), component.ActionToExecute) + "ing component: " + component.ComponentName + Environment.NewLine);


                if (!component.EvaluateFarmState(wizard.SetupManifest))
                {
                    // find the farm installer, if it was not already run and failed or uninstalled, then add it as a dependency and install it.
                    ComponentInstaller farmInstaller = wizard.SetupManifest.Installers.Find("Corkscrew_ConfigDB");
                    if ((farmInstaller != null) && ((farmInstaller.Status != LastActionState.InstallFailed) && (farmInstaller.Status != LastActionState.RolledBack) && (farmInstaller.Status != LastActionState.Uninstalled) && (farmInstaller.Status != LastActionState.UninstallFailed)))
                    {
                        if (component.Dependencies.Find("Corkscrew_ConfigDB") == null)
                        {
                            component.Dependencies.Add(farmInstaller);  // we will take care of this when Install() is fired
                        }
                    }
                    else
                    {
                        tbProgressMessages.AppendText(Environment.NewLine + "Farm required state was not met. Not installing component.");
                        componentResult = false;
                    }
                }

                if (componentResult && (component.ActionToExecute != ActionTypeEnum.Uninstall))
                {
                    tbProgressMessages.AppendText(Environment.NewLine + "Extracting component cabinet... ");
                    if (EnsureComponentCABExtracted(component))
                    {
                        tbProgressMessages.AppendText("[Done]");
                    }
                    else
                    {
                        tbProgressMessages.AppendText(Environment.NewLine + "[Failed]: CAB file for this component was not found.");
                        componentResult = false;
                    }
                }

                if (componentResult)
                {
                    // attach event listeners
                    tbProgressMessages.AppendText(Environment.NewLine + "Signing up for child-installer events... ");
                    foreach (IProgressableInstaller installer in component.Installers)
                    {
                        installer.ProgressChanged += Installer_ProgressChanged;
                    }
                    tbProgressMessages.AppendText("[Done]");


                    switch (component.ActionToExecute)
                    {
                        case ActionTypeEnum.Install:
                            tbProgressMessages.AppendText(Environment.NewLine + "Installing... ");
                            componentResult = component.Install();
                            break;

                        case ActionTypeEnum.Repair:
                            tbProgressMessages.AppendText(Environment.NewLine + "Repairing... ");
                            componentResult = component.Repair();
                            break;

                        case ActionTypeEnum.Uninstall:
                            tbProgressMessages.AppendText(Environment.NewLine + "Uninstalling... ");
                            componentResult = component.Uninstall();
                            break;
                    }
                    tbProgressMessages.AppendText("[Done]");

                    tbProgressMessages.AppendText(Environment.NewLine + "Cleaning up temporary files created... ");
                    CleanupCabFolder(component);
                    tbProgressMessages.AppendText("[Done]");
                }

                if (!componentResult)
                {
                    tbProgressMessages.AppendText(Environment.NewLine + "----------------------------------------" + Environment.NewLine +
                                            "Failed " + Enum.GetName(typeof(ActionTypeEnum), component.ActionToExecute) + "ing component: " + component.ComponentName + Environment.NewLine +
                                            "Rolling back... " + Environment.NewLine);
                    component.Rollback();

                    if ((component.ActionToExecute == ActionTypeEnum.Install) && (component.InstallFailureIsFatal))
                    {
                        tbProgressMessages.AppendText(Environment.NewLine + "----------------------------------------" + Environment.NewLine +
                                            "Component: " + component.ComponentName + " was marked as critical. Aborting setup..." + Environment.NewLine);
                        wasFatalAbort = true;
                    }
                }

                // detach event listeners
                tbProgressMessages.AppendText(Environment.NewLine + "Unsubscribing from child-installer events... ");
                foreach (IProgressableInstaller installer in component.Installers)
                {
                    installer.ProgressChanged -= Installer_ProgressChanged;
                }

                tbProgressMessages.AppendText("[Done]");

                result = result & componentResult;
                if (wasFatalAbort)
                {
                    break;
                }
            }

            if ((!result) && wasFatalAbort)
            {
                // rollback everything
                foreach (ComponentInstaller component in componentCollection)
                {
                    if (component.Status == LastActionState.Installed)
                    {
                        tbProgressMessages.AppendText(Environment.NewLine + "----------------------------------------" + Environment.NewLine +
                                                "Rolling back component: " + component.ComponentName + Environment.NewLine);
                        component.Rollback();
                    }
                }
            }

            tbProgressMessages.AppendText(Environment.NewLine + "----------------------------------------" + Environment.NewLine + "Cleaning up... ");
            if (Directory.Exists(wizard.SetupManifest.CabExtractDirectoryRoot))
            {
                Directory.Delete(wizard.SetupManifest.CabExtractDirectoryRoot, true);
            }

            // if nothing was installed (or everything was uninstalled or rolled back), the install target directory may still exist. delete it
            if (Directory.Exists(wizard.SetupManifest.InstallBaseDirectory))
            {
                if (Directory.GetFiles(wizard.SetupManifest.InstallBaseDirectory, "*.*", SearchOption.AllDirectories).Length == 0)
                {
                    Directory.Delete(wizard.SetupManifest.InstallBaseDirectory, true);
                }
            }

            // clean up the registry
            RegistryKeyAction.CleanupRegistryKey();

            tbProgressMessages.AppendText("[Done]");

            // if we have something installed, register with Windows for uninstall
            if (wizard.SetupManifest.Installers.Where(c => (c.Status == LastActionState.Installed)).Count() > 0)
            {
                tbProgressMessages.AppendText(Environment.NewLine + "----------------------------------------" + Environment.NewLine + "Registering with Windows Uninstall System... ");
                RegistryKeyAction.RegisterWithWindows(wizard.SetupManifest.InstallBaseDirectory);
                tbProgressMessages.AppendText("[Done]");
            }

            pbOverall.Value = 100;
            pbCurrent.Value = 100;
            tbProgressMessages.AppendText(Environment.NewLine + "----------------------------------------" + Environment.NewLine +
                                        "Setup has finished making changes to your system(s)." + Environment.NewLine +
                                        "Please click on Next to proceed..." + Environment.NewLine);

        }

        private void Installer_ProgressChanged(int value, string text)
        {
            if (value > 0)
            {
                text = Environment.NewLine + text;
            }

            int newValue = pbCurrent.Value + (value * 10);  // 10 is step value
            if ((newValue < 0) || (newValue > 100))
            {
                newValue = 0;
            }

            if (tbProgressMessages.InvokeRequired)
            {
                tbProgressMessages.Invoke(
                    new MethodInvoker(
                        () => {
                            tbProgressMessages.AppendText(text);
                        }
                    )
                );
            }
            else
            {
                tbProgressMessages.AppendText(text);
            }

            if (pbCurrent.InvokeRequired)
            {
                pbCurrent.Invoke(
                    new MethodInvoker(
                        () => {
                            pbCurrent.Value = newValue;
                        }
                    )
                );
            }
            else
            {
                pbCurrent.Value = newValue;
            }

            Application.DoEvents();
        }

        private bool EnsureComponentCABExtracted(ComponentInstaller component)
        {
            string cabFilePath = Path.Combine(Application.StartupPath, Utility.SafeString(component.CABFileName, expectEnd: ".cab"));
            if (!File.Exists(cabFilePath))
            {
                return false;
            }

            SelfExtractor sfx = new SelfExtractor();
            sfx.Extract(cabFilePath);

            return true;
        }

        private void CleanupCabFolder(ComponentInstaller component)
        {
            string extractedPath = Path.Combine(Application.StartupPath, "_layout", Path.GetFileNameWithoutExtension(component.CABFileName));
            if (Directory.Exists(extractedPath))
            {
                Directory.Delete(extractedPath, true);
            }
        }

        private void tbProgressMessages_TextChanged(object sender, EventArgs e)
        {
            // too many places where text is written out to this textbox. Force a wait
            Application.DoEvents();
        }
    }
}
