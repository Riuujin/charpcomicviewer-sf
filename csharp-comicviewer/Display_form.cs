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

namespace csharp_comicviewer
{
	public partial class Display_form : Form
	{
		private ComicBook ComicBook;
		private ImageEdit ImageEdit = new ImageEdit();
		private Thread MessageThread;
		private Thread PageCountThread;
		private String MessageString;
		private Boolean NextPageBoolean = false;
		private int NextPageCount = 2;
		private Boolean PreviousPageBoolean = false;
		private int PreviousPageCount = 2;
		private Double MouseX, MouseY = 0.0;
		private Boolean MouseDrag = false;
		private Configuration Configuration = new Configuration();
		private String OpeningFile;
		private CustomStackTrace CustomStackTrace = new CustomStackTrace();
		private InfoText InfoText;
		private int scrollValueVertical = (int)(SystemInformation.PrimaryMonitorSize.Height * 0.05);
		private	int scrollValueHorizontal = (int)(SystemInformation.PrimaryMonitorSize.Width * 0.05);
		private	int ScreenHeight = SystemInformation.PrimaryMonitorSize.Height;
		private	int ScreenWidth = SystemInformation.PrimaryMonitorSize.Width;
		

		public Display_form(String OpeningFile)
		{
			InitializeComponent();
			this.OpeningFile = OpeningFile;
		}

		private void Display_form_Load(object sender, EventArgs e)
		{
			//set mousewheel event (scrolling)
			this.MouseWheel += new MouseEventHandler(Mouse_Wheelevent);
			
			//Load config
			LoadConfiguration();
			SetBookmarkMenu();

			if (OpeningFile != null)
			{
				Cursor = Cursors.WaitCursor;
				Application.DoEvents();
				LoadArchive Archives = new LoadArchive();
				string[] Files = new string[1];
				Files[0] = OpeningFile;
				ComicBook = Archives.CreateComicBook(Files);
				SetImage(ComicBook.GetPage(0, 0));
				Cursor = Cursors.Default;
			}

			//gray out resume last file if the files dont't exist
			Boolean AllResumeExists = true;
			if (Configuration.Resume_Files != null)
			{
				if (Configuration.Resume_Files.Length > 0)
				{
					foreach (String ComicBook in Configuration.Resume_Files)
					{
						if (!File.Exists(ComicBook))
							AllResumeExists = false;
					}
					if (!AllResumeExists)
						Resume_item.Enabled = false;
				}
			}
			else
				Resume_item.Enabled = false;
		}

