using CSharpComicViewerLib.ViewModel;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using System.Windows;

namespace CSharpComicViewer
{
    /// <summary>
    /// Interaction logic for BookmarkManager.xaml
    /// </summary>
    public partial class BookmarkManager : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BookmarkManager"/> class.
        /// </summary>
        public BookmarkManager()
        {
            InitializeComponent();
            DataContext = Ioc.Default.GetRequiredService<BookmarkManagerViewModel>();
        }

        /// <summary>
        /// Handles the Click event of the Close control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
