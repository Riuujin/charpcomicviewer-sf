/*
  Copyright 2011 Rutger Spruyt
  
  This file is part of csharp comicviewer.

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
using System.Collections;
using System.Drawing;
using System.IO;
using System.Collections.Generic;

namespace csharp_comicviewer
{
    /// <summary>
    /// A Collection of ComicFile's (archives)
    /// </summary>
    class ComicBook
    {
        List<ComicFile> Files = new List<ComicFile>();
        int TotalPages;
        int CurrentFile;
        int CurrentPageOfFile;
        int CurrentPageOfTotal;

        /// <summary>
        /// Create a ComicBook
        /// </summary>
        public ComicBook()
        {
            TotalPages = 0;
            CurrentFile = 0;
            CurrentPageOfFile = 0;
            CurrentPageOfTotal = 0;
        }

        /// <summary>
        /// Check if the ComicBook has ComicFiles
        /// </summary>
        public Boolean HasFiles()
        {
            if (Files.Count > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Count the total pages of the ComicBook (Pages of all ComicFiles)
        /// </summary>
        private void CountTotalPages()
        {
            TotalPages = 0;
            for (int i = 0; i < Files.Count; i++)
            {
                TotalPages += Files[i].GetTotalPages();
            }
        }

        /// <summary>
        /// Get the total pages of the ComicBook (Pages of all ComicFiles)
        /// </summary>
        /// <returns>The total pages of the ComicBook (Pages of all ComicFiles)</returns>
        public int GetTotalPages()
        {
            if (TotalPages == 0)
                CountTotalPages();
            return TotalPages;
        }

        /// <summary>
        /// Get the current of total pages of the ComicBook
        /// </summary>
        /// <returns>The current of  total pages of the ComicBook</returns>
        public int GetCurrentPageOfTotal()
        {
        	if(GetCurrentFile() != 0)
        	{
        		CurrentPageOfTotal = CurrentPageOfFile;
        		for (int i = 0; i < GetCurrentFile(); i++)
        		{
        			CurrentPageOfTotal += Files[i].GetTotalPages();
        		}
        	}
            else 
                CurrentPageOfTotal = CurrentPageOfFile;
            return CurrentPageOfTotal;
        }

        /// <summary>
        /// Get the current of total pages of the ComicFile
        /// </summary>
        /// <param name="FilesIndex">The number of the ComicFile</param>
        /// <returns>The current of  total pages of the ComicFile</returns>
        public int GetTotalPagesOfFile(int FilesIndex)
        {
            return Files[FilesIndex].GetTotalPages();
        }

        /// <summary>
        /// Get the total number of ComicFiles of the ComicBook
        /// </summary>
        /// <returns>Total files in the ComicBook</returns>
        public int GetTotalFiles()
        {
            return Files.Count;
        }

        /// <summary>
        /// Get the index of the current ComicFile of the ComicBook
        /// </summary>
        /// <returns>The index of the current ComicFile in the ComicBook</returns>
        public int GetCurrentFile()
        {
            return CurrentFile;
        }

        /// <summary>
        /// Get a page (image) of the ComicBook
        /// </summary>
        /// <param name="FileNumber">Index number of the ComicFile</param>
        /// <param name="PageNumber">Index number of page from the ComicFile</param>
        /// <returns>The requested image</returns>
        public Image GetPage(int FileNumber, int PageNumber)
        {
            if (GetTotalFiles() > 0)
            {
                if (Files[FileNumber].GetPage(PageNumber) != null)
                {
                    CurrentFile = FileNumber;
                    CurrentPageOfFile = PageNumber;
                    Console.WriteLine("Filenr " + FileNumber + " Page " + PageNumber);
                    return Files[FileNumber].GetPage(PageNumber);
                }
                else
                    return null;
            }
               else
                    return null;
        }

        /// <summary>
        /// Get the information text from a ComicFile
        /// </summary>
        /// <param name="FileNumber">Index number of the ComicFile</param>
        /// <returns>The text</returns>
        public String GetInfoText(int FileNumber)
        {
            return Files[CurrentFile].GetInfoText();
        }

        /// <summary>
        /// Get the information needed to save the ComicBook as bookmark or resume
        /// </summary>
        /// <returns>ArrayList with the information, [0] = ComicFile locations, [1] = current ComicFile, [2] = current page of ComicFile</returns>
        public ArrayList GetComicBookSaveInformation()
        {
            ArrayList Data = new ArrayList();
            String[] FileLocations = new String[Files.Count];
            for (int i = 0; i < Files.Count; i++)
            {
                FileLocations[i] = Files[i].GetLocation();
            }
            Data.Add(FileLocations); //[0]
            Data.Add(CurrentFile); //[1]
            Data.Add(CurrentPageOfFile); //[2]
            return Data;
        }

        /// <summary>
        /// Create a ComicFile
        /// </summary>
        /// <param name="Location">Location of the ComicFile</param>
        /// <param name="Images">Images inside the ComicFile</param>
        /// <param name="InfoText">Information text if any in the ComicFile</param>
        public void CreateComicFile(String Location, List<byte[]> Images, String InfoText)
        {
            ComicFile File = new ComicFile(Location, Images, InfoText);
            Files.Add(File);
        }

        /// <summary>
        /// Get the next page of the ComicBook
        /// </summary>
        /// <returns>The next page of the ComicBook</returns>
        public Image NextPage()
        {
            Image Page = null;
            if (CurrentPageOfFile + 1 < GetTotalPagesOfFile(CurrentFile))
            {
                Page = GetPage(CurrentFile, CurrentPageOfFile + 1);
            }
            else if (CurrentFile < Files.Count - 1)
            {
                Page = GetPage(CurrentFile + 1, 0);

            }
            if(Page != null)
                CurrentPageOfTotal++;
            return Page;
        }

        /// <summary>
        /// Get the previous page of the ComicBook
        /// </summary>
        /// <returns>The previous page of the ComicBook</returns>
        public Image PreviousPage()
        {
            Image Page = null;
            if (CurrentPageOfFile - 1 >= 0)
            {
                Page = GetPage(CurrentFile, CurrentPageOfFile - 1);
            }
            else if (CurrentFile > 0)
            {
                Page = GetPage(CurrentFile - 1, GetTotalPagesOfFile(CurrentFile - 1) -1);

            }
            if (Page != null)
                CurrentPageOfTotal--;
            return Page;
        }

        /// <summary>
        /// Get the current page of the ComicBook
        /// </summary>
        /// <returns>The current page of the ComicBook</returns>
        public Image GetCurrentPage()
        {
            Image Page = null;
            Page = GetPage(CurrentFile, CurrentPageOfFile);
            return Page;
        }

        /// <summary>
        /// Get the file location of a ComicFile
        /// </summary>
        /// <param name="FileNumber">Index of the ComicFile</param>
        /// <returns>The file location of the ComicFile</returns>
        public String GetFileLocation(int FileNumber)
        {
        	return Files[FileNumber].GetLocation();
        }

    }
}
