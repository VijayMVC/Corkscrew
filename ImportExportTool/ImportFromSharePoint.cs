using Corkscrew.SDK.objects;
using Corkscrew.SDK.security;
using Corkscrew.SDK.tools;
using Microsoft.SharePoint;
using System.DirectoryServices.AccountManagement;
using System;
using System.IO;
using System.Xml;
using Corkscrew.SDK.ActiveDirectory;
using System.Collections.Generic;

namespace Corkscrew.Tools.ImportExportTool
{

    /// <summary>
    /// This class imports information from SharePoint into Corkscrew
    /// </summary>
    public class ImportFromSharePoint
    {

        private string sourceUrl = null, sourceUserName = null, sourcePassword = null;
        private string destinationUrl = null, destinationUserName = null, destinationPassword = null;
        private string importedUsersPassword = string.Empty;
        private bool overwrite = true, skipSecurity = false;

        private CSFarm corkscrewFarm = null;
        private PrincipalContext activeDirectoryPrincipalContext = null;
        private SPUser sharepointSystemAccount = null;

        /// <summary>
        /// Import element from the manifest Xml
        /// </summary>
        public XmlElement OperationElement { get; set; }

        /// <summary>
        /// Constructor. This method does not start the operation.
        /// </summary>
        /// <param name="operationElement">The XmlElement reference to the Operation tag in the manifest Xml</param>
        public ImportFromSharePoint(XmlElement operationElement)
        {
            OperationElement = operationElement;

            Program.Log("Determining import/export credentials and paths...");
            foreach (XmlElement childElement in OperationElement.ChildNodes)
            {
                if (childElement.Name.Equals("Source", StringComparison.InvariantCultureIgnoreCase))
                {
                    foreach (XmlAttribute attrib in childElement.Attributes)
                    {
                        switch (attrib.Name.ToUpper())
                        {
                            case "URL":
                                sourceUrl = attrib.Value;
                                break;

                            case "USERNAME":
                                sourceUserName = attrib.Value;
                                break;

                            case "PASSWORD":
                                sourcePassword = attrib.Value;
                                break;
                        }
                    }
                }
                else if (childElement.Name.Equals("Destination", StringComparison.InvariantCultureIgnoreCase))
                {
                    foreach (XmlAttribute attrib in childElement.Attributes)
                    {
                        switch (attrib.Name.ToUpper())
                        {
                            case "URL":
                                destinationUrl = attrib.Value;
                                break;

                            case "USERNAME":
                                destinationUserName = attrib.Value;
                                break;

                            case "PASSWORD":
                                destinationPassword = attrib.Value;
                                break;
                        }
                    }
                }
                else if (childElement.Name.Equals("Options", StringComparison.InvariantCultureIgnoreCase))
                {
                    overwrite = Utility.SafeConvertToBool((childElement.Attributes["overwrite"] != null) ? childElement.Attributes["overwrite"].Value : "true");
                    skipSecurity = Utility.SafeConvertToBool((childElement.Attributes["skipSecurity"] != null) ? childElement.Attributes["skipSecurity"].Value : "false");
                    importedUsersPassword = Utility.SafeString(childElement.Attributes["passwordForImportedUsers"]?.Value, string.Empty);
                }
            }

            // validate if we have everything!
            if (
                    string.IsNullOrEmpty(sourceUrl)
                 || string.IsNullOrEmpty(destinationUrl) || string.IsNullOrEmpty(destinationUserName) || string.IsNullOrEmpty(destinationPassword)
            )
            {
                throw new Exception("Source/destination url, username and password must be provided.");
            }
        }

        /// <summary>
        /// Runs the import operation
        /// </summary>
        /// <returns>Success of the operation</returns>
        public bool Run()
        {
            // break to attach debugger
            Console.WriteLine("Hit ENTER after debugger is attached.");
            Console.ReadLine();

            // are we domain connected?
            string adDomainName = sourceUserName.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries)[0];
            sourceUserName = sourceUserName.Replace(adDomainName + "\\", "");

            try
            {
                activeDirectoryPrincipalContext = new PrincipalContext(ContextType.Domain, "", sourceUserName, sourcePassword);
            }
            catch
            {
                // not a domain or we dont have access with those credentials.
                Program.Log("Could not establish Active Directory Context. This is okay if you are not using an Active Directory domain. If you are, ensure the Username and Password provided for Source element has access to read user and group information from Active Directory.");
            }

