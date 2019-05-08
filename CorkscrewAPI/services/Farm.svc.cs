using Corkscrew.API.business;
using Corkscrew.API.datacontracts;
using Corkscrew.SDK.objects;
using Corkscrew.SDK.security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

namespace Corkscrew.API.services
{
    /// <summary>
    /// This service allows interaction with the Corkscrew system at the Farm level.
    /// </summary>
    public class Farm : IFarm
    {

        private CSFarm GetFarm(string token)
        {
            CSUser user = Tools.GetCurrentlyAuthenticatedUser(token);
            if (user == null)
            {
                throw new FaultException("Token is not valid.");
            }

            CSFarm farm = CSFarm.Open(user);
            if (farm == null)
            {
                throw new FaultException("Farm could not be opened.");
            }

            return farm;
        }

        /// <summary>
        /// Returns all the Administrator users in the farm
        /// </summary>
        /// <param name="token">The API token</param>
        /// <returns>Array of CSPrincipalDataContract objects</returns>
        public CSPrincipalDataContract[] GetAllAdministrators(string token)
        {
            List<CSPrincipalDataContract> principals = new List<CSPrincipalDataContract>();
            foreach(CSUser user in GetFarm(token).AllAdministrators)
            {
                principals.Add(new CSPrincipalDataContract(user));
            }
            return principals.ToArray();
        }

        /// <summary>
        /// Returns all configuration keys for the farm
        /// </summary>
        /// <param name="token">The API token</param>
        /// <returns>Dictionary of configuration data</returns>
        public Dictionary<string, string> GetAllConfiguration(string token)
        {
            Dictionary<string, string> config = new Dictionary<string, string>();

            foreach (CSKeyValuePair pair in GetFarm(token).AllConfiguration)
            {
                config.Add(pair.Key, pair.Value);
            }

            return config;
        }

        /// <summary>
        /// Returns all content types registered in the farm
        /// </summary>
        /// <param name="token">The API token</param>
        /// <returns>Dictionary of content types</returns>
        public Dictionary<string, string> GetAllContentTypes(string token)
        {
            Dictionary<string, string> ct = new Dictionary<string, string>();

            foreach (CSMIMEType type in GetFarm(token).AllContentTypes)
            {
                ct.Add(type.FileExtension, type.KnownMimeType);
            }

            return ct;
        }

        /// <summary>
        /// Returns the Guids of all the Sites in the farm
        /// </summary>
        /// <param name="token">The API token</param>
        /// <returns>Array of the Guids of all the sites in the farm</returns>
        public Guid[] GetAllSites(string token)
        {
            List<Guid> sites = new List<Guid>();
            foreach (CSSite site in GetFarm(token).AllSites)
            {
                sites.Add(site.Id);
            }

            return sites.ToArray();
        }

        /// <summary>
        /// Returns the Guids of all the users in the farm
        /// </summary>
        /// <param name="token">The API token</param>
        /// <returns>Array of the Guids of all the users in the farm</returns>
        public Guid[] GetAllUsers(string token)
        {
            List<Guid> users = new List<Guid>();
            foreach (CSUser usr in GetFarm(token).AllUsers)
            {
                users.Add(usr.Id);
            }

            return users.ToArray();
        }

        /// <summary>
        /// Create a new site in the farm
        /// </summary>
        /// <param name="token">The API token</param>
        /// <param name="name">Name of the site</param>
        /// <param name="description">Description for the site. Can be NULL.</param>
        /// <param name="databaseName">Name of the site database. Set to NULL to auto-generate.</param>
        /// <param name="quota">Quota for content size in the site. Set to 0 to disable quota.</param>
        /// <returns>Guid of the newly created site</returns>
        public Guid CreateSite(string token, string name, string description, string databaseName, long quota)
        {
            CSSite site = GetFarm(token).CreateSite(name, description, databaseName, quota);
            if (site == null)
            {
                return Guid.Empty;
            }

            return site.Id;
        }

