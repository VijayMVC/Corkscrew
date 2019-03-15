using Corkscrew.SDK.odm;
using Corkscrew.SDK.tools;
using System;
using System.Collections.Generic;

namespace Corkscrew.SDK.security
{

    /// <summary>
    /// This class represents a single user account in the Corkscrew system.
    /// </summary>
    public class CSUser : CSSecurityPrincipal
    {

        /// <summary>
        /// These usernames cannot be used for creating a new user and will be rejected.
        /// </summary>
        internal static List<string> RESERVED_USERNAMES =
            new List<string>()
            {
                "system",
                "anonymous"
            };

        /// <summary>
        /// Guid of the SYSTEM user
        /// </summary>
        public static Guid CORKSCREW_SYSTEM_USER_GUID = Guid.Parse("99999999-0000-0043-4f52-4b5343524557");

        /// <summary>
        /// Guid of the Anonymous user
        /// </summary>
        public static Guid ANONYMOUS_USER_GUID = Guid.Parse("99999999-0100-0043-4f52-4b5343524557");

        /// <summary>
        /// The password or the hash is never visible to any object. But we store it here for comparison purposes.
        /// </summary>
        internal string CurrentPasswordHash = null;

        /// <summary>
        /// Cheat flag to indicate this is an AD-backed user account
        /// </summary>
        internal bool IsWindowsActiveDirectoryUser = false;

        /// <summary>
        /// flag indicating if Login() has been called at least once.
        /// </summary>
        private bool b_isLoggedIn = false;

        #region Properties

        /// <summary>
        /// Groups this user is a member of. Elements are of type CSUserGroup. Readonly.
        /// </summary>
        public CSUserGroupCollection Memberships
        {
            get
            {
                if (_memberships == null)
                {
                    _memberships = new CSUserGroupCollection
                    (
                        (new OdmUsers()).GetUserMemberships(this),
                        true                                            // collection is always readonly
                    );
                }

                return _memberships;
            }
        }
        private CSUserGroupCollection _memberships = null;

        #endregion

        #region Constructors

        /// <summary>
        /// Blank constructor used internally
        /// </summary>
        protected CSUser() : base() { IsGroup = false; }

        /// <summary>
        /// Rehydration constructor
        /// </summary>
        /// <param name="id">Guid of the user</param>
        /// <param name="username">Username for user.</param>
        /// <param name="displayName">Display or friendly name</param>
        /// <param name="password">Password. WARNING! Pass the SHA256 hash or DB-stored value here. No conversion is done!</param>
        /// <param name="emailAddress">Email address for the user</param>
        internal CSUser(Guid id, string username, string displayName, string password, string emailAddress = null)
            : base(id, username, displayName, emailAddress)
        {
            IsGroup = false;
            CurrentPasswordHash = password;
        }

        /// <summary>
        /// Create a new user in the Corkscrew system
        /// </summary>
        /// <param name="username">Username for user. Cannot be a reserved system name</param>
        /// <param name="displayName">Display or friendly name</param>
        /// <param name="password">Plain-text Password (cannot be empty or null). We will always encrypt this, so do not set an already encrypted password here.</param>
        /// <param name="emailAddress">Email address for the user</param>
        /// <returns>The newly created user</returns>
        /// <exception cref="ArgumentException">If the username is already in use by an existing user account</exception>
        /// <exception cref="ArgumentNullException">If username, password or display name are null</exception>
        /// <exception cref="ArgumentOutOfRangeException">If the username is one of the reserved usernames</exception>
        public static CSUser CreateUser(string username, string displayName, string password, string emailAddress = null)
        {

            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentNullException("Username cannot be null.");
            }

            if (CSUser.RESERVED_USERNAMES.ContainsNoCase(username))
            {
                throw new ArgumentOutOfRangeException("Cannot set username, is system reserved.");
            }

            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException("Password cannot be null.");
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

            CSUser newUser = new CSUser();

            // generate a Guid that does not map to a system user.
            // we dont want to check actual user existance in IsSystemUser 
            // because we dont want a future system user to collide. 
            // so we want to stay off the entire guid range.

            do
            {
                newUser.Id = Guid.NewGuid();
            } while (newUser.Id.Equals(ANONYMOUS_USER_GUID) || newUser.Id.Equals(CORKSCREW_SYSTEM_USER_GUID));

            username = username.SafeString(removeAtStart: "CORKSCREW\\", removeAtEnd: "@corscrew");

            newUser.DisplayName = displayName;
            newUser.Username = username;
            newUser.CurrentPasswordHash = Utility.GetSha256Hash(password);
            newUser.EmailAddress = emailAddress;
            newUser.IsWindowsActiveDirectoryUser = false;

            (new OdmUsers()).Save(newUser);

            return newUser;
        }
        
        /// <summary>
        /// Create an instance of a Corkscrew System User
        /// </summary>
        /// <returns>A CSUser object with the system user data.</returns>
        public static CSUser CreateSystemUser()
        {
            return new CSUser(CORKSCREW_SYSTEM_USER_GUID, "system", "Corkscrew System User", GenerateNewPassword(), "system@corkscrew");
        }

