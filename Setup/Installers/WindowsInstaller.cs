using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace CMS.Setup.Installers
{
    /// <summary>
    /// Installs items to the Windows system.
    /// This is just a base class. Classes like the RegistryInstaller and ShortcutInstaller inherit from this class.
    /// </summary>
    public class WindowsInstaller : IProgressableInstaller
    {

        #region Properties

        /// <summary>
        /// Status of the last installation, repair or uninstallation attempt
        /// </summary>
        public LastActionState LastStatus
        {
            get;
            protected set;
        } = LastActionState.NotExecuted;

        private EventHandlerList EventHandlers
        {
            get
            {
                lock (eventSystemLock)
                {
                    if (_eventHandlersList == null)
                    {
                        _eventHandlersList = new EventHandlerList();
                    }
                }

                return _eventHandlersList;
            }
        }
        private EventHandlerList _eventHandlersList = null;

        /// <summary>
        /// The progress changed event
        /// </summary>
        public event ProgressChangedEventHandler ProgressChanged
        {
            add
            {
                lock (eventSystemLock)
                {
                    EventHandlers.AddHandler(ProgressChangedEvent, value);
                }
            }
            remove
            {
                lock (eventSystemLock)
                {
                    EventHandlers.RemoveHandler(ProgressChangedEvent, value);
                }
            }
        }
        private object ProgressChangedEvent = new object();
        private object eventSystemLock = new object();

        /// <summary>
        /// Undo commands filed by the installer
        /// </summary>
        public Stack<Action> UndoCommands
        {
            get
            {
                if (_undoList == null)
                {
                    _undoList = new Stack<Action>();
                }
                return _undoList;
            }
        }
        private Stack<Action> _undoList = null;

        #endregion

        #region Constructors

        /// <summary>
        /// Blank constructor
        /// </summary>
        protected WindowsInstaller()
        {
            LastStatus = LastActionState.NotExecuted;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Installs the items to Windows
        /// </summary>
        /// <returns>True if installation succeeded</returns>
        public virtual bool Install()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Repairs the items in Windows
        /// </summary>
        /// <returns>True if repair succeeded</returns>
        public virtual bool Repair()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Uninstalls items from Windows
        /// </summary>
        public virtual void Uninstall()
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// Event handler for progress changed event of a child installer
        /// </summary>
        /// <param name="value">Progress bar increment value</param>
        /// <param name="message">The progress message</param>
        protected void OnProgressChanged(int value, string message)
        {
            ((ProgressChangedEventHandler)EventHandlers[ProgressChangedEvent])?.Invoke(value, message);
        }

        /// <summary>
        /// Resolve all path properties in the installer
        /// </summary>
        /// <param name="environment">Key is special variable name, Value is value of that variable</param>
        /// <remarks>Variables are: 
        ///  $(InstallSource) - will be Application.StartupPath + "_layout" (absolute path)
        ///  $(InstallTargetRoot) - OperationManifest.InstallBaseDirectory (absolute path)
        ///  $(ComponentSource) - the particular path within $(InstallSource) where the files for this component are located (absolute path)
        ///  $(ComponentDestination) - ComponentInstaller.InstallFolderName (relative path)
        ///  $(ComponentName) - name of the current component from the manifest Xml
        /// </remarks>
        public virtual void ResolvePathProperties(Dictionary<string, string> environment)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
