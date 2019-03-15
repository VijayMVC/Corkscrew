using Corkscrew.SDK.objects;
using Corkscrew.SDK.tools;
using System;
using System.Collections.Generic;

namespace Corkscrew.SDK.security
{
    /// <summary>
    /// This class is a common representation of CSUser and CSGroup for cases where one of the either is to be used.
    /// </summary>
    public class CSSecurityPrincipal
    {

        #region Properties

        /// <summary>
        /// Principal Id
        /// </summary>
        public Guid Id { get; protected set; }

        /// <summary>
        /// Username if user, Groupname if Group
        /// </summary>
        public string Username
        {
            get { return _name; }
            set { _name = value.SafeString(255, false, true, nameof(Username)); }
        }
        private string _name = string.Empty;

        /// <summary>
        /// Display name
        /// </summary>
        public string DisplayName
        {
            get { return _displayName; }
            set
            {
                _displayName = value.SafeString(255, false, true, nameof(DisplayName));
            }
        }
        private string _displayName = string.Empty;

        /// <summary>
        /// Email address of the user or group
        /// </summary>
        public string EmailAddress
        {
            get
            {
                return _emailAddress;
            }
            set
            {
                _emailAddress = value.SafeString(512, true, true, nameof(EmailAddress), null);
            }
        }
        private string _emailAddress = null;

        /// <summary>
        /// A long-format of the display name that combines the Display and User names in a 
        /// ["Display Name" &lt;EmailAddress&gt;] or if EmailAddress is empty, ["Display Name" &lt;Username&gt;] format.
        /// </summary>
        public string LongformDisplayName
        {
            get
            {
                return string.Format
                (
                    "\"{0}\" <{1}>",
                    DisplayName,
                    (string.IsNullOrEmpty(EmailAddress) ? Username : EmailAddress)
                );
            }
        }

        /// <summary>
        /// Returns if the principal is a group
        /// </summary>
        public bool IsGroup
        {
            get;
            internal set;
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Blank constructor used internally
        /// </summary>
        protected CSSecurityPrincipal() { }

        /// <summary>
        /// Rehydration constructor
        /// </summary>
        /// <param name="id">Guid of the user</param>
        /// <param name="username">Username for user.</param>
        /// <param name="displayName">Display or friendly name</param>
        /// <param name="emailAddress">Email address for the user</param>
        internal CSSecurityPrincipal(Guid id, string username, string displayName, string emailAddress = null)
        {
            Id = id;
            Username = username;
            DisplayName = displayName;
            EmailAddress = emailAddress;
        }

        #endregion

    }

    /// <summary>
    /// Collection of security principal objects
    /// </summary>
    public class CSSecurityPrincipalCollection : CSBaseCollection<CSSecurityPrincipal>
    {

        /// <summary>
        /// Returns the principal with the given Guid if present or NULL
        /// </summary>
        /// <param name="id">Guid of the principal to fetch</param>
        /// <returns>CSSecurityPrincipal if found or NULL</returns>
        public CSUser this[Guid id]
        {
            get
            {
                foreach(CSUser item in Collection)
                {
                    if (item.Id.Equals(id))
                    {
                        return item;
                    }
                }

                return null;
            }
        }

        /// <summary>
        /// Returns the principal with the given Username if present or NULL
        /// </summary>
        /// <param name="username">Username of the principal to fetch</param>
        /// <returns>CSSecurityPrincipal if found or NULL</returns>
        public CSUser this[string username]
        {
            get
            {
                foreach (CSUser item in Collection)
                {
                    if (item.Username.Equals(username))
                    {
                        return item;
                    }
                }

                return null;
            }
        }

        internal CSSecurityPrincipalCollection() : base() { }

        internal CSSecurityPrincipalCollection(IEnumerable<CSSecurityPrincipal> list, bool asReadonly) 
            : base(list, asReadonly)
        {
        }

        internal void Add(CSSecurityPrincipal principal)
        {
            AddInternal(principal, false);
        }

        internal void Remove(CSSecurityPrincipal principal)
        {
            RemoveInternal(principal, false);
        }



        /// <summary>
        /// Find a security principal in the collection
        /// </summary>
        /// <param name="id">Guid of the principal</param>
        /// <returns>Principal found or NULL</returns>
        public CSSecurityPrincipal Find(Guid id)
        {
            foreach (CSSecurityPrincipal item in Collection)
            {
                if (item.Id.Equals(id))
                {
                    return item;
                }
            }

            return null;
        }

        /// <summary>
        /// Find a security principal in the collection
        /// </summary>
        /// <param name="name">Username value of the security principal</param>
        /// <returns>Security principal found or NULL</returns>
        public CSSecurityPrincipal Find(string name)
        {
            foreach (CSSecurityPrincipal item in Collection)
            {
                if (item.Username.Equals(name))
                {
                    return item;
                }
            }

            return null;
        }

    }

}
