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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CSharpComicLoader.Comic;

namespace CSharpComicLoader.File
{
    /// <summary>
    /// Load images
    /// </summary>
    public class ImageLoader : IFileLoader
    {
        public ImageLoader()
        {
        }

        public LoadedFilesData LoadComicBook(string[] files)
        {
            LoadedFilesData returnValue = new LoadedFilesData();
            returnValue.ComicBook = new ComicBook();
            returnValue.ComicBook.FilesAreArchives = false;

            Array.Sort(files);
            string CurrentFile;
            List<byte[]> ImagesAsBytes = new List<byte[]>();
            FileStream fs;
            bool NextFile = false;

            foreach (string image in files)
            {
                if (!System.IO.File.Exists(image))
                {
                    returnValue.Error = "One or more images where not found";
                    return returnValue;
                }
            }

            try
            {
                for (int i = 0; i < files.Length; i++)
                {
                    //open archive
                    CurrentFile = files[i];


                    for (int x = 0; x < Enum.GetNames(typeof(SupportedImages)).Length; x++)
                    {

                        //if it is an image add it to arraylist
                        int startExtension = files[i].ToLower().LastIndexOf('.');
                        if (startExtension < 0)
                        {
                            //File does not have an extension so skip it
                            break;
                        }

                        string extension = files[i].ToLower().Substring(startExtension + 1);
                        SupportedImages empty;
                        if (Enum.TryParse<SupportedImages>(extension, true, out empty))
                        {

                            fs = System.IO.File.OpenRead(files[i]);
                            fs.Position = 0;
                            try
                            {
                                byte[] b = new byte[fs.Length];
                                fs.Read(b, 0, b.Length);
                                ImagesAsBytes.Add(b);
                            }
                            catch (Exception)
                            {
                                fs.Close();
                                returnValue.Error = "One or more files are corrupted, and where skipped";
                                return returnValue;
                            }
                            fs.Close();
                            NextFile = true;
                        }

                        if (NextFile)
                        {
                            NextFile = false;
                            x = Enum.GetNames(typeof(SupportedImages)).Length;
                        }
                    }

                    //make a ComicFile, with either an InfoTxt or without
                    if (ImagesAsBytes.Count > 0)
                    {
                        returnValue.ComicBook.CreateComicFile(CurrentFile, ImagesAsBytes, null);
                    }
                    ImagesAsBytes.Clear();
                }

                //return the ComicBook on succes
                return returnValue;
            }
            catch (Exception e)
            {
                //show error and return nothing
                returnValue.Error = e.Message;
                return returnValue;
            }
        }
    }
}
