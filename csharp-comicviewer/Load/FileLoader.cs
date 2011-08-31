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

	class FileLoader
	{
		
		public FileLoader()
		{
			ComicBook = null;
			HasFile = false;
			Error = null;
			Archive = true;
		}
		
		public bool Load(String[] Files)
		{
			if(Archive)
			{
				comicBook comicbook;
				bool hasfile;
				string error;
				
				LoadArchives LoadArchives = new LoadArchives();
				bool ReturnValue = LoadArchives.Load(Files,out comicbook,out hasfile,out error);
				
				ComicBook = comicbook;
				HasFile = hasfile;
				Error = error;
				return ReturnValue;				
			}
			else
			{
				Error = "Not yet implemented!";
				return false;
			}			
		}
		
		/// <summary>
		/// Get or set to load an archive
		/// </summary>
		/// <value><c>true</c> if it should use <see cref="loadLoadArchives"/>, else <c>false</c> if it should load loose images</value>
		public bool Archive 
		{ 
			get; 
			set; 
		}
		
		
		/// <summary>
		/// Sets or Gets the ComicBook
		/// </summary>
		public comicBook ComicBook
		{
			get;
			private set;
		}

		
		/// <summary>
		/// Indicates wheter ComicBook has one or more files
		/// </summary>
		/// <value>If value <c>true</c> then ComicBook has one or more files, else value is <c>false</c></value>
		public bool HasFile
		{
			get;
			set;
		}

		/// <summary>
		/// Error message that occured during load action
		/// </summary>
		/// <value>The error message</value>
		public string Error
		{
			get;
			private set;
		}
	}
}
