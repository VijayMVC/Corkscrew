using Corkscrew.SDK.objects;
using Corkscrew.SDK.security;
using Corkscrew.SDK.tools;
using System;
using System.IO;
using System.Threading;
using System.Web;

namespace Corkscrew.ControlCenter.filesystem
{
    public partial class Download : System.Web.UI.Page
    {

        private CSFileSystemEntry targetItem = null;

        #region Properties

        public bool DownloadsAreAllowed
        {
            get
            {
                if (_downloadsAllowed == null)
                {
                    _downloadsAllowed = Utility.SafeConvertToBool(System.Configuration.ConfigurationManager.AppSettings["FileManager:AllowFolderDownload"]);
                }

                return Utility.SafeConvertToBool(_downloadsAllowed);
            }
        }
        private bool? _downloadsAllowed = null;

        public Guid SiteId
        {
            get
            {
                return Utility.SafeConvertToGuid(Request.QueryString["SiteId"]);
            }
        }

        public CSUser CurrentUser
        {
            get
            {
                if (_currentUser == null)
                {
                    _currentUser = WebHelpers.GetSessionUser(HttpContext.Current);
                }

                return _currentUser;
            }
        }
        private CSUser _currentUser = null;

        public CSSite CurrentSite
        {
            get
            {
                if (_currentSite == null)
                {
                    _currentSite = CSFarm.Open(CurrentUser).AllSites.Find(SiteId);
                }

                return _currentSite;
            }
        }
        private CSSite _currentSite = null;

        public string BrowserPath
        {
            get
            {
                return Utility.SafeString(Server.UrlDecode(Request.QueryString["Path"]), "/");
            }
        }

        public string BrowserPathCorkscrewUri
        {
            get
            {
                return CSPath.GetFullPath(CurrentSite, BrowserPath);
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (SiteId.Equals(Guid.Empty) || (CurrentSite == null))
            {
                Response.Redirect("/sites/All.aspx");
            }

            targetItem = CurrentSite.GetFileSystemItem(BrowserPathCorkscrewUri);
            if (targetItem == null)
            {
                throw new HttpException(404, "File or directory cannot be found. Perhaps it was deleted, or you no longer have access to it. Contact your site or farm administrator if you believe this should not be the case.");
            }

            if (targetItem.IsFolder && (!DownloadsAreAllowed))
            {
                throw new HttpException(404, "Target is not a file. Contact your site or farm administrator if you believe this should not be the case.");
            }

            try
            {
                byte[] buffer = null;
                string sendAsContentType = CSMIMEType.DEFAULT_MIME_TYPE;
                string fileName = targetItem.FilenameWithExtension;

                if (targetItem.IsFolder)
                {
                    // we already checked if folder downloads are allowed above.
                    MemoryStream zipStream = new MemoryStream();
                    zipStream = (MemoryStream)CSZipFiles.CreateArchive(new CSFileSystemEntryDirectory(targetItem), zipStream);
                    if (zipStream != null)
                    {
                        buffer = zipStream.ToArray();
                        sendAsContentType = "application/zip";
                        fileName = targetItem.FilenameWithExtension + ".zip";
                    }
                }
                else
                {
                    CSFileSystemEntryFile currentFile = new CSFileSystemEntryFile(targetItem);
                    if (currentFile != null)
                    {
                        if (currentFile.Size > 0)
                        {
                            if (currentFile.Open(System.IO.FileAccess.Read))
                            {
                                buffer = new byte[currentFile.Size];
                                currentFile.Read(buffer, 0, currentFile.Size);
                                currentFile.Close();
                            }
                        }
                    }
                }

                if (buffer == null)
                {
                    Response.Clear();
                    Response.Status = "400. File does not exist or could not be opened.";
                    Response.End();
                }
                else
                {
                    Response.Clear();
                    Response.ContentType = sendAsContentType;
                    Response.AddHeader("Content-disposition", "attachment; filename=" + fileName);
                    Response.AddHeader("Content-length", ((buffer == null) ? "0" : buffer.Length.ToString()));
                    Response.BinaryWrite(buffer);
                    Response.Flush();
                    Response.Close();
                    Response.End();
                }
            }
            catch (ThreadAbortException)
            {
                Thread.ResetAbort();
            }
        }
    }
}