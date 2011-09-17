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
    using System.Windows;
    using System.Windows.Input;
    using Csharp_comicviewer.Comic;
    using Csharp_comicviewer.Other;
    using Csharp_comicviewer.WPF;

    /// <summary>
    /// Description of KeyboardHandler.
    /// </summary>
    public class KeyboardHandler
    {
        /// <summary>
        /// Initializes a new instance of the KeyboardHandler class.
        /// </summary>
        /// <param name="primaryDisplay">The primary display <see cref="Csharp_comicviewer.WPF.Display_Form"/>.</param>
        /// <param name="comicBook">The comic book.</param>
        /// <param name="imageEditPrimaryDisplay">The image edit object <see cref=" Csharp_comicviewer.Other.ImageEdit"/> used by the primary display <see cref="Csharp_comicviewer.WPF.Display_Form"/>.</param>
        /// <param name="mouseHandler">The MouseHandler <see cref="Csharp_comicviewer.Events.MouseHandler"/> for the primary display <see cref="Csharp_comicviewer.WPF.Display_Form"/>.</param>
        public KeyboardHandler(Display_Form primaryDisplay, ComicBook comicBook, ImageEdit imageEditPrimaryDisplay, MouseHandler mouseHandler)
        {
            PrimaryDisplay = primaryDisplay;
            ComicBook = comicBook;
            SecondaryDisplay = null;
            ImageEditPrimaryDisplay = imageEditPrimaryDisplay;
            ImageEditSecondaryDisplay = null;
            MouseHandler = mouseHandler;
        }

        /// <summary>
        /// Gets or sets the primary display form
        /// </summary>
        public Display_Form PrimaryDisplay
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the optional secondary display form
        /// </summary>
        public Display_Form SecondaryDisplay
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the comic book that will be displayed
        /// </summary>
        public ComicBook ComicBook
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the MouseHandler that is used
        /// </summary>
        public MouseHandler MouseHandler
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the image edit object <see cref=" Csharp_comicviewer.Other.ImageEdit"/> used to edit the image (size) for the primary display
        /// </summary>
        public ImageEdit ImageEditPrimaryDisplay
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the image edit object <see cref=" Csharp_comicviewer.Other.ImageEdit"/> used to edit the image (size) for the secondary display
        /// </summary>
        public ImageEdit ImageEditSecondaryDisplay
        {
            get;
            set;
        }

        /// <summary>
        /// The preview key down event.
        /// </summary>
        /// <param name="senderDisplay">The display <see cref="Csharp_comicviewer.WPF.Display_Form"/> that triggered the event.</param>
        /// <param name="e">The event arguments.</param>
        public void PreviewKeyDown(Display_Form senderDisplay, KeyEventArgs e)
        {
            // TODO make it work
            if (e.Key == Key.Home && !Keyboard.Modifiers.HasFlag(ModifierKeys.Alt))
            {
                // first page of all
                if (ComicBook.GetTotalFiles() != 0)
                {
                    byte[] image = ComicBook.GetPage(0, 0);
                    if (image != null)
                    {
                        senderDisplay.DisplayImage(image, "top");
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
                        senderDisplay.DisplayImage(image, "top");
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
                        senderDisplay.DisplayImage(image, "top");
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
                        senderDisplay.DisplayImage(image, "top");
                    }
                }
            }

            if (e.Key == Key.PageDown)
            {
                NextPage(senderDisplay);
            }

            if (e.Key == Key.PageUp)
            {
                PreviousPage(senderDisplay);
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
                ArrowKeyEventAllDisplays(senderDisplay, Key.Down);
            }

            if (e.Key == Key.Up)
            {
                ArrowKeyEventAllDisplays(senderDisplay, Key.Up);
            }

            if (e.Key == Key.Right)
            {
                ArrowKeyEventAllDisplays(senderDisplay, Key.Right);
            }

            if (e.Key == Key.Left)
            {
                ArrowKeyEventAllDisplays(senderDisplay, Key.Left);
            }
        }

        /// <summary>
        /// The key down event.
        /// </summary>
        /// <param name="senderDisplay">The display <see cref="Csharp_comicviewer.WPF.Display_Form"/> that triggered the event.</param>
        /// <param name="e">The event arguments.</param>
        public void KeyDown(Display_Form senderDisplay, KeyEventArgs e)
        {
            // TODO make it work
            if (e.Key == Key.X)
            {
                PrimaryDisplay.ApplicationExit(null, e);
            }

            if (e.Key == Key.R)
            {
                //    if (Resume_item.Enabled)
                //        ResumeLastFiles_Click(sender, e);
                //    else
                senderDisplay.ShowMessage("No archive to resume");
            }
            if (e.Key == Key.I)
            {
                PrimaryDisplay.ShowPageInformation();
            }

            if (e.Key == Key.L)
            {
                MouseHandler.LastMouseMove = DateTime.Now;

                if (MouseHandler.MouseIsHidden)
                {
                    Mouse.OverrideCursor = Cursors.Arrow;
                    MouseHandler.MouseIsHidden = false;
                }

                senderDisplay.LoadAndDisplayComic(true);
            }

            if (e.Key == Key.M)
            {
                PrimaryDisplay.WindowState = System.Windows.WindowState.Minimized;
                if (SecondaryDisplay != null)
                {
                    SecondaryDisplay.WindowState = System.Windows.WindowState.Minimized;
                }
            }

            if (e.Key == Key.T)
            {
                MessageBox.Show("functie nog niet af");

                // PrimaryDisplay.ToggleImageOptions();
                if (SecondaryDisplay != null)
                {
                    if (SecondaryDisplay.DisplayedImage.Source != null)
                    {
                        SecondaryDisplay.DisplayImage(ComicBook.GetCurrentPage(), "top");
                    }
                }
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
            if (e.Key == Key.W && SecondaryDisplay == null)
            {
                MessageBox.Show("functie nog niet af");

                //                if (!PrimaryDisplay.Configuration.windowed)
                //                {
                //                    PrimaryDisplay.Configuration.windowed = true;
                //                    PrimaryDisplay.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
                //                    PrimaryDisplay.ControlBox = true;
                //                    PrimaryDisplay.WindowState = FormWindowState.Maximized;
                //                    PrimaryDisplay.ResizeFix(null, null);
                //                    PrimaryDisplay.MenuBar.Visible = true;
                //                    PrimaryDisplay.MenuBar.Enabled = true;
                //                }
                //                else
                //                {
                //                    PrimaryDisplay.Configuration.windowed = false;
                //                    PrimaryDisplay.MenuBar.Visible = false;
                //                    PrimaryDisplay.MenuBar.Enabled = false;
                //                    PrimaryDisplay.ControlBox = false;
                //                    PrimaryDisplay.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                //                    PrimaryDisplay.WindowState = FormWindowState.Maximized;
                //                    PrimaryDisplay.SetWinFullScreen();
                //                    PrimaryDisplay.ResizeFix(null, null);
                //                }
            }
        }

        /// <summary>
        /// Next Page event
        /// </summary>
        /// <param name="senderDisplay">The display <see cref="Csharp_comicviewer.WPF.Display_Form"/> that triggered the event.</param>
        private void NextPage(Display_Form senderDisplay)
        {
            if (senderDisplay == PrimaryDisplay)
            {
                PrimaryDisplay.NextPage();
                if (SecondaryDisplay != null)
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

        /// <summary>
        /// Previous Page event
        /// </summary>
        /// <param name="senderDisplay">The display <see cref="Csharp_comicviewer.WPF.Display_Form"/> that triggered the event.</param>
        private void PreviousPage(Display_Form senderDisplay)
        {
            if (senderDisplay == PrimaryDisplay)
            {
                PrimaryDisplay.PreviousPage();
                if (SecondaryDisplay != null)
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

        /// <summary>
        /// Arrow key event for all displays
        /// </summary>
        /// <param name="senderDisplay">The display <see cref="Csharp_comicviewer.WPF.Display_Form"/> that triggered the event.</param>
        /// <param name="e">The key that triggered the event</param>
        private void ArrowKeyEventAllDisplays(Display_Form senderDisplay, Key e)
        {
            if (senderDisplay == PrimaryDisplay)
            {
                ArrowKeyEvent(PrimaryDisplay, ImageEditPrimaryDisplay, e);
                if (SecondaryDisplay != null)
                {
                    ArrowKeyEvent(SecondaryDisplay, ImageEditSecondaryDisplay, e);
                }
            }
            else
            {
                ArrowKeyEvent(SecondaryDisplay, ImageEditSecondaryDisplay, e);
                ArrowKeyEvent(PrimaryDisplay, ImageEditPrimaryDisplay, e);
            }
        }

        /// <summary>
        /// Arrow key event
        /// </summary>
        /// <param name="senderDisplay">The display <see cref="Csharp_comicviewer.WPF.Display_Form"/> that triggered the event.</param>
        /// <param name="imageEdit">The image edit object <see cref=" Csharp_comicviewer.Other.ImageEdit"/> used</param>
        /// <param name="e">The key that triggered the event</param>
        private void ArrowKeyEvent(Display_Form senderDisplay, ImageEdit imageEdit, Key e)
        {
            //scroll down
            if (e == Key.Down && senderDisplay.DisplayedImage.Source != null)
            {
                senderDisplay.ScrollField.ScrollToVerticalOffset(senderDisplay.ScrollField.VerticalOffset + 50);
            }

            //scroll up
            if (e == Key.Up && senderDisplay.DisplayedImage.Source != null)
            {
                senderDisplay.ScrollField.ScrollToVerticalOffset(senderDisplay.ScrollField.VerticalOffset - 50);
            }

            //scroll right
            if (e == Key.Right && senderDisplay.DisplayedImage.Source != null)
            {
                senderDisplay.ScrollField.ScrollToHorizontalOffset(senderDisplay.ScrollField.HorizontalOffset + 50);
            }

            //scroll left
            if (e == Key.Left && senderDisplay.DisplayedImage.Source != null)
            {
                senderDisplay.ScrollField.ScrollToHorizontalOffset(senderDisplay.ScrollField.HorizontalOffset - 50);
            }

            if (senderDisplay.ScrollField.VerticalOffset > senderDisplay.ScrollField.ScrollableHeight || senderDisplay.ScrollField.VerticalOffset < 0)
            {
                senderDisplay.ScrollField.ScrollToVerticalOffset(senderDisplay.ScrollField.ScrollableHeight);
            }

            if (senderDisplay.ScrollField.HorizontalOffset > senderDisplay.ScrollField.ScrollableWidth || senderDisplay.ScrollField.HorizontalOffset < 0)
            {
                senderDisplay.ScrollField.ScrollToHorizontalOffset(senderDisplay.ScrollField.ScrollableWidth);
            }
        }

        /// <summary>
        /// Loads the first file found afther the current one and displays the first image if possible.
        /// </summary>
        private void NextFile()
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
                        PrimaryDisplay.LoadAndDisplayComic(Files);

                        // TODO make it work with secondary
                    }
                    Mouse.OverrideCursor = Cursors.Arrow;
                }
                else
                { 
                    // TODO make it work with secondary
                    byte[] image = ComicBook.GetPage(ComicBook.GetCurrentFile() + 1, 0);
                    if (image != null)
                    {
                        PrimaryDisplay.DisplayImage(image, "top");
                    }
                }
            }
        }


        private void PreviousFile()
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
                        PrimaryDisplay.LoadAndDisplayComic(Files);
                        
                        // TODO make it work with secondary
                    }
                    Mouse.OverrideCursor = Cursors.Arrow;
                }
                else
                {
                    // TODO make it work with secondary
                    byte[] image = ComicBook.GetPage(ComicBook.GetCurrentFile() - 1, ComicBook.GetTotalPagesOfFile(ComicBook.GetCurrentFile() - 1) - 1);
                    if (image != null)
                    {
                        PrimaryDisplay.DisplayImage(image, "bottom");
                    }
                }
            }
        }
    }
}
