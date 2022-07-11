using CSharpComicViewerLib.Service;
using CSharpComicViewerLib.ViewModel;
using CSharpComicViewerLib.ViewModel.Mocks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Mvvm.DependencyInjection;

namespace CSharpComicViewer.Service
{
    public class ServiceLocator
    {
        public ServiceLocator()
        {
            var services = new ServiceCollection();

            services.AddTransient<IComicService, ComicService>();
            services.AddTransient<IDataStorageService, DataStorageService>();
            services.AddTransient<ILegacyConfigurationMigrationService, LegacyConfigurationMigrationService>();
            services.AddTransient<ITranslationService, TranslationService>();
            services.AddTransient<IApplicationService, ApplicationService>();
           
            services.AddSingleton<MainViewModel>();
            services.AddTransient<BookmarkManagerViewModel>();

#if DEBUG
            if (App.IsInDesignMode)
            {
                services.AddTransient<IAboutViewModel, MockedAboutViewModel>();
            }
            else
            {
                services.AddTransient<IAboutViewModel, AboutViewModel>();
            }
#endif
#if !DEBUG
                services.AddTransient<IAboutViewModel, AboutViewModel>();
#endif
            Ioc.Default.ConfigureServices(services.BuildServiceProvider());
        }
    }
}