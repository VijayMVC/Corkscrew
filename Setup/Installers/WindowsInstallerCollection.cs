using Corkscrew.SDK.objects;
using System;
using System.Linq;

namespace CMS.Setup.Installers
{

    /// <summary>
    /// Collection of WindowsInstaller installers
    /// </summary>
    public class WindowsInstallerCollection : CSBaseCollection<WindowsInstaller>
    {

        #region Constructors

        /// <summary>
        /// Constructor, creates a writable collection
        /// </summary>
        public WindowsInstallerCollection() : base(false) { }

        #endregion

        #region Methods

        /// <summary>
        /// Adds the given installer to the collection. Installer must be uniquely defined in the collection (same definition cannot be added twice).
        /// </summary>
        /// <param name="installer">Installer to add</param>
        public void Add(WindowsInstaller installer)
        {
            if (Collection.Contains(installer))
            {
                throw new ArgumentException("There is already an installer matching this definition in the collection");
            }

            AddInternal(installer);
        }

        #endregion

    }
}
