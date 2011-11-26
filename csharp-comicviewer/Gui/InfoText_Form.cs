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
using System.Drawing;
using System.Windows.Forms;

namespace csharp_comicviewer
{
    /// <summary>
    /// Show information text in a dialog
    /// </summary>
	public partial class InfoText_Form : Form
	{
        /// <summary>
        /// Show the information text
        /// </summary>
        /// <param name="FileLocation">Location of the archive containing the text</param>
        /// <param name="InfoText">The information text</param>
		public InfoText_Form(String FileLocation,String InfoText)
		{
			InitializeComponent();
			this.Text= "Info text from: \"" + FileLocation + "\"";
			if(!String.IsNullOrEmpty(InfoText))
			{
				InfoText_RchTxtBx.Text = InfoText;
				this.ShowDialog();
			}
			else
				this.Close();
		}
		
        /// <summary>
        /// Close this dialog
        /// </summary>
		void Close_btnClick(object sender, EventArgs e)
		{
			this.Close();
            this.Dispose();
		}
	}
}
