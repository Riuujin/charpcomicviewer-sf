using CSharpComicViewer.Service;
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
	/// Interaction logic for Main.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			var ws = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetService(typeof(IWindowService)) as IWindowService;
			ws.SetMainWindow(this);
		}

		private void OnMouseWheel(object sender, MouseWheelEventArgs e)
		{
			pageViewer.OnMouseWheel(sender, e);
		}
	}
}
