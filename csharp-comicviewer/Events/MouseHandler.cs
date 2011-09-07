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
namespace Csharp_comicviewer
{
	using System;
	using System.Windows.Forms;
	using System.Drawing;
	using Csharp_comicviewer.Comic;
	
	/// <summary>
	/// Description of MouseEvent.
	/// </summary>
	public class MouseHandler
	{
		public MouseHandler(Display_Form PrimaryDisplay,ComicBook ComicBook)
		{
			this.PrimaryDisplay = PrimaryDisplay;
			this.ComicBook = ComicBook;
			SecondaryDisplay = null;
			MouseIsHidden = false;
			TimeoutToHide = TimeSpan.FromSeconds(2);
		}
		
		
		public Display_Form PrimaryDisplay
		{
			get;
			set;
		}

		public Display_Form SecondaryDisplay
		{
			get;
			set;
		}
		
		public ComicBook ComicBook
		{
			get;
			set;
		}
		
		private TimeSpan TimeoutToHide
		{
			get;
			set;
		}
		
		private bool MouseDrag
		{
			get;
			set;
		}
		
		public DateTime LastMouseMove
		{
			get;
			set;
		}
		
		public bool MouseIsHidden
		{
			get;
			set;
		}
		
		private Point CurrentMousePosition
		{
			get;
			set;
		}
		
		private double MouseX
		{
			get;
			set;
		}
		
		private double MouseY
		{
			get;
			set;
		}
		
		public bool RightMouseMenuOpened
		{
			get;
			set;
		}
		

		
		private void NextPage(Display_Form SenderDisplay)
		{
			if(SenderDisplay == PrimaryDisplay)
			{
				if(SecondaryDisplay != null)
				{
					if(PrimaryDisplay.GoToNextPage && SecondaryDisplay.GoToNextPage)
					{
						PrimaryDisplay.NextPage();
						SecondaryDisplay.NextPage();
						PrimaryDisplay.GoToNextPage = false;
						PrimaryDisplay.NextPageCount = 2;
						SenderDisplay.GoToNextPage = false;
						SenderDisplay.NextPageCount = 2;
					}
				}
				else
				{
					PrimaryDisplay.NextPage();
					PrimaryDisplay.GoToNextPage = false;
					PrimaryDisplay.NextPageCount = 2;
				}
			}
			else
			{
				SecondaryDisplay.NextPage();
				PrimaryDisplay.NextPage();
				PrimaryDisplay.GoToNextPage = false;
				PrimaryDisplay.NextPageCount = 2;
				SenderDisplay.GoToNextPage = false;
				SenderDisplay.NextPageCount = 2;
			}
		}
		
		private void PreviousPage(Display_Form SenderDisplay)
		{
			if(SenderDisplay == PrimaryDisplay)
			{
				if(SecondaryDisplay != null)
				{
					if(PrimaryDisplay.GoToNextPage && SecondaryDisplay.GoToNextPage)
					{
						PrimaryDisplay.PreviousPage();
						SecondaryDisplay.PreviousPage();
						PrimaryDisplay.GoToPreviousPage = false;
						PrimaryDisplay.PreviousPageCount = 2;
						SenderDisplay.GoToPreviousPage = false;
						SenderDisplay.PreviousPageCount = 2;
					}
				}
				else
				{
					PrimaryDisplay.PreviousPage();
					PrimaryDisplay.GoToPreviousPage = false;
					PrimaryDisplay.PreviousPageCount = 2;

				}
			}
			else
			{
				SecondaryDisplay.PreviousPage();
				PrimaryDisplay.PreviousPage();
				PrimaryDisplay.GoToPreviousPage = false;
				PrimaryDisplay.PreviousPageCount = 2;
				SenderDisplay.GoToPreviousPage = false;
				SenderDisplay.PreviousPageCount = 2;
			}
		}
		
