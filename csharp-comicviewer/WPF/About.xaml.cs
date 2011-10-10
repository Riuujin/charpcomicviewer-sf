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

namespace Csharp_comicviewer.WPF
{
    using System;
    using System.Diagnostics;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Documents;
    using System.Windows.Markup;

    /// <summary>
    /// Interaction logic for About.xaml
    /// </summary>
    public partial class About : Window
    {
        public About()
        {
            InitializeComponent();
            SetDescription();
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {

            // open URL

            Hyperlink source = sender as Hyperlink;

            if (source != null)
            {

                System.Diagnostics.Process.Start(source.NavigateUri.ToString());

            }

        }

        public string ProgramName
        {
            get
            {
                return ((AssemblyTitleAttribute)AssemblyTitleAttribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(AssemblyTitleAttribute))).Title;
            }
        }

        public string Version
        {
            get
            {
                return String.Format("Version: {0}", FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion);
            }
        }

        public void SetDescription()
        {
                Description_TextBox.Text=  ((AssemblyDescriptionAttribute)AssemblyDescriptionAttribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(AssemblyDescriptionAttribute))).Description;
        }

        public string Copyright
        {
            get
            {
                return ((AssemblyCopyrightAttribute)AssemblyCopyrightAttribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(AssemblyCopyrightAttribute))).Copyright;
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();          
        }

    }
}