            // try to open a connection to the SPSite
            Program.Log("Connecting to SharePoint...");
            SPSite sharepointSite = new SPSite(sourceUrl, SPUserToken.SystemAccount);
            if (sharepointSite == null)
            {
                Program.Log("Could not open a connection to the SharePoint site at " + sourceUrl);
                return false;
            }

            // save it to the global var
            sharepointSystemAccount = sharepointSite.SystemAccount;

            // now open a Corkscrew connection
            Program.Log("Connecting to Corkscrew...");

            Program.Log("Logging in (this can take a few seconds the first time)... ");
            CSUser corkscrewUser = CSUser.Login(destinationUserName, Utility.GetSha256Hash(destinationPassword));
            if (corkscrewUser == null)
            {
                Program.Log("Could not login to Corkscrew with username and password. Username was: " + destinationUserName);
                return false;
            }

            Program.Log("Opening connection to Farm...");
            corkscrewFarm = CSFarm.Open(corkscrewUser);
            if (corkscrewFarm == null)
            {
                Program.Log("Could not open the Corkscrew Farm.");
                return false;
            }

            Program.Log("Enumerating SharePoint webs...");
            foreach (SPWeb spWeb in sharepointSite.AllWebs)
            {
                string autoWebName = Utility.SafeString(spWeb.Name, string.Format("ImportedSPSWeb{0}", spWeb.ID.ToString("n")));
                Program.Log("Processing SharePoint web " + autoWebName);

                CSSite corkscrewSite = corkscrewFarm.AllSites.Find(autoWebName);
                if (corkscrewSite == null)
                {
                    Program.Log("Attempting to create Corkscrew Site...");
                    string autoWebDescription = Utility.SafeString(spWeb.Description, string.Format("Imported from SharePoint {0} Web ID {1}", spWeb.Url, spWeb.ID.ToString("d")));
                    long quota = ((sharepointSite.Quota != null) ? sharepointSite.Quota.StorageMaximumLevel : 0);

                    corkscrewSite = corkscrewFarm.AllSites.Add(autoWebName, corkscrewUser, autoWebDescription, null, quota);
                    if (corkscrewSite == null)
                    {
                        Program.Log("Unable to open or create Corkscrew site for SPWeb " + spWeb.Name + "(Guid: " + spWeb.ID.ToString("d") + ")");
                        continue;
                    }
                }

                // process Site Admins
                if (!skipSecurity)
                {
                    Program.Log("Processing Site Admins...");
                    foreach (SPUser webAdmin in spWeb.SiteAdministrators)
                    {
                        ImportSharepointUser(webAdmin, corkscrewSite.FullPath, true, true, true); // add as admin
                    }

                    // import site groups
                    foreach (SPGroup siteGroup in spWeb.SiteGroups)
                    {
                        if (siteGroup.CanCurrentUserViewMembership)
                        {
                            ImportSharepointPrincipals(spWeb.RoleAssignments, corkscrewSite.FullPath);
                        }
                    }
                }

                foreach (SPList spList in spWeb.Lists)
                {
                    Program.Log("-".PadRight(40, '-'));
                    if ((spList.BaseType != SPBaseType.DocumentLibrary) && (spList.BaseType != SPBaseType.GenericList))
                    {
                        Program.Log("Not importing SharePoint list with name " + spList.Title + " (Guid: " + spList.ID.ToString("d") + ")");
                        continue;
                    }

                    Program.Log("Processing SharePoint list " + spList.Title);

                    switch (spList.BaseType)
                    {
                        case SPBaseType.DocumentLibrary:
                            Program.Log("Determining if we have a matching directory in Corkscrew...");
                            CSFileSystemEntryDirectory corkscrewImportDirectory = corkscrewSite.RootFolder.Directories.Find(spList.Title);
                            if (corkscrewImportDirectory == null)
                            {
                                Program.Log("Matching folder not found. Creating Corkscrew import destination directory " + spList.Title);
                                corkscrewImportDirectory = corkscrewSite.RootFolder.Directories.Add(corkscrewSite.RootFolder, spList.Title, "", spList.Created, corkscrewUser);
                                if (corkscrewImportDirectory == null)
                                {
                                    Program.Log("Unable to find or create import destination in Corkscrew for SharePoint list with name " + spList.Title + " (Guid: " + spList.ID.ToString("d") + ")");
                                    return false;
                                }
                            }

                            Program.Log("Will import " + spList.Title + " to " + corkscrewImportDirectory.FullPath);

                            if (!skipSecurity)
                            {
                                Program.Log("Importing list security users and ACLs...");
                                ImportSharepointPrincipals(spList.RoleAssignments, corkscrewImportDirectory.FullPath);
                            }

                            Program.Log("Importing list items for list " + spList.Title);
                            foreach (SPListItem spListItem in spList.Items)
                            {
                                ImportDocumentLibraryListItem(spListItem, corkscrewImportDirectory);
                            }

                            break;

                        case SPBaseType.GenericList:
                            ImportSharepointList(spList, corkscrewSite);
                            break;
                    }
                }
            }

