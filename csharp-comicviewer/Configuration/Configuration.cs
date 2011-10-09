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
using System.Collections;
using Csharp_comicviewer.Other;
using System.Collections.Generic;

namespace Csharp_comicviewer.Configuration
{
    /// <summary>
    /// Configuration that is saved to an xml file
    /// </summary>
    [Serializable]
    public class Configuration
    {
        /// <summary>
        /// Version of C# Comicviewer (currently unused)
        /// </summary>
        public string Version = "";
        /// <summary>
        /// Resume data
        /// </summary>
        public Bookmark Resume;
        /// <summary>
        /// The stored bookmarks
        /// </summary>
        public List<Bookmark> Bookmarks;
        /// <summary>
        /// Should height be overridden
        /// </summary>
        public Boolean OverideHeight = false;
        /// <summary>
        /// Should width be overridden
        /// </summary>
        public Boolean OverideWidth = false;
        /// <summary>
        /// Windowed mode
        /// </summary>
        public Boolean windowed = false;
    }
}
