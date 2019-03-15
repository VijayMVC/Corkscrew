using Corkscrew.SDK.objects;
using Corkscrew.SDK.odm;
using System;
using System.Collections.Generic;

namespace Corkscrew.SDK.security
{
    /// <summary>
    /// Collection of CSUser. Allows you to manage CSUser objects 
    /// transparently.
    /// </summary>
    public class CSUserCollection : CSBaseCollection<CSUser>
    {

        #region Constructors

        /// <summary>
        /// Creates collection of CSUser
        /// </summary>
        /// <param name="isReadonly">If set, creates a readonly collection</param>
        internal CSUserCollection(bool isReadonly = false) : base(isReadonly) { }

        /// <summary>
        /// Creates collection of CSUser from an enumeration
        /// </summary>
        /// <param name="users">Enumeration of CSUsers</param>
        /// <param name="isReadonly">If set, creates a readonly collection</param>
        public CSUserCollection(List<CSUser> users, bool isReadonly = false) : base(users, isReadonly) { }

        /// <summary>
        /// Gets a list of all Farm administrators
        /// </summary>
        /// <returns>Readonly collection of farm administrators</returns>
        public static CSUserCollection GetFarmAdministrators()
        {
            List<CSUser> adminsList = new List<CSUser>();
            CSPermissionCollection adminAcls = CSPermission.GetAdministrators();        // get only farm admins
            foreach (CSPermission acl in adminAcls)
            {
                if (acl.IsFarmAdministrator)
                {
                    CSUser admin = (CSUser)acl.SecurityPrincipal;   // GetAdministrators() will only return users
                    foreach (CSUser user in adminsList)
                    {
                        if (user.Id.Equals(admin.Id))
                        {
                            admin = null;
                            break;
                        }
                    }

                    adminsList.Add(admin);
                }
            }

            return new CSUserCollection(adminsList, true);
        }

        /// <summary>
        /// Gets a list of all administrators of the given Site
        /// </summary>
        /// <param name="site">The site to get administrators for</param>
        /// <returns>Readonly collection of farm administrators</returns>
        public static CSUserCollection GetSiteAdministrators(CSSite site)
        {
            List<CSUser> adminsList = new List<CSUser>();
            CSPermissionCollection adminAcls = CSPermission.GetAdministrators(site);
            foreach (CSPermission acl in adminAcls)
            {
                if (acl.IsFarmAdministrator || acl.IsSiteAdministrator)
                {
                    CSUser admin = (CSUser)acl.SecurityPrincipal;   // GetAdministrators will only return CSUsers
                    foreach (CSUser user in adminsList)
                    {
                        if (user.Id.Equals(admin.Id))
                        {
                            admin = null;
                            break;
                        }
                    }

                    adminsList.Add(admin);
                }
            }

            return new CSUserCollection(adminsList, true);
        }

        /// <summary>
        /// Gets a list of all users in the given farm. 
        /// If the user is not a Farm admin, will only return the currently authenticated user.
        /// </summary>
        /// <param name="farm">Farm to get users for</param>
        /// <returns>Readonly collection of users in the farm</returns>
        public static CSUserCollection GetAllUsers(CSFarm farm)
        {
            if (! farm.IsAuthenticatedUserFarmAdministrator)
            {
                return new CSUserCollection(new List<CSUser>() { farm.AuthenticatedUser }, true);
            }
            else
            {
                return new CSUserCollection((new OdmUsers()).GetUsersAll(), false);
            }
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Add a CSUser to the collection. User is not "added" to the backend.
        /// </summary>
        /// <param name="user">CSUser to add</param>
        /// <returns>Returns the newly created user</returns>
        public CSUser Add(CSUser user)
        {
            base.AddInternal(user);
            return user;
        }

        /// <summary>
        /// Adds a new user in the Corkscrew system (adds it to the backend) and to the internal collection
        /// </summary>
        /// <param name="username">Username for user. Cannot be a reserved system name</param>
        /// <param name="displayName">Display or friendly name</param>
        /// <param name="password">Plain-text Password (cannot be empty or null). We will always encrypt this, so do not set an already encrypted password here.</param>
        /// <param name="emailAddress">Email address for the user</param>
        /// <returns>The newly created user</returns>
        /// <exception cref="ArgumentException">If the username is already in use by an existing user account</exception>
        /// <exception cref="ArgumentNullException">If username, password or display name are null</exception>
        /// <exception cref="ArgumentOutOfRangeException">If the username is one of the reserved usernames</exception>
        public CSUser Add(string username, string displayName, string password, string emailAddress = null)
        {
            return Add(CSUser.CreateUser(username, displayName, password, emailAddress));
        }


        /// <summary>
        /// Deletes the user from the backend and removes from collection
        /// </summary>
        /// <param name="user">CSUser to remove</param>
        /// <exception cref="Exception">If user could not be deleted from the backend</exception>
        public void Remove(CSUser user)
        {
            if (! user.Delete())
            {
                throw new Exception("Could not delete the user");
            }

            base.RemoveInternal(user);
        }

        /// <summary>
        /// Deletes the user from the backend and removes from collection
        /// </summary>
        /// <param name="id">Guid of the CSUser to remove</param>
        /// <exception cref="Exception">If the user could not be found</exception>
        public void Remove(Guid id)
        {
            if (! id.Equals(Guid.Empty))
            {
                CSUser user = Find(id);
                if (user == null)
                {
                    throw new Exception("Could not find user with given Id.");
                }

                Remove(user);
            }
        }

        /// <summary>
        /// Find the CSUser with the given Id
        /// </summary>
        /// <param name="id">Guid of the user to find in the collection</param>
        /// <returns>CSUser instance matching Guid. NULL if not found.</returns>
        public CSUser Find(Guid id)
        {
            CSUser u = null;

            lock (base.SyncRoot)
            {
                foreach(CSUser user in Collection)
                {
                    if (user.Id.Equals(id))
                    {
                        u = user;
                        break;
                    }
                }
            }

            return u;
        }

        /// <summary>
        /// Find the CSUser with the given Id
        /// </summary>
        /// <param name="userName">Username of the user to find in the collection</param>
        /// <returns>CSUser instance matching Guid. NULL if not found.</returns>
        public CSUser Find(string userName)
        {
            CSUser u = null;

            lock (base.SyncRoot)
            {
                foreach (CSUser user in Collection)
                {
                    if (user.Username.Equals(userName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        u = user;
                        break;
                    }
                }
            }

            return u;
        }

        #endregion

    }
}
