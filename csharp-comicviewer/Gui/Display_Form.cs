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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Xml.Serialization;
using System.Collections;
using System.Diagnostics;
using System.Runtime.InteropServices;
using csharp_comicviewer.Other;
using csharp_comicviewer.Gui;

namespace csharp_comicviewer
{
    /// <summary>
    /// The main display and control part
    /// </summary>
    public partial class Display_Form : Form
    {
        private ComicBook ComicBook;
        private String OpeningFile;
        private CustomStackTrace CustomStackTrace = new CustomStackTrace();
        private System.Windows.Forms.Timer MouseIdleTimer;

        #region Display_Form & Application
        /// <summary>
        /// Create the display and immediately load an archive
        /// </summary>
        /// <param name="OpeningFile">The location of the archive</param>
        public Display_Form(String OpeningFile)
        {
            InitializeComponent();
            this.OpeningFile = OpeningFile;
        }

        /// <summary>
        /// On load of the display, load configuration, create controls
        /// </summary>
        private void Display_Form_Load(object sender, EventArgs e)
        {
            //set mousewheel event (scrolling)
            this.MouseWheel += new MouseEventHandler(DisplayMouseWheel);

            //set mouse idle timer
            MouseIdleTimer = new System.Windows.Forms.Timer();
            MouseIdleTimer.Interval = 1000;
            MouseIdleTimer.Tick += new EventHandler(MouseIdleChecker);
            MouseIdleTimer.Start();

            //Load config
            LoadConfiguration();
            SetBookmarkMenu();

            //set window mode
            if (Configuration.windowed)
            {
                Configuration.windowed = true;
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
                this.ControlBox = true;
                this.WindowState = FormWindowState.Maximized;
                ResizeFix(null, null);
                MenuBar.Visible = true;

            }
            else //if fullscreen
                SetWinFullScreen(this.Handle);

            //gray out resume last file if the files dont't exist
            if (Configuration.Resume != null)
            {
                foreach (String file in Configuration.Resume.Files)
                {
                    if (!File.Exists(file))
                    {
                        Resume_item.Enabled = false;
                        Resume_item_bar.Enabled = false;
                    }
                }
            }
            else
            {
                Resume_item.Enabled = false;
                Resume_item_bar.Enabled = false;
            }

            if (!Configuration.windowed)
                ScreenHeight = this.Height;
            else
                ScreenHeight = this.Height - 38;
            ScreenWidth = this.Width;
            scrollValueHorizontal = (int)(ScreenHeight * 0.05);
            scrollValueVertical = (int)(ScreenWidth * 0.05);
            ImageEdit.SetScreenHeight(ScreenHeight);
            ImageEdit.SetScreenWidth(ScreenWidth);

            //open file (when opening assosicated by double click)
            if (OpeningFile != null)
            {
                LoadAndDisplayComic(false);
            }
        }

        /// <summary>
        /// Form closing event
        /// </summary>
        private void Display_Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            ApplicationExit(sender, e);
        }

        /// <summary>
        /// Exit C# Comicviewer and save configuration
        /// </summary>
        private void ApplicationExit(object sender, EventArgs e)
        {
            if (PageInformationThread != null)
            {
                PageInformationThread.Abort();
            }
            if (MessageThread != null)
            {
                MessageThread.Abort();
            }
            SaveResumeToConfiguration();
            SaveConfiguration();
            Application.Exit();
        }
        #endregion

        #region Fullscreen Fix

        private int ScreenHeight = 0;
        private int ScreenWidth = 0;

        /// <summary>
        /// Windows dll function needed for hiding taskbar in fullscreen
        /// </summary>
        [DllImport("user32.dll", EntryPoint = "GetSystemMetrics")]
        public static extern int GetSystemMetrics(int which);
        /// <summary>
        /// Windows dll function needed for hiding taskbar in fullscreen
        /// </summary>
        [DllImport("user32.dll")]
        public static extern void SetWindowPos(IntPtr hwnd, IntPtr hwndInsertAfter, int X, int Y, int width, int height, uint flags);
        //variables needed for fullscreen
        private const int SM_CXSCREEN = 0;
        private const int SM_CYSCREEN = 1;
        private static IntPtr HWND_TOP = IntPtr.Zero;
        private const int SWP_SHOWWINDOW = 64; // 0x0040

        /// <summary>
        /// Set a window to fullscreen
        /// </summary>
        /// <param name="handle">The handle of the form</param>
        public static void SetWinFullScreen(IntPtr handle)
        {
            SetWindowPos(handle, HWND_TOP, 0, 0, GetSystemMetrics(SM_CXSCREEN), GetSystemMetrics(SM_CYSCREEN), SWP_SHOWWINDOW);
        }

