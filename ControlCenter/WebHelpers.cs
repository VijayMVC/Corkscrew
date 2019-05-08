using Corkscrew.SDK.security;
using System;
using System.Web;

namespace Corkscrew.ControlCenter
{
    public static class WebHelpers
    {

        public static CSUser GetSessionUser(HttpContext context)
        {
            if (context != null)
            {
                if ((context.User != null) && (context.User is CSWebPrincipal))
                {
                    CSWebPrincipal principal = (CSWebPrincipal)context.User;
                    if (principal != null)
                    {
                        return principal.User;
                    }
                }
            }

            return null;
        }

        public static bool IsUserFarmAdmin(HttpContext context)
        {
            CSUser user = GetSessionUser(context);
            if (user != null)
            {
                return CSPermission.TestAccess(null, null, user).IsFarmAdministrator;
            }

            return false;
        }

        public static void ThrowAppException(string message)
        {
            throw new ApplicationException(message);
        }

    }
}