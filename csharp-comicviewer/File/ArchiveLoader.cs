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
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Csharp_comicviewer.Comic;
    using SevenZip;

    /// <summary>
    /// Load archives using SevenZipSharp
    /// </summary>
    public class ArchiveLoader
    {
        /// <summary>
        /// The supported image extensions
        /// </summary>
        private ArrayList supportedImageExtensions;

        // private InfoText_Form InfoText;

        /// <summary>
        /// Initializes a new instance of the ArchiveLoader class.
        /// </summary>
        /// <param name="supportedImageExtensions">The supported image extensions</param>
        public ArchiveLoader(ArrayList supportedImageExtensions)
        {
            this.supportedImageExtensions = supportedImageExtensions;

            //Get the location of the 7z dll (location .EXE is in)
            String executableName = System.Reflection.Assembly.GetExecutingAssembly().Location;
            FileInfo executableFileInfo = new FileInfo(executableName);
            String executableDirectoryName = executableFileInfo.DirectoryName;

            //load the  dll
            SevenZipExtractor.SetLibraryPath(executableDirectoryName + "//7z.dll");
        }

        /// <summary>
        /// Creates a ComicBook from an array of archive paths.
        /// </summary>
        /// <param name="archives">Array of archive paths</param>
        /// <param name="comicbook">The ComicBook.</param>
        /// <param name="hasFile"><value>True</value> if ComicBook has files otherwise <value>false</value>.</param>
        /// <param name="error">An error message if any</param>
        /// <returns><value>True</value> on succes otherwise <value>false</value>.</returns>
        public bool Load(String[] archives, out ComicBook comicbook, out bool hasFile, out string error)
        {
            error = null;
            hasFile = false;
            Array.Sort(archives);
            comicbook = new ComicBook();
            String infoTxt = "";
            String currentFile;
            List<byte[]> imagesAsBytes = new List<byte[]>();
            MemoryStream ms = new MemoryStream();
            SevenZipExtractor extractor;
            Boolean nextFile = false;

            foreach (String archive in archives)
            {
                if (!File.Exists(archive))
                {
                    error = "One or more archives where not found";
                    return false;
                }
            }

            try
            {
                for (int y = 0; y < archives.Length; y++)
                {
                    //open archive
                    currentFile = archives[y];
                    extractor = new SevenZipExtractor(currentFile);
                    string[] fileNames = extractor.ArchiveFileNames.ToArray();
                    Array.Sort(fileNames);

                    //create ComicFiles for every single archive
                    for (int i = 0; i < extractor.FilesCount; i++)
                    {
                        for (int x = 0; x < supportedImageExtensions.Count; x++)
                        {
                            //if it is an image add it to arraylist
                            if (fileNames[i].ToLower().EndsWith(supportedImageExtensions[x].ToString()))
                            {
                                ms = new MemoryStream();
                                extractor.ExtractFile(fileNames[i], ms);
                                ms.Position = 0;
                                try
                                {
                                    imagesAsBytes.Add(ms.ToArray());
                                }
                                catch (Exception)
                                {
                                    ms.Close();
                                    error = "One or more files are corrupted, and where skipped";
                                }

                                ms.Close();
                                nextFile = true;
                            }

                            //if it is a txt file set it as InfoTxt
                            else if (fileNames[i].EndsWith(".txt") || fileNames[i].EndsWith(".TXT"))
                            {
                                ms = new MemoryStream();
                                extractor.ExtractFile(fileNames[i], ms);
                                ms.Position = 0;
                                try
                                {
                                    StreamReader sr = new StreamReader(ms);
                                    infoTxt = sr.ReadToEnd();
                                }
                                catch (Exception)
                                {
                                    ms.Close();
                                    error = "One or more files are corrupted, and where skipped";
                                }

                                ms.Close();
                                nextFile = true;
                            }

                            if (nextFile)
                            {
                                nextFile = false;
                                x = supportedImageExtensions.Count;
                            }
                        }
                    }

                    //unlock files again
                    extractor.Dispose();

                    //make a ComicFile, with either an InfoTxt or without
                    if (imagesAsBytes.Count > 0)
                    {
                        if (infoTxt.Length > 0)
                        {
                            comicbook.CreateComicFile(currentFile, imagesAsBytes, infoTxt);

                            //InfoText = new InfoText_Form(Archives[y], InfoTxt);
                            infoTxt = "";
                        }
                        else
                        {
                            comicbook.CreateComicFile(currentFile, imagesAsBytes, null);
                        }
                    }

                    imagesAsBytes.Clear();
                }

                if (comicbook.HasFiles())
                {
                    hasFile = true;
                }
                else
                {
                    hasFile = false;
                }

                //return the ComicBook on succes
                return true;
            }
            catch
            {
                //show error and return nothing
                return false;
            }
        }
    }
}
