using Corkscrew.SDK.objects;
using Corkscrew.SDK.odm;
using System;

namespace Corkscrew.SDK.security
{

    /// <summary>
    /// Denotes a generic ACL in the Corkscrew system. 
    /// There is no "Farm" participant. Any ACL that has Site set to NULL applies at the Farm scope.
    /// </summary>
    public class CSPermission
    {

        #region Properties

        #region Participants

        /// <summary>
        /// Corkscrew Uri of the resource being protected.
        /// </summary>
        public string ResourceUri
        {
            get;
            set;
        }

        /// <summary>
        /// User participating in this ACL
        /// </summary>
        public CSSecurityPrincipal SecurityPrincipal
        {
            get;
            internal set;
        }

        #endregion

        #region ACL

        /// <summary>
        /// Grant of READ permission
        /// </summary>
        public bool CanRead
        {
            get
            {
                if (CanContribute)
                {
                    return true;
                }

                return _canRead;
            }
            set { _canRead = value; }
        }
        private bool _canRead = false;

        /// <summary>
        /// Grant of CONTRIBUTE permission
        /// </summary>
        public bool CanContribute
        {
            get
            {
                if (CanFullControl)
                {
                    return true;
                }

                return _canContribute;
            }
            set { _canContribute = value; }
        }
        private bool _canContribute = false;

        /// <summary>
        /// Grant of FULL CONTROL (can do anything) permission
        /// </summary>
        public bool CanFullControl
        {
            get;
            set;
        }


        /// <summary>
        /// Is an indirect permission, used when the user has a finer ACL 
        /// for a child object with no direct ACL on top level items. 
        /// i.e., this is a BOTTOM-UP acl.
        /// </summary>
        public bool IsHierarchicalAccess
        {
            get;
            set;
        }

        /// <summary>
        /// Returns if the security principal is a user group (false if principal is a CSUser).
        /// </summary>
        public bool IsGroupAcl
        {
            get
            {
                return (SecurityPrincipal is CSUserGroup);
            }
        }

        #endregion

        #region ACL Tests
        /// <summary>
        /// Returns if the ACL denotes a Site Administrator mapping
        /// </summary>
        public bool IsSiteAdministrator
        {
            get
            {
                PATH_INFO pathInfo = CSPath.GetPathInfo(ResourceUri);
                if (! pathInfo.IsValid)
                {
                    return false;
                }

                return
                (
                    ((pathInfo.ResourceUriScope == constants.ScopeEnum.Site) || (pathInfo.ResourceUriScope == constants.ScopeEnum.Farm))
                        && (CanFullControl)
                );
            }
        }

        /// <summary>
        /// Returns if the ACL denotes a Farm Administrator mapping
        /// </summary>
        public bool IsFarmAdministrator
        {
            get
            {
                PATH_INFO pathInfo = CSPath.GetPathInfo(ResourceUri);
                if (!pathInfo.IsValid)
                {
                    return false;
                }

                return
                (
                    (pathInfo.ResourceUriScope == constants.ScopeEnum.Farm)
                        && (CanFullControl)
                );
            }
        }

        /// <summary>
        /// Returns if any permission is set to True
        /// </summary>
        public bool HasAny
        {
            get
            {
                return
                (
                    CanRead
                        || CanContribute
                            || CanFullControl
                                || IsHierarchicalAccess
                );
            }
        }

        #endregion

        #endregion

        #region Constructors

        internal CSPermission(string resourceUri, CSSecurityPrincipal principal, bool read, bool contribute, bool fullControl, bool childAccess)
        {
            ResourceUri = resourceUri;
            SecurityPrincipal = principal;
            CanRead = read;
            CanContribute = contribute;
            CanFullControl = fullControl;
            IsHierarchicalAccess = childAccess;
        }

        internal static CSPermission CreateAdministratorAcl(string resourceUri, CSSecurityPrincipal principal)
        {
            return new CSPermission
            (
                resourceUri,
                principal,
                true,
                true,
                true,
                false
            );
        }

        #endregion

        #region Methods

        /// <summary>
        /// Entry point function, test if the combination of Site, FileSystem entry and User have any kind of access permissions
        /// </summary>
        /// <param name="site">CSSite participant</param>
        /// <param name="fileSystemEntry">CSFileSystemEntry participant</param>
        /// <param name="principal">Security principal participant</param>
        /// <returns>CSPermission object populated with the particular ACL</returns>
        /// <exception cref="ArgumentException">If site or filesystementry or user is non-NULL but invalid</exception>
        public static CSPermission TestAccess(CSSite site, CSFileSystemEntry fileSystemEntry, CSSecurityPrincipal principal)
        {
            string resourceUri = CSPath.CmsPathPrefix;

            if (fileSystemEntry != null)
            {
                resourceUri = fileSystemEntry.FullPath;
            }
            else if (site != null)
            {
                resourceUri = site.RootFolder.FullPath;
            }

            return TestAccess(resourceUri, principal);
        }

