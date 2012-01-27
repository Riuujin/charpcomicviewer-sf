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

namespace CSharpComicViewer.Comic
{
    using System;
    using System.Collections.Generic;
    using CSharpComicViewer.Other;

    /// <summary>
    /// A Collection of ComicFile's (archives).
    /// </summary>
    public class ComicBook
    {
        /// <summary>
        /// All the files in the comicbook.
        /// </summary>
        private List<ComicFile> files = new List<ComicFile>();

        /// <summary>
        /// The total pages in the comicbook.
        /// </summary>
        private int totalPages;

        /// <summary>
        /// Index of the current file in the list.
        /// </summary>
        private int currentFile;

        /// <summary>
        /// Index of the current page within the file.
        /// </summary>
        private int currentPageOfFile;

        /// <summary>
        /// Number of the current page of the total pages within the comicbook.
        /// </summary>
        private int currentPageOfTotal;

        /// <summary>
        /// Initializes a new instance of the ComicBook class.
        /// </summary>
        public ComicBook()
        {
            this.totalPages = 0;
            this.currentFile = 0;
            this.currentPageOfFile = 0;
            this.currentPageOfTotal = 0;
        }

        /// <summary>
        /// Gets or sets a value indicating whether comicfiles are archives
        /// </summary>
        public bool FilesAreArchives { get; set; }

