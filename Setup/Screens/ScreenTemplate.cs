using System;
using System.Windows.Forms;

namespace CMS.Setup.Screens
{
    public partial class ScreenTemplate : UserControl
    {
        public ScreenTemplate()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Called from InstallWizardForm when showing the control. Will be called for each show.
        /// </summary>
        public virtual void InitializeUI()
        {
            throw new NotImplementedException();
        }

    }
}
