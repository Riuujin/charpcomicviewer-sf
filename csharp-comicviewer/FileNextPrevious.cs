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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace csharp_comicviewer
{
    class FileNextPrevious
    {
        CustomStackTrace CustomStackTrace = new CustomStackTrace();

        public String[] GetNextFile(String[] CurrentFiles)
        {
            string[] Files = CurrentFiles;
            Array.Sort(Files);
            String file = Files.Last();

            String nextfile = nextFile(file);
            String dir = Directory.GetParent(file).ToString();
            String lastdir = dir;
            while (String.IsNullOrEmpty(nextfile))
            {
                lastdir = dir;
                dir = nextDir(dir);
                if (String.IsNullOrEmpty(dir))
                {
                    dir = nextDir(Directory.GetParent(lastdir).ToString());
                }
                if (String.IsNullOrEmpty(dir))
                    break;


                nextfile = firstOrLastFileInDir(dir, true);
            }
            String[] returnarray = new String[1];
            returnarray[0] = nextfile;
            return returnarray;
        }

        public String[] GetPreviousFile(String[] CurrentFiles)
        {
            string[] Files = CurrentFiles;
            Array.Sort(Files);
            String file = Files.Last();

            String nextfile = previousFile(file);
            String dir = Directory.GetParent(file).ToString();
            String lastdir = dir;
            while (String.IsNullOrEmpty(nextfile))
            {
                lastdir = dir;
                dir = previousDir(dir);
                if (String.IsNullOrEmpty(dir))
                {
                    dir = previousDir(Directory.GetParent(lastdir).ToString());
                }
                if (String.IsNullOrEmpty(dir))
                    break;


                nextfile = firstOrLastFileInDir(dir, false);
            }
            String[] returnarray = new String[1];
            returnarray[0] = nextfile;
            return returnarray;
        }

        private String nextDir(String currentdir)
        {
            Boolean next = false;
            try
            {
                String[] Dirs = Directory.GetDirectories(Directory.GetParent(currentdir).ToString());
                Array.Sort(Dirs);
                foreach (String dir in Dirs)
                {
                    if (next)
                        return dir.ToString();

                    if (currentdir == dir)
                        next = true;
                }
            }
            catch(Exception e)
            {
                CustomStackTrace.CreateStackTrace(e.StackTrace);
                return null;
            }
            return null;
        }

        private String nextFile(String currentfile)
        {
            String dir = Directory.GetParent(currentfile).FullName;
            string[] cbr = Directory.GetFiles(dir, "*.cbr");
            string[] cbz = Directory.GetFiles(dir, "*.cbz");
            string[] rar = Directory.GetFiles(dir, "*.rar");
            string[] zip = Directory.GetFiles(dir, "*.zip");

            List<String> filePaths = new List<string>();

            foreach (String filename in cbr)
            {
                filePaths.Add(filename);
            }
            foreach (String filename in cbz)
            {
                filePaths.Add(filename);
            }
            foreach (String filename in rar)
            {
                filePaths.Add(filename);
            }
            foreach (String filename in zip)
            {
                filePaths.Add(filename);
            }


            filePaths.Sort();


            Boolean next = false;
            foreach (String filename in filePaths)
            {
                if (next)

                    return filename;


                if (currentfile == filename && filename != filePaths.Last())
                    next = true;
            }
            return null;
        }

        private String previousDir(String currentdir)
        {
            Boolean next = false;
            try
            {
                String[] Dirs = Directory.GetDirectories(Directory.GetParent(currentdir).ToString());
                Array.Sort(Dirs);
                for(int i = 0; i < Dirs.Length;i++)
                {
                    if (currentdir == Dirs.First())
                        return null;

                    if (currentdir == Dirs[i])
                        return Dirs[i - 1];
                }
            }
            catch (Exception e)
            {
                CustomStackTrace.CreateStackTrace(e.StackTrace);
                return null;
            }
            return null;
        }

        private String previousFile(String currentfile)
        {
            String dir = Directory.GetParent(currentfile).FullName;
            string[] cbr = Directory.GetFiles(dir, "*.cbr");
            string[] cbz = Directory.GetFiles(dir, "*.cbz");
            string[] rar = Directory.GetFiles(dir, "*.rar");
            string[] zip = Directory.GetFiles(dir, "*.zip");

            List<String> filePaths = new List<string>();

            foreach (String filename in cbr)
            {
                filePaths.Add(filename);
            }
            foreach (String filename in cbz)
            {
                filePaths.Add(filename);
            }
            foreach (String filename in rar)
            {
                filePaths.Add(filename);
            }
            foreach (String filename in zip)
            {
                filePaths.Add(filename);
            }


            filePaths.Sort();


            for(int i= 0; i< filePaths.Count;i++)
            {
                if (currentfile == filePaths.First())
                    return null;

                if (currentfile == filePaths[i])
                    return filePaths[i - 1];
            }
            return null;
        }

        private String firstOrLastFileInDir(String dir,Boolean First)
        {
            try
            {
                string[] cbr = Directory.GetFiles(dir, "*.cbr");
                string[] cbz = Directory.GetFiles(dir, "*.cbz");
                string[] rar = Directory.GetFiles(dir, "*.rar");
                string[] zip = Directory.GetFiles(dir, "*.zip");

                List<String> filePaths = new List<string>();

                foreach (String filename in cbr)
                {
                    filePaths.Add(filename);
                }
                foreach (String filename in cbz)
                {
                    filePaths.Add(filename);
                }
                foreach (String filename in rar)
                {
                    filePaths.Add(filename);
                }
                foreach (String filename in zip)
                {
                    filePaths.Add(filename);
                }

                if (filePaths.Count > 0)
                {
                    filePaths.Sort();

                    if(First)
                        return filePaths.First();
                    else
                        return filePaths.Last();
                }
                return null;
            }
            catch (Exception e)
            {
                CustomStackTrace.CreateStackTrace(e.StackTrace);
                return null;
            }
        }

        

    }
}
