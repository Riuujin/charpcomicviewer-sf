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
	/// <summary>
	/// The return information of a load action <see cref="LoadArchives"/>
	/// </summary>
    class loadReturnValue
    {  
    	/// <summary>
    	/// Sets or Gets the ComicBook
    	/// </summary>
        public comicBook comicBook
        {
            get;
            set;
        }

        
        /// <summary>
        /// Indicates wheter ComicBook has one or more files
        /// </summary>
        /// <value>If value <c>true</c> then ComicBook has one or more files, else value is <c>false</c></value>
        public bool hasFile
        {
            get;
            set;
        }

        /// <summary>
        /// Error message that occured during load action
        /// </summary>
        /// <value>The error message</value>
        public string error
        {
            get;
            set;
        }
    }
}
