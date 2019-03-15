using Corkscrew.SDK.tools;
using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace CMS.Setup
{
    /// <summary>
    /// Class generates and extracts the shipping CAB files. 
    /// This class is used to generate the CAB files during the Setup packaging process in the Post-Build Event, 
    /// and is used to extract cab files prior to installation during the normal user execution of the Setup program
    /// </summary>
    public class SelfExtractor
    {

        #region Properties

        /// <summary>
        /// During SFX creation, this path should contain the files to be packaged. 
        /// During SFX extraction, this is the target path to extract an SFX to.
        /// </summary>
        public string LayoutPath
        {
            get;
            set;
        }

        /// <summary>
        /// Temp folder to marshall files during creation of SFX. Only used during SFX generation.
        /// </summary>
        public string WorkingDirectory
        {
            get;
            set;
        }

        /// <summary>
        /// The filename (fullpath including filename extension) of the SFX file that is generated. 
        /// We do not need this during SFX extraction because running the SFX would extract all its CABs
        /// </summary>
        public string OutputSfxFileName
        {
            get;
            set;
        } = "CorkscrewSetup.exe";

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor, use when creating SFX
        /// </summary>
        /// <param name="sourceFolder">Folder containing source files to be packaged</param>
        /// <param name="workingDirectory">Temporary working directory</param>
        /// <param name="sfxName">Desired name of generated SFX file</param>
        public SelfExtractor(string sourceFolder, string workingDirectory, string sfxName)
        {
            // When generating the SFX, we need to package the current program (Setup.exe) as well. But we cannot do this 
            // since we would run into a "file in use" exception from the Windows system. 
            // Therefore, ensure that if this constructor is called, the executable running is NOT "setup.exe"

            if (Path.GetFileName(Application.ExecutablePath).ToLower() == "setup.exe")
            {
                throw new InvalidProgramException("Cannot run packager when running as \"Setup.exe\". Please create a copy (eg: \"Setup1.exe\" and call that program to generate the SFX.");
            }

            LayoutPath = sourceFolder;
            WorkingDirectory = (string.IsNullOrEmpty(workingDirectory) ? Path.GetTempPath() : workingDirectory);
            OutputSfxFileName = (string.IsNullOrEmpty(sfxName) ? Path.Combine(Application.StartupPath, "CorkscrewSetup.exe") : sfxName);

            // perform validations/cleanup
            if (!Directory.Exists(LayoutPath))
            {
                throw new FileNotFoundException("SourceFolder does not exist: " + LayoutPath);
            }

            if (Directory.GetFiles(LayoutPath, "*.*", SearchOption.AllDirectories).Length == 0)
            {
                throw new FileNotFoundException("There are no files in SourceFolder: " + LayoutPath);
            }

            if (!Directory.Exists(WorkingDirectory))
            {
                Directory.CreateDirectory(WorkingDirectory);
            }

            if (File.Exists(OutputSfxFileName))
            {
                File.Delete(OutputSfxFileName);
            }

        }

        /// <summary>
        /// Constructor, use when extracting CABs from the SFX
        /// </summary>
        public SelfExtractor()
        {
            LayoutPath = Path.Combine(Application.StartupPath, "_layout");
            WorkingDirectory = Application.StartupPath;
            OutputSfxFileName = null;

            // perform validations and cleanup
            if (Directory.GetFiles(WorkingDirectory, "*.cab", SearchOption.AllDirectories).Length == 0)
            {
                throw new FileNotFoundException("There are no .CAB files in : " + LayoutPath);
            }


            if (!Directory.Exists(LayoutPath))
            {
                Directory.CreateDirectory(LayoutPath);
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Main entry point function to generate the SFX
        /// </summary>
        public void Generate()
        {
            if (File.Exists(OutputSfxFileName))
            {
                File.Delete(OutputSfxFileName);
            }

            GenerateCABFiles();
            GenerateSEDForIExpress();
            GenerateSFX();

            // cleanup vestige files
            File.Delete(Path.Combine(Application.StartupPath, "setup.inf"));
            File.Delete(Path.Combine(Application.StartupPath, "setup.rpt"));
        }


        /// <summary>
        /// Main entry point function to extract a component .CAB file
        /// </summary>
        /// <param name="cabFilename">Filename of the CAB file. Extension may or may not be included.</param>
        public void Extract(string cabFilename)
        {
            string cabFilePath = Path.Combine(WorkingDirectory, Utility.SafeString(cabFilename, expectEnd: ".cab"));
            string extractFolderName = Path.Combine(LayoutPath, Path.GetFileNameWithoutExtension(cabFilename));

            if (!File.Exists(cabFilePath))
            {
                throw new FileNotFoundException("CAB file not found: " + cabFilePath);
            }

            if (!Directory.Exists(extractFolderName))
            {
                Directory.CreateDirectory(extractFolderName);
            }

            RunCommandlineTool("extrac32.exe", string.Format("/Y /E /L \"{0}\" \"{1}\"", extractFolderName, cabFilePath));
        }

        #endregion

        #region Helper Methods - Generate SFX

        private void GenerateSFX()
        {
            RunCommandlineTool("iexpress", "/N " + Path.Combine(WorkingDirectory, Path.GetFileNameWithoutExtension(OutputSfxFileName) + ".sed"));
        }

        private void GenerateCABFiles()
        {
            string cabsPath = Path.Combine(WorkingDirectory, "cabs");
            string manifestXml = string.Empty;
            string manifestFile = Path.Combine(Application.StartupPath, "manifest.xml");

            if (!Directory.Exists(cabsPath))
            {
                Directory.CreateDirectory(cabsPath);
            }
            else
            {
                // delete old cabs, otherwise MAKECAB will end up appending/freshing instead of replacing the cab!
                CleanupCabFiles();
            }

            // now we simply bundle each top level directory inside "LayoutsPath" as its own CAB file
            foreach (string componentDirectory in Directory.GetDirectories(LayoutPath, "*.*", SearchOption.TopDirectoryOnly))
            {
                string componentName = Path.GetFileNameWithoutExtension(componentDirectory);
                string ddfFilePath = Path.Combine(WorkingDirectory, componentName + ".ddf");
                string cabFilePath = Path.Combine(cabsPath, componentName + ".cab");

                GenerateDDF(ddfFilePath, cabFilePath, componentDirectory);
                RunCommandlineTool("makecab.exe", " /f \"" + ddfFilePath + "\" /L \"" + cabsPath);

                string componentManifestPath = Path.Combine(componentDirectory, "manifest.xml");
                if (File.Exists(componentManifestPath))
                {
                    manifestXml = GenerateCombineManifestXml(manifestXml, componentManifestPath);
                }

                //TODO: Uncomment this line to enable generation of SFX files for each component
                //GenerateSFXForComponent(componentName, Path.ChangeExtension(ddfFilePath, ".cab"));
            }

            // create the grand manifest file
            using (StreamWriter sw = new StreamWriter(manifestFile))
            {
                sw.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
                sw.WriteLine("<Corkscrew>");
                sw.WriteLine("\t<Components>");
                sw.WriteLine(manifestXml);
                sw.WriteLine("\t</Components>");
                sw.WriteLine("</Corkscrew>");

                sw.Flush();
                sw.Close();
            }

            // cleanup DDF files
            foreach (string ddf in Directory.GetFiles(WorkingDirectory, "*.ddf", SearchOption.TopDirectoryOnly))
            {
                File.Delete(ddf);
            }
        }

        private void GenerateDDF(string ddfFilePath, string cabFilePath, string compressFolder)
        {
            string directoryForDDF = Path.GetDirectoryName(ddfFilePath);
            if (!Directory.Exists(directoryForDDF))
            {
                Directory.CreateDirectory(directoryForDDF);
            }

            using (StreamWriter sw = new StreamWriter(File.Create(ddfFilePath)))
            {
                // main options
                sw.WriteLine(".OPTION EXPLICIT");
                sw.WriteLine(".SET CabinetNameTemplate=\"" + Path.GetFileName(cabFilePath) + "\"");
                sw.WriteLine(".SET DiskDirectoryTemplate=" + Path.GetDirectoryName(cabFilePath));
                sw.WriteLine(".SET Cabinet=on");
                sw.WriteLine(".SET Compress=on");
                sw.WriteLine(".SET CompressionType=MSZIP");
                sw.WriteLine(".SET CabinetFileCountThreshold=0");
                sw.WriteLine(".SET FolderFileCountThreshold=0");
                sw.WriteLine(".SET MaxCabinetSize=0");
                sw.WriteLine(".SET MaxDiskFileCount=0");
                sw.WriteLine(".SET MaxDiskSize=0");

                // write the specific DDF statements for this folder

                GenerateDDFFolder(sw, compressFolder, compressFolder);

                sw.Flush();
                sw.Close();
            }
        }

        private void GenerateSFXForComponent(string componentName, string cabFilename)
        {
            string sedFilePath = Path.Combine(WorkingDirectory, componentName + ".sed");
            string cabsPath = Path.Combine(WorkingDirectory, "cabs");

            if (File.Exists(sedFilePath))
            {
                File.Delete(sedFilePath);
            }

            using (StreamWriter sw = new StreamWriter(File.OpenWrite(sedFilePath)))
            {
                sw.WriteLine("[Version]");
                sw.WriteLine("Class=IEXPRESS");
                sw.WriteLine("SEDVersion=3");
                sw.WriteLine("");
                sw.WriteLine("[Options]");
                sw.WriteLine("PackagePurpose=InstallApp");
                sw.WriteLine("ShowInstallProgramWindow=0");
                sw.WriteLine("MultiInstanceCheck=1");
                sw.WriteLine("CheckAdminRights=1");
                sw.WriteLine("HideExtractAnimation=0");
                sw.WriteLine("RebootMode=N");
                sw.WriteLine("CAB_FixedSize=0");
                sw.WriteLine("CAB_ResvCodeSigning=0");
                sw.WriteLine("InsideCompressed=0");
                sw.WriteLine("UseLongFileName=1");
                sw.WriteLine("CompressionType=MSZIP");
                sw.WriteLine("KeepCabinet=0");
                sw.WriteLine("SourceFiles=SourceFiles");
                sw.WriteLine("InstallPrompt=%InstallPrompt%");
                sw.WriteLine("DisplayLicense=%DisplayLicense%");
                sw.WriteLine("FinishMessage=%FinishMessage%");
                sw.WriteLine("TargetName=%TargetName%");
                sw.WriteLine("FriendlyName=%FriendlyName%");
                sw.WriteLine("AppLaunched=%AppLaunched%");
                sw.WriteLine("PostInstallCmd=%PostInstallCmd%");
                sw.WriteLine("AdminQuietInstCmd=%AdminQuietInstCmd%");
                sw.WriteLine("UserQuietInstCmd=%UserQuietInstCmd%");
                sw.WriteLine("");
                sw.WriteLine("[Strings]");
                sw.WriteLine("InstallPrompt=");
                sw.WriteLine("DisplayLicense=");
                sw.WriteLine("FinishMessage=");
                sw.WriteLine("TargetName=\"" + componentName + "-Setup.exe\"");
                sw.WriteLine("FriendlyName=\"" + componentName + " (Corkscrew Content Management System)\"");
                sw.WriteLine("AppLaunched=\"Setup.exe\"");
                sw.WriteLine("PostInstallCmd=<None>");
                sw.WriteLine("AdminQuietInstCmd=");
                sw.WriteLine("UserQuietInstCmd=");
                sw.WriteLine("");

                StringBuilder sbFiles = new StringBuilder(), sbLocalizedSourceFilesSection = new StringBuilder();
                long fileCount = 0;

                sbLocalizedSourceFilesSection.AppendLine("[SourceFiles0]");

                // add installer's own files
                sbFiles.AppendLine(string.Format("FILE{0}=\"Setup.exe\"", fileCount));
                sbLocalizedSourceFilesSection.AppendLine(string.Format("%FILE{0}%=", fileCount));
                fileCount++;
                sbFiles.AppendLine(string.Format("FILE{0}=\"Setup.exe.config\"", fileCount));
                sbLocalizedSourceFilesSection.AppendLine(string.Format("%FILE{0}%=", fileCount));
                fileCount++;
                sbFiles.AppendLine(string.Format("FILE{0}=\"Corkscrew.SDK.dll\"", fileCount));
                sbLocalizedSourceFilesSection.AppendLine(string.Format("%FILE{0}%=", fileCount));
                fileCount++;
                sbFiles.AppendLine(string.Format("FILE{0}=\"Microsoft.Web.Administration.dll\"", fileCount));
                sbLocalizedSourceFilesSection.AppendLine(string.Format("%FILE{0}%=", fileCount));
                fileCount++;

                sbLocalizedSourceFilesSection.AppendLine("");
                sbLocalizedSourceFilesSection.AppendLine("[SourceFiles1]");

                sbFiles.AppendLine(string.Format("FILE{0}=\"Manifest.xml\"", fileCount));
                sbLocalizedSourceFilesSection.AppendLine(string.Format("%FILE{0}%=", fileCount));
                fileCount++;

                sbFiles.AppendLine(string.Format("FILE{0}=\"{1}\"", fileCount, Path.GetFileName(cabFilename)));
                sbLocalizedSourceFilesSection.AppendLine(string.Format("%FILE{0}%=", fileCount));
                fileCount++;

                sw.WriteLine(sbFiles.ToString());
                sw.WriteLine("");
                sw.WriteLine(sbLocalizedSourceFilesSection.ToString());
                sw.WriteLine("");


                sw.WriteLine("[SourceFiles]");
                sw.WriteLine(string.Format("SourceFiles0=\"{0}\"", Utility.SafeString(Application.StartupPath, expectEnd: "\\")));
                sw.WriteLine(string.Format("SourceFiles1=\"{0}\"", Utility.SafeString(cabsPath, expectEnd: "\\")));

                sw.WriteLine("");

                sw.Flush();
                sw.Close();
            }

            // create a copy of the Manifest.xml 
            string componentSetupManifestPath = Path.Combine(cabsPath, "Manifest.xml");
            File.Copy(Path.Combine(LayoutPath, componentName, "Manifest.xml"), componentSetupManifestPath, true);
            File.WriteAllText(componentSetupManifestPath, File.ReadAllText(componentSetupManifestPath).Replace("<?xml version=\"1.0\" encoding=\"utf-8\" ?>", "<?xml version=\"1.0\" encoding=\"utf-8\" ?><Corkscrew><Components>") + "</Components></Corkscrew>");

            RunCommandlineTool("iexpress", "/N " + sedFilePath);
        }

        private void GenerateDDFFolder(StreamWriter sw, string compressFolder, string rootFolder)
        {
            string[] files = Directory.GetFiles(compressFolder, "*.*", SearchOption.TopDirectoryOnly);

            if ((files != null) && (files.Length > 0))
            {
                string destinationDirectory = Utility.SafeString(compressFolder.Replace(rootFolder, ""), removeAtStart: "\\");

                foreach (string file in files)
                {
                    // Do not package temp and .vshost files
                    if ((!file.Contains(".vshost")) && (!file.Contains("tmp")) && (!file.Contains("temp")) && (!file.Contains("~")) && (!Path.GetFileName(file).Equals("manifest.xml", StringComparison.InvariantCultureIgnoreCase)))
                    {
                        sw.WriteLine(string.Format("\"{0}\" \"{1}\\{2}\"", file, destinationDirectory, Path.GetFileName(file)));

                        if (Path.GetExtension(file) == ".config")
                        {
                            // remove connection strings and other unwanted stuff from the .config file
                            try
                            {
                                ExeConfigurationFileMap map = new ExeConfigurationFileMap()
                                {
                                    ExeConfigFilename = file
                                };

                                Configuration cfg = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
                                cfg.ConnectionStrings.ConnectionStrings.Clear();
                                cfg.ConnectionStrings.ConnectionStrings.EmitClear = true;

                                try
                                {
                                    if (cfg.Sections.Get("system.codedom") != null)
                                    {
                                        cfg.Sections.Remove("system.codedom");
                                    }
                                }
                                catch { }

                                cfg.Save(ConfigurationSaveMode.Modified);
                            }
                            catch { }
                        }
                    }
                }
            }

            foreach (string directory in Directory.GetDirectories(compressFolder, "*.*", SearchOption.TopDirectoryOnly))
            {
                GenerateDDFFolder(sw, directory, rootFolder);
            }
        }

        private void GenerateSEDForIExpress()
        {
            string sedFilePath = Path.Combine(WorkingDirectory, Path.GetFileNameWithoutExtension(OutputSfxFileName) + ".sed");
            string cabsPath = Path.Combine(WorkingDirectory, "cabs");

            if (File.Exists(sedFilePath))
            {
                File.Delete(sedFilePath);
            }

            using (StreamWriter sw = new StreamWriter(File.OpenWrite(sedFilePath)))
            {
                sw.WriteLine("[Version]");
                sw.WriteLine("Class=IEXPRESS");
                sw.WriteLine("SEDVersion=3");
                sw.WriteLine("");
                sw.WriteLine("[Options]");
                sw.WriteLine("PackagePurpose=InstallApp");
                sw.WriteLine("ShowInstallProgramWindow=0");
                sw.WriteLine("MultiInstanceCheck=1");
                sw.WriteLine("CheckAdminRights=1");
                sw.WriteLine("HideExtractAnimation=0");
                sw.WriteLine("RebootMode=N");
                sw.WriteLine("CAB_FixedSize=0");
                sw.WriteLine("CAB_ResvCodeSigning=0");
                sw.WriteLine("InsideCompressed=0");
                sw.WriteLine("UseLongFileName=1");
                sw.WriteLine("CompressionType=MSZIP");
                sw.WriteLine("KeepCabinet=0");
                sw.WriteLine("SourceFiles=SourceFiles");
                sw.WriteLine("InstallPrompt=%InstallPrompt%");
                sw.WriteLine("DisplayLicense=%DisplayLicense%");
                sw.WriteLine("FinishMessage=%FinishMessage%");
                sw.WriteLine("TargetName=%TargetName%");
                sw.WriteLine("FriendlyName=%FriendlyName%");
                sw.WriteLine("AppLaunched=%AppLaunched%");
                sw.WriteLine("PostInstallCmd=%PostInstallCmd%");
                sw.WriteLine("AdminQuietInstCmd=%AdminQuietInstCmd%");
                sw.WriteLine("UserQuietInstCmd=%UserQuietInstCmd%");
                sw.WriteLine("");
                sw.WriteLine("[Strings]");
                sw.WriteLine("InstallPrompt=");
                sw.WriteLine("DisplayLicense=");
                sw.WriteLine("FinishMessage=");
                sw.WriteLine("TargetName=\"CorkscrewSetup.exe\"");
                sw.WriteLine("FriendlyName=\"Corkscrew Content Management System\"");
                sw.WriteLine("AppLaunched=\"Setup.exe\"");
                sw.WriteLine("PostInstallCmd=<None>");
                sw.WriteLine("AdminQuietInstCmd=");
                sw.WriteLine("UserQuietInstCmd=");
                sw.WriteLine("");

                StringBuilder sbFiles = new StringBuilder(), sbLocalizedSourceFilesSection = new StringBuilder();
                long fileCount = 0;

                sbLocalizedSourceFilesSection.AppendLine("[SourceFiles0]");

                // add installer's own files
                sbFiles.AppendLine(string.Format("FILE{0}=\"Setup.exe\"", fileCount));
                sbLocalizedSourceFilesSection.AppendLine(string.Format("%FILE{0}%=", fileCount));
                fileCount++;
                sbFiles.AppendLine(string.Format("FILE{0}=\"Setup.exe.config\"", fileCount));
                sbLocalizedSourceFilesSection.AppendLine(string.Format("%FILE{0}%=", fileCount));
                fileCount++;
                sbFiles.AppendLine(string.Format("FILE{0}=\"Corkscrew.SDK.dll\"", fileCount));
                sbLocalizedSourceFilesSection.AppendLine(string.Format("%FILE{0}%=", fileCount));
                fileCount++;
                sbFiles.AppendLine(string.Format("FILE{0}=\"Microsoft.Web.Administration.dll\"", fileCount));
                sbLocalizedSourceFilesSection.AppendLine(string.Format("%FILE{0}%=", fileCount));
                fileCount++;
                sbFiles.AppendLine(string.Format("FILE{0}=\"Manifest.xml\"", fileCount));
                sbLocalizedSourceFilesSection.AppendLine(string.Format("%FILE{0}%=", fileCount));
                fileCount++;

                sbLocalizedSourceFilesSection.AppendLine("");
                sbLocalizedSourceFilesSection.AppendLine("[SourceFiles1]");
                foreach (string cabFile in Directory.GetFiles(cabsPath, "*.cab", SearchOption.TopDirectoryOnly))
                {
                    sbFiles.AppendLine(string.Format("FILE{0}=\"{1}\"", fileCount, Path.GetFileName(cabFile)));
                    sbLocalizedSourceFilesSection.AppendLine(string.Format("%FILE{0}%=", fileCount));
                    fileCount++;
                }

                sw.WriteLine(sbFiles.ToString());
                sw.WriteLine("");
                sw.WriteLine(sbLocalizedSourceFilesSection.ToString());
                sw.WriteLine("");


                sw.WriteLine("[SourceFiles]");
                sw.WriteLine(string.Format("SourceFiles0=\"{0}\"", Utility.SafeString(Application.StartupPath, expectEnd: "\\")));
                sw.WriteLine(string.Format("SourceFiles1=\"{0}\"", Utility.SafeString(cabsPath, expectEnd: "\\")));

                sw.WriteLine("");

                sw.Flush();
                sw.Close();
            }
        }

        private string GenerateCombineManifestXml(string currentManifest, string componentFile)
        {
            return string.Format(
                        "{0}{1}{2}",
                        currentManifest,
                        Environment.NewLine,
                        File.ReadAllText(componentFile).Replace("<?xml version=\"1.0\" encoding=\"utf-8\" ?>", "")
                   );
        }

        private void CleanupCabFiles()
        {
            foreach (string oldCabFile in Directory.GetFiles(Path.Combine(WorkingDirectory, "cabs"), "*.cab", SearchOption.AllDirectories))
            {
                File.Delete(oldCabFile);
            }
        }

        #endregion

        #region Helper Methods - Common

        private void RunCommandlineTool(string toolExeName, string arguments, bool wait = true)
        {
            ProcessStartInfo toolStartInfo = new ProcessStartInfo(toolExeName, arguments)
            {
                CreateNoWindow = true,
                ErrorDialog = false,
                LoadUserProfile = false
            };

            Process tool = Process.Start(toolStartInfo);
            if (wait)
            {
                tool.WaitForExit();
            }
        }

        #endregion

    }
}
