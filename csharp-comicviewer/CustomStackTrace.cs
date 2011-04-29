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
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace csharp_comicviewer
{
    /// <summary>
    /// Class that allows the creation of custom stacktraces within install directory
    /// </summary>
    class CustomStackTrace
    {
        ///// <summary>
        ///// Create a custom stacktrace and show a pop-up message
        ///// </summary>
        //public void CreateStackTrace()
        //{
        //    String name = "StackTrace_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm") + ".txt";
        //    TextWriter tw = new StreamWriter(name);
        //    // Create a StackTrace that captures filename,
        //    // line number and column information.
        //    StackTrace st = new StackTrace(true);
        //    string stackIndent = "";
        //    for (int i = 0; i < st.FrameCount; i++)
        //    {
        //        // Note that at this level, there are four
        //        // stack frames, one for each method invocation.
        //        StackFrame sf = st.GetFrame(i);
        //        tw.WriteLine();
        //        tw.WriteLine(stackIndent + " Method: {0}",
        //            sf.GetMethod());
        //        tw.WriteLine(stackIndent + " File: {0}",
        //            sf.GetFileName());
        //        tw.WriteLine(stackIndent + " Line Number: {0}",
        //            sf.GetFileLineNumber());
        //        stackIndent += "  ";
        //    }
        //    tw.Dispose();
        //    MessageBox.Show(String.Format("An error has occured. A stack trace has been saved in the installation directory with the name: \"{0}\" \r\nYou can, if you want, upload the file by making a new item in this projects bug tracker (found on the website).", name));
        //}

        /// <summary>
        /// Create a custom stacktrace and show a pop-up message
        /// </summary>
        public void CreateStackTrace(String ExceptionStackTrace)
        {
            String name = "StackTrace_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm") + ".txt";
            TextWriter tw = new StreamWriter(name);
            tw.WriteLine(ExceptionStackTrace);            
            tw.Dispose();
            MessageBox.Show(String.Format("An error has occured. A stack trace has been saved in the installation directory with the name: \"{0}\" \r\nYou can, if you want, upload the file by making a new item in this projects bug tracker (found on the website).", name));
        }
    }
}
