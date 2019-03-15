using Corkscrew.SDK.objects;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;

namespace Corkscrew.SDK.tools
{
    /// <summary>
    /// Class provides functionality to interact with Zip archives, to extract data from them and to create them.
    /// </summary>
    public static class CSZipFiles
    {

        /// <summary>
        /// Given an archive (file stream), extract the archive and create a directory/file structure in the backend. 
        /// All file structures will be persisted before return.
        /// </summary>
        /// <param name="archiveFileStream">Stream pointing to the archive. This is usually from an uploaded file on our file manager.</param>
        /// <param name="parentFolder">Parent folder to start restoring into (everything from the archive will parent to this folder).</param>
        /// <returns>List of added file and folder objects.</returns>
        /// <exception cref="IOException">If archiveFileStream is null or cannot be read from. Also thrown if one of the zip members cannot be persisted into the Corkscrew system</exception>
        public static List<CSFileSystemEntry> ExtractArchive(Stream archiveFileStream, CSFileSystemEntryDirectory parentFolder)
        {
            if ((archiveFileStream == null) || (!archiveFileStream.CanRead))
            {
                throw new IOException("Cannot read archive stream for extraction.");
            }

            // keep track of the files we added so that we can delete them on a rollback.
            List<CSFileSystemEntry> undoList = new List<CSFileSystemEntry>();
            
            Dictionary<string, CSFileSystemEntryDirectory> folders = new Dictionary<string, CSFileSystemEntryDirectory>();
            string currentFolderPath = string.Empty;

            using (ZipInputStream zip = new ZipInputStream(archiveFileStream))
            {
                try
                {
                    ZipEntry e = null;
                    while ((e = zip.GetNextEntry()) != null)
                    {
                        if ((!e.IsDirectory) && (!e.IsFile))
                        {
                            continue;
                        }

                        string zipSubFolderCSPath = string.Join("/", parentFolder.FullPath, Path.GetDirectoryName(e.Name).Replace("\\", "/"));
                        string zipSubFolderCSPathLower = zipSubFolderCSPath.ToLower();
                        string zipFilenameCSPath = string.Join("/", zipSubFolderCSPath, Path.GetFileName(e.Name));

                        CSFileSystemEntryDirectory targetFolder = null;
                        if ((! string.IsNullOrEmpty(currentFolderPath)) && (currentFolderPath.Equals(zipSubFolderCSPathLower, StringComparison.InvariantCultureIgnoreCase)))
                        {
                            if (folders.ContainsKey(zipSubFolderCSPathLower))
                            {
                                targetFolder = folders[zipSubFolderCSPathLower];
                            }
                            else
                            {
                                targetFolder = CSFileSystemEntryDirectory.GetInfo(parentFolder.Site, zipSubFolderCSPath);
                            }
                        }

                        if (targetFolder == null)
                        {
                            targetFolder = parentFolder.CreateDirectoryTree(zipSubFolderCSPath, true);

                            if ((targetFolder != null) && (!undoList.Contains(targetFolder)))
                            {
                                undoList.Add(targetFolder);
                            }
                        }

                        if (targetFolder == null)
                        {
                            throw new IOException("Can neither find nor create the folder: " + zipSubFolderCSPathLower);
                        }

                        currentFolderPath = targetFolder.FullPath.ToLower();
                        if (!folders.ContainsKey(currentFolderPath))
                        {
                            folders.Add(currentFolderPath, targetFolder);
                        }

                        if ((e.IsFile) && (zip.CanRead) && (targetFolder != null))
                        {
                            CSFileSystemEntryFile targetFile = CSFileSystemEntryFile.GetInfo(parentFolder.Site, zipFilenameCSPath);
                            if (targetFile == null)
                            {
                                string filename = Path.GetFileNameWithoutExtension(e.Name);
                                string extension = Path.GetExtension(e.Name);

                                if (zip.Length > int.MaxValue)
                                {
                                    filename = filename + extension;
                                    extension = ".TOO_BIG";
                                }

                                targetFile = targetFolder.CreateFile
                                (
                                    filename,
                                    extension
                                );

                                undoList.Add(targetFile);
                            }

                            // if incoming data is too big for CS, then we do not update the file.
                            if ((targetFile != null) && (zip.Length <= int.MaxValue))
                            {
                                if (targetFile.Open(FileAccess.Write))
                                {
                                    using (MemoryStream memory = new MemoryStream())
                                    {
                                        zip.CopyTo(memory);

                                        byte[] data = memory.ToArray();
                                        targetFile.Write(data, 0, data.Length);
                                    }

                                    targetFile.Close();
                                }
                            }
                            else if (zip.Length > int.MaxValue)
                            {
                                undoList.Remove(targetFile);
                            }
                        }
                    }
                }
                catch
                {
                    if (undoList.Count > 0)
                    {
                        foreach (CSFileSystemEntry entry in undoList)
                        {
                            entry.Delete();
                        }

                        return new List<CSFileSystemEntry>();
                    }
                }
            }

            folders = null;
            return undoList;
        }

