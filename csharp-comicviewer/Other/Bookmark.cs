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
using System;
using System.Xml;
using System.Xml.Serialization;

namespace Csharp_comicviewer.Other
{
    /// <summary>
    /// A Bookmark
    /// </summary>
    [Serializable]
    [XmlRoot("Bookmark")]
    public class Bookmark
    {
        private String[] _Files;
        private int _CurrentFile;
        private int _CurrentPageOfFile;
        
        /// <summary>
        /// The files
        /// </summary>
        [XmlArray("Files")]
        public String[] Files
        {
            get { return _Files; }
            set { _Files = value; }
        }

        /// <summary>
        /// Current file being read
        /// </summary>
        [XmlElement("CurrentFile")]
        public int FileNumber
        {
            get { return _CurrentFile; }
            set { _CurrentFile = value; }
        }

        /// <summary>
        /// Current page of file being read
        /// </summary>
        [XmlElement("CurrentPage")]
        public int PageNumber
        {
            get { return _CurrentPageOfFile; }
            set { _CurrentPageOfFile = value; }
        }

        /// <summary>
        /// Create a bookmark without specified data
        /// </summary>
        public Bookmark()
        { }

        /// <summary>
        /// Create a bookmark with specified data
        /// </summary>
        /// <param name="Files">The files</param>
        /// <param name="CurrentFile">Current file being read</param>
        /// <param name="CurrentPageOfFile">Current page of file being read</param>
        public Bookmark(String[] Files, int CurrentFile, int CurrentPageOfFile)
        {
            this.Files = Files;
            this.FileNumber = CurrentFile;
            this.PageNumber = CurrentPageOfFile;
        }

        /// <summary>
        /// Get the file name of the CurrentFile
        /// </summary>
        /// <returns>Filename of current file</returns>
        public String GetCurrentFileName()
        {
            String FilePath = _Files[_CurrentFile];
            String[] FilePathSplit = FilePath.Split('\\');
            String FileNameWithExtension = FilePathSplit[FilePathSplit.Length - 1];
            FilePathSplit = FileNameWithExtension.Split('.');
            String Filename = "";
            for (int i = 0; i < FilePathSplit.Length - 1; i++)
            {
                Filename += FilePathSplit[i];
            }
            return Filename;
        }

        /// <summary>
        /// Get the directory location of the CurrentFile
        /// </summary>
        /// <returns>directory location of the current file</returns>
        public String GetCurrentDirectoryLocation()
        {
            String FilePath = _Files[_CurrentFile];
            String[] FilePathSplit = FilePath.Split('\\');
            String Directory = "";
            for (int i = 0; i < FilePathSplit.Length - 1; i++)
            {
                Directory += FilePathSplit[i] + "\\";
            }
            return Directory;
        }
    }
}
