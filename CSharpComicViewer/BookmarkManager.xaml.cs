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
    /// Interaction logic for BookmarkManager.xaml
    /// </summary>
    public partial class BookmarkManager : Window
    {
        public BookmarkManager()
        {
            InitializeComponent();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Close_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
    }
}
