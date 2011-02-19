﻿/*
 * Created by SharpDevelop.
 * User: Revvion
 * Date: 29-1-2011
 * Time: 0:06
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace csharp_comicviewer
{
	partial class InfoText
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InfoText));
            this.InfoText_RchTxtBx = new System.Windows.Forms.RichTextBox();
            this.Close_btn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // InfoText_RchTxtBx
            // 
            this.InfoText_RchTxtBx.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.InfoText_RchTxtBx.BackColor = System.Drawing.SystemColors.Info;
            this.InfoText_RchTxtBx.Location = new System.Drawing.Point(12, 12);
            this.InfoText_RchTxtBx.Name = "InfoText_RchTxtBx";
            this.InfoText_RchTxtBx.ReadOnly = true;
            this.InfoText_RchTxtBx.Size = new System.Drawing.Size(693, 442);
            this.InfoText_RchTxtBx.TabIndex = 0;
            this.InfoText_RchTxtBx.Text = "";
            // 
            // Close_btn
            // 
            this.Close_btn.Location = new System.Drawing.Point(309, 458);
            this.Close_btn.Name = "Close_btn";
            this.Close_btn.Size = new System.Drawing.Size(75, 23);
            this.Close_btn.TabIndex = 1;
            this.Close_btn.Text = "Close";
            this.Close_btn.UseVisualStyleBackColor = true;
            this.Close_btn.Click += new System.EventHandler(this.Close_btnClick);
            // 
            // InfoText
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(717, 493);
            this.Controls.Add(this.Close_btn);
            this.Controls.Add(this.InfoText_RchTxtBx);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InfoText";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Info text of:";
            this.ResumeLayout(false);

		}
		private System.Windows.Forms.Button Close_btn;
		private System.Windows.Forms.RichTextBox InfoText_RchTxtBx;
	}
}
