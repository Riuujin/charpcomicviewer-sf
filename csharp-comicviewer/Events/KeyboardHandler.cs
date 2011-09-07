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
using System.Windows.Forms;
using csharp_comicviewer.Comic;
using System.Drawing;
using csharp_comicviewer.Other;
using csharp_comicviewer.Events;

namespace csharp_comicviewer.Events
{
	/// <summary>
	/// Description of KeyboardHandler.
	/// </summary>
	public class KeyboardHandler
	{
		public KeyboardHandler(Display_Form PrimaryDisplay,ComicBook ComicBook ,ImageEdit ImageEditPrimaryDisplay, MouseHandler MouseHandler)
		{
			this.PrimaryDisplay = PrimaryDisplay;
			this.ComicBook = ComicBook;
			SecondaryDisplay = null;
			this.ImageEditPrimaryDisplay = ImageEditPrimaryDisplay;
			ImageEditSecondaryDisplay = null;
			this.MouseHandler = MouseHandler;
		}
		
		
		/// <summary>
		/// The primary display form
		/// </summary>
		public Display_Form PrimaryDisplay
		{
			get;
			set;
		}

		
		/// <summary>
		/// The optional secondary display form
		/// </summary>
		public Display_Form SecondaryDisplay
		{
			get;
			set;
		}
		
		
		/// <summary>
		/// The comic book that will be displayed
		/// </summary>
		public ComicBook ComicBook
		{
			get;
			set;
		}
		
		public MouseHandler MouseHandler 
		{ 
			get; 
			set; 
		}
		
		
		/// <summary>
		/// Object used to edit the image (size) for the primary display
		/// </summary>
		public ImageEdit ImageEditPrimaryDisplay
		{
			get;
			set;
		}
		
		/// <summary>
		/// Object used to edit the image (size) for the secondary display
		/// </summary>
		public ImageEdit ImageEditSecondaryDisplay
		{
			get;
			set;
		}
		

		
		public void KeyDown(Display_Form SenderDisplay, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Home && e.Modifiers != Keys.Alt) //first page of all
			{
				if (ComicBook.GetTotalFiles() != 0)
				{
					Image image = ComicBook.GetPage(0, 0);
					if (image != null)
					{
						SenderDisplay.DisplayImage(image);
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
						SenderDisplay.DisplayImage(image);
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
						SenderDisplay.DisplayImage(image);
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
						SenderDisplay.DisplayImage(image);
					}
				}
			}

			if (e.KeyCode == Keys.PageDown)
			{
				NextPage(SenderDisplay);
			}
			if (e.KeyCode == Keys.PageUp)
			{
				PreviousPage(SenderDisplay);
			}
//
//			if (e.KeyCode == Keys.PageDown && e.Modifiers == Keys.Alt)
//			{
//				NextFile_Click(sender, e);
//			}
//			if (e.KeyCode == Keys.PageUp && e.Modifiers == Keys.Alt)
//			{
//				PreviousFile_Click(sender, e);
//			}


			if (e.KeyCode == Keys.Down)
			{
				ArrowKeyEventAllDisplays(SenderDisplay, Keys.Down);
			}
			if (e.KeyCode == Keys.Up)
			{
				ArrowKeyEventAllDisplays(SenderDisplay, Keys.Up);
			}
			if (e.KeyCode == Keys.Right)
			{
				ArrowKeyEventAllDisplays(SenderDisplay, Keys.Right);
			}
			if (e.KeyCode == Keys.Left)
			{
				ArrowKeyEventAllDisplays(SenderDisplay, Keys.Left);
			}

		}
		

