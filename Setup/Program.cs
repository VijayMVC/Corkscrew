using System;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Windows.Forms;

namespace CMS.Setup
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                if (args[0] == "/package")
                {
                    ExecPackageMode(args);
                }
            }
            else
            {
                ExecWizardMode();
            }
        }

        static void ExecPackageMode(string[] args)
        {
            // verify we are not running as Setup.exe
            if (System.IO.Path.GetFileName(Application.ExecutablePath).Equals("setup.exe", StringComparison.InvariantCultureIgnoreCase))
            {
                string errorMessage = "You cannot run the packager with the primary executable. Please copy \"Setup.exe\" as \"Setup1.exe\" and execute \"Setup1.exe\". You can create the copy with any name except \"Setup.exe\".";
                if (Environment.UserInteractive)
                {
                    UI.ShowMessage(null, errorMessage, MessageBoxButtons.OK, MessageBoxIcon.Stop, "STOP!");
                }
                else
                {
                    Console.WriteLine(errorMessage);
                }

                Application.Exit();
                return;
            }

            string sourceFolder = Application.StartupPath, workingDirectory = System.IO.Path.Combine(Application.StartupPath, "_layout"), outputSfxName = "CorkscrewSetup.exe";
            foreach (string p in args)
            {
                string fixedArgument = p;
                if (fixedArgument.StartsWith("--"))
                {
                    fixedArgument = "/" + fixedArgument.Substring(2);
                }
                else if (fixedArgument.StartsWith("-"))
                {
                    fixedArgument = "/" + fixedArgument.Substring(1);
                }

                if (fixedArgument.StartsWith("/sources:", StringComparison.InvariantCultureIgnoreCase))
                {
                    sourceFolder = fixedArgument.Substring("/sources:".Length);
                }

                if (fixedArgument.StartsWith("/temp:", StringComparison.InvariantCultureIgnoreCase))
                {
                    workingDirectory = fixedArgument.Substring("/temp:".Length);
                }

                if (fixedArgument.StartsWith("/sfx:", StringComparison.InvariantCultureIgnoreCase))
                {
                    outputSfxName = fixedArgument.Substring("/sfx:".Length);
                }
            }

            Console.WriteLine("Generating self extractor...");
            SelfExtractor sfx = new SelfExtractor(sourceFolder, workingDirectory, outputSfxName);
            sfx.Generate();
            Console.WriteLine("SFX created at: " + sfx.OutputSfxFileName);
            sfx = null;
        }

        static void ExecWizardMode()
        {
            if (!(new WindowsPrincipal(WindowsIdentity.GetCurrent())).IsInRole(WindowsBuiltInRole.Administrator))
            {
                string errorMessage = "You must run this program as Administrator. Right-click on the app or shortcut and select \"Run as Administrator\"";
                if (Environment.UserInteractive)
                {
                    UI.ShowMessage(null, errorMessage, MessageBoxButtons.OK, MessageBoxIcon.Stop, "STOP!");
                }
                else
                {
                    Console.WriteLine(errorMessage);
                }

                Application.Exit();
                return;
            }

            if (Environment.Is64BitOperatingSystem && (! Environment.Is64BitProcess))
            {
                string errorMessage = "You are running Setup as a 32-bit process in a 64-bit machine. This will force all installed components to run in 32-bit mode. Please restart Setup as a 64-bit program.";
                if (Environment.UserInteractive)
                {
                    if (UI.ShowMessage(null, errorMessage + "Do you wish to exit?", MessageBoxButtons.OK, MessageBoxIcon.Warning, "STOP!") == DialogResult.Yes)
                    {
                        Application.Exit();
                        return;
                    }
                }
                else
                {
                    Console.WriteLine(errorMessage);
                }
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Screens.InstallWizardForm());
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

        public static DialogResult ShowMessage(Form parentForm, string message, MessageBoxButtons buttons = MessageBoxButtons.OK, MessageBoxIcon icon = MessageBoxIcon.Information, string caption = "")
        {
            if (caption == null)
            {
                caption = parentForm.Text;
            }

            caption = string.Format("{0} : ", Application.ProductName, caption);

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