		private void MouseIdleChecker(object sender, EventArgs e)
		{
			TimeSpan elaped = DateTime.Now - LastMouseMove;
			if (elaped >= TimeoutToHide && !MouseIsHidden)
			{
				Form ActiveForm = Form.ActiveForm;
				if ((PrimaryDisplay == ActiveForm
				     && !PrimaryDisplay.File_bar.Selected && !PrimaryDisplay.File_bar.Pressed
				     && !PrimaryDisplay.PageControl_bar.Selected && !PrimaryDisplay.PageControl_bar.Pressed
				     && !PrimaryDisplay.Bookmark_menu_bar.Selected && !PrimaryDisplay.Bookmark_menu_bar.Pressed
				     && !PrimaryDisplay.InformationText_item_bar.Selected && !PrimaryDisplay.About_item_bar.Selected
				     && !RightMouseMenuOpened
				     && SecondaryDisplay == ActiveForm
				     && !SecondaryDisplay.File_bar.Selected && !SecondaryDisplay.File_bar.Pressed
				     && !SecondaryDisplay.PageControl_bar.Selected && !SecondaryDisplay.PageControl_bar.Pressed
				     && !SecondaryDisplay.Bookmark_menu_bar.Selected && !SecondaryDisplay.Bookmark_menu_bar.Pressed
				     && !SecondaryDisplay.InformationText_item_bar.Selected && !SecondaryDisplay.About_item_bar.Selected
				     && !RightMouseMenuOpened))
				{
					Cursor.Hide();
					MouseIsHidden = true;
				}
				else if (PrimaryDisplay.File_bar.Selected || PrimaryDisplay.File_bar.Pressed
				         || PrimaryDisplay.PageControl_bar.Selected || PrimaryDisplay.PageControl_bar.Pressed
				         || PrimaryDisplay.Bookmark_menu_bar.Selected || PrimaryDisplay.Bookmark_menu_bar.Pressed
				         || PrimaryDisplay.InformationText_item_bar.Selected || PrimaryDisplay.About_item_bar.Selected
				         || SecondaryDisplay.File_bar.Selected || SecondaryDisplay.File_bar.Pressed
				         || SecondaryDisplay.PageControl_bar.Selected || SecondaryDisplay.PageControl_bar.Pressed
				         || SecondaryDisplay.Bookmark_menu_bar.Selected || SecondaryDisplay.Bookmark_menu_bar.Pressed
				         || SecondaryDisplay.InformationText_item_bar.Selected || SecondaryDisplay.About_item_bar.Selected
				         || RightMouseMenuOpened)
				{
					LastMouseMove = DateTime.Now;
				}
			}
		}
		
		public void OnMouseMove(Display_Form SenderForm, object sender, MouseEventArgs e)
		{
			int? Left;
			int? Top;
			if(SenderForm == PrimaryDisplay)
			{
				MouseMove(PrimaryDisplay,sender,e, out Left, out Top);
				if(SecondaryDisplay != null)
				{
					MoveDisplayedImageOnDisplay(SecondaryDisplay,Left,Top);
				}
			}
			else
			{
				MouseMove(SecondaryDisplay,sender,e, out Left, out Top);
				MoveDisplayedImageOnDisplay(PrimaryDisplay,Left,Top);
			}
		}

