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
    using System.Collections;
    using System.IO;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media.Imaging;
    using System.Windows.Threading;
    using System.Xml.Serialization;
    using Csharp_comicviewer.Comic;
    using Csharp_comicviewer.Configuration;
    using Csharp_comicviewer.Events;
    using Csharp_comicviewer.Other;
    using Microsoft.Win32;


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
            this.ComicBook = ComicBook;

        }

        private string OpeningFile
        {
            get;
            set;
        }

        private void Display_Form_Loaded(object sender, RoutedEventArgs e)
        {
            if (PrimaryDisplay == this)
            {
                mouseHandler = new MouseHandler(this, ComicBook);
                keyboardHandler = new KeyboardHandler(this, ComicBook, ImageEdit, mouseHandler);
            }


            ImageEdit = new ImageEdit();
            FileLoader = new FileLoader();


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
                WindowHeight = Convert.ToInt32(this.Height);
            else
                WindowHeight = Convert.ToInt32(this.Height - 38);
            WindowWidth = Convert.ToInt32(this.Width);
            scrollValueHorizontal = (int)(WindowHeight * 0.05);
            scrollValueVertical = (int)(WindowWidth * 0.05);
            ImageEdit.SetScreenHeight(WindowHeight);
            ImageEdit.SetScreenWidth(WindowWidth);



            //MouseHandler = new Csharp_comicviewer.MouseHandler(this);


            //open file (when opening assosicated by double click)
            if (OpeningFile != null)
            {
                LoadAndDisplayComic(false);
            }
        }

        private Boolean LoadConfiguration()
        {
            //xml config load
            try
            {
                String localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                String userFilePath = Path.Combine(localAppData, "C# Comicviewer");

                XmlSerializer mySerializer = new XmlSerializer(typeof(Configuration));
                if (File.Exists(userFilePath + "\\Configuration.xml"))
                {
                    System.IO.FileStream myFileStream = new System.IO.FileStream(userFilePath + "\\Configuration.xml", System.IO.FileMode.Open);

                    Configuration = (Configuration)mySerializer.Deserialize(myFileStream);
                    myFileStream.Close();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                ShowMessage("xml config load failed:" + ex.Message);
                return false;
            }
        }

        public void ApplicationExit(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        #region Bookmarks
        private void SetBookmarkMenu()
        {
            Bookmarks_MenuRightClick.Items.Clear();
            Bookmarks_MenuRightClick.Items.Add(AddBookmark_MenuRightClick);
            Bookmarks_MenuRightClick.Items.Add(ManageBookmarks_MenuRightClick);
            Bookmarks_MenuRightClick.Items.Add(new Separator());

            Bookmarks_MenuBar.Items.Clear();
            Bookmarks_MenuBar.Items.Add(AddBookmark_MenuBar);
            Bookmarks_MenuBar.Items.Add(ManageBookmarks_MenuBar);
            Bookmarks_MenuBar.Items.Add(new Separator());

            if (Configuration != null)
            {
                if (Configuration.Bookmarks.Count > 0)
                {
                    for (int i = 0; i < Configuration.Bookmarks.Count; i++)
                    {
                        Bookmark Data = Configuration.Bookmarks[i];
                        String[] Files = Data.Files;
                        MenuItem Bookmark = new MenuItem();
                        Bookmark.Header = Data.GetCurrentFileName();
                        Bookmark.ToolTip = Files[Data.FileNumber];
                        Bookmark.Click += new RoutedEventHandler(LoadBookmark_Click);
                        Bookmarks_MenuRightClick.Items.Add(Bookmark);

                        MenuItem Bookmark_bar = new MenuItem();
                        Bookmark_bar.Header = Data.GetCurrentFileName();
                        Bookmark_bar.ToolTip = Files[Data.FileNumber];
                        Bookmark_bar.Click += new RoutedEventHandler(LoadBookmark_Click);
                        Bookmarks_MenuBar.Items.Add(Bookmark_bar);
                    }
                }
            }
        }
        private void LoadBookmark_Click(object sender, EventArgs e)
        {
            //right click menu
            ArrayList Data = new ArrayList();
            for (int i = 0; i < Bookmarks_MenuRightClick.Items.Count; i++)
            {
                if ((MenuItem)sender == Bookmarks_MenuRightClick.Items[i])
                {
                    Bookmark Bookmark = Configuration.Bookmarks[i - 3];

                    LoadAndDisplayComic(Bookmark.Files, Bookmark.FileNumber, Bookmark.PageNumber);
                }
            }

            //the bar
            for (int i = 0; i < Bookmarks_MenuBar.Items.Count; i++)
            {
                if ((MenuItem)sender == Bookmarks_MenuBar.Items[i])
                {
                    Bookmark Bookmark = Configuration.Bookmarks[i - 3];

                    LoadAndDisplayComic(Bookmark.Files, Bookmark.FileNumber, Bookmark.PageNumber);
                }
            }
        }
        #endregion

        #region Keyboard & Mouse
        private void Display_Form_KeyDown(object sender, KeyEventArgs e)
        {

            keyboardHandler.KeyDown(this, e);
        }

        private void Display_Form_PreviewKeyDown(object sender, KeyEventArgs e)
        {

            keyboardHandler.PreviewKeyDown(this, e);
        }

        private void Display_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            throw new NotImplementedException();
        }

        void Display_MouseMove(object sender, MouseEventArgs e)
        {
            mouseHandler.OnMouseMove(this, sender, e);
        }
        #endregion

        #region Menus
        private void Resume_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Load_Click(object sender, RoutedEventArgs e)
        {
            LoadAndDisplayComic(true);
        }

        private void NextPage_Click(object sender, RoutedEventArgs e)
        {
            NextPage();
        }

        private void PreviousPage_Click(object sender, RoutedEventArgs e)
        {
            PreviousPage();
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
            ShowPageInformation();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            ApplicationExit(sender, e);
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
        public void ShowMessage(String Message)
        {
            MessageBox.Text = Message;
            MessageBox.Visibility = System.Windows.Visibility.Visible;

            if (ShowMessageTimer != null)
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
            if (ComicBook != null)
            {
                PageInfoBox.Text = "Archive" + (Convert.ToInt32(ComicBook.GetCurrentFile()) + 1) + "/" + ComicBook.GetTotalFiles() + "\r\nPage: " + (Convert.ToInt32(ComicBook.GetCurrentPageOfTotal()) + 1) + "/" + ComicBook.GetTotalPages();
                PageInfoBox.Visibility = System.Windows.Visibility.Visible;

                if (PageInformationTimer != null)
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

        private void UpdatePageInformation()
        {
            PageInfoBox.Text = "Archive" + (Convert.ToInt32(ComicBook.GetCurrentFile()) + 1) + "/" + ComicBook.GetTotalFiles() + "\r\nPage: " + (Convert.ToInt32(ComicBook.GetCurrentPageOfTotal()) + 1) + "/" + ComicBook.GetTotalPages();
        }
        #endregion

        #region Load an Display
        public void DisplayImage(byte[] image, string scrollTo)
        {
            // If page information is displayed update it with new information
            if (PageInfoBox.Visibility == System.Windows.Visibility.Visible)
            {
                UpdatePageInformation();
            }

            switch (scrollTo)
            {
                case "top":
                    {
                        ScrollField.ScrollToTop();
                        ScrollField.ScrollToLeftEnd();
                        break;
                    }
                case "bottom":
                    {
                        ScrollField.ScrollToBottom();
                        ScrollField.ScrollToRightEnd();
                        break;
                    }
                default:
                    {
                        break;
                    }
            }

            BitmapImage bitmapimage = new BitmapImage();
            bitmapimage.BeginInit();
            MemoryStream stream = new MemoryStream(image);
            bitmapimage.StreamSource = stream;
            bitmapimage.EndInit();

            DisplayedImage.Source = bitmapimage;

            DisplayedImage.Width = bitmapimage.PixelWidth;
            DisplayedImage.Height = bitmapimage.PixelHeight;

            if (DisplayedImage.Width < WindowWidth)
            {
                DisplayedImage.HorizontalAlignment = HorizontalAlignment.Center;
            }
            else if (DisplayedImage.Width >= WindowWidth)
            {
                DisplayedImage.HorizontalAlignment = HorizontalAlignment.Left;
            }
            if (DisplayedImage.Height < WindowHeight)
            {
                DisplayedImage.VerticalAlignment = VerticalAlignment.Center;
            }
            else if (DisplayedImage.Height >= WindowHeight)
            {
                DisplayedImage.VerticalAlignment = VerticalAlignment.Top;
            }

        }

        /// <summary>
        /// Load archive(s) and display first page
        /// </summary>
        /// <param name="AskOpenFileDialog">Should file dialog be used?</param>
        public void LoadAndDisplayComic(Boolean AskOpenFileDialog)
        {
            try
            {
                String[] Files;

                if (AskOpenFileDialog)
                {
                    OpenFileDialog OpenFileDialog = new OpenFileDialog();
                    if (ComicBook != null)
                    {
                        Bookmark Bookmark = ComicBook.GetBookmark();
                        OpenFileDialog.InitialDirectory = Bookmark.GetCurrentDirectoryLocation();
                    }
                    OpenFileDialog.Filter = "Supported formats (*.cbr;*.cbz;*.zip;*.rar)|*.cbr;*.cbz;*.zip;*.rar|All files (*.*)|*.*";
                    OpenFileDialog.Multiselect = true;
                    OpenFileDialog.ShowDialog();
                    if (OpenFileDialog.FileNames.Length <= 0)
                        throw new Exception();
                    Files = OpenFileDialog.FileNames;

                }
                else
                {
                    Files = new String[] { OpeningFile };
                }

                foreach (String file in Files)
                {
                    if (!File.Exists(file))
                    {
                        ShowMessage("One or more archives not found");
                        throw new Exception();
                    }
                }
                Mouse.OverrideCursor = Cursors.Wait;
                LoadArchive(Files, 0, 0);
            }
            catch { }
            Mouse.OverrideCursor = Cursors.Arrow;

        }

        /// <summary>
        /// Load archive(s) and display first page
        /// </summary>
        /// <param name="Files">Array with archive locations</param>
        public void LoadAndDisplayComic(String[] Files)
        {
            try
            {
                foreach (String file in Files)
                {
                    if (!File.Exists(file))
                    {
                        ShowMessage("One or more archives not found");
                        throw new Exception();
                    }
                }

                Mouse.OverrideCursor = Cursors.Wait;
                LoadArchive(Files, 0, 0);
            }
            catch { }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        /// <summary>
        /// Load archive(s) and display a page of choice
        /// </summary>
        /// <param name="Files">Array with archive locations</param>
        /// <param name="FileNumber">File in array to start at</param>
        /// <param name="PageNumber">Page on wich to start from selected file</param>
        public void LoadAndDisplayComic(String[] Files, int FileNumber, int PageNumber)
        {
            try
            {
                foreach (String file in Files)
                {
                    if (!File.Exists(file))
                    {
                        ShowMessage("One or more archives not found");
                        throw new Exception();
                    }
                }
                Mouse.OverrideCursor = Cursors.Wait;
                LoadArchive(Files, FileNumber, PageNumber);
            }
            catch { }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        /// <summary>
        /// Load the archives
        /// </summary>
        /// <param name="Files">Archive location</param>
        /// <param name="FileNumber">File in array to start at</param>
        /// <param name="PageNumber">Page on wich to start from selected file</param>
        public void LoadArchive(String[] Files, int FileNumber, int PageNumber)
        {
            FileLoader.Load(Files);

            if (FileLoader.HasFile)
            {
                ComicBook = FileLoader.ComicBook;
                DisplayImage(ComicBook.GetPage(FileNumber, PageNumber), "top");
                if (!String.IsNullOrEmpty(FileLoader.Error))
                    ShowMessage(FileLoader.Error);
            }
            else if (!String.IsNullOrEmpty(FileLoader.Error))
                ShowMessage(FileLoader.Error);
            else
                ShowMessage("No supported files found in archive");
        }

        /// <summary>
        /// Toggle the images options (fit to screen etc.)
        /// </summary>
        public void ToggleImageOptions()
        {
            //normal to hight
            if (!Configuration.overideHight && !Configuration.overideWidth)
            {
                Configuration.overideHight = true;
                ShowMessage("Fit to hight.");
            }
            //hight to width
            else if (Configuration.overideHight && !Configuration.overideWidth)
            {
                Configuration.overideHight = false;
                Configuration.overideWidth = true;
                ShowMessage("Fit to width.");
            }
            //width to screen
            else if (!Configuration.overideHight && Configuration.overideWidth)
            {
                Configuration.overideHight = true;
                Configuration.overideWidth = true;
                ShowMessage("Fit to screen.");
            }
            //screen to normal
            else if (Configuration.overideHight && Configuration.overideWidth)
            {
                Configuration.overideHight = false;
                Configuration.overideWidth = false;
                ShowMessage("Normal mode.");
            }
            if (DisplayedImage.Source != null)
                DisplayImage(ComicBook.GetCurrentPage(), "top");

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
                    DisplayImage(image, "top");
                    ScrollField.ScrollToVerticalOffset(0);
                    ScrollField.ScrollToHorizontalOffset(0);
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
                    DisplayImage(image, "bottom");
                }
            }
        }

        #region Properties
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
                if (value != null)
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

        public KeyboardHandler keyboardHandler
        {
            get;
            set;
        }

        public MouseHandler mouseHandler
        {
            get;
            set;
        }

        private ComicBook _ComicBook;

        public ComicBook ComicBook
        {
            get { return _ComicBook; }
            set
            {
                _ComicBook = value;
                keyboardHandler.ComicBook = value;
                mouseHandler.ComicBook = value;
            }
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

        public ImageEdit ImageEdit
        {
            get;
            set;
        }

        private FileLoader FileLoader
        {
            get;
            set;
        }

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

        public Configuration Configuration
        {
            get;
            set;
        }


        public int WindowHeight
        {
            get;
            set;
        }
        public int WindowWidth
        {
            get;
            set;
        }

        public int scrollValueVertical
        {
            get;
            set;
        }

        public int scrollValueHorizontal
        {
            get;
            set;
        }
        #endregion
    }
}