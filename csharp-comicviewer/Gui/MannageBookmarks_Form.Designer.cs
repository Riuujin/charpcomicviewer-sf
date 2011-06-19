namespace csharp_comicviewer.Gui
{
    partial class MannageBookmarks_Form
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
            this.Boomarks_dataGridView = new System.Windows.Forms.DataGridView();
            this.DeleteCol = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.CurrentFileNameCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PageNumberCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FileLocationCOl = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Ok_btn = new System.Windows.Forms.Button();
            this.Cancel_btn = new System.Windows.Forms.Button();
            this.Delete_btn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.Boomarks_dataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // Boomarks_dataGridView
            // 
            this.Boomarks_dataGridView.AllowUserToAddRows = false;
            this.Boomarks_dataGridView.AllowUserToDeleteRows = false;
            this.Boomarks_dataGridView.AllowUserToResizeColumns = false;
            this.Boomarks_dataGridView.AllowUserToResizeRows = false;
            this.Boomarks_dataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.Boomarks_dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Boomarks_dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DeleteCol,
            this.CurrentFileNameCol,
            this.PageNumberCol,
            this.FileLocationCOl});
            this.Boomarks_dataGridView.Location = new System.Drawing.Point(12, 12);
            this.Boomarks_dataGridView.MultiSelect = false;
            this.Boomarks_dataGridView.Name = "Boomarks_dataGridView";
            this.Boomarks_dataGridView.RowHeadersVisible = false;
            this.Boomarks_dataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.Boomarks_dataGridView.Size = new System.Drawing.Size(760, 238);
            this.Boomarks_dataGridView.TabIndex = 0;
            // 
            // DeleteCol
            // 
            this.DeleteCol.HeaderText = "Delete";
            this.DeleteCol.MinimumWidth = 15;
            this.DeleteCol.Name = "DeleteCol";
            this.DeleteCol.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.DeleteCol.Width = 44;
            // 
            // CurrentFileNameCol
            // 
            this.CurrentFileNameCol.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.CurrentFileNameCol.HeaderText = "Current file name";
            this.CurrentFileNameCol.MinimumWidth = 250;
            this.CurrentFileNameCol.Name = "CurrentFileNameCol";
            this.CurrentFileNameCol.ReadOnly = true;
            this.CurrentFileNameCol.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.CurrentFileNameCol.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.CurrentFileNameCol.Width = 250;
            // 
            // PageNumberCol
            // 
            this.PageNumberCol.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.PageNumberCol.HeaderText = "Page number";
            this.PageNumberCol.MinimumWidth = 80;
            this.PageNumberCol.Name = "PageNumberCol";
            this.PageNumberCol.ReadOnly = true;
            this.PageNumberCol.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.PageNumberCol.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.PageNumberCol.Width = 80;
            // 
            // FileLocationCOl
            // 
            this.FileLocationCOl.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.FileLocationCOl.HeaderText = "Curent file location";
            this.FileLocationCOl.MinimumWidth = 383;
            this.FileLocationCOl.Name = "FileLocationCOl";
            this.FileLocationCOl.ReadOnly = true;
            this.FileLocationCOl.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.FileLocationCOl.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.FileLocationCOl.Width = 383;
            // 
            // Ok_btn
            // 
            this.Ok_btn.Location = new System.Drawing.Point(616, 255);
            this.Ok_btn.Name = "Ok_btn";
            this.Ok_btn.Size = new System.Drawing.Size(75, 23);
            this.Ok_btn.TabIndex = 1;
            this.Ok_btn.Text = "Ok";
            this.Ok_btn.UseVisualStyleBackColor = true;
            this.Ok_btn.Click += new System.EventHandler(this.Ok_btn_Click);
            // 
            // Cancel_btn
            // 
            this.Cancel_btn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel_btn.Location = new System.Drawing.Point(697, 255);
            this.Cancel_btn.Name = "Cancel_btn";
            this.Cancel_btn.Size = new System.Drawing.Size(75, 23);
            this.Cancel_btn.TabIndex = 2;
            this.Cancel_btn.Text = "Cancel";
            this.Cancel_btn.UseVisualStyleBackColor = true;
            this.Cancel_btn.Click += new System.EventHandler(this.Cancel_btn_Click);
            // 
            // Delete_btn
            // 
            this.Delete_btn.Location = new System.Drawing.Point(326, 256);
            this.Delete_btn.Name = "Delete_btn";
            this.Delete_btn.Size = new System.Drawing.Size(89, 23);
            this.Delete_btn.TabIndex = 3;
            this.Delete_btn.Text = "Delete selected";
            this.Delete_btn.UseVisualStyleBackColor = true;
            this.Delete_btn.Click += new System.EventHandler(this.Delete_btn_Click);
            // 
            // MannageBookmarks_Form
            // 
            this.AcceptButton = this.Ok_btn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.Cancel_btn;
            this.ClientSize = new System.Drawing.Size(784, 290);
            this.Controls.Add(this.Delete_btn);
            this.Controls.Add(this.Cancel_btn);
            this.Controls.Add(this.Ok_btn);
            this.Controls.Add(this.Boomarks_dataGridView);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MannageBookmarks_Form";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Bookmark manager";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.ManageBookmarks_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Boomarks_dataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView Boomarks_dataGridView;
        private System.Windows.Forms.Button Ok_btn;
        private System.Windows.Forms.Button Cancel_btn;
        private System.Windows.Forms.Button Delete_btn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn DeleteCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn CurrentFileNameCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn PageNumberCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn FileLocationCOl;
    }
}