using CSharpComicViewerLib.Service;
using System.Windows;
using System.Windows.Input;

namespace CSharpComicViewer
{
    /// <summary>
    /// Interaction logic for Main.xaml
    /// </summary>
    public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			var ws = CommonServiceLocator.ServiceLocator.Current.GetInstance<IWindowService>();
			ws.SetMainWindow(this);
		}

		private void OnMouseWheel(object sender, MouseWheelEventArgs e)
		{
			pageViewer.OnMouseWheel(sender, e);
		}
	}
}
