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
using System.Drawing;
using System.IO;

namespace csharp_comicviewer
{
    /// <summary>
    /// A ComicFile contains the images and possible some information text from one archive
    /// </summary>
    class ComicFile
    {
        List<byte[]> ImagesAsBytes = new List<byte[]>();
        String InfoText;
        int TotalPages;
        String Location;

        /// <summary>
        /// Create a ComicFile
        /// </summary>
        /// <param name="Location">Location of the archive</param>
        /// <param name="Images">The images of the archive</param>
        /// <param name="InfoText">Information text within the archive</param>
        public ComicFile(String Location, List<byte[]> Images, String InfoText)
        {
            this.Location = Location;
            this.ImagesAsBytes.AddRange(Images);
            CountTotalPages();
            if (InfoText != null)
                this.InfoText = InfoText;
        }

        /// <summary>
        /// Count the total pages(images) inside the ComicFile
        /// </summary>
        private void CountTotalPages()
        {
            TotalPages = ImagesAsBytes.Count;
        }

        /// <summary>
        /// Get the total pages(images) of the ComicFile
        /// </summary>
        /// <returns>Total pages of the ComicFile</returns>
        public int GetTotalPages()
        {
            return TotalPages;
        }

        /// <summary>
        /// Get a page(image)
        /// </summary>
        /// <param name="PageNumber">The page number</param>
        /// <returns>The page(image) with the corresponding number</returns>
        public Image GetPage(int PageNumber)
        {
            using (MemoryStream ms = new MemoryStream(ImagesAsBytes[PageNumber], 0, ImagesAsBytes[PageNumber].Length))
            {
                return Image.FromStream(ms, true);
            }

        }

        /// <summary>
        /// Get the information text from the ComicFile
        /// </summary>
        /// <returns>The information text</returns>
        public String GetInfoText()
        {
            return InfoText;
        }

        /// <summary>
        /// Get the location of the ComicFile
        /// </summary>
        /// <returns>Location of the ComicFile(archive)</returns>
        public String GetLocation()
        {
            return Location;
        }
    }
}
