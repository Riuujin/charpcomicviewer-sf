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
namespace csharp_comicviewer
{
	partial class About_Form
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(About_Form));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.title_lbl = new System.Windows.Forms.Label();
            this.version_lbl = new System.Windows.Forms.Label();
            this.icon_panel = new System.Windows.Forms.Panel();
            this.website_lbl = new System.Windows.Forms.LinkLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.Using_lbl = new System.Windows.Forms.Label();
            this.SevenZipSharp_lbl = new System.Windows.Forms.LinkLabel();
            this.Zip_lbl = new System.Windows.Forms.LinkLabel();
            this.Close_btn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.icon_panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(13, 40);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(48, 48);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // title_lbl
            // 
            this.title_lbl.Location = new System.Drawing.Point(80, 9);
            this.title_lbl.Name = "title_lbl";
            this.title_lbl.Size = new System.Drawing.Size(113, 23);
            this.title_lbl.TabIndex = 1;
            this.title_lbl.Text = "C# Comicviewer";
            // 
            // version_lbl
            // 
            this.version_lbl.Location = new System.Drawing.Point(80, 23);
            this.version_lbl.Name = "version_lbl";
            this.version_lbl.Size = new System.Drawing.Size(113, 23);
            this.version_lbl.TabIndex = 2;
            this.version_lbl.Text = "Version:";
            // 
            // icon_panel
            // 
            this.icon_panel.BackColor = System.Drawing.Color.White;
            this.icon_panel.Controls.Add(this.pictureBox1);
            this.icon_panel.Location = new System.Drawing.Point(-1, -1);
            this.icon_panel.Name = "icon_panel";
            this.icon_panel.Size = new System.Drawing.Size(75, 139);
            this.icon_panel.TabIndex = 3;
            // 
            // website_lbl
            // 
            this.website_lbl.Location = new System.Drawing.Point(80, 50);
            this.website_lbl.Name = "website_lbl";
            this.website_lbl.Size = new System.Drawing.Size(100, 23);
            this.website_lbl.TabIndex = 4;
            this.website_lbl.TabStop = true;
            this.website_lbl.Text = "Website";
            this.website_lbl.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.Website_lblLinkClicked);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(80, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(151, 24);
            this.label1.TabIndex = 5;
            this.label1.Text = "Made by: Rutger Spruyt";
            // 
            // Using_lbl
            // 
            this.Using_lbl.Location = new System.Drawing.Point(81, 64);
            this.Using_lbl.Name = "Using_lbl";
            this.Using_lbl.Size = new System.Drawing.Size(100, 23);
            this.Using_lbl.TabIndex = 6;
            this.Using_lbl.Text = "Using:";
            // 
            // SevenZipSharp_lbl
            // 
            this.SevenZipSharp_lbl.Location = new System.Drawing.Point(98, 77);
            this.SevenZipSharp_lbl.Name = "SevenZipSharp_lbl";
            this.SevenZipSharp_lbl.Size = new System.Drawing.Size(100, 23);
            this.SevenZipSharp_lbl.TabIndex = 7;
            this.SevenZipSharp_lbl.TabStop = true;
            this.SevenZipSharp_lbl.Text = "SevenZipSharp";
            this.SevenZipSharp_lbl.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.SevenZipSharp_lblLinkClicked);
            // 
            // Zip_lbl
            // 
            this.Zip_lbl.Location = new System.Drawing.Point(98, 91);
            this.Zip_lbl.Name = "Zip_lbl";
            this.Zip_lbl.Size = new System.Drawing.Size(100, 23);
            this.Zip_lbl.TabIndex = 8;
            this.Zip_lbl.TabStop = true;
            this.Zip_lbl.Text = "7-Zip";
            this.Zip_lbl.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.Zip_lblLinkClicked);
            // 
            // Close_btn
            // 
            this.Close_btn.Location = new System.Drawing.Point(137, 107);
            this.Close_btn.Name = "Close_btn";
            this.Close_btn.Size = new System.Drawing.Size(75, 23);
            this.Close_btn.TabIndex = 9;
            this.Close_btn.Text = "Close";
            this.Close_btn.UseVisualStyleBackColor = true;
            this.Close_btn.Click += new System.EventHandler(this.Close_btnClick);
            // 
            // About_Form
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(335, 132);
            this.Controls.Add(this.Close_btn);
            this.Controls.Add(this.Zip_lbl);
            this.Controls.Add(this.SevenZipSharp_lbl);
            this.Controls.Add(this.Using_lbl);
            this.Controls.Add(this.website_lbl);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.icon_panel);
            this.Controls.Add(this.version_lbl);
            this.Controls.Add(this.title_lbl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "About_Form";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "About";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.icon_panel.ResumeLayout(false);
            this.icon_panel.PerformLayout();
            this.ResumeLayout(false);

		}
		private System.Windows.Forms.Button Close_btn;
		private System.Windows.Forms.LinkLabel Zip_lbl;
		private System.Windows.Forms.LinkLabel SevenZipSharp_lbl;
		private System.Windows.Forms.Label Using_lbl;
		private System.Windows.Forms.Panel icon_panel;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.LinkLabel website_lbl;
		private System.Windows.Forms.Label title_lbl;
		private System.Windows.Forms.Label version_lbl;
		private System.Windows.Forms.PictureBox pictureBox1;
	}
}
