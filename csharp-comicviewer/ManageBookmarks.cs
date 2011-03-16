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
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace csharp_comicviewer
{
    /// <summary>
    /// Bookmark manager
    /// </summary>
    public partial class ManageBookmarks : Form
    {

        private Configuration Configuration = new Configuration();
        private Configuration ConfigurationBackup = new Configuration();

        /// <summary>
        /// Load configuration
        /// </summary>
        /// <param name="Configuration"></param>
        public ManageBookmarks(Configuration Configuration)
        {
            InitializeComponent();
            this.Configuration = Configuration;
            BackupConfiguration();
        }

        /// <summary>
        /// Back up configuration in case of cancel
        /// </summary>
        private void BackupConfiguration()
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new MemoryStream();
            using (stream)
            {
                formatter.Serialize(stream, Configuration);
                stream.Seek(0, SeekOrigin.Begin);
                ConfigurationBackup = (Configuration)formatter.Deserialize(stream);
            }
        }

        /// <summary>
        /// Return the configuration to the backup made
        /// </summary>
        private void ReturnToBackupConfiguration()
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new MemoryStream();
            using (stream)
            {
                formatter.Serialize(stream, ConfigurationBackup);
                stream.Seek(0, SeekOrigin.Begin);
                Configuration = (Configuration)formatter.Deserialize(stream);
            }
        }

        /// <summary>
        /// Load existing bookmarks into display
        /// </summary>
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

        /// <summary>
        /// Delete checked bookmarks and update display
        /// </summary>
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
            ManageBookmarks_Load(sender, e);
        }

        /// <summary>
        /// Cancel all actions and close dialog
        /// </summary>
        private void Cancel_btn_Click(object sender, EventArgs e)
        {
            ReturnToBackupConfiguration();
            this.Close();
        }

        /// <summary>
        /// Close this dialog
        /// </summary>
        private void Ok_btn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Get the new configuration
        /// </summary>
        /// <returns>A configuration with updated bookmarks</returns>
        public Configuration GetConfiguration()
        {
            return Configuration;
        }

    }
}