		public void KeyPress(Display_Form SenderDisplay, KeyPressEventArgs e)
		{
			if (char.ToLower((char)e.KeyChar) == char.ToLower((char)Keys.X))
			{
				PrimaryDisplay.ApplicationExit(null, e);
			}
//			if (char.ToLower((char)e.KeyChar) == char.ToLower((char)Keys.R))
//				if (Resume_item.Enabled)
//					ResumeLastFiles_Click(sender, e);
//				else
//					ShowMessage("No archive to resume");
//			if (char.ToLower((char)e.KeyChar) == char.ToLower((char)Keys.I))
//				ShowPageInformation();
			if (char.ToLower((char)e.KeyChar) == char.ToLower((char)Keys.L))
			{
				MouseHandler.LastMouseMove = DateTime.Now;

				if (MouseHandler.MouseIsHidden)
				{
					Cursor.Show();
					MouseHandler.MouseIsHidden = false;
				}
				SenderDisplay.LoadAndDisplayComic(true);
			}
			if (char.ToLower((char)e.KeyChar) == char.ToLower((char)Keys.M))
			{
				PrimaryDisplay.WindowState = FormWindowState.Minimized;
				if(SecondaryDisplay != null)
				{
					SecondaryDisplay.WindowState = FormWindowState.Minimized;
				}
			}
			if (char.ToLower((char)e.KeyChar) == char.ToLower((char)Keys.T))
			{
				PrimaryDisplay.ToggleImageOptions();
				if(SecondaryDisplay != null)
				{
					if (SecondaryDisplay.DisplayedImage.Image != null)
					{
						SecondaryDisplay.DisplayImage(ComicBook.GetCurrentPage());
					}
				}
			}
//			if (char.ToLower((char)e.KeyChar) == char.ToLower((char)Keys.N))
//			{
//				if (ComicBook != null && ComicBook.GetTotalFiles() != 0)
//				{
//					if (String.IsNullOrEmpty(ComicBook.GetInfoText(ComicBook.GetCurrentFile())))
//						ShowMessage("No information text");
//					else
//						InfoText = new InfoText_Form(ComicBook.GetFileLocation(ComicBook.GetCurrentFile()), ComicBook.GetInfoText(ComicBook.GetCurrentFile()));
//				}
//				else
//					ShowMessage("No archive loaded");
//			}
//
			if (char.ToLower((char)e.KeyChar) == char.ToLower((char)Keys.W) && SecondaryDisplay == null)
			{
				MessageBox.Show("functie nog niet af");
				if (!PrimaryDisplay.Configuration.windowed)
				{
					PrimaryDisplay.Configuration.windowed = true;
					PrimaryDisplay.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
					PrimaryDisplay.ControlBox = true;
					PrimaryDisplay.WindowState = FormWindowState.Maximized;
					PrimaryDisplay.ResizeFix(null, null);
					PrimaryDisplay.MenuBar.Visible = true;
					PrimaryDisplay.MenuBar.Enabled = true;
				}
				else
				{
					PrimaryDisplay.Configuration.windowed = false;
					PrimaryDisplay.MenuBar.Visible = false;
					PrimaryDisplay.MenuBar.Enabled = false;
					PrimaryDisplay.ControlBox = false;
					PrimaryDisplay.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
					PrimaryDisplay.WindowState = FormWindowState.Maximized;
					PrimaryDisplay.SetWinFullScreen();
					PrimaryDisplay.ResizeFix(null, null);
				}
			}
		}

		
		private void NextPage(Display_Form SenderDisplay)
		{
			if(SenderDisplay == PrimaryDisplay)
			{
				PrimaryDisplay.NextPage();
				if(SecondaryDisplay != null)
				{
					SecondaryDisplay.NextPage();
				}
			}
			else
			{
				SecondaryDisplay.NextPage();
				PrimaryDisplay.NextPage();
			}
		}
		
