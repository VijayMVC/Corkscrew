using System;
using System.Web;
using System.Web.Security;

namespace Corkscrew.ControlCenter
{
    public partial class Logout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpCookie formsCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (formsCookie != null)
            {
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(formsCookie.Value);
                if (ticket != null)
                {
                    FormsAuthentication.SignOut();
                }

                Response.Cookies.Remove(FormsAuthentication.FormsCookieName);

                formsCookie = new HttpCookie(FormsAuthentication.FormsCookieName, "")
                {
                    Expires = DateTime.Now.AddYears(-1)
                };

                Response.Cookies.Add(formsCookie);

                Session.Abandon();
            }
        }
    }
}