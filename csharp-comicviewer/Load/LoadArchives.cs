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
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using SevenZip;

namespace csharp_comicviewer
{
    /// <summary>
    /// Load archives using SevenZipSharp
    /// </summary>
    class LoadArchives
    {
        private ArrayList SupportedImageFormats = new ArrayList();
        private CustomStackTrace CustomStackTrace = new CustomStackTrace();
        private InfoText_Form InfoText;

        /// <summary>
        /// Load the dll required
        /// </summary>
        public LoadArchives()
        {
            //Get the location of the 7z dll (location .EXE is in)
            String executableName = Application.ExecutablePath;
            FileInfo executableFileInfo = new FileInfo(executableName);
            String executableDirectoryName = executableFileInfo.DirectoryName;

            //load the  dll
            SevenZipExtractor.SetLibraryPath(executableDirectoryName + "//7z.dll");

            setSupported_image_formats();
        }

        /// <summary>
        /// Set supported images types
        /// </summary>
        private void setSupported_image_formats()
        {
            SupportedImageFormats.Add(".jpg");
            SupportedImageFormats.Add(".JPG");
            SupportedImageFormats.Add(".bmp");
            SupportedImageFormats.Add(".BMP");
            SupportedImageFormats.Add(".png");
            SupportedImageFormats.Add(".PNG");
        }

        /// <summary>
        /// Creates a ComicBook from an array of archive paths
        /// </summary>
        /// <param name="Archives">Array of archive paths</param>
        /// <returns>ComicBook</returns>
        public LoadReturnValue CreateComicBook(String[] Archives)
        {
            LoadReturnValue ReturnValue = new LoadReturnValue();
            
            Array.Sort(Archives);
            ComicBook ComicBook = new ComicBook();
            String InfoTxt = "";
            String CurrentFile;
            List<byte[]> ImagesAsBytes = new List<byte[]>();
            MemoryStream ms = new MemoryStream();
            SevenZipExtractor Extractor;
            Boolean NextFile = false;

            foreach (String Archive in Archives)
            {
                if (!File.Exists(Archive))
                {
                    ReturnValue.Error = "One or more archives where not found";
                    return ReturnValue;
                }
            }

            try
            {
                for (int y = 0; y < Archives.Length; y++)
                {
                    //open archive
                    CurrentFile = Archives[y];
                    Extractor = new SevenZipExtractor(CurrentFile);
                    String[] FileNames = Extractor.ArchiveFileNames.ToArray();
                    Array.Sort(FileNames);

                    //create ComicFiles for every single archive
                    for (int i = 0; i < Extractor.FilesCount; i++)
                    {
                        for (int x = 0; x < SupportedImageFormats.Count; x++)
                        {
                            //if it is an image add it to arraylist
                            if (FileNames[i].EndsWith(SupportedImageFormats[x].ToString()))
                            {
                                ms = new MemoryStream();
                                Extractor.ExtractFile(FileNames[i], ms);
                                ms.Position = 0;
                                try
                                {
                                    ImagesAsBytes.Add(ms.ToArray());
                                }
                                catch (Exception)
                                {
                                    ms.Close();
                                    ReturnValue.Error = "One or more files are corrupted, and where skipped";
                                }
                                ms.Close();
                                NextFile = true;
                            }
                            //if it is a txt file set it as InfoTxt
                            else if (FileNames[i].EndsWith(".txt") || FileNames[i].EndsWith(".TXT"))
                            {
                                ms = new MemoryStream();
                                Extractor.ExtractFile(FileNames[i], ms);
                                ms.Position = 0;
                                try
                                {
                                    StreamReader sr = new StreamReader(ms);
                                    InfoTxt = sr.ReadToEnd();
                                }
                                catch (Exception)
                                {
                                    ms.Close();
                                    ReturnValue.Error = "One or more files are corrupted, and where skipped";
                                }
                                ms.Close();
                                NextFile = true;
                            }
                            if (NextFile)
                            {
                                NextFile = false;
                                x = SupportedImageFormats.Count;
                            }
                        }
                    }
                    //unlock files again
                    Extractor.Dispose();

                    //make a ComicFile, with either an InfoTxt or without
                    if (ImagesAsBytes.Count > 0)
                    {
                        if (InfoTxt.Length > 0)
                        {
                            ComicBook.CreateComicFile(CurrentFile, ImagesAsBytes, InfoTxt);
                            InfoText = new InfoText_Form(Archives[y], InfoTxt);
                            InfoTxt = "";
                        }
                        else
                            ComicBook.CreateComicFile(CurrentFile, ImagesAsBytes, null);
                    }
                    ImagesAsBytes.Clear();
                }

                if (ComicBook.HasFiles())
                {
                    ReturnValue.ComicBook = ComicBook;
                    ReturnValue.HasFile = true;
                }
                else
                    ReturnValue.HasFile = false;

                //return the ComicBook on succes
                return ReturnValue;
            }
            catch (Exception e)
            {
                //show error and return nothing
                CustomStackTrace.CreateStackTrace(e.StackTrace);
                return null;
            }
        }
    }
}
