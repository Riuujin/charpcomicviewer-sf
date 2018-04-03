using CSharpComicViewer.Data;
using CSharpComicViewer.Service;
using CSharpComicViewer.ViewModel;
using System.Windows;

namespace CSharpComicViewer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static ServiceLocator serviceLocator;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            this.DispatcherUnhandledException += App_DispatcherUnhandledException;
            serviceLocator = new Service.ServiceLocator();

            var mainWindow = new MainWindow();

            var ws = CommonServiceLocator.ServiceLocator.Current.GetService(typeof(IWindowService)) as IWindowService;
            ws.SetMainWindow(mainWindow);

            var mv = CommonServiceLocator.ServiceLocator.Current.GetInstance<MainViewModel>();
            mv.LoadFromStorage();

            mainWindow.Show();
        }

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            var mv = CommonServiceLocator.ServiceLocator.Current.GetInstance<MainViewModel>();
            mv.HandleException(e.Exception);
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            var mv = CommonServiceLocator.ServiceLocator.Current.GetInstance<MainViewModel>();
            mv.SaveToStorage();
        }

        
    }
}