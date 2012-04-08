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

namespace CSharpComicViewer.WPF
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
	using CSharpComicLoader;
	using CSharpComicLoader.Comic;
	using CSharpComicViewer.Configuration;
	using Microsoft.Win32;
	using CSharpComicLoader.File;


	/// <summary>
	/// Interaction logic for Window2.xaml
	/// </summary>
	public partial class MainDisplay : Window
	{
		#region Properties

		/// <summary>
		/// Gets or sets the opening file.
		/// </summary>
		/// <value>
		/// The opening file.
		/// </value>
		private string _openingFile;

		/// <summary>
		/// Gets or sets the comic book.
		/// </summary>
		/// <value>
		/// The comic book.
		/// </value>
		private ComicBook _comicBook;

		/// <summary>
		/// Gets or sets a value indicating whether to go to next page.
		/// </summary>
		/// <value>
		///   <c>true</c> if go to next page; otherwise, <c>false</c>.
		/// </value>
		private bool _goToNextPage;

		/// <summary>
		/// Gets or sets a value indicating whether to go to previous page.
		/// </summary>
		/// <value>
		///   <c>true</c> if go to previous page; otherwise, <c>false</c>.
		/// </value>
		private bool _goToPreviousPage;

		/// <summary>
		/// Gets or sets the next page count.
		/// </summary>
		/// <value>
		/// The next page count.
		/// </value>
		private int _nextPageCount = 2;

		/// <summary>
		/// Gets or sets the previous page count.
		/// </summary>
		/// <value>
		/// The previous page count.
		/// </value>
		private int _previousPageCount;

		/// <summary>
		/// The comic page (ea the displayed image)
		/// </summary>
		private ImageUtils _imageUtils;

		/// <summary>
		/// The information Text
		/// </summary>
		private InformationText _informationText;

		/// <summary>
		/// The file loader.
		/// </summary>
		private FileLoader _fileLoader;

		/// <summary>
		/// The show message timer.
		/// </summary>
		private DispatcherTimer _showMessageTimer;

		/// <summary>
		/// The page information timer.
		/// </summary>
		private DispatcherTimer _pageInformationTimer;

		/// <summary>
		/// The last mouse move.
		/// </summary>
		private DateTime _lastMouseMove;

		/// <summary>
		/// Gets or sets a value indicating whether mouse is hidden.
		/// </summary>
		/// <value>
		///   <c>true</c> if mouse is hidden; otherwise, <c>false</c>.
		/// </value>
		private bool _MouseIsHidden;

		/// <summary>
		/// Gets or sets the scroll value vertical.
		/// </summary>
		/// <value>
		/// The scroll value vertical.
		///   </value>
		private int _scrollValueVertical;

		/// <summary>
		/// Gets or sets the scroll value horizontal.
		/// </summary>
		/// <value>
		/// The scroll value horizontal.
		/// </value>
		private int _scrollValueHorizontal;

		/// <summary>
		/// Gets or sets the current mouse position.
		/// </summary>
		/// <value>
		/// The current mouse position.
		/// </value>
		private Point _currentMousePosition;

		/// <summary>
		/// Gets or sets the mouse X.
		/// </summary>
		/// <value>
		/// The mouse X.
		/// </value>
		private double _mouseX;

		/// <summary>
		/// Gets or sets the mouse Y.
		/// </summary>
		/// <value>
		/// The mouse Y.
		/// </value>
		private double _mouseY;

		/// <summary>
		/// Gets or sets a value indicating whether mouse drag.
		/// </summary>
		/// <value>
		///   <c>true</c> if mouse drag; otherwise, <c>false</c>.
		/// </value>
		private bool _mouseDrag;

		/// <summary>
		/// Gets or sets a value indicating whether going to next page is allowed.
		/// </summary>
		/// <value>
		///   <c>true</c> if going to next page is allowed; otherwise, <c>false</c>.
		/// </value>
		private bool _nextPageBoolean;

		/// <summary>
		/// Gets or sets a value indicating whether going to previous page is allowed.
		/// </summary>
		/// <value>
		///   <c>true</c> if going to previous page is allowed; otherwise, <c>false</c>.
		/// </value>
		private bool _previousPageBoolean;

		/// <summary>
		/// Gets or sets the timeout to hide.
		/// </summary>
		/// <value>
		/// The timeout to hide.
		/// </value>
		private TimeSpan _timeoutToHide;

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

		/// <summary>
		/// Initializes a new instance of the <see cref="MainDisplay"/> class.
		/// </summary>
		public MainDisplay()
		{
			_nextPageCount = 2;
			_previousPageCount = 2;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="MainDisplay"/> class.
		/// </summary>
		/// <param name="OpeningFile">The opening file.</param>
		public MainDisplay(string OpeningFile)
			: this()
		{
			InitializeComponent();
			this._openingFile = OpeningFile;
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

			_imageUtils = new ImageUtils();
			_fileLoader = new FileLoader();

			//set mouse idle timer
			_timeoutToHide = TimeSpan.FromSeconds(2);
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
				this.ResizeMode = System.Windows.ResizeMode.CanResize;
				MenuBar.Visibility = Visibility.Visible;
			}
			else //if fullscreen
			{
				this.WindowStyle = System.Windows.WindowStyle.None;
				this.WindowState = System.Windows.WindowState.Maximized;
				this.ResizeMode = System.Windows.ResizeMode.NoResize;
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

			_scrollValueHorizontal = (int)(ScrollField.ViewportHeight * 0.05);
			_scrollValueVertical = (int)(ScrollField.ViewportWidth * 0.05);
			_imageUtils.ScreenHeight = (int)ScrollField.ViewportHeight;
			_imageUtils.ScreenWidth = (int)ScrollField.ViewportWidth;


			//open file (when opening assosicated by double click)
			if (_openingFile != null)
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
			if (_comicBook != null && _comicBook.TotalFiles != 0)
			{
				Bookmark Data = _comicBook.GetBookmark();
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
				_lastMouseMove = DateTime.Now;

				if (_MouseIsHidden)
				{
					Mouse.OverrideCursor = Cursors.Arrow;
					_MouseIsHidden = false;
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

			if (e.Key == Key.N)
			{
				if (_comicBook != null && _comicBook.TotalFiles != 0)
				{
					if (String.IsNullOrEmpty(_comicBook.CurrentFile.InfoText))
					{
						ShowMessage("No information text");
					}
					else
					{
						_informationText = new InformationText(_comicBook.CurrentFile.Location, _comicBook.CurrentFile.InfoText);
						_informationText.ShowDialog();
					}
				}
				else
					ShowMessage("No archive loaded");
			}
			if (e.Key == Key.W)
			{
				if (Configuration.windowed)
				{
					//go full screen if windowed
					Configuration.windowed = false;

					this.WindowStyle = System.Windows.WindowStyle.None;
					//go minimized first to hide taskbar
					this.WindowState = System.Windows.WindowState.Minimized;
					this.WindowState = System.Windows.WindowState.Maximized;
					this.ResizeMode = System.Windows.ResizeMode.NoResize;
					MenuBar.Visibility = Visibility.Collapsed;

				}
				else
				{
					//go windowed if fullscreen
					//go hidden first to fix size bug
					MenuBar.Visibility = Visibility.Hidden;
					this.WindowStyle = System.Windows.WindowStyle.SingleBorderWindow;
					this.WindowState = System.Windows.WindowState.Maximized;
					this.ResizeMode = System.Windows.ResizeMode.CanResize;
					MenuBar.Visibility = Visibility.Visible;
					Configuration.windowed = true;
				}

				_scrollValueHorizontal = (int)(ScrollField.ViewportHeight * 0.05);
				_scrollValueVertical = (int)(ScrollField.ViewportWidth * 0.05);
				_imageUtils.ScreenHeight = (int)ScrollField.ViewportHeight;
				_imageUtils.ScreenWidth = (int)ScrollField.ViewportWidth;

				if (DisplayedImage.Source != null)
					DisplayImage(_comicBook.CurrentFile.CurrentPage, ImageStartPosition.Top);

			}
		}

		private void OnPreviewKeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Home && !Keyboard.Modifiers.HasFlag(ModifierKeys.Alt))
			{
				// first page of all
				if (_comicBook.TotalFiles != 0)
				{
					byte[] image = _comicBook.GetPage(0, 0);
					if (image != null)
					{
						DisplayImage(image, ImageStartPosition.Top);
					}
				}
			}

			if (e.Key == Key.Home && Keyboard.Modifiers.HasFlag(ModifierKeys.Alt))
			{
				// first page of current archive
				if (_comicBook.TotalFiles != 0)
				{
					byte[] image = _comicBook.GetPage(0);
					if (image != null)
					{
						DisplayImage(image, ImageStartPosition.Top);
					}
				}
			}

			if (e.Key == Key.End && !Keyboard.Modifiers.HasFlag(ModifierKeys.Alt))
			{
				// last page of all
				if (_comicBook.TotalFiles != 0)
				{
					byte[] image = _comicBook.GetPage(_comicBook.TotalFiles - 1, _comicBook[_comicBook.TotalFiles - 1].TotalPages - 1);
					if (image != null)
					{
						DisplayImage(image, ImageStartPosition.Top);
					}
				}
			}

			if (e.Key == Key.End && Keyboard.Modifiers.HasFlag(ModifierKeys.Alt))
			{
				// last page of current archive
				if (_comicBook.TotalFiles != 0)
				{
					byte[] image = _comicBook.GetPage(_comicBook.CurrentFile.TotalPages - 1);
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
				_previousPageBoolean = false;
				_previousPageCount = 2;
				if (DisplayedImage.Width > ScrollField.ViewportWidth)
				{
					//image widther then screen
					if (ScrollField.HorizontalOffset == ScrollField.ScrollableWidth && ScrollField.VerticalOffset == ScrollField.ScrollableHeight)
					{
						//Can count down for next page
						_nextPageBoolean = true;
						_nextPageCount--;
					}
					else if (ScrollField.VerticalOffset == ScrollField.ScrollableHeight)
					{
						//scroll horizontal
						ScrollField.ScrollToHorizontalOffset(ScrollField.HorizontalOffset + _scrollValueHorizontal);
					}
				}
				else if (ScrollField.VerticalOffset == ScrollField.ScrollableHeight)
				{
					//Can count down for next page
					_nextPageBoolean = true;
					_nextPageCount--;
				}
			}
			//scroll up
			else if (e.Delta > 0 && DisplayedImage.Source != null)
			{
				_nextPageBoolean = false;
				_nextPageCount = 2;
				if (DisplayedImage.Width > ScrollField.ViewportWidth)
				{
					//image widther then screen
					if (ScrollField.HorizontalOffset == 0)
					{
						//Can count down for previous page
						_previousPageBoolean = true;
						_previousPageCount--;
					}
					else if (ScrollField.VerticalOffset == 0)
					{
						//scroll horizontal
						ScrollField.ScrollToHorizontalOffset(ScrollField.HorizontalOffset - _scrollValueHorizontal);
					}
				}
				else if (ScrollField.VerticalOffset == 0)
				{
					//Can count down for previous page
					_previousPageBoolean = true;
					_previousPageCount--;
				}
			}

			if (_nextPageBoolean && _nextPageCount <= 0)
			{
				NextPage();
				_nextPageBoolean = false;
				_nextPageCount = 2;
			}
			else if (_previousPageBoolean && _previousPageCount <= 0)
			{
				PreviousPage();
				_previousPageBoolean = false;
				_previousPageCount = 2;
			}
		}

		private void OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
		{
			OnMouseWheel(sender, e);
		}

		private void OnMouseMove(object sender, MouseEventArgs e)
		{
			_lastMouseMove = DateTime.Now;

			if (_MouseIsHidden && (Mouse.GetPosition(this) != _currentMousePosition))
			{
				Mouse.OverrideCursor = Cursors.Arrow;
				_MouseIsHidden = false;
			}

			_currentMousePosition = Mouse.GetPosition(this);

			int Speed = 2; //amount by with mouse_x/y - MousePosition.X/Y is divided, determines drag speed
			//am i dragging the mouse with left button pressed
			if (e.LeftButton == MouseButtonState.Pressed)
			{
				//did i already change position
				if (_mouseDrag == false)
				{
					//if yes then i need to reset check position
					_mouseX = _currentMousePosition.X;
					_mouseY = _currentMousePosition.Y;
					_mouseDrag = true;
				}
				else
				//if no then i can perform checks on drag
				{
					//Drag left
					if (_currentMousePosition.X < _mouseX && DisplayedImage.Source != null)
					{
						ScrollField.ScrollToHorizontalOffset(ScrollField.HorizontalOffset + (_mouseX - _currentMousePosition.X) / Speed);
						_mouseDrag = false;
					}
					//Drag right
					else if (_currentMousePosition.X > _mouseX && DisplayedImage.Source != null)
					{
						ScrollField.ScrollToHorizontalOffset(ScrollField.HorizontalOffset + (_mouseX - _currentMousePosition.X) / Speed);
						_mouseDrag = false;
					}
					//Drag up
					if (_currentMousePosition.Y < _mouseY && DisplayedImage.Source != null)
					{
						ScrollField.ScrollToVerticalOffset(ScrollField.VerticalOffset + (_mouseY - _currentMousePosition.Y) / Speed);
						_mouseDrag = false;
					}
					//Drag down
					else if (_currentMousePosition.Y > _mouseY && DisplayedImage.Source != null)
					{
						ScrollField.ScrollToVerticalOffset(ScrollField.VerticalOffset + (_mouseY - _currentMousePosition.Y) / Speed);
						_mouseDrag = false;
					}
				}
			}
			//make it possible to drag on next check
			else
				_mouseDrag = false;
		}

		private void OnRightButtonDown(object sender, MouseButtonEventArgs e)
		{
			Mouse.OverrideCursor = Cursors.Arrow;
			_MouseIsHidden = false;
		}

		private void MouseIdleChecker(object sender, EventArgs e)
		{
			TimeSpan elaped = DateTime.Now - _lastMouseMove;
			if (elaped >= _timeoutToHide && !_MouseIsHidden)
			{
				if (this.IsActive && !MenuRightClick.IsOpen)
				{
					Mouse.OverrideCursor = Cursors.None;
					_MouseIsHidden = true;
				}
				else if (this.IsActive && MenuRightClick.IsOpen)
				{
					_lastMouseMove = DateTime.Now;
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
			if (_comicBook != null)
			{
				Configuration.Bookmarks.Add(_comicBook.GetBookmark());
				SetBookmarkMenus();
				ShowMessage("Bookmark added.");
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

			if (_showMessageTimer != null)
			{
				_showMessageTimer.Stop();
			}

			_showMessageTimer = new DispatcherTimer();
			_showMessageTimer.Tick += new EventHandler(HideMessage);
			_showMessageTimer.Interval = new TimeSpan(0, 0, 2);
			_showMessageTimer.Start();

		}

		private void HideMessage(object sender, EventArgs e)
		{

			MessageBox.Visibility = System.Windows.Visibility.Hidden;
		}

		public void ShowPageInformation()
		{
			if (_comicBook != null)
			{
				if (_fileLoader.PageType == PageType.Archive)
				{
					PageInfoBox.Text = "Archive " + _comicBook.CurrentFileNumber + "/" + _comicBook.TotalFiles + "\r\nArchive name: " + _comicBook.CurrentFile.FileName + "\r\nPage: " + _comicBook.CurrentPageNumber + "/" + _comicBook.TotalPages;
				}
				else
				{
					PageInfoBox.Text = "File name: " + _comicBook.CurrentFile.FileName + "\r\nPage: " + _comicBook.CurrentPageNumber + "/" + _comicBook.TotalPages;

				}

				PageInfoBox.Visibility = System.Windows.Visibility.Visible;

				if (_pageInformationTimer != null)
				{
					_pageInformationTimer.Stop();
				}

				_pageInformationTimer = new DispatcherTimer();
				_pageInformationTimer.Tick += new EventHandler(HidePageInformation);
				_pageInformationTimer.Interval = new TimeSpan(0, 0, 5);
				_pageInformationTimer.Start();
			}
		}

		private void HidePageInformation(object sender, EventArgs e)
		{
			PageInfoBox.Visibility = System.Windows.Visibility.Hidden;
		}

		private void UpdatePageInformation()
		{
			PageInfoBox.Text = "Archive" + _comicBook.CurrentFileNumber + "/" + _comicBook.TotalFiles + "\r\nPage: " + _comicBook.CurrentPageNumber + "/" + _comicBook.TotalPages;
		}
		#endregion

		#region Load an Display



		public void DisplayImage(byte[] ImageAsBytes, ImageStartPosition scrollTo)
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

			BitmapImage bitmapimage = GetImage(ImageAsBytes);
			_imageUtils.ObjectValue = ImageAsBytes;

			if (Configuration.OverideHeight || Configuration.OverideWidth)
			{
				_imageUtils.ResizeImage(new System.Drawing.Size(bitmapimage.PixelWidth, (int)ScrollField.ViewportHeight), Configuration.OverideHeight, Configuration.OverideWidth);
				bitmapimage = GetImage(_imageUtils.ObjectValueAsBytes);
			}

			DisplayedImage.Source = bitmapimage;

			DisplayedImage.Width = bitmapimage.PixelWidth;
			DisplayedImage.Height = bitmapimage.PixelHeight;
			this.Background = _imageUtils.BackgroundColor;

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
					//If it fails the normal way try it again with a convert, possible quality loss.
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
					if (_comicBook != null)
					{
						Bookmark Bookmark = _comicBook.GetBookmark();
						OpenFileDialog.InitialDirectory = Bookmark.GetCurrentFileDirectoryLocation();
					}
					OpenFileDialog.Filter = Utils.FileLoaderFilter;
					OpenFileDialog.Multiselect = true;
					OpenFileDialog.ShowDialog();
					if (OpenFileDialog.FileNames.Length <= 0)
						throw new Exception();
					Files = OpenFileDialog.FileNames;

				}
				else
				{
					Files = new String[] { _openingFile };
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
			_fileLoader.Load(Files);

			if (_fileLoader.LoadedFileData.HasFile)
			{
				_comicBook = _fileLoader.LoadedFileData.ComicBook;

				foreach (ComicFile comicFile in _comicBook)
				{
					if (!String.IsNullOrEmpty(comicFile.InfoText))
					{
						Mouse.OverrideCursor = Cursors.Arrow;
						_informationText = new InformationText(comicFile.Location, comicFile.InfoText);
						_informationText.ShowDialog();
						Mouse.OverrideCursor = Cursors.Wait;
					}
				}

				DisplayImage(_comicBook.GetPage(FileNumber, PageNumber), ImageStartPosition.Top);
				if (!String.IsNullOrEmpty(_fileLoader.Error))
				{
					ShowMessage(_fileLoader.Error);
				}
			}
			else if (!String.IsNullOrEmpty(_fileLoader.Error))
				ShowMessage(_fileLoader.Error);
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
				DisplayImage(_comicBook.CurrentFile.CurrentPage, ImageStartPosition.Top);

		}

		/// <summary>
		/// Loads the first file found after the current one and displays the first image if possible.
		/// </summary>
		private void NextFile()
		{
			if (_comicBook != null)
			{
				if (_comicBook.NextFile() == null)
				{
					FileNextPrevious FileNextPrevious = new FileNextPrevious();
					string file = FileNextPrevious.GetNextFileInDirectory(_comicBook.CurrentFile.Location);
					if (!String.IsNullOrEmpty(file))
					{
						LoadAndDisplayComic(new string[] { file });
					}
				}
				else
				{
					byte[] image = _comicBook.CurrentFile.CurrentPage;
					if (image != null)
					{
						DisplayImage(image, ImageStartPosition.Top);
					}
				}
			}
		}


		private void PreviousFile()
		{
			if (_comicBook != null)
			{
				if (_comicBook.PreviousFile() == null)
				{
					FileNextPrevious FileNextPrevious = new FileNextPrevious();
					string file = FileNextPrevious.GetPreviousFileInDirectory(_comicBook.CurrentFile.Location);
					if (!String.IsNullOrEmpty(file))
					{
						LoadAndDisplayComic(new string[] { file });
					}
				}
				else
				{
					byte[] image = _comicBook.CurrentFile.CurrentPage;
					if (image != null)
					{
						DisplayImage(image, ImageStartPosition.Bottom);
					}
				}
			}
		}

		/// <summary>
		/// Go to next page
		/// </summary>
		public void NextPage()
		{
			if (_comicBook != null)
			{
				if (_comicBook.TotalFiles != 0)
				{
					byte[] image = _comicBook.NextPage();
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
			if (_comicBook != null)
			{
				if (_comicBook.TotalFiles != 0)
				{
					byte[] image = _comicBook.PreviousPage();
					if (image != null)
					{
						DisplayImage(image, ImageStartPosition.Bottom);
					}
				}
			}
		}

		#endregion
	}
}