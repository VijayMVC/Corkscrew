using Corkscrew.SDK.objects;
using Corkscrew.SDK.providers.database;
using Corkscrew.SDK.tools;
using System;
using System.Collections.Generic;
using System.Data;

namespace Corkscrew.SDK.odm
{
    /// <summary>
    /// Object/Data manager for CSSite
    /// </summary>
    internal class OdmSite : OdmBase
    {

        private OdmUsers _odmUsers = new OdmUsers();
        private string ConfigDBServerName = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public OdmSite() : base()
        {
            ICSDatabaseProvider dataProvider = CSDatabaseProviderFactory.GetProvider("configdb", null, null);
            ConfigDBServerName = dataProvider.Connection.DataSource;
            dataProvider = null;
        }

        /// <summary>
        /// Get all Sites defined in this instance
        /// </summary>
        /// <returns>List[CSSite] found or empty list.</returns>
        public List<CSSite> GetAll()
        {
            DataSet ds = base.GetData
                    (
                        "SitesGetAll",
                        null
                    );

            // if we are still here, [ds] is automatically a valid DataSet containing data.
            List<CSSite> sites = new List<CSSite>();
            if (base.HasData(ds))
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    sites.Add(Populate(row));
                }
            }

            return sites;
        }

        /// <summary>
        /// Get site configured with the given url. 
        /// NOTE: We should never get more than one site matching a Url.
        /// </summary>
        /// <param name="dnsName">Url of the Site to retrieve</param>
        /// <returns>CSSite</returns>
        public CSSite GetByDnsName(string dnsName)
        {
            if (string.IsNullOrEmpty(dnsName))
            {
                throw new ArgumentNullException("dnsName");
            }

            DataSet ds = base.GetData
                 (
                     "SiteGetByDnsName",
                     new Dictionary<string, object>()
                    {
                        { "@DnsName", dnsName }
                    }
                 );

            if (!base.HasData(ds))
            {
                return null;
            }

            return Populate(ds.Tables[0].Rows[0]);
        }

        /// <summary>
        /// Get site configured with the given name. 
        /// NOTE: We should never get more than one site matching a Name.
        /// </summary>
        /// <param name="name">Name of the Site to retrieve</param>
        /// <returns>CSSite</returns>
        public CSSite GetByName(string name)
        {

            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }

            DataSet ds = base.GetData
                (
                    "SiteGetByName",
                    new Dictionary<string, object>()
                    {
                        { "@Name", name }
                    }
                );

            if (!base.HasData(ds))
            {
                return null;
            }

            return Populate(ds.Tables[0].Rows[0]);
        }

        /// <summary>
        /// Get site configured with the given Id. 
        /// NOTE: We should never get more than one site matching a Id.
        /// </summary>
        /// <param name="id">Id of the Site to retrieve</param>
        /// <returns>CSSite</returns>
        public CSSite GetById(Guid id)
        {

            if (id.Equals(Guid.Empty))
            {
                throw new ArgumentNullException("id");
            }

            DataSet ds = base.GetData
                (
                    "SiteGetById",
                    new Dictionary<string, object>()
                    {
                        { "@Id", id }
                    }
                );

            if (!base.HasData(ds))
            {
                return null;
            }
            return Populate(ds.Tables[0].Rows[0]);
        }

        /// <summary>
        /// Get all DNS hostnames mapped to the given site
        /// </summary>
        /// <param name="id">Guid of the site to lookup</param>
        /// <returns>List[string] of the DNS hostnames, or empty list.</returns>
        public List<string> GetAllDnsNames(Guid id)
        {

            if (id.Equals(Guid.Empty))
            {
                throw new ArgumentNullException("id");
            }

            DataSet ds = base.GetData
                    (
                        "SiteGetAllDnsNames",
                        new Dictionary<string, object>()
                        {
                            { "@SiteId", id.ToString("D") }
                        }
                    );

            // if we are still here, [ds] is automatically a valid DataSet containing data.
            List<string> names = new List<string>();
            if (base.HasData(ds))
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    names.Add(Utility.SafeString(row["DnsName"]));
                }
            }

            return names;
        }

        /// <summary>
        /// Save a DNS hostname to a Site. The business rule check of validating if the name is already 
        /// mapped to another site is done at the database side. The caller MUST check the return value of 
        /// this call to know if the map was successful.
        /// </summary>
        /// <param name="siteId">Guid of the Site</param>
        /// <param name="dnsName">DNS hostname to map</param>
        /// <returns>True if the mapping was successful.</returns>
        public bool SaveDnsName(Guid siteId, string dnsName)
        {

            if (siteId.Equals(Guid.Empty))
            {
                throw new ArgumentNullException("siteId");
            }

            if (string.IsNullOrEmpty(dnsName))
            {
                throw new ArgumentNullException("dnsName");
            }

            return base.CommitChanges
                (
                    "DnsSitesSave",
                    new Dictionary<string, object>()
                    {
                        { "@DnsName", dnsName }, 
                        { "@SiteId", siteId.ToString("D") }
                    }
                );

        }

        /// <summary>
        /// Delete a DNS hostname from a Site. Since a DNS name can be mapped only to 
        /// one Site, the Site reference is not required. However, the database will delete 
        /// the mapping from all sites (just in case)
        /// </summary>
        /// <param name="dnsName">DNS hostname to map</param>
        public bool DeleteDnsName(string dnsName)
        {
            return base.CommitChanges
            (
                "DnsSitesDeleteByDnsName",
                new Dictionary<string, object>()
                {
                    { "@DnsName", dnsName }
                }
            );
        }

        /// <summary>
        /// Save an instance of CSSite
        /// </summary>
        /// <param name="site">The site to save</param>
        public bool Save(CSSite site)
        {
            bool result = base.CommitChanges
            (
                "SiteSave",
                new Dictionary<string, object>()
                {
                    { "@Id", site.Id },
                    { "@Name", site.Name },
                    { "@Description", site.Description },
                    { "@Created", site.Created },
                    { "@CreatedBy", site.CreatedBy.Id.ToString("D") },
                    { "@Modified", site.Modified },
                    { "@ModifiedBy", site.ModifiedBy.Id.ToString("D") },
                    { "@ContentDBServer", site.ContentDatabaseServerName },
                    { "@ContentDBName", site.ContentDatabaseName },
                    { "@QuotaBytes", site.QuotaBytes }
                }
            );

            // because of limitations on the db side, we may not have saved what was requested. 
            // so, ensure Site matches what the DB knows
            site = GetById(site.Id);

            return result;
        }

        /// <summary>
        /// Test connection
        /// </summary>
        /// <param name="site">Site to try to connect to</param>
        /// <returns>Success</returns>
        public bool TestConnection(CSSite site)
        {
            ICSDatabaseProvider prov = CSDatabaseProviderFactory.GetProvider("sitedb", site.ContentDatabaseServerName, site.ContentDatabaseName);
            return prov.TryConnect(prov.Connection.ConnectionString);
        }

        /// <summary>
        /// Delete the given CSSite
        /// </summary>
        /// <param name="site">CSSite to delete</param>
        /// <returns>True if it was dropped from Config DB (actually sufficient for purposes of Corkscrew CMS)</returns>
        public bool Delete(CSSite site)
        {
            return
                base.CommitChanges
                (
                    "SiteDeleteById",
                    new Dictionary<string, object>()
                    {
                        { "@SiteId", site.Id },
                        { "@DeletedById", site.AuthenticatedUser.Id.ToString("d") }
                    }
                );
        }


        public List<CSSiteHistoryRecord> GetAllHistory(CSSite site)
        {
            List<CSSiteHistoryRecord> list = new List<CSSiteHistoryRecord>();

            DataSet ds = base.GetData
            (
                "SiteGetHistoryBySiteId",
                new Dictionary<string, object>()
                {
                    { "Id", site.Id.ToString("d") }
                }
            );

            if (HasData(ds))
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    list.Add(PopulateHistoryRecord(row));
                }
            }

            return list;
        }

        public CSSiteHistoryRecord GetHistoryRecord(long recordId)
        {
            DataSet ds = base.GetData
            (
                "SiteGetHistoryById",
                new Dictionary<string, object>()
                {
                    { "Id", recordId }
                }
            );

            if (!HasData(ds))
            {
                return null;
            }

            return PopulateHistoryRecord(ds.Tables[0].Rows[0]);
        }

        private CSSiteHistoryRecord PopulateHistoryRecord(DataRow row)
        {
            CSSiteHistoryRecord record = new CSSiteHistoryRecord()
            {
                Id = Utility.SafeConvertToLong(row["Id"]),
                Site = GetById(Utility.SafeConvertToGuid(row["SiteId"])),
                ChangeTimestamp = Utility.SafeConvertToDateTime(row["ChangeTimeStamp"]),
                ChangedBy = _odmUsers.GetUserById(Utility.SafeConvertToGuid(row["ChangedBy"])),
                Name = Utility.SafeString(row["PrevName"]),
                Description = Utility.SafeString(row["PrevDescription"]),
                ContentDatabaseServerName = Utility.SafeString(row["PrevDBServer"]),
                ContentDatabaseName = Utility.SafeString(row["PrevDBName"]),
                QuotaBytes = Utility.SafeConvertToLong(row["PrevQuotaBytes"]),
                PreviousModified = Utility.SafeConvertToDateTime(row["PrevModified"]),
                PreviousModifiedBy = _odmUsers.GetUserById(Utility.SafeConvertToGuid(row["PrevModifiedBy"]))
            };

            switch (Utility.SafeString(row["ChangeType"]))
            {
                case "I":
                    record.ChangeType = constants.ChangeTypeEnum.Create;
                    break;

                case "U":
                    record.ChangeType = constants.ChangeTypeEnum.Update;
                    break;

                case "D":
                    record.ChangeType = constants.ChangeTypeEnum.Delete;
                    break;
            }

            return record;
        }


        /// <summary>
        /// Private helper to populate the CSSite object
        /// </summary>
        /// <param name="row">DataRow with the data</param>
        /// <returns>CSSite object populated with the retrieved data</returns>
        private CSSite Populate(DataRow row)
        {
            CSSite site = new CSSite()
            {
                Id = Utility.SafeConvertToGuid(row["Id"]),
                Name = Utility.SafeString(row["Name"]),
                Description = Utility.SafeString(row["Description"]), 
                Created = Utility.SafeConvertToDateTime(row["Created"]),
                CreatedBy = _odmUsers.GetUserById(Utility.SafeConvertToGuid(row["CreatedBy"])),
                Modified = Utility.SafeConvertToDateTime(row["Modified"]),
                ModifiedBy = _odmUsers.GetUserById(Utility.SafeConvertToGuid(row["ModifiedBy"])),
                //ContentDatabaseServerName = Utility.SafeString(row["ContentDBServerName"]),               <--- Disabled
                ContentDatabaseServerName = ConfigDBServerName,
                ContentDatabaseName = Utility.SafeString(row["ContentDBName"]),
                QuotaBytes = Utility.SafeConvertToLong(row["QuotaBytes"])
            };

            return site;
        }

    }
}
