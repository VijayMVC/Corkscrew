using System;
using System.Security.Principal;

namespace Corkscrew.SDK.security
{

    /// <summary>
    /// Implements the IPrincipal interface over the CSUser security object
    /// This class is not used anywhere other than in the web-authentication module!
    /// </summary>
    public class CSWebPrincipal : IPrincipal
    {

        /// <summary>
        /// Implements IIdentity property for CSSecurityPrincipal via CSWebIdentity.
        /// Contains the underlying CSUser object
        /// </summary>
        public IIdentity Identity { get; private set; }

        /// <summary>
        /// Wrapper around Identity property, exposes as CSWebIdentity.
        /// </summary>
        public CSWebIdentity CSIdentity
        {
            get { return Identity as CSWebIdentity; }
            set { Identity = value; }
        }

        /// <summary>
        /// The underlying user
        /// </summary>
        public CSUser User
        {
            get
            {
                if (Identity.GetType() != typeof(CSWebIdentity))
                {
                    return null;
                }

                return ((CSWebIdentity)Identity).User;
            }
        }

        /// <summary>
        /// Returns the Guid of the CSUser contained in the Identity.
        /// </summary>
        public Guid Id
        {
            get
            {
                if (Identity.GetType() != typeof(CSWebIdentity))
                {
                    return Guid.Empty;
                }

                return ((CSWebIdentity)Identity).User.Id;
            }
        }

        /// <summary>
        /// Returns if Identity contains a valid CSUser
        /// </summary>
        public bool IsValid
        {
            get { return (User != null); }
        }

        /// <summary>
        /// Returns if this principal is a member of the given role (group). 
        /// Will always be false as Corkscrew does not use groups.
        /// </summary>
        /// <param name="role">Name of the group or role to check</param>
        /// <returns>False.</returns>
        public bool IsInRole(string role)
        {
            // we have not implemented user groups, 
            // so user cannot belong to any group
            return false;
        }

        /// <summary>
        /// Constructor. 
        /// </summary>
        /// <param name="user">CSUser to populate</param>
        /// <exception cref="ArgumentNullException">If user is NULL</exception>
        public CSWebPrincipal(CSUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            Identity = new CSWebIdentity(user);
        }


        /// <summary>
        /// Creates an instance of CSSecurityPrincipal by populating the Identity 
        /// using the CSUser retrieved from the given Guid.
        /// A valid instance of CSSecurityPrincipal will be returned even if user is not found. 
        /// Caller must check IsValid property to ensure a valid user was set.
        /// </summary>
        /// <param name="id">Guid of the user principal to populate</param>
        /// <returns>CSSecurityPrincipal</returns>
        public static CSWebPrincipal GetByUserId(Guid id)
        {
            return new CSWebPrincipal(CSUser.GetById(id));
        }

    }
}
