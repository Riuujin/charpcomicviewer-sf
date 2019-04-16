using CSharpComicViewerLib.ViewModel;
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
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<AboutViewModel>();
            SimpleIoc.Default.Register<BookmarkManagerViewModel>();
        }

        /// <summary>
        /// Gets the main view model.
        /// </summary>
        /// <value>
        /// The main view model.
        /// </value>
        public MainViewModel Main
        {
            get
            {
                return SimpleIoc.Default.GetInstance<MainViewModel>();
            }
        }

        /// <summary>
        /// Gets the about view model.
        /// </summary>
        /// <value>
        /// The about view model.
        /// </value>
        public IAboutViewModel About
        {
            get
            {
#if DEBUG
                if (App.IsInDesignMode)
                {
                    return new CSharpComicViewerLib.ViewModel.Mocks.MockedAboutViewModel();
                }
#endif
                return SimpleIoc.Default.GetInstance<AboutViewModel>();
            }
        }

        /// <summary>
        /// Gets the bookmark manager.
        /// </summary>
        /// <value>
        /// The bookmark manager.
        /// </value>
        public BookmarkManagerViewModel BookmarkManager
        {
            get
            {
                return SimpleIoc.Default.GetInstance<BookmarkManagerViewModel>();
            }
        }
    }
}