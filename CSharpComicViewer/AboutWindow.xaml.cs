using CSharpComicViewerLib.ViewModel;
using System.Windows;
using System.Windows.Documents;

namespace CSharpComicViewer
{
    /// <summary>
    /// Interaction logic for AboutWindow.xaml
    /// </summary>
    public partial class AboutWindow : Window
    {
        public AboutWindow()
        {
            InitializeComponent();
            var viewModel = CommonServiceLocator.ServiceLocator.Current.GetInstance<AboutViewModel>();
            if (viewModel.LatestVersion == null)
            {
                viewModel.CheckUpdateCommand.Execute(null);
            }
        }

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
