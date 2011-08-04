namespace csharp_comicviewer.Gui
{
    partial class SecondaryDisplay_Form
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SecondaryDisplay_Form));
            this.RightClick_menu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.Resume_item = new System.Windows.Forms.ToolStripMenuItem();
            this.Load_item = new System.Windows.Forms.ToolStripMenuItem();
            this.NextPage_item = new System.Windows.Forms.ToolStripMenuItem();
            this.PreviousPage_item = new System.Windows.Forms.ToolStripMenuItem();
            this.NextFile_item = new System.Windows.Forms.ToolStripMenuItem();
            this.PreviousFile_item = new System.Windows.Forms.ToolStripMenuItem();
            this.Bookmark_menu = new System.Windows.Forms.ToolStripMenuItem();
            this.AddBookmark_item = new System.Windows.Forms.ToolStripMenuItem();
            this.ManageBookmarks_item = new System.Windows.Forms.ToolStripMenuItem();
            this.Bookmark_Separator = new System.Windows.Forms.ToolStripSeparator();
            this.About_item = new System.Windows.Forms.ToolStripMenuItem();
            this.Exit_item = new System.Windows.Forms.ToolStripMenuItem();
            this.RightClick_menu.SuspendLayout();
            this.SuspendLayout();
            // 
            // RightClick_menu
            // 
            this.RightClick_menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Resume_item,
            this.Load_item,
            this.NextPage_item,
            this.PreviousPage_item,
            this.NextFile_item,
            this.PreviousFile_item,
            this.Bookmark_menu,
            this.About_item,
            this.Exit_item});
            this.RightClick_menu.Name = "contextMenuStrip1";
            this.RightClick_menu.Size = new System.Drawing.Size(225, 202);
            // 
            // Resume_item
            // 
            this.Resume_item.Name = "Resume_item";
            this.Resume_item.Size = new System.Drawing.Size(224, 22);
            this.Resume_item.Text = "Resume last file(s) (R)";
            // 
            // Load_item
            // 
            this.Load_item.Name = "Load_item";
            this.Load_item.Size = new System.Drawing.Size(224, 22);
            this.Load_item.Text = "Load File(s) (L)";
            // 
            // NextPage_item
            // 
            this.NextPage_item.Name = "NextPage_item";
            this.NextPage_item.Size = new System.Drawing.Size(224, 22);
            this.NextPage_item.Text = "Next Page (Page Down)";
            // 
            // PreviousPage_item
            // 
            this.PreviousPage_item.Name = "PreviousPage_item";
            this.PreviousPage_item.Size = new System.Drawing.Size(224, 22);
            this.PreviousPage_item.Text = "Previous Page (Page Up)";
            // 
            // NextFile_item
            // 
            this.NextFile_item.Name = "NextFile_item";
            this.NextFile_item.Size = new System.Drawing.Size(224, 22);
            this.NextFile_item.Text = "Next File (Alt + Page Down)";
            // 
            // PreviousFile_item
            // 
            this.PreviousFile_item.Name = "PreviousFile_item";
            this.PreviousFile_item.Size = new System.Drawing.Size(224, 22);
            this.PreviousFile_item.Text = "Previous File (Alt + Page Up)";
            // 
            // Bookmark_menu
            // 
            this.Bookmark_menu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AddBookmark_item,
            this.ManageBookmarks_item,
            this.Bookmark_Separator});
            this.Bookmark_menu.Name = "Bookmark_menu";
            this.Bookmark_menu.Size = new System.Drawing.Size(224, 22);
            this.Bookmark_menu.Text = "Bookmarks";
            // 
            // AddBookmark_item
            // 
            this.AddBookmark_item.Name = "AddBookmark_item";
            this.AddBookmark_item.Size = new System.Drawing.Size(179, 22);
            this.AddBookmark_item.Text = "Add bookmark";
            // 
            // ManageBookmarks_item
            // 
            this.ManageBookmarks_item.Name = "ManageBookmarks_item";
            this.ManageBookmarks_item.Size = new System.Drawing.Size(179, 22);
            this.ManageBookmarks_item.Text = "Manage bookmarks";
            // 
            // Bookmark_Separator
            // 
            this.Bookmark_Separator.Name = "Bookmark_Separator";
            this.Bookmark_Separator.Size = new System.Drawing.Size(176, 6);
            // 
            // About_item
            // 
            this.About_item.Name = "About_item";
            this.About_item.Size = new System.Drawing.Size(224, 22);
            this.About_item.Text = "About";
            // 
            // Exit_item
            // 
            this.Exit_item.Name = "Exit_item";
            this.Exit_item.Size = new System.Drawing.Size(224, 22);
            this.Exit_item.Text = "Exit (X)";
            // 
            // SecondaryDisplay_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(725, 477);
            this.ContextMenuStrip = this.RightClick_menu;
            this.ControlBox = false;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SecondaryDisplay_Form";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "C# Comicviewer";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.SecondaryDisplay_Form_Load);
            this.RightClick_menu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip RightClick_menu;
        private System.Windows.Forms.ToolStripMenuItem Resume_item;
        private System.Windows.Forms.ToolStripMenuItem Load_item;
        private System.Windows.Forms.ToolStripMenuItem NextPage_item;
        private System.Windows.Forms.ToolStripMenuItem PreviousPage_item;
        private System.Windows.Forms.ToolStripMenuItem NextFile_item;
        private System.Windows.Forms.ToolStripMenuItem PreviousFile_item;
        private System.Windows.Forms.ToolStripMenuItem Bookmark_menu;
        private System.Windows.Forms.ToolStripMenuItem AddBookmark_item;
        private System.Windows.Forms.ToolStripMenuItem ManageBookmarks_item;
        private System.Windows.Forms.ToolStripSeparator Bookmark_Separator;
        private System.Windows.Forms.ToolStripMenuItem About_item;
        private System.Windows.Forms.ToolStripMenuItem Exit_item;
    }
}