        /// <summary>
        /// Given a CSFileSystemEntryDirectory folder, will create a zip file of the entire structure from that point (recursively) and 
        /// return the archive stream.
        /// </summary>
        /// <param name="folder">CSFileSystemEntryDirectory to start the archive at</param>
        /// <param name="outputStream">The stream the archive is to be written to</param>
        /// <returns>Stream pointing to the archive. This can then be used to send the file as a download to the requesting user or client.</returns>
        /// <exception cref="ArgumentNullException">If folder is null</exception>
        public static Stream CreateArchive(CSFileSystemEntryDirectory folder, Stream outputStream)
        {
            if (folder == null)
            {
                throw new ArgumentNullException("folder");
            }

            string baseFolderName = folder.FilenameWithExtension;

            // DONT use:
            //using (ZipOutputStream zip = new ZipOutputStream(outputStream))
            //if you do that then [outputStream] would be CLOSED preventing any use of the created archive!!

            ZipEntryFactory entryFactory = new ZipEntryFactory();
            ZipOutputStream zip = new ZipOutputStream(outputStream);
            zip.SetLevel(9);    // best compression
            zip = RecurseFolder(folder, zip, entryFactory, baseFolderName);
            zip.Finish();

            // DONT use:
            //zip.Close(); 
            //if you do that then [outputStream] would be CLOSED preventing any use of the created archive!!

            return outputStream;
        }

        // recurse through the folders creating the zip file
        private static ZipOutputStream RecurseFolder(CSFileSystemEntryDirectory folder, ZipOutputStream stream, ZipEntryFactory entryFactory, string relativePath)
        {
            foreach (CSFileSystemEntryFile item in folder.Files)
            {
                ZipEntry f = new ZipEntry(string.Format("{0}/{1}", relativePath, item.FilenameWithExtension));
                f.DateTime = item.Modified;
                stream.PutNextEntry(f);

                if (item.Open(FileAccess.Read))
                {
                    Stream fileStream = item.GetStream();
                    byte[] fileContent = new byte[fileStream.Length];

                    if ((fileContent != null) && (fileContent.Length > 0))
                    {
                        int bytesRead = item.Read(fileContent, 0, (int)fileStream.Length);
                        stream.Write(fileContent, 0, bytesRead);
                    }
                }
            }

            foreach (CSFileSystemEntryDirectory item in folder.Directories)
            {
                string name = item.FilenameWithExtension;
                string recursivePath = string.Format("{0}/{1}", relativePath, name).Replace("//", "/").Replace("//", "/");

                ZipEntry d = entryFactory.MakeDirectoryEntry(recursivePath);
                d.DateTime = item.Modified;
                stream.PutNextEntry(d);


                stream = RecurseFolder(item, stream, entryFactory, recursivePath);
            }

            return stream;
        }

    }
}
