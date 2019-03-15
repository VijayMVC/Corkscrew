using Corkscrew.SDK.objects;
using System.IO;
using System.Web.Hosting;

namespace Corkscrew.SDK.providers.filesystem
{

    /// <summary>
    /// Implementation of virtual file provider
    /// </summary>
    public sealed class CSVirtualFile : VirtualFile
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
        /// Returns the virtual file
        /// </summary>
        public CSFileSystemEntryFile CorkscrewFile
        {
            get;
            internal set;
        }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="virtualPath">Virtual path to the directory</param>
        /// <param name="driver">Reference tot he CSFileSystemProvider</param>
        /// <param name="file">Reference to the file being used</param>
        public CSVirtualFile(string virtualPath, CSFileSystemProvider driver, CSFileSystemEntryFile file)
            : base(virtualPath)
        {
            VirtualFilesystemProvider = driver;
            CorkscrewFile = file;
        }

        /// <summary>
        /// Opens the stream for reading.
        /// </summary>
        /// <returns>Stream or NULL</returns>
        public override Stream Open()
        {
            // this func will always have 0 references, it is called automatically by the IIS/WAS host.

            // only need this to Read, since we are a Virtual File provider
            if (! CorkscrewFile.Open(FileAccess.Read))
            {
                return null;
            }

            return CorkscrewFile.FileStream;
        }

    }
}
