using Corkscrew.SDK.security;
using Corkscrew.SDK.tools;
using System;
using System.Collections.Generic;
using System.Data;

namespace Corkscrew.SDK.odm
{

    /// <summary>
    /// ODM class for CSUsers
    /// </summary>
    internal class OdmUsers : OdmBase
    {

        /// <summary>
        ///  Constructor
        /// </summary>
        public OdmUsers() : base() { }
        
        /// <summary>
        /// Get a single user given the Guid. 
        /// NOTE: We do not deal with System users here. To get 
        /// system users, use [CSUser.GetById()].
        /// </summary>
        /// <param name="id">Guid of the user to fetch</param>
        /// <returns>CSUser populated with the user information</returns>
        public CSUser GetUserById(Guid id)
        {
            DataSet ds = null;

            if (id.Equals(CSUser.ANONYMOUS_USER_GUID))
            {
                return CSUser.CreateAnonymousUser();
            }

            if (id.Equals(CSUser.CORKSCREW_SYSTEM_USER_GUID))
            {
                return CSUser.CreateSystemUser();
            }

            ds = base.GetData
                (
                    "UsersGetById"
                    , new Dictionary<string, object>()
                    {
                        { "@Id", id }
                    }
                );

            if (!base.HasData(ds))
            {
                return null;
            }

            return PopulateUser(ds.Tables[0].Rows[0]);
        }

        public CSUserGroup GetGroupById(Guid id)
        {
            DataSet ds = base.GetData
                (
                    "UserGroupGetById"
                    , new Dictionary<string, object>()
                    {
                        { "@GroupId", id }
                    }
                );

            if (!base.HasData(ds))
            {
                return null;
            }

            return PopulateUserGroup(ds.Tables[0].Rows[0]);
        }

        /// <summary>
        /// Get a single user given the username. 
        /// NOTE: We do not deal with System users here. To get 
        /// system users, use [CSUser.GetById()].
        /// </summary>
        /// <param name="name">Username of the user to fetch</param>
        /// <returns>CSUser populated with the user information</returns>
        public CSUser GetUserByName(string name)
        {
            DataSet ds = base.GetData
               (
                   "UserGetByName"
                   , new Dictionary<string, object>()
                   {
                        { "@Username", name }
                   }
               );

            if (!base.HasData(ds))
            {
                return null;
            }
            return PopulateUser(ds.Tables[0].Rows[0]);
        }

        public CSUserGroup GetGroupByName(string name)
        {
            DataSet ds = base.GetData
               (
                   "UserGroupGetByName"
                   , new Dictionary<string, object>()
                   {
                        { "@Username", name }
                   }
               );

            if (!base.HasData(ds))
            {
                return null;
            }
            return PopulateUserGroup(ds.Tables[0].Rows[0]);
        }