            Program.Log("SharePoint contents have been processed.");

            return true;
        }

        private void ImportSharepointList(SPList spList, CSSite corkscrewSite)
        {
            Program.Log("Determining if we have a matching data table in Corkscrew...");

            string tableName = spList.Title;
            bool tableCanBeRolledBack = false;
            List<CSTableColumnDefinition> _undoColumnDefs = new List<CSTableColumnDefinition>();

            CSTable tableDef = corkscrewSite.Tables[tableName];
            if (tableDef == null)
            {
                Program.Log("Matching data table not found. Creating destination table " + tableName);
                tableDef = new CSTable(corkscrewSite, tableName, tableName, spList.Description);
                tableCanBeRolledBack = true;
            }

            foreach (SPField splistField in spList.Fields)
            {
                if (!splistField.Hidden)
                {
                    Program.Log("Processing field " + splistField.InternalName);
                    if (corkscrewSite.Columns[splistField.InternalName] == null)
                    {
                        CSTableColumnDataTypeEnum type = GetCSTypeForSPType(splistField.Type);
                        if (type == CSTableColumnDataTypeEnum.Unknown)
                        {
                            Program.Log("Skipping field. Type not supported: " + Enum.GetName(typeof(SPFieldType), splistField.Type));
                            continue;
                        }

                        int maxLength = 0;
                        if (type == CSTableColumnDataTypeEnum.String)
                        {
                            if (splistField.Type == SPFieldType.Text)
                            {
                                maxLength = ((SPFieldText)splistField).MaxLength;
                                if (maxLength > 4000)
                                {
                                    type = CSTableColumnDataTypeEnum.Text;
                                }
                            }
                            else if ((splistField.Type == SPFieldType.Choice) || (splistField.Type == SPFieldType.MultiChoice))
                            {
                                SPFieldChoice chcField = (SPFieldChoice)splistField;

                                // set max length to maximum choice length
                                foreach (string ch in chcField.Choices)
                                {
                                    if ((!string.IsNullOrEmpty(ch)) && (maxLength < ch.Length))
                                    {
                                        maxLength = ch.Length;
                                    }
                                }
                            }
                        }

                        Program.Log("Creating corkscrew table column " + splistField.InternalName);
                        CSTableColumnDefinition colDef = null;

                        if (maxLength != 0)
                        {
                            colDef = corkscrewSite.Columns.Add(splistField.InternalName, maxLength, (!splistField.Required));
                        }
                        else
                        {
                            colDef = corkscrewSite.Columns.Add(splistField.InternalName, type, (!splistField.Required));
                        }

                        if (colDef != null)
                        {
                            _undoColumnDefs.Add(colDef);
                            tableDef.Columns.Add(colDef, splistField.StaticName);
                        }
                    }
                    else
                    {
                        CSTableColumnDefinition colDef = corkscrewSite.Columns[splistField.InternalName];
                        CSTableColumnDataTypeEnum splistfieldDataType = GetCSTypeForSPType(splistField.Type);

                        if (colDef.DataType != splistfieldDataType)
                        {
                            Program.Log("Column " + splistField.InternalName + " exists in Corkscrew table, but is of different type. Expected: " + Enum.GetName(typeof(CSTableColumnDataTypeEnum), colDef.DataType) + " but is a " + Enum.GetName(typeof(CSTableColumnDataTypeEnum), splistfieldDataType));

                            // will cause the if statement outside to skip the list.
                            tableDef.Columns.Clear();
                            break;
                        }

                        if (tableDef.Columns[splistField.StaticName] == null)
                        {
                            tableDef.Columns.Add(colDef, splistField.StaticName);
                        }
                    }
                }
            }

            tableDef.RefreshColumns();
            if (tableDef.Columns.Count == 0)
            {
                // no columns, skip to next list
                Program.Log("Did not find any usable columns in list");

                if (tableCanBeRolledBack)
                {
                    // undo coldefs
                    foreach(CSTableColumnDefinition colDef in _undoColumnDefs)
                    {
                        colDef.Drop();
                    }

                    // drop table
                    tableDef.Drop();
                }

                return;
            }

            // now import list data.
            Program.Log("Importing datalist items (count: " + spList.Items.Count + " items)...");
            foreach (SPListItem item in spList.Items)
            {
                ImportDataListLibraryListItem(item, tableDef);
            }

            Program.Log("Import from " + spList.Title + " is complete.");
        }

