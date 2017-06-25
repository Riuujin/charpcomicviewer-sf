using System;
using System.Windows;

namespace CSharpComicViewer.Service
{
    public class WindowService : IWindowService
    {
        private Window window;

        public void OpenAboutWindow()
        {
            var window = new AboutWindow();
            window.ShowDialog();
        }

        /// <summary>
        /// Sets the main window.
        /// </summary>
        /// <param name="window">The window.</param>
        public void SetMainWindow(Window window)
        {
            this.window = window;
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
            if (window == null) {
                return false;
            }

            if (window.WindowStyle == WindowStyle.None)
            {
                window.WindowStyle = WindowStyle.SingleBorderWindow;
                window.ResizeMode = ResizeMode.CanResize;
                window.WindowState = System.Windows.WindowState.Normal;
                return false;
            }
            else
            {
                window.WindowStyle = WindowStyle.None;
                window.ResizeMode = ResizeMode.NoResize;
                window.WindowState = System.Windows.WindowState.Maximized;
                return true;
            }
        }
    }
}
