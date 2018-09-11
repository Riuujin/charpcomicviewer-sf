﻿using CSharpComicViewerLib.Data;
using CSharpComicViewerLib.Service;
using Microsoft.Win32;
using System.Configuration;
using System.Diagnostics;
using System.Reflection;
using System.Windows;

namespace CSharpComicViewer.Service
{
    class ApplicationService : IApplicationService
    {
        private const string GITHUB_URL = "http://riuujin.github.io/charpcomicviewer-sf";
        private const string LATEST_VERSION_URL = "https://api.github.com/repos/Riuujin/charpcomicviewer-sf/releases/latest";
        private Assembly assembly;
        private Window mainWindow;
        private string _fileLoaderFilter;

        public void ApplicationShutdown()
        {
            Application.Current.Shutdown();
        }

        public string GetFileVersion()
        {
            return FileVersionInfo.GetVersionInfo(GetAssembly().Location).FileVersion;
        }

        private Assembly GetAssembly()
        {
            if (assembly == null)
            {
                assembly = Assembly.GetExecutingAssembly();
            }
            return assembly;
        }

        public string GetProgramName()
        {
            return ((AssemblyTitleAttribute)AssemblyTitleAttribute.GetCustomAttribute(GetAssembly(), typeof(AssemblyTitleAttribute))).Title;
        }

        public string GetCopyright()
        {
            return ((AssemblyCopyrightAttribute)AssemblyTitleAttribute.GetCustomAttribute(GetAssembly(), typeof(AssemblyCopyrightAttribute))).Copyright;
        }

        public string GetGitHubUrl()
        {
            return GITHUB_URL;
        }

        public string GetLatestVersionUrl()
        {
            return LATEST_VERSION_URL;
        }


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

        public string[] OpenFileDialog(string initialPath)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = fileLoaderFilter;
            openFileDialog.Multiselect = true;

            openFileDialog.ShowDialog();

            return openFileDialog.FileNames;
        }

        /// <summary>
        /// Sets the main window.
        /// </summary>
        /// <param name="window">The window.</param>
        public void SetMainWindow(object window)
        {
            this.mainWindow = window as Window;
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
            if (mainWindow == null)
            {
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

        private string fileLoaderFilter
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_fileLoaderFilter))
                {
                    _fileLoaderFilter = "";

                    //Add all supported to filter
                    _fileLoaderFilter += "Supported formats|";

                    foreach (string extension in SupportedExtensions.SupportedArchives)
                    {
                        _fileLoaderFilter += "*." + extension + ";";
                    }

                    foreach (string extension in SupportedExtensions.SupportedImages)
                    {
                        _fileLoaderFilter += "*." + extension + ";";
                    }

                    //Add separator
                    _fileLoaderFilter += "|";

                    //Add Archives to filter
                    _fileLoaderFilter += "Supported archive formats (";

                    foreach (string extension in SupportedExtensions.SupportedArchives)
                    {
                        _fileLoaderFilter += "*." + extension + ";";
                    }

                    _fileLoaderFilter += ")|";

                    foreach (string extension in SupportedExtensions.SupportedArchives)
                    {
                        _fileLoaderFilter += "*." + extension + ";";
                    }

                    //Add separator
                    _fileLoaderFilter += "|";

                    //Add Images to filter
                    _fileLoaderFilter += "Supported image formats (";

                    foreach (string extension in SupportedExtensions.SupportedImages)
                    {
                        _fileLoaderFilter += "*." + extension + ";";
                    }

                    _fileLoaderFilter += ")|";

                    foreach (string extension in SupportedExtensions.SupportedImages)
                    {
                        _fileLoaderFilter += "*." + extension + ";";
                    }

                    //Add *.*
                    _fileLoaderFilter += "|All files (*.*)|*.*";

                }

                return _fileLoaderFilter;
            }
        }
    }
}
