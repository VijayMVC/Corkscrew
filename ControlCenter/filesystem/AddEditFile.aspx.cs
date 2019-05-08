using Corkscrew.SDK.objects;
using Corkscrew.SDK.tools;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace Corkscrew.ControlCenter.filesystem
{
    public partial class AddEditFile : System.Web.UI.Page
    {
        private Guid siteId = Guid.Empty;
        private CSSite editSite = null;
        private string parentPath = null;
        private CSFileSystemEntryDirectory parentDirectory = null;
        private CSFileSystemEntryFile editItem = null;
        public bool editorEnabled = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Request.QueryString["SiteId"]) || string.IsNullOrEmpty("Path"))
            {
                // we dont have a SiteId to redirect to
                Response.Redirect("/sites/All.aspx");
            }

            siteId = Utility.SafeConvertToGuid(Request.QueryString["SiteId"]);
            editSite = CSFarm.Open(WebHelpers.GetSessionUser(HttpContext.Current)).AllSites.Find(siteId);
            if (editSite == null)
            {
                Response.Redirect("/sites/All.aspx");
            }

            parentPath = Server.UrlDecode(Request.QueryString["Path"]);
            parentDirectory = editSite.GetDirectory(CSPath.GetFullPath(editSite, parentPath));
            if (parentDirectory == null)
            {
                RedirectToSiteExplorer();
            }

            ParentDirectoryPath.Text = parentDirectory.FullPath;
            CorkscrewUri.Text = "(calculated after creation)";
            FileContentEditor.Visible = false;

            List<string> editorEnabledExtensions = ConfigurationManager.AppSettings["OnPageEditor:EnabledExtensions"].ToLower().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();

            if (!string.IsNullOrEmpty(Request.QueryString["ItemId"]))
            {
                Guid editItemId = Utility.SafeConvertToGuid(Request.QueryString["ItemId"]);
                editItem = editSite.GetFile(editItemId);
                if (editItem == null)
                {
                    RedirectToSiteExplorer();
                }

                if (editorEnabledExtensions.Contains(editItem.FilenameExtension.ToLower()))
                {
                    FileContentEditor.Visible = true;
                    editorEnabled = true;
                }

                if (!IsPostBack)
                {
                    CorkscrewUri.Text = editItem.FullPath;
                    Filename.Text = editItem.Filename;
                    FilenameExtension.Text = editItem.FilenameExtension;
                    Attributes.Items[0].Selected = editItem.IsReadonly;
                    Attributes.Items[1].Selected = editItem.IsHidden;

                    OptionUseNamedFileMethod.Checked = true;
                    panelNamedMethod.Visible = true;
                    panelUploadMethod.Visible = false;

                    if (editorEnabled && (editItem.Size > 0))
                    {
                        byte[] buffer = new byte[editItem.Size];
                        if (editItem.Open(FileAccess.Read))
                        {
                            if (editItem.Read(buffer, 0, editItem.Size) > 0)
                            {
                                FileContent.Text = Encoding.UTF8.GetString(buffer);
                            }
                        }
                    }
                }
            }
            else
            {
                if (! IsPostBack)
                {
                    OptionUseUploadFileMethod.Checked = true;
                    panelNamedMethod.Visible = false;
                    panelUploadMethod.Visible = true;
                }
            }
        }

        protected void CancelButton_Click(object sender, EventArgs e)
        {
            RedirectToSiteExplorer();
        }

        private void RedirectToSiteExplorer()
        {
            Response.Redirect(string.Format("/filesystem/Explorer.aspx?SiteId={0}&Path={1}", siteId.ToString("d"), Server.UrlEncode(parentPath)));
        }

        protected void CreateButton_Click(object sender, EventArgs e)
        {

            ErrorMessage.Text = "";

            if (OptionUseNamedFileMethod.Checked)
            {
                if (string.IsNullOrEmpty(Filename.Text) && string.IsNullOrEmpty(FilenameExtension.Text))
                {
                    ErrorMessage.Text = "Either filename or filename extension must be provided.";
                    return;
                }
            }
            else if (OptionUseUploadFileMethod.Checked)
            {
                if ((!UploadedFile.HasFile) || (UploadedFile.FileBytes.Length == 0))
                {
                    ErrorMessage.Text = "No file was selected for upload.";
                    return;
                }
            }

            string name = (OptionUseNamedFileMethod.Checked ? Filename.Text : Path.GetFileNameWithoutExtension(UploadedFile.FileName));
            string extension = (OptionUseNamedFileMethod.Checked ? FilenameExtension.Text : Path.GetExtension(UploadedFile.FileName));

            if (editItem != null)
            {
                string newItemFileNameWithExtension = Utility.SafeString
                    (
                        string.Format(
                            "{0}{1}",
                            Utility.SafeString(name, ""),
                            Utility.SafeString(extension, "", expectStart: ".")
                        )
                        , removeAtEnd: "."      // full filename cannot end with a ".", but it can start with a "." if the filename is missing.
                    );

                if (OptionUseUploadFileMethod.Checked && (!Path.GetFileName(UploadedFile.FileName).Equals(newItemFileNameWithExtension)))
                {
                    ErrorMessage.Text = "Name of the uploaded file does not match the file you are editing.";
                    return;
                }

                if (editItem.FilenameWithExtension != newItemFileNameWithExtension)
                {
                    editItem.Rename(name, extension);
                }
            }
            else
            {
                editItem = parentDirectory.CreateFile(name, extension);
            }

            if (editItem != null)
            {
                byte[] data = null;
                bool deleteFile = false;

                if (UploadedFile.HasFile && (UploadedFile.FileBytes.Length > 0))
                {
                    data = UploadedFile.FileBytes;
                }
                else if (editorEnabled)
                {
                    data = Encoding.UTF8.GetBytes(FileContent.Text);
                }

                if (data != null)
                {
                    if (editItem.Open(FileAccess.Write))
                    {
                        editItem.Write(data, 0, data.Length);
                        editItem.Close();

                        data = null;
                    }
                    else
                    {
                        deleteFile = true;
                    }
                }
                else
                {
                    deleteFile = true;
                }

                if (deleteFile)
                {
                    // undo file create instead of leaving a 0-byte file
                    editItem.Delete();
                    editItem = null;

                    ErrorMessage.Text = "File could not be opened for editing.";
                    return;
                }
            }

            if (Attributes.Items[0].Selected != editItem.IsReadonly)
            {
                editItem.IsReadonly = Attributes.Items[0].Selected;
                editItem.Save();
            }

            if (Attributes.Items[1].Selected != editItem.IsHidden)
            {
                editItem.IsHidden = Attributes.Items[1].Selected;
                editItem.Save();
            }

            RedirectToSiteExplorer();
        }

        protected void OptionUseNamedFileMethod_CheckedChanged(object sender, EventArgs e)
        {
            if (OptionUseNamedFileMethod.Checked)
            {
                panelNamedMethod.Visible = true;
                panelUploadMethod.Visible = false;
            }
        }

        protected void OptionUseUploadFileMethod_CheckedChanged(object sender, EventArgs e)
        {
            if (OptionUseUploadFileMethod.Checked)
            {
                panelNamedMethod.Visible = false;
                panelUploadMethod.Visible = true;

                if (editItem != null)
                {
                    Filename.Text = "";
                    FilenameExtension.Text = "";
                }
            }
        }
    }
}