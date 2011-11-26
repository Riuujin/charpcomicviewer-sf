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
using Csharp_comicviewer.Comic;

namespace Csharp_comicviewer
{
    /// <summary>
    /// Load images
    /// </summary>
    class ImageLoader
    {
        private ArrayList SupportedImageFormats;
        // private InfoText_Form InfoText;

        public ImageLoader(ArrayList supportedImageFormats)
        {
            this.SupportedImageFormats = supportedImageFormats;
        }


        /// <summary>
        /// Creates a ComicBook from an array of image paths
        /// </summary>
        /// <param name="images">Array of image paths</param>
        /// <returns>ComicBook</returns>
        public bool Load(string[] images, out ComicBook comicbook, out bool hasFile, out string error)
        {
            error = null;
            hasFile = false;
            Array.Sort(images);
            comicbook = new ComicBook();
            string InfoTxt = "";
            string CurrentFile;
            List<byte[]> ImagesAsBytes = new List<byte[]>();
            FileStream fs;
            bool NextFile = false;

            foreach (string image in images)
            {
                if (!File.Exists(image))
                {
                    error = "One or more images where not found";
                    return false;
                }
            }

            try
            {
                for (int i = 0; i < images.Length; i++)
                {
                    //open archive
                    CurrentFile = images[i];

                
                        for (int x = 0; x < SupportedImageFormats.Count; x++)
                        {
                            //if it is an image add it to arraylist
                            if (images[i].ToLower().EndsWith(SupportedImageFormats[x].ToString()))
                            {
                                fs = File.OpenRead(images[i]);
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
                                    error = "One or more files are corrupted, and where skipped";
                                }
                                fs.Close();
                                NextFile = true;
                            }
                           
                            if (NextFile)
                            {
                                NextFile = false;
                                x = SupportedImageFormats.Count;
                            }
                        }
                    
                    //make a ComicFile, with either an InfoTxt or without
                    if (ImagesAsBytes.Count > 0)
                    {
                        comicbook.CreateComicFile(CurrentFile, ImagesAsBytes, null);
                    }
                    ImagesAsBytes.Clear();
                }

                if (comicbook.HasFiles())
                {
                    hasFile = true;
                }
                else
                    hasFile = false;

                //return the ComicBook on succes
                return true;
            }
            catch (Exception e)
            {
                //show error and return nothing
                return false;
            }
        }
    }
}
