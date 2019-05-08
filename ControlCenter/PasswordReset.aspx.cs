using Corkscrew.SDK.security;
using Corkscrew.SDK.tools;
using System;

namespace Corkscrew.ControlCenter
{
    public partial class PasswordReset : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void ChangePasswordButton_Click(object sender, EventArgs e)
        {
            LoginErrorMessage.Text = "";
            string userName = Utility.SafeString(LoginUsername.Text);
            string passwordHash = Utility.GetSha256Hash(CurrentPassword.Text);
            CSUser user = CSUser.Login(userName, passwordHash);
            if (user == null)
            {
                LoginErrorMessage.Text = "The username or password you entered is not correct. Please try again.";
                return;
            }

            if (user.IsCurrentPassword(NewPassword.Text))
            {
                LoginErrorMessage.Text = "New password cannot be the same as the current password. Please try again.";
                return;
            }

            user.ChangePassword(NewPassword.Text);
            LoginErrorMessage.Text = "The password has been changed successfully. Please terminate all applications running with the old password and login again with the new password.";
        }
    }
}