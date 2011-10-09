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
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.IO;
using System.Windows.Media;

namespace Csharp_comicviewer.Other
{
    /// <summary>
    /// Calculate stat location,background color and resize images
    /// </summary>
    public class ImageEdit
    {
        private Point LocationImage = new Point(0, 0);
        private System.Drawing.Color BackColor;
        private int ViewportHeight = 0;
        private int ViewportWidth = 0;


        public void SetScreenHeight(int Height)
        {
            ViewportHeight = Height;
        }


        public void SetScreenWidth(int Width)
        {
            ViewportWidth = Width;
        }


        public System.Windows.Media.Brush GetBackgroundColor(byte[] ImageAsByteArray)
        {
            Bitmap objBitmap = new Bitmap(ByteArrayToImage(ImageAsByteArray));
            int DividedBy = 100;
            System.Drawing.Color[] Colors = new System.Drawing.Color[DividedBy * 4];

            //get the color of a pixels at the edge of image
            int i = 0;
            //left
            for (int y = 0; y < DividedBy; y++)
            {
                Colors[i++] = objBitmap.GetPixel(0, y * (objBitmap.Height / DividedBy));
            }
            //top
            for (int x = 0; x < DividedBy; x++)
            {

                Colors[i++] = objBitmap.GetPixel(x * (objBitmap.Width / DividedBy), 0);
            }
            //right
            for (int y = 0; y < DividedBy; y++)
            {
                Colors[i++] = objBitmap.GetPixel(objBitmap.Width - 1, y * (objBitmap.Height / DividedBy));
            }
            //bottom
            for (int x = 0; x < DividedBy; x++)
            {

                Colors[i++] = objBitmap.GetPixel(x * (objBitmap.Width / DividedBy), objBitmap.Height - 1);
            }
            //get mode of colors
            int Color = GetModeOfColorArray(Colors);
            //set bgcolor

            if (Color != -1)
                BackColor = Colors[Color];
            System.Windows.Media.Color BackColorWPF = new System.Windows.Media.Color();
            BackColorWPF.A= BackColor.A;
            BackColorWPF.B = BackColor.B;
            BackColorWPF.G = BackColor.G;
            BackColorWPF.R = BackColor.R;
            return new SolidColorBrush(BackColorWPF); 
        }

        /// <summary>
        /// Gets the mode of a Color[]
        /// </summary>
        /// <param name="colors">Array of Colors</param>
        /// <returns>Index of mode, -1 if non found</returns>
        private int GetModeOfColorArray(System.Drawing.Color[] colors)
        {
            System.Drawing.Color[] distinctcolors = colors.Distinct().ToArray();
            int[] countcolors = new int[distinctcolors.Length];
            int highest = 1;
            int highestindex = -1;
            Boolean mode = false;

            //count how many time distinct values are in colors
            for (int i = 0; i < distinctcolors.Length; i++)
            {
                for (int x = 0; x < colors.Length; x++)
                {
                    if (colors[x] == distinctcolors[i])
                        countcolors[i]++;
                }
            }
            //check what the highest value is
            for (int i = 0; i < countcolors.Length; i++)
            {
                if (countcolors[i] > highest)
                {
                    highest = countcolors[i];
                    highestindex = i;
                    mode = true;
                }
            }


            if (mode)
                return Array.IndexOf(colors, distinctcolors[highestindex]);
            else
                return -1;
        }


        public Boolean IsImageWidtherOrEquelThenViewport(Image image)
        {
            if (image.Width >= ViewportWidth)
                return true;
            else return false;
        }


        public Boolean IsImageHigherOrEquelThenViewport(Image image)
        {
            if (image.Height >= ViewportHeight)
                return true;
            else return false;
        }


        public Image ByteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            ms.Close();
            return returnImage;
        }

        public byte[] ImageToByteArray(System.Drawing.Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
            byte[] ReturnValue = ms.ToArray();
            ms.Close();
            return ReturnValue;
        }


        
        public byte[] ResizeImage(byte[] ImageAsByteArray, Size size, bool overideHight, bool overideWidth)
        {
            Image Image = ByteArrayToImage(ImageAsByteArray);


            int sourceWidth = Image.Width;
            int sourceHeight = Image.Height;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            if (!overideHight && overideWidth)
            {
                size.Width = ViewportWidth;
            }

            nPercentW = ((float)size.Width / (float)sourceWidth);
            nPercentH = ((float)size.Height / (float)sourceHeight);

            if (!overideHight && !overideWidth)
            {
                if (nPercentH < nPercentW)
                    nPercent = nPercentH;
                else
                    nPercent = nPercentW;
            }
            else if (overideHight && !overideWidth)
                nPercent = nPercentH;
            else if (!overideHight && overideWidth)
            {
                nPercent = nPercentW;
            }

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            if (overideHight && overideWidth)
            {
                destWidth = (int)(ViewportWidth);
                destHeight = (int)(ViewportHeight);
            }


            Bitmap b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage((Image)b);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            g.DrawImage(Image, 0, 0, destWidth, destHeight);
            g.Dispose();

            return   ImageToByteArray((Image)b);
        }
    }
}