        /// <summary>
        /// Gets metadata of a site given its Guid
        /// </summary>
        /// <param name="token">The API token</param>
        /// <param name="id">Guid of the site</param>
        /// <returns>Metadata of the site or NULL</returns>
        public CSSiteDataContract GetSite(string token, Guid id)
        {
            CSSite site = GetFarm(token).AllSites.Find(id);
            if (site == null)
            {
                return null;
            }

            return new CSSiteDataContract(site);
        }

        /// <summary>
        /// Create a new user in the farm
        /// </summary>
        /// <param name="token">The API token</param>
        /// <param name="username">Username for user. Cannot be a reserved system name</param>
        /// <param name="displayName">Display or friendly name</param>
        /// <param name="password">Plain-text Password (cannot be empty or null). Do not set an already encrypted password here.</param>
        /// <param name="emailAddress">Email address for the user. This is optional, but without an email address the user may not be able to participate in some operations (like Workflows).</param>
        /// <returns>The Guid of the newly created user</returns>
        public Guid CreateUser(string token, string username, string displayName, string password, string emailAddress = null)
        {
            CSUser user = GetFarm(token).AllUsers.Add(username, displayName, password, emailAddress);
            if (user == null)
            {
                return Guid.Empty;
            }

            return user.Id;
        }

        /// <summary>
        /// Gets metadata of a user given its Guid
        /// </summary>
        /// <param name="token">The API token</param>
        /// <param name="id">Guid of the user</param>
        /// <returns>Metadata of the user or NULL</returns>
        public CSPrincipalDataContract GetUser(string token, Guid id)
        {
            CSUser user = GetFarm(token).AllUsers.Find(id);
            if (user == null)
            {
                return null;
            }

            return new CSPrincipalDataContract(user);
        }

        /// <summary>
        /// Create a new user group in the farm
        /// </summary>
        /// <param name="token">The API token</param>
        /// <param name="username">Username for user group. Cannot be a reserved system name</param>
        /// <param name="displayName">Display or friendly name</param>
        /// <param name="emailAddress">Email address for the user group. This is optional, but without an email address the group may not be able to participate in some operations (like Workflows).</param>
        /// <returns>The Guid of the newly created user group</returns>
        public Guid CreateUserGroup(string token, string username, string displayName, string emailAddress = null)
        {
            CSUserGroup group = GetFarm(token).AllUserGroups.Add(username, displayName, emailAddress);
            if (group == null)
            {
                return Guid.Empty;
            }

            return group.Id;
        }

        /// <summary>
        /// Gets metadata of a user group given its Guid
        /// </summary>
        /// <param name="token">The API token</param>
        /// <param name="id">Guid of the user group</param>
        /// <returns>Metadata of the user group or NULL</returns>
        public CSPrincipalDataContract GetUserGroup(string token, Guid id)
        {
            CSUserGroup group = GetFarm(token).AllUserGroups.Find(id);
            if (group == null)
            {
                return null;
            }

            return new CSPrincipalDataContract(group);
        }

        /// <summary>
        /// Gets the list of Guids of groups that the user belongs to
        /// </summary>
        /// <param name="token">The API token</param>
        /// <param name="id">Guid of the user</param>
        /// <returns>Array of Guids of the user groups</returns>
        public Guid[] GetUserMemberships(string token, Guid id)
        {
            List<Guid> list = new List<Guid>();
            CSUser user = GetFarm(token).AllUsers.Find(id);
            if (user != null)
            {
                foreach (CSUserGroup group in user.Memberships)
                {
                    list.Add(group.Id);
                }
            }

            return list.ToArray();
        }

        /// <summary>
        /// Gets the list of Guids of users in the given user group
        /// </summary>
        /// <param name="token">The API token</param>
        /// <param name="id">Guid of the user group</param>
        /// <returns>Array of Guids of the users</returns>
        public Guid[] GetGroupMembership(string token, Guid id)
        {
            List<Guid> list = new List<Guid>();
            CSUserGroup group = GetFarm(token).AllUserGroups.Find(id);
            if (group != null)
            {
                foreach (CSUser user in group.Members)
                {
                    list.Add(user.Id);
                }
            }

            return list.ToArray();
        }

