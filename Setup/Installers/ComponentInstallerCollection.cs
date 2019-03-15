using Corkscrew.SDK.objects;
using System;
using System.Collections.Generic;

namespace CMS.Setup.Installers
{
    /// <summary>
    /// Collection of ComponentInstaller objects. Used within ComponentInstaller (for Dependencies) and OperationManifest.
    /// </summary>
    public class ComponentInstallerCollection : CSBaseCollection<ComponentInstaller>
    {

        #region Constructors

        /// <summary>
        /// Blank constructor, creates a writable collection
        /// </summary>
        public ComponentInstallerCollection() : base(false) { }

        /// <summary>
        /// Creates a writable collection populated from the list
        /// </summary>
        /// <param name="list">Enumerable collection to populate collection from</param>
        public ComponentInstallerCollection(IEnumerable<ComponentInstaller> list) : base(list, false) { }

        #endregion

        #region Methods

        /// <summary>
        /// Add the component installer to the collection. Installer must not be present directly or as a dependency in the collection hierarchy.
        /// </summary>
        /// <param name="installer">Installer to add</param>
        public void Add(ComponentInstaller installer)
        {
            if (IsAlreadyDependency(this, installer))
            {
                throw new ArgumentException("Component installer is already present either directly or as a dependant component.");
            }

            // if we are still here, safe to add
            AddInternal(installer);
        }

        /// <summary>
        /// Returns a flat List of components added to the collection, effectively flattening any dependency hierarchies.
        /// </summary>
        /// <returns>Flat list of components</returns>
        public List<ComponentInstaller> GetFlattenedComponentList()
        {
            List<ComponentInstaller> list = new List<ComponentInstaller>();
            GetFlattenedComponentListRecursively(this, list);
            return list;
        }

        private void GetFlattenedComponentListRecursively(ComponentInstallerCollection collection, List<ComponentInstaller> returnList)
        {
            foreach (ComponentInstaller item in collection)
            {
                // dependencies should be added first
                if (item.Dependencies.Count > 0)
                {
                    GetFlattenedComponentListRecursively(item.Dependencies, returnList);
                }

                // and then the component
                if (! ListContainsComponent(returnList, item))
                {
                    returnList.Add(item);
                }
            }
        }

        private bool ListContainsComponent(List<ComponentInstaller> list, ComponentInstaller component)
        {
            foreach(ComponentInstaller item in list)
            {
                if (item.ComponentName.Equals(component.ComponentName, StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsAlreadyDependency(ComponentInstallerCollection collection, ComponentInstaller installer)
        {
            foreach(ComponentInstaller item in collection)
            {
                if (item.ComponentName.Equals(installer.ComponentName, StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }

                if (item.Dependencies.Count > 0)
                {
                    foreach(ComponentInstaller dependency in item.Dependencies)
                    {
                        if (dependency.ComponentName.Equals(installer.ComponentName, StringComparison.InvariantCultureIgnoreCase))
                        {
                            return true;
                        }

                        if (dependency.Dependencies.Count > 0)
                        {
                            if (IsAlreadyDependency(dependency.Dependencies, installer))
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Returns if the collection contains an installer with the given name
        /// </summary>
        /// <param name="name">Name of the component (ComponentName)</param>
        /// <returns>True if found</returns>
        public bool Contains(string name)
        {
            foreach(ComponentInstaller item in Collection)
            {
                if (item.ComponentName.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Returns the component installer with the given name
        /// </summary>
        /// <param name="name">Name of the component (ComponentName)</param>
        /// <returns>ComponentInstaller if found, or NULL</returns>
        public ComponentInstaller Find(string name)
        {
            foreach (ComponentInstaller item in Collection)
            {
                if (item.ComponentName.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                {
                    return item;
                }
            }

            return null;
        }

        #endregion


    }
}
