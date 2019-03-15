using System;
using System.Collections.Generic;

namespace CMS.Setup.Installers
{
    /// <summary>
    /// Defines an installer class that raises events to notify of progress changes
    /// </summary>
    public interface IProgressableInstaller
    {

        /// <summary>
        /// Status of the last installation, repair or uninstallation attempt
        /// </summary>
        LastActionState LastStatus { get; }

        /// <summary>
        /// Undo commands filed by the installer
        /// </summary>
        Stack<Action> UndoCommands { get; }

        /// <summary>
        /// Notification for progress changes
        /// </summary>
        event ProgressChangedEventHandler ProgressChanged;

        /// <summary>
        /// Installs a component
        /// </summary>
        /// <returns>True if successful, False if failed (Caller may rollback on failure)</returns>
        bool Install();

        /// <summary>
        /// Repairs a component
        /// </summary>
        /// <returns>True if successful, False if failed (Caller may rollback on failure)</returns>
        bool Repair();

        /// <summary>
        /// Uninstalls a component
        /// </summary>
        void Uninstall();

    }

    /// <summary>
    /// Progress changed event delegate
    /// </summary>
    /// <param name="value">Value of progress, will be from 1 to 100</param>
    /// <param name="text">Text progress</param>
    public delegate void ProgressChangedEventHandler(int value, string text);
}
