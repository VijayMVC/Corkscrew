using Corkscrew.SDK.objects;
using Corkscrew.SDK.tools;
using Corkscrew.SDK.workflow;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace Corkscrew.ControlCenter.workflows.DefineWorkflow
{
    public partial class FinishUpZippedItemUpload : System.Web.UI.Page
    {

        private Guid defId = Guid.Empty;
        private CSFarm farm = null;
        private CSWorkflowDefinition def = null;
        private CSWorkflowManifest defManifest = null;
        public CSFileSystemEntryDirectory workingDirectory = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            farm = CSFarm.Open(WebHelpers.GetSessionUser(HttpContext.Current));

            if ((string.IsNullOrEmpty(Request.QueryString["DefinitionId"])) || (string.IsNullOrEmpty(Request.QueryString["workingDirectoryId"])))
            {
                Response.Redirect("/workflows/WorkflowDefinitions.aspx");
            }

            defId = Utility.SafeConvertToGuid(Request.QueryString["DefinitionId"]);
            if (defId == Guid.Empty)
            {
                Response.Redirect("/workflows/WorkflowDefinitions.aspx");
            }

            def = farm.AllWorkflowDefinitions.Find(defId);
            if (def == null)
            {
                Response.Redirect("/workflows/WorkflowDefinitions.aspx");
            }

            defManifest = def.GetManifest();
            if (defManifest == null)
            {
                Response.Redirect("/workflows/Step2.aspx?DefinitionId=" + defId);
            }

            Guid workingdirectoryId = Utility.SafeConvertToGuid(Request.QueryString["workingDirectoryId"]);
            if (workingdirectoryId == Guid.Empty)
            {
                Response.Redirect("/workflows/DefineWorkflow/Step3.aspx?DefinitionId=" + def.Id.ToString("d"));
            }

            workingDirectory = farm.DefaultSite.GetDirectory(workingdirectoryId);
            if (workingDirectory == null)
            {
                Response.Redirect("/workflows/DefineWorkflow/Step3.aspx?DefinitionId=" + def.Id.ToString("d"));
            }

            UploadedFiles.ItemDataBound += UploadedFiles_ItemDataBound;
            UploadedFiles.ItemCommand += UploadedFiles_ItemCommand;

            if (! IsPostBack)
            {
                List<CSFileSystemEntryFile> files = GetFilesRecursively(workingDirectory);
                if (files.Count == 0)
                {
                    Response.Redirect("/workflows/WorkflowDefinitions.aspx");
                }

                UploadedFiles.DataSource = files;
                UploadedFiles.DataBind();
            }
        }

        private void UploadedFiles_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            Guid tempFileId = Utility.SafeConvertToGuid(e.CommandArgument);
            CSFileSystemEntryFile file = farm.DefaultSite.GetFile(tempFileId);

            switch (e.CommandName)
            {
                case "Update":
                    TextBox fileName = (TextBox)e.Item.FindControl("rowFilename");
                    TextBox extension = (TextBox)e.Item.FindControl("rowFilenameExtension");
                    DropDownList list = (DropDownList)e.Item.FindControl("rowItemType");
                    WorkflowManifestItemTypeEnum type = (WorkflowManifestItemTypeEnum)Enum.Parse(typeof(WorkflowManifestItemTypeEnum), list.SelectedItem.Value);
                    CheckBox reqExec = (CheckBox)e.Item.FindControl("rowRequiredForExec");
                    TextBox buildFolder = (TextBox)e.Item.FindControl("rowBuildFolder");
                    TextBox runFolder = (TextBox)e.Item.FindControl("rowRuntimeFolder");
                    byte[] data = null;

                    if (file.Open(FileAccess.Read))
                    {
                        data = new byte[file.Size];
                        file.Read(data, 0, file.Size);
                        file.Close();
                    }

                    CSWorkflowManifestItem item = defManifest.AddItem(fileName.Text, extension.Text, type, reqExec.Checked, data, buildFolder.Text, runFolder.Text);
                    if (item != null)
                    {
                        file.Delete();
                    }

                    break;

                case "Delete":
                    file.Delete();
                    break;
            }

            List<CSFileSystemEntryFile> files = GetFilesRecursively(workingDirectory);
            if (files.Count == 0)
            {
                Response.Redirect("/workflows/WorkflowDefinitions.aspx");
            }

            UploadedFiles.DataSource = files;
            UploadedFiles.DataBind();
        }

        private void UploadedFiles_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item)
            {
                return;
            }

            CSFileSystemEntryFile file = (CSFileSystemEntryFile)e.Item.DataItem;
            if (file == null)
            {
                return;
            }

            DropDownList list = (DropDownList)e.Item.FindControl("rowItemType");
            if (list == null)
            {
                return;
            }

            WorkflowManifestItemTypeEnum guessedItemType = WorkflowManifestItemTypeEnum.CustomDataFile;
            switch (file.FilenameExtension)
            {
                case ".dll":
                    guessedItemType = WorkflowManifestItemTypeEnum.DependencyAssembly;
                    break;

                case ".cs":
                case ".vb":
                    guessedItemType = WorkflowManifestItemTypeEnum.SourceCodeFile;
                    break;

                case ".xaml":
                case ".xamlx":
                case ".xoml":
                    guessedItemType = WorkflowManifestItemTypeEnum.XamlFile;
                    break;

                case ".config":
                case ".ini":
                case ".inf":
                case ".settings":
                    guessedItemType = WorkflowManifestItemTypeEnum.ConfigurationFile;
                    break;

                case ".jpg":
                case ".png":
                case ".ico":
                case ".mp3":
                case ".mp4":
                case ".wav":
                case ".mpeg":
                    guessedItemType = WorkflowManifestItemTypeEnum.MediaResourceFile;
                    break;

                case ".css":
                    guessedItemType = WorkflowManifestItemTypeEnum.Stylesheet;
                    break;

                case ".res":
                case ".resx":
                case ".resource":
                case ".resources":
                    guessedItemType = WorkflowManifestItemTypeEnum.ResourceFile;
                    break;
            }

            list.Items.FindByValue(Enum.GetName(typeof(WorkflowManifestItemTypeEnum), guessedItemType)).Selected = true;

        }

        private List<CSFileSystemEntryFile> GetFilesRecursively(CSFileSystemEntryDirectory directory)
        {
            List<CSFileSystemEntryFile> files = new List<CSFileSystemEntryFile>();

            foreach(CSFileSystemEntryFile file in directory.Files)
            {
                files.Add(file);
            }

            foreach(CSFileSystemEntryDirectory dir in directory.Directories)
            {
                files.AddRange(GetFilesRecursively(dir));
            }

            return files;   
        }

    }
}