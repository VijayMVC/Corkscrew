using Corkscrew.SDK.objects;
using System;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace Corkscrew.ControlCenter.usergroups
{
    public partial class All : System.Web.UI.Page
    {
        private CSFarm farm = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            farm = CSFarm.Open(WebHelpers.GetSessionUser(HttpContext.Current));

            if (lvDataView != null)
            {
                lvDataView.ItemCommand += lvDataView_ItemCommand;

                if (! IsPostBack)
                {
                    lvDataView.DataSource = farm.AllUserGroups.OrderBy(g => g.LongformDisplayName);
                    lvDataView.DataBind();
                }
            }
        }

        private void lvDataView_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            string url = string.Empty;

            switch (e.CommandName)
            {
                case "EditGroup":
                    url = string.Format("/usergroups/AddEdit.aspx?id={0}", e.CommandArgument);
                    break;

                case "DeleteGroup":
                    url = string.Format("/usergroups/Delete.aspx?id={0}", e.CommandArgument);
                    break;

                case "SiteACL":
                    url = string.Format("/usergroups/SiteACL.aspx?id={0}", e.CommandArgument);
                    break;

                case "ManageMembership":
                    url = string.Format("/usergroups/AddMember.aspx?id={0}", e.CommandArgument);
                    break;

            }

            if (! string.IsNullOrEmpty(url))
            {
                Response.Redirect(url);
            }
        }
    }
}