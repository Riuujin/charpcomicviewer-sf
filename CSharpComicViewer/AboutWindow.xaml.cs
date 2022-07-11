using CSharpComicViewerLib.ViewModel;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using System.Windows;
using System.Windows.Documents;

namespace CSharpComicViewer
{
    /// <summary>
    /// Interaction logic for AboutWindow.xaml
    /// </summary>
    public partial class AboutWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AboutWindow"/> class.
        /// </summary>
        public AboutWindow()
        {
            InitializeComponent();
            var viewModel = Ioc.Default.GetRequiredService<AboutViewModel>();
            DataContext = viewModel;
            if (viewModel.LatestVersion == null)
            {
                viewModel.CheckUpdateCommand.Execute(null);
            }
        }

        /// <summary>
        /// Handles the Click event of the Hyperlink control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            // open URL
            if (sender  is Hyperlink source)
            {
                System.Diagnostics.Process.Start(source.NavigateUri.ToString());
            }
        }
    }
}
