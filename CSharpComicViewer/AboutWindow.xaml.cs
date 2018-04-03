using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
            var viewModel = CommonServiceLocator.ServiceLocator.Current.GetInstance<ViewModel.AboutViewModel>();
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
