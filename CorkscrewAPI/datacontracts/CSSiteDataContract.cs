using Corkscrew.SDK.objects;
using Corkscrew.SDK.security;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Corkscrew.API.datacontracts
{

    /// <summary>
    /// Data contract object representing a CSSite
    /// </summary>
    [DataContract]
    public class CSSiteDataContract
    {

        /// <summary>
        /// Unique Id of the site
        /// </summary>
        [DataMember]
        public Guid Id
        {
            get;
            private set;
        }

        /// <summary>
        /// User friendly name of the site
        /// </summary>
        [DataMember]
        public string Name
        {
            get;
            private set;
        }

        /// <summary>
        /// Description of the site
        /// </summary>
        [DataMember]
        public string Description
        {
            get;
            private set;
        }

        /// <summary>
        /// Set the site quota, in bytes. A value of zero disables quota.
        /// </summary>
        [DataMember]
        public long QuotaBytes
        {
            get;
            private set;
        }

        /// <summary>
        /// Date and time of creation. 
        /// Set by constructor.
        /// </summary>
        [DataMember]
        public DateTime Created
        {
            get;
            private set;
        }

        /// <summary>
        /// User who created.
        /// Set by constructor.
        /// </summary>
        //[DataMember]
        public CSUser CreatedBy
        {
            get;
            private set;
        }

        /// <summary>
        /// Date and time of modification. 
        /// Set internally or by persistence.
        /// </summary>
        [DataMember]
        public DateTime Modified
        {
            get;
            private set;
        }

        /// <summary>
        /// User who modified. 
        /// Set internally or by persistence.
        /// </summary>
        //[DataMember]
        public CSUser ModifiedBy
        {
            get;
            private set;
        }

        /// <summary>
        /// Get/Set the name of the SiteDB database for this Site. If no explicit name has been set, 
        /// this is calculated from the Id.
        /// To reset the name to default name, simply set the value to NULL.
        /// </summary>
        [DataMember]
        public string ContentDatabaseName
        {
            get;
            private set;
        }

        /// <summary>
        /// Get/Set the name of the server hosting the content database. 
        /// ConfigDB site will have NULL here.
        /// </summary>
        [DataMember]
        public string ContentDatabaseServerName
        {
            get;
            private set;
        }


        /// <summary>
        /// Manage DNS name mappings for this Site.
        /// Cannot manage DNS names for ConfigDB site.
        /// </summary>
        //[DataMember]
        public string[] DNSNames
        {
            get;
            private set;
        }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="site">The CSSite to construct the contract from</param>
        public CSSiteDataContract(CSSite site)
        {
            if (site == null)
            {
                return;
            }

            Id = site.Id;
            Name = site.Name;
            Description = site.Description;
            QuotaBytes = site.QuotaBytes;
            Created = site.Created;
            CreatedBy = site.CreatedBy;
            Modified = site.Modified;
            ModifiedBy = site.ModifiedBy;
            ContentDatabaseName = site.ContentDatabaseName;
            ContentDatabaseServerName = site.ContentDatabaseServerName;

            List<string> names = new List<string>();
            if (!site.Id.Equals(Guid.Empty))
            {
                foreach (string name in site.DNSNames)
                {
                    names.Add(name);
                }
            }

            DNSNames = names.ToArray();

        }
    }
}