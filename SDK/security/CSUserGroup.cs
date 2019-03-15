using Corkscrew.SDK.objects;
using Corkscrew.SDK.odm;
using System;
using System.Collections.Generic;

namespace Corkscrew.SDK.security
{

    /// <summary>
    /// This class represents a group of users in the Corkscrew System
    /// </summary>
    public class CSUserGroup : CSSecurityPrincipal
    {

        #region Properties

        /// <summary>
        /// Members of the group. Elements are of type CSUser. Collection is readonly.
        /// </summary>
        public CSUserCollection Members
        {
            get
            {
                if (_members == null)
                {
                    _members = new CSUserCollection
                    (
                        (new OdmUsers()).GetGroupMemberships(this),
                        true                                            // collection is always readonly
                    );
                }

                return _members;
            }
        }
        private CSUserCollection _members = null;

        /// <summary>
        /// Cheat flag to indicate this is an AD-backed user group
        /// </summary>
        internal bool IsWindowsActiveDirectoryGroup = false;

        #endregion

        #region Constructors

        /// <summary>
        /// Blank constructor used internally
        /// </summary>
        protected CSUserGroup() : base() { IsGroup = true; }

        /// <summary>
        /// Rehydration constructor
        /// </summary>
        /// <param name="id">Guid of the user</param>
        /// <param name="username">Username for user.</param>
        /// <param name="displayName">Display or friendly name</param>
        /// <param name="emailAddress">Email address for the user</param>
        internal CSUserGroup(Guid id, string username, string displayName, string emailAddress = null)
            : base(id, username, displayName, emailAddress)
        {
            IsGroup = true;
        }

        /// <summary>
        /// Creates a new user group
        /// </summary>
        /// <param name="username">Username or alias of the group</param>
        /// <param name="displayName">Display name for the group</param>
        /// <param name="emailAddress">Email address for the group</param>
        /// <returns>The newly created usergroup</returns>
        public static CSUserGroup CreateUserGroup(string username, string displayName, string emailAddress = null)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentNullException("Username cannot be null");
            }

            if (string.IsNullOrEmpty(displayName))
            {
                throw new ArgumentNullException("Display name cannot be null");
            }

            OdmUsers odm = new OdmUsers();
            if (odm.GetGroupByName(username) != null)
            {
                throw new ArgumentException("Another group with the same group name already exists.");
            }

            CSUserGroup group = new CSUserGroup()
            {
                Id = Guid.NewGuid(),
                Username = username,
                DisplayName = displayName,
                EmailAddress = emailAddress
            };

            odm.Save(group);