		private void MouseMove(Display_Form Display,object sender, MouseEventArgs e, out int? Left, out int? Top)
		{
			Left = null;
			Top = null;
			
			
			LastMouseMove = DateTime.Now;

			if (MouseIsHidden && (e.Location != CurrentMousePosition))
			{
				Cursor.Show();
				MouseIsHidden = false;
			}

			CurrentMousePosition = e.Location;

			int Speed = 2; //amount by with mouse_x/y - MousePosition.X/Y is divided, determines drag speed
			//am i dragging the mouse with left button pressed
			if (e.Button == MouseButtons.Left)
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
					if (CurrentMousePosition.X < MouseX && Display.DisplayedImage.Left < (Display.DisplayedImage.Size.Width - Display.ScreenWidth))
					{
						if (-Display.DisplayedImage.Left + Convert.ToInt32((MouseX - CurrentMousePosition.X) / Speed) <= (Display.DisplayedImage.Size.Width - Display.ScreenWidth))
						{
							Display.DisplayedImage.Left -= Convert.ToInt32((MouseX - CurrentMousePosition.X) / Speed);
							Left = Display.DisplayedImage.Left;
						}
						else
						{
							Display.DisplayedImage.Left = -(Display.DisplayedImage.Size.Width - Display.ScreenWidth);
							Left = Display.DisplayedImage.Left;
						}
						MouseDrag = false;
					}
					//Drag right
					else if (CurrentMousePosition.X > MouseX && -Display.DisplayedImage.Left > 0)
					{
						if (-Display.DisplayedImage.Left - Convert.ToInt32((CurrentMousePosition.X - MouseX) / Speed) >= 0)
						{
							Display.DisplayedImage.Left += Convert.ToInt32((CurrentMousePosition.X - MouseX) / Speed);
							Left = Display.DisplayedImage.Left;
						}
						else
						{
							Display.DisplayedImage.Left = 0;
							Left = Display.DisplayedImage.Left;
						}
						MouseDrag = false;
					}
					//Drag up
					if (CurrentMousePosition.Y < MouseY && Display.DisplayedImage.Top < (Display.DisplayedImage.Size.Height - Display.ScreenHeight))
					{
						if (-Display.DisplayedImage.Top + Convert.ToInt32((MouseY - CurrentMousePosition.Y) / Speed) <= (Display.DisplayedImage.Size.Height - Display.ScreenHeight))
						{
							Display.DisplayedImage.Top -= Convert.ToInt32((MouseY - CurrentMousePosition.Y) / Speed);
							Top = Display.DisplayedImage.Top;
						}
						else
						{
							Display.DisplayedImage.Top = -(Display.DisplayedImage.Size.Height - Display.ScreenHeight);
							Top = Display.DisplayedImage.Top;
						}
						MouseDrag = false;
					}
					//Drag down
					else if (CurrentMousePosition.Y > MouseY && -Display.DisplayedImage.Top > 0)
					{
						if (-Display.DisplayedImage.Top - Convert.ToInt32((CurrentMousePosition.Y - MouseY) / Speed) >= 0)
						{
							Display.DisplayedImage.Top += Convert.ToInt32((CurrentMousePosition.Y - MouseY) / Speed);
							Top = Display.DisplayedImage.Top;
						}
						else
						{
							Display.DisplayedImage.Top = 0;
							Top = Display.DisplayedImage.Top;
						}
						MouseDrag = false;
					}
				}
			}
			//make it possible to drag on next check
			else
				MouseDrag = false;
		}
		
		public void OnMouseButton(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Right)
			{
				LastMouseMove = DateTime.Now;

				if (MouseIsHidden)
				{
					Cursor.Show();
					MouseIsHidden = false;
				}
			}
		}
		
		private void MoveDisplayedImageOnDisplay(Display_Form Display, int? Left, int? Top)
		{
			if(Left != null)
			{
				Display.DisplayedImage.Left = Left.Value;
			}
			if(Top != null)
			{
				Display.DisplayedImage.Top = Top.Value;
			}
		}


		public void MouseWheel(Display_Form SenderDisplay, MouseEventArgs e)
		{ 
			//scroll down
			if (e.Delta < 0 && SenderDisplay.DisplayedImage.Image != null)
			{
				SenderDisplay.PreviousPageCount = 2;
				if (e.Delta < 0 && SenderDisplay.DisplayedImage.Image != null)
				{
					if (SenderDisplay.ImageEdit.IsImageHigherOrEquelThenScreen(SenderDisplay.DisplayedImage.Image))
					{
						if (-SenderDisplay.DisplayedImage.Top < (SenderDisplay.DisplayedImage.Size.Height - SenderDisplay.ScreenHeight) - SenderDisplay.scrollValueVertical)
							SenderDisplay.DisplayedImage.Top -= SenderDisplay.scrollValueVertical;
						else if ((-SenderDisplay.DisplayedImage.Top >= (SenderDisplay.DisplayedImage.Size.Height - SenderDisplay.ScreenHeight) - SenderDisplay.scrollValueVertical) &&
						         !(SenderDisplay.DisplayedImage.Top == -(SenderDisplay.DisplayedImage.Size.Height - SenderDisplay.ScreenHeight)))
							SenderDisplay.DisplayedImage.Top = -(SenderDisplay.DisplayedImage.Size.Height - SenderDisplay.ScreenHeight);
						else if (SenderDisplay.DisplayedImage.Top == -(SenderDisplay.DisplayedImage.Size.Height - SenderDisplay.ScreenHeight) && !SenderDisplay.ImageEdit.IsImageWidtherOrEquelThenScreen(SenderDisplay.DisplayedImage.Image))
						{
							SenderDisplay.GoToNextPage = true;
							SenderDisplay.NextPageCount--;
						}

						else if (SenderDisplay.DisplayedImage.Left != -(SenderDisplay.DisplayedImage.Size.Width - SenderDisplay.ScreenWidth) && SenderDisplay.ImageEdit.IsImageWidtherOrEquelThenScreen(SenderDisplay.DisplayedImage.Image))
						{
							if (-SenderDisplay.DisplayedImage.Left < (SenderDisplay.DisplayedImage.Size.Width - SenderDisplay.ScreenWidth) - SenderDisplay.scrollValueHorizontal)
								SenderDisplay.DisplayedImage.Left -= SenderDisplay.scrollValueHorizontal;
							else if ((-SenderDisplay.DisplayedImage.Left >= (SenderDisplay.DisplayedImage.Size.Width - SenderDisplay.ScreenWidth) - SenderDisplay.scrollValueHorizontal) &&
							         !(SenderDisplay.DisplayedImage.Left == -(SenderDisplay.DisplayedImage.Size.Width - SenderDisplay.ScreenWidth)))
								SenderDisplay.DisplayedImage.Left = -(SenderDisplay.DisplayedImage.Size.Width - SenderDisplay.ScreenWidth);
						}
						else if (SenderDisplay.DisplayedImage.Left == -(SenderDisplay.DisplayedImage.Size.Width - SenderDisplay.ScreenWidth))
						{
							SenderDisplay.GoToNextPage = true;
							SenderDisplay.NextPageCount--;
						}
					}
					else
					{
						NextPage(SenderDisplay);
					}
				}
			}
			//scroll up
			else if (e.Delta > 0 && SenderDisplay.DisplayedImage.Image != null)
			{
				int TopOfImageStart = 0;
				if (SenderDisplay.Configuration.windowed)
					TopOfImageStart = 24;

				if (SenderDisplay.ImageEdit.IsImageHigherOrEquelThenScreen(SenderDisplay.DisplayedImage.Image))
				{
					if (SenderDisplay.DisplayedImage.Top < TopOfImageStart - SenderDisplay.scrollValueVertical)
						SenderDisplay.DisplayedImage.Top += SenderDisplay.scrollValueVertical;
					else if ((SenderDisplay.DisplayedImage.Top >= TopOfImageStart - SenderDisplay.scrollValueVertical) &&
					         !(SenderDisplay.DisplayedImage.Top == TopOfImageStart))
						SenderDisplay.DisplayedImage.Top = TopOfImageStart;
					else if (SenderDisplay.DisplayedImage.Top == TopOfImageStart && !SenderDisplay.ImageEdit.IsImageWidtherOrEquelThenScreen(SenderDisplay.DisplayedImage.Image))
					{
						SenderDisplay.GoToPreviousPage = true;
						SenderDisplay.PreviousPageCount--;
					}

					else if (SenderDisplay.DisplayedImage.Left != 0 && SenderDisplay.ImageEdit.IsImageWidtherOrEquelThenScreen(SenderDisplay.DisplayedImage.Image))
					{
						if (SenderDisplay.DisplayedImage.Left < 0 - SenderDisplay.scrollValueHorizontal)
							SenderDisplay.DisplayedImage.Left += SenderDisplay.scrollValueHorizontal;
						else if ((-SenderDisplay.DisplayedImage.Left > 0 - SenderDisplay.scrollValueHorizontal) &&
						         !(SenderDisplay.DisplayedImage.Left == 0))
							SenderDisplay.DisplayedImage.Left = 0;
					}
					else if (SenderDisplay.DisplayedImage.Left == 0)
					{
						SenderDisplay.GoToPreviousPage = true;
						SenderDisplay.PreviousPageCount--;
					}
				}
				else
				{
					PreviousPage(SenderDisplay);
				}
			}

			if (SenderDisplay.GoToNextPage && SenderDisplay.NextPageCount <= 0)
			{
				NextPage(SenderDisplay);
			}
			else if (SenderDisplay.GoToPreviousPage && SenderDisplay.PreviousPageCount <= 0)
			{
				PreviousPage(SenderDisplay);
			}
		}

		
	}
}
