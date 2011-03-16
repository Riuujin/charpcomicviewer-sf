namespace csharp_comicviewer
{
    partial class ManageBookmarks
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
            this.Bookmarks_chckdLstBx = new System.Windows.Forms.CheckedListBox();
            this.Bookmarks_lbl = new System.Windows.Forms.Label();
            this.Delete_btn = new System.Windows.Forms.Button();
            this.Cancel_btn = new System.Windows.Forms.Button();
            this.Ok_btn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Bookmarks_chckdLstBx
            // 
            this.Bookmarks_chckdLstBx.BackColor = System.Drawing.SystemColors.Control;
            this.Bookmarks_chckdLstBx.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Bookmarks_chckdLstBx.CheckOnClick = true;
            this.Bookmarks_chckdLstBx.FormattingEnabled = true;
            this.Bookmarks_chckdLstBx.HorizontalScrollbar = true;
            this.Bookmarks_chckdLstBx.Location = new System.Drawing.Point(12, 34);
            this.Bookmarks_chckdLstBx.Name = "Bookmarks_chckdLstBx";
            this.Bookmarks_chckdLstBx.Size = new System.Drawing.Size(752, 255);
            this.Bookmarks_chckdLstBx.TabIndex = 0;
            // 
            // Bookmarks_lbl
            // 
            this.Bookmarks_lbl.AutoSize = true;
            this.Bookmarks_lbl.Location = new System.Drawing.Point(13, 3);
            this.Bookmarks_lbl.Name = "Bookmarks_lbl";
            this.Bookmarks_lbl.Size = new System.Drawing.Size(203, 13);
            this.Bookmarks_lbl.TabIndex = 1;
            this.Bookmarks_lbl.Text = "Select the bookmarks you want to delete.";
            // 
            // Delete_btn
            // 
            this.Delete_btn.Location = new System.Drawing.Point(339, 297);
            this.Delete_btn.Name = "Delete_btn";
            this.Delete_btn.Size = new System.Drawing.Size(92, 23);
            this.Delete_btn.TabIndex = 2;
            this.Delete_btn.Text = "Delete Checked";
            this.Delete_btn.UseVisualStyleBackColor = true;
            this.Delete_btn.Click += new System.EventHandler(this.Delete_btn_Click);
            // 
            // Cancel_btn
            // 
            this.Cancel_btn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel_btn.Location = new System.Drawing.Point(689, 297);
            this.Cancel_btn.Name = "Cancel_btn";
            this.Cancel_btn.Size = new System.Drawing.Size(75, 23);
            this.Cancel_btn.TabIndex = 3;
            this.Cancel_btn.Text = "Cancel";
            this.Cancel_btn.UseVisualStyleBackColor = true;
            this.Cancel_btn.Click += new System.EventHandler(this.Cancel_btn_Click);
            // 
            // Ok_btn
            // 
            this.Ok_btn.Location = new System.Drawing.Point(608, 297);
            this.Ok_btn.Name = "Ok_btn";
            this.Ok_btn.Size = new System.Drawing.Size(75, 23);
            this.Ok_btn.TabIndex = 4;
            this.Ok_btn.Text = "Ok";
            this.Ok_btn.UseVisualStyleBackColor = true;
            this.Ok_btn.Click += new System.EventHandler(this.Ok_btn_Click);
            // 
            // ManageBookmarks
            // 
            this.AcceptButton = this.Delete_btn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.Cancel_btn;
            this.ClientSize = new System.Drawing.Size(776, 332);
            this.Controls.Add(this.Ok_btn);
            this.Controls.Add(this.Cancel_btn);
            this.Controls.Add(this.Delete_btn);
            this.Controls.Add(this.Bookmarks_lbl);
            this.Controls.Add(this.Bookmarks_chckdLstBx);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ManageBookmarks";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Manage Bookmarks";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.ManageBookmarks_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckedListBox Bookmarks_chckdLstBx;
        private System.Windows.Forms.Label Bookmarks_lbl;
        private System.Windows.Forms.Button Delete_btn;
        private System.Windows.Forms.Button Cancel_btn;
        private System.Windows.Forms.Button Ok_btn;
    }
}