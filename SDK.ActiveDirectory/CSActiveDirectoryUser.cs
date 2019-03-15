using Corkscrew.SDK.ActiveDirectory.odm;
using Corkscrew.SDK.security;
using Corkscrew.SDK.tools;
using System;
using System.Collections.Generic;

namespace Corkscrew.SDK.ActiveDirectory
{

    /// <summary>
    /// Represents a user account that exists in the Windows Active Directory. Such an account will not have a password set 
    /// and requires the authentication to happen in the Windows OS layer. Corscrew will only perform the Authorization for 
    /// such users.
    /// </summary>
    public class CSActiveDirectoryUser : CSUser
    {

        static List<string> RESERVED_USERNAMES =
            new List<string>()
            {
                "CORKSCREW\\Corkscrew System User",
                "CORKSCREW\\Anonymous User"
            };

        private CSActiveDirectoryUser() { }

        /// <summary>
        /// Creates a new user in Corkscrew to match the given Active Directory user
        /// </summary>
        /// <param name="username">Username (login Id)</param>
        /// <param name="displayName">Display name of the user</param>
        /// <param name="emailAddress">Email address (UPN) of the user</param>
        /// <returns>The newly created user object</returns>
        /// <exception cref="ArgumentException">If the username is already in use by an existing user account</exception>
        /// <exception cref="ArgumentNullException">If username, password or display name are null</exception>
        /// <exception cref="ArgumentOutOfRangeException">If the username is one of the reserved usernames</exception>
        public static CSActiveDirectoryUser CreateUser(string username, string displayName, string emailAddress = null)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentNullException("Username cannot be null.");
            }

            if (RESERVED_USERNAMES.ContainsNoCase(username))
            {
                throw new ArgumentOutOfRangeException("Cannot set username, is system reserved.");
            }

            if (string.IsNullOrEmpty(displayName))
            {
                throw new ArgumentNullException("DisplayName cannot be null.");
            }

            // check that the user does not already exist
            if (UserExists(username))
            {
                throw new ArgumentException("Another user with the same username already exists.");
            }

            CSActiveDirectoryUser newUser = new CSActiveDirectoryUser();

            // generate a Guid that does not map to a system user.
            // we dont want to check actual user existance in IsSystemUser 
            // because we dont want a future system user to collide. 
            // so we want to stay off the entire guid range.

            do
            {
                newUser.Id = Guid.NewGuid();
            } while (newUser.Id.Equals(ANONYMOUS_USER_GUID) || newUser.Id.Equals(CORKSCREW_SYSTEM_USER_GUID));

            newUser.DisplayName = displayName;
            newUser.Username = username;
            newUser.EmailAddress = emailAddress;

            (new OdmActiveDirectoryUser()).Save(newUser);

            return newUser;
        }

    }

    /// <summary>
    /// Shadows an Active Directory User Group in Corkscrew. Derives from the CSUserGroup class in the SDK.Security namespace.
    /// </summary>
    public class CSActiveDirectoryUserGroup : CSUserGroup
    {
        private CSActiveDirectoryUserGroup() { }

        /// <summary>
        /// Creates a new user group in Corkscrew to match the Active Directory group
        /// </summary>
        /// <param name="username">Username or alias of the group</param>
        /// <param name="displayName">Display name of the group</param>
        /// <param name="emailAddress">Email address (UPN) of the group</param>
        /// <returns>The newly created user group object</returns>
        public static new CSActiveDirectoryUserGroup CreateUserGroup(string username, string displayName, string emailAddress = null)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentNullException("Username cannot be null");
            }

            if (string.IsNullOrEmpty(displayName))
            {
                throw new ArgumentNullException("Display name cannot be null");
            }

            // after creation, all groups behave the same
            if (CSUserGroup.Exists(username))
            {
                throw new ArgumentException("Another group with the same group name already exists.");
            }

            CSActiveDirectoryUserGroup group = new CSActiveDirectoryUserGroup()
            {
                Id = Guid.NewGuid(),
                Username = username,
                DisplayName = displayName,
                EmailAddress = emailAddress
            };

            return group;
        }
    }

}
