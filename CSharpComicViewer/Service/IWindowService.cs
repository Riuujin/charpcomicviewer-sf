using System.Windows;

namespace CSharpComicViewer.Service
{
    public interface IWindowService
    {
        /// <summary>
        /// Opens the about window.
        /// </summary>
        void OpenAboutWindow();

        /// <summary>
        /// Opens the bookmark manager window.
        /// </summary>
        void OpenBookmarkManagerWindow();

        /// <summary>
        /// Sets the main window.
        /// </summary>
        /// <param name="window">The window.</param>
        void SetMainWindow(Window window);

        /// <summary>
        /// Toggles the fullscreen.
        /// </summary>
        /// <remarks>
        /// This is done for the main window.
        /// </remarks>
        /// <returns><c>true</c> if set to fullscreen;otherwise <c>false</c>.</returns>
        bool ToggleFullscreen();

        /// <summary>
        /// Confirms the specified confirmation text.
        /// </summary>
        /// <param name="messageBoxText">The message box text.</param>
        /// <param name="caption">The caption.</param>
        /// <returns>
        ///   <c>true</c> if confirmed; otherwise <c>false</c>.
        /// </returns>
        bool Confirm(string messageBoxText, string caption);
    }
}