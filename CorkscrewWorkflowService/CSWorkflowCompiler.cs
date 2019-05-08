using Corkscrew.SDK.exceptions;
using Corkscrew.SDK.tools;
using Corkscrew.SDK.workflow;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Workflow.ComponentModel.Compiler;

namespace CorkscrewWorkflowService
{

    /// <summary>
    /// Compiler for workflows.
    /// </summary>
    /// <remarks>Though this class is named "Workflow" compiler and lives in the Workflow space, it is possible to use it to compile almost any .NET code 
    /// by setting the WorkflowEngine to CS1 (no dependencies on WF3 or WF4 libraries).</remarks>
    public class CSWorkflowCompiler
    {

        // This warning must be disabled to enable usage of WF3 objects below
#pragma warning disable 0618

        private string _buildFolder = null;         // temporary folder used during build
        private string _runtimeFolder = null;       // temporary folder containing runtime objects

        #region Properties

        /// <summary>
        /// Returns errors encountered during last compilation run. When the Corkscrew runtime is offloaded, this information is lost and not persisted anywhere.
        /// </summary>
        public IReadOnlyList<CompilerError> CompileErrors
        {
            get;
            protected set;
        } = null;

        /// <summary>
        /// Returns if the last compilation attempt was successful. 
        /// NOTE: If this flag is FALSE, then CompilerOutputAssembly will be NULL
        /// </summary>
        public bool CompileSuccessful
        {
            get;
            protected set;
        } = false;

        /// <summary>
        /// The output assembly post compilation
        /// </summary>
        public Assembly CompilerOutputAssembly
        {
            get;
            protected set;
        } = null;

        /// <summary>
        /// The workflow manifest to compile
        /// </summary>
        public CSWorkflowManifest Manifest
        {
            get;
            private set;
        } = null;

