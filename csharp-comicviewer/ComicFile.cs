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
using System.Linq;
using System.Text;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace csharp_comicviewer
{
    class ComicFile
    {
        List<byte[]> ImagesAsBytes = new List<byte[]>();
        String InfoTxt;
        int TotalPages;
        String Location;

        public ComicFile(String Location, List<byte[]> Images, String InfoTxt)
        {
            this.Location = Location;
            this.ImagesAsBytes.AddRange(Images);
            CountTotalPages();
            if (InfoTxt != null)
                this.InfoTxt = InfoTxt;
        }

        private void CountTotalPages()
        {
            TotalPages = ImagesAsBytes.Count;
        }

        public int GetTotalPages()
        {
            return TotalPages;
        }

        public Image GetPage(int PageNumber)
        {
            using (MemoryStream ms = new MemoryStream(ImagesAsBytes[PageNumber], 0, ImagesAsBytes[PageNumber].Length))
            {
                return Image.FromStream(ms,true);
            }
            
        }

        public String GetInfoTxt()
        {
            return InfoTxt;
        }

        public String GetLocation()
        {
            return Location;
        }
    }
}
