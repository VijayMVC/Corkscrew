using System;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace Corkscrew.Tools.ImportExportTool
{
    class Program
    {
        static void Main(string[] args)
        {
            string pathToManifestFile = Path.Combine(Application.StartupPath, "OperationManifest.xml");
            if (args.Length > 0)
            {
                foreach(string par in args)
                {
                    if (! string.IsNullOrEmpty(par))
                    {
                        if ((File.Exists(par)) && (Path.GetExtension(par).Equals(".xml", StringComparison.InvariantCultureIgnoreCase)))
                        {
                            // parameter is an xml file
                            pathToManifestFile = par;
                            break;
                        }
                    }
                }
            }

            if (! File.Exists(pathToManifestFile))
            {
                Log("Error! No valid manifest file provided.");
                return;
            }

            Log("Will use manifest file at: " + pathToManifestFile);

            // are we running on a machine with SharePoint installed?
            TestIfOnSharePointSystem test = new TestIfOnSharePointSystem();
            try
            {
                Log("Testing for SharePoint...");
                test.Test();
                Log("SharePoint libraries can be correctly loaded.");
            }
            catch
            {
                Log("This program must be run only on a system that has SharePoint Server or SharePoint Foundation installed.");
                return;
            }
            finally
            {
                test = null;
            }

            // if we got here, we have a manifest file. Determine what operation we need to do
            Log("Loading manifest...");
            XmlDocument cfg = new XmlDocument();
            cfg.Load(pathToManifestFile);

            int operationIndex = 0;
            foreach(XmlElement manifestElement in cfg.DocumentElement.GetElementsByTagName("Operation"))
            {
                Log("Found operation " + (++operationIndex));
                foreach(XmlAttribute attrib in manifestElement.Attributes)
                {
                    if (attrib.Name.Equals("type", StringComparison.InvariantCultureIgnoreCase))
                    {
                        switch (attrib.Value.ToUpper())
                        {
                            case "IMPORT":
                                Log("Filing IMPORT operation...");
                                ImportFromSharePoint importer = new ImportFromSharePoint(manifestElement);
                                importer.Run();
                                Log("Import operation completed.");
                                break;

                            case "EXPORT":
                                Log("Filing EXPORT operation...");
                                Log("Export operation completed.");
                                break;
                        }
                    }
                }
            }

            Log("All operations in manifest have been completed.");
        }

        public static void Log(string message)
        {
            Console.WriteLine(message);
        }
    }
}
