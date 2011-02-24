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

        public Boolean HasFiles()
        {
            if (Files.Count > 0)
                return true;
            else
                return false;
        }

        private void CountTotalPages()
        {
            TotalPages = 0;
            for (int i = 0; i < Files.Count; i++)
            {
                TotalPages += Files[i].GetTotalPages();
            }
        }

        public int GetTotalPages()
        {
            if (TotalPages == 0)
                CountTotalPages();
            return TotalPages;
        }

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

        public int GetTotalPagesOfFile(int FilesIndex)
        {
            return Files[FilesIndex].GetTotalPages();
        }

        public int GetTotalFiles()
        {
            return Files.Count;
        }

        public int GetCurrentFile()
        {
            return CurrentFile;
        }

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

        public String GetInfoTxt(int FileNumber)
        {
            return Files[CurrentFile].GetInfoTxt();
        }

        public ArrayList GetData()
        {
            ArrayList Data = new ArrayList();
            String[] FileNames = new String[Files.Count];
            for (int i = 0; i < Files.Count; i++)
            {
                FileNames[i] = Files[i].GetLocation();
            }
            Data.Add(FileNames); //[0]
            Data.Add(CurrentFile); //[1]
            Data.Add(CurrentPageOfFile); //[2]
            return Data;
        }

        public void CreateFile(String Location, List<byte[]> Images, String InfoTxt)
        {
            ComicFile File = new ComicFile(Location, Images, InfoTxt);
            Files.Add(File);
        }

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

        public Image GetCurrentPage()
        {
            Image Page = null;
            Page = GetPage(CurrentFile, CurrentPageOfFile);
            return Page;
        }
        
        public String GetFileLocation(int FileNumber)
        {
        	return Files[FileNumber].GetLocation();
        }

    }
}
