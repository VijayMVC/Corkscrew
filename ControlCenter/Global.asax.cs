using System;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using Corkscrew.SDK.security;
using Corkscrew.SDK.tools;

namespace Corkscrew.ControlCenter
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            string requiresJquery = System.Configuration.ConfigurationManager.AppSettings["ValidationSettings:UnobtrusiveValidationMode"];
            if ((!string.IsNullOrEmpty(requiresJquery)) && (requiresJquery.Equals("WebForms")))
            {
                ScriptManager.ScriptResourceMapping.AddDefinition("jquery", new ScriptResourceDefinition
                {
                    Path = "/external/jquery/jquery-1.10.2.js",
                    DebugPath = "/external/jquery/jquery-1.10.2.js",
                    CdnPath = "//code.jquery.com/jquery-1.10.2.min.js",
                    CdnDebugPath = "//code.jquery.com/jquery-1.10.2.min.js"
                });
            }
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            HttpCookie formsCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (formsCookie != null)
            {
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(formsCookie.Value);
                if (ticket != null)
                {
                    if (ticket.Expired)
                    {
                        ticket = FormsAuthentication.RenewTicketIfOld(ticket);
                    }

                    if (ticket != null)
                    {
                        Guid userId = Utility.SafeConvertToGuid(ticket.UserData);
                        if (userId != Guid.Empty)
                        {
                            CSUser user = CSUser.GetById(userId);
                            if (user != null)
                            {
                                Context.User = new CSWebPrincipal(user);
                            }
                        }
                    }
                }
            }
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();
            //Server.ClearError();

            if (ex != null)
            {
                if (ex.GetType() == typeof(HttpException))
                {
                    HttpException hex = (HttpException)ex;
                    if (hex.Message.StartsWith("Validation of viewstate MAC failed", StringComparison.InvariantCultureIgnoreCase))
                    {
                        // the simplest handler for the ViewState MAC exception is to reload the page afresh.
                        // typical cause is user forces reload of an old cached page that has form controls.
                        Session.Abandon();
                        Response.Redirect(Request.Url.ToString());
                    }
                    else
                    {
                        Response.Redirect("/ErrorPages/ErrorPage.aspx?Error=" + Utility.Base64Encode(hex.Message));
                    }
                }
                else
                {
                    if (ex.GetType() == typeof(HttpUnhandledException))
                    {
                        ex = ex.InnerException;
                        Response.Redirect("/ErrorPages/ErrorPage.aspx?Error=" + Utility.Base64Encode(ex.Message));
                    }
                }
            }
        }

    }
}