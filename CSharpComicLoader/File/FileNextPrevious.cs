﻿//-------------------------------------------------------------------------------------
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
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CSharpComicLoader
{
	/// <summary>
	/// Find and locate next and previous usable files(archives) on disk
	/// </summary>
	public class FileNextPrevious
	{
		/// <summary>
		/// Get next file in known file list
		/// </summary>
		/// <param name="CurrentFiles">Known file paths</param>
		/// <returns>The next file path</returns>
		public string[] GetNextFile(string[] CurrentFiles)
		{
			string[] Files = CurrentFiles;
			Array.Sort(Files);
			string file = Files.Last();

			string nextfile = nextFile(file);
			string dir = Directory.GetParent(file).ToString();
			string lastdir = dir;
			while (string.IsNullOrEmpty(nextfile))
			{
				lastdir = dir;
				dir = nextDir(dir);
				if (string.IsNullOrEmpty(dir))
				{
					dir = nextDir(Directory.GetParent(lastdir).ToString());
				}
				if (string.IsNullOrEmpty(dir))
					break;


				nextfile = firstOrLastFileInDir(dir, true);
			}
			string[] returnarray = new string[1];
			returnarray[0] = nextfile;
			return returnarray;
		}

		/// <summary>
		/// Get previous file in known file list
		/// </summary>
		/// <param name="CurrentFiles">Known file path</param>
		/// <returns>The previous file path</returns>
		public string[] GetPreviousFile(string[] CurrentFiles)
		{
			string[] Files = CurrentFiles;
			Array.Sort(Files);
			string file = Files.Last();

			string nextfile = previousFile(file);
			string dir = Directory.GetParent(file).ToString();
			string lastdir = dir;
			while (string.IsNullOrEmpty(nextfile))
			{
				lastdir = dir;
				dir = previousDir(dir);
				if (string.IsNullOrEmpty(dir))
				{
					dir = previousDir(Directory.GetParent(lastdir).ToString());
				}
				if (string.IsNullOrEmpty(dir))
					break;


				nextfile = firstOrLastFileInDir(dir, false);
			}
			string[] returnarray = new string[1];
			returnarray[0] = nextfile;
			return returnarray;
		}

		/// <summary>
		/// Get the next directory path to search in
		/// </summary>
		/// <param name="currentdir">The current directory path</param>
		/// <returns>The next directory path</returns>
		private string nextDir(string currentdir)
		{
			Boolean next = false;
			try
			{
				string[] Dirs = Directory.GetDirectories(Directory.GetParent(currentdir).ToString());
				Array.Sort(Dirs);
				foreach (string dir in Dirs)
				{
					if (next)
						return dir.ToString();

					if (currentdir == dir)
						next = true;
				}
			}
			catch
			{
				return null;
			}
			return null;
		}

		/// <summary>
		/// Get the next usable file location on disk
		/// </summary>
		/// <param name="currentfile">Current file path</param>
		/// <returns>The next file path</returns>
		private string nextFile(string currentfile)
		{
			string dir = Directory.GetParent(currentfile).FullName;
			string[] cbr = Directory.GetFiles(dir, "*.cbr");
			string[] cbz = Directory.GetFiles(dir, "*.cbz");
			string[] rar = Directory.GetFiles(dir, "*.rar");
			string[] zip = Directory.GetFiles(dir, "*.zip");

			List<string> filePaths = new List<string>();

			foreach (string filename in cbr)
			{
				filePaths.Add(filename);
			}
			foreach (string filename in cbz)
			{
				filePaths.Add(filename);
			}
			foreach (string filename in rar)
			{
				filePaths.Add(filename);
			}
			foreach (string filename in zip)
			{
				filePaths.Add(filename);
			}


			filePaths.Sort();


			Boolean next = false;
			foreach (string filename in filePaths)
			{
				if (next)

					return filename;


				if (currentfile == filename && filename != filePaths.Last())
					next = true;
			}
			return null;
		}

		/// <summary>
		/// Get the previous directory path to search in
		/// </summary>
		/// <param name="currentdir">The current directory path</param>
		/// <returns>The previous directory path</returns>
		private string previousDir(string currentdir)
		{
			try
			{
				string[] Dirs = Directory.GetDirectories(Directory.GetParent(currentdir).ToString());
				Array.Sort(Dirs);
				for (int i = 0; i < Dirs.Length; i++)
				{
					if (currentdir == Dirs.First())
						return null;

					if (currentdir == Dirs[i])
						return Dirs[i - 1];
				}
			}
			catch
			{
				return null;
			}
			return null;
		}

		/// <summary>
		/// Get the previous usable file location on disk
		/// </summary>
		/// <param name="currentfile">Current file path</param>
		/// <returns>The previous file path</returns>
		private string previousFile(string currentfile)
		{
			string dir = Directory.GetParent(currentfile).FullName;
			string[] cbr = Directory.GetFiles(dir, "*.cbr");
			string[] cbz = Directory.GetFiles(dir, "*.cbz");
			string[] rar = Directory.GetFiles(dir, "*.rar");
			string[] zip = Directory.GetFiles(dir, "*.zip");

			List<string> filePaths = new List<string>();

			foreach (string filename in cbr)
			{
				filePaths.Add(filename);
			}
			foreach (string filename in cbz)
			{
				filePaths.Add(filename);
			}
			foreach (string filename in rar)
			{
				filePaths.Add(filename);
			}
			foreach (string filename in zip)
			{
				filePaths.Add(filename);
			}


			filePaths.Sort();


			for (int i = 0; i < filePaths.Count; i++)
			{
				if (currentfile == filePaths.First())
					return null;

				if (currentfile == filePaths[i])
					return filePaths[i - 1];
			}
			return null;
		}

		/// <summary>
		/// Get first or last file in a directory
		/// </summary>
		/// <param name="dir">Directory path</param>
		/// <param name="First">Return first file?</param>
		/// <returns>The file path</returns>
		private string firstOrLastFileInDir(string dir, Boolean First)
		{
			try
			{
				string[] cbr = Directory.GetFiles(dir, "*.cbr");
				string[] cbz = Directory.GetFiles(dir, "*.cbz");
				string[] rar = Directory.GetFiles(dir, "*.rar");
				string[] zip = Directory.GetFiles(dir, "*.zip");

				List<string> filePaths = new List<string>();

				foreach (string filename in cbr)
				{
					filePaths.Add(filename);
				}
				foreach (string filename in cbz)
				{
					filePaths.Add(filename);
				}
				foreach (string filename in rar)
				{
					filePaths.Add(filename);
				}
				foreach (string filename in zip)
				{
					filePaths.Add(filename);
				}

				if (filePaths.Count > 0)
				{
					filePaths.Sort();

					if (First)
						return filePaths.First();
					else
						return filePaths.Last();
				}
				return null;
			}
			catch
			{
				return null;
			}
		}



	}
}