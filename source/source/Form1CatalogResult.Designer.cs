namespace DazUnpacker
{
    partial class Form1CatalogResult
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

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.labelSummary = new System.Windows.Forms.Label();
            this.checkBoxUniqueArchive = new System.Windows.Forms.CheckBox();
            this.labelOnDisk = new System.Windows.Forms.Label();
            this.gridOnDisk = new DazUnpacker.Form1CatalogGrid();
            this.labelMissing = new System.Windows.Forms.Label();
            this.gridMissing = new DazUnpacker.Form1CatalogGrid();
            this.labelNotFound = new System.Windows.Forms.Label();
            this.listBoxNotFound = new System.Windows.Forms.ListBox();
            this.contextMenuNotFound = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonClose = new System.Windows.Forms.Button();
            this.tableLayoutPanel.SuspendLayout();
            this.contextMenuNotFound.SuspendLayout();
            this.SuspendLayout();
            //
            // tableLayoutPanel
            //
            this.tableLayoutPanel.ColumnCount = 1;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Controls.Add(this.labelSummary, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.checkBoxUniqueArchive, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.labelOnDisk, 0, 2);
            this.tableLayoutPanel.Controls.Add(this.gridOnDisk, 0, 3);
            this.tableLayoutPanel.Controls.Add(this.labelMissing, 0, 4);
            this.tableLayoutPanel.Controls.Add(this.gridMissing, 0, 5);
            this.tableLayoutPanel.Controls.Add(this.labelNotFound, 0, 6);
            this.tableLayoutPanel.Controls.Add(this.listBoxNotFound, 0, 7);
            this.tableLayoutPanel.Controls.Add(this.buttonClose, 0, 8);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.Padding = new System.Windows.Forms.Padding(8);
            this.tableLayoutPanel.RowCount = 9;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 38F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 38F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 24F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.Size = new System.Drawing.Size(1000, 760);
            this.tableLayoutPanel.TabIndex = 0;
            //
            // labelSummary
            //
            this.labelSummary.AutoSize = true;
            this.labelSummary.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
            this.labelSummary.Location = new System.Drawing.Point(11, 8);
            this.labelSummary.Name = "labelSummary";
            this.labelSummary.Padding = new System.Windows.Forms.Padding(0, 0, 0, 6);
            this.labelSummary.Size = new System.Drawing.Size(60, 22);
            this.labelSummary.TabIndex = 0;
            //
            // checkBoxUniqueArchive
            //
            this.checkBoxUniqueArchive.AutoSize = true;
            this.checkBoxUniqueArchive.Location = new System.Drawing.Point(11, 33);
            this.checkBoxUniqueArchive.Name = "checkBoxUniqueArchive";
            this.checkBoxUniqueArchive.Size = new System.Drawing.Size(95, 17);
            this.checkBoxUniqueArchive.TabIndex = 1;
            this.checkBoxUniqueArchive.Text = "Unique archive";
            this.checkBoxUniqueArchive.UseVisualStyleBackColor = true;
            this.checkBoxUniqueArchive.CheckedChanged += new System.EventHandler(this.checkBoxUniqueArchive_CheckedChanged);
            //
            // labelOnDisk
            //
            this.labelOnDisk.AutoSize = true;
            this.labelOnDisk.Location = new System.Drawing.Point(11, 53);
            this.labelOnDisk.Name = "labelOnDisk";
            this.labelOnDisk.Padding = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.labelOnDisk.Size = new System.Drawing.Size(60, 21);
            this.labelOnDisk.TabIndex = 2;
            //
            // gridOnDisk
            //
            this.gridOnDisk.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridOnDisk.Location = new System.Drawing.Point(11, 54);
            this.gridOnDisk.Name = "gridOnDisk";
            this.gridOnDisk.Size = new System.Drawing.Size(978, 200);
            this.gridOnDisk.TabIndex = 3;
            //
            // labelMissing
            //
            this.labelMissing.AutoSize = true;
            this.labelMissing.Location = new System.Drawing.Point(11, 257);
            this.labelMissing.Name = "labelMissing";
            this.labelMissing.Padding = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.labelMissing.Size = new System.Drawing.Size(60, 21);
            this.labelMissing.TabIndex = 4;
            //
            // gridMissing
            //
            this.gridMissing.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridMissing.Location = new System.Drawing.Point(11, 281);
            this.gridMissing.Name = "gridMissing";
            this.gridMissing.Size = new System.Drawing.Size(978, 200);
            this.gridMissing.TabIndex = 5;
            //
            // labelNotFound
            //
            this.labelNotFound.AutoSize = true;
            this.labelNotFound.Location = new System.Drawing.Point(11, 484);
            this.labelNotFound.Name = "labelNotFound";
            this.labelNotFound.Padding = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.labelNotFound.Size = new System.Drawing.Size(60, 21);
            this.labelNotFound.TabIndex = 6;
            //
            // listBoxNotFound
            //
            this.listBoxNotFound.ContextMenuStrip = this.contextMenuNotFound;
            this.listBoxNotFound.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxNotFound.FormattingEnabled = true;
            this.listBoxNotFound.HorizontalScrollbar = true;
            this.listBoxNotFound.Location = new System.Drawing.Point(11, 508);
            this.listBoxNotFound.Name = "listBoxNotFound";
            this.listBoxNotFound.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBoxNotFound.Size = new System.Drawing.Size(978, 130);
            this.listBoxNotFound.TabIndex = 7;
            this.listBoxNotFound.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listBoxNotFound_MouseDown);
            //
            // contextMenuNotFound
            //
            this.contextMenuNotFound.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemCopy});
            this.contextMenuNotFound.Name = "contextMenuNotFound";
            this.contextMenuNotFound.Size = new System.Drawing.Size(100, 26);
            this.contextMenuNotFound.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuNotFound_Opening);
            //
            // toolStripMenuItemCopy
            //
            this.toolStripMenuItemCopy.Name = "toolStripMenuItemCopy";
            this.toolStripMenuItemCopy.Size = new System.Drawing.Size(99, 22);
            this.toolStripMenuItemCopy.Text = "Copy";
            this.toolStripMenuItemCopy.Click += new System.EventHandler(this.toolStripMenuItemCopy_Click);
            //
            // buttonClose
            //
            this.buttonClose.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.buttonClose.AutoSize = true;
            this.buttonClose.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonClose.Location = new System.Drawing.Point(914, 644);
            this.buttonClose.MinimumSize = new System.Drawing.Size(75, 28);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(75, 28);
            this.buttonClose.TabIndex = 8;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            //
            // Form1CatalogResult
            //
            this.AcceptButton = this.buttonClose;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this.buttonClose;
            this.ClientSize = new System.Drawing.Size(1000, 760);
            this.Controls.Add(this.tableLayoutPanel);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(600, 500);
            this.Name = "Form1CatalogResult";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Search & Install: result";
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.contextMenuNotFound.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Label labelSummary;
        private System.Windows.Forms.CheckBox checkBoxUniqueArchive;
        private System.Windows.Forms.Label labelOnDisk;
        private Form1CatalogGrid gridOnDisk;
        private System.Windows.Forms.Label labelMissing;
        private Form1CatalogGrid gridMissing;
        private System.Windows.Forms.Label labelNotFound;
        private System.Windows.Forms.ListBox listBoxNotFound;
        private System.Windows.Forms.ContextMenuStrip contextMenuNotFound;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemCopy;
        private System.Windows.Forms.Button buttonClose;
    }
}
