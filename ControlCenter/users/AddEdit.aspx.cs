using Corkscrew.SDK.security;
using Corkscrew.SDK.tools;
using System;

namespace Corkscrew.ControlCenter.users
{
    public partial class AddEdit : System.Web.UI.Page
    {

        private Guid editUserId = Guid.Empty;
        private CSUser editUser = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (! string.IsNullOrEmpty(Request.QueryString["id"]))
            {
                editUserId = Utility.SafeConvertToGuid(Request.QueryString["id"]);
                if (editUserId != Guid.Empty)
                {
                    editUser = CSUser.GetById(editUserId);
                    if (editUser == null)
                    {
                        Response.Redirect("/users/All.aspx");
                    }

                    if (!IsPostBack)
                    {
                        UsersUsername.Text = editUser.Username;
                        UsersDisplayname.Text = editUser.DisplayName;
                        UsersEmailAddress.Text = editUser.EmailAddress;

                        UsersUsername.ReadOnly = true;
                        UsersDisplayname.Focus();
                    }
                }
            }

            if (string.IsNullOrEmpty(UsersUsername.Text))
            {
                UsersUsername.Focus();
            }
        }

        protected void AddEditButton_Click(object sender, EventArgs e)
        {
            // check if another user by same name exists
            if (editUser == null)
            {
                if (!CSUser.UserExists(UsersUsername.Text))
                {
                    CSUser.CreateUser
                    (
                        UsersUsername.Text,
                        UsersDisplayname.Text,
                        UsersPassword.Text,
                        UsersEmailAddress.Text
                    );
                }
                else
                {
                    rfvUsersUserName.IsValid = false;
                    return;
                }
            }
            else
            {
                editUser.DisplayName = UsersDisplayname.Text;
                editUser.EmailAddress = UsersEmailAddress.Text;
                editUser.Save();

                if (! string.IsNullOrEmpty(UsersPassword.Text))
                {
                    if (! editUser.IsCurrentPassword(UsersPassword.Text))
                    {
                        editUser.ChangePassword(UsersPassword.Text);
                    }
                }
            }

            Response.Redirect("/users/All.aspx");
        }
    }
}