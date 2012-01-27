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
using System.Collections.Generic;
using System.IO;

namespace CSharpComicViewer.Comic
{
    /// <summary>
    /// A ComicFile contains the images and possible some information text from one archive
    /// </summary>
    public class ComicFile
    {
        private List<byte[]> ImagesAsBytes = new List<byte[]>();

        /// <summary>
        /// Create a ComicFile
        /// </summary>
        /// <param name="location">Location of the archive</param>
        /// <param name="images">The images of the archive</param>
        /// <param name="infoText">Information text within the archive</param>
        public ComicFile(String location, List<byte[]> images, String infoText)
        {
            this.Location = location;
            this.ImagesAsBytes.AddRange(images);
            CountTotalPages();
            if (infoText != null)
                this.InfoText = infoText;
        }

        /// <summary>
        /// Count the total pages(images) inside the ComicFile
        /// </summary>
        private void CountTotalPages()
        {
            TotalPages = ImagesAsBytes.Count;
        }

        /// <summary>
        /// Get a page(image)
        /// </summary>
        /// <param name="PageNumber">The page number</param>
        /// <returns>The page(image) with the corresponding number</returns>
        public byte[] GetPage(int PageNumber)
        {
            return ImagesAsBytes[PageNumber];
        }

        public String InfoText
        {
            get;
            set;
        }

        public int? TotalPages
        {
            get;
            set;
        }

        public String Location
        {
            get;
            private set;
        }
    }
}