        private CSTableColumnDataTypeEnum GetCSTypeForSPType(SPFieldType spFieldType)
        {
            CSTableColumnDataTypeEnum csType = CSTableColumnDataTypeEnum.Unknown;

            switch (spFieldType)
            {
                case SPFieldType.Boolean:
                    csType = CSTableColumnDataTypeEnum.Boolean;
                    break;

                case SPFieldType.DateTime:
                    csType = CSTableColumnDataTypeEnum.DateTime;
                    break;

                case SPFieldType.Integer:
                case SPFieldType.Counter:
                    csType = CSTableColumnDataTypeEnum.Integer;
                    break;

                case SPFieldType.Text:
                case SPFieldType.Choice:
                case SPFieldType.MultiChoice:
                    csType = CSTableColumnDataTypeEnum.String;
                    break;

                case SPFieldType.Number:
                    csType = CSTableColumnDataTypeEnum.FloatingPoint;
                    break;

                case SPFieldType.Currency:
                    csType = CSTableColumnDataTypeEnum.FloatingPoint;
                    break;

                case SPFieldType.Guid:
                    csType = CSTableColumnDataTypeEnum.Guid;
                    break;
            }

            return csType;
        }

        private void ImportDocumentLibraryListItem(SPListItem sharepointListItem, CSFileSystemEntryDirectory corkscrewImportDirectory)
        {
            CSFileSystemEntry importedItem = null;

            Program.Log("Processing list item " + sharepointListItem.Name);

            switch (sharepointListItem.FileSystemObjectType)
            {
                case SPFileSystemObjectType.Folder:
                    Program.Log("Finding directory in Corkscrew " + sharepointListItem.Name);
                    CSFileSystemEntryDirectory childDirectory = corkscrewImportDirectory.Directories.Find(sharepointListItem.Name);
                    if (childDirectory == null)
                    {
                        Program.Log("Creating directory in Corkscrew " + sharepointListItem.Name);
                        childDirectory = corkscrewImportDirectory.CreateDirectory(sharepointListItem.Name);
                    }
                    importedItem = childDirectory;

                    foreach (SPListItem childItem in sharepointListItem.ListItems)
                    {
                        ImportDocumentLibraryListItem(childItem, childDirectory);
                    }
                    break;

                case SPFileSystemObjectType.File:
                    SPFile listitemFile = sharepointListItem.File;
                    if (listitemFile != null)
                    {
                        // check if file length is within our limits
                        if (listitemFile.Length > int.MaxValue)
                        {
                            Program.Log("SharePoint file is too big. Must be 2GB or less.");
                            return;
                        }

                        Program.Log("Finding file in Corkscrew " + listitemFile.Name);
                        importedItem = corkscrewImportDirectory.Files.Find(listitemFile.Name);
                        if (importedItem == null)
                        {
                            Program.Log("Creating file in Corkscrew " + listitemFile.Name);
                            importedItem = corkscrewImportDirectory.CreateFile(Path.GetFileNameWithoutExtension(listitemFile.Name), Path.GetExtension(listitemFile.Name), listitemFile.OpenBinary());
                        }
                        else
                        {
                            // update file?
                            if (listitemFile.TimeLastModified > importedItem.Modified)
                            {
                                CSFileSystemEntryFile file = new CSFileSystemEntryFile(importedItem, false);
                                if (file.Open(FileAccess.Write))
                                {
                                    file.Write(listitemFile.OpenBinary(), 0, (int)listitemFile.Length); // safe as int because we checked the file size above
                                    file.Close();
                                }
                            }
                        }
                    }
                    break;

                default:
                    Program.Log("Not importing SharePoint list item with path " + sharepointListItem.Url + " (Type: " + Enum.GetName(typeof(SPFileSystemObjectType), sharepointListItem.FileSystemObjectType) + ", Guid: " + sharepointListItem.ID.ToString("d") + ")");
                    break;
            }

            if ((importedItem != null) && (! skipSecurity))
            {
                Program.Log("Importing list security users and ACLs...");
                ImportSharepointPrincipals(sharepointListItem.RoleAssignments, corkscrewImportDirectory.FullPath);
            }
        }

