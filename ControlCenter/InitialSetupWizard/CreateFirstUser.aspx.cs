using Corkscrew.SDK.security;
using Corkscrew.SDK.tools;
using System;
using System.Web.UI.WebControls;

namespace Corkscrew.ControlCenter.InitialSetupWizard
{
    public partial class CreateFirstUser : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // this should only be usable if there are no administrators available
                CSPermissionCollection admins = CSPermission.GetAdministrators();
                if ((admins != null) && (admins.Count > 0))
                {
                    // we have admins already, a farm admin should use the /users/allusers.aspx page
                    Response.Redirect("/Login.aspx");
                }

                UsersPassword.Attributes.Add("Value", "");
            }
        }

        protected void UsersAddNewUserButton_Click(object sender, EventArgs e)
        {
            string username = Utility.SafeString(UsersUsername.Text, string.Empty, string.Empty, string.Empty, "\\", "\\");
            if (username.IndexOf("\\") < 0)
            {
                username = string.Format("CORKSCREW\\{0}", username);
            }

            // check if another user by same name exists
            if (!CSUser.UserExists(username))
            {
                CSUser user = CSUser.CreateUser
                (
                    username,
                    UsersDisplayname.Text,
                    UsersPassword.Text, 
                    UsersEmailAddress.Text
                );

                // Set user as global admin
                CSPermission acl = CSPermission.TestAccess(null, null, user);
                acl.CanFullControl = true;
                acl.Save();

                Response.Redirect("/Login.aspx");
            }
            else
            {
                rfvUsersUserName.IsValid = false;
            }
        }
    }
}