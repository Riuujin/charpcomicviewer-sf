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

namespace csharp_comicviewer
{
	public partial class InfoText : Form
	{
		public InfoText(String FileLocation,String InfoText)
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
		
		void Close_btnClick(object sender, EventArgs e)
		{
			this.Close();
		}
	}
}
