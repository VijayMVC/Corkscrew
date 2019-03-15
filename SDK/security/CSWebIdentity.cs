using System.Security.Principal;

namespace Corkscrew.SDK.security
{

    /// <summary>
    /// IIdentity implementation for web-authentication module. 
    /// This class is not used anywhere other than in the web-authentication module.
    /// </summary>
    public class CSWebIdentity : IIdentity
    {

        /// <summary>
        /// Get type of authentication. Always "Corkscrew".
        /// </summary>
        public string AuthenticationType { get { return "Corkscrew"; } }

        /// <summary>
        /// Get if user is authenticated. Always true because class is created only post-authentication.
        /// </summary>
        public bool IsAuthenticated { get { return true; } }

        /// <summary>
        /// Name of the user (Fullname)
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The CSUser object
        /// </summary>
        public CSUser User { get; private set; } 


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="user">CSUser to build from</param>
        public CSWebIdentity(CSUser user)
        {
            Name = user.DisplayName;
            User = user;
        }

    }
}