        /// <summary>
        /// Test access to a Corkscrew resource
        /// </summary>
        /// <param name="resourceUri">Uri to the resource to test access for</param>
        /// <param name="principal">The security principal participant</param>
        /// <returns>The ACL permission</returns>
        public static CSPermission TestAccess(string resourceUri, CSSecurityPrincipal principal)
        {
            bool isUserPrincipal = ((principal != null) && (!principal.IsGroup));
            CSPermission acl = new CSPermission(resourceUri, principal, false, false, false, false);
            OdmPermissions odm = new OdmPermissions();

            if (principal == null)
            {
                principal = CSUser.CreateAnonymousUser();
            }
            else
            {
                if (isUserPrincipal && (!((CSUser)principal).Login()))
                {
                    return acl;
                }
            }

            bool isAnonUser = (isUserPrincipal ? ((CSUser)principal).IsAnonymousUser() : false);
            bool isSysUser = (isUserPrincipal ? ((CSUser)principal).IsSystemUser() : false);

            if (isAnonUser)
            {
                acl.CanRead = true;
                acl.CanContribute = false;
                acl.CanFullControl = false;
                acl.ResourceUri = resourceUri;
            }
            else if (isSysUser)
            {
                acl.CanRead = true;
                acl.CanContribute = true;
                acl.CanFullControl = true;

                // global admin
                acl.ResourceUri = CSPath.CmsPathPrefix;
            }
            else
            {
                // if user is a global admin
                if (odm.TestAdministration(null, principal))
                {
                    acl.CanRead = true;
                    acl.CanContribute = true;
                    acl.CanFullControl = true;

                    acl.ResourceUri = CSPath.CmsPathPrefix; // global admin
                }
                else
                {
                    acl = odm.TestAccess(resourceUri, principal);

                    if (!acl.HasAny)
                    {
                        // try further up the hierarchy
                        string parentUri = CSPath.GetDirectoryName(resourceUri);
                        while (! string.IsNullOrEmpty(parentUri))
                        {
                            CSPermission parentAcl = odm.TestAccess(parentUri, principal);
                            if (parentAcl.HasAny)
                            {
                                acl.CanRead = parentAcl.CanRead;
                                acl.CanContribute = parentAcl.CanContribute;
                                acl.CanFullControl = parentAcl.CanFullControl;
                                acl.IsHierarchicalAccess = true;                // because we found it on a parent node

                                break;
                            }

                            parentUri = CSPath.GetDirectoryName(parentUri);
                        }
                    }
                }
            }

            // optimize
            if (acl.CanFullControl)
            {
                acl.CanContribute = false;
                acl.CanRead = false;
            }
            else if (acl.CanContribute)
            {
                acl.CanRead = false;
            }

            return acl;
        }

        /// <summary>
        /// Gets all the administrators
        /// </summary>
        /// <param name="site">If not NULL, gets administrators for this CSSite</param>
        /// <returns>CSPermissionCollection populated with the relevant administrators</returns>
        public static CSPermissionCollection GetAdministrators(CSSite site = null)
        {
            return new CSPermissionCollection((new OdmPermissions()).GetAdministrators(site));
        }


        /// <summary>
        /// Saves the ACL
        /// </summary>
        public void Save()
        {

            // select the best ACL
            if (CanFullControl)
            {
                CanContribute = false;
                CanRead = false;
            }
            else if (CanContribute)
            {
                CanRead = false;
            }


            // let's optimize the acls a bit.. if we already have a permissive setting up-level, lets let that be inherited 
            // instead of setting a specific one here.
            CSPermission parentAcl = TestAccess(ResourceUri, SecurityPrincipal);
            bool saveAcl = false;

            if (parentAcl.ResourceUri == ResourceUri)
            {
                // ACL is for same leaf node we were trying to modify
                // save the ACL only if any useful change has been made...

                if ((CanFullControl != parentAcl.CanFullControl) || (CanContribute != parentAcl.CanContribute) || (CanRead != parentAcl.CanRead))
                {
                    saveAcl = true;
                }
            }
            else
            {
                // we got back an ACL for a parent node higher up. 
                // save the ACL only if we dont have an equal or higher-permissive ACL already set.
                if (
                    (CanFullControl && (! parentAcl.CanFullControl)) 
                    || (CanContribute && (! parentAcl.CanContribute)) 
                    || (CanRead && (! parentAcl.CanRead))
                )
                {
                    saveAcl = true;
                }
            }

            if (saveAcl)
            {
                (new OdmPermissions()).Save(this);
            }
            
        }

        /// <summary>
        /// Delete the ACL
        /// </summary>
        public void Delete()
        {
            CanRead = false;
            CanContribute = false;
            CanFullControl = false;
            Save();
        }

        #endregion

    }
}
