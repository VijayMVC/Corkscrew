using Corkscrew.SDK.security;
using System;
using System.Runtime.Serialization;

namespace Corkscrew.API.datacontracts
{

    /// <summary>
    /// Data contract object representing a CSSecurityPrincipal instance
    /// </summary>
    [DataContract]
    public class CSPrincipalDataContract
    {

        /// <summary>
        /// User Guid
        /// </summary>
        [DataMember]
        public Guid Id
        {
            get;
            private set;
        }

        /// <summary>
        /// Login or sign-in name
        /// </summary>
        [DataMember]
        public string Username
        {
            get;
            private set;
        }

        /// <summary>
        /// Display name
        /// </summary>
        [DataMember]
        public string DisplayName
        {
            get;
            private set;
        }

        /// <summary>
        /// Email address of the user
        /// </summary>
        [DataMember]
        public string EmailAddress
        {
            get;
            private set;
        }

        /// <summary>
        /// A long-format of the display name that combines the display name and username of the user account
        /// </summary>
        [DataMember]
        public string LongformDisplayName
        {
            get;
            private set;
        }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="principal">Security principal this data contract belongs to</param>
        public CSPrincipalDataContract(CSSecurityPrincipal principal)
        {
            if (principal == null)
            {
                return;
            }

            Id = principal.Id;
            Username = principal.Username;
            DisplayName = principal.DisplayName;
            LongformDisplayName = principal.LongformDisplayName;
            EmailAddress = principal.EmailAddress;
        }

    }
}