namespace csharp_comicviewer
{
    partial class Display_Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Display_Form));
            this.Message_lbl = new System.Windows.Forms.Label();
            this.Page_lbl = new System.Windows.Forms.Label();
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
            this.MenuBar = new System.Windows.Forms.MenuStrip();
            this.File_bar = new System.Windows.Forms.ToolStripMenuItem();
            this.Resume_item_bar = new System.Windows.Forms.ToolStripMenuItem();
            this.Load_item_bar = new System.Windows.Forms.ToolStripMenuItem();
            this.NextFile_item_bar = new System.Windows.Forms.ToolStripMenuItem();
            this.PreviousFile_item_bar = new System.Windows.Forms.ToolStripMenuItem();
            this.Exit_item_bar = new System.Windows.Forms.ToolStripMenuItem();
            this.PageControl_bar = new System.Windows.Forms.ToolStripMenuItem();
            this.NextPage_item_bar = new System.Windows.Forms.ToolStripMenuItem();
            this.PreviousPage_item_bar = new System.Windows.Forms.ToolStripMenuItem();
            this.ShowPageInformation_item_bar = new System.Windows.Forms.ToolStripMenuItem();
            this.Bookmark_menu_bar = new System.Windows.Forms.ToolStripMenuItem();
            this.AddBookmark_item_bar = new System.Windows.Forms.ToolStripMenuItem();
            this.ManageBookmarks_item_bar = new System.Windows.Forms.ToolStripMenuItem();
            this.Bookmark_Separator_bar = new System.Windows.Forms.ToolStripSeparator();
            this.About_item_bar = new System.Windows.Forms.ToolStripMenuItem();
            this.InformationText_item_bar = new System.Windows.Forms.ToolStripMenuItem();
            this.DisplayedImage = new System.Windows.Forms.PictureBox();
            this.RightClick_menu.SuspendLayout();
            this.MenuBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DisplayedImage)).BeginInit();
            this.SuspendLayout();
            // 
            // Message_lbl
            // 
            this.Message_lbl.BackColor = System.Drawing.Color.Black;
            this.Message_lbl.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.Message_lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Message_lbl.ForeColor = System.Drawing.Color.White;
            this.Message_lbl.Location = new System.Drawing.Point(-1, 277);
            this.Message_lbl.Name = "Message_lbl";
            this.Message_lbl.Size = new System.Drawing.Size(866, 35);
            this.Message_lbl.TabIndex = 4;
            this.Message_lbl.Text = "Message";
            this.Message_lbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.Message_lbl.Visible = false;
            // 
            // Page_lbl
            // 
            this.Page_lbl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Page_lbl.AutoSize = true;
            this.Page_lbl.BackColor = System.Drawing.SystemColors.Control;
            this.Page_lbl.Location = new System.Drawing.Point(821, 542);
            this.Page_lbl.Name = "Page_lbl";
            this.Page_lbl.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Page_lbl.Size = new System.Drawing.Size(32, 13);
            this.Page_lbl.TabIndex = 5;
            this.Page_lbl.Text = "Page";
            this.Page_lbl.Visible = false;
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
            this.Resume_item.Click += new System.EventHandler(this.ResumeLastFiles_Click);
            // 
            // Load_item
            // 
            this.Load_item.Name = "Load_item";
            this.Load_item.Size = new System.Drawing.Size(224, 22);
            this.Load_item.Text = "Load File(s) (L)";
            this.Load_item.Click += new System.EventHandler(this.LoadArchives_Click);
            // 
            // NextPage_item
            // 
            this.NextPage_item.Name = "NextPage_item";
            this.NextPage_item.Size = new System.Drawing.Size(224, 22);
            this.NextPage_item.Text = "Next Page (Page Down)";
            this.NextPage_item.Click += new System.EventHandler(this.NextPage);
            // 
            // PreviousPage_item
            // 
            this.PreviousPage_item.Name = "PreviousPage_item";
            this.PreviousPage_item.Size = new System.Drawing.Size(224, 22);
            this.PreviousPage_item.Text = "Previous Page (Page Up)";
            this.PreviousPage_item.Click += new System.EventHandler(this.PreviousPage);
            // 
            // NextFile_item
            // 
            this.NextFile_item.Name = "NextFile_item";
            this.NextFile_item.Size = new System.Drawing.Size(224, 22);
            this.NextFile_item.Text = "Next File (Alt + Page Down)";
            this.NextFile_item.Click += new System.EventHandler(this.NextFile_Click);
            // 
            // PreviousFile_item
            // 
            this.PreviousFile_item.Name = "PreviousFile_item";
            this.PreviousFile_item.Size = new System.Drawing.Size(224, 22);
            this.PreviousFile_item.Text = "Previous File (Alt + Page Up)";
            this.PreviousFile_item.Click += new System.EventHandler(this.PreviousFile_Click);
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
            this.AddBookmark_item.Click += new System.EventHandler(this.AddBookmark_item_Click);
            // 
            // ManageBookmarks_item
            // 
            this.ManageBookmarks_item.Name = "ManageBookmarks_item";
            this.ManageBookmarks_item.Size = new System.Drawing.Size(179, 22);
            this.ManageBookmarks_item.Text = "Manage bookmarks";
            this.ManageBookmarks_item.Click += new System.EventHandler(this.ManageBookmarks_item_Click);
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
            this.About_item.Click += new System.EventHandler(this.About_itemClick);
            // 
            // Exit_item
            // 
            this.Exit_item.Name = "Exit_item";
            this.Exit_item.Size = new System.Drawing.Size(224, 22);
            this.Exit_item.Text = "Exit (X)";
            this.Exit_item.Click += new System.EventHandler(this.ApplicationExit);
            // 
            // MenuBar
            // 
            this.MenuBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.File_bar,
            this.PageControl_bar,
            this.Bookmark_menu_bar,
            this.About_item_bar,
            this.InformationText_item_bar});
            this.MenuBar.Location = new System.Drawing.Point(0, 0);
            this.MenuBar.Name = "MenuBar";
            this.MenuBar.Size = new System.Drawing.Size(865, 24);
            this.MenuBar.TabIndex = 6;
            this.MenuBar.Text = "MenuBar";
            this.MenuBar.Visible = false;
            // 
            // File_bar
            // 
            this.File_bar.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Resume_item_bar,
            this.Load_item_bar,
            this.NextFile_item_bar,
            this.PreviousFile_item_bar,
            this.Exit_item_bar});
            this.File_bar.Name = "File_bar";
            this.File_bar.Size = new System.Drawing.Size(37, 20);
            this.File_bar.Text = "File";
            // 
            // Resume_item_bar
            // 
            this.Resume_item_bar.Name = "Resume_item_bar";
            this.Resume_item_bar.Size = new System.Drawing.Size(224, 22);
            this.Resume_item_bar.Text = "Resume last file(s) (R)";
            this.Resume_item_bar.Click += new System.EventHandler(this.ResumeLastFiles_Click);
            // 
            // Load_item_bar
            // 
            this.Load_item_bar.Name = "Load_item_bar";
            this.Load_item_bar.Size = new System.Drawing.Size(224, 22);
            this.Load_item_bar.Text = "Load File(s) (L)";
            this.Load_item_bar.Click += new System.EventHandler(this.LoadArchives_Click);
            // 
            // NextFile_item_bar
            // 
            this.NextFile_item_bar.Name = "NextFile_item_bar";
            this.NextFile_item_bar.Size = new System.Drawing.Size(224, 22);
            this.NextFile_item_bar.Text = "Next File (Alt + Page Down)";
            this.NextFile_item_bar.Click += new System.EventHandler(this.NextFile_Click);
            // 
            // PreviousFile_item_bar
            // 
            this.PreviousFile_item_bar.Name = "PreviousFile_item_bar";
            this.PreviousFile_item_bar.Size = new System.Drawing.Size(224, 22);
            this.PreviousFile_item_bar.Text = "Previous File (Alt + Page Up)";
            this.PreviousFile_item_bar.Click += new System.EventHandler(this.PreviousFile_Click);
            // 
            // Exit_item_bar
            // 
            this.Exit_item_bar.Name = "Exit_item_bar";
            this.Exit_item_bar.Size = new System.Drawing.Size(224, 22);
            this.Exit_item_bar.Text = "Exit (X)";
            this.Exit_item_bar.Click += new System.EventHandler(this.ApplicationExit);
            // 
            // PageControl_bar
            // 
            this.PageControl_bar.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.NextPage_item_bar,
            this.PreviousPage_item_bar,
            this.ShowPageInformation_item_bar});
            this.PageControl_bar.Name = "PageControl_bar";
            this.PageControl_bar.Size = new System.Drawing.Size(86, 20);
            this.PageControl_bar.Text = "Page control";
            // 
            // NextPage_item_bar
            // 
            this.NextPage_item_bar.Name = "NextPage_item_bar";
            this.NextPage_item_bar.Size = new System.Drawing.Size(212, 22);
            this.NextPage_item_bar.Text = "Next Page (Page Down)";
            // 
            // PreviousPage_item_bar
            // 
            this.PreviousPage_item_bar.Name = "PreviousPage_item_bar";
            this.PreviousPage_item_bar.Size = new System.Drawing.Size(212, 22);
            this.PreviousPage_item_bar.Text = "Previous Page (Page Up)";
            // 
            // ShowPageInformation_item_bar
            // 
            this.ShowPageInformation_item_bar.Name = "ShowPageInformation_item_bar";
            this.ShowPageInformation_item_bar.Size = new System.Drawing.Size(212, 22);
            this.ShowPageInformation_item_bar.Text = "Show Page Information (I)";
            this.ShowPageInformation_item_bar.Click += new System.EventHandler(this.ShowPageInformation_item_bar_Click);
            // 
            // Bookmark_menu_bar
            // 
            this.Bookmark_menu_bar.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AddBookmark_item_bar,
            this.ManageBookmarks_item_bar,
            this.Bookmark_Separator_bar});
            this.Bookmark_menu_bar.Name = "Bookmark_menu_bar";
            this.Bookmark_menu_bar.Size = new System.Drawing.Size(78, 20);
            this.Bookmark_menu_bar.Text = "Bookmarks";
            // 
            // AddBookmark_item_bar
            // 
            this.AddBookmark_item_bar.Name = "AddBookmark_item_bar";
            this.AddBookmark_item_bar.Size = new System.Drawing.Size(179, 22);
            this.AddBookmark_item_bar.Text = "Add bookmark";
            this.AddBookmark_item_bar.Click += new System.EventHandler(this.AddBookmark_item_Click);
            // 
            // ManageBookmarks_item_bar
            // 
            this.ManageBookmarks_item_bar.Name = "ManageBookmarks_item_bar";
            this.ManageBookmarks_item_bar.Size = new System.Drawing.Size(179, 22);
            this.ManageBookmarks_item_bar.Text = "Manage bookmarks";
            this.ManageBookmarks_item_bar.Click += new System.EventHandler(this.ManageBookmarks_item_Click);
            // 
            // Bookmark_Separator_bar
            // 
            this.Bookmark_Separator_bar.Name = "Bookmark_Separator_bar";
            this.Bookmark_Separator_bar.Size = new System.Drawing.Size(176, 6);
            // 
            // About_item_bar
            // 
            this.About_item_bar.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.About_item_bar.Name = "About_item_bar";
            this.About_item_bar.Size = new System.Drawing.Size(52, 20);
            this.About_item_bar.Text = "About";
            this.About_item_bar.Click += new System.EventHandler(this.About_itemClick);
            // 
            // InformationText_item_bar
            // 
            this.InformationText_item_bar.Name = "InformationText_item_bar";
            this.InformationText_item_bar.Size = new System.Drawing.Size(124, 20);
            this.InformationText_item_bar.Text = "Information text (N)";
            this.InformationText_item_bar.Click += new System.EventHandler(this.InformationText_item_bar_Click);
            // 
            // DisplayedImage
            // 
            this.DisplayedImage.Location = new System.Drawing.Point(0, 0);
            this.DisplayedImage.Margin = new System.Windows.Forms.Padding(0);
            this.DisplayedImage.Name = "DisplayedImage";
            this.DisplayedImage.Size = new System.Drawing.Size(103, 61);
            this.DisplayedImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.DisplayedImage.TabIndex = 1;
            this.DisplayedImage.TabStop = false;
            this.DisplayedImage.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Dragging);
            // 
            // Display_form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(865, 564);
            this.ContextMenuStrip = this.RightClick_menu;
            this.ControlBox = false;
            this.Controls.Add(this.MenuBar);
            this.Controls.Add(this.Page_lbl);
            this.Controls.Add(this.Message_lbl);
            this.Controls.Add(this.DisplayedImage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.MenuBar;
            this.Name = "Display_form";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "C# Comicviewer";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Display_Form_FormClosing);
            this.Load += new System.EventHandler(this.Display_Form_Load);
            this.ResizeEnd += new System.EventHandler(this.ResizeFix);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.KeyDown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.KeyPress);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Dragging);
            this.Resize += new System.EventHandler(this.ResizeFix);
            this.RightClick_menu.ResumeLayout(false);
            this.MenuBar.ResumeLayout(false);
            this.MenuBar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DisplayedImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        private System.Windows.Forms.ToolStripMenuItem About_item;

        #endregion

        private System.Windows.Forms.Label Message_lbl;
        private System.Windows.Forms.Label Page_lbl;
        private System.Windows.Forms.ContextMenuStrip RightClick_menu;
        private System.Windows.Forms.ToolStripMenuItem Resume_item;
        private System.Windows.Forms.ToolStripMenuItem Load_item;
        private System.Windows.Forms.ToolStripMenuItem NextPage_item;
        private System.Windows.Forms.ToolStripMenuItem PreviousPage_item;
        private System.Windows.Forms.ToolStripMenuItem Bookmark_menu;
        private System.Windows.Forms.ToolStripMenuItem AddBookmark_item;
        private System.Windows.Forms.ToolStripMenuItem Exit_item;
        private System.Windows.Forms.ToolStripSeparator Bookmark_Separator;
        private System.Windows.Forms.ToolStripMenuItem ManageBookmarks_item;
        private System.Windows.Forms.ToolStripMenuItem PreviousFile_item;
        private System.Windows.Forms.ToolStripMenuItem NextFile_item;
        private System.Windows.Forms.MenuStrip MenuBar;
        private System.Windows.Forms.ToolStripMenuItem File_bar;
        private System.Windows.Forms.ToolStripMenuItem Resume_item_bar;
        private System.Windows.Forms.ToolStripMenuItem Load_item_bar;
        private System.Windows.Forms.ToolStripMenuItem NextFile_item_bar;
        private System.Windows.Forms.ToolStripMenuItem PreviousFile_item_bar;
        private System.Windows.Forms.ToolStripMenuItem Exit_item_bar;
        private System.Windows.Forms.ToolStripMenuItem PageControl_bar;
        private System.Windows.Forms.ToolStripMenuItem NextPage_item_bar;
        private System.Windows.Forms.ToolStripMenuItem PreviousPage_item_bar;
        private System.Windows.Forms.ToolStripMenuItem Bookmark_menu_bar;
        private System.Windows.Forms.ToolStripMenuItem AddBookmark_item_bar;
        private System.Windows.Forms.ToolStripMenuItem ManageBookmarks_item_bar;
        private System.Windows.Forms.ToolStripSeparator Bookmark_Separator_bar;
        private System.Windows.Forms.ToolStripMenuItem About_item_bar;
        private System.Windows.Forms.PictureBox DisplayedImage;
        private System.Windows.Forms.ToolStripMenuItem ShowPageInformation_item_bar;
        private System.Windows.Forms.ToolStripMenuItem InformationText_item_bar;
    }
}