        /// <summary>
        /// Returns the engines this compiler will accept compilation requests for
        /// </summary>
        public WorkflowEngineEnum AcceptRequestsForEngine
        {
            get
            {
                return WorkflowEngineEnum.WF3C | WorkflowEngineEnum.WF3X | WorkflowEngineEnum.WF4C | WorkflowEngineEnum.WF4X | WorkflowEngineEnum.CS1C;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="manifest">The manifest to be compiled</param>
        /// <exception cref="CSWorkflowException">If the WorkflowEngine set in the manifest is not supported by the compiler</exception>
        public CSWorkflowCompiler(CSWorkflowManifest manifest)
        {
            if (!AcceptRequestsForEngine.HasFlag(manifest.WorkflowEngine))
            {
                throw new CSWorkflowException("This compiler does not compile workflows of the engine type specified in the manifest.");
            }

            Manifest = manifest;
            CompileSuccessful = false;
            CompileErrors = null;
            CompilerOutputAssembly = null;

            // we use the manifest.Id to seperate the contents, ensuring that each manifest gets consistently the same folder.
            _buildFolder = Path.Combine(Path.GetTempPath(), manifest.Id.ToString("d"), "build");
            _runtimeFolder = Path.Combine(Path.GetTempPath(), manifest.Id.ToString("d"), "runtime");
        }

        #endregion

        #region Methods

        // sets properties to indicate failed state
        private void SetFailedCompile(string stepName, string errorName, string error)
        {
            error = "Build folder: [" + _buildFolder + "], " +
                    "Runtime folder: [" + _runtimeFolder + "], " + 
                    "Temp files collection folder: [" + Path.Combine(_buildFolder, "~temp") + "], " + 
                    "Workflow Definition Id: {" + Manifest.WorkflowDefinition.Id.ToString("D") + "} : Compilation Error: " + 
                    error;


            CompileSuccessful = false;
            CompileErrors = (new List<CompilerError>() { new CompilerError(stepName, 0, 0, errorName, error) }).AsReadOnly();
            CompilerOutputAssembly = null;
        }

        /// <summary>
        /// Performs the actual compilation
        /// </summary>
        /// <param name="retainLastOutput">If set, before compilation starts the existing build folder is not cleared.</param>
        /// <remarks>This method does not throw any exceptions or return a value. Caller must check the value of CompileSuccessful property and then the 
        /// CompileErrors or CompilerOutputAssembly properties to know the result.</remarks>
        /// <seealso cref="CompileErrors"/>
        /// <seealso cref="CompilerOutputAssembly"/>
        public void Compile(bool retainLastOutput = false)
        {

            // all of these go in as parameters to our Compiler
            List<string> embeddedResourceFiles = new List<string>();
            List<string> libraryPaths = new List<string>();
            List<string> linkedResourceFiles = new List<string>();
            List<string> referencedAssemblies = new List<string>();
            List<string> sourceFiles = new List<string>();

            string tempFileCollectionPath = Path.Combine(_buildFolder, "~temp");

            CompileSuccessful = false;
            CompileErrors = null;

            if (!retainLastOutput)
            {
                CompilerOutputAssembly = null;

                try
                {
                    ClearFolder(_buildFolder);
                    ClearFolder(_runtimeFolder);
                    ClearFolder(tempFileCollectionPath);
                }
                catch
                {
                    SetFailedCompile("Pre-compile Cleanup", "CSWF001", "Unable to create build, runtime and temp folders. Files may be open in Explorer or other apps. Please close before trying again.");
                    return;
                }
            }

            try
            {
                CreateFolder(_buildFolder);
                CreateFolder(_runtimeFolder);
                CreateFolder(tempFileCollectionPath);
            }
            catch
            {
                SetFailedCompile("Pre-compile Cleanup", "CSWF001", "Unable to create build, runtime and temp folders. Check permissions in folder " + Path.GetDirectoryName(_buildFolder) + ".");
                return;
            }

            // deserialize the files from the manifest

            IReadOnlyList<CSWorkflowManifestItem> manifestItems = Manifest.GetItems();
            if (manifestItems.Count == 0)
            {
                SetFailedCompile("Pre-compile Deserialization", "CSWF002", "Workflow Manifest does not contain any items.");
                return;
            }

            bool hasAssemblyInfo = false;                               // manifest contains an AssemblyInfo.* file
            string languageToUse = string.Empty;                        // "cs" or "vb"
            string assemblyInfoFilename = null;                         // name of the AssemblyInfo.* file created by the compiler (if one is not in the manifest)
            CSWorkflowManifestItem primaryAssemblyItem = null;          // the manifestitem corresponding to the PrimaryAssembly (need this to update results)
            DateTime latestCompilerInputFile = DateTime.MinValue;       // date/time of newest compilation input file

            foreach (CSWorkflowManifestItem item in manifestItems)
            {
                if (item.IsCompilerInput)
                {
                    try
                    {
                        if (item.ItemType == WorkflowManifestItemTypeEnum.SourceCodeFile)
                        {
                            if (string.IsNullOrEmpty(languageToUse))
                            {
                                languageToUse = (item.FilenameExtension.EndsWith(".cs") ? "cs" : (item.FilenameExtension.EndsWith(".vb") ? "vb" : null));
                            }

                            // we have a .cs or a .vb file, but this is of a different extension than the first encountered language extension.
                            if (((item.FilenameExtension.EndsWith(".cs")) || (item.FilenameExtension.EndsWith(".vb"))) && (!item.FilenameExtension.EndsWith("." + languageToUse)))
                            {
                                SetFailedCompile("Pre-compile Deserialization", "CSWF003", "Workflow Manifest item [" + item.FilenameWithExtension + "] is of a different language than a previously deserialized source file.");
                                return;
                            }

                            if ((item.FilenameWithExtension.Equals("assemblyinfo.cs", StringComparison.InvariantCultureIgnoreCase)) || (item.FilenameWithExtension.Equals("assemblyinfo.vb", StringComparison.InvariantCultureIgnoreCase)))
                            {
                                hasAssemblyInfo = true;
                            }
                        }

                        string itemDiskFilePath = item.WriteToDiskForCompiler(_buildFolder);
                        if (itemDiskFilePath == null)
                        {
                            SetFailedCompile("Pre-compile Deserialization", "CSWF004", "Workflow Manifest item [" + item.FilenameWithExtension + "] has no data.");
                            return;
                        }

                        if (latestCompilerInputFile < item.Modified)
                        {
                            latestCompilerInputFile = item.Modified;
                        }

                        switch (item.ItemType)
                        {
                            case WorkflowManifestItemTypeEnum.DependencyAssembly:
                                referencedAssemblies.Add(itemDiskFilePath);
                                break;

                            case WorkflowManifestItemTypeEnum.ResourceFile:
                                if (item.FilenameExtension.EndsWith(".resources"))
                                {
                                    //Workaround: Do we really need to add them to BOTH these lists?? Which one is correct?
                                    embeddedResourceFiles.Add(itemDiskFilePath);
                                    linkedResourceFiles.Add(itemDiskFilePath);
                                }
                                else if ((item.FilenameExtension.EndsWith(".resx")) || (item.FilenameExtension.EndsWith(".res")))
                                {
                                    sourceFiles.Add(itemDiskFilePath);
                                }
                                break;

                            case WorkflowManifestItemTypeEnum.SourceCodeFile:
                            case WorkflowManifestItemTypeEnum.XamlFile:
                                sourceFiles.Add(itemDiskFilePath);
                                break;
                        }
                    }
                    catch
                    {
                        SetFailedCompile("Pre-compile Deserialization", "CSWF005", "Workflow Manifest item [" + item.FilenameWithExtension + "] could not be written to disk.");
                        return;
                    }
                }

                if (item.IsRuntimeComponent)
                {
                    try
                    {
                        if (item.WriteToDiskForRuntime(_runtimeFolder) == null)
                        {
                            // primary assembly is allowed to have no data
                            if (item.ItemType != WorkflowManifestItemTypeEnum.PrimaryAssembly)
                            {
                                SetFailedCompile("Pre-compile Deserialization", "CSWF006", "Workflow Manifest item [" + item.FilenameWithExtension + "] has no data.");
                                return;
                            }
                        }

                        if (item.ItemType == WorkflowManifestItemTypeEnum.PrimaryAssembly)
                        {
                            primaryAssemblyItem = item;
                        }
                    }
                    catch
                    {
                        SetFailedCompile("Pre-compile Deserialization", "CSWF007", "Workflow Manifest item [" + item.FilenameWithExtension + "] could not be written to disk.");
                        return;
                    }
                }
            }

            if (latestCompilerInputFile >= Manifest.LastCompiled)
            {
                primaryAssemblyItem = null;
            }

            // if we have a valid primary assembly, load it and return
            if (primaryAssemblyItem != null)
            {
                string primaryAssemblyPath = primaryAssemblyItem.GetFullPathForRuntime(_runtimeFolder);

                try
                {
                    // because of all the AppDomain stuff, use the Assembly.Load() to do this
                    CompilerOutputAssembly = Assembly.Load(File.ReadAllBytes(primaryAssemblyPath));
                    CompileSuccessful = true;
                    CompileErrors = null;

                    try
                    {
                        ClearFolder(_buildFolder);
                    }
                    catch
                    {
                        // we dont care for errors here at this point of time.
                    }

                    return;
                }
                catch
                {
                    // couldnt load the assembly, no problem... we can recompile it.
                    if (File.Exists(primaryAssemblyPath))
                    {
                        try
                        {
                            File.Delete(primaryAssemblyPath);
                        }
                        catch
                        {
                            // oops, we cannot compile it and we cannot load the existing one either. Something is seriously wrong!
                            SetFailedCompile("Pre-compiled Assembly Load Attempt", "CSWF00A", "Unable to load existing assembly or compile a new one.");
                            return;
                        }
                    }
                }
            }

            // if we got here, we have to compile

            if (!hasAssemblyInfo)
            {
                // generate an assembly info
                assemblyInfoFilename = Path.Combine(_buildFolder, "AssemblyInfo." + languageToUse);

                try
                {
                    using (StreamWriter sw = new StreamWriter(assemblyInfoFilename))
                    {
                        switch (languageToUse)
                        {
                            case "cs":
                                sw.WriteLine("using System;");
                                sw.WriteLine("using System.Reflection;");
                                sw.WriteLine("using System.Runtime.InteropServices;");
                                sw.WriteLine();
                                sw.WriteLine("[assembly: AssemblyTitle(\"" + Utility.SafeString(Manifest.BuildAssemblyTitle) + "\")]");
                                sw.WriteLine("[assembly: AssemblyDescription(\"" + Utility.SafeString(Manifest.BuildAssemblyDescription) + "\")]");
                                sw.WriteLine("[assembly: AssemblyCompany(\"" + Utility.SafeString(Manifest.BuildAssemblyCompany) + "\")]");
                                sw.WriteLine("[assembly: AssemblyProduct(\"" + Utility.SafeString(Manifest.BuildAssemblyProduct) + "\")]");
                                sw.WriteLine("[assembly: AssemblyCopyright(\"" + Utility.SafeString(Manifest.BuildAssemblyCopyright) + "\")]");
                                sw.WriteLine("[assembly: AssemblyTrademark(\"" + Utility.SafeString(Manifest.BuildAssemblyTrademark) + "\")]");
                                sw.WriteLine();
                                sw.WriteLine("[assembly: ComVisible(false)]");
                                sw.WriteLine("[assembly: CLSCompliant(true)]");
                                sw.WriteLine();
                                sw.WriteLine("[assembly: AssemblyVersion(\"" + ((Manifest.BuildAssemblyVersion == null) ? "1.0.0.0" : Manifest.BuildAssemblyVersion.ToString(4)) + "\")]");
                                sw.WriteLine("[assembly: AssemblyFileVersion(\"" + ((Manifest.BuildAssemblyFileVersion == null) ? "1.0.0.0" : Manifest.BuildAssemblyFileVersion.ToString(4)) + "\")]");
                                sw.WriteLine();
                                break;

                            case "vb":
                                sw.WriteLine("Imports System");
                                sw.WriteLine("Imports System.Reflection");
                                sw.WriteLine("Imports System.Runtime.InteropServices");
                                sw.WriteLine();
                                sw.WriteLine("<Assembly: AssemblyTitle(\"" + Utility.SafeString(Manifest.BuildAssemblyTitle) + "\")>");
                                sw.WriteLine("<Assembly: AssemblyDescription(\"" + Utility.SafeString(Manifest.BuildAssemblyDescription) + "\")>");
                                sw.WriteLine("<Assembly: AssemblyCompany(\"" + Utility.SafeString(Manifest.BuildAssemblyCompany) + "\")>");
                                sw.WriteLine("<Assembly: AssemblyProduct(\"" + Utility.SafeString(Manifest.BuildAssemblyProduct) + "\")>");
                                sw.WriteLine("<Assembly: AssemblyCopyright(\"" + Utility.SafeString(Manifest.BuildAssemblyCopyright) + "\")>");
                                sw.WriteLine("<Assembly: AssemblyTrademark(\"" + Utility.SafeString(Manifest.BuildAssemblyTrademark) + "\")>");
                                sw.WriteLine();
                                sw.WriteLine("<Assembly: ComVisible(False)>");
                                sw.WriteLine("<Assembly: CLSCompliant(True)>");
                                sw.WriteLine();
                                sw.WriteLine("<Assembly: AssemblyVersion(\"" + ((Manifest.BuildAssemblyVersion == null) ? "1.0.0.0" : Manifest.BuildAssemblyVersion.ToString(4)) + "\")>");
                                sw.WriteLine("<Assembly: AssemblyFileVersion(\"" + ((Manifest.BuildAssemblyFileVersion == null) ? "1.0.0.0" : Manifest.BuildAssemblyFileVersion.ToString(4)) + "\")>");
                                sw.WriteLine();
                                break;
                        }
                    }

                    sourceFiles.Add(assemblyInfoFilename);
                }
                catch
                {
                    // there is no need to fail on this, we just continue without the info...
                    //SetFailedCompile("Pre-compile Deserialization", "CSWF008", "AssemblyInfo file could not be created to disk.");
                    //return;
                }
            }

            switch (Manifest.WorkflowEngine)
            {
                case WorkflowEngineEnum.WF3C:
                case WorkflowEngineEnum.WF3X:
                    referencedAssemblies.AddRange(
                        new string[]
                        {
                            "System.Workflow.Activities.dll",
                            "System.Workflow.ComponentModel.dll",
                            "System.Workflow.Runtime.dll",
                            "System.WorkflowServices.dll"
                        }
                    );
                    break;

                case WorkflowEngineEnum.WF4C:
                case WorkflowEngineEnum.WF4X:
                    referencedAssemblies.AddRange(
                        new string[]
                        {
                            "System.Activities.dll",
                            "System.Activities.DurableInstancing.dll"
                        }
                    );
                    break;
            }

            if ((Manifest.WorkflowEngine == WorkflowEngineEnum.WF3X) || (Manifest.WorkflowEngine == WorkflowEngineEnum.WF4X))
            {
                referencedAssemblies.Add("System.Xaml.dll");
            }

            // corkscrew library and its dependencies
            referencedAssemblies.AddRange(
                new string[]
                {
                    Path.Combine(_buildFolder, "Corkscrew.SDK.dll"),
                    Path.Combine(_buildFolder, "DocumentFormat.OpenXml.dll"),
                    Path.Combine(_buildFolder, "WindowsBase.dll"),
                    Path.Combine(_buildFolder, "ICSharpCode.SharpZipLib.dll"),
                    Path.Combine(_buildFolder, "MySql.Data.dll")
                }
            );

            // copy Corkscrew DLL dependencies to build folder with overwrite
            string baseDirectory = Path.GetDirectoryName(typeof(CSWorkflowCompiler).Assembly.Location);

            try
            {
                if (File.Exists(Path.Combine(baseDirectory, "Corkscrew.SDK.dll"))) File.Copy(Path.Combine(baseDirectory, "Corkscrew.SDK.dll"), Path.Combine(_buildFolder, "Corkscrew.SDK.dll"), true);
                if (File.Exists(Path.Combine(baseDirectory, "DocumentFormat.OpenXml.dll"))) File.Copy(Path.Combine(baseDirectory, "DocumentFormat.OpenXml.dll"), Path.Combine(_buildFolder, "DocumentFormat.OpenXml.dll"), true);
                if (File.Exists(Path.Combine(baseDirectory, "WindowsBase.dll"))) File.Copy(Path.Combine(baseDirectory, "WindowsBase.dll"), Path.Combine(_buildFolder, "WindowsBase.dll"), true);
                if (File.Exists(Path.Combine(baseDirectory, "ICSharpCode.SharpZipLib.dll"))) File.Copy(Path.Combine(baseDirectory, "ICSharpCode.SharpZipLib.dll"), Path.Combine(_buildFolder, "ICSharpCode.SharpZipLib.dll"), true);
                if (File.Exists(Path.Combine(baseDirectory, "MySql.Data.dll"))) File.Copy(Path.Combine(baseDirectory, "MySql.Data.dll"), Path.Combine(_buildFolder, "MySql.Data.dll"), true);
            }
            catch (Exception fileCopyException)
            {
                SetFailedCompile("Pre-compile Reference Assemblies", "CSWF009", "One of more Corkscrew dependency DLLs could not be copied to Build folder. " + fileCopyException.Message);
                return;
            }

            WorkflowCompiler compiler = new WorkflowCompiler();
            WorkflowCompilerParameters compilerParameters = new WorkflowCompilerParameters();

            compilerParameters.GenerateExecutable = false;                          // we want a DLL not an EXE
            compilerParameters.GenerateInMemory = false;                            // write to disk
            compilerParameters.TreatWarningsAsErrors = false;                       // warnings as warnings please
            compilerParameters.MainClass = Manifest.WorkflowClassName;              // starting point class

            // this has to be full path to runtimeFolder, otherwise DLL will be created in *our* bin folder! :-(
            compilerParameters.OutputAssembly = Path.Combine(_runtimeFolder, Manifest.OutputAssemblyName);

            compilerParameters.CompilerOptions = "/optimize";
            if ((Manifest.WorkflowEngine == WorkflowEngineEnum.WF3C) || (Manifest.WorkflowEngine == WorkflowEngineEnum.WF3X))
            {
                compilerParameters.CompilerOptions += " /nowarn:618";               // dont warn for CS0618 (obsolete WF3 stuff)
            }

            compilerParameters.WarningLevel = 3;
            compilerParameters.TempFiles = new System.CodeDom.Compiler.TempFileCollection(tempFileCollectionPath, true);

            compilerParameters.EmbeddedResources.AddRange(embeddedResourceFiles.ToArray());
            compilerParameters.LibraryPaths.AddRange(libraryPaths.ToArray());
            compilerParameters.LinkedResources.AddRange(linkedResourceFiles.ToArray());
            compilerParameters.ReferencedAssemblies.AddRange(referencedAssemblies.ToArray());

            WorkflowCompilerResults results = compiler.Compile(compilerParameters, sourceFiles.ToArray());
            if (results.Errors.HasErrors)
            {
                bool hasRealErrors = false;
                List<CompilerError> errors = new List<CompilerError>();

                errors.Add(
                    new CompilerError
                    (
                        Manifest.Id.ToString() + ".manifest", 
                        1, 
                        1, 
                        "COMPILE_FAILED",
                        "Build folder: [" + _buildFolder + "] " + Environment.NewLine +
                            "Runtime folder: [" + _runtimeFolder + "], " + Environment.NewLine +
                            "Temp files collection folder: [" + Path.Combine(_buildFolder, "~temp") + "], " + Environment.NewLine +
                            "Workflow Definition Id: " + Manifest.WorkflowDefinition.Id.ToString("b")
                    )
                );

                foreach (CompilerError error in results.Errors)
                {
                    if (!error.IsWarning)
                    {
                        hasRealErrors = true;
                    }

                    errors.Add(error);
                }

                CompileErrors = errors.AsReadOnly();

                StringBuilder errorLines = new StringBuilder();
                foreach (CompilerError error in CompileErrors)
                {
                    errorLines.AppendLine(
                        string.Format(
                            "{0} {1} in file {2} at Line {3}: {4}",
                            (error.IsWarning ? "Warning" : "Error"),
                            error.ErrorNumber,
                            error.FileName,
                            error.Line,
                            error.ErrorText
                        )
                    );
                    errorLines.AppendLine();    // blank line for readability
                }

                if (hasRealErrors)
                {
                    SetFailedCompile("Compile", "CSWF010", "Compilation failed with errors: " + errorLines.ToString());
                    // no need to return here, we fall through to end of func
                }
            }
            else
            {
                CompileSuccessful = true;
                CompileErrors = null;
                CompilerOutputAssembly = results.CompiledAssembly;

                if (Manifest.CacheCompileResults)
                {
                    if (primaryAssemblyItem == null)
                    {
                        primaryAssemblyItem = Manifest.AddItem
                        (
                            Path.GetFileNameWithoutExtension(Manifest.OutputAssemblyName),
                            Path.GetExtension(Manifest.OutputAssemblyName),
                            WorkflowManifestItemTypeEnum.PrimaryAssembly,
                            true,                                               // requiredForExecution
                            File.ReadAllBytes(results.PathToAssembly),
                            null,                                               // build relative folder
                            null                                                // runtime relative folder
                        );
                    }
                    else
                    {
                        primaryAssemblyItem.FileContent = File.ReadAllBytes(results.PathToAssembly);
                        primaryAssemblyItem.Save();
                    }
                }

                Manifest.UpdateCompileResults();
            }
        }

        private void ClearFolder(string path)
        {
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
        }

        private void CreateFolder(string path)
        {
            if (! Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        #endregion

#pragma warning restore 0618
        // This warning must be disabled to enable usage of WF3 objects above

    }

}