        /// <summary>
        /// Make sure current size is correct
        /// </summary>
        private void ResizeFix(object sender, EventArgs e)
        {
            if (!Configuration.windowed)
                ScreenHeight = this.Height;
            else
                ScreenHeight = this.Height - 38;
            ScreenWidth = this.Width;
            scrollValueHorizontal = (int)(ScreenHeight * 0.05);
            scrollValueVertical = (int)(ScreenWidth * 0.05);
            ImageEdit.SetScreenHeight(ScreenHeight);
            ImageEdit.SetScreenWidth(ScreenWidth);
            DisplayImage(DisplayedImage.Image);
        }

        #endregion

        #region Controls

        private int scrollValueVertical = 0;
        private int scrollValueHorizontal = 0;
        private Boolean NextPageBoolean = false;
        private int NextPageCount = 2;
        private Boolean PreviousPageBoolean = false;
        private int PreviousPageCount = 2;
        private Double MouseX, MouseY = 0.0;
        private Boolean MouseDrag = false;
        private InfoText_Form InfoText;
        private TimeSpan TimeoutToHide = TimeSpan.FromSeconds(2);
        private DateTime LastMouseMove = DateTime.Now;
        private Boolean MouseIsHidden = false;

        private void MouseIdleChecker(object sender, EventArgs e)
        {
            TimeSpan elaped = DateTime.Now - LastMouseMove;
            if (elaped >= TimeoutToHide && !MouseIsHidden)
            {
                Cursor.Hide();
                MouseIsHidden = true;
            }
        }

