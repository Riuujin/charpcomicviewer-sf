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
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Threading;
    using Csharp_comicviewer.Comic;
    using Csharp_comicviewer.Events;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.IO;

    /// <summary>
    /// Interaction logic for Window2.xaml
    /// </summary>
    public partial class Display_Form : Window
    {
        public Display_Form(string OpeningFile)
        {
            InitializeComponent();
            this.OpeningFile = OpeningFile;
            PrimaryDisplay = this;
        }
        
        public Display_Form(Display_Form Creator, ComicBook ComicBook)
        {
            InitializeComponent();
            SecondaryDisplay = this;
            PrimaryDisplay = Creator;
            this.comicBook = ComicBook;
            
        }
        
        private string OpeningFile
        {
            get;
            set;
        }
        
        
        private void Display_Form_Loaded(object sender, RoutedEventArgs e)
        {
            if(PrimaryDisplay == this)
            {
                Mouse = new MouseHandler(this, comicBook);
                Keyboard = new KeyboardHandler(this,comicBook,ImageEdit, Mouse);
            }
            
            //Load config
            LoadConfiguration();
            SetBookmarkMenu();
            
            //set window mode
            if (Configuration.windowed)
            {
                this.WindowState = System.Windows.WindowState.Maximized;
                MenuBar.Visibility = Visibility.Visible;
            }
            else //if fullscreen
            {
                this.WindowStyle = System.Windows.WindowStyle.None;
                this.WindowState = System.Windows.WindowState.Maximized;
            }

            //gray out resume last file if the files dont't exist
            if (Configuration.Resume != null)
            {
                foreach (String file in Configuration.Resume.Files)
                {
                    if (!File.Exists(file))
                    {
                        ResumeFile_MenuBar.IsEnabled = false;
                        ResumeFile_RightClick.IsEnabled = false;
                    }
                }
            }
            else
            {
                ResumeFile_MenuBar.IsEnabled = false;
                ResumeFile_RightClick.IsEnabled = false;
            }

            if (!Configuration.windowed)
                ScreenHeight = grid.Height;
            else
                ScreenHeight = grid.Height - 38;
            ScreenWidth = this.Width;
            scrollValueHorizontal = (int)(ScreenHeight * 0.05);
            scrollValueVertical = (int)(ScreenWidth * 0.05);
            ImageEdit.SetScreenHeight(ScreenHeight);
            ImageEdit.SetScreenWidth(ScreenWidth);

            
            
            //MouseHandler = new Csharp_comicviewer.MouseHandler(this);
            
            
            //open file (when opening assosicated by double click)
            if (OpeningFile != null)
            {
                LoadAndDisplayComic(false);
            }
        }
        
        public void ApplicationExit(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }
        
        public void DisplayImage(byte[] image)
        {
            BitmapImage bitmapimage = new BitmapImage();
            bitmapimage.BeginInit();
            MemoryStream stream = new MemoryStream(image);
            bitmapimage.StreamSource = stream;
            bitmapimage.EndInit();
            
            DisplayedImage.Source = bitmapimage;
            
            DisplayedImage.Width = bitmapimage.PixelWidth;
            DisplayedImage.Height = bitmapimage.PixelHeight;
            
            if(DisplayedImage.Width < grid.Width)
            {
                DisplayedImage.HorizontalAlignment = HorizontalAlignment.Center;
            }
            else if(DisplayedImage.Width >= grid.Width)
            {
                DisplayedImage.HorizontalAlignment = HorizontalAlignment.Left;
            }
            if(DisplayedImage.Height < grid.Height)
            {
                DisplayedImage.VerticalAlignment = VerticalAlignment.Center;
            }
            else if(DisplayedImage.Height >= grid.Height)
            {
                DisplayedImage.VerticalAlignment = VerticalAlignment.Top;
            }
        }
        
        #region Keyboard & Mouse
        private void Display_Form_KeyDown(object sender, KeyEventArgs e)
        {
            
            if(e.Key == Key.X)
            {
                Application.Current.Shutdown();
            }
            else
            {
                ShowMessage("test");
                ShowPageInformation();
            }
        }
        
        private void Display_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            throw new NotImplementedException();
        }
        #endregion
        
        #region Menus
        private void Resume_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
        
        private void Load_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
        
        private void NextPage_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
        
        private void PreviousPage_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
        
        private void NextFile_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
        
        private void PreviousFile_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
        
        private void ShowPageInformation_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
        
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
        
        private void AddBookmark_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
        
        private void ManageBookmarks_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
        
        private void About_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Messages and Page information
        private DispatcherTimer ShowMessageTimer
        {
            get;
            set;
        }
        
        private DispatcherTimer PageInformationTimer
        {
            get;
            set;
        }
        
        public void ShowMessage(String Message)
        {
            MessageBox.Text = Message;
            MessageBox.Visibility = System.Windows.Visibility.Visible;
            
            if(ShowMessageTimer != null)
            {
                ShowMessageTimer.Stop();
            }
            
            ShowMessageTimer = new DispatcherTimer();
            ShowMessageTimer.Tick += new EventHandler(HideMessage);
            ShowMessageTimer.Interval = new TimeSpan(0, 0, 2);
            ShowMessageTimer.Start();
            
        }

        private void HideMessage(object sender, EventArgs e)
        {
            
            MessageBox.Visibility = System.Windows.Visibility.Hidden;
        }
        
        public void ShowPageInformation()
        {
            if(ComicBook != null)
            {
                PageInfoBox.Text = "Archive" + (Convert.ToInt32(ComicBook.GetCurrentFile()) + 1) + "/" + ComicBook.GetTotalFiles() + "\r\n Page: " + (Convert.ToInt32(ComicBook.GetCurrentPageOfTotal()) + 1) + "/" + ComicBook.GetTotalPages();
                PageInfoBox.Visibility = System.Windows.Visibility.Visible;

                if(PageInformationTimer != null)
                {
                    PageInformationTimer.Stop();
                }
                
                PageInformationTimer = new DispatcherTimer();
                PageInformationTimer.Tick += new EventHandler(HidePageInformation);
                PageInformationTimer.Interval = new TimeSpan(0, 0, 5);
                PageInformationTimer.Start();
            }
        }
        
        private void HidePageInformation(object sender, EventArgs e)
        {
            
            PageInfoBox.Visibility = System.Windows.Visibility.Hidden;
        }
        #endregion
        
        /// <summary>
        /// Go to next page
        /// </summary>
        public void NextPage()
        {
            if (ComicBook.GetTotalFiles() != 0)
            {
                byte[] image = ComicBook.NextPage();
                if (image != null)
                {
                    DisplayImage(image);
                }
            }
        }
        
        /// <summary>
        /// Go to previous page
        /// </summary>
        public void PreviousPage()
        {
            if (ComicBook.GetTotalFiles() != 0)
            {
                byte[] image = ComicBook.PreviousPage();
                if (image != null)
                {
                    DisplayImage(image);
                }
            }
        }
        
        
        /// <summary>
        /// The primary display form
        /// </summary>
        private Display_Form PrimaryDisplay
        {
            get;
            set;
        }
        
        private Display_Form _SecondaryDisplay;
        
        /// <summary>
        /// The optional secondary display form
        /// </summary>
        private Display_Form SecondaryDisplay
        {
            get
            {
                return _SecondaryDisplay;
            }
            set
            {
                _SecondaryDisplay = value;
                if(value != null)
                {
                    SecondaryDisplayExists = true;
                }
                else
                {
                    SecondaryDisplayExists = false;
                }
            }
        }
        
        public bool SecondaryDisplayExists
        {
            get;
            private set;
        }
        
        public KeyboardHandler Keyboard
        {
            get;
            set;
        }
        
        public MouseHandler Mouse
        {
            get;
            set;
        }
        
        public ComicBook ComicBook
        {
            get;
            set;
        }
        
        public bool GoToNextPage
        {
            get;
            set;
        }
        
        public bool GoToPreviousPage
        {
            get;
            set;
        }

        public int NextPageCount
        {
            get;
            set;
        }
        
        public int PreviousPageCount
        {
            get;
            set;
        }
    }
}