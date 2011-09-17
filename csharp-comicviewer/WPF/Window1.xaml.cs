/*
  Copyright 2011 Rutger Spruyt
  
  This file is part of C# Comicviewer.

  csharp comicviewer is free software: you can redistribute it and/or modify
  it under the terms of the GNU General Public License as published by
  the Free Software Foundation, either version 3 of the License, or
  (at your option) any later version.

  csharp comicviewer is distributed in the hope that it will be useful,
  but WITHOUT ANY WARRANTY; without even the implied warranty of
  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
  GNU General Public License for more details.

  You should have received a copy of the GNU General Public License
  along with csharp comicviewer.  If not, see <http://www.gnu.org/licenses/>.
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using Microsoft.Win32;
using Csharp_comicviewer.Comic;

namespace Csharp_comicviewer.WPF
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        
        ComicBook cb;
        int i = 0;
        public Window1()
        {
            InitializeComponent();
        }
        
        void scrollViewer1_MouseWheel(object sender, MouseWheelEventArgs e)
        {
             if(e.Delta > 0 && scrollViewer1.VerticalOffset == 0 && scrollViewer1.HorizontalOffset == 0)
            {
                DisplayImage(cb.GetPage(0,--i));
                scrollViewer1.ScrollToVerticalOffset(0);
                scrollViewer1.ScrollToHorizontalOffset(0);
            }
            
            if(e.Delta < 0 && scrollViewer1.VerticalOffset == scrollViewer1.ScrollableHeight  && scrollViewer1.HorizontalOffset == scrollViewer1.ScrollableWidth)
            {
                DisplayImage(cb.GetPage(0,++i));
                scrollViewer1.ScrollToVerticalOffset(0);
                scrollViewer1.ScrollToHorizontalOffset(0);
            }
        }
        
        
        public bool DisplayImage(byte[] imageAsBytes)
        {
            BitmapImage bitmapimage = new BitmapImage();
            bitmapimage.BeginInit();
            MemoryStream stream = new MemoryStream(imageAsBytes);
            bitmapimage.StreamSource = stream;
            bitmapimage.EndInit();
            
            image.Source = bitmapimage;
            
            image.Width = bitmapimage.PixelWidth;
            image.Height = bitmapimage.PixelHeight;
            
            
            return true;
        }
        
        void window1_Loaded(object sender, RoutedEventArgs e)
        {
            this.WindowStyle = System.Windows.WindowStyle.None;
            this.WindowState = System.Windows.WindowState.Maximized;
        }
        
        void window1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.L)
            {
                OpenFileDialog OpenFileDialog = new OpenFileDialog();
                OpenFileDialog.ShowDialog();
                String[] Files = OpenFileDialog.FileNames;
                FileLoader Archives = new FileLoader();
                Archives.Load(Files);
                cb = Archives.ComicBook;
                DisplayImage(cb.GetPage(0,0));
            }
        }
        
        void scrollViewer1_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            scrollViewer1_MouseWheel(sender,e);
        }
    }
}