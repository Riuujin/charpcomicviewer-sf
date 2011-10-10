//-------------------------------------------------------------------------------------
//  Copyright 2011 Rutger Spruyt
//
//  This file is part of C# Comicviewer.
//
//  csharp comicviewer is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  csharp comicviewer is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with csharp comicviewer.  If not, see <http://www.gnu.org/licenses/>.
//-------------------------------------------------------------------------------------

using System;
using System.Windows;
using System.Data;
using System.Xml;
using System.Configuration;
using Csharp_comicviewer.WPF;

namespace Csharp_comicviewer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            
            if (e.Args.Length > 0)
            {
                var mainWindow = new Csharp_comicviewer.WPF.MainDisplay(e.Args[0]);
                mainWindow.Show();
            }
            else
            {
                var mainWindow = new Csharp_comicviewer.WPF.MainDisplay(null);
                mainWindow.Show();
            }
        }
    }
}