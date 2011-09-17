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
namespace Csharp_comicviewer.Events
{
    using System;
    using Csharp_comicviewer.Comic;
    using Csharp_comicviewer.WPF;
    using System.Windows;
    using System.Windows.Input;

    /// <summary>
    /// Description of MouseEvent.
    /// </summary>
    public class MouseHandler
    {
        public MouseHandler(Display_Form PrimaryDisplay, ComicBook ComicBook)
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
            if (SenderDisplay == PrimaryDisplay)
            {
                if (SecondaryDisplay != null)
                {
                    if (PrimaryDisplay.GoToNextPage && SecondaryDisplay.GoToNextPage)
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
            if (SenderDisplay == PrimaryDisplay)
            {
                if (SecondaryDisplay != null)
                {
                    if (PrimaryDisplay.GoToNextPage && SecondaryDisplay.GoToNextPage)
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
            MessageBox.Show("Mot nog");
            TimeSpan elaped = DateTime.Now - LastMouseMove;
            if (elaped >= TimeoutToHide && !MouseIsHidden)
            {
                if (false)
                {
                    Mouse.OverrideCursor = Cursors.None;
                    MouseIsHidden = true;
                }
                else if (true)
                {
                    LastMouseMove = DateTime.Now;
                }
            }
        }

        public void OnMouseMove(Display_Form SenderForm, object sender, MouseEventArgs e)
        {
            double? Horizontal;
            double? Vertical;
            if (SenderForm == PrimaryDisplay)
            {
                MouseMove(PrimaryDisplay, sender, e, out Horizontal, out Vertical);
                if (SecondaryDisplay != null)
                {
                    MoveDisplayedImageOnDisplay(SecondaryDisplay, Horizontal, Vertical);
                }
            }
            else
            {
                MouseMove(SecondaryDisplay, sender, e, out Horizontal, out Vertical);
                MoveDisplayedImageOnDisplay(PrimaryDisplay, Horizontal, Vertical);
            }
        }

        private void MouseMove(Display_Form SenderDisplay, object sender, MouseEventArgs e, out double? Horizontal, out double? Vertical)
        {
            // TODO Make it work

            Horizontal = null;
            Vertical = null;


            LastMouseMove = DateTime.Now;

            if (MouseIsHidden && (Mouse.GetPosition(SenderDisplay) != CurrentMousePosition))
            {
                Mouse.OverrideCursor = Cursors.Arrow;
                MouseIsHidden = false;
            }

            CurrentMousePosition = Mouse.GetPosition(SenderDisplay);

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
                    if (CurrentMousePosition.X < MouseX && SenderDisplay.DisplayedImage.Source != null)
                    {
                        SenderDisplay.ScrollField.ScrollToHorizontalOffset(SenderDisplay.ScrollField.HorizontalOffset + (MouseX - CurrentMousePosition.X) / Speed);
                        Horizontal = SenderDisplay.ScrollField.HorizontalOffset;
                        MouseDrag = false;
                    }
                    //Drag right
                    else if (CurrentMousePosition.X > MouseX && SenderDisplay.DisplayedImage.Source != null)
                    {
                        SenderDisplay.ScrollField.ScrollToHorizontalOffset(SenderDisplay.ScrollField.HorizontalOffset +(MouseX - CurrentMousePosition.X) / Speed);
                        Horizontal = SenderDisplay.ScrollField.HorizontalOffset;
                        MouseDrag = false;
                    }
                    //Drag up
                    if (CurrentMousePosition.Y < MouseY && SenderDisplay.DisplayedImage.Source != null)
                    {
                        SenderDisplay.ScrollField.ScrollToVerticalOffset(SenderDisplay.ScrollField.VerticalOffset + (MouseY - CurrentMousePosition.Y) / Speed);
                        Horizontal = SenderDisplay.ScrollField.VerticalOffset;
                        MouseDrag = false;
                    }
                    //Drag down
                    else if (CurrentMousePosition.Y > MouseY && SenderDisplay.DisplayedImage.Source != null)
                    {
                        SenderDisplay.ScrollField.ScrollToVerticalOffset(SenderDisplay.ScrollField.VerticalOffset + (MouseY - CurrentMousePosition.Y) / Speed);
                        Horizontal = SenderDisplay.ScrollField.VerticalOffset;
                        MouseDrag = false;
                    }

                    if (SenderDisplay.ScrollField.VerticalOffset > SenderDisplay.ScrollField.ScrollableHeight || SenderDisplay.ScrollField.VerticalOffset < 0)
                    {
                        SenderDisplay.ScrollField.ScrollToVerticalOffset(SenderDisplay.ScrollField.ScrollableHeight);
                    }
                    if (SenderDisplay.ScrollField.HorizontalOffset > SenderDisplay.ScrollField.ScrollableWidth || SenderDisplay.ScrollField.HorizontalOffset < 0)
                    {
                        SenderDisplay.ScrollField.ScrollToHorizontalOffset(SenderDisplay.ScrollField.ScrollableWidth);
                    }
                }
            }
            //make it possible to drag on next check
            else
                MouseDrag = false;
        }

        public void OnMouseButton(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed || e.RightButton == MouseButtonState.Pressed)
            {
                LastMouseMove = DateTime.Now;

                if (MouseIsHidden)
                {
                    Mouse.OverrideCursor = Cursors.Arrow;
                    MouseIsHidden = false;
                }
            }
        }