		private void SetBookmarkMenu()
		{
			Bookmark_menu.DropDownItems.Clear();
			Bookmark_menu.DropDownItems.Add(AddBookmark_item);
			Bookmark_menu.DropDownItems.Add(ManageBookmarks_item);
			Bookmark_menu.DropDownItems.Add(Bookmark_Separator);
			if (Configuration != null)
			{
				if (Configuration.Bookmarks.Count > 0)
				{
					ArrayList Data;
					for (int i = 0; i < Configuration.Bookmarks.Count; i++)
					{
						Data = (ArrayList)Configuration.Bookmarks[i];
						String[] Files = (String[])Data[0];
						ToolStripMenuItem Bookmark = new ToolStripMenuItem(Files[(int)Data[1]]);
						Bookmark.Click += new EventHandler(LoadBookmark);
						Bookmark_menu.DropDownItems.Add(Bookmark);
					}
				}
			}
		}

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
				CustomStackTrace.CreateStackTrace();
				return false;
			}

		}

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
				CustomStackTrace.CreateStackTrace();
				return false;
			}

		}

		private void Mouse_Wheelevent(object sender, MouseEventArgs e)
		{
			//scroll down
			if (e.Delta < 0 && DisplayedImage.Image != null)
			{
				PreviousPageCount = 2;
				if (e.Delta < 0 && DisplayedImage.Image != null)
				{
					if (ImageEdit.IsImageHigherOrEquelThenScreen(DisplayedImage.Image))
					{
						if(-DisplayedImage.Top < (DisplayedImage.Size.Height - ScreenHeight) - scrollValueVertical)
							DisplayedImage.Top -= scrollValueVertical;
						else if((-DisplayedImage.Top >= (DisplayedImage.Size.Height - ScreenHeight) - scrollValueVertical) &&
						        !(DisplayedImage.Top == -(DisplayedImage.Size.Height - ScreenHeight)))
							DisplayedImage.Top = -(DisplayedImage.Size.Height - ScreenHeight);
						else if(DisplayedImage.Top == -(DisplayedImage.Size.Height - ScreenHeight) && !ImageEdit.IsImageWidtherOrEquelThenScreen(DisplayedImage.Image))
						{
							NextPageBoolean = true;
							NextPageCount--;
						}
						
						else if(DisplayedImage.Left != -(DisplayedImage.Size.Width - ScreenWidth) && ImageEdit.IsImageWidtherOrEquelThenScreen(DisplayedImage.Image))
						{
							if(-DisplayedImage.Left < (DisplayedImage.Size.Width - ScreenWidth) - scrollValueHorizontal)
								DisplayedImage.Left -= scrollValueHorizontal;
							else if((-DisplayedImage.Left >= (DisplayedImage.Size.Width - ScreenWidth) - scrollValueHorizontal) &&
							        !(DisplayedImage.Left == -(DisplayedImage.Size.Width - ScreenWidth)))
								DisplayedImage.Left = -(DisplayedImage.Size.Width - ScreenWidth);
						}
						else if(DisplayedImage.Left == -(DisplayedImage.Size.Width - ScreenWidth))
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
				if (ImageEdit.IsImageHigherOrEquelThenScreen(DisplayedImage.Image))
				{
					if(DisplayedImage.Top < 0 - scrollValueVertical)
						DisplayedImage.Top += scrollValueVertical;
					else if((DisplayedImage.Top >= 0 - scrollValueVertical) &&
					        !(DisplayedImage.Top == 0))
						DisplayedImage.Top = 0;
					else if(DisplayedImage.Top == 0 && !ImageEdit.IsImageWidtherOrEquelThenScreen(DisplayedImage.Image))
					{
						PreviousPageBoolean = true;
						PreviousPageCount--;
					}
					
					else if(DisplayedImage.Left != 0 && ImageEdit.IsImageWidtherOrEquelThenScreen(DisplayedImage.Image))
					{
						if(DisplayedImage.Left < 0 - scrollValueHorizontal)
							DisplayedImage.Left += scrollValueHorizontal;
						else if((-DisplayedImage.Left > 0 - scrollValueHorizontal) &&
						        !(DisplayedImage.Left == 0))
							DisplayedImage.Left = 0;
					}
					else if(DisplayedImage.Left == 0)
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

		private void ArrowKeyevent(Keys e)
		{
			//scroll down
			if (e == Keys.Down && DisplayedImage.Image != null)
			{
				if (ImageEdit.IsImageHigherOrEquelThenScreen(DisplayedImage.Image))
				{
					if(-DisplayedImage.Top <= (DisplayedImage.Size.Height - ScreenHeight) - scrollValueVertical)
						DisplayedImage.Top -= scrollValueVertical;
					else if((-DisplayedImage.Top >= (DisplayedImage.Size.Height - ScreenHeight) - scrollValueVertical) &&
					        !(DisplayedImage.Top == -(DisplayedImage.Size.Height - ScreenHeight)))
						DisplayedImage.Top = -(DisplayedImage.Size.Height - ScreenHeight);
				}
			}

			//scroll up
			if (e == Keys.Up && DisplayedImage.Image != null)
			{
				if (ImageEdit.IsImageHigherOrEquelThenScreen(DisplayedImage.Image))
				{
					if(DisplayedImage.Top <= 0 - scrollValueVertical)
						DisplayedImage.Top += scrollValueVertical;
					else if((DisplayedImage.Top >= 0 - scrollValueVertical) &&
					        !(DisplayedImage.Top == 0))
						DisplayedImage.Top = 0;
				}
			}

			//scroll right
			if (e == Keys.Right && DisplayedImage.Image != null)
			{
				if (ImageEdit.IsImageWidtherOrEquelThenScreen(DisplayedImage.Image))
				{
					if(-DisplayedImage.Left <= (DisplayedImage.Size.Width - ScreenWidth) - scrollValueHorizontal)
						DisplayedImage.Left -= scrollValueHorizontal;
					else if((-DisplayedImage.Left >= (DisplayedImage.Size.Width - ScreenWidth) - scrollValueHorizontal) &&
					        !(DisplayedImage.Left == -(DisplayedImage.Size.Width - ScreenWidth)))
						DisplayedImage.Left = -(DisplayedImage.Size.Width - ScreenWidth);
				}
			}

			//scroll left
			if (e == Keys.Left && DisplayedImage.Image != null)
			{
				if (ImageEdit.IsImageWidtherOrEquelThenScreen(DisplayedImage.Image))
				{
					if(DisplayedImage.Left <= 0 - scrollValueHorizontal)
						DisplayedImage.Left += scrollValueHorizontal;
					else if((-DisplayedImage.Left >= 0 - scrollValueHorizontal) &&
					        !(DisplayedImage.Left == 0))
						DisplayedImage.Left = 0;
				}
			}

		}

		private void LoadArchives(object sender, EventArgs e)
		{

			LoadArchive Archives = new LoadArchive();

			OpenFileDialog OpenFileDialog = new OpenFileDialog();
			OpenFileDialog.Filter = "Supported formats (*.cbr;*.cbz;*.zip;*.rar)|*.cbr;*.cbz;*.zip;*.rar|All files (*.*)|*.*";
			OpenFileDialog.Multiselect = true;
			OpenFileDialog.ShowDialog();
			Cursor = Cursors.WaitCursor;
			string[] Files = OpenFileDialog.FileNames;


			if (Files.Length > 0)
			{
				ComicBook = Archives.CreateComicBook(Files);
                if (ComicBook.GetTotalFiles() != 0)
                    SetImage(ComicBook.GetPage(0, 0));
                else
                    ShowMessage("No supported files found in archive");
			}
			this.Cursor = Cursors.Default;
		}

		private void SetImage(Image image)
		{
			if (image != null)
			{
				if (Configuration.overideHight || Configuration.overideWidth)
					image = ImageEdit.resizeImage(image, new Size(image.Width, ScreenHeight), Configuration.overideHight, Configuration.overideWidth);
                SetBackColor(image);
                DisplayedImage.Image = image;
				SetImageLocation();
                ShowPageCount();
			}
		}

		/*
		 * Show a message by using a thread, with "message"  as the message
		 * message - string that will be displayed as the message
		 */
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

		/*
		 * Shows a message for 1000ms, must be used as thread
		 */
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
				Thread.Sleep(1000);
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
					CustomStackTrace.CreateStackTrace();
				}
			}
		}

		private void ShowPageCount()
		{
			if (ComicBook.GetTotalFiles() != 0)
			{
				if (ComicBook.HasFiles())
				{
					if (PageCountThread == null)
					{
						PageCountThread = new Thread(new ThreadStart(PageCount));
						PageCountThread.Start();
					}
					else
					{
						PageCountThread.Abort();
						PageCountThread = new Thread(new ThreadStart(PageCount));
						PageCountThread.Start();
					}
				}
			}
		}

		private void PageCount()
		{
			try
			{
				int width = this.Width;
				int height = this.Height;
				string pagestring = "Archive" + (Convert.ToInt32(ComicBook.GetCurrentFile()) + 1) + "/" + ComicBook.GetTotalFiles() + "\r\n Page: " + (Convert.ToInt32(ComicBook.GetCurrentPageOfTotal()) + 1) + "/" + ComicBook.GetTotalPages();

				this.Invoke((MethodInvoker)delegate
				            {
				            	Page_lbl.Text = pagestring;
				            	Page_lbl.SetBounds(width - Page_lbl.Width, height - Page_lbl.Height, Page_lbl.Width, Page_lbl.Height);
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
					CustomStackTrace.CreateStackTrace();
				}
			}
		}

		private void NextPage(object sender, EventArgs e)
		{
			if (ComicBook.GetTotalFiles() != 0)
			{
				Image image = ComicBook.NextPage();
				if (image != null)
				{
					SetImage(image);
				}
			}
		}

		private void NextPage()
		{
			if (ComicBook.GetTotalFiles() != 0)
			{
				Image image = ComicBook.NextPage();
				if (image != null)
				{
					SetImage(image);
				}
			}
		}

		private void PreviousPage(object sender, EventArgs e)
		{
			if (ComicBook.GetTotalFiles() != 0)
			{
				Image image = ComicBook.PreviousPage();
				if (image != null)
				{
					SetImage(image);
                    if (ImageEdit.IsImageHigherOrEquelThenScreen(image))
                    DisplayedImage.Top = -(DisplayedImage.Size.Height - ScreenHeight);
                    if(ImageEdit.IsImageWidtherOrEquelThenScreen(image))
                        DisplayedImage.Left = -(DisplayedImage.Size.Width - ScreenWidth);
				}
			}
		}

		private void PreviousPage()
		{
			if (ComicBook.GetTotalFiles() != 0)
			{
				Image image = ComicBook.PreviousPage();
				if (image != null)
				{
					SetImage(image);
                    if (ImageEdit.IsImageHigherOrEquelThenScreen(image))
                    DisplayedImage.Top = -(DisplayedImage.Size.Height - ScreenHeight);
                    if (ImageEdit.IsImageWidtherOrEquelThenScreen(image))
                        DisplayedImage.Left = -(DisplayedImage.Size.Width - ScreenWidth);
				}
			}
		}

		private void ApplicationExit(object sender, EventArgs e)
		{
			if (PageCountThread != null)
			{
				PageCountThread.Abort();
			}
			if (MessageThread != null)
			{
				MessageThread.Abort();
			}
			SaveResumeToConfiguration();
			SaveConfiguration();
			Application.Exit();
		}

		private void SaveResumeToConfiguration()
		{
			if (ComicBook != null && ComicBook.GetTotalFiles() != 0)
			{
				ArrayList Data = ComicBook.GetComicBookSaveInformation();
				Configuration.Resume_Files = (String[])Data[0];
				Configuration.Resume_Start_At[0] = Convert.ToInt32(Data[1]);
				Configuration.Resume_Start_At[1] = Convert.ToInt32(Data[2]);
			}
		}

		private void DisplayKeyPress(object sender, KeyPressEventArgs e)
		{
			if (char.ToLower((char)e.KeyChar) == char.ToLower((char)Keys.X))
				ApplicationExit(sender, e);
			if (char.ToLower((char)e.KeyChar) == char.ToLower((char)Keys.R))
				if(Resume_item.Enabled)
					ResumeLastFiles(sender, e);
				else
					ShowMessage("No archive to resume");
			if (char.ToLower((char)e.KeyChar) == char.ToLower((char)Keys.I))
				ShowPageCount();
			if (char.ToLower((char)e.KeyChar) == char.ToLower((char)Keys.L))
				LoadArchives(sender, e);
			if (char.ToLower((char)e.KeyChar) == char.ToLower((char)Keys.M))
				this.WindowState = FormWindowState.Minimized;
			if (char.ToLower((char)e.KeyChar) == char.ToLower((char)Keys.T))
				ToggleImageOptions();
			if (char.ToLower((char)e.KeyChar) == char.ToLower((char)Keys.N))
            {
                if (ComicBook != null && ComicBook.GetTotalFiles() != 0)
				{
					if(String.IsNullOrEmpty(ComicBook.GetInfoText(ComicBook.GetCurrentFile())))
						ShowMessage("No info text");
					else
						InfoText = new InfoText(ComicBook.GetFileLocation(ComicBook.GetCurrentFile()),ComicBook.GetInfoText(ComicBook.GetCurrentFile()));
				}
				else
					ShowMessage("No archive loaded");
			}
		}

		private void DisplayKeyDown(object sender, KeyEventArgs e)
		{

            if (e.KeyCode == Keys.Home && e.Modifiers != Keys.Alt) //first page of all
            {
                if (ComicBook.GetTotalFiles() != 0)
                {
                    Image image = ComicBook.GetPage(0, 0);
                    if (image != null)
                    {
                        SetImage(image);
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
                        SetImage(image);
                    }
                }
            }

            if (e.KeyCode == Keys.End && e.Modifiers != Keys.Alt) //last page of all
            {
                if (ComicBook.GetTotalFiles() != 0)
                {
                    Image image = ComicBook.GetPage(ComicBook.GetTotalFiles() -1, ComicBook.GetTotalPagesOfFile(ComicBook.GetTotalFiles()-1)-1);
                    if (image != null)
                    {
                        SetImage(image);
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
                        SetImage(image);
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
				SetImage(ComicBook.GetCurrentPage());

		}

		private void SetImageLocation()
		{
            DisplayedImage.Location = ImageEdit.CalculateLocationImage(DisplayedImage.Image);
		}

		private void SetBackColor(Image image)
		{
			try
			{
				this.BackColor = ImageEdit.setBackColor(image);
			}
			catch (Exception)
			{
				this.BackColor = DefaultBackColor;
			}
		}

		private void DisplayMouseMove(object sender, MouseEventArgs e)
		{
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
					else if (MousePosition.X > MouseX && -DisplayedImage.Left  > 0)
					{
						if (-DisplayedImage.Left  - Convert.ToInt32((MousePosition.X - MouseX) / Speed) >= 0)
							DisplayedImage.Left  += Convert.ToInt32((MousePosition.X - MouseX) / Speed);
						else DisplayedImage.Left  = 0;
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

		private void ResumeLastFiles(object sender, EventArgs e)
		{
			if (Configuration.Resume_Files.Length > 0)
			{
				if (File.Exists(Configuration.Resume_Files[0]))
				{
					Cursor = Cursors.WaitCursor;
					LoadArchive Archives = new LoadArchive();
					ComicBook = Archives.CreateComicBook(Configuration.Resume_Files);
                    if (ComicBook.GetTotalFiles() != 0)
					    SetImage(ComicBook.GetPage(Configuration.Resume_Start_At[0], Configuration.Resume_Start_At[1]));
                    else
                        ShowMessage("No supported files found in archive");
					this.Cursor = Cursors.Default;
				}
			}
		}

		private void AddBookmark_item_Click(object sender, EventArgs e)
		{
			if (ComicBook.GetTotalFiles() != 0)
			{
				ArrayList Data = ComicBook.GetComicBookSaveInformation();
				Configuration.Bookmarks.Add(Data);
				String[] Names = (String[])Data[0];
				ShowMessage("Bookmark added");
				SetBookmarkMenu();
			}
		}

		private void LoadBookmark(object sender, EventArgs e)
		{
			ArrayList Data = new ArrayList();
			for (int i = 0; i < Bookmark_menu.DropDownItems.Count; i++)
			{
				if ((ToolStripMenuItem)sender == Bookmark_menu.DropDownItems[i])
				{
					Data = (ArrayList)Configuration.Bookmarks[i - 3];
					String[] Files = (String[])Data[0];

					try
					{
						Cursor = Cursors.WaitCursor;
						LoadArchive Archives = new LoadArchive();
						ComicBook = Archives.CreateComicBook(Files);
						SetImage(ComicBook.GetPage((int)Data[1], (int)Data[2]));
						this.Cursor = Cursors.Default;
					}
					catch (Exception)
					{
						this.Cursor = Cursors.Default;
					}
				}
			}
		}

		private void ManageBookmarks_item_Click(object sender, EventArgs e)
		{
			ManageBookmarks mb = new ManageBookmarks(Configuration);
			mb.ShowDialog();
			SetBookmarkMenu();
		}
        
        void About_itemClick(object sender, EventArgs e)
        {
        	About_Form About = new About_Form();
        	About.ShowDialog();
        }

        private void PreviousFile_Click(object sender, EventArgs e)
        {
            if (ComicBook.GetTotalFiles() != 0)
            {
                if (ComicBook.GetTotalFiles() == 1 || ComicBook.GetCurrentFile() == 0)
                {
                    LoadArchive Archives = new LoadArchive();
                    Cursor = Cursors.WaitCursor;
                    FileNextPrevious FileNextPrevious = new FileNextPrevious();
                    String[] CurrentLastFile = new String[1];
                    CurrentLastFile[0] = ComicBook.GetFileLocation(ComicBook.GetTotalFiles() - 1);
                    String[] Files = FileNextPrevious.GetPreviousFile(CurrentLastFile);
                    if (Files.Length > 0)
                    {
                        ComicBook = Archives.CreateComicBook(Files);
                        if (ComicBook.GetTotalFiles() != 0)
                            SetImage(ComicBook.GetPage(0, ComicBook.GetTotalPagesOfFile(0) - 1));
                    }
                    this.Cursor = Cursors.Default;
                }
                else
                {
                    Image image = ComicBook.GetPage(ComicBook.GetCurrentFile() - 1, ComicBook.GetTotalPagesOfFile(ComicBook.GetCurrentFile() - 1) - 1);
                    if (image != null)
                    {
                        SetImage(image);
                    }
                }
            }
        }

        private void NextFile_Click(object sender, EventArgs e)
        {
            if (ComicBook.GetTotalFiles() != 0)
            {
                if (ComicBook.GetTotalFiles() == 1 || ComicBook.GetCurrentFile() == ComicBook.GetTotalFiles() - 1)
                {
                    LoadArchive Archives = new LoadArchive();
                    Cursor = Cursors.WaitCursor;
                    FileNextPrevious FileNextPrevious = new FileNextPrevious();
                    String[] CurrentLastFile = new String[1];
                    CurrentLastFile[0] = ComicBook.GetFileLocation(ComicBook.GetTotalFiles() - 1);
                    String[] Files = FileNextPrevious.GetNextFile(CurrentLastFile);
                    if (Files.Length > 0)
                    {
                        ComicBook = Archives.CreateComicBook(Files);
                        if (ComicBook.GetTotalFiles() != 0)
                            SetImage(ComicBook.GetPage(0, 0));
                    }
                    this.Cursor = Cursors.Default;
                }
                else
                {
                    Image image = ComicBook.GetPage(ComicBook.GetCurrentFile() + 1, 0);
                    if (image != null)
                    {
                        SetImage(image);
                    }
                }
            }
        }
	}
}
