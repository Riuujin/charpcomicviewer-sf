using GalaSoft.MvvmLight.Ioc;

namespace CSharpComicViewer.Service
{
    /// <summary>
    /// This class contains static references to all the services in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ServiceLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ServiceLocator()
        {
            if (!Microsoft.Practices.ServiceLocation.ServiceLocator.IsLocationProviderSet)
            {
                Microsoft.Practices.ServiceLocation.ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            }
            SimpleIoc.Default.Register<IComicService, ComicService>();
            SimpleIoc.Default.Register<IWindowService, WindowService>();
            SimpleIoc.Default.Register<IDataStorageService, DataStorageService>();
        }
    }
}