		private void PreviousPage(Display_Form SenderDisplay)
		{
			if(SenderDisplay == PrimaryDisplay)
			{
				PrimaryDisplay.PreviousPage();
				if(SecondaryDisplay != null)
				{
					SecondaryDisplay.PreviousPage();
				}
			}
			else
			{
				SecondaryDisplay.PreviousPage();
				PrimaryDisplay.PreviousPage();
			}
		}
		
		
		private void ArrowKeyEventAllDisplays(Display_Form SenderDisplay, Keys e)
		{
			if(SenderDisplay == PrimaryDisplay)
			{
				ArrowKeyEvent(PrimaryDisplay,ImageEditPrimaryDisplay,e);
				if(SecondaryDisplay != null)
				{
					ArrowKeyEvent(SecondaryDisplay, ImageEditSecondaryDisplay,e);
				}
			}
			else
			{
				ArrowKeyEvent(SecondaryDisplay, ImageEditSecondaryDisplay,e);
				ArrowKeyEvent(PrimaryDisplay, ImageEditPrimaryDisplay,e);
			}
		}

		
		private void ArrowKeyEvent(Display_Form Display,ImageEdit ImageEdit, Keys e)
		{
			//scroll down
			if (e == Keys.Down && Display.DisplayedImage.Image != null)
			{
				if (ImageEdit.IsImageHigherOrEquelThenScreen(Display.DisplayedImage.Image))
				{
					if (-Display.DisplayedImage.Top <= (Display.DisplayedImage.Size.Height - Display.ScreenHeight) - Display.scrollValueVertical)
						Display.DisplayedImage.Top -= Display.scrollValueVertical;
					else if ((-Display.DisplayedImage.Top >= (Display.DisplayedImage.Size.Height - Display.ScreenHeight) - Display.scrollValueVertical) &&
					         !(Display.DisplayedImage.Top == -(Display.DisplayedImage.Size.Height - Display.ScreenHeight)))
						Display.DisplayedImage.Top = -(Display.DisplayedImage.Size.Height - Display.ScreenHeight);
				}
			}

			//scroll up
			if (e == Keys.Up && Display.DisplayedImage.Image != null)
			{
				if (ImageEdit.IsImageHigherOrEquelThenScreen(Display.DisplayedImage.Image))
				{
					if (Display.DisplayedImage.Top <= 0 - Display.scrollValueVertical)
						Display.DisplayedImage.Top += Display.scrollValueVertical;
					else if ((Display.DisplayedImage.Top >= 0 - Display.scrollValueVertical) &&
					         !(Display.DisplayedImage.Top == 0))
						Display.DisplayedImage.Top = 0;
				}
			}

			//scroll right
			if (e == Keys.Right && Display.DisplayedImage.Image != null)
			{
				if (ImageEdit.IsImageWidtherOrEquelThenScreen(Display.DisplayedImage.Image))
				{
					if (-Display.DisplayedImage.Left <= (Display.DisplayedImage.Size.Width - Display.ScreenWidth) - Display.scrollValueHorizontal)
						Display.DisplayedImage.Left -= Display.scrollValueHorizontal;
					else if ((-Display.DisplayedImage.Left >= (Display.DisplayedImage.Size.Width - Display.ScreenWidth) - Display.scrollValueHorizontal) &&
					         !(Display.DisplayedImage.Left == -(Display.DisplayedImage.Size.Width - Display.ScreenWidth)))
						Display.DisplayedImage.Left = -(Display.DisplayedImage.Size.Width - Display.ScreenWidth);
				}
			}

			//scroll left
			if (e == Keys.Left && Display.DisplayedImage.Image != null)
			{
				if (ImageEdit.IsImageWidtherOrEquelThenScreen(Display.DisplayedImage.Image))
				{
					if (Display.DisplayedImage.Left <= 0 - Display.scrollValueHorizontal)
						Display.DisplayedImage.Left += Display.scrollValueHorizontal;
					else if ((-Display.DisplayedImage.Left >= 0 - Display.scrollValueHorizontal) &&
					         !(Display.DisplayedImage.Left == 0))
						Display.DisplayedImage.Left = 0;
				}
			}

		}
	}
}
