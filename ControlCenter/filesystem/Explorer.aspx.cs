using Corkscrew.SDK.objects;
using Corkscrew.SDK.security;
using Corkscrew.SDK.tools;
using System;
using System.Linq;
using System.Web;

namespace Corkscrew.ControlCenter.filesystem
{
    public partial class Explorer : System.Web.UI.Page
    {

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

        public bool UserIsGlobalAdministrator
        {
            get
            {
                if (_userIsGlobalAdmin == null)
                {
                    _userIsGlobalAdmin = CSPermission.TestAccess(null, null, CurrentUser).IsFarmAdministrator;
                }

                return Utility.SafeConvertToBool(_userIsGlobalAdmin);
            }
        }
        private bool? _userIsGlobalAdmin = null;

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

        public string FolderNavigationBreadCrumb
        {
            get
            {
                if (string.IsNullOrEmpty(_folderBreadCrumb))
                {
                    string result = string.Format("<a href=\"Explorer.aspx?SiteId={0}&Path={1}\">[Root]</a>", SiteId.ToString("d"), Server.UrlEncode("/"));
                    string subPath = string.Empty;

                    string path = BrowserPath;
                    if (CSPath.IsValidUri(BrowserPath))
                    {
                        path = CSPath.GetPathInfo(BrowserPath).ResourceURI;
                    }

                    foreach (string piece in path.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        subPath = Utility.SafeString(string.Join("/", subPath, piece.Trim()).RemoveExtraSlashes(), "/", removeAtStart: "/", removeAtEnd: "/");
                        result = string.Format("{0} &gt; <a href=\"Explorer.aspx?SiteId={1}&Path={2}\">{3}</a>", result, SiteId.ToString("d"), Server.UrlEncode(subPath), piece);
                    }

                    _folderBreadCrumb = result;
                }

                return _folderBreadCrumb;
            }
        }
        private string _folderBreadCrumb = null;

        #endregion

        protected CSFileSystemEntryDirectory currentFolder = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (
                    (SiteId.Equals(Guid.Empty) && (!UserIsGlobalAdministrator)) 
                 || (CurrentSite == null)
            )
            {
                Response.Redirect("/sites/All.aspx");
            }

            currentFolder = CurrentSite.GetDirectory(BrowserPathCorkscrewUri);
            if (currentFolder == null)
            {
                throw new HttpException(404, "Folder cannot be found. Perhaps it was deleted, or you no longer have access to it. Contact your site or farm administrator if you believe this should not be the case.");
            }

            if (!IsPostBack)
            {
                lvFileSystemView.DataSource = currentFolder.Items
                                                .OrderByDescending(i => i.IsFolder)
                                                    .OrderBy(i => i.FilenameExtension)
                                                        .OrderBy(i => i.Filename);
                lvFileSystemView.DataBind();
            }
        }

    }
}