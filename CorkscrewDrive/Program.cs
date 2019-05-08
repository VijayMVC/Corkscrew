using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Corkscrew.Drive
{
    static class Program
    {
        [DllImport("user32.dll")]
        static extern int ReleaseCapture();

        [DllImport("User32.dll")]
        static extern int SendMessage(IntPtr hWnd, int msg, int wParam, ref int lParam);

        const int WM_SYSCOMMAND = 0x112;
        const int MOUSE_MOVE = 0xF012;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new IconForm());
        }

        public static void ShowMessage(string message)
        {
            MessageBox.Show(
                message,
                "Corkscrew Drive",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
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

    public enum SyncStatusEnum
    {
        NotStarted = 0,
        Paused,
        Running
    }

}