        /// <summary>
        /// Check if the ComicBook has ComicFiles.
        /// </summary>
        /// <returns>Returns <value>true</value> if comicbook has files else returns <value>false</value>.</returns>
        public bool HasFiles()
        {
            if (this.files.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Get the total pages of the ComicBook (Pages of all ComicFiles).
        /// </summary>
        /// <returns>The total pages of the ComicBook (Pages of all ComicFiles).</returns>
        public int GetTotalPages()
        {
            if (this.totalPages == 0)
            { 
                CountTotalPages(); 
            }

            return this.totalPages;
        }

        /// <summary>
        /// Get the current of total pages of the ComicBook.
        /// </summary>
        /// <returns>The current of  total pages of the ComicBoo.k</returns>
        public int GetCurrentPageOfTotal()
        {
            if (GetCurrentFile() != 0)
            {
                this.currentPageOfTotal = this.currentPageOfFile;
                for (int i = 0; i < GetCurrentFile(); i++)
                {
                    this.currentPageOfTotal += this.files[i].TotalPages.Value;
                }
            }
            else
            {
                this.currentPageOfTotal = this.currentPageOfFile;
            }

            return this.currentPageOfTotal;
        }

        /// <summary>
        /// Get the current of total pages of the ComicFile.
        /// </summary>
        /// <param name="filesIndex">The number of the ComicFile.</param>
        /// <returns>The current of  total pages of the ComicFile.</returns>
        public int GetTotalPagesOfFile(int filesIndex)
        {
            return this.files[filesIndex].TotalPages.Value;
        }

        /// <summary>
        /// Get the total number of ComicFiles of the ComicBook.
        /// </summary>
        /// <returns>Total files in the ComicBook.</returns>
        public int GetTotalFiles()
        {
            return files.Count;
        }

        /// <summary>
        /// Get the index of the current ComicFile of the ComicBook.
        /// </summary>
        /// <returns>The index of the current ComicFile in the ComicBook.</returns>
        public int GetCurrentFile()
        {
            return this.currentFile;
        }

        /// <summary>
        /// Gets the filename of the current active file.
        /// </summary>
        /// <returns>Filename of the current file.</returns>
        public string GetCurrentFileName()
        {
            string filePath = this.files[GetCurrentFile()].Location;
            string[] filePathSplit = filePath.Split('\\');
            string fileNameWithExtension = filePathSplit[filePathSplit.Length - 1];
            return fileNameWithExtension;
        }

        /// <summary>
        /// Get a page (image) of the ComicBook.
        /// </summary>
        /// <param name="fileNumber">Index number of the ComicFile.</param>
        /// <param name="pageNumber">Index number of page from the ComicFile.</param>
        /// <returns>The requested image.</returns>
        public byte[] GetPage(int fileNumber, int pageNumber)
        {
            if (GetTotalFiles() > 0)
            {
                if (this.files[fileNumber].GetPage(pageNumber) != null)
                {
                    this.currentFile = fileNumber;
                    this.currentPageOfFile = pageNumber;
                    return this.files[fileNumber].GetPage(pageNumber);
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Get the information text from a ComicFile.
        /// </summary>
        /// <param name="fileNumber">Index number of the ComicFile.</param>
        /// <returns>The text</returns>
        public string GetInfoText(int fileNumber)
        {
            return this.files[currentFile].InfoText;
        }

        /// <summary>
        /// Get the information needed to save the ComicBook as bookmark or resume.
        /// </summary>
        /// <returns>Bookmark.</returns>
        public Bookmark GetBookmark()
        {
            Bookmark data;
            string[] fileLocations = new string[this.files.Count];
            for (int i = 0; i < this.files.Count; i++)
            {
                fileLocations[i] = this.files[i].Location;
            }

            data = new Bookmark(fileLocations, this.currentFile, this.currentPageOfFile);
            return data;
        }

        /// <summary>
        /// Create a ComicFile using an archive.
        /// </summary>
        /// <param name="location">Location of the ComicFile.</param>
        /// <param name="images">Images inside the ComicFile.</param>
        /// <param name="infoText">Information text if any in the ComicFile.</param>
        public void CreateComicFile(string location, List<byte[]> images, string infoText)
        {
            ComicFile file = new ComicFile(location, images, infoText);
            this.files.Add(file);
        }

        /// <summary>
        /// Create a ComicFile using loose images.
        /// </summary>
        /// <param name="location">Locations of the Files.</param>
        /// <param name="images">The Images.</param>
        /// <param name="infoText">Information text if any.</param>
        public void CreateComicFile(List<string> location, List<byte[]> images, string infoText)
        {
            ComicFile file = new ComicFile(location[0], images, infoText);
            this.files.Add(file);
        }

        /// <summary>
        /// Get the next page of the ComicBook.
        /// </summary>
        /// <returns>The next page of the ComicBook.</returns>
        public byte[] NextPage()
        {
            byte[] page = null;
            if (this.currentPageOfFile + 1 < GetTotalPagesOfFile(this.currentFile))
            {
                page = GetPage(this.currentFile, this.currentPageOfFile + 1);
            }
            else if (this.currentFile < this.files.Count - 1)
            {
                page = GetPage(this.currentFile + 1, 0);
            }

            if (page != null)
            {
                this.currentPageOfTotal++;
            }

            return page;
        }

        /// <summary>
        /// Get the previous page of the ComicBook.
        /// </summary>
        /// <returns>The previous page of the ComicBook.</returns>
        public byte[] PreviousPage()
        {
            byte[] page = null;
            if (this.currentPageOfFile - 1 >= 0)
            {
                page = GetPage(this.currentFile, this.currentPageOfFile - 1);
            }
            else if (this.currentFile > 0)
            {
                page = GetPage(this.currentFile - 1, GetTotalPagesOfFile(this.currentFile - 1) - 1);
            }

            if (page != null)
            {
                this.currentPageOfTotal--;
            }

            return page;
        }

        /// <summary>
        /// Get the current page of the ComicBook.
        /// </summary>
        /// <returns>The current page of the ComicBook.</returns>
        public byte[] GetCurrentPage()
        {
            byte[] page = null;
            page = GetPage(this.currentFile, this.currentPageOfFile);
            return page;
        }

        /// <summary>
        /// Get the file location of a ComicFile.
        /// </summary>
        /// <param name="fileNumber">Index of the ComicFile.</param>
        /// <returns>The file location of the ComicFile.</returns>
        public string GetFileLocation(int fileNumber)
        {
            return this.files[fileNumber].Location;
        }

        /// <summary>
        /// Count the total pages of the ComicBook (Pages of all ComicFiles).
        /// </summary>
        private void CountTotalPages()
        {
            totalPages = 0;
            for (int i = 0; i < this.files.Count; i++)
            {
                totalPages += this.files[i].TotalPages.Value;
            }
        }
    }
}
