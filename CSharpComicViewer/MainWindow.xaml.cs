using CSharpComicViewerLib.Service;
using CSharpComicViewerLib.ViewModel;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using System.Windows;
using System.Windows.Input;

namespace CSharpComicViewer
{
    /// <summary>
    /// Interaction logic for Main.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            DataContext = Ioc.Default.GetRequiredService<MainViewModel>();

            var ws = Ioc.Default.GetRequiredService<IApplicationService>();
            ws.SetMainWindow(this);
        }

        /// <summary>
        /// Called when mouse wheel is used.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="MouseWheelEventArgs"/> instance containing the event data.</param>
        private void OnMouseWheel(object sender, MouseWheelEventArgs e)
        {
            pageViewer.OnMouseWheel(sender, e);
        }
    }
}