        private void OnMouseButton(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Right)
            {
                LastMouseMove = DateTime.Now;
                Cursor.Show();
                MouseIsHidden = false;
            }
        }

        /// <summary>
        /// Takes care of the scrolling with the mousewheel
        /// </summary>
        private void DisplayMouseWheel(object sender, MouseEventArgs e)
        {
            //scroll down
            if (e.Delta < 0 && DisplayedImage.Image != null)
            {
                PreviousPageCount = 2;
                if (e.Delta < 0 && DisplayedImage.Image != null)
                {
                    if (ImageEdit.IsImageHigherOrEquelThenScreen(DisplayedImage.Image))
                    {
                        if (-DisplayedImage.Top < (DisplayedImage.Size.Height - ScreenHeight) - scrollValueVertical)
                            DisplayedImage.Top -= scrollValueVertical;
                        else if ((-DisplayedImage.Top >= (DisplayedImage.Size.Height - ScreenHeight) - scrollValueVertical) &&
                                !(DisplayedImage.Top == -(DisplayedImage.Size.Height - ScreenHeight)))
                            DisplayedImage.Top = -(DisplayedImage.Size.Height - ScreenHeight);
                        else if (DisplayedImage.Top == -(DisplayedImage.Size.Height - ScreenHeight) && !ImageEdit.IsImageWidtherOrEquelThenScreen(DisplayedImage.Image))
                        {
                            NextPageBoolean = true;
                            NextPageCount--;
                        }

                        else if (DisplayedImage.Left != -(DisplayedImage.Size.Width - ScreenWidth) && ImageEdit.IsImageWidtherOrEquelThenScreen(DisplayedImage.Image))
                        {
                            if (-DisplayedImage.Left < (DisplayedImage.Size.Width - ScreenWidth) - scrollValueHorizontal)
                                DisplayedImage.Left -= scrollValueHorizontal;
                            else if ((-DisplayedImage.Left >= (DisplayedImage.Size.Width - ScreenWidth) - scrollValueHorizontal) &&
                                    !(DisplayedImage.Left == -(DisplayedImage.Size.Width - ScreenWidth)))
                                DisplayedImage.Left = -(DisplayedImage.Size.Width - ScreenWidth);
                        }
                        else if (DisplayedImage.Left == -(DisplayedImage.Size.Width - ScreenWidth))
                        {
                            NextPageBoolean = true;
                            NextPageCount--;
                        }
                    }
                    else
                        NextPage();
                }
            }
            //scroll up
            else if (e.Delta > 0 && DisplayedImage.Image != null)
            {
                int TopOfImageStart = 0;
                if (Configuration.windowed)
                    TopOfImageStart = 24;

                if (ImageEdit.IsImageHigherOrEquelThenScreen(DisplayedImage.Image))
                {
                    if (DisplayedImage.Top < TopOfImageStart - scrollValueVertical)
                        DisplayedImage.Top += scrollValueVertical;
                    else if ((DisplayedImage.Top >= TopOfImageStart - scrollValueVertical) &&
                            !(DisplayedImage.Top == TopOfImageStart))
                        DisplayedImage.Top = TopOfImageStart;
                    else if (DisplayedImage.Top == TopOfImageStart && !ImageEdit.IsImageWidtherOrEquelThenScreen(DisplayedImage.Image))
                    {
                        PreviousPageBoolean = true;
                        PreviousPageCount--;
                    }

                    else if (DisplayedImage.Left != 0 && ImageEdit.IsImageWidtherOrEquelThenScreen(DisplayedImage.Image))
                    {
                        if (DisplayedImage.Left < 0 - scrollValueHorizontal)
                            DisplayedImage.Left += scrollValueHorizontal;
                        else if ((-DisplayedImage.Left > 0 - scrollValueHorizontal) &&
                                !(DisplayedImage.Left == 0))
                            DisplayedImage.Left = 0;
                    }
                    else if (DisplayedImage.Left == 0)
                    {
                        PreviousPageBoolean = true;
                        PreviousPageCount--;
                    }
                }
                else
                    PreviousPage();
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

        /// <summary>
        /// Mouse dragging
        /// </summary>
        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            LastMouseMove = DateTime.Now;

            if (MouseIsHidden)
            {
                Cursor.Show();
                MouseIsHidden = false;
            }

            int Speed = 2; //amount by with mouse_x/y - MousePosition.X/Y is divided, determines drag speed
            //am i dragging the mouse with left button pressed
            if (e.Button == MouseButtons.Left)
            {
                //did i already change position
                if (MouseDrag == false)
                {
                    //if yes then i need to reset check position
                    MouseX = MousePosition.X;
                    MouseY = MousePosition.Y;
                    MouseDrag = true;
                }
                else
                //if no then i can perform checks on drag
                {
                    //Drag left
                    if (MousePosition.X < MouseX && DisplayedImage.Left < (DisplayedImage.Size.Width - ScreenWidth))
                    {
                        if (-DisplayedImage.Left + Convert.ToInt32((MouseX - MousePosition.X) / Speed) <= (DisplayedImage.Size.Width - ScreenWidth))
                            DisplayedImage.Left -= Convert.ToInt32((MouseX - MousePosition.X) / Speed);
                        else DisplayedImage.Left = -(DisplayedImage.Size.Width - ScreenWidth);
                        MouseDrag = false;
                    }
                    //Drag right
                    else if (MousePosition.X > MouseX && -DisplayedImage.Left > 0)
                    {
                        if (-DisplayedImage.Left - Convert.ToInt32((MousePosition.X - MouseX) / Speed) >= 0)
                            DisplayedImage.Left += Convert.ToInt32((MousePosition.X - MouseX) / Speed);
                        else DisplayedImage.Left = 0;
                        MouseDrag = false;
                    }
                    //Drag up
                    if (MousePosition.Y < MouseY && DisplayedImage.Top < (DisplayedImage.Size.Height - ScreenHeight))
                    {
                        if (-DisplayedImage.Top + Convert.ToInt32((MouseY - MousePosition.Y) / Speed) <= (DisplayedImage.Size.Height - ScreenHeight))
                            DisplayedImage.Top -= Convert.ToInt32((MouseY - MousePosition.Y) / Speed);
                        else DisplayedImage.Top = -(DisplayedImage.Size.Height - ScreenHeight);
                        MouseDrag = false;
                    }
                    //Drag down
                    else if (MousePosition.Y > MouseY && -DisplayedImage.Top > 0)
                    {
                        if (-DisplayedImage.Top - Convert.ToInt32((MousePosition.Y - MouseY) / Speed) >= 0)
                            DisplayedImage.Top += Convert.ToInt32((MousePosition.Y - MouseY) / Speed);
                        else DisplayedImage.Top = 0;
                        MouseDrag = false;
                    }
                }
            }
            //make it possible to drag on next check
            else
                MouseDrag = false;
        }

        /// <summary>
        /// Takes care of scrolling with the arrow keys
        /// </summary>
        private void ArrowKeyevent(Keys e)
        {
            //scroll down
            if (e == Keys.Down && DisplayedImage.Image != null)
            {
                if (ImageEdit.IsImageHigherOrEquelThenScreen(DisplayedImage.Image))
                {
                    if (-DisplayedImage.Top <= (DisplayedImage.Size.Height - ScreenHeight) - scrollValueVertical)
                        DisplayedImage.Top -= scrollValueVertical;
                    else if ((-DisplayedImage.Top >= (DisplayedImage.Size.Height - ScreenHeight) - scrollValueVertical) &&
                            !(DisplayedImage.Top == -(DisplayedImage.Size.Height - ScreenHeight)))
                        DisplayedImage.Top = -(DisplayedImage.Size.Height - ScreenHeight);
                }
            }

            //scroll up
            if (e == Keys.Up && DisplayedImage.Image != null)
            {
                if (ImageEdit.IsImageHigherOrEquelThenScreen(DisplayedImage.Image))
                {
                    if (DisplayedImage.Top <= 0 - scrollValueVertical)
                        DisplayedImage.Top += scrollValueVertical;
                    else if ((DisplayedImage.Top >= 0 - scrollValueVertical) &&
                            !(DisplayedImage.Top == 0))
                        DisplayedImage.Top = 0;
                }
            }

            //scroll right
            if (e == Keys.Right && DisplayedImage.Image != null)
            {
                if (ImageEdit.IsImageWidtherOrEquelThenScreen(DisplayedImage.Image))
                {
                    if (-DisplayedImage.Left <= (DisplayedImage.Size.Width - ScreenWidth) - scrollValueHorizontal)
                        DisplayedImage.Left -= scrollValueHorizontal;
                    else if ((-DisplayedImage.Left >= (DisplayedImage.Size.Width - ScreenWidth) - scrollValueHorizontal) &&
                            !(DisplayedImage.Left == -(DisplayedImage.Size.Width - ScreenWidth)))
                        DisplayedImage.Left = -(DisplayedImage.Size.Width - ScreenWidth);
                }
            }

            //scroll left
            if (e == Keys.Left && DisplayedImage.Image != null)
            {
                if (ImageEdit.IsImageWidtherOrEquelThenScreen(DisplayedImage.Image))
                {
                    if (DisplayedImage.Left <= 0 - scrollValueHorizontal)
                        DisplayedImage.Left += scrollValueHorizontal;
                    else if ((-DisplayedImage.Left >= 0 - scrollValueHorizontal) &&
                            !(DisplayedImage.Left == 0))
                        DisplayedImage.Left = 0;
                }
            }

        }

        /// <summary>
        /// Key press events
        /// </summary>
        private void DisplayKeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.ToLower((char)e.KeyChar) == char.ToLower((char)Keys.D))
            {
                SecondaryDisplay_Form f = new SecondaryDisplay_Form();

                Screen[] sc;
                sc = Screen.AllScreens;
                
                f.FormBorderStyle = FormBorderStyle.None;
                f.Left = sc[0].Bounds.Width;
                f.Top = sc[0].Bounds.Height;
                f.StartPosition = FormStartPosition.Manual;
                f.Show();

                Cursor.Show();
                MouseIsHidden = false;

            }
            if (char.ToLower((char)e.KeyChar) == char.ToLower((char)Keys.X))
                ApplicationExit(sender, e);
            if (char.ToLower((char)e.KeyChar) == char.ToLower((char)Keys.R))
                if (Resume_item.Enabled)
                    ResumeLastFiles_Click(sender, e);
                else
                    ShowMessage("No archive to resume");
            if (char.ToLower((char)e.KeyChar) == char.ToLower((char)Keys.I))
                ShowPageInformation();
            if (char.ToLower((char)e.KeyChar) == char.ToLower((char)Keys.L))
            {
                LastMouseMove = DateTime.Now;
                Cursor.Show();
                MouseIsHidden = false;
                LoadArchives_Click(sender, e);
            }
            if (char.ToLower((char)e.KeyChar) == char.ToLower((char)Keys.M))
                this.WindowState = FormWindowState.Minimized;
            if (char.ToLower((char)e.KeyChar) == char.ToLower((char)Keys.T))
                ToggleImageOptions();
            if (char.ToLower((char)e.KeyChar) == char.ToLower((char)Keys.N))
            {
                if (ComicBook != null && ComicBook.GetTotalFiles() != 0)
                {
                    if (String.IsNullOrEmpty(ComicBook.GetInfoText(ComicBook.GetCurrentFile())))
                        ShowMessage("No information text");
                    else
                        InfoText = new InfoText_Form(ComicBook.GetFileLocation(ComicBook.GetCurrentFile()), ComicBook.GetInfoText(ComicBook.GetCurrentFile()));
                }
                else
                    ShowMessage("No archive loaded");
            }

            if (char.ToLower((char)e.KeyChar) == char.ToLower((char)Keys.W))
            {
                if (!Configuration.windowed)
                {
                    Configuration.windowed = true;
                    this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
                    this.ControlBox = true;
                    this.WindowState = FormWindowState.Maximized;
                    ResizeFix(null, null);
                    MenuBar.Visible = true;
                    MenuBar.Enabled = true;
                }
                else
                {
                    Configuration.windowed = false;
                    MenuBar.Visible = false;
                    MenuBar.Enabled = false;
                    this.ControlBox = false;
                    this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                    this.WindowState = FormWindowState.Maximized;
                    SetWinFullScreen(this.Handle);
                    ResizeFix(null, null);
                }
            }
        }

        /// <summary>
        /// Key down events
        /// </summary>
        private void DisplayKeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Home && e.Modifiers != Keys.Alt) //first page of all
            {
                if (ComicBook.GetTotalFiles() != 0)
                {
                    Image image = ComicBook.GetPage(0, 0);
                    if (image != null)
                    {
                        DisplayImage(image);
                    }
                }
            }

            if (e.KeyCode == Keys.Home && e.Modifiers == Keys.Alt) //first page of current archive
            {
                if (ComicBook.GetTotalFiles() != 0)
                {
                    Image image = ComicBook.GetPage(ComicBook.GetCurrentFile(), 0);
                    if (image != null)
                    {
                        DisplayImage(image);
                    }
                }
            }

            if (e.KeyCode == Keys.End && e.Modifiers != Keys.Alt) //last page of all
            {
                if (ComicBook.GetTotalFiles() != 0)
                {
                    Image image = ComicBook.GetPage(ComicBook.GetTotalFiles() - 1, ComicBook.GetTotalPagesOfFile(ComicBook.GetTotalFiles() - 1) - 1);
                    if (image != null)
                    {
                        DisplayImage(image);
                    }
                }
            }

            if (e.KeyCode == Keys.End && e.Modifiers == Keys.Alt) //last page of current archive
            {
                if (ComicBook.GetTotalFiles() != 0)
                {
                    Image image = ComicBook.GetPage(ComicBook.GetCurrentFile(), ComicBook.GetTotalPagesOfFile(ComicBook.GetCurrentFile()) - 1);
                    if (image != null)
                    {
                        DisplayImage(image);
                    }
                }
            }

            if (e.KeyCode == Keys.PageDown)
                NextPage(sender, e);
            if (e.KeyCode == Keys.PageUp)
                PreviousPage(sender, e);

            if (e.KeyCode == Keys.PageDown && e.Modifiers == Keys.Alt)
                NextFile_Click(sender, e);
            if (e.KeyCode == Keys.PageUp && e.Modifiers == Keys.Alt)
                PreviousFile_Click(sender, e);


            if (e.KeyCode == Keys.Down)
            {
                ArrowKeyevent(Keys.Down);
            }
            if (e.KeyCode == Keys.Up)
            {
                ArrowKeyevent(Keys.Up);
            }
            if (e.KeyCode == Keys.Right)
            {
                ArrowKeyevent(Keys.Right);
            }
            if (e.KeyCode == Keys.Left)
            {
                ArrowKeyevent(Keys.Left);
            }

        }

        /// <summary>
        /// Show page information bar button
        /// </summary>
        private void ShowPageInformation_item_bar_Click(object sender, EventArgs e)
        {
            ShowPageInformation();
        }

        /// <summary>
        /// Show information text bar button
        /// </summary>
        private void InformationText_item_bar_Click(object sender, EventArgs e)
        {
            if (ComicBook != null && ComicBook.GetTotalFiles() != 0)
            {
                if (String.IsNullOrEmpty(ComicBook.GetInfoText(ComicBook.GetCurrentFile())))
                    ShowMessage("No information text");
                else
                    InfoText = new InfoText_Form(ComicBook.GetFileLocation(ComicBook.GetCurrentFile()), ComicBook.GetInfoText(ComicBook.GetCurrentFile()));
            }
            else
                ShowMessage("No archive loaded");
        }

        /// <summary>
        /// Load one or more archives
        /// </summary>
        private void LoadArchives_Click(object sender, EventArgs e)
        {
            LoadAndDisplayComic(true);
        }

        /// <summary>
        /// Resume the last file(s)
        /// </summary>
        private void ResumeLastFiles_Click(object sender, EventArgs e)
        {
            if (Configuration.Resume != null)
            {
                LoadAndDisplayComic(Configuration.Resume.Files, Configuration.Resume.FileNumber, Configuration.Resume.PageNumber);
            }
        }

        /// <summary>
        /// Open About Dialog
        /// </summary>
        void About_itemClick(object sender, EventArgs e)
        {
            About_Form About = new About_Form();
            About.ShowDialog();
        }

        /// <summary>
        /// Open previous file on disk
        /// </summary>
        private void PreviousFile_Click(object sender, EventArgs e)
        {
            if (ComicBook.GetTotalFiles() != 0)
            {
                if (ComicBook.GetTotalFiles() == 1 || ComicBook.GetCurrentFile() == 0)
                {
                    LoadArchives Archives = new LoadArchives();
                    Cursor = Cursors.WaitCursor;
                    FileNextPrevious FileNextPrevious = new FileNextPrevious();
                    String[] CurrentLastFile = new String[1];
                    CurrentLastFile[0] = ComicBook.GetFileLocation(ComicBook.GetTotalFiles() - 1);
                    String[] Files = FileNextPrevious.GetPreviousFile(CurrentLastFile);
                    if (Files.Length > 0)
                    {
                        LoadAndDisplayComic(Files);
                    }
                    this.Cursor = Cursors.Default;
                }
                else
                {
                    Image image = ComicBook.GetPage(ComicBook.GetCurrentFile() - 1, ComicBook.GetTotalPagesOfFile(ComicBook.GetCurrentFile() - 1) - 1);
                    if (image != null)
                    {
                        DisplayImage(image);
                    }
                }
            }
        }

        /// <summary>
        /// Open next file on disk
        /// </summary>
        private void NextFile_Click(object sender, EventArgs e)
        {
            if (ComicBook.GetTotalFiles() != 0)
            {
                if (ComicBook.GetTotalFiles() == 1 || ComicBook.GetCurrentFile() == ComicBook.GetTotalFiles() - 1)
                {
                    LoadArchives Archives = new LoadArchives();
                    Cursor = Cursors.WaitCursor;
                    FileNextPrevious FileNextPrevious = new FileNextPrevious();
                    String[] CurrentLastFile = new String[1];
                    CurrentLastFile[0] = ComicBook.GetFileLocation(ComicBook.GetTotalFiles() - 1);
                    String[] Files = FileNextPrevious.GetNextFile(CurrentLastFile);
                    if (Files.Length > 0)
                    {
                        LoadAndDisplayComic(Files);
                    }
                    this.Cursor = Cursors.Default;
                }
                else
                {
                    Image image = ComicBook.GetPage(ComicBook.GetCurrentFile() + 1, 0);
                    if (image != null)
                    {
                        DisplayImage(image);
                    }
                }
            }
        }
        #endregion

        #region Bookmarks
        /// <summary>
        /// Set the bookmark menu (updates items)
        /// </summary>
        private void SetBookmarkMenu()
        {
            Bookmark_menu.DropDownItems.Clear();
            Bookmark_menu.DropDownItems.Add(AddBookmark_item);
            Bookmark_menu.DropDownItems.Add(ManageBookmarks_item);
            Bookmark_menu.DropDownItems.Add(Bookmark_Separator);

            Bookmark_menu_bar.DropDownItems.Clear();
            Bookmark_menu_bar.DropDownItems.Add(AddBookmark_item_bar);
            Bookmark_menu_bar.DropDownItems.Add(ManageBookmarks_item_bar);
            Bookmark_menu_bar.DropDownItems.Add(Bookmark_Separator_bar);

            if (Configuration != null)
            {
                if (Configuration.Bookmarks.Count > 0)
                {
                    for (int i = 0; i < Configuration.Bookmarks.Count; i++)
                    {
                        Bookmark Data = Configuration.Bookmarks[i];
                        String[] Files = Data.Files;
                        ToolStripMenuItem Bookmark = new ToolStripMenuItem(Data.GetCurrentFileName());
                        Bookmark.ToolTipText = Files[Data.FileNumber];
                        Bookmark.Click += new EventHandler(LoadBookmark_Click);
                        Bookmark_menu.DropDownItems.Add(Bookmark);

                        ToolStripMenuItem Bookmark_bar = new ToolStripMenuItem(Data.GetCurrentFileName());
                        Bookmark_bar.ToolTipText = Files[Data.FileNumber];
                        Bookmark_bar.Click += new EventHandler(LoadBookmark_Click);
                        Bookmark_menu_bar.DropDownItems.Add(Bookmark_bar);
                    }
                }
            }
        }

        /// <summary>
        /// Add bookmark current file/page
        /// </summary>
        private void AddBookmark_item_Click(object sender, EventArgs e)
        {
            if (ComicBook != null)
            {
                Configuration.Bookmarks.Add(ComicBook.GetBookmark());

                ShowMessage("Bookmark added");
                SetBookmarkMenu();
            }
        }

        /// <summary>
        /// Load bookmark
        /// </summary>
        private void LoadBookmark_Click(object sender, EventArgs e)
        {
            //right click menu
            ArrayList Data = new ArrayList();
            for (int i = 0; i < Bookmark_menu.DropDownItems.Count; i++)
            {
                if ((ToolStripMenuItem)sender == Bookmark_menu.DropDownItems[i])
                {
                    Bookmark Bookmark = Configuration.Bookmarks[i - 3];

                    LoadAndDisplayComic(Bookmark.Files, Bookmark.FileNumber, Bookmark.PageNumber);
                }
            }

            //the bar
            for (int i = 0; i < Bookmark_menu.DropDownItems.Count; i++)
            {
                if ((ToolStripMenuItem)sender == Bookmark_menu_bar.DropDownItems[i])
                {
                    Bookmark Bookmark = Configuration.Bookmarks[i - 3];

                    LoadAndDisplayComic(Bookmark.Files, Bookmark.FileNumber, Bookmark.PageNumber);
                }
            }
        }

        /// <summary>
        /// Open manage bookmark dialog
        /// </summary>
        private void ManageBookmarks_item_Click(object sender, EventArgs e)
        {
            MannageBookmarks_Form mb = new MannageBookmarks_Form(Configuration);
            mb.ShowDialog();
            Configuration = mb.GetConfiguration();
            SetBookmarkMenu();
            mb.Dispose();
        }
        #endregion

        #region Configuration

        private Configuration Configuration = new Configuration();

        /// <summary>
        /// Save configuration
        /// </summary>
        /// <returns>succesfull</returns>
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
                MessageBox.Show("xml config save failed:" + ex.Message);
                CustomStackTrace.CreateStackTrace(ex.StackTrace);
                return false;
            }

        }

        /// <summary>
        /// Load configuration
        /// </summary>
        /// <returns>succesfull</returns>
        private Boolean LoadConfiguration()
        {
            //xml config load
            try
            {
                string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                string userFilePath = Path.Combine(localAppData, "C# Comicviewer");

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
                MessageBox.Show("xml config load failed:" + ex.Message);
                CustomStackTrace.CreateStackTrace(ex.StackTrace);
                return false;
            }

        }

        /// <summary>
        /// Save Resume last file(s) information
        /// </summary>
        private void SaveResumeToConfiguration()
        {
            if (ComicBook != null && ComicBook.GetTotalFiles() != 0)
            {
                Bookmark Data = ComicBook.GetBookmark();
                Configuration.Resume = Data;
            }
        }
        #endregion

        #region Messages and Page information

        private Thread MessageThread;
        private Thread PageInformationThread;
        private String MessageString;

        /// <summary>
        /// Show a message by using a thread, with "message"  as the message
        /// </summary>
        /// <param name="message">String that will be displayed as the message</param>
        private void ShowMessage(String message)
        {
            this.MessageString = message;

            if (MessageThread == null)
            {
                MessageThread = new Thread(new ThreadStart(Message));
                MessageThread.Start();
            }
            else
            {
                MessageThread.Abort();
                MessageThread = new Thread(new ThreadStart(Message));
                MessageThread.Start();
            }
        }

        /// <summary>
        /// Shows a message for 2000ms, must be used as thread
        /// </summary>
        private void Message()
        {
            try
            {
                this.Invoke((MethodInvoker)delegate
                            {
                                Message_lbl.Text = MessageString;
                                Message_lbl.Top = this.Height / 2;
                                Message_lbl.Left = 0;
                                Message_lbl.Width = this.Width;
                                Message_lbl.Visible = true;
                            });
                Thread.Sleep(2000);
                this.Invoke((MethodInvoker)delegate
                            {
                                Message_lbl.Visible = false;
                            });
            }
            catch (Exception ex)
            {
                if (!ex.Message.Contains("aborted"))
                {
                    MessageBox.Show(ex.Message);
                    CustomStackTrace.CreateStackTrace(ex.StackTrace);
                }
            }
        }

        /// <summary>
        /// Shows the page information (uses PageInformation())
        /// </summary>
        private void ShowPageInformation()
        {
            if (ComicBook != null)
            {
                if (ComicBook.GetTotalFiles() != 0)
                {
                    if (ComicBook.HasFiles())
                    {
                        if (PageInformationThread == null)
                        {
                            PageInformationThread = new Thread(new ThreadStart(PageInformation));
                            PageInformationThread.Start();
                        }
                        else
                        {
                            PageInformationThread.Abort();
                            PageInformationThread = new Thread(new ThreadStart(PageInformation));
                            PageInformationThread.Start();
                        }
                    }
                }
            }
            else
                ShowMessage("No archive loaded");
        }

        /// <summary>
        /// Shows the page information for 5000ms, must be used as thread
        /// </summary>
        private void PageInformation()
        {
            try
            {
                int width = this.Width;
                int height = this.Height;
                string pagestring = "Archive" + (Convert.ToInt32(ComicBook.GetCurrentFile()) + 1) + "/" + ComicBook.GetTotalFiles() + "\r\n Page: " + (Convert.ToInt32(ComicBook.GetCurrentPageOfTotal()) + 1) + "/" + ComicBook.GetTotalPages();

                this.Invoke((MethodInvoker)delegate
                            {
                                Page_lbl.Text = pagestring;
                                if (!Configuration.windowed)
                                    Page_lbl.SetBounds(ScreenWidth - Page_lbl.Width, ScreenHeight - Page_lbl.Height, Page_lbl.Width, Page_lbl.Height);
                                else
                                    Page_lbl.SetBounds(ScreenWidth - Page_lbl.Width - 15, ScreenHeight - Page_lbl.Height, Page_lbl.Width, Page_lbl.Height);
                                Page_lbl.Visible = true;
                            });
                Thread.Sleep(5000);
                this.Invoke((MethodInvoker)delegate
                            {
                                Page_lbl.Visible = false;
                            });
            }
            catch (Exception ex)
            {
                if (!ex.Message.Contains("aborted"))
                {
                    MessageBox.Show(ex.Message);
                    CustomStackTrace.CreateStackTrace(ex.StackTrace);
                }
            }
        }
        #endregion

        #region Navigation
        /// <summary>
        /// Go to next page
        /// </summary>
        private void NextPage(object sender, EventArgs e)
        {
            if (ComicBook.GetTotalFiles() != 0)
            {
                Image image = ComicBook.NextPage();
                if (image != null)
                {
                    DisplayImage(image);
                }
            }
        }

        /// <summary>
        /// Go to next page
        /// </summary>
        private void NextPage()
        {
            if (ComicBook.GetTotalFiles() != 0)
            {
                Image image = ComicBook.NextPage();
                if (image != null)
                {
                    DisplayImage(image);
                }
            }
        }

        /// <summary>
        /// Go to previous page
        /// </summary>
        private void PreviousPage(object sender, EventArgs e)
        {
            if (ComicBook.GetTotalFiles() != 0)
            {
                Image image = ComicBook.PreviousPage();
                if (image != null)
                {
                    DisplayImage(image);
                    if (ImageEdit.IsImageHigherOrEquelThenScreen(image))
                        DisplayedImage.Top = -(DisplayedImage.Size.Height - ScreenHeight);
                    if (ImageEdit.IsImageWidtherOrEquelThenScreen(image))
                        DisplayedImage.Left = -(DisplayedImage.Size.Width - ScreenWidth);
                }
            }
        }

        /// <summary>
        /// Go to previous page
        /// </summary>
        private void PreviousPage()
        {
            if (ComicBook.GetTotalFiles() != 0)
            {
                Image image = ComicBook.PreviousPage();
                if (image != null)
                {
                    DisplayImage(image);
                    if (ImageEdit.IsImageHigherOrEquelThenScreen(image))
                        DisplayedImage.Top = -(DisplayedImage.Size.Height - ScreenHeight);
                    if (ImageEdit.IsImageWidtherOrEquelThenScreen(image))
                        DisplayedImage.Left = -(DisplayedImage.Size.Width - ScreenWidth);
                }
            }
        }
        #endregion

        #region Load an Display

        private ImageEdit ImageEdit = new ImageEdit();

        /// <summary>
        /// Load archive(s) and display first page
        /// </summary>
        /// <param name="AskOpenFileDialog">Should file dialog be used?</param>
        private void LoadAndDisplayComic(Boolean AskOpenFileDialog)
        {
            try
            {
                String[] Files;

                if (AskOpenFileDialog)
                {
                    OpenFileDialog OpenFileDialog = new OpenFileDialog();
                    if (ComicBook != null)
                    {
                        Bookmark bookmark = ComicBook.GetBookmark();
                        OpenFileDialog.InitialDirectory = bookmark.GetCurrentDirectoryLocation();
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

                Cursor = Cursors.WaitCursor;
                LoadArchive(Files,0,0);
            }
            catch { }
            Cursor = Cursors.Default;

        }

        /// <summary>
        /// Load archive(s) and display first page
        /// </summary>
        /// <param name="Files">Array with archive locations</param>
        private void LoadAndDisplayComic(String[] Files)
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

                Cursor = Cursors.WaitCursor;
                LoadArchive(Files,0,0);
            }
            catch { }
            Cursor = Cursors.Default;
        }

        /// <summary>
        /// Load archive(s) and display a page of choice
        /// </summary>
        /// <param name="Files">Array with archive locations</param>
        /// <param name="FileNumber">File in array to start at</param>
        /// <param name="PageNumber">Page on wich to start from selected file</param>
        private void LoadAndDisplayComic(String[] Files, int FileNumber, int PageNumber)
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
                Cursor = Cursors.WaitCursor;
                LoadArchive(Files,FileNumber,PageNumber);
            }
            catch { }
            Cursor = Cursors.Default;
        }

        /// <summary>
        /// Load the archives
        /// </summary>
        /// <param name="Files">Archive location</param>
        /// <param name="FileNumber">File in array to start at</param>
        /// <param name="PageNumber">Page on wich to start from selected file</param>
        private void LoadArchive(String[] Files, int FileNumber, int PageNumber)
        {
            LoadArchives Archives = new LoadArchives();
            LoadReturnValue ArchivesReturnValue = Archives.CreateComicBook(Files);

            if (ArchivesReturnValue.HasFile)
            {
                ComicBook = ArchivesReturnValue.ComicBook;
                DisplayImage(ComicBook.GetPage(FileNumber, PageNumber));
                if (!String.IsNullOrEmpty(ArchivesReturnValue.Error))
                    ShowMessage(ArchivesReturnValue.Error);
            }
            else if (!String.IsNullOrEmpty(ArchivesReturnValue.Error))
                ShowMessage(ArchivesReturnValue.Error);
            else
                ShowMessage("No supported files found in archive");
        }

        /// <summary>
        /// Display an image on the form
        /// </summary>
        /// <param name="image">The image</param>
        private void DisplayImage(Image image)
        {
            if (image != null)
            {
                if (Configuration.overideHight || Configuration.overideWidth)
                    image = ImageEdit.ResizeImage(image, new Size(image.Width, ScreenHeight), Configuration.overideHight, Configuration.overideWidth);
                try
                {
                    this.BackColor = ImageEdit.GetBackgroundColor(image);
                }
                catch (Exception)
                {
                    this.BackColor = DefaultBackColor;
                }
                DisplayedImage.Image = image;
                DisplayedImage.Location = ImageEdit.GetImageStartLocation(DisplayedImage.Image);
                if (Configuration.windowed)
                    DisplayedImage.Location = new Point(DisplayedImage.Location.X, DisplayedImage.Location.Y + 24);
                ShowPageInformation();
            }
        }

        /// <summary>
        /// Toggle the images options (fit to screen etc.)
        /// </summary>
        private void ToggleImageOptions()
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
            if (DisplayedImage.Image != null)
                DisplayImage(ComicBook.GetCurrentPage());

        }

        #endregion




    }
}
