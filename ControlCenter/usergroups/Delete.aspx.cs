using Corkscrew.SDK.security;
using Corkscrew.SDK.tools;
using System;

namespace Corkscrew.ControlCenter.usergroups
{
    public partial class Delete : System.Web.UI.Page
    {

        private Guid deleteId = Guid.Empty;
        private CSUserGroup deleteGroup = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Request.QueryString["id"]))
            {
                Response.Redirect("/usergroups/All.aspx");
            }

            deleteId = Utility.SafeConvertToGuid(Request.QueryString["id"]);
            if (deleteId == Guid.Empty)
            {
                Response.Redirect("/usergroups/All.aspx");
            }

            deleteGroup = CSUserGroup.GetById(deleteId);
            if (deleteGroup == null)
            {
                Response.Redirect("/usergroups/All.aspx");
            }

            if (!IsPostBack)
            {
                UsersGuid.Text = deleteGroup.Id.ToString("d");
                UsersUsername.Text = deleteGroup.Username;
                UsersDisplayname.Text = deleteGroup.DisplayName;
                UsersEmailAddress.Text = deleteGroup.EmailAddress;
            }
        }

        protected void CancelButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("/usergroups/All.aspx");
        }

        protected void DeleteButton_Click(object sender, EventArgs e)
        {
            deleteGroup.Delete();
            Response.Redirect("/usergroups/All.aspx");
        }
    }
}