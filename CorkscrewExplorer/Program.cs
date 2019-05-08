using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Corkscrew.Explorer
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMainWindow());
        }
    }

    /// <summary>
    /// Type of operation
    /// </summary>
    public enum OperationTypeEnum
    {
        /// <summary>
        /// Create a new item
        /// </summary>
        Create,

        /// <summary>
        /// Edit an existing item
        /// </summary>
        Edit
    }

    static class UI
    {

        [DllImport("user32.dll")]
        static extern int ReleaseCapture();

        [DllImport("User32.dll")]
        static extern int SendMessage(IntPtr hWnd, int msg, int wParam, ref int lParam);

        const int WM_SYSCOMMAND = 0x112;
        const int MOUSE_MOVE = 0xF012;

        public static DialogResult ShowMessage(Form parentForm, string message, MessageBoxButtons buttons = MessageBoxButtons.OK, string caption = "")
        {
            if (caption == null)
            {
                caption = parentForm.Text;
            }

            caption = string.Format("{0} : ", Application.ProductName, caption);

            MessageBoxIcon icon = MessageBoxIcon.Information;
            if (buttons.HasFlag(MessageBoxButtons.OK))
            {
                icon = MessageBoxIcon.Information;
            }
            else if (buttons.HasFlag(MessageBoxButtons.YesNo))
            {
                icon = MessageBoxIcon.Question;
            }
            else if (buttons.HasFlag(MessageBoxButtons.RetryCancel) || buttons.HasFlag(MessageBoxButtons.AbortRetryIgnore))
            {
                icon = MessageBoxIcon.Error;
            }

            return MessageBox.Show(parentForm, message, caption, buttons, icon);
        }

        public static void EnableBorderlessFormMove(IntPtr windowHandle, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                int lParm = 0;
                ReleaseCapture();
                SendMessage(windowHandle, WM_SYSCOMMAND, MOUSE_MOVE, ref lParm);
            }
        }
    }
}
