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
namespace CSharpComicViewer
{
	using System.Collections;
	using CSharpComicLoader.File;
	using CSharpComicLoader.Comic;
	using System;

	class FileLoader
	{
		public FileLoader()
		{
			this.PageType = PageType.Archive;
		}

		public bool Load(string[] files)
		{
			this.PageType = PageType.Archive;
			bool ReturnValue;

			foreach (string file in files)
			{
				//if it is an image add it to arraylist
				int startExtension = file.ToLower().LastIndexOf('.');
				if (startExtension < 0)
				{
					//File does not have an extension so skip it
					break;
				}

				string extension = file.ToLower().Substring(startExtension + 1);
				SupportedImages empty;
				if (Enum.TryParse<SupportedImages>(extension, true, out empty))
				{
					this.PageType = PageType.Image;
					break;
				}
				else if (this.PageType == PageType.Image)
				{
					this.Error = "Please select only archives or only images.";
				}

			}

			if (String.IsNullOrEmpty(this.Error))
			{
				if (PageType == PageType.Archive)
				{
					ArchiveLoader archiveLoader = new ArchiveLoader();
					LoadedFileData = archiveLoader.LoadComicBook(files);

					this.Error = LoadedFileData.Error;
				}
				else if (PageType == PageType.Image)
				{
					ImageLoader imageLoader = new ImageLoader();
					LoadedFileData = imageLoader.LoadComicBook(files);

					this.Error = LoadedFileData.Error;
				}
			}

			ReturnValue = String.IsNullOrEmpty(this.Error) ? true : false;
			return ReturnValue;
		}

		/// <summary>
		/// Get or set the type of file to load.
		/// </summary>
		/// <value>The type of file.</value>
		public PageType PageType
		{
			get;
			set;
		}

		public LoadedFilesData LoadedFileData { get; private set; }

		/// <summary>
		/// Error message that occured during load action
		/// </summary>
		/// <value>The error message</value>
		public string Error { get; private set; }

	}

	public enum PageType
	{
		Archive = 0,
		Image
	}
}
