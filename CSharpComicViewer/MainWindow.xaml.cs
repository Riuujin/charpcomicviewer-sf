using CSharpComicViewerLib.Service;
using GalaSoft.MvvmLight.Ioc;
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
			var ws = SimpleIoc.Default.GetInstance<IApplicationService>();
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
