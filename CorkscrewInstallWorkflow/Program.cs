using Corkscrew.SDK.objects;
using Corkscrew.SDK.security;
using Corkscrew.SDK.tools;
using Corkscrew.SDK.workflow;
using System;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace Corkscrew.Tools.InstallWorkflow
{
    class Program
    {
        static void Main(string[] args)
        {

            string manifestXmlPath = Path.Combine(Application.StartupPath, "InstallManifest.xml");

            if (args.Length > 0)
            {
                // we expect only 1 argument, the path to the manifest Xml file
                manifestXmlPath = args[0];
            }

            if (!File.Exists(manifestXmlPath))
            {
                Console.WriteLine("Manifest file was not provided or does not exist.  Path searched for file: " + manifestXmlPath);
                Application.Exit();
                return;
            }

            Console.WriteLine("WorkflowInstaller: Starting install");

            XmlDocument cfg = new XmlDocument();
            cfg.Load(manifestXmlPath);

            CSFarm farm = CSFarm.Open(CSUser.CreateSystemUser());
            if (! farm.IsWorkflowEnabled)
            {
                Console.WriteLine("Sorry, this farm does not have workflows enabled. Please enable workflows and try again.");
                Application.Exit();
                return;
            }

            foreach (XmlElement definitionElement in cfg.DocumentElement.GetElementsByTagName("WorkflowDefinition"))
            {
                string workflowDefinitionName = null;
                CSWorkflowDefinition definition = null;

                try
                {
                    workflowDefinitionName = definitionElement.Attributes["name"].Value;
                    Console.WriteLine("Installing workflow: " + workflowDefinitionName);

                    definition = farm.AllWorkflowDefinitions.Find(workflowDefinitionName);
                    if (definition != null)
                    {
                        Console.Write("Workflow definition [" + workflowDefinitionName + "] already exists. Do you wish to drop it before continuing [Y/n]?");
                        string choice = Console.ReadKey().KeyChar.ToString();
                        Console.WriteLine();
                        if (choice.ToUpper() == "Y")
                        {
                            Console.WriteLine("Deleting existing definition...");
                            definition.Delete();
                        }
                        else
                        {
                            continue;
                        }
                    }

                    definition = farm.AllWorkflowDefinitions.Add
                    (
                        workflowDefinitionName,
                        Utility.SafeString(definitionElement.Attributes["description"].Value, ""),
                        Utility.SafeString(definitionElement.Attributes["defaultAssociationData"].Value, ""),
                        Utility.SafeConvertToBool(definitionElement.Attributes["startOnCreateNewItem"].Value),
                        Utility.SafeConvertToBool(definitionElement.Attributes["startOnModifyItem"].Value),
                        Utility.SafeConvertToBool(definitionElement.Attributes["allowStartWorkflowManually"].Value)
                    );

                    Console.WriteLine("Installing workflow: " + workflowDefinitionName + " - Created.");

                    if (definition == null)
                    {
                        Console.WriteLine("Workflow definition [" + workflowDefinitionName + "] could not be created.");
                        continue;
                    }

                    XmlNodeList eventsElements = definitionElement.GetElementsByTagName("Events");
                    if (eventsElements.Count > 0)
                    {
                        XmlNode eventsElement = eventsElements[0];
                        foreach (XmlNode eventItemNode in eventsElement.ChildNodes)
                        {
                            WorkflowTriggerEventNamesEnum triggerName = WorkflowTriggerEventNamesEnum.None;
                            if (Enum.TryParse<WorkflowTriggerEventNamesEnum>(eventItemNode.Attributes["name"].Value, out triggerName))
                            {
                                definition.RegisterTrigger(triggerName);
                            }
                        }
                    }

                    XmlNodeList manifestElements = definitionElement.GetElementsByTagName("Manifest");
                    if (manifestElements.Count == 0)
                    {
                        Console.WriteLine("Workflow manifest entry for [" + workflowDefinitionName + "] not found. Please fix and try again.");
                        definition.Delete();
                        continue;
                    }

                    XmlNode manifestElement = manifestElements[0];

                    Console.WriteLine("Creating workflow manifest...");

                    CSWorkflowManifest manifest = definition.CreateManifest
                    (
                        (WorkflowEngineEnum)Enum.Parse(typeof(WorkflowEngineEnum), manifestElement.Attributes["engine"].Value),
                        Utility.SafeString(manifestElement.Attributes["assemblyName"].Value, Guid.NewGuid().ToString("n") + ".dll"),
                        Utility.SafeString(manifestElement.Attributes["className"].Value, "CorkscrewWorkflow"),
                        Utility.SafeConvertToBool(manifestElement.Attributes["alwaysCompile"].Value),
                        Utility.SafeConvertToBool(manifestElement.Attributes["cacheCompileResults"].Value)
                    );

                    if (manifest == null)
                    {
                        Console.WriteLine("Could not create manifest.");
                        definition.Delete();
                        continue;
                    }

                    foreach (XmlNode manifestNodeChildElement in manifestElement.ChildNodes)
                    {
                        if (manifestNodeChildElement.Name.Equals("Build"))
                        {
                            manifest.BuildAssemblyCompany = Utility.SafeString(manifestNodeChildElement.Attributes["company"].Value, null);
                            manifest.BuildAssemblyCopyright = Utility.SafeString(manifestNodeChildElement.Attributes["copyright"].Value, null);
                            manifest.BuildAssemblyDescription = Utility.SafeString(manifestNodeChildElement.Attributes["description"].Value, null);
                            manifest.BuildAssemblyFileVersion = new Version(Utility.SafeString(manifestNodeChildElement.Attributes["fileversion"].Value, "1.0.0.0"));
                            manifest.BuildAssemblyProduct = Utility.SafeString(manifestNodeChildElement.Attributes["product"].Value, null);
                            manifest.BuildAssemblyTitle = Utility.SafeString(manifestNodeChildElement.Attributes["name"].Value, null);
                            manifest.BuildAssemblyTrademark = Utility.SafeString(manifestNodeChildElement.Attributes["trademark"].Value, null);
                            manifest.BuildAssemblyVersion = new Version(Utility.SafeString(manifestNodeChildElement.Attributes["version"].Value, "1.0.0.0"));
                        }

                        if (manifestNodeChildElement.Name.Equals("ManifestItems"))
                        {
                            foreach (XmlNode manifestItemElement in manifestNodeChildElement.ChildNodes)
                            {
                                if (manifestItemElement.Name.Equals("Item"))
                                {
                                    string fileName = manifestItemElement.Attributes["name"].Value;
                                    string itemResourceFilePath = fileName;                         // this could be an absolute path
                                    WorkflowManifestItemTypeEnum itemType = WorkflowManifestItemTypeEnum.Unknown;
                                    if (!Enum.TryParse<WorkflowManifestItemTypeEnum>(manifestItemElement.Attributes["type"].Value, out itemType))
                                    {
                                        Console.WriteLine("Manifest item " + fileName + " has an invalid type configured. Skipping this item. You may add this item manually through another tool.");
                                        continue;
                                    }

                                    if (!File.Exists(itemResourceFilePath))
                                    {
                                        itemResourceFilePath = Path.Combine(Path.GetDirectoryName(manifestXmlPath), fileName);
                                        if (!File.Exists(itemResourceFilePath))
                                        {
                                            // primary assembly is allowed to not exist.. it will be compiled.
                                            if (itemType != WorkflowManifestItemTypeEnum.PrimaryAssembly)
                                            {
                                                Console.WriteLine("Manifest file item for [" + workflowDefinitionName + "] with name [" + fileName + "] was not found. Please fix and try again.");
                                                definition.Delete();    // will also clear the manifest and all items created thus far

                                                continue;
                                            }
                                        }
                                    }

                                    Console.WriteLine("Creating workflow manifest item... " + fileName);
                                    byte[] buffer = null;

                                    if (File.Exists(itemResourceFilePath))
                                    {
                                        buffer = File.ReadAllBytes(itemResourceFilePath);
                                    }

                                    manifest.AddItem(
                                        Path.GetFileNameWithoutExtension(fileName),
                                        Path.GetExtension(fileName),
                                        (WorkflowManifestItemTypeEnum)Enum.Parse(typeof(WorkflowManifestItemTypeEnum), manifestItemElement.Attributes["type"].Value),
                                        Utility.SafeConvertToBool(manifestItemElement.Attributes["requiredForExecution"].Value),
                                        buffer,
                                        Utility.SafeString(manifestItemElement.Attributes["buildRelativeFolder"].Value, null),
                                        Utility.SafeString(manifestItemElement.Attributes["runtimeRelativeFolder"].Value, null)
                                    );
                                }
                            }
                        }
                    }

                }
                catch
                {
                    if (definition != null)
                    {
                        definition.Delete();
                    }
                }

                Console.WriteLine("Workflow definition [" + workflowDefinitionName + "] created.");
            }

            Console.WriteLine("Completed.");
        }
    }
}