        /// <summary>
        /// Gets the list of Guids of all user groups in the farm
        /// </summary>
        /// <param name="token">The API token</param>
        /// <returns>Array of Guids of the users</returns>
        public Guid[] GetGroups(string token)
        {
            List<Guid> list = new List<Guid>();
            foreach (CSUserGroup group in GetFarm(token).AllUserGroups)
            {
                list.Add(group.Id);
            }

            return list.ToArray();
        }

        /// <summary>
        /// Sets the configuration
        /// </summary>
        /// <param name="token">The API token</param>
        /// <param name="key">Configuration key name</param>
        /// <param name="value">Configuration value to set</param>
        /// <returns>True if configuration was set up correctly</returns>
        public bool SetConfiguration(string token, string key, string value)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(value))
            {
                throw new FaultException("Key and value cannot be NULL");
            }

            CSConfigurationCollection config = GetFarm(token).AllConfiguration;

            if (config.Contains(key))
            {
                config.Update(key, value);
            }
            else
            {
                config.Add(key, value);
            }

            return true;
        }

        /// <summary>
        /// Sets a new content type
        /// </summary>
        /// <param name="token">The API token</param>
        /// <param name="filenameExtension">Filename extension</param>
        /// <param name="contentTypeName">The content type name</param>
        /// <returns>True if the content type was set up correctly</returns>
        public bool SetContentType(string token, string filenameExtension, string contentTypeName)
        {
            if (string.IsNullOrEmpty(filenameExtension) || string.IsNullOrEmpty(contentTypeName))
            {
                throw new FaultException("Filename extension and content type names cannot be NULL");
            }

            CSMIMETypeCollection types = GetFarm(token).AllContentTypes;

            CSMIMEType ct = types.Find(filenameExtension);
            if (ct != null)
            {
                ct.KnownMimeType = contentTypeName;
                return ct.Save();
            }
            else
            {
                types.Add(filenameExtension, contentTypeName);
            }

            return true;
        }

        /// <summary>
        /// Updates the properties of the user account
        /// </summary>
        /// <param name="token">The API token</param>
        /// <param name="id">Guid of the user to update</param>
        /// <param name="displayName">Display name of the user</param>
        /// <param name="emailAddress">Email address of the user</param>
        /// <returns>True if the user account was updated successfully</returns>
        public bool UpdateUser(string token, Guid id, string displayName, string emailAddress = null)
        {
            CSUser user = GetFarm(token).AllUsers.Find(id);
            if (user == null)
            {
                return false;
            }

            user.DisplayName = displayName;
            user.EmailAddress = emailAddress;
            user.Save();

            return true;
        }

        /// <summary>
        /// Updates the properties of the user group
        /// </summary>
        /// <param name="token">The API token</param>
        /// <param name="id">Guid of the user to update</param>
        /// <param name="displayName">Display name of the user</param>
        /// <param name="emailAddress">Email address of the user</param>
        /// <returns>True if the user account was updated successfully</returns>
        public bool UpdateUserGroup(string token, Guid id, string displayName, string emailAddress = null)
        {
            CSUserGroup group = GetFarm(token).AllUserGroups.Find(id);
            if (group == null)
            {
                return false;
            }

            group.DisplayName = displayName;
            group.EmailAddress = emailAddress;
            group.Save();

            return true;
        }

        /// <summary>
        /// Change the password of the user account
        /// </summary>
        /// <param name="token">The API token</param>
        /// <param name="id">Guid if the user to update</param>
        /// <param name="newPassword">The new password to set</param>
        /// <returns>True if the password was successfully changed</returns>
        public bool UpdateUserPassword(string token, Guid id, string newPassword)
        {
            CSUser user = GetFarm(token).AllUsers.Find(id);
            if (user == null)
            {
                return false;
            }

            user.ChangePassword(newPassword);
            return true;
        }
    }
}
