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
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;
using System.Diagnostics;

namespace csharp_comicviewer
{
	/// <summary>
	/// Description of About_Form.
	/// </summary>
	public partial class About_Form : Form
	{
		public About_Form()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			Assembly asm = Assembly.GetExecutingAssembly();
			FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(asm.Location);
			version_lbl.Text = String.Format( "Version: {0}", fvi.FileVersion);
			
		}
		
		void Website_lblLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			ProcessStartInfo psi = new ProcessStartInfo("http://csharpcomicview.sf.net/);
			psi.UseShellExecute = true;
			Process.Start(psi);
		}
		
		void SevenZipSharp_lblLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			ProcessStartInfo psi = new ProcessStartInfo("http://sevenzipsharp.codeplex.com/");
			psi.UseShellExecute = true;
			Process.Start(psi);
		}
		
		void Zip_lblLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			ProcessStartInfo psi = new ProcessStartInfo("http://7-zip.org");
			psi.UseShellExecute = true;
			Process.Start(psi);
		}
		
		
		void Close_btnClick(object sender, EventArgs e)
		{
			this.Close();
		}
	}
}
