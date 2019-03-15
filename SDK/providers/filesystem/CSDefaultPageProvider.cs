using Corkscrew.SDK.objects;
using Corkscrew.SDK.security;
using System;
using System.Linq;

namespace Corkscrew.SDK.providers.filesystem
{
    /// <summary>
    /// Handles the concept of a "default" page like how IIS maps a request for "/" to "/default.aspx".
    /// </summary>
    public class CSDefaultPageProvider
    {

        private IOrderedEnumerable<string> defaultPageNames = null;
        private CSConfigurationCollection configuration = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public CSDefaultPageProvider()
        {
            configuration = CSFarm.Open(CSUser.CreateSystemUser()).AllConfiguration;
            
            foreach (CSKeyValuePair pair in configuration)
            {
                if (pair.Key.Equals("Corkscrew/Farm/Defaults/DefaultPageNames", StringComparison.InvariantCultureIgnoreCase))
                {
                    defaultPageNames = pair.Value
                                                .Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
                                                    .ToList()
                                                        .OrderBy(n => n, StringComparer.InvariantCultureIgnoreCase);
                    break;
                }
            }
        }

        /// <summary>
        /// Calculates the "default" document for the given path
        /// </summary>
        /// <param name="path">The CSFileSystemEntry path</param>
        /// <returns>The default document or NULL</returns>
        public CSFileSystemEntryFile GetDefaultPageForPath(CSFileSystemEntry path)
        {
            if (path == null)
            {
                return null;
            }

            if (! path.IsFolder)
            {
                return new CSFileSystemEntryFile(path);
            }

            CSFileSystemEntryDirectory dir = new CSFileSystemEntryDirectory(path);
            IOrderedEnumerable<string> siteLocalDefaultPages = null;

            foreach (CSKeyValuePair pair in configuration)
            {
                if (pair.Key.Equals("Corkscrew/Sites/" + dir.Site.Id.ToString("d") + "/Defaults/DefaultPageNames", StringComparison.InvariantCultureIgnoreCase))
                {
                    siteLocalDefaultPages = pair.Value
                                                .Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
                                                    .ToList()
                                                        .OrderBy(n => n, StringComparer.InvariantCultureIgnoreCase);

                    foreach (string name in siteLocalDefaultPages)
                    {
                        CSFileSystemEntry entry = dir.Find(name, false, (e => (e.IsFolder == false))).FirstOrDefault();
                        if (entry != default(CSFileSystemEntry))
                        {
                            return new CSFileSystemEntryFile(entry);
                        }
                    }

                    break;
                }
            }

            // search farm-level default pages if configured
            foreach (string name in defaultPageNames)
            {
                CSFileSystemEntry entry = dir.Find(name, false, (e => (e.IsFolder == false))).FirstOrDefault();
                if (entry != default(CSFileSystemEntry))
                {
                    return new CSFileSystemEntryFile(entry);
                }
            }

            return null;
        }

    }
}