        private void MoveDisplayedImageOnDisplay(Display_Form SenderDisplay, double? Horizontal, double? Vertical)
        {
            if (Horizontal.HasValue)
            {
                SenderDisplay.ScrollField.ScrollToHorizontalOffset(Horizontal.Value);
            }
            if (Vertical.HasValue)
            {
                SenderDisplay.ScrollField.ScrollToVerticalOffset(Vertical.Value);
            }
        }


        public void MouseWheel(Display_Form SenderDisplay, MouseWheelEventArgs e)
        {
            //            //scroll down
            //            if (e.Delta < 0 && SenderDisplay.DisplayedImage.Source != null)
            //            {
            //                SenderDisplay.PreviousPageCount = 2;
            //                if (e.Delta < 0 && SenderDisplay.DisplayedImage.Source != null)
            //                {
            //                    if (SenderDisplay.ImageEdit.IsImageHigherOrEquelThenScreen(SenderDisplay.DisplayedImage.Source))
            //                    {
            //                        if (-SenderDisplay.DisplayedImage.Top < (SenderDisplay.DisplayedImage.Size.Height - SenderDisplay.ScreenHeight) - SenderDisplay.scrollValueVertical)
            //                            SenderDisplay.DisplayedImage.Top -= SenderDisplay.scrollValueVertical;
            //                        else if ((-SenderDisplay.DisplayedImage.Top >= (SenderDisplay.DisplayedImage.Size.Height - SenderDisplay.ScreenHeight) - SenderDisplay.scrollValueVertical) &&
            //                                 !(SenderDisplay.DisplayedImage.Top == -(SenderDisplay.DisplayedImage.Size.Height - SenderDisplay.ScreenHeight)))
            //                            SenderDisplay.DisplayedImage.Top = -(SenderDisplay.DisplayedImage.Size.Height - SenderDisplay.ScreenHeight);
            //                        else if (SenderDisplay.DisplayedImage.Top == -(SenderDisplay.DisplayedImage.Size.Height - SenderDisplay.ScreenHeight) && !SenderDisplay.ImageEdit.IsImageWidtherOrEquelThenScreen(SenderDisplay.DisplayedImage.Image))
            //                        {
            //                            SenderDisplay.GoToNextPage = true;
            //                            SenderDisplay.NextPageCount--;
            //                        }
            //
            //                        else if (SenderDisplay.DisplayedImage.Left != -(SenderDisplay.DisplayedImage.Size.Width - SenderDisplay.ScreenWidth) && SenderDisplay.ImageEdit.IsImageWidtherOrEquelThenScreen(SenderDisplay.DisplayedImage.Image))
            //                        {
            //                            if (-SenderDisplay.DisplayedImage.Left < (SenderDisplay.DisplayedImage.Size.Width - SenderDisplay.ScreenWidth) - SenderDisplay.scrollValueHorizontal)
            //                                SenderDisplay.DisplayedImage.Left -= SenderDisplay.scrollValueHorizontal;
            //                            else if ((-SenderDisplay.DisplayedImage.Left >= (SenderDisplay.DisplayedImage.Size.Width - SenderDisplay.ScreenWidth) - SenderDisplay.scrollValueHorizontal) &&
            //                                     !(SenderDisplay.DisplayedImage.Left == -(SenderDisplay.DisplayedImage.Size.Width - SenderDisplay.ScreenWidth)))
            //                                SenderDisplay.DisplayedImage.Left = -(SenderDisplay.DisplayedImage.Size.Width - SenderDisplay.ScreenWidth);
            //                        }
            //                        else if (SenderDisplay.DisplayedImage.Left == -(SenderDisplay.DisplayedImage.Size.Width - SenderDisplay.ScreenWidth))
            //                        {
            //                            SenderDisplay.GoToNextPage = true;
            //                            SenderDisplay.NextPageCount--;
            //                        }
            //                    }
            //                    else
            //                    {
            //                        NextPage(SenderDisplay);
            //                    }
            //                }
            //            }
            //            //scroll up
            //            else if (e.Delta > 0 && SenderDisplay.DisplayedImage.Image != null)
            //            {
            //                int TopOfImageStart = 0;
            //                if (SenderDisplay.Configuration.windowed)
            //                    TopOfImageStart = 24;
            //
            //                if (SenderDisplay.ImageEdit.IsImageHigherOrEquelThenScreen(SenderDisplay.DisplayedImage.Image))
            //                {
            //                    if (SenderDisplay.DisplayedImage.Top < TopOfImageStart - SenderDisplay.scrollValueVertical)
            //                        SenderDisplay.DisplayedImage.Top += SenderDisplay.scrollValueVertical;
            //                    else if ((SenderDisplay.DisplayedImage.Top >= TopOfImageStart - SenderDisplay.scrollValueVertical) &&
            //                             !(SenderDisplay.DisplayedImage.Top == TopOfImageStart))
            //                        SenderDisplay.DisplayedImage.Top = TopOfImageStart;
            //                    else if (SenderDisplay.DisplayedImage.Top == TopOfImageStart && !SenderDisplay.ImageEdit.IsImageWidtherOrEquelThenScreen(SenderDisplay.DisplayedImage.Image))
            //                    {
            //                        SenderDisplay.GoToPreviousPage = true;
            //                        SenderDisplay.PreviousPageCount--;
            //                    }
            //
            //                    else if (SenderDisplay.DisplayedImage.Left != 0 && SenderDisplay.ImageEdit.IsImageWidtherOrEquelThenScreen(SenderDisplay.DisplayedImage.Image))
            //                    {
            //                        if (SenderDisplay.DisplayedImage.Left < 0 - SenderDisplay.scrollValueHorizontal)
            //                            SenderDisplay.DisplayedImage.Left += SenderDisplay.scrollValueHorizontal;
            //                        else if ((-SenderDisplay.DisplayedImage.Left > 0 - SenderDisplay.scrollValueHorizontal) &&
            //                                 !(SenderDisplay.DisplayedImage.Left == 0))
            //                            SenderDisplay.DisplayedImage.Left = 0;
            //                    }
            //                    else if (SenderDisplay.DisplayedImage.Left == 0)
            //                    {
            //                        SenderDisplay.GoToPreviousPage = true;
            //                        SenderDisplay.PreviousPageCount--;
            //                    }
            //                }
            //                else
            //                {
            //                    PreviousPage(SenderDisplay);
            //                }
            //            }
            //
            //            if (SenderDisplay.GoToNextPage && SenderDisplay.NextPageCount <= 0)
            //            {
            //                NextPage(SenderDisplay);
            //            }
            //            else if (SenderDisplay.GoToPreviousPage && SenderDisplay.PreviousPageCount <= 0)
            //            {
            //                PreviousPage(SenderDisplay);
            //            }
        }


    }
}