        private void ImportDataListLibraryListItem(SPListItem datalistListItem, CSTable table)
        {
            if (datalistListItem.Folder != null)
            {
                Program.Log("Listitem " + datalistListItem.ID.ToString() + " is a folder. Diving...");
                foreach (SPListItem item in datalistListItem.ListItems)
                {
                    ImportDataListLibraryListItem(item, table);
                }
            }
            else
            {
                CSTableRow row = table.Rows.NewRow();
                Program.Log("Processing list item " + Utility.SafeString(datalistListItem["ID"]));
                foreach (SPField field in datalistListItem.Fields)
                {
                    if (table.Columns[field.InternalName] != null)
                    {
                        row[field.InternalName] = datalistListItem[field.InternalName];
                    }
                }

                row.CommitChanges();

                // dont add the row to the tabledef row collection. 
                // we are potentially importing thousands of rows from SharePoint!

                if (!skipSecurity)
                {
                    Program.Log("Importing datalist security users and ACLs...");
                    ImportSharepointPrincipals(datalistListItem.RoleAssignments, table.FullPath);
                }
            }
        }

        private string GetProperPrincipalName(string spPrincipalName)
        {
            // spPrincipalName will be like "i:0#.w|domain\user"
            // we need to get just the domain\user part
            if (spPrincipalName.Contains("|"))
            {
                return spPrincipalName.Substring(spPrincipalName.IndexOf("|") + 1);
            }

            return spPrincipalName;
        }

        private void ImportSharepointPrincipals(SPRoleAssignmentCollection roleAssignments, string resourceUri)
        {
            if (skipSecurity)
            {
                return;
            }

            foreach (SPRoleAssignment role in roleAssignments)
            {
                if ((role.Member is SPUser) && (role.Member.ID == sharepointSystemAccount.ID))
                {
                    continue;   // skip system account
                }

                bool read = false, contribute = false, fullcontrol = false;

                foreach (SPRoleDefinition roleDef in role.RoleDefinitionBindings)
                {
                    switch (roleDef.Type)
                    {
                        case SPRoleType.Administrator:
                            fullcontrol = true;
                            break;

                        case SPRoleType.Contributor:
                        case SPRoleType.WebDesigner:
                            contribute = true;
                            break;

                        case SPRoleType.Reader:
                            read = true;
                            break;
                    }
                }

                // keep only highest privilege
                if (fullcontrol)
                {
                    contribute = false;
                    read = false;
                }
                else if (contribute)
                {
                    read = false;
                }

                if ((! read) && (! contribute) && (! fullcontrol))
                {
                    // no acl
                    continue;
                }

                // what is the member type?
                if (role.Member is SPUser)
                {
                    ImportSharepointUser((SPUser)role.Member, resourceUri, read, contribute, fullcontrol);
                }
                else
                {
                    if (role.Member is SPGroup)
                    {
                        SPGroup sharepointGroup = (SPGroup)role.Member;

                        string loginName = GetProperPrincipalName(sharepointGroup.LoginName);
                        Program.Log("Creating usergroup in Corkscrew for " + loginName);
                        CSUserGroup corkscrewGroup = CSUserGroup.GetByName(loginName);
                        if (corkscrewGroup == null)
                        {
                            Program.Log("Creating usergroup in Corkscrew for " + loginName);
                            corkscrewGroup = CSUserGroup.CreateUserGroup(loginName, sharepointGroup.Name, sharepointGroup.DistributionGroupEmail);
                        }

                        if (corkscrewGroup != null)
                        {
                            Program.Log("Setting ACL Corkscrew for usergroup " + loginName);
                            SetCorkscrewACL(corkscrewGroup, resourceUri, read, contribute, fullcontrol);
                        }

                        Program.Log("Expanding and importing SharePoint group " + sharepointGroup.Name);
                        foreach (SPUser sharepointUser in sharepointGroup.Users)
                        {
                            ImportSharepointUser(sharepointUser, resourceUri, read, contribute, fullcontrol);
                        }
                    }
                }
            }
        }

