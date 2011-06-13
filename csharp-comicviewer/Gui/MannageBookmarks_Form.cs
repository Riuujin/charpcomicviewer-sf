using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using csharp_comicviewer.Other;
using System.Runtime.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace csharp_comicviewer.Gui
{
    /// <summary>
    /// Manage the bookmarks
    /// </summary>
    public partial class MannageBookmarks_Form : Form
    {
        private Configuration Configuration = new Configuration();
        private Configuration ConfigurationBackup = new Configuration();
        private List<Bookmark> Bookmarks = new List<Bookmark>();

        /// <summary>
        /// create a manage bookmarks form
        /// </summary>
        /// <param name="Configuration">The configuration from wich bookmarks are loaded</param>
        public MannageBookmarks_Form(Configuration Configuration)
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
            Boomarks_dataGridView.Rows.Clear();
            if (Configuration != null)
            {
                foreach (Bookmark bookmark in Configuration.Bookmarks)
                {
                    int row = Boomarks_dataGridView.Rows.Add();
                    Boomarks_dataGridView.Rows[row].Cells[0].Value = false;
                    Boomarks_dataGridView.Rows[row].Cells[1].Value = bookmark.GetCurrentFileName();
                    Boomarks_dataGridView.Rows[row].Cells[2].Value = bookmark.PageNumber;
                    Boomarks_dataGridView.Rows[row].Cells[3].Value = bookmark.Files[bookmark.FileNumber];
                }
            }
        }

        /// <summary>
        /// Delete checked bookmarks and update display
        /// </summary>
        private void Delete_btn_Click(object sender, EventArgs e)
        {
            Bookmarks.Clear();
            for (int i = 0; i < Configuration.Bookmarks.Count; i++)
            {
                if (Boolean.Parse(Boomarks_dataGridView.Rows[i].Cells[0].Value.ToString()) == false)
                {
                    Bookmarks.Add(Configuration.Bookmarks[i]);
                }
            }
            Configuration.Bookmarks.Clear();
            Configuration.Bookmarks = Bookmarks;
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
