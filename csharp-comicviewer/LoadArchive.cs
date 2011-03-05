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
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using SevenZip;
using System.Collections.Generic;

namespace csharp_comicviewer
{
	class LoadArchive
	{
		ArrayList SupportedImageFormats = new ArrayList();
		CustomStackTrace CustomStackTrace = new CustomStackTrace();
		private InfoText InfoText;

		public LoadArchive()
		{
			//Get the location of the 7z dll (location .EXE is in)
			String executableName = Application.ExecutablePath;
			FileInfo executableFileInfo = new FileInfo(executableName);
			String executableDirectoryName = executableFileInfo.DirectoryName;

			//load the  dll
			SevenZipExtractor.SetLibraryPath(executableDirectoryName + "//7z.dll");
			
			setSupported_image_formats();
		}

		/* 
		 * Images types suported
		 */
		private void setSupported_image_formats()
		{
			SupportedImageFormats.Add(".jpg");
			SupportedImageFormats.Add(".JPG");
			SupportedImageFormats.Add(".bmp");
			SupportedImageFormats.Add(".BMP");
			SupportedImageFormats.Add(".png");
			SupportedImageFormats.Add(".PNG");
		}

		/*
		 * Creates a ComicBook from an array of archives.
		 */
		public ComicBook CreateComicBook(String[] Archive)
		{
			Array.Sort(Archive);
			ComicBook ComicBook = new ComicBook();
			String InfoTxt = "";
			String File;
			List<byte[]> ImagesAsBytes = new List<byte[]>();
			MemoryStream ms = new MemoryStream();
			SevenZipExtractor Extractor;
			Boolean NextFile = false;

			try
			{
				for (int y = 0; y < Archive.Length; y++)
				{
					//open archive
					File = Archive[y];
					Extractor = new SevenZipExtractor(File);
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
								ImagesAsBytes.Add(ms.ToArray());
								ms.Close();
								NextFile = true;
							}
							//if it is a txt file set it as InfoTxt
							else if (FileNames[i].EndsWith(".txt") || FileNames[i].EndsWith(".TXT"))
							{
                                ms = new MemoryStream();
                                Extractor.ExtractFile(FileNames[i], ms);
                                ms.Position = 0;
                                StreamReader sr = new StreamReader(ms);
                                InfoTxt = sr.ReadToEnd();
                                ms.Close();
                                InfoText = new InfoText(Archive[y], InfoTxt);
  								NextFile = true;
							}
							if(NextFile)
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
						if (InfoTxt.Length > 0 )
							ComicBook.CreateComicFile(File, ImagesAsBytes, InfoTxt);
						else
							ComicBook.CreateComicFile(File, ImagesAsBytes, null);
					}
					ImagesAsBytes.Clear();
				}
				//return the ComicBook on succes
				return ComicBook;
			}
			catch (Exception e)
			{
				//show error and return nothing
				CustomStackTrace.CreateStackTrace();
				return null;
			}
		}
	}
}