        private void ImportSharepointUser(SPUser sharepointUser, string resourceUri, bool read, bool contribute, bool fullcontrol)
        {
            if (skipSecurity)
            {
                return;
            }

            string loginName = GetProperPrincipalName(sharepointUser.LoginName);

            Program.Log("Processing SharePoint user " + loginName);

            if (sharepointUser.IsDomainGroup)
            {
                // AD domain group
                Program.Log(loginName + " is an active directory group. Searching for Corkscrew Group...");
                CSUserGroup corkscrewGroup = CSActiveDirectoryUserGroup.GetByName(loginName);
                if (corkscrewGroup == null)
                {
                    Program.Log("User group not found. Creating usergroup in Corkscrew for " + loginName);
                    corkscrewGroup = CSActiveDirectoryUserGroup.CreateUserGroup(loginName, sharepointUser.Name, sharepointUser.Email);
                }

                if (corkscrewGroup != null)
                {
                    Program.Log("Setting ACL Corkscrew for " + loginName);
                    SetCorkscrewACL(corkscrewGroup, resourceUri, read, contribute, fullcontrol);
                }
            }
            else
            {
                Program.Log(loginName + " is an active directory user. Searching for Corkscrew User...");
                CSUser corkscrewUser = CSActiveDirectoryUser.GetByUsername(loginName);
                if (corkscrewUser == null)
                {
                    Program.Log("User not found. Creating user in Corkscrew for " + loginName);
                    corkscrewUser = CSActiveDirectoryUser.CreateUser(loginName, sharepointUser.Name, sharepointUser.Email);
                }

                if (corkscrewUser != null)
                {
                    Program.Log("Setting ACL in Corkscrew for " + loginName);
                    SetCorkscrewACL(corkscrewUser, resourceUri, read, contribute, fullcontrol);
                }
            }

            Program.Log("User " + loginName + " processed.");
        }

        private void SetCorkscrewACL(CSSecurityPrincipal corkscrewPrincipal, string resourceUri, bool read, bool contribute, bool fullcontrol)
        {

            if (skipSecurity)
            {
                return;
            }

            Program.Log("Testing current access for user " + corkscrewPrincipal.Username + " on " + resourceUri);
            CSPermission acl = CSPermission.TestAccess(resourceUri, corkscrewPrincipal);

            bool aclChanged = false;
            if (acl.CanFullControl != fullcontrol)
            {
                Program.Log("Adding ACL: Full control...");
                acl.CanFullControl = fullcontrol;
                if (fullcontrol)
                {
                    acl.CanContribute = false;
                    acl.CanRead = false;
                }
                aclChanged = true;
            }
            else if (acl.CanContribute != contribute)
            {
                Program.Log("Adding ACL: Contribute...");
                acl.CanContribute = contribute;
                if (contribute)
                {
                    acl.CanFullControl = false;
                    acl.CanRead = false;
                }

                aclChanged = true;
            }
            else if (acl.CanRead != read)
            {
                Program.Log("Adding ACL: Readonly...");
                acl.CanRead = read;
                if (read)
                {
                    acl.CanFullControl = false;
                    acl.CanContribute = false;
                }
                aclChanged = true;
            }

            if (aclChanged)
            {
                Program.Log("Saving ACL...");
                acl.Save();
            }

            Program.Log((aclChanged ? "ACL was saved successfully." : "ACL was not changed as required or higher access was already granted."));
        }

    }
}
