using Corkscrew.SDK.objects;
using System.IO;
using System.Web.Hosting;

namespace Corkscrew.SDK.providers.filesystem
{

    /// <summary>
    /// Implementation of virtual directory provider
    /// </summary>
    public sealed class CSVirtualDirectory : VirtualDirectory
    {

        /// <summary>
        /// Reference to the Corkscrew File System driver
        /// </summary>
        public CSFileSystemProvider VirtualFilesystemProvider
        {
            get;
            internal set;
        }

        /// <summary>
        /// Reference to the directory
        /// </summary>
        public CSFileSystemEntryDirectory Directory
        {
            get;
            internal set;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="virtualPath">Virtual path to the directory</param>
        /// <param name="driver">Reference tot he CSFileSystemProvider</param>
        public CSVirtualDirectory(string virtualPath, CSFileSystemProvider driver)
            : base(virtualPath)
        {
            if (driver.FileSystemEntry == null)
            {
                throw new FileNotFoundException("Directory cannot be accessed or opened.");
            }

            if (!driver.FileSystemEntry.IsFolder)
            {
                throw new FileNotFoundException("Resource is a file, not a directory.");
            }

            VirtualFilesystemProvider = driver;

            Directory = CSFileSystemEntryDirectory.GetInfo
                    (
                        VirtualFilesystemProvider.FileSystemEntry.Site,
                        VirtualFilesystemProvider.FileSystemEntry.FullPath
                    );
        }


        /// <summary>
        /// Gets enumerable reference to all immediate child elements (folders and files)
        /// </summary>
        public override System.Collections.IEnumerable Children
        {
            get
            {
                if (Directory == null)
                {
                    return null;
                }

                return Directory.Items;
            }
        }

        /// <summary>
        /// Gets enumerable reference to all immediate child folder elements
        /// </summary>
        public override System.Collections.IEnumerable Directories
        {
            get
            {
                if (Directory == null)
                {
                    return null;
                }

                return Directory.Directories;
            }
        }

        /// <summary>
        /// Gets enumerable reference to all immediate child file elements
        /// </summary>
        public override System.Collections.IEnumerable Files
        {
            get
            {
                if (Directory == null)
                {
                    return null;
                }

                return Directory.Files;
            }
        }
    }
}
