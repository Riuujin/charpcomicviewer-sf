using System;
using System.Windows;

namespace CSharpComicViewer.Service
{
    public class WindowService : IWindowService
    {
        private Window mainWindow;

        /// <summary>
        /// Confirms the specified confirmation text.
        /// </summary>
        /// <param name="messageBoxText">The message box text.</param>
        /// <param name="caption">The caption.</param>
        /// <returns>
        ///   <c>true</c> if confirmed; otherwise <c>false</c>.
        /// </returns>
        public bool Confirm(string messageBoxText, string caption)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show(messageBoxText, caption, System.Windows.MessageBoxButton.YesNo);
            return messageBoxResult == MessageBoxResult.Yes;
        }

        /// <summary>
        /// Opens the about window.
        /// </summary>
        public void OpenAboutWindow()
        {
            var window = new AboutWindow();
            window.Owner = mainWindow;
            window.ShowDialog();
        }

        /// <summary>
        /// Opens the bookmark manager window.
        /// </summary>
        public void OpenBookmarkManagerWindow()
        {
            var window = new BookmarkManager();
            window.Owner = mainWindow;
            window.ShowDialog();
        }

        /// <summary>
        /// Sets the main window.
        /// </summary>
        /// <param name="window">The window.</param>
        public void SetMainWindow(Window window)
        {
            this.mainWindow = window;
        }

        /// <summary>
        /// Toggles the fullscreen.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if set to fullscreen;otherwise <c>false</c>.
        /// </returns>
        /// <remarks>
        /// This is done for the main window.
        /// </remarks>
        public bool ToggleFullscreen()
        {
            if (mainWindow == null) {
                return false;
            }

            if (mainWindow.WindowStyle == WindowStyle.None)
            {
                mainWindow.WindowStyle = WindowStyle.SingleBorderWindow;
                mainWindow.ResizeMode = ResizeMode.CanResize;
                mainWindow.WindowState = System.Windows.WindowState.Normal;
                return false;
            }
            else
            {
                mainWindow.WindowStyle = WindowStyle.None;
                mainWindow.ResizeMode = ResizeMode.NoResize;
                mainWindow.WindowState = System.Windows.WindowState.Maximized;
                return true;
            }
        }
    }
}
