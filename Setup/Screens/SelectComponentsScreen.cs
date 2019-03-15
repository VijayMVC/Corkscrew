using CMS.Setup.Installers;
using System;
using System.Windows.Forms;

namespace CMS.Setup.Screens
{
    public partial class SelectComponentsScreen : ScreenTemplate
    {
        private InstallWizardForm _parentForm = null;

        public SelectComponentsScreen()
        {
            InitializeComponent();
        }

        private void SelectComponentsScreen_Load(object sender, EventArgs e)
        {

        }

        public override void InitializeUI()
        {
            _parentForm = (InstallWizardForm)this.ParentForm;

            clbComponentsForInstall.Items.Clear();
            clbComponentsForRepair.Items.Clear();
            clbComponentsForUninstall.Items.Clear();

            foreach (ComponentInstaller component in _parentForm.SetupManifest.Installers)
            {
                if (RegistryKeyAction.IsComponentInstalled(component.ComponentName))
                {
                    clbComponentsForRepair.Items.Add(component.ComponentName);
                    clbComponentsForUninstall.Items.Add(component.ComponentName);
                }
                else
                {
                    clbComponentsForInstall.Items.Add(component.ComponentName);
                }
            }
        }

        private void ComponentListBoxes_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComponentInstaller component = _parentForm.SetupManifest.Installers.Find(((CheckedListBox)sender).SelectedItem.ToString());
            lblComponentDescription.Text = component.Description;
        }

        private void ComponentListBoxes_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            CheckedListBox clb = ((CheckedListBox)sender);
            string componentName = clb.SelectedItem.ToString();
            ComponentInstaller component = _parentForm.SetupManifest.Installers.Find(componentName);

            if (clb.Name.EndsWith("Install"))
            {
                if (e.NewValue == CheckState.Checked)
                {
                    component.ActionToExecute = ActionTypeEnum.Install;
                }
                else
                {
                    component.ActionToExecute = ActionTypeEnum.Undefined;
                }
            }
            else if (clb.Name.EndsWith("Repair"))
            {
                if (e.NewValue == CheckState.Checked)
                {
                    if (component.ActionToExecute != ActionTypeEnum.Undefined)
                    {
                        UI.ShowMessage(_parentForm, "You cannot select the same component for both repair and uninstall. Uncheck one of the options.", icon: MessageBoxIcon.Exclamation);
                        e.NewValue = CheckState.Unchecked;
                        return;
                    }

                    component.ActionToExecute = ActionTypeEnum.Repair;
                }
                else
                {
                    component.ActionToExecute = ActionTypeEnum.Undefined;
                }
            }
            else if (clb.Name.EndsWith("Uninstall"))
            {
                if (e.NewValue == CheckState.Checked)
                {
                    if (component.ActionToExecute != ActionTypeEnum.Undefined)
                    {
                        UI.ShowMessage(_parentForm, "You cannot select the same component for both repair and uninstall. Uncheck one of the options.", icon: MessageBoxIcon.Exclamation);
                        e.NewValue = CheckState.Unchecked;
                        return;
                    }

                    component.ActionToExecute = ActionTypeEnum.Uninstall;
                }
                else
                {
                    component.ActionToExecute = ActionTypeEnum.Undefined;
                }
            }


        }
    }
}
