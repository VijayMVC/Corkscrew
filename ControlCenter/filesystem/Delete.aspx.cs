using Corkscrew.SDK.objects;
using Corkscrew.SDK.tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Corkscrew.ControlCenter.filesystem
{
    public partial class Delete : System.Web.UI.Page
    {

        private Guid deleteSiteId = Guid.Empty;
        private Guid deleteItemId = Guid.Empty;
        private CSSite deleteSite = null;
        private CSFileSystemEntry deleteItem = null;


        public string ItemType
        {
            get;
            private set;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if ((string.IsNullOrEmpty(Request.QueryString["SiteId"])) || (string.IsNullOrEmpty(Request.QueryString["ItemId"])))
            {
                // we dont have a SiteId to redirect to
                Response.Redirect("/sites/All.aspx");
            }

            deleteSiteId = Utility.SafeConvertToGuid(Request.QueryString["SiteId"]);
            deleteSite = CSFarm.Open(WebHelpers.GetSessionUser(HttpContext.Current)).AllSites.Find(deleteSiteId);
            if (deleteSite == null)
            {
                // we dont have a valid SiteId to redirect to
                Response.Redirect("/sites/All.aspx");
            }

            deleteItemId = Utility.SafeConvertToGuid(Request.QueryString["ItemId"]);
            if (deleteItemId == Guid.Empty)
            {
                RedirectToSiteExplorer();
            }

            deleteItem = deleteSite.GetFileSystemItem(deleteItemId);
            if (deleteItem == null)
            {
                RedirectToSiteExplorer();
            }

            ItemType = (deleteItem.IsFolder ? "Directory" : "File");

            if (! IsPostBack)
            {
                ItemGuid.Text = deleteItemId.ToString("d");
                ItemNameWithExtension.Text = deleteItem.FilenameWithExtension;
                CorkscrewUri.Text = deleteItem.FullPath;
            }
        }

        protected void DeleteButton_Click(object sender, EventArgs e)
        {
            deleteItem.Delete();
            RedirectToSiteExplorer();
        }

        protected void CancelButton_Click(object sender, EventArgs e)
        {
            RedirectToSiteExplorer();
        }


        private void RedirectToSiteExplorer()
        {
            Response.Redirect(string.Format("/filesystem/Explorer.aspx?SiteId={0}", deleteSiteId.ToString("d")));
        }

    }
}