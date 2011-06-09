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

namespace csharp_comicviewer
{
    class LoadReturnValue
    {
        private ComicBook _ComicBook;
        private String _Error;
        private Boolean _HasFile;


        public ComicBook ComicBook
        {
            get { return _ComicBook; }
            set { _ComicBook = value; }
        }

        public Boolean HasFile
        {
            get { return _HasFile; }
            set { _HasFile = value; }
        }

        public String Error
        {
            get { return _Error; }
            set { _Error = value; }
        }
    }
}
