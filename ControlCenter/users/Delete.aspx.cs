using Corkscrew.SDK.security;
using Corkscrew.SDK.tools;
using System;

namespace Corkscrew.ControlCenter.users
{
    public partial class Delete : System.Web.UI.Page
    {

        private Guid deleteUserId = Guid.Empty;
        private CSUser deleteUser = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Request.QueryString["id"]))
            {
                Response.Redirect("/users/All.aspx");
            }

            deleteUserId = Utility.SafeConvertToGuid(Request.QueryString["id"]);
            if (deleteUserId == Guid.Empty)
            {
                Response.Redirect("/users/All.aspx");
            }

            deleteUser = CSUser.GetById(deleteUserId);
            if (deleteUser == null)
            {
                Response.Redirect("/users/All.aspx");
            }

            if (!IsPostBack)
            {
                UsersGuid.Text = deleteUser.Id.ToString("d");
                UsersUsername.Text = deleteUser.Username;
                UsersDisplayname.Text = deleteUser.DisplayName;
                UsersEmailAddress.Text = deleteUser.EmailAddress;
            }
        }

        protected void DeleteButton_Click(object sender, EventArgs e)
        {
            deleteUser.Delete();
            Response.Redirect("/users/All.aspx");
        }

        protected void CancelButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("/users/All.aspx");
        }
    }
}