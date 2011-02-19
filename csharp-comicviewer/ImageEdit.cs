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
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace csharp_comicviewer
{
	class ImageEdit
	{
		private Point LocationImage = new Point(0, 0);
		private Color BackColor;

		public Point CalculateLocationImage(Image image)
		{
			int Primary_Monitor_Width = SystemInformation.PrimaryMonitorSize.Width;
			int Primary_Monitor_Height = SystemInformation.PrimaryMonitorSize.Height;

			if (!IsImageWidtherOrEquelThenScreen(image) && !IsImageHigherOrEquelThenScreen(image))
			{
				//image is smaller then the hight and the width of the screen
				LocationImage = new Point((Primary_Monitor_Width / 2) - (image.Width / 2), (Primary_Monitor_Height / 2) - (image.Height / 2));
			}
			else if (IsImageWidtherOrEquelThenScreen(image) && IsImageHigherOrEquelThenScreen(image))
			{
				//image is bigger then the hight and the width of the screen
				LocationImage = new Point(0, 0);
			}
			else if (!IsImageWidtherOrEquelThenScreen(image) && IsImageHigherOrEquelThenScreen(image))
			{
				//image is bigger then the hight and smaller then the width of the screen
				LocationImage = new Point((Primary_Monitor_Width / 2) - (image.Width / 2), 0);
			}
			else if (IsImageWidtherOrEquelThenScreen(image) && !IsImageHigherOrEquelThenScreen(image))
			{
				//image is smaller then the hight and bigger then the width of the screen
				LocationImage = new Point(0, (Primary_Monitor_Height / 2) - (image.Height / 2));
			}
			//if all else fails just load image
			else LocationImage = new Point(0, 0);

			return LocationImage;
		}

		/*
		 * change background color to make it easier on the eyes
		 * image - the image used to get values
		 */
		public Color setBackColor(Image image)
		{
			Bitmap objBitmap = new Bitmap(image);
			int DividedBy = 100;
			Color[] Colors = new Color[DividedBy * 4];

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
			int Color = ModeOfColorArray(Colors);
			//set bgcolor

			if (Color != -1)
				BackColor = Colors[Color];

			return BackColor;
		}

		/*
		 * Gets mode from Color[]. Returns the index of the mode, if there isnt any it returns -1
		 * colors - an Color[]
		 */
		private int ModeOfColorArray(Color[] colors)
		{
			Color[] distinctcolors = colors.Distinct().ToArray();
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

		public Boolean IsImageWidtherOrEquelThenScreen(Image image)
		{
			if (image.Width >= SystemInformation.PrimaryMonitorSize.Width)
				return true;
			else return false;
		}

		public Boolean IsImageHigherOrEquelThenScreen(Image image)
		{
			if (image.Height >= SystemInformation.PrimaryMonitorSize.Height)
				return true;
			else return false;
		}

		/*
		 * Resize an image while keeping aspect ratio
		 * imgToResize - the image to resize
		 * size - the new size of the image (note will take smallest value in order to keep aspect example: w:10 h:5  it will use the height)
		 * overideHight - resized the image so it will fit with the height
		 * overideWidth - resized the image so it will fit with the width
		 */
		public Image resizeImage(Image imgToResize, Size size, Boolean overideHight,Boolean overideWidth)
		{
			int Primary_Monitor_Width = SystemInformation.PrimaryMonitorSize.Width;
			int Primary_Monitor_Height = SystemInformation.PrimaryMonitorSize.Height;

			int sourceWidth = imgToResize.Width;
			int sourceHeight = imgToResize.Height;

			float nPercent = 0;
			float nPercentW = 0;
			float nPercentH = 0;

			if (!overideHight && overideWidth)
			{
				size.Width= Primary_Monitor_Width ;
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
				destWidth = (int)(Primary_Monitor_Width);
				destHeight = (int)(Primary_Monitor_Height);
			}


			Bitmap b = new Bitmap(destWidth, destHeight);
			Graphics g = Graphics.FromImage((Image)b);
			g.InterpolationMode = InterpolationMode.HighQualityBicubic;

			g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
			g.Dispose();

			return (Image)b;
		}
	}
}
