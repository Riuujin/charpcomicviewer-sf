using CSharpComicViewer.Service;
using CSharpComicViewerLib.Data;
using CSharpComicViewerLib.Service;
using CSharpComicViewerLib.ViewModel;
using System.Windows;

namespace CSharpComicViewer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static ServiceLocator serviceLocator;

        /// <summary>
        /// Handles the DispatcherUnhandledException event of the App control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Threading.DispatcherUnhandledExceptionEventArgs"/> instance containing the event data.</param>
        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            var mv = CommonServiceLocator.ServiceLocator.Current.GetInstance<MainViewModel>();
            mv.HandleException(e.Exception);
        }

        /// <summary>
        /// Handles the Exit event of the Application control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ExitEventArgs"/> instance containing the event data.</param>
        private void Application_Exit(object sender, ExitEventArgs e)
        {
            SaveToStorage();
        }

        /// <summary>
        /// Handles the Startup event of the Application control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="StartupEventArgs"/> instance containing the event data.</param>
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            this.DispatcherUnhandledException += App_DispatcherUnhandledException;
            serviceLocator = new Service.ServiceLocator();

            var mainWindow = new MainWindow();

            var ws = CommonServiceLocator.ServiceLocator.Current.GetInstance<IApplicationService>();
            ws.SetMainWindow(mainWindow);

            var mv = CommonServiceLocator.ServiceLocator.Current.GetInstance<MainViewModel>();
            LoadFromStorage(mv);

            mainWindow.Show();
        }

        /// <summary>
        /// Loads the initial data from storage and sets the data on the <see cref="MainViewModel"/>.
        /// </summary>
        /// <param name="mv">The mv.</param>
        private void LoadFromStorage(MainViewModel mv)
        {
            var service = CommonServiceLocator.ServiceLocator.Current.GetInstance<IDataStorageService>();

            var state = service.Load<State>("state");
            var resumeData = service.Load<Bookmark>("resumeData");
            var bookmarks = service.Load<Bookmark[]>("bookmarks");

            if (state == null && resumeData == null && bookmarks == null)
            {
                var legacyService = CommonServiceLocator.ServiceLocator.Current.GetInstance<ILegacyConfigurationMigrationService>();
                legacyService.Migrate();

                //Reload data, it might have been changed.
                resumeData = service.Load<Bookmark>("resumeData");
                bookmarks = service.Load<Bookmark[]>("bookmarks");
            }

            if (state != null)
            {
                mv.ViewMode = state.ViewMode;

                if (state.IsFullScreen)
                {
                    //Initial state will never be fullscreen, toggle to fullscreen if state requires it.
                    var ws = CommonServiceLocator.ServiceLocator.Current.GetInstance<IApplicationService>();
                    mv.IsFullscreen = ws.ToggleFullscreen();
                }
            }

            if (resumeData?.FilePaths?.Length >= 1)
            {
                bool allFilesExist = true;

                foreach (var filePath in resumeData.FilePaths)
                {
                    if (!System.IO.File.Exists(filePath))
                    {
                        allFilesExist = false;
                        break;
                    }
                }

                if (allFilesExist)
                {
                    mv.SetResumeData(resumeData);
                }
            }

            if (bookmarks?.Length > 0)
            {
                foreach (var bookmark in bookmarks)
                {
                    mv.Bookmarks.Add(bookmark);
                }
            }
        }

        /// <summary>
        /// Saves data from the <see cref="MainViewModel"/> to storage, so it can be loaded next time.
        /// </summary>
        private void SaveToStorage()
        {
            var mv = CommonServiceLocator.ServiceLocator.Current.GetInstance<MainViewModel>();
            var service = CommonServiceLocator.ServiceLocator.Current.GetInstance<IDataStorageService>();

            if (mv.Comic != null)
            {
                var resumeData = new Bookmark
                {
                    FilePaths = mv.Comic.GetFilePaths(),
                    Page = mv.PageNumber
                };

                service.Save("resumeData", resumeData);
            }

            service.Save("bookmarks", mv.Bookmarks);

            service.Save("state", new State
            {
                ViewMode = mv.ViewMode,
                IsFullScreen = mv.IsFullscreen
            });
        }
    }
}