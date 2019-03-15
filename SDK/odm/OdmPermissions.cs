using Corkscrew.SDK.objects;
using Corkscrew.SDK.security;
using Corkscrew.SDK.tools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace Corkscrew.SDK.odm
{

    /// <summary>
    /// Object/data manager for CSPermission
    /// </summary>
    internal class OdmPermissions : OdmBase
    {

        /// <summary>
        /// Constructor
        /// </summary>
        public OdmPermissions() : base() { }

        /// <summary>
        /// Saves the given ACL
        /// </summary>
        /// <param name="acl">ACL to save</param>
        public void Save(CSPermission acl)
        {
            if (acl == null)
            {
                throw new ArgumentNullException("acl");
            }

            base.CommitChanges
            (
                "PermissionSave",
                new Dictionary<string, object>()
                {
                    { "@PrincipalId", ((acl.SecurityPrincipal != null) ? acl.SecurityPrincipal.Id : Guid.Empty) },
                    { "@CorkscrewUri",  acl.ResourceUri },
                    { "@Read", acl.CanRead },
                    { "@Contribute", acl.CanContribute },
                    { "@FullControl", acl.CanFullControl }
                }
            );

            if (acl.IsHierarchicalAccess)
            {
                base.CommitChanges
                (
                    "PermissionsSetChildAccess",
                    new Dictionary<string, object>()
                    {
                        { "@CorkscrewResourceUri", acl.ResourceUri },
                        { "@PrincipalId", ((acl.SecurityPrincipal != null) ? acl.SecurityPrincipal.Id : Guid.Empty) },
                        { "@ChildAccess", acl.IsHierarchicalAccess }
                    }
                );
            }
        }

        public CSPermission FindAggregatePermissionForUser(CSUser user, string resourceUri)
        {
            CSPermission acl = new CSPermission(resourceUri, user, false, false, false, false);

            if (user == null)
            {
                return acl;
            }

            // ro,rw,rwx must be true. Otherwise the "&" operations below will fail
            bool ro = true, rw = true, rwx = true, foundAtleastOne = false;

            foreach (CSUserGroup group in user.Memberships)
            {
                DataSet ds = base.GetData
                (
                    "PermissionsTestAccess",
                    new Dictionary<string, object>()
                    {
                        { "@CorkscrewResourceUri", resourceUri },
                        { "@PrincipalId", group.Id }
                    }
                );

                if (base.HasData(ds))
                {
                    DataRow row = ds.Tables[0].Rows[0];
                    bool wasFound = Utility.SafeConvertToBool(row["PermissionsFound"]);
                    if (wasFound)
                    {
                        foundAtleastOne = true;

                        ro &= Utility.SafeConvertToBool(row["IsRead"]);
                        rw &= Utility.SafeConvertToBool(row["IsContribute"]);
                        rwx &= Utility.SafeConvertToBool(row["IsFullControl"]);
                    }
                }
            }

            // since we inited ro,rw,rwx to true, we need to pick up values only if we actually found any ACLs in the loop.
            if (foundAtleastOne)
            {
                acl.CanRead = ro;
                acl.CanContribute = rw;
                acl.CanFullControl = rwx;
            }

            return acl;
        }

        /// <summary>
        /// Tests if the principal provided in the object has access to the provided CSSite. 
        /// </summary>
        /// <param name="resourceUri">Corkscrew resource Uri to test access for</param>
        /// <param name="principal">The user to test the access for</param>
        /// <returns>The permission ACL</returns>
        public CSPermission TestAccess(string resourceUri, CSSecurityPrincipal principal)
        {
            CSPermission acl = new CSPermission(resourceUri, principal, false, false, false, false);
            bool wasFound = false;

            DataSet ds = base.GetData
            (
                "PermissionsTestAccess",
                new Dictionary<string, object>()
                {
                    { "@CorkscrewResourceUri", resourceUri },
                    { "@PrincipalId", ((principal != null) ? principal.Id : Guid.Empty) }
                }
            );

            if (base.HasData(ds))
            {
                DataRow row = ds.Tables[0].Rows[0];

                wasFound = Utility.SafeConvertToBool(row["PermissionsFound"]);
                if (wasFound)
                {
                    acl.CanRead = Utility.SafeConvertToBool(row["IsRead"]);
                    acl.CanContribute = Utility.SafeConvertToBool(row["IsContribute"]);
                    acl.CanFullControl = Utility.SafeConvertToBool(row["IsFullControl"]);
                    acl.IsHierarchicalAccess = Utility.SafeConvertToBool(row["IsChildAccess"]);
                }
                else if ((!wasFound) && (principal != null) && (!principal.IsGroup))
                {
                    acl = FindAggregatePermissionForUser((CSUser)principal, resourceUri);
                }
            }

            // of all the other potential values for the enum, this is the least problematic
            return acl;
        }

        /// <summary>
        /// Tests if the principal provided in the object has FULL CONTROL access to the provided Site. 
        /// </summary>
        /// <param name="site">The CSSite to test</param>
        /// <param name="principal">The CSSecurityPrincipal to test</param>
        /// <returns>True if the user is any kind of admin (Site/Global)</returns>
        public bool TestAdministration(CSSite site, CSSecurityPrincipal principal)
        {
            DataSet ds = base.GetData
            (
                "PermissionsTestAdministrator",
                new Dictionary<string, object>()
                {
                    { "@SiteId", ((site != null) ? site.Id : Guid.Empty) },
                    { "@PrincipalId", ((principal != null) ? principal.Id : Guid.Empty) }
                }
            );

            if (base.HasData(ds))
            {
                DataRow row = ds.Tables[0].Rows[0];
                return Utility.SafeConvertToBool(row["IsAdmin"]);
            }

            return false;
        }

        /// <summary>
        /// Get permission ACLs for the site/entry/user combination
        /// </summary>
        /// <param name="corkscrewResourceUri">The resource in Corkscrew to get ACL for</param>
        /// <param name="principal">CSUser to get ACL for</param>
        /// <returns>CSPermission object with permissions (never NULL)</returns>
        public List<CSPermission> GetAcls(string corkscrewResourceUri, CSSecurityPrincipal principal)
        {
            List<CSPermission> permissions = new List<CSPermission>();
            string sprocName = string.Empty;
            Dictionary<string, object> sprocParams = null;

            PATH_INFO pathInfo = CSPath.GetPathInfo(corkscrewResourceUri);
            if (!pathInfo.IsValid)
            {
                return new List<CSPermission>();
            }

            // If Principal evaluates to a CSUser and is a SystemUser, no need to fetch, we are automatic admins!
            if ((principal != null) && (principal is CSUser) && (((CSUser)principal).IsSystemUser()))
            {
                // SystemUsers have access to everything!
                if (pathInfo.ResourceUriScope != constants.ScopeEnum.FileOrDirectory)
                {
                    permissions.Add(CSPermission.CreateAdministratorAcl(corkscrewResourceUri, principal));
                }
                else
                {
                    // user-wide, fetch and add for all sites we have
                    List<CSSite> allSites = (new OdmSite()).GetAll();
                    foreach (CSSite s in allSites)
                    {
                        permissions.Add(CSPermission.CreateAdministratorAcl(s.RootFolder.FullPath, principal));
                    }
                }
            }
            else
            {
                if ((!string.IsNullOrEmpty(corkscrewResourceUri)) && (principal == null))
                {
                    sprocName = "PermissionsGetByResourceUri";
                    sprocParams = new Dictionary<string, object>()
                    {
                        {"@CorkscrewResourceUri", corkscrewResourceUri }
                    };

                }
                else if ((principal != null) && (string.IsNullOrEmpty(corkscrewResourceUri)))
                {
                    sprocName = "PermissionsGetByPrincipalId";
                    sprocParams = new Dictionary<string, object>()
                    {
                        {"@PrincipalId", principal.Id }
                    };
                }
                else
                {
                    sprocName = "PermissionsTestAccess";
                    sprocParams = new Dictionary<string, object>()
                    {
                        { "@CorkscrewResourceUri", corkscrewResourceUri },
                        { "@PrincipalId", ((principal == null) ? Guid.Empty : principal.Id) }
                    };
                }

                if (string.IsNullOrEmpty(sprocName))
                {
                    throw new ArgumentException("GetAcls(): One or more arguments was incorrect. Examine stack trace and logs and fix the issue please!");
                }

                DataSet ds = base.GetData
                (
                    sprocName,
                    sprocParams
                );

                if (base.HasData(ds))
                {
                    Hashtable htPrincipals = new Hashtable();

                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        string corkscrewUri = CSPath.CmsPathPrefix;
                        if (ds.Tables[0].Columns.Contains("CorkscrewUri"))
                        {
                            corkscrewUri = Utility.SafeString(row["CorkscrewUri"], CSPath.CmsPathPrefix);
                        }
                        else if (ds.Tables[0].Columns.Contains("CorkscrewResourceUri"))
                        {
                            corkscrewUri = Utility.SafeString(row["CorkscrewResourceUri"], CSPath.CmsPathPrefix);
                        }

                        Guid principalId = Utility.SafeConvertToGuid(row["PrincipalId"]);
                        CSSecurityPrincipal rowPrincipal = null;

                        if (!htPrincipals.Contains(principalId))
                        {
                            if ((principal != null) && (principal.Id.Equals(principalId)))
                            {
                                rowPrincipal = principal;
                            }
                            else
                            {
                                if (principalId.Equals(Guid.Empty))
                                {
                                    rowPrincipal = CSUser.CreateAnonymousUser();
                                }
                                else
                                {
                                    // principalId can be a User or a Group. Try user first (no particular reason)
                                    rowPrincipal = CSUser.GetById(principalId);
                                    if (rowPrincipal == null)
                                    {
                                        rowPrincipal = CSUserGroup.GetById(principalId);
                                    }
                                }
                            }

                            htPrincipals.Add(rowPrincipal.Id, rowPrincipal);
                        }

                        permissions.Add(
                            new CSPermission
                            (
                                corkscrewUri,
                                rowPrincipal,
                                Utility.SafeConvertToBool(row["IsRead"]),
                                Utility.SafeConvertToBool(row["IsContribute"]),
                                Utility.SafeConvertToBool(row["IsFullControl"]),
                                Utility.SafeConvertToBool(row["IsChildAccess"])
                            )
                        );
                    }
                }
            }


            return permissions;
        }

        /// <summary>
        /// Get all administrators
        /// </summary>
        /// <param name="site">The Site to return administrators for (if given). If null, gets for all sites</param>
        /// <returns>List of CSPermission containing administrators</returns>
        public List<CSPermission> GetAdministrators(CSSite site)
        {
            List<CSPermission> permissions = new List<CSPermission>();
            DataSet ds = base.GetData("PermissionsGetAllAdministrators");
            if (base.HasData(ds))
            {
                DataTable table = ds.Tables[0];

                OdmSite _siteOdm = new OdmSite();
                OdmUsers _userOdm = new OdmUsers();
                Hashtable siteCache = new Hashtable();

                foreach (DataRow row in table.Rows)
                {
                    string corkscrewuri = Utility.SafeString(row["CorkscrewUri"]).ToLower();
                    Guid userId = Utility.SafeConvertToGuid(row["PrincipalId"]);
                    CSUser aclUser = ((userId == Guid.Empty) ? CSUser.CreateAnonymousUser() : _userOdm.GetUserById(userId));

                    CSSite aclSite = null;
                    if (site != null)
                    {
                        // only pick up record that matches the given site's Id or is a Farm Admin record.
                        if (corkscrewuri.Equals(site.FullPath))
                        {
                            aclSite = site;
                        }
                    }
                    else if (site == null)
                    {
                        if (siteCache.ContainsKey(corkscrewuri))
                        {
                            aclSite = (CSSite)siteCache[corkscrewuri];
                        }
                        else
                        {
                            Guid siteId = Guid.Empty;
                            PATH_INFO path = CSPath.GetPathInfo(corkscrewuri);
                            if (path.ResourceUriScope != constants.ScopeEnum.Farm)
                            {
                                siteId = path.SiteId;
                            }

                            aclSite = ((siteId == Guid.Empty) ? aclSite = CSSite.GetConfigSite(aclUser) : _siteOdm.GetById(siteId));
                            aclSite.AuthenticatedUser = aclUser;

                            siteCache.Add(corkscrewuri, aclSite);
                        }
                    }

                    if (aclUser != null)
                    {
                        CSPermission acl = new CSPermission(aclSite.RootFolder.FullPath, aclUser, true, true, true, false);
                        foreach (CSPermission p in permissions)
                        {
                            if (p.SecurityPrincipal.Id.Equals(userId))
                            {
                                acl = null;
                                break;
                            }
                        }

                        if (acl != null)
                        {
                            permissions.Add(acl);
                        }
                    }
                }
            }

            return permissions;
        }
    }
}
