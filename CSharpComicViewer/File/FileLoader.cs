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
            this.FileType = FileType.Archive;
        }

        public bool Load(string[] files)
        {
            this.FileType = FileType.Archive;
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
                    this.FileType = FileType.Images;
                    break;
                }
                else if (this.FileType == FileType.Images)
                {
                    this.Error = "Please select only archives or only images.";
                }

            }

            if (String.IsNullOrEmpty(this.Error))
            {
                if (FileType == FileType.Archive)
                {
                    ArchiveLoader archiveLoader = new ArchiveLoader();
                    LoadedFileData = archiveLoader.LoadComicBook(files);

                    this.Error = LoadedFileData.Error;
                }
                else if (FileType == FileType.Images)
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
        public FileType FileType
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

    public enum FileType
    {
        Archive = 0,
        Images
    }
}
