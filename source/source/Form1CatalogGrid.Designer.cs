namespace DazUnpacker
{
    partial class Form1CatalogGrid
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.columnFilePath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnFileName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnFileType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnModified = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnArchivePath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnArchiveName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnArchiveType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnArchiveCreated = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuGrid = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemCopy = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.contextMenuGrid.SuspendLayout();
            this.SuspendLayout();
            //
            // dataGridView
            //
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.AllowUserToDeleteRows = false;
            this.dataGridView.AllowUserToResizeRows = false;
            this.dataGridView.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dataGridView.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.columnFilePath,
            this.columnFileName,
            this.columnFileType,
            this.columnModified,
            this.columnArchivePath,
            this.columnArchiveName,
            this.columnArchiveType,
            this.columnArchiveCreated});
            this.dataGridView.ContextMenuStrip = this.contextMenuGrid;
            this.dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView.Location = new System.Drawing.Point(0, 0);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.ReadOnly = true;
            this.dataGridView.RowHeadersVisible = false;
            this.dataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dataGridView.Size = new System.Drawing.Size(600, 300);
            this.dataGridView.TabIndex = 0;
            this.dataGridView.VirtualMode = true;
            this.dataGridView.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView_CellMouseDown);
            this.dataGridView.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.dataGridView_CellValueNeeded);
            this.dataGridView.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView_ColumnHeaderMouseClick);
            //
            // columnFilePath
            //
            this.columnFilePath.HeaderText = "FilePath";
            this.columnFilePath.Name = "columnFilePath";
            this.columnFilePath.ReadOnly = true;
            this.columnFilePath.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.columnFilePath.Width = 420;
            //
            // columnFileName
            //
            this.columnFileName.HeaderText = "FileName";
            this.columnFileName.Name = "columnFileName";
            this.columnFileName.ReadOnly = true;
            this.columnFileName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.columnFileName.Width = 200;
            //
            // columnFileType
            //
            this.columnFileType.HeaderText = "FileType";
            this.columnFileType.Name = "columnFileType";
            this.columnFileType.ReadOnly = true;
            this.columnFileType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.columnFileType.Width = 80;
            //
            // columnModified
            //
            this.columnModified.HeaderText = "Modified";
            this.columnModified.Name = "columnModified";
            this.columnModified.ReadOnly = true;
            this.columnModified.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.columnModified.Width = 130;
            //
            // columnArchivePath
            //
            this.columnArchivePath.HeaderText = "ArchivePath";
            this.columnArchivePath.Name = "columnArchivePath";
            this.columnArchivePath.ReadOnly = true;
            this.columnArchivePath.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.columnArchivePath.Width = 320;
            //
            // columnArchiveName
            //
            this.columnArchiveName.HeaderText = "ArchiveName";
            this.columnArchiveName.Name = "columnArchiveName";
            this.columnArchiveName.ReadOnly = true;
            this.columnArchiveName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.columnArchiveName.Width = 220;
            //
            // columnArchiveType
            //
            this.columnArchiveType.HeaderText = "ArchiveType";
            this.columnArchiveType.Name = "columnArchiveType";
            this.columnArchiveType.ReadOnly = true;
            this.columnArchiveType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.columnArchiveType.Width = 100;
            //
            // columnArchiveCreated
            //
            this.columnArchiveCreated.HeaderText = "ArchiveCreated";
            this.columnArchiveCreated.Name = "columnArchiveCreated";
            this.columnArchiveCreated.ReadOnly = true;
            this.columnArchiveCreated.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.columnArchiveCreated.Width = 130;
            //
            // contextMenuGrid
            //
            this.contextMenuGrid.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemCopy});
            this.contextMenuGrid.Name = "contextMenuGrid";
            this.contextMenuGrid.Size = new System.Drawing.Size(100, 26);
            this.contextMenuGrid.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuGrid_Opening);
            //
            // toolStripMenuItemCopy
            //
            this.toolStripMenuItemCopy.Name = "toolStripMenuItemCopy";
            this.toolStripMenuItemCopy.Size = new System.Drawing.Size(99, 22);
            this.toolStripMenuItemCopy.Text = "Copy";
            this.toolStripMenuItemCopy.Click += new System.EventHandler(this.toolStripMenuItemCopy_Click);
            //
            // Form1CatalogGrid
            //
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.dataGridView);
            this.Name = "Form1CatalogGrid";
            this.Size = new System.Drawing.Size(600, 300);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.contextMenuGrid.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnFilePath;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnFileName;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnFileType;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnModified;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnArchivePath;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnArchiveName;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnArchiveType;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnArchiveCreated;
        private System.Windows.Forms.ContextMenuStrip contextMenuGrid;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemCopy;
    }
}
