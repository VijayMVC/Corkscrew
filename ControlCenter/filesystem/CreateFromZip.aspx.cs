using Corkscrew.SDK.objects;
using Corkscrew.SDK.tools;
using System;
using System.Collections.Generic;
using System.Web;

namespace Corkscrew.ControlCenter.filesystem
{
    public partial class CreateFromZip : System.Web.UI.Page
    {
        private Guid siteId = Guid.Empty;
        private CSSite editSite = null;
        private string parentPath = null;
        private CSFileSystemEntryDirectory parentDirectory = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Request.QueryString["SiteId"]) || string.IsNullOrEmpty(Request.QueryString["Path"]))
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
        }

        protected void CancelButton_Click(object sender, EventArgs e)
        {
            RedirectToSiteExplorer();
        }

        protected void UploadAndCreateButton_Click(object sender, EventArgs e)
        {
            if ((! ZipFileUpload.HasFile) || (ZipFileUpload.FileBytes.Length < 1))
            {
                ErrorMessage.Text = "No file was selected for upload.";
                return;
            }

            List<CSFileSystemEntry> createdItems = CSZipFiles.ExtractArchive(ZipFileUpload.FileContent, parentDirectory);

            ErrorMessage.Text = "The following items were created or updated: <br />";
            createdItems.ForEach(i => ErrorMessage.Text += string.Format("{0}<br />", Server.HtmlEncode(i.FullPath)));

            BackToFSLink.NavigateUrl = string.Format("/filesystem/Explorer.aspx?SiteId={0}&Path={1}", siteId.ToString("d"), Server.UrlEncode(parentPath));
            BackToFSLink.Visible = true;
        }

        private void RedirectToSiteExplorer()
        {
            Response.Redirect(string.Format("/filesystem/Explorer.aspx?SiteId={0}&Path={1}", siteId.ToString("d"), Server.UrlEncode(parentPath)));
        }
    }
}