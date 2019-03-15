using Corkscrew.SDK.objects;
using Corkscrew.SDK.security;
using Corkscrew.SDK.tools;
using System;
using System.Collections;
using System.IO;
using System.Web;
using System.Web.Caching;
using System.Web.Hosting;

namespace Corkscrew.SDK.providers.filesystem
{
    /// <summary>
    /// Implements VirtualPathProvider for Corkscrew File System. This class can only be used when the Corkscrew engine is hosted on a web application, 
    /// for example, with a website that uses Corkscrew as its filesystem.
    /// </summary>
    public class CSFileSystemProvider : VirtualPathProvider
    {

        /// <summary>
        /// sync locking object. Used only in RegisterWithHostingEnvironment()
        /// </summary>
        private object _syncLock = null;

        private CSDefaultPageProvider _defaultPageProvider = null;
        private CSSite _site = null;
#if DEBUG
        private StreamWriter _logWriter = null;
#endif

        #region Properties

        /// <summary>
        /// Gets if the provider is registered with the hosting environment
        /// </summary>
        public bool IsRegistered
        {
            get;
            internal set;
        } = false;

        /// <summary>
        /// Filesystem entry resolved from the request
        /// </summary>
        public CSFileSystemEntry FileSystemEntry
        {
            get;
            internal set;
        } = null;

        /// <summary>
        /// Returns if the requested resource is a valid Corkscrew file item
        /// </summary>
        public bool IsValidCorkscrewItem
        {
            get;
            private set;
        } = false;

        #endregion

        /// <summary>
        /// Constructor. 
        /// Initializes ApplicationRootAbsolutePath, and registers itself to the hosting environment.
        /// </summary>
        /// <remarks>Call this constructor from the Global.asax code file</remarks>
        public CSFileSystemProvider()
            : base()
        {
            _defaultPageProvider = new CSDefaultPageProvider();

#if DEBUG
            _logWriter = new StreamWriter(HttpContext.Current.Server.MapPath("~/access_log.txt"));
#endif

            // register
            _syncLock = new object();
            IsRegistered = false;
            RegisterWithHostingEnvironment();
        }

        #region Methods

        /// <summary>
        /// Register with the hosting environment as a virtual path provider
        /// </summary>
        /// <returns>True if the registration was successful, false if not.</returns>
        public bool RegisterWithHostingEnvironment()
        {
            try
            {
                lock (_syncLock)
                {
                    if (!IsRegistered)
                    {
                        HostingEnvironment.RegisterVirtualPathProvider(this);
                        IsRegistered = true;
                    }
                }
            }
            catch
            {
                // dont change IsRegistered, we may already be registered through another thread.
                throw;
            }

            LogAndFlush("CSFileSystemProvider.RegisterWithHostingEnvironment(): " + IsRegistered.ToString());

            return IsRegistered;
        }


        internal bool Exists(string virtualPath)
        {
            LogAndFlush("CSFileSystemProvider.Exists(): " + virtualPath);

            IsValidCorkscrewItem = false;

            CSUser authenticatedUser = CSUser.CreateAnonymousUser();

            if ((HttpContext.Current != null) && (HttpContext.Current.Request != null) && (HttpContext.Current.Request.IsAuthenticated) && (HttpContext.Current.User != null))
            {
                CSWebPrincipal p = null;

                try
                {
                    p = (CSWebPrincipal)HttpContext.Current.User;
                    authenticatedUser = p.User;
                }
                catch
                {
                    // authenticated user is not a CSSecurityPrincipal.
                    // user will remain set to Anonymous User
                }
            }

            _site = CSSite.OpenByDnsName(authenticatedUser, HttpContext.Current.Request.Url.Authority);

            try
            {
                virtualPath = CSPath.GetFullPath(_site, virtualPath.SafeString(onEmpty: "/", expectStart: "/", removeAtStart: "~", removeAtEnd: "/"));
                FileSystemEntry = _site.GetFileSystemItem(virtualPath);
                IsValidCorkscrewItem = (FileSystemEntry != null);
            }
            catch
            {
                FileSystemEntry = null;
                IsValidCorkscrewItem = false;
            }

            LogAndFlush("CSFileSystemProvider.Exists(): " + virtualPath + ": result = " + IsValidCorkscrewItem.ToString());
            return IsValidCorkscrewItem;
        }

        private CSFileSystemEntryFile GetDefaultPageIfExists()
        {
            LogAndFlush("CSFileSystemProvider.GetDefaultPageIfExists(): " + FileSystemEntry.FullPath);
            return _defaultPageProvider.GetDefaultPageForPath(FileSystemEntry);
        }


        /// <summary>
        /// Checks if the given path corresponds to a file. 
        /// </summary>
        /// <param name="virtualPath">The path to check</param>
        /// <returns>True if the location is a file</returns>
        public override bool FileExists(string virtualPath)
        {
            LogAndFlush("CSFileSystemProvider.FileExists(): " + virtualPath);

            // skip checking if it is a virtualized ASP.NET resource path (App_)
            if ((!virtualPath.Contains("/App_")) && (! virtualPath.Contains(".axd")))
            {
                bool directCSFSFileExists = Exists(virtualPath);

                if (directCSFSFileExists && (!FileSystemEntry.IsFolder))
                {
                    return true;
                }

                // do we need a default page map?
                if ((!directCSFSFileExists) && (GetDefaultPageIfExists() != null))
                {
                    return true;
                }
            }

            LogAndFlush("CSFileSystemProvider.FileExists(): " + virtualPath + ": Not Corkscrew File");
            return Previous.FileExists(virtualPath);
        }

