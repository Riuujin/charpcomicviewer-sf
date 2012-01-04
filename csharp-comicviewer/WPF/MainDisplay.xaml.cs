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
    using Csharp_comicviewer.Other;
    using Microsoft.Win32;
    using System.Collections.Generic;


	/// <summary>
	/// Interaction logic for Window2.xaml
	/// </summary>
    public partial class MainDisplay : Window
    {
		/// <summary>
		/// Initializes a new instance of the <see cref="MainDisplay"/> class.
		/// </summary>
		/// <param name="OpeningFile">The opening file.</param>
        public MainDisplay(string OpeningFile)
        {
            InitializeComponent();
            this.OpeningFile = OpeningFile;
        }

		/// <summary>
		/// Handles the Loaded event of the MainDisplay control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void MainDisplay_Loaded(object sender, RoutedEventArgs e)
        {
            //Ensure that the window is active on start
            this.Activate();

            ImageEdit = new ImageEdit();
            FileLoader = new FileLoader();

            //set mouse idle timer
            TimeoutToHide = TimeSpan.FromSeconds(2);
            MouseIdle = new DispatcherTimer();
            MouseIdle.Interval = TimeSpan.FromSeconds(1);
            MouseIdle.Tick += new EventHandler(MouseIdleChecker);
            MouseIdle.Start();

            //Load config
            LoadConfiguration();
            SetBookmarkMenus();

            //set window mode
            if (Configuration.windowed)
            {
                //go hidden first to fix size bug
                MenuBar.Visibility = Visibility.Hidden;
                this.WindowStyle = System.Windows.WindowStyle.SingleBorderWindow;
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

            scrollValueHorizontal = (int)(ScrollField.ViewportHeight * 0.05);
            scrollValueVertical = (int)(ScrollField.ViewportWidth * 0.05);
            ImageEdit.SetScreenHeight((int)ScrollField.ViewportHeight);
            ImageEdit.SetScreenWidth((int)ScrollField.ViewportWidth);


            //open file (when opening assosicated by double click)
            if (OpeningFile != null)
            {
                LoadAndDisplayComic(false);
            }
        }

		/// <summary>
		/// Exit the applications.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void ApplicationExit(object sender, EventArgs e)
        {
            SaveResumeToConfiguration();
            SaveConfiguration();
            Application.Current.Shutdown();
        }

		/// <summary>
		/// Loads the configuration.
		/// </summary>
        private void LoadConfiguration()
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
                }
                if (Configuration == null)
                {
                    Configuration = new Configuration();
                }
            }
            catch (Exception ex)
            {
                Configuration = new Configuration();
            }
        }

		/// <summary>
		/// Saves the configuration.
		/// </summary>
		/// <returns><c>True</c> if succes, otherwise returns <c>false</c>.</returns>
        private Boolean SaveConfiguration()
        {
            //xml config save
            try
            {
                string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                string userFilePath = Path.Combine(localAppData, "C# Comicviewer");
                if (!Directory.Exists(userFilePath))
                    Directory.CreateDirectory(userFilePath);

                XmlSerializer mySerializer = new XmlSerializer(typeof(Configuration));
                System.IO.StreamWriter myWriter = new System.IO.StreamWriter(userFilePath + "\\Configuration.xml");
                mySerializer.Serialize(myWriter, Configuration);
                myWriter.Close();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

		/// <summary>
		/// Saves the resume to configuration.
		/// </summary>
        private void SaveResumeToConfiguration()
        {
            if (ComicBook != null && ComicBook.GetTotalFiles() != 0)
            {
                Bookmark Data = ComicBook.GetBookmark();
                Configuration.Resume = Data;
            }
        }

        #region Bookmarks
        private void SetBookmarkMenus()
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
                if (Configuration.Bookmarks != null)
                {
                    if (Configuration.Bookmarks.Count > 0)
                    {
                        foreach (Bookmark bookmark in Configuration.Bookmarks)
                        {
                            String[] Files = bookmark.Files;
                            MenuItem Bookmark = new MenuItem();
                            Bookmark.Header = bookmark.CurrentFileName;
                            Bookmark.ToolTip = Files[bookmark.FileNumber];
                            Bookmark.Click += new RoutedEventHandler(LoadBookmark_Click);
                            Bookmarks_MenuRightClick.Items.Add(Bookmark);

                            MenuItem Bookmark_bar = new MenuItem();
                            Bookmark_bar.Header = bookmark.CurrentFileName;
                            Bookmark_bar.ToolTip = Files[bookmark.FileNumber];
                            Bookmark_bar.Click += new RoutedEventHandler(LoadBookmark_Click);
                            Bookmarks_MenuBar.Items.Add(Bookmark_bar);
                        }
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
        private void OnKeyDown(object sender, KeyEventArgs e)
        {

            // TODO N button nfo text
            if (e.Key == Key.X)
            {
                ApplicationExit(null, e);
            }

            if (e.Key == Key.R)
            {
                if (ResumeFile_RightClick.IsEnabled)
                    Resume_Click(sender, e);
                else
                    ShowMessage("No archive to resume");
            }
            if (e.Key == Key.I)
            {
                ShowPageInformation();
            }

            if (e.Key == Key.L)
            {
                LastMouseMove = DateTime.Now;

                if (MouseIsHidden)
                {
                    Mouse.OverrideCursor = Cursors.Arrow;
                    MouseIsHidden = false;
                }

                LoadAndDisplayComic(true);
            }

            if (e.Key == Key.M)
            {
                WindowState = System.Windows.WindowState.Minimized;
            }

            if (e.Key == Key.T)
            {
                ToggleImageOptions();
            }

            // if (char.ToLower((char)e.KeyChar) == char.ToLower((char)Keys.N))
            // {
            //    if (ComicBook != null && ComicBook.GetTotalFiles() != 0)
            //    {
            //        if (String.IsNullOrEmpty(ComicBook.GetInfoText(ComicBook.GetCurrentFile())))
            //            ShowMessage("No information text");
            //        else
            //            InfoText = new InfoText_Form(ComicBook.GetFileLocation(ComicBook.GetCurrentFile()), ComicBook.GetInfoText(ComicBook.GetCurrentFile()));
            //    }
            //    else
            //        ShowMessage("No archive loaded");
            // }
            if (e.Key == Key.W)
            {
                if (Configuration.windowed)
                {
                    //go fullscreen if windowed
                    Configuration.windowed = false;

                    this.WindowStyle = System.Windows.WindowStyle.None;
                    //go minimized first to hide taskbar
                    this.WindowState = System.Windows.WindowState.Minimized;
                    this.WindowState = System.Windows.WindowState.Maximized;
                    MenuBar.Visibility = Visibility.Collapsed;

                }
                else
                {
                    //go windowed if fullscreen
                    //go hidden first to fix size bug
                    MenuBar.Visibility = Visibility.Hidden;
                    this.WindowStyle = System.Windows.WindowStyle.SingleBorderWindow;
                    this.WindowState = System.Windows.WindowState.Maximized;
                    MenuBar.Visibility = Visibility.Visible;
                    Configuration.windowed = true;
                }

                scrollValueHorizontal = (int)(ScrollField.ViewportHeight * 0.05);
                scrollValueVertical = (int)(ScrollField.ViewportWidth * 0.05);
                ImageEdit.SetScreenHeight((int)ScrollField.ViewportHeight);
                ImageEdit.SetScreenWidth((int)ScrollField.ViewportWidth);

                if (DisplayedImage.Source != null)
                    DisplayImage(ComicBook.GetCurrentPage(), ImageStartPosition.Top);

            }
        }

        private void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Home && !Keyboard.Modifiers.HasFlag(ModifierKeys.Alt))
            {
                // first page of all
                if (ComicBook.GetTotalFiles() != 0)
                {
                    byte[] image = ComicBook.GetPage(0, 0);
                    if (image != null)
                    {
                        DisplayImage(image, ImageStartPosition.Top);
                    }
                }
            }

            if (e.Key == Key.Home && Keyboard.Modifiers.HasFlag(ModifierKeys.Alt))
            {
                // first page of current archive
                if (ComicBook.GetTotalFiles() != 0)
                {
                    byte[] image = ComicBook.GetPage(ComicBook.GetCurrentFile(), 0);
                    if (image != null)
                    {
                        DisplayImage(image, ImageStartPosition.Top);
                    }
                }
            }

            if (e.Key == Key.End && !Keyboard.Modifiers.HasFlag(ModifierKeys.Alt))
            {
                // last page of all
                if (ComicBook.GetTotalFiles() != 0)
                {
                    byte[] image = ComicBook.GetPage(ComicBook.GetTotalFiles() - 1, ComicBook.GetTotalPagesOfFile(ComicBook.GetTotalFiles() - 1) - 1);
                    if (image != null)
                    {
                        DisplayImage(image, ImageStartPosition.Top);
                    }
                }
            }

            if (e.Key == Key.End && Keyboard.Modifiers.HasFlag(ModifierKeys.Alt))
            {
                // last page of current archive
                if (ComicBook.GetTotalFiles() != 0)
                {
                    byte[] image = ComicBook.GetPage(ComicBook.GetCurrentFile(), ComicBook.GetTotalPagesOfFile(ComicBook.GetCurrentFile()) - 1);
                    if (image != null)
                    {
                        DisplayImage(image, ImageStartPosition.Top);
                    }
                }
            }

            if (e.Key == Key.PageDown)
            {
                NextPage();
            }

            if (e.Key == Key.PageUp)
            {
                PreviousPage();
            }

            if (e.SystemKey == Key.PageDown && Keyboard.Modifiers.HasFlag(ModifierKeys.Alt))
            {
                NextFile();
            }

            if (e.SystemKey == Key.PageUp && Keyboard.Modifiers.HasFlag(ModifierKeys.Alt))
            {
                PreviousFile();
            }

            if (e.Key == Key.Down)
            {
                OnArrowKey(Key.Down);
            }

            if (e.Key == Key.Up)
            {
                OnArrowKey(Key.Up);
            }

            if (e.Key == Key.Right)
            {
                OnArrowKey(Key.Right);
            }

            if (e.Key == Key.Left)
            {
                OnArrowKey(Key.Left);
            }
        }

        private void OnArrowKey(Key e)
        {
            int ScrollAmmount = 50;

            //scroll down
            if (e == Key.Down && DisplayedImage.Source != null)
            {
                ScrollField.ScrollToVerticalOffset(ScrollField.VerticalOffset + ScrollAmmount);
            }

            //scroll up
            if (e == Key.Up && DisplayedImage.Source != null)
            {
                ScrollField.ScrollToVerticalOffset(ScrollField.VerticalOffset - ScrollAmmount);
            }

            //scroll right
            if (e == Key.Right && DisplayedImage.Source != null)
            {
                ScrollField.ScrollToHorizontalOffset(ScrollField.HorizontalOffset + ScrollAmmount);
            }

            //scroll left
            if (e == Key.Left && DisplayedImage.Source != null)
            {
                ScrollField.ScrollToHorizontalOffset(ScrollField.HorizontalOffset - ScrollAmmount);
            }

            if (ScrollField.VerticalOffset > ScrollField.ScrollableHeight || ScrollField.VerticalOffset < 0)
            {
                ScrollField.ScrollToVerticalOffset(ScrollField.ScrollableHeight);
            }

            if (ScrollField.HorizontalOffset > ScrollField.ScrollableWidth || ScrollField.HorizontalOffset < 0)
            {
                ScrollField.ScrollToHorizontalOffset(ScrollField.ScrollableWidth);
            }
        }

		/// <summary>
		/// Called when mouse wheel scrolls.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="System.Windows.Input.MouseWheelEventArgs"/> instance containing the event data.</param>
        private void OnMouseWheel(object sender, MouseWheelEventArgs e)
        {

            //scroll down
            if (e.Delta < 0 && DisplayedImage.Source != null)
            {
                PreviousPageBoolean = false;
                PreviousPageCount = 2;
                if (DisplayedImage.Width > ScrollField.ViewportWidth)
                {
                    //image widther then screen
					if (ScrollField.HorizontalOffset == ScrollField.ScrollableWidth && ScrollField.VerticalOffset == ScrollField.ScrollableHeight)
                    {
                        //Can count down for next page
                        NextPageBoolean = true;
                        NextPageCount--;
                    }
                    else if (ScrollField.VerticalOffset == ScrollField.ScrollableHeight)
                    {
                        //scroll horizontal
                        ScrollField.ScrollToHorizontalOffset(ScrollField.HorizontalOffset + scrollValueHorizontal);
                    }
                }
                else if (ScrollField.VerticalOffset == ScrollField.ScrollableHeight)
                {
                    //Can count down for next page
                    NextPageBoolean = true;
                    NextPageCount--;
                }
            }
            //scroll up
            else if (e.Delta > 0 && DisplayedImage.Source != null)
            {
                NextPageBoolean = false;
                NextPageCount = 2;
                if (DisplayedImage.Width > ScrollField.ViewportWidth)
                {
                    //image widther then screen
                    if (ScrollField.HorizontalOffset == 0)
                    {
                        //Can count down for previous page
                        PreviousPageBoolean = true;
                        PreviousPageCount--;
                    }
                    else if (ScrollField.VerticalOffset == 0)
                    {
                        //scroll horizontal
                        ScrollField.ScrollToHorizontalOffset(ScrollField.HorizontalOffset - scrollValueHorizontal);
                    }
                }
                else if (ScrollField.VerticalOffset == 0)
                {
                    //Can count down for previous page
                    PreviousPageBoolean = true;
                    PreviousPageCount--;
                }
            }

            if (NextPageBoolean && NextPageCount <= 0)
            {
                NextPage();
                NextPageBoolean = false;
                NextPageCount = 2;
            }
            else if (PreviousPageBoolean && PreviousPageCount <= 0)
            {
                PreviousPage();
                PreviousPageBoolean = false;
                PreviousPageCount = 2;
            }
        }

        private void OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            OnMouseWheel(sender, e);
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            LastMouseMove = DateTime.Now;

            if (MouseIsHidden && (Mouse.GetPosition(this) != CurrentMousePosition))
            {
                Mouse.OverrideCursor = Cursors.Arrow;
                MouseIsHidden = false;
            }

            CurrentMousePosition = Mouse.GetPosition(this);

            int Speed = 2; //amount by with mouse_x/y - MousePosition.X/Y is divided, determines drag speed
            //am i dragging the mouse with left button pressed
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                //did i already change position
                if (MouseDrag == false)
                {
                    //if yes then i need to reset check position
                    MouseX = CurrentMousePosition.X;
                    MouseY = CurrentMousePosition.Y;
                    MouseDrag = true;
                }
                else
                //if no then i can perform checks on drag
                {
                    //Drag left
                    if (CurrentMousePosition.X < MouseX && DisplayedImage.Source != null)
                    {
                        ScrollField.ScrollToHorizontalOffset(ScrollField.HorizontalOffset + (MouseX - CurrentMousePosition.X) / Speed);
                        MouseDrag = false;
                    }
                    //Drag right
                    else if (CurrentMousePosition.X > MouseX && DisplayedImage.Source != null)
                    {
                        ScrollField.ScrollToHorizontalOffset(ScrollField.HorizontalOffset + (MouseX - CurrentMousePosition.X) / Speed);
                        MouseDrag = false;
                    }
                    //Drag up
                    if (CurrentMousePosition.Y < MouseY && DisplayedImage.Source != null)
                    {
                        ScrollField.ScrollToVerticalOffset(ScrollField.VerticalOffset + (MouseY - CurrentMousePosition.Y) / Speed);
                        MouseDrag = false;
                    }
                    //Drag down
                    else if (CurrentMousePosition.Y > MouseY && DisplayedImage.Source != null)
                    {
                        ScrollField.ScrollToVerticalOffset(ScrollField.VerticalOffset + (MouseY - CurrentMousePosition.Y) / Speed);
                        MouseDrag = false;
                    }
                }
            }
            //make it possible to drag on next check
            else
                MouseDrag = false;
        }

        private void OnRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
            MouseIsHidden = false;
        }

        private void MouseIdleChecker(object sender, EventArgs e)
        {
            TimeSpan elaped = DateTime.Now - LastMouseMove;
            if (elaped >= TimeoutToHide && !MouseIsHidden)
            {
                if (this.IsActive && !MenuRightClick.IsOpen)
                {
                    Mouse.OverrideCursor = Cursors.None;
                    MouseIsHidden = true;
                }
                else if (this.IsActive && MenuRightClick.IsOpen)
                {
                    LastMouseMove = DateTime.Now;
                }
            }
        }

        #endregion

        #region Menus
        private void Resume_Click(object sender, RoutedEventArgs e)
        {
            if (Configuration.Resume != null)
            {
                LoadAndDisplayComic(Configuration.Resume.Files, Configuration.Resume.FileNumber, Configuration.Resume.PageNumber);
            }
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
            NextFile();
        }

        private void PreviousFile_Click(object sender, RoutedEventArgs e)
        {
            PreviousFile();
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
            if (ComicBook != null)
            {
                Configuration.Bookmarks.Add(ComicBook.GetBookmark());
                SetBookmarkMenus();
            }
        }

        private void ManageBookmarks_Click(object sender, RoutedEventArgs e)
        {
            BookmarkManager bookmarkManager = new BookmarkManager(this);
            bookmarkManager.ShowDialog();
            SetBookmarkMenus();
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            About about = new About();
            about.ShowDialog();
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
                if (ComicBook.FilesAreArchives)
                {
                    PageInfoBox.Text = "Archive " + (Convert.ToInt32(ComicBook.GetCurrentFile()) + 1) + "/" + ComicBook.GetTotalFiles() + "\r\nArchive name: " + ComicBook.GetCurrentFileName() + "\r\nPage: " + (Convert.ToInt32(ComicBook.GetCurrentPageOfTotal()) + 1) + "/" + ComicBook.GetTotalPages();
                }
                else
                {
                    PageInfoBox.Text = "File name: " + ComicBook.GetCurrentFileName() + "\r\nPage: " + (Convert.ToInt32(ComicBook.GetCurrentPageOfTotal()) + 1) + "/" + ComicBook.GetTotalPages();

                }

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



        public void DisplayImage(byte[] ImageAsByteArray, ImageStartPosition scrollTo)
        {
            // If page information is displayed update it with new information
            if (PageInfoBox.Visibility == System.Windows.Visibility.Visible)
            {
                UpdatePageInformation();
            }

            switch (scrollTo.ToString())
            {
                case "Top":
                    {
                        ScrollField.ScrollToTop();
                        ScrollField.ScrollToLeftEnd();
                        break;
                    }
                case "Bottom":
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

            BitmapImage bitmapimage = GetImage(ImageAsByteArray);

            if (Configuration.OverideHeight || Configuration.OverideWidth)
            {
                ImageAsByteArray = ImageEdit.ResizeImage(ImageAsByteArray, new System.Drawing.Size(bitmapimage.PixelWidth, (int)ScrollField.ViewportHeight), Configuration.OverideHeight, Configuration.OverideWidth);
                bitmapimage = GetImage(ImageAsByteArray);
            }

            DisplayedImage.Source = bitmapimage;

            DisplayedImage.Width = bitmapimage.PixelWidth;
            DisplayedImage.Height = bitmapimage.PixelHeight;
            this.Background = ImageEdit.GetBackgroundColor(ImageAsByteArray);

            if (DisplayedImage.Width < ScrollField.ViewportWidth)
            {
                DisplayedImage.HorizontalAlignment = HorizontalAlignment.Center;
            }
            else if (DisplayedImage.Width >= ScrollField.ViewportWidth)
            {
                DisplayedImage.HorizontalAlignment = HorizontalAlignment.Left;
            }
            if (DisplayedImage.Height < ScrollField.ViewportHeight)
            {
                DisplayedImage.VerticalAlignment = VerticalAlignment.Center;
            }
            else if (DisplayedImage.Height >= ScrollField.ViewportHeight)
            {
                DisplayedImage.VerticalAlignment = VerticalAlignment.Top;
            }
            ShowPageInformation();
        }

        private BitmapImage GetImage(byte[] ImageAsByteArray)
        {
            BitmapImage bi = new BitmapImage();

            try
            {
                bi.CacheOption = BitmapCacheOption.OnLoad;
                MemoryStream ms = new MemoryStream(ImageAsByteArray);
                ms.Position = 0;
                bi.BeginInit();
                bi.StreamSource = ms;
                bi.EndInit();
            }
            catch
            {
                try
                {
                    //If it fails the normal way try it again with a convert (possible quality loss.
                    System.Drawing.ImageConverter ic = new System.Drawing.ImageConverter();
                    System.Drawing.Image img = (System.Drawing.Image)ic.ConvertFrom(ImageAsByteArray);
                    System.Drawing.Bitmap bitmap1 = new System.Drawing.Bitmap(img);
                    MemoryStream ms = new MemoryStream();
                    bitmap1.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    ms.Position = 0;
                    bi = new BitmapImage();
                    bi.CacheOption = BitmapCacheOption.OnLoad;
                    bi.BeginInit();
                    bi.StreamSource = ms;
                    bi.EndInit();
                }
                catch
                {
                    ShowMessage("Could not load image.");
                }
            }
            return bi;

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
                        OpenFileDialog.InitialDirectory = Bookmark.GetCurrentFileDirectoryLocation();
                    }
                    OpenFileDialog.Filter = "Supported archive formats (*.cbr; *.cbz; *.zip; *.rar)|*.cbr;*.cbz;*.zip;*.rar|Supported image formats (*.jpg; *.bmp; *.png)|*.jpg;*.bmp;*.png|All files (*.*)|*.*";
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
                LoadFile(Files, 0, 0);
            }
            catch (Exception e) { }
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
                LoadFile(Files, 0, 0);
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
                LoadFile(Files, FileNumber, PageNumber);
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
        public void LoadFile(String[] Files, int FileNumber, int PageNumber)
        {
            FileLoader.Load(Files);

            if (FileLoader.HasFile)
            {
                ComicBook = FileLoader.ComicBook;
                DisplayImage(ComicBook.GetPage(FileNumber, PageNumber), ImageStartPosition.Top);
                if (!String.IsNullOrEmpty(FileLoader.Error))
                    ShowMessage(FileLoader.Error);
            }
            else if (!String.IsNullOrEmpty(FileLoader.Error))
                ShowMessage(FileLoader.Error);
            else
                ShowMessage("No supported files found.");
        }

        /// <summary>
        /// Toggle the images options (fit to screen etc.)
        /// </summary>
        public void ToggleImageOptions()
        {
            //normal to hight
            if (!Configuration.OverideHeight && !Configuration.OverideWidth)
            {
                Configuration.OverideHeight = true;

                DisplayedImage.Height = 40;
                ShowMessage("Fit to hight.");
            }
            //hight to width
            else if (Configuration.OverideHeight && !Configuration.OverideWidth)
            {
                Configuration.OverideHeight = false;
                Configuration.OverideWidth = true;
                ShowMessage("Fit to width.");
            }
            //width to screen
            else if (!Configuration.OverideHeight && Configuration.OverideWidth)
            {
                Configuration.OverideHeight = true;
                Configuration.OverideWidth = true;
                ShowMessage("Fit to screen.");
            }
            //screen to normal
            else if (Configuration.OverideHeight && Configuration.OverideWidth)
            {
                Configuration.OverideHeight = false;
                Configuration.OverideWidth = false;
                ShowMessage("Normal mode.");
            }
            if (DisplayedImage.Source != null)
                DisplayImage(ComicBook.GetCurrentPage(), ImageStartPosition.Top);

        }

        /// <summary>
        /// Loads the first file found afther the current one and displays the first image if possible.
        /// </summary>
        private void NextFile()
        {
            if (ComicBook != null)
            {
                if (ComicBook.GetTotalFiles() != 0)
                {
                    if (ComicBook.GetTotalFiles() == 1 || ComicBook.GetCurrentFile() == ComicBook.GetTotalFiles() - 1)
                    {
                        Mouse.OverrideCursor = Cursors.Wait;
                        FileNextPrevious FileNextPrevious = new FileNextPrevious();
                        String[] CurrentLastFile = new String[1];
                        CurrentLastFile[0] = ComicBook.GetFileLocation(ComicBook.GetTotalFiles() - 1);
                        String[] Files = FileNextPrevious.GetNextFile(CurrentLastFile);
                        if (Files.Length > 0)
                        {
                            LoadAndDisplayComic(Files);
                        }
                        Mouse.OverrideCursor = Cursors.Arrow;
                    }
                    else
                    {
                        byte[] image = ComicBook.GetPage(ComicBook.GetCurrentFile() + 1, 0);
                        if (image != null)
                        {
                            DisplayImage(image, ImageStartPosition.Top);
                        }
                    }
                }
            }
        }


        private void PreviousFile()
        {
            if (ComicBook != null)
            {
                if (ComicBook.GetTotalFiles() != 0)
                {
                    if (ComicBook.GetTotalFiles() == 1 || ComicBook.GetCurrentFile() == 0)
                    {
                        Mouse.OverrideCursor = Cursors.Wait;
                        FileNextPrevious FileNextPrevious = new FileNextPrevious();
                        String[] CurrentLastFile = new String[1];
                        CurrentLastFile[0] = ComicBook.GetFileLocation(ComicBook.GetTotalFiles() - 1);
                        String[] Files = FileNextPrevious.GetPreviousFile(CurrentLastFile);
                        if (Files.Length > 0)
                        {
                            LoadAndDisplayComic(Files);
                        }
                        Mouse.OverrideCursor = Cursors.Arrow;
                    }
                    else
                    {
                        byte[] image = ComicBook.GetPage(ComicBook.GetCurrentFile() - 1, ComicBook.GetTotalPagesOfFile(ComicBook.GetCurrentFile() - 1) - 1);
                        if (image != null)
                        {
                            DisplayImage(image, ImageStartPosition.Bottom);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Go to next page
        /// </summary>
        public void NextPage()
        {
            if (ComicBook != null)
            {
                if (ComicBook.GetTotalFiles() != 0)
                {
                    byte[] image = ComicBook.NextPage();
                    if (image != null)
                    {
                        DisplayImage(image, ImageStartPosition.Top);
                    }
                }
            }
        }

        /// <summary>
        /// Go to previous page
        /// </summary>
        public void PreviousPage()
        {
            if (ComicBook != null)
            {
                if (ComicBook.GetTotalFiles() != 0)
                {
                    byte[] image = ComicBook.PreviousPage();
                    if (image != null)
                    {
                        DisplayImage(image, ImageStartPosition.Bottom);
                    }
                }
            }
        }

		#endregion
		
		#region Properties
		/// <summary>
		/// Gets or sets the opening file.
		/// </summary>
		/// <value>
		/// The opening file.
		/// </value>
		private string OpeningFile
        {
            get;
            set;
        }

		/// <summary>
		/// Gets or sets the comic book.
		/// </summary>
		/// <value>
		/// The comic book.
		/// </value>
        private ComicBook ComicBook
        {
            get;
            set;
        }

		/// <summary>
		/// Gets or sets a value indicating whether to go to next page.
		/// </summary>
		/// <value>
		///   <c>true</c> if go to next page; otherwise, <c>false</c>.
		/// </value>
        private bool GoToNextPage
        {
            get;
            set;
        }

		/// <summary>
		/// Gets or sets a value indicating whether to go to previous page.
		/// </summary>
		/// <value>
		///   <c>true</c> if go to previous page; otherwise, <c>false</c>.
		/// </value>
        private bool GoToPreviousPage
        {
            get;
            set;
        }

        private int _NextPageCount = 2;

		/// <summary>
		/// Gets or sets the next page count.
		/// </summary>
		/// <value>
		/// The next page count.
		/// </value>
        private int NextPageCount
        {
            get
            {
                return _NextPageCount;
            }
            set
            {
                _NextPageCount = value;
            }
        }

        private int _PreviousPageCount = 2;

		/// <summary>
		/// Gets or sets the previous page count.
		/// </summary>
		/// <value>
		/// The previous page count.
		/// </value>
        private int PreviousPageCount
        {
            get
            {
                return _PreviousPageCount;
            }
            set
            {
                _PreviousPageCount = value;
            }
        }

		/// <summary>
		/// Gets or sets the image edit.
		/// </summary>
		/// <value>
		/// The image edit.
		/// </value>
        private ImageEdit ImageEdit
        {
            get;
            set;
        }

		/// <summary>
		/// Gets or sets the file loader.
		/// </summary>
		/// <value>
		/// The file loader.
		/// </value>
        private FileLoader FileLoader
        {
            get;
            set;
        }

		/// <summary>
		/// Gets or sets the show message timer.
		/// </summary>
		/// <value>
		/// The show message timer.
		/// </value>
        private DispatcherTimer ShowMessageTimer
        {
            get;
            set;
        }

		/// <summary>
		/// Gets or sets the page information timer.
		/// </summary>
		/// <value>
		/// The page information timer.
		/// </value>
        private DispatcherTimer PageInformationTimer
        {
            get;
            set;
        }

		/// <summary>
		/// Gets or sets the last mouse move.
		/// </summary>
		/// <value>
		/// The last mouse move.
		/// </value>
        private DateTime LastMouseMove
        {
            get;
            set;
        }

		/// <summary>
		/// Gets or sets a value indicating whether mouse is hidden.
		/// </summary>
		/// <value>
		///   <c>true</c> if mouse is hidden; otherwise, <c>false</c>.
		/// </value>
        private bool MouseIsHidden
        {
            get;
            set;
        }

		/// <summary>
		/// Gets or sets the scroll value vertical.
		/// </summary>
		/// <value>
		/// The scroll value vertical.
		/// </value>
        private int scrollValueVertical
        {
            get;
            set;
        }

		/// <summary>
		/// Gets or sets the scroll value horizontal.
		/// </summary>
		/// <value>
		/// The scroll value horizontal.
		/// </value>
        private int scrollValueHorizontal
        {
            get;
            set;
        }

		/// <summary>
		/// Gets or sets the current mouse position.
		/// </summary>
		/// <value>
		/// The current mouse position.
		/// </value>
        private Point CurrentMousePosition
        {
            get;
            set;
        }

		/// <summary>
		/// Gets or sets the mouse X.
		/// </summary>
		/// <value>
		/// The mouse X.
		/// </value>
        private double MouseX
        {
            get;
            set;
        }

		/// <summary>
		/// Gets or sets the mouse Y.
		/// </summary>
		/// <value>
		/// The mouse Y.
		/// </value>
        private double MouseY
        {
            get;
            set;
        }

		/// <summary>
		/// Gets or sets a value indicating whether mouse drag.
		/// </summary>
		/// <value>
		///   <c>true</c> if mouse drag; otherwise, <c>false</c>.
		/// </value>
        private bool MouseDrag
        {
            get;
            set;
        }

		/// <summary>
		/// Gets or sets a value indicating whether going to next page is allowed.
		/// </summary>
		/// <value>
		///   <c>true</c> if going to next page is allowed; otherwise, <c>false</c>.
		/// </value>
        private bool NextPageBoolean
        {
            get;
            set;
        }

		/// <summary>
		/// Gets or sets a value indicating whether going to previous page is allowed.
		/// </summary>
		/// <value>
		///   <c>true</c> if going to previous page is allowed; otherwise, <c>false</c>.
		/// </value>
        private bool PreviousPageBoolean
        {
            get;
            set;
        }

		/// <summary>
		/// Gets or sets the timeout to hide.
		/// </summary>
		/// <value>
		/// The timeout to hide.
		/// </value>
        private TimeSpan TimeoutToHide
        {
            get;
            set;
        }

		/// <summary>
		/// Gets or sets the mouse idle.
		/// </summary>
		/// <value>
		/// The mouse idle.
		/// </value>
		public DispatcherTimer MouseIdle
		{
			get;
			set;
		}

		/// <summary>
		/// Gets the configuration.
		/// </summary>
		public Configuration Configuration
		{
			get;
			private set;
		}

		/// <summary>
		/// Start position on the image
		/// </summary>
        public enum ImageStartPosition
        {
            Top,
            Bottom
        }
        #endregion


    }
}