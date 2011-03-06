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

namespace csharp_comicviewer
{
    /// <summary>
    /// Configuration that is saved to an xml file
    /// </summary>
    [Serializable]
    public class Configuration
    {
        public String Version = "";
        public String[] Resume_Files;
        public int[] Resume_Start_At = new int[2]; //[file number,page number]
        public ArrayList Bookmarks =  new ArrayList();// inside are Arraylist with [0] = String[] files, [1] = int current file, [2] = int current page of file
        public Boolean overideHight = false;
        public Boolean overideWidth = false;
    }
}
