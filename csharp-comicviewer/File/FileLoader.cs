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
namespace Csharp_comicviewer
{
    using Csharp_comicviewer.Comic;
    using System.Collections;

    class FileLoader
    {

        public ArrayList SupportedImageExtensions { get; set; }

        public FileLoader()
        {
            this.ComicBook = null;
            this.HasFile = false;
            this.Error = null;
            this.FileType = FileType.Archive;
            SetSupportedImageExtensions();
        }

        public bool Load(string[] files)
        {
            this.FileType = FileType.Archive;
            bool ReturnValue = false;

            foreach (string file in files)
            {
                for (int i = 0; i < SupportedImageExtensions.Count; i++)
                {
                    if (file.ToLower().EndsWith(SupportedImageExtensions[i].ToString()))
                    {
                        this.FileType = FileType.Images;
                        break;
                    }
                    else
                    {
                        if (i == SupportedImageExtensions.Count - 1 && this.FileType == FileType.Images)
                        {
                            this.HasFile = false;
                            this.Error = "Please select only archives or only images.";
                            return ReturnValue;
                        }
                    }
                }
            }

            if (FileType == FileType.Archive)
            {
                ComicBook comicbook;
                bool hasfile;
                string error;

                ArchiveLoader archiveLoader = new ArchiveLoader(SupportedImageExtensions);
                ReturnValue = archiveLoader.Load(files, out comicbook, out hasfile, out error);

                this.ComicBook = comicbook;
                this.ComicBook.FilesAreArchives = true;
                this.HasFile = hasfile;
                this.Error = error;
            }
            else if (FileType == FileType.Images)
            {
                ComicBook comicbook;
                bool hasfile;
                string error;

                ImageLoader imageLoader = new ImageLoader(SupportedImageExtensions);
                ReturnValue = imageLoader.Load(files, out comicbook, out hasfile, out error);

                this.ComicBook = comicbook;
                this.ComicBook.FilesAreArchives = false;
                this.HasFile = hasfile;
                this.Error = error;
            }

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


        /// <summary>
        /// Sets or Gets the ComicBook
        /// </summary>
        public ComicBook ComicBook
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

        /// <summary>
        /// Set supported images types
        /// </summary>
        private void SetSupportedImageExtensions()
        {
            SupportedImageExtensions = new ArrayList();
            SupportedImageExtensions.Add(".jpg");
            SupportedImageExtensions.Add(".bmp");
            SupportedImageExtensions.Add(".png");
        }

    }

    public enum FileType
    {
        Archive = 0,
        Images
    }
}