        /// <summary>
        /// Checks if the given path corresponds to a directory. 
        /// </summary>
        /// <param name="virtualDir">The path to check</param>
        /// <returns>True if the location is a directory</returns>
        public override bool DirectoryExists(string virtualDir)
        {
            LogAndFlush("CSFileSystemProvider.DirectoryExists(): " + virtualDir);

            // skip checking if it is a virtualized ASP.NET resource path (App_)
            if ((!virtualDir.Contains("/App_")) && (!virtualDir.Contains(".axd")))
            {
                if ((Exists(virtualDir)) && (FileSystemEntry.IsFolder))
                {
                    return true;
                }
            }

            LogAndFlush("CSFileSystemProvider.DirectoryExists(): " + virtualDir + ": Not Corkscrew Directory");

            // check any other providers...
            return Previous.DirectoryExists(virtualDir);
        }

        /// <summary>
        /// Gets the file represented by the given virtual path
        /// </summary>
        /// <param name="virtualPath">The path that supposedly is a file</param>
        /// <returns>VirtualFile instance if found, else NULL</returns>
        public override VirtualFile GetFile(string virtualPath)
        {
            CSFileSystemEntryFile file = null;

            LogAndFlush("CSFileSystemProvider.GetFile(): " + virtualPath);

            if (string.IsNullOrEmpty(virtualPath) || virtualPath.EndsWith("/"))
            {
                file = GetDefaultPageIfExists();
            }
            else if ((Exists(virtualPath)) && (!FileSystemEntry.IsFolder))
            {
                file = new CSFileSystemEntryFile(FileSystemEntry);
            }

            if (file != null)
            {
                return new CSVirtualFile(virtualPath, this, file);
            }

            LogAndFlush("CSFileSystemProvider.GetFile(): " + virtualPath + ": Not Corkscrew File");

            return Previous.GetFile(virtualPath);
        }

        /// <summary>
        /// Gets the directory represented by the given virtual path
        /// </summary>
        /// <param name="virtualDir">The path that supposedly is a directory</param>
        /// <returns>VirtualDirectory instance if found, else NULL</returns>
        public override VirtualDirectory GetDirectory(string virtualDir)
        {
            LogAndFlush("CSFileSystemProvider.GetDirectory(): " + virtualDir);

            if ((Exists(virtualDir)) && (FileSystemEntry.IsFolder))
            {
                return new CSVirtualDirectory(virtualDir, this);
            }

            LogAndFlush("CSFileSystemProvider.GetDirectory(): " + virtualDir + ": Not Corkscrew Directory");

            // IIS virtual path handler
            return Previous.GetDirectory(virtualDir);
        }

        #region -- File Caching --

        /*
         * These two methods GetCacheDependency and GetFileHash have been written to ensure that 
         * no dependencies on CSFS objects are set or taken
         * 
         */

        /// <summary>
        /// Gets the dependency on the virtual path or one of its dependencies
        /// </summary>
        /// <param name="virtualPath">The virtual path to get dependencies for</param>
        /// <param name="virtualPathDependencies">Existing dependencies on the virtual path</param>
        /// <param name="utcStart">Start date/time for the cache</param>
        /// <returns>The dependency or NULL (this function will always return NULL for a Corkscrew-hosted object)</returns>
        public override CacheDependency GetCacheDependency(string virtualPath, IEnumerable virtualPathDependencies, DateTime utcStart)
        {
            // if our main item (also in virtualPath) is a valid CSFS item, return NULL
            // no cache dependency set
            if (IsValidCorkscrewItem)
            {
                return null;
            }

            foreach (string path in virtualPathDependencies)
            {
                if (CSFileSystemEntry.Exists(_site, path))
                {
                    // return NULL for no dependency
                    return null;
                }
            }

            return Previous.GetCacheDependency(virtualPath, virtualPathDependencies, utcStart);
        }

        /// <summary>
        /// Gets a hash value for the given virtual path
        /// </summary>
        /// <param name="virtualPath">Virtual path to get the hashvalue for</param>
        /// <param name="virtualPathDependencies">Dependencies for the virtual path</param>
        /// <returns>The file hash</returns>
        public override string GetFileHash(string virtualPath, IEnumerable virtualPathDependencies)
        {
            if (IsValidCorkscrewItem)
            {
                return Utility.GetSha256Hash(virtualPath);

                //// return a new guid everytime so that no hash will match with a previous one.
                //return Guid.NewGuid().ToString();
            }

            return Previous.GetFileHash(virtualPath, virtualPathDependencies);
        }

        #endregion

        #endregion


        private void LogAndFlush(string message)
        {
#if DEBUG
            _logWriter.WriteLine(message);
            _logWriter.Flush();
#endif
        }

    }
    }
