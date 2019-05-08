using Corkscrew.SDK.objects;
using Corkscrew.SDK.tools;
using System;
using System.Web;

namespace Corkscrew.ControlCenter.filesystem
{
    public partial class AddEditDirectory : System.Web.UI.Page
    {

        private Guid siteId = Guid.Empty;
        private CSSite editSite = null;
        private string parentPath = null;
        private CSFileSystemEntryDirectory parentDirectory = null;
        private CSFileSystemEntryDirectory editItem = null;

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

            if (! string.IsNullOrEmpty(Request.QueryString["ItemId"]))
            {
                Guid editItemId = Utility.SafeConvertToGuid(Request.QueryString["ItemId"]);
                editItem = editSite.GetDirectory(editItemId);
                if (editItem == null)
                {
                    RedirectToSiteExplorer();
                }

                if (! IsPostBack)
                {
                    Filename.Text = editItem.Filename;
                    Attributes.Items[0].Selected = editItem.IsReadonly;
                    Attributes.Items[1].Selected = editItem.IsHidden;
                    CorkscrewUri.Text = editItem.FullPath;
                }
            }
        }

        protected void CreateButton_Click(object sender, EventArgs e)
        {
            if (editItem != null)
            {
                if (editItem.Filename != Filename.Text)
                {
                    editItem.Rename(Filename.Text, null);
                }
            }
            else
            {
                editItem = parentDirectory.CreateDirectory(Filename.Text);
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

        protected void CancelButton_Click(object sender, EventArgs e)
        {
            RedirectToSiteExplorer();
        }

        private void RedirectToSiteExplorer()
        {
            Response.Redirect(string.Format("/filesystem/Explorer.aspx?SiteId={0}&Path={1}", siteId.ToString("d"), Server.UrlEncode(parentPath)));
        }
    }
}