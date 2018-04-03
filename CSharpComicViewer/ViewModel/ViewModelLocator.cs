using CommonServiceLocator;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;

namespace CSharpComicViewer.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            if (!ServiceLocator.IsLocationProviderSet)
            {
                ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            }
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<AboutViewModel>();
            SimpleIoc.Default.Register<BookmarkManagerViewModel>();
        }

        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        public AboutViewModel About
        {
            get
            {
                return ServiceLocator.Current.GetInstance<AboutViewModel>();
            }
        }

        public BookmarkManagerViewModel BookmarkManager
        {
            get
            {
                return ServiceLocator.Current.GetInstance<BookmarkManagerViewModel>();
            }
        }
    }
}