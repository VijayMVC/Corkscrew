using Corkscrew.API.datacontracts;
using Corkscrew.SDK.security;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace Corkscrew.API.services
{

    /// <summary>
    /// This service allows interaction with the Corkscrew system at the Farm level.
    /// </summary>
    [ServiceContract]
    public interface IFarm
    {

        /// <summary>
        /// Returns all the Administrator users in the farm
        /// </summary>
        /// <param name="token">The API token</param>
        /// <returns>Array of CSPrincipalDataContract objects</returns>
        [OperationContract]
        CSPrincipalDataContract[] GetAllAdministrators(string token);

        /// <summary>
        /// Returns all configuration keys for the farm
        /// </summary>
        /// <param name="token">The API token</param>
        /// <returns>Dictionary of configuration data</returns>
        [OperationContract]
        Dictionary<string, string> GetAllConfiguration(string token);

        /// <summary>
        /// Returns all content types registered in the farm
        /// </summary>
        /// <param name="token">The API token</param>
        /// <returns>Dictionary of content types</returns>
        [OperationContract]
        Dictionary<string, string> GetAllContentTypes(string token);

        /// <summary>
        /// Returns the Guids of all the Sites in the farm
        /// </summary>
        /// <param name="token">The API token</param>
        /// <returns>Array of the Guids of all the sites in the farm</returns>
        [OperationContract]
        Guid[] GetAllSites(string token);

        /// <summary>
        /// Returns the Guids of all the users in the farm
        /// </summary>
        /// <param name="token">The API token</param>
        /// <returns>Array of the Guids of all the users in the farm</returns>
        [OperationContract]
        Guid[] GetAllUsers(string token);

        /// <summary>
        /// Create a new site in the farm
        /// </summary>
        /// <param name="token">The API token</param>
        /// <param name="name">Name of the site</param>
        /// <param name="description">Description for the site. Can be NULL.</param>
        /// <param name="databaseName">Name of the site database. Set to NULL to auto-generate.</param>
        /// <param name="quota">Quota for content size in the site. Set to 0 to disable quota.</param>
        /// <returns>Guid of the newly created site</returns>
        [OperationContract]
        Guid CreateSite(string token, string name, string description, string databaseName, long quota);

        /// <summary>
        /// Gets metadata of a site given its Guid
        /// </summary>
        /// <param name="token">The API token</param>
        /// <param name="id">Guid of the site</param>
        /// <returns>Metadata of the site or NULL</returns>
        [OperationContract]
        CSSiteDataContract GetSite(string token, Guid id);

        /// <summary>
        /// Create a new user in the farm
        /// </summary>
        /// <param name="token">The API token</param>
        /// <param name="username">Username for user. Cannot be a reserved system name</param>
        /// <param name="displayName">Display or friendly name</param>
        /// <param name="password">Plain-text Password (cannot be empty or null). Do not set an already encrypted password here.</param>
        /// <param name="emailAddress">Email address for the user. This is optional, but without an email address the user may not be able to participate in some operations (like Workflows).</param>
        /// <returns>The Guid of the newly created user</returns>
        [OperationContract]
        Guid CreateUser(string token, string username, string displayName, string password, string emailAddress = null);

        /// <summary>
        /// Gets metadata of a user given its Guid
        /// </summary>
        /// <param name="token">The API token</param>
        /// <param name="id">Guid of the user</param>
        /// <returns>Metadata of the user or NULL</returns>
        [OperationContract]
        CSPrincipalDataContract GetUser(string token, Guid id);

        /// <summary>
        /// Create a new user group in the farm
        /// </summary>
        /// <param name="token">The API token</param>
        /// <param name="username">Username for user group. Cannot be a reserved system name</param>
        /// <param name="displayName">Display or friendly name</param>
        /// <param name="emailAddress">Email address for the user group. This is optional, but without an email address the group may not be able to participate in some operations (like Workflows).</param>
        /// <returns>The Guid of the newly created user group</returns>
        [OperationContract]
        Guid CreateUserGroup(string token, string username, string displayName, string emailAddress = null);

        /// <summary>
        /// Gets metadata of a user group given its Guid
        /// </summary>
        /// <param name="token">The API token</param>
        /// <param name="id">Guid of the user group</param>
        /// <returns>Metadata of the user group or NULL</returns>
        [OperationContract]
        CSPrincipalDataContract GetUserGroup(string token, Guid id);

        /// <summary>
        /// Gets the list of Guids of groups that the user belongs to
        /// </summary>
        /// <param name="token">The API token</param>
        /// <param name="id">Guid of the user</param>
        /// <returns>Array of Guids of the user groups</returns>
        [OperationContract]
        Guid[] GetUserMemberships(string token, Guid id);

        /// <summary>
        /// Gets the list of Guids of users in the given user group
        /// </summary>
        /// <param name="token">The API token</param>
        /// <param name="id">Guid of the user group</param>
        /// <returns>Array of Guids of the users</returns>
        [OperationContract]
        Guid[] GetGroupMembership(string token, Guid id);

        /// <summary>
        /// Gets the list of Guids of all user groups in the farm
        /// </summary>
        /// <param name="token">The API token</param>
        /// <returns>Array of Guids of the users</returns>
        [OperationContract]
        Guid[] GetGroups(string token);


        /// <summary>
        /// Sets the configuration
        /// </summary>
        /// <param name="token">The API token</param>
        /// <param name="key">Configuration key name</param>
        /// <param name="value">Configuration value to set</param>
        /// <returns>True if configuration was set up correctly</returns>
        [OperationContract]
        bool SetConfiguration(string token, string key, string value);

        /// <summary>
        /// Sets a new content type
        /// </summary>
        /// <param name="token">The API token</param>
        /// <param name="filenameExtension">Filename extension</param>
        /// <param name="contentTypeName">The content type name</param>
        /// <returns>True if the content type was set up correctly</returns>
        [OperationContract]
        bool SetContentType(string token, string filenameExtension, string contentTypeName);

        /// <summary>
        /// Updates the properties of the user account
        /// </summary>
        /// <param name="token">The API token</param>
        /// <param name="id">Guid of the user to update</param>
        /// <param name="displayName">Display name of the user</param>
        /// <param name="emailAddress">Email address of the user</param>
        /// <returns>True if the user account was updated successfully</returns>
        [OperationContract]
        bool UpdateUser(string token, Guid id, string displayName, string emailAddress = null);

        /// <summary>
        /// Updates the properties of the user group
        /// </summary>
        /// <param name="token">The API token</param>
        /// <param name="id">Guid of the user to update</param>
        /// <param name="displayName">Display name of the user</param>
        /// <param name="emailAddress">Email address of the user</param>
        /// <returns>True if the user account was updated successfully</returns>
        [OperationContract]
        bool UpdateUserGroup(string token, Guid id, string displayName, string emailAddress = null);

        /// <summary>
        /// Change the password of the user account
        /// </summary>
        /// <param name="token">The API token</param>
        /// <param name="id">Guid if the user to update</param>
        /// <param name="newPassword">The new password to set</param>
        /// <returns>True if the password was successfully changed</returns>
        [OperationContract]
        bool UpdateUserPassword(string token, Guid id, string newPassword);
    }
}