        /// <summary>
        /// Creates the system anonymous user
        /// </summary>
        /// <returns>CSUser corresponding to the CORKSCREW\Anonymous user.</returns>
        public static CSUser CreateAnonymousUser()
        {
            return new CSUser(ANONYMOUS_USER_GUID, "anonymous", "Anonymous User", GenerateNewPassword(), "anonymous@corkscrew");
        }

        /// <summary>
        /// Generates a new password
        /// </summary>
        /// <returns>Plain-text string password</returns>
        public static string GenerateNewPassword()
        {
            string password = string.Empty;
            int passwordLength = 8;
            string passwordCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()-_+={[}]:;<,>.?";
            int passwordCharactersLength = passwordCharacters.Length;

            Random rnd = new Random();

            while (passwordLength > 0)
            {
                int index = rnd.Next(passwordCharactersLength);
                password = string.Format("{0}{1}", password, passwordCharacters[index]);
                passwordLength--;
            }

            return password;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Change the password for this user
        /// </summary>
        /// <param name="plainTextNewPassword">Plain text new password to set up</param>
        /// <exception cref="InvalidOperationException">If this operation is attempted for a system or anonymous user</exception>
        public void ChangePassword(string plainTextNewPassword)
        {
            if (IsSystemUser() || IsAnonymousUser())
            {
                throw new InvalidOperationException("Cannot change password for system user.");
            }

            if (IsWindowsActiveDirectoryUser)
            {
                throw new InvalidOperationException("Cannot change password for OS-backed user.");
            }

            CurrentPasswordHash = Utility.GetSha256Hash(plainTextNewPassword);

            // this method may be called independantly of any other operation,
            // so save it
            Save();
        }

        /// <summary>
        /// Change the password for this user. A new random password is set. 
        /// </summary>
        /// <remarks>BE CAREFUL while using this method, since it is not possible to retrieve the plain-text password!</remarks>
        /// <exception cref="InvalidOperationException">If this operation is attempted for a system or anonymous user</exception>
        public void ChangePassword()
        {
            ChangePassword(GenerateNewPassword());
        }

        /// <summary>
        /// Returns if the given plain text password is the same as the one currently set.
        /// </summary>
        /// <param name="plainTextPassword">Plain text password to test (cannot be null or empty)</param>
        /// <returns>True if passwords are same.</returns>
        /// <exception cref="ArgumentNullException">If the plainTextPassword is null or empty</exception>
        public bool IsCurrentPassword(string plainTextPassword)
        {


            if (IsWindowsActiveDirectoryUser)
            {
                throw new InvalidOperationException("Cannot check password for OS-backed user.");
            }


            if (string.IsNullOrEmpty(plainTextPassword))
            {
                throw new ArgumentNullException("Password cannot be null or empty.");
            }

            if (Utility.GetSha256Hash(plainTextPassword).Equals(CurrentPasswordHash))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Save this instance of CSUser object
        /// </summary>
        public void Save()
        {
            if (!IsSystemUser() && !IsAnonymousUser())
            {
                (new OdmUsers()).Save(this);
            }

        }

        /// <summary>
        /// Deletes the current user
        /// </summary>
        /// <returns>True if deleted</returns>
        public bool Delete()
        {
            if (!IsSystemUser() && !IsAnonymousUser())
            {
                // we also drop ACLs in the SPROC we call
                return (new OdmUsers()).Delete(this);
            }

            return false;
        }

        /// <summary>
        /// Resolve a user Guid to the user
        /// </summary>
        /// <param name="id">Guid of the user</param>
        /// <returns>A valid CSUser</returns>
        /// <remarks>It is possible to pass in the Guids of the system or anonymous user and get those users through this method as well</remarks>
        public static CSUser GetById(Guid id)
        {

            if (id.Equals(Guid.Empty))
            {
                return null;
            }

            if (id.Equals(ANONYMOUS_USER_GUID))
            {
                return CreateAnonymousUser();
            }

            if (id.Equals(CORKSCREW_SYSTEM_USER_GUID))
            {
                return CreateSystemUser();
            }

            OdmUsers odm = new OdmUsers();
            return odm.GetUserById(id);
        }

        /// <summary>
        /// Returns if the given username exists in the system. 
        /// </summary>
        /// <param name="userName">Username to test</param>
        /// <returns>True if username exists</returns>
        public static bool UserExists(string userName)
        {
            return (GetByUsername(userName) != null);
        }

        /// <summary>
        /// Returns the user matching the given username
        /// </summary>
        /// <param name="userName">Username</param>
        /// <returns>The matching user or null</returns>
        public static CSUser GetByUsername(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                return null;
            }

            if (userName.EndsWith("@corkscrew"))
            {
                userName = userName.Replace("@corkscrew", "");
            }

            if (CreateSystemUser().Username.Equals(userName))
            {
                return CreateSystemUser();
            }
            else if (CreateAnonymousUser().Username.Equals(userName))
            {
                return CreateAnonymousUser();
            }

            return (new OdmUsers()).GetUserByName(userName);
        }

        /// <summary>
        /// Try to login the user
        /// </summary>
        /// <param name="username">Username of the user</param>
        /// <param name="passwordSHA256Hash">SHA256 hash of the user's login password</param>
        /// <returns>CSUser if the login was successful. Else NULL.</returns>
        /// <remarks>If username is that of the system or anonymous users, that user is directly returned (successful login)</remarks>
        public static CSUser Login(string username, string passwordSHA256Hash)
        {
            CSUser user = null;

            if (string.IsNullOrEmpty(username))
            {
                return null;
            }

            switch (username.ToLower())
            {
                case "system":
                    user = CreateSystemUser();
                    break;

                case "anonymous":
                    user = CreateAnonymousUser();
                    break;

                default:
                    user = (new OdmUsers()).Login(username, passwordSHA256Hash);
                    break;
            }

            if (user != null)
            {
                user.b_isLoggedIn = true;
            }

            return user;
        }

        /// <summary>
        /// This method is used by the API web service to login. Note that built-in users cannot login using this method 
        /// as built-in users are denied access to the API service for security reasons.
        /// </summary>
        /// <remarks>
        /// Built-in users are denied access to the API service as they are granted full access to the 
        /// Corkscrew system by default.
        /// </remarks>
        /// <param name="username">Username of the user</param>
        /// <param name="passwordSHA256Hash">SHA256 hash of the user's login password</param>
        /// <param name="ipaddress">IP address the remote client is logging in from</param>
        /// <returns>A token string, will be 64 characters long. Will be NULL if login failed.</returns>
        public static string APILogin(string username, string passwordSHA256Hash, string ipaddress)
        {
            CSUser user = null;

            if (string.IsNullOrEmpty(username))
            {
                return null;
            }

            string token = Utility.GetSha256Hash(GenerateNewPassword());        // new token

            user = (new OdmUsers()).LoginAPI(username, passwordSHA256Hash, token, ipaddress);
            if (user == null)
            {
                return null;
            }

            return token;
        }

        /// <summary>
        /// Verify that the API login token and IP address are valid.
        /// </summary>
        /// <param name="token">API token granted by the APILogin call</param>
        /// <param name="ipaddress">IP address the remote client is logging in from</param>
        /// <returns>The matching CSUser if the token was validated correctly, NULL if validation failed.</returns>
        public static CSUser VerifyAPILogin(string token, string ipaddress)
        {
            return (new OdmUsers()).VerifyAPILogin(token, ipaddress);
        }

        /// <summary>
        /// Attempts to login the current user object
        /// </summary>
        /// <returns>True if login was successful.</returns>
        /// <remarks>If username is that of the system or anonymous users, a value of True directly returned (successful login)</remarks>
        public bool Login()
        {
            if (IsSystemUser() || IsAnonymousUser())
            {
                return true;
            }

            if (b_isLoggedIn)
            {
                return true;
            }

            CSUser result = (new OdmUsers()).Login(Username, CurrentPasswordHash);
            if ((result != null) && (result.Id.Equals(Id)))
            {
                b_isLoggedIn = true;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns if the user is a System user
        /// </summary>
        /// <returns>True if system user</returns>
        public bool IsSystemUser()
        {
            if (Id.Equals(CORKSCREW_SYSTEM_USER_GUID))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns if the user is the Anonymous user
        /// </summary>
        /// <returns>True if anonymous</returns>
        public bool IsAnonymousUser()
        {
            if (Id.Equals(ANONYMOUS_USER_GUID))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns if the user is the anonymous or the system user
        /// </summary>
        /// <returns>True if the user is Anonymous or System user</returns>
        public bool IsOobeUser()
        {
            return (IsAnonymousUser() || IsSystemUser());
        }

        /// <summary>
        /// Returns if the user is a valid user who is not the system or anonymous users.
        /// </summary>
        /// <returns>True if valid user</returns>
        public bool IsNonOobeValidUser()
        {
            if (Id.Equals(Guid.Empty))
            {
                return false;
            }

            if (IsOobeUser())
            {
                return false;
            }

            CSUser user = GetById(Id);
            if (user == null)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Validates the given user
        /// </summary>
        /// <param name="user">CSUser to validate</param>
        /// <returns>True if user is valid</returns>
        public static bool Validate(CSUser user)
        {
            return user.Login();
        }

        /// <summary>
        /// Join the given user group
        /// </summary>
        /// <param name="group">User group to join</param>
        /// <returns>True if successfully joined</returns>
        public bool Join(CSUserGroup group)
        {
            return (new OdmUsers()).Add(this, group);
        }

        /// <summary>
        /// Leave the given user group
        /// </summary>
        /// <param name="group">User group to leave</param>
        /// <returns>True if successfully departed from the group</returns>
        public bool Leave(CSUserGroup group)
        {
            return (new OdmUsers()).Delete(this, group);
        }

        #endregion

    }
}
