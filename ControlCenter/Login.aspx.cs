using Corkscrew.SDK.security;
using Corkscrew.SDK.tools;
using System;
using System.Web;
using System.Web.Security;

namespace Corkscrew.ControlCenter
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (CSPermission.GetAdministrators().Count == 0)
                {
                    // administrators don't exist
                    Response.Redirect("/InitialSetupWizard/CreateFirstUser.aspx");
                }

                HttpCookie formsCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
                if (formsCookie != null)
                {
                    FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(formsCookie.Value);

                    Guid userId = Utility.SafeConvertToGuid(ticket.UserData);
                    if (userId != Guid.Empty)
                    {
                        CSUser user = CSUser.GetById(userId);
                        if (user != null)
                        {
                            FormsAuthentication.RenewTicketIfOld(ticket);
                            string url = FormsAuthentication.GetRedirectUrl(user.Username, true);
                            Response.Redirect(url);
                        }
                    }
                }
            }
        }

        protected void LoginButton_Click(object sender, EventArgs e)
        {
            LoginErrorMessage.Text = "";

            if (LoginPassword.Text == string.Empty)
            {
                LoginErrorMessage.Text = "Please enter your login password.";
                return;
            }

            string userName = Utility.SafeString(LoginUsername.Text);
            string passwordHash = Utility.GetSha256Hash(LoginPassword.Text);
            CSUser user = CSUser.Login(userName, passwordHash);
            if (user == null)
            {
                LoginErrorMessage.Text = "The username or password you entered is not correct. Please try again.";
                return;
            }

            DateTime issued = DateTime.Now,
                     expiry = issued.AddSeconds(86400); // 1 day
            bool persistent = LoginRememberMe.Checked;

            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket
            (
                1,                              // ver
                userName,
                issued,                         // issued
                expiry,                         // expiry
                persistent,
                user.Id.ToString("N")           // userdata
            );

            string encryptedTicket = FormsAuthentication.Encrypt(ticket);

            HttpCookie authenticationCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            if (persistent)
            {
                authenticationCookie.Expires = expiry;
            }
            Response.AppendCookie(authenticationCookie);

            string url = FormsAuthentication.GetRedirectUrl(userName, persistent);
            Response.Redirect(url);
        }
    }
}