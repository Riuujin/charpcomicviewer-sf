using CSharpComicViewerLib.Service;
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
            SimpleIoc.Default.Register<IComicService, ComicService>();
            SimpleIoc.Default.Register<IDataStorageService, DataStorageService>();
            SimpleIoc.Default.Register<ILegacyConfigurationMigrationService, LegacyConfigurationMigrationService>();
            SimpleIoc.Default.Register<ITranslationService, TranslationService>();
            SimpleIoc.Default.Register<IApplicationService, ApplicationService>();
        }
    }
}