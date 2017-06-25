using System.Windows;

namespace CSharpComicViewer.Service
{
    public interface IWindowService
    {
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
        /// Opens the about window.
        /// </summary>
        void OpenAboutWindow();
    }
}