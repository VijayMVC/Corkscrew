using Corkscrew.SDK.objects;
using Corkscrew.SDK.tools;
using Corkscrew.SDK.workflow;
using System;
using System.IO;
using System.Web;

namespace Corkscrew.ControlCenter.workflows.DefineWorkflow
{
    public partial class Step3 : System.Web.UI.Page
    {

        private Guid defId = Guid.Empty;
        private CSFarm farm = null;
        private CSWorkflowDefinition def = null;
        private CSWorkflowManifest defManifest = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            farm = CSFarm.Open(WebHelpers.GetSessionUser(HttpContext.Current));

            if (string.IsNullOrEmpty(Request.QueryString["DefinitionId"]))
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

            if (! IsPostBack)
            {
                DefinitionId.Text = def.Id.ToString("d");
            }
        }

        protected void BackButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("/workflows/DefineWorkflow/Step2.aspx?DefinitionId=" + def.Id.ToString("d"));
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            string fileName = null;
            string filenameExtension = null;
            byte[] data = null;

            ErrorMessage.Text = "";

            if (ManifestFileUpload.HasFile && (ManifestFileUpload.FileBytes.Length > 0))
            {
                if ((Path.GetExtension(ManifestFileUpload.FileName).EndsWith(".zip", StringComparison.InvariantCultureIgnoreCase)) && (UnzipUploadedFileIfZipFile.Checked))
                {
                    CSFileSystemEntryDirectory manifestDirectory = farm.RootDirectory.CreateDirectoryTree(CSPath.GetFullPath(farm.DefaultSite, string.Format("/temp/{0}/_manifestItems", def.Id.ToString("n"))));
                    if (manifestDirectory != null)
                    {
                        if (CSZipFiles.ExtractArchive(ManifestFileUpload.FileContent, manifestDirectory).Count > 0)
                        {
                            Response.Redirect(string.Format("/workflows/DefineWorkflow/FinishUpZippedItemUpload.aspx?DefinitionId={0}&workingDirectoryId={1}", def.Id.ToString("d"), manifestDirectory.Id.ToString("d")));
                        }
                    }
                }
                else
                {
                    fileName = Path.GetFileNameWithoutExtension(ManifestFileUpload.FileName);
                    filenameExtension = Path.GetExtension(ManifestFileUpload.FileName);
                    data = ManifestFileUpload.FileBytes;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(RenameUploadedFile.Text))
                {
                    ErrorMessage.Text = "No file was uploaded and no name was provided either.";
                    return;
                }

                fileName = Path.GetFileNameWithoutExtension(RenameUploadedFile.Text);
                filenameExtension = Path.GetExtension(RenameUploadedFile.Text);
                data = new byte[1] { 0 };                                               // add new manifest item does not allow null data
            }

            WorkflowManifestItemTypeEnum itemType = WorkflowManifestItemTypeEnum.CustomDataFile;
            switch (filenameExtension)
            {
                case ".dll":
                    itemType = WorkflowManifestItemTypeEnum.DependencyAssembly;
                    break;

                case ".cs":
                case ".vb":
                    itemType = WorkflowManifestItemTypeEnum.SourceCodeFile;
                    break;

                case ".xaml":
                case ".xamlx":
                case ".xoml":
                    itemType = WorkflowManifestItemTypeEnum.XamlFile;
                    break;

                case ".config":
                case ".ini":
                case ".inf":
                case ".settings":
                    itemType = WorkflowManifestItemTypeEnum.ConfigurationFile;
                    break;

                case ".jpg":
                case ".png":
                case ".ico":
                case ".mp3":
                case ".mp4":
                case ".wav":
                case ".mpeg":
                    itemType = WorkflowManifestItemTypeEnum.MediaResourceFile;
                    break;

                case ".css":
                    itemType = WorkflowManifestItemTypeEnum.Stylesheet;
                    break;

                case ".res":
                case ".resx":
                case ".resource":
                case ".resources":
                    itemType = WorkflowManifestItemTypeEnum.ResourceFile;
                    break;
            }

            CSWorkflowManifestItem item = defManifest.AddItem(fileName, filenameExtension, itemType, false, data, null, null);
            if (item == null)
            {
                ErrorMessage.Text = "Could not add item.";
                return;
            }

            Response.Redirect(string.Format("/workflows/DefineWorkflow/FinishSingleItem.aspx?DefinitionId={0}&ItemId={1}", def.Id.ToString("d"), item.Id.ToString("d")));
        }
    }
}