        /// <summary>
        /// Get all users in the database
        /// </summary>
        /// <returns>List[CSUsers]. Will be empty list if no users exist.</returns>
        public List<CSUser> GetUsersAll()
        {
            DataSet ds = base.GetData
                    (
                        "UsersGetAll",
                        null
                    );

            List<CSUser> users = new List<CSUser>();
            if (base.HasData(ds))
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    users.Add(PopulateUser(row));
                }
            }
            return users;
        }

        public List<CSUserGroup> GetUserGroupsAll()
        {
            DataSet ds = base.GetData
                    (
                        "UserGroupGetAll",
                        null
                    );

            List<CSUserGroup> groups = new List<CSUserGroup>();
            if (base.HasData(ds))
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    groups.Add(PopulateUserGroup(row));
                }
            }

            return groups;
        }

        public List<CSUserGroup> GetUserMemberships(CSUser user)
        {
            DataSet ds = base.GetData
                    (
                        "GetUserMemberships",
                        new Dictionary<string, object>()
                        {
                            { "@UserId", user.Id }
                        }
                    );

            List<CSUserGroup> groups = new List<CSUserGroup>();
            if (base.HasData(ds))
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    groups.Add(PopulateUserGroup(row));
                }
            }

            return groups;
        }

        public List<CSUser> GetGroupMemberships(CSUserGroup group)
        {
            DataSet ds = base.GetData
                    (
                        "GetGroupMembership",
                        new Dictionary<string, object>()
                        {
                            { "@GroupId", group.Id }
                        }
                    );

            List<CSUser> users = new List<CSUser>();
            if (base.HasData(ds))
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    users.Add(PopulateUser(row));
                }
            }

            return users;
        }

        public List<CSUser> GetUsersNotInGroup(CSUserGroup group)
        {
            DataSet ds = base.GetData
                    (
                        "GetUsersNotInGroupByGroupId",
                        new Dictionary<string, object>()
                        {
                            { "@GroupId", group.Id }
                        }
                    );

            List<CSUser> users = new List<CSUser>();
            if (base.HasData(ds))
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    users.Add(PopulateUser(row));
                }
            }

            return users;
        }

        /// <summary>
        /// Saves the user
        /// </summary>
        /// <param name="user">CSUser to save</param>
        public void Save(CSUser user)
        {
            base.CommitChanges
            (
                "UserSave",
                new Dictionary<string, object>()
                {
                    { "@Id", user.Id },
                    { "@Username", user.Username },
                    { "@SecretHash", user.CurrentPasswordHash },
                    { "@DisplayName", user.DisplayName },
                    { "@EmailAddress", user.EmailAddress },
                    { "@IsWinADUser", false }                           // this is always FALSE from this Odm. Only OdmActiveDirectoryUsers set this flag
                }
            );
        }

        public void Save(CSUserGroup group)
        {
            base.CommitChanges
            (
                "UserGroupSave",
                new Dictionary<string, object>()
                {
                    { "@Id", group.Id },
                    { "@Username", group.Username },
                    { "@DisplayName", group.DisplayName },
                    { "@EmailAddress", group.EmailAddress },
                    { "@IsWinADGroup", false }                           // this is always FALSE from this Odm. Only OdmActiveDirectoryUsers set this flag
                }
            );
        }

        public bool Add(CSUser user, CSUserGroup group)
        {
            return base.CommitChanges
            (
                "AddUserToGroup",
                new Dictionary<string, object>()
                {
                    { "@UserId", user.Id },
                    { "@GroupId", group.Id},
                    { "@IsWinADMap", false }                           // this is always FALSE from this Odm. Only OdmActiveDirectoryUsers set this flag
                }
            );
        }

        /// <summary>
        /// Delete the given CSUser from the system. 
        /// Also deletes all associated group mappings and ACLs
        /// </summary>
        /// <param name="user">The CSUser to delete</param>
        public bool Delete(CSUser user)
        {
            return base.CommitChanges
            (
                "UserDeleteById",
                new Dictionary<string, object>()
                {
                    { "@Id", user.Id }
                }
            );
        }

        public bool Delete(CSUserGroup group)
        {
            return base.CommitChanges
            (
                "UserGroupDeleteById",
                new Dictionary<string, object>()
                {
                    { "@GroupId", group.Id }
                }
            );
        }

        public bool Delete(CSUser user, CSUserGroup group)
        {
            return base.CommitChanges
            (
                "RemoveUserFromGroup",
                new Dictionary<string, object>()
                {
                    { "@UserId", user.Id },
                    { "@GroupId", group.Id}
                }
            );
        }

        /// <summary>
        /// Try to login the user
        /// </summary>
        /// <param name="username">Username of the user</param>
        /// <param name="passwordSHA256Hash">SHA256 hash of the user's login password</param>
        /// <returns>CSUser if the login was successful. Else NULL.</returns>
        public CSUser Login(string username, string passwordSHA256Hash)
        {
            DataSet ds = base.GetData
                (
                    "UserLogin"
                    , new Dictionary<string, object>()
                    {
                        { "@Username", username },
                        { "@PasswordHash", passwordSHA256Hash },
                        { "@APILogin", false },
                        { "@APIToken", string.Empty },
                        { "@RemoteAddress", string.Empty }
                    }
                );

            if (!base.HasData(ds))
            {
                return null;
            }

            // we cannot use "PopulateUser" function because 
            // UserTestLogin sproc does not return "SecretHash" column.
            return new CSUser
            (
                Utility.SafeConvertToGuid(ds.Tables[0].Rows[0]["Id"]),
                Utility.SafeString(ds.Tables[0].Rows[0]["Username"]),
                Utility.SafeString(ds.Tables[0].Rows[0]["DisplayName"]),
                Utility.SafeString(passwordSHA256Hash),                      // populate from parameter
                Utility.SafeString(ds.Tables[0].Rows[0]["EmailAddress"])
            )
            {
                IsWindowsActiveDirectoryUser = Utility.SafeConvertToBool(ds.Tables[0].Rows[0]["IsWinADUser"])
            };
        }

        public CSUser LoginAPI(string username, string passwordSHA256Hash, string token, string ipaddress)
        {
            DataSet ds = base.GetData
                (
                    "UserLogin"
                    , new Dictionary<string, object>()
                    {
                        { "@Username", username },
                        { "@PasswordHash", passwordSHA256Hash },
                        { "@APILogin", true },
                        { "@APIToken", token },
                        { "@RemoteAddress", ipaddress }
                    }
                );

            if (!base.HasData(ds))
            {
                return null;
            }

            // we cannot use "PopulateUser" function because 
            // UserTestLogin sproc does not return "SecretHash" column.
            return new CSUser
            (
                Utility.SafeConvertToGuid(ds.Tables[0].Rows[0]["Id"]),
                Utility.SafeString(ds.Tables[0].Rows[0]["Username"]),
                Utility.SafeString(ds.Tables[0].Rows[0]["DisplayName"]),
                Utility.SafeString(passwordSHA256Hash),                      // populate from parameter
                Utility.SafeString(ds.Tables[0].Rows[0]["EmailAddress"])
            )
            {
                IsWindowsActiveDirectoryUser = Utility.SafeConvertToBool(ds.Tables[0].Rows[0]["IsWinADUser"])
            };
        }

        public CSUser VerifyAPILogin(string token, string ipaddress)
        {
            DataSet ds = base.GetData
                (
                    "UserVerifyAPILogin"
                    , new Dictionary<string, object>()
                    {
                        { "@APIToken", token },
                        { "@RemoteAddress", ipaddress }
                    }
                );

            if (!base.HasData(ds))
            {
                return null;
            }

            return PopulateUser(ds.Tables[0].Rows[0]);
        }

        private CSUser PopulateUser(DataRow row)
        {
            return new CSUser
            (
                Utility.SafeConvertToGuid(row["Id"]),
                Utility.SafeString(row["Username"]),
                Utility.SafeString(row["DisplayName"]),
                Utility.SafeString(row["SecretHash"]),
                Utility.SafeString(row["EmailAddress"])
            )
            {
                IsWindowsActiveDirectoryUser = Utility.SafeConvertToBool(row["IsWinADUser"])
            };
        }

        private CSUserGroup PopulateUserGroup(DataRow row)
        {
            return new CSUserGroup
            (
                Utility.SafeConvertToGuid(row["Id"]),
                Utility.SafeString(row["Username"]),
                Utility.SafeString(row["DisplayName"]),
                Utility.SafeString(row["EmailAddress"])
            )
            {
                IsWindowsActiveDirectoryGroup = Utility.SafeConvertToBool(row["IsWinADGroup"])
            };
        }


    }
}
