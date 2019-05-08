using Corkscrew.SDK.objects;
using Corkscrew.SDK.tools;
using System;
using System.Web;

namespace Corkscrew.ControlCenter.sites
{
    public partial class Delete : System.Web.UI.Page
    {
        private Guid deleteSiteId = Guid.Empty;
        private CSSite deleteSite = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Request.QueryString["id"]))
            {
                Response.Redirect("/sites/All.aspx");
            }

            deleteSiteId = Utility.SafeConvertToGuid(Request.QueryString["id"]);
            if (deleteSiteId == Guid.Empty)
            {
                Response.Redirect("/sites/All.aspx");
            }

            deleteSite = CSFarm.Open(WebHelpers.GetSessionUser(HttpContext.Current)).AllSites.Find(deleteSiteId);
            if (deleteSite == null)
            {
                Response.Redirect("/sites/All.aspx");
            }

            if (!IsPostBack)
            {
                SiteGuid.Text = deleteSite.Id.ToString("d");
                SiteName.Text = deleteSite.Name;
                SiteDescription.Text = deleteSite.Description;
            }
        }

        protected void CancelButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("/sites/All.aspx");
        }

        protected void DeleteButton_Click(object sender, EventArgs e)
        {

            deleteSite.Farm.AllSites.Remove(deleteSite);

            Response.Redirect("/sites/All.aspx");
        }
    }
}