            return group;
        }

        /// <summary>
        /// Fetches the user group with the given name
        /// </summary>
        /// <param name="name">Name of the user group to fetch</param>
        /// <returns>The usergroup or NULL</returns>
        public static CSUserGroup GetByName(string name)
        {
            return (new OdmUsers()).GetGroupByName(name);
        }

        /// <summary>
        /// Fetches the user group with the given Guid
        /// </summary>
        /// <param name="id">Guid of the user group to fetch</param>
        /// <returns>The usergroup or NULL</returns>
        public static CSUserGroup GetById(Guid id)
        {
            return (new OdmUsers()).GetGroupById(id);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Add the user to this user group. The members collection is updated if the user is successfully added.
        /// </summary>
        /// <param name="user">User to add to the group</param>
        /// <returns>True if added successfully</returns>
        public bool Add(CSUser user)
        {
            if ((new OdmUsers()).Add(user, this))
            {
                Members.Add(user);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Remove the user from this user group. The members collection is updated if the user is successfully removed.
        /// </summary>
        /// <param name="user">User to remove from the group</param>
        /// <returns>True if removed successfully</returns>
        public bool Remove(CSUser user)
        {
            if ((new OdmUsers()).Delete(user, this))
            {
                Members.Remove(user);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Delete the current user group
        /// </summary>
        /// <returns>True if deleted successfully</returns>
        public bool Delete()
        {
            return (new OdmUsers()).Delete(this);
        }

        /// <summary>
        /// Saves the current user group
        /// </summary>
        public void Save()
        {
            (new OdmUsers()).Save(this);
        }

        /// <summary>
        /// Returns if a usergroup by the given name exists
        /// </summary>
        /// <param name="name">Name of the user group to check for</param>
        /// <returns>True if group exists</returns>
        public static bool Exists(string name)
        {
            return (GetByName(name) != null);
        }

        /// <summary>
        /// Get users who are not members of this group
        /// </summary>
        /// <returns>CSUserCollection of users not in this group</returns>
        public CSUserCollection GetNonMembers()
        {
            return new CSUserCollection((new OdmUsers()).GetUsersNotInGroup(this), true);
        }

        #endregion

    }

    /// <summary>
    /// Collection of CSUserGroup. Allows you to manage CSUserGroup objects transparently.
    /// </summary>
    public class CSUserGroupCollection : CSBaseCollection<CSUserGroup>
    {

        #region Constructors

        /// <summary>
        /// Creates collection of CSUserGroup
        /// </summary>
        /// <param name="isReadonly">If set, creates a readonly collection</param>
        internal CSUserGroupCollection(bool isReadonly = false) : base(isReadonly) { }

        /// <summary>
        /// Creates collection of CSUserGroup from an enumeration
        /// </summary>
        /// <param name="list">Enumeration of user groups</param>
        /// <param name="isReadonly">If set, creates a readonly collection</param>
        public CSUserGroupCollection(IEnumerable<CSUserGroup> list, bool isReadonly = false) : base(list, isReadonly) { }

        /// <summary>
        /// Gets a list of all user groups in the farm. If the user is not a farm admin, collection will be readonly.
        /// </summary>
        /// <param name="farm">The farm to get users for</param>
        /// <returns>Collection of user groups</returns>
        public static CSUserGroupCollection GetAllUserGroups(CSFarm farm)
        {
            return new CSUserGroupCollection((new OdmUsers()).GetUserGroupsAll(), (!farm.IsAuthenticatedUserFarmAdministrator));
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Add the given group to the collection. Group is not saved to the backend.
        /// </summary>
        /// <param name="group">Group to add</param>
        /// <returns>Returns the added group</returns>
        public CSUserGroup Add(CSUserGroup group)
        {
            base.AddInternal(group);
            return group;
        }

        /// <summary>
        /// Creates a new user group
        /// </summary>
        /// <param name="username">Username or alias of the group</param>
        /// <param name="displayName">Display name for the group</param>
        /// <param name="emailAddress">Email address for the group</param>
        /// <returns>The newly created usergroup</returns>
        public CSUserGroup Add(string username, string displayName, string emailAddress = null)
        {
            return Add(CSUserGroup.CreateUserGroup(username, displayName, emailAddress));
        }

        /// <summary>
        /// Deletes the user group from the backend and the collection
        /// </summary>
        /// <param name="group">Group to delete</param>
        public void Remove(CSUserGroup group)
        {
            if (! group.Delete())
            {
                throw new Exception("Could not delete user group");
            }

            base.RemoveInternal(group);
        }

        /// <summary>
        /// Deletes the user group from the backend and the collection
        /// </summary>
        /// <param name="id">Guid of the group to delete</param>
        public void Remove(Guid id)
        {
            if (! id.Equals(Guid.Empty))
            {
                CSUserGroup group = Find(id);
                if (group == null)
                {
                    throw new Exception("Could not find user group with the given Id.");
                }

                Remove(group);
            }
        }

        /// <summary>
        /// Find the CSUserGroup with the given Id
        /// </summary>
        /// <param name="id">Guid of the user group to find in the collection</param>
        /// <returns>CSUserGroup instance matching the Guid. NULL if not found.</returns>
        public CSUserGroup Find(Guid id)
        {
            CSUserGroup g = null;

            lock(base.SyncRoot)
            {
                foreach(CSUserGroup group in Collection)
                {
                    if (group.Id.Equals(id))
                    {
                        g = group;
                        break;
                    }
                }
            }

            return g;
        }

        /// <summary>
        /// Find the CSUserGroup with the given username
        /// </summary>
        /// <param name="username">Username of the user group to find in the collection</param>
        /// <returns>CSUserGroup instance matching the username. NULL if not found.</returns>
        public CSUserGroup Find(string username)
        {
            CSUserGroup g = null;

            lock (base.SyncRoot)
            {
                foreach (CSUserGroup group in Collection)
                {
                    if (group.Username.Equals(username, StringComparison.InvariantCultureIgnoreCase))
                    {
                        g = group;
                        break;
                    }
                }
            }

            return g;
        }

        #endregion
    }


}
