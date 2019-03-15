using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Corkscrew.Tools.ProvisionWebsite
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
            Application.Run(new MainWindow());
        }
    }

    static class UI
    {
        [DllImport("user32.dll")]
        static extern int ReleaseCapture();

        [DllImport("User32.dll")]
        static extern int SendMessage(IntPtr hWnd, int msg, int wParam, ref int lParam);

        const int WM_SYSCOMMAND = 0x112;
        const int MOUSE_MOVE = 0xF012;

        public static void EnableBorderlessMouseMove(Form form, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                int lParm = 0;
                ReleaseCapture();
                SendMessage(form.Handle, WM_SYSCOMMAND, MOUSE_MOVE, ref lParm);
            }
        }
    }
}
