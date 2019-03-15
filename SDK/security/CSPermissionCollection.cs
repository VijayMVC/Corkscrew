using Corkscrew.SDK.objects;
using Corkscrew.SDK.odm;
using System.Collections.Generic;

namespace Corkscrew.SDK.security
{
    /// <summary>
    /// Collection of CSPermission ACLs
    /// </summary>
    public class CSPermissionCollection : CSBaseCollection<CSPermission>
    {

        /// <summary>
        /// Constructor. Creates using given IEnumerable collection
        /// </summary>
        /// <param name="permissions">Enumeration of permissions</param>
        /// <param name="isReadonly">If set, creates the collection as readonly</param>
        public CSPermissionCollection(IEnumerable<CSPermission> permissions, bool isReadonly = false)
            : base(permissions, isReadonly)
        {
        }

        /// <summary>
        /// Constructor. Creates access control list for given combination of CSSite, CSFileSystemEntry and CSUser. 
        /// Self populates children based on ACL pulled.
        /// </summary>
        /// <param name="site">Site for ACL. Can be NULL</param>
        /// <param name="fileSystemEntry">FileSystemEntry for ACL. Can be NULL</param>
        /// <param name="principal">Security Principal for ACL. Can be NULL</param>
        public CSPermissionCollection(CSSite site, CSFileSystemEntry fileSystemEntry, CSSecurityPrincipal principal)
        {
            string corkscrewUri = CSPath.CmsPathPrefix;
            if (fileSystemEntry != null)
            {
                corkscrewUri = fileSystemEntry.FullPath;
            }
            else if (site != null)
            {
                corkscrewUri = site.RootFolder.FullPath;
            }

            OdmPermissions odm = new OdmPermissions();
            List<CSPermission> acls = odm.GetAcls(corkscrewUri, principal);
            
            foreach(CSPermission acl in acls)
            {
                base.AddInternal(acl);
            }
        }
        

    }
}
