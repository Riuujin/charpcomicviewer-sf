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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace csharp_comicviewer
{
    public partial class ManageBookmarks : Form
    {

        private Configuration Configuration = new Configuration();


        public ManageBookmarks(Configuration Configuration)
        {
            InitializeComponent();
            this.Configuration = Configuration;
        }

        private void ManageBookmarks_Load(object sender, EventArgs e)
        {
            CheckBox Checkbox = new CheckBox();
            if (Configuration != null)
            {
                if (Configuration.Bookmarks.Count > 0)
                {
                    ArrayList Data;
                    Bookmarks_chckdLstBx.Items.Clear();
                    for (int i = 0; i < Configuration.Bookmarks.Count; i++)
                    {
                        Data = (ArrayList)Configuration.Bookmarks[i];
                        String[] Files = (String[])Data[0];
                        Bookmarks_chckdLstBx.Items.Add(Files[(int)Data[1]]);
                    }
                }
            }
        }

        private void Delete_btn_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < Configuration.Bookmarks.Count; i++)
            {

                if (Bookmarks_chckdLstBx.GetItemChecked(i))
                {
                    Configuration.Bookmarks.RemoveAt(i);
                    Bookmarks_chckdLstBx.Items.RemoveAt(i);
                    i = 0;
                }
            }
            this.Close();
        }

        private void Cancel_btn_Click(object sender, EventArgs e)
        {
            this.Close();
        }


    }
}
