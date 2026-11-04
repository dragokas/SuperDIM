namespace DazUnpacker
{
    partial class Form1Catalog
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
            this.flowLayoutPanelScan = new System.Windows.Forms.FlowLayoutPanel();
            this.labelFolder = new System.Windows.Forms.Label();
            this.textBoxFolder = new System.Windows.Forms.TextBox();
            this.buttonSelectFolder = new System.Windows.Forms.Button();
            this.labelThreads = new System.Windows.Forms.Label();
            this.numericUpDownThreads = new System.Windows.Forms.NumericUpDown();
            this.buttonScan = new System.Windows.Forms.Button();
            this.buttonStop = new System.Windows.Forms.Button();
            this.flowLayoutPanelFilter = new System.Windows.Forms.FlowLayoutPanel();
            this.labelFilter = new System.Windows.Forms.Label();
            this.textBoxFilter = new System.Windows.Forms.TextBox();
            this.checkBoxIgnoreCase = new System.Windows.Forms.CheckBox();
            this.labelCount = new System.Windows.Forms.Label();
            this.flowLayoutPanelActions = new System.Windows.Forms.FlowLayoutPanel();
            this.buttonExportCsv = new System.Windows.Forms.Button();
            this.buttonImportCsv = new System.Windows.Forms.Button();
            this.buttonClearDatabase = new System.Windows.Forms.Button();
            this.buttonSearchInstall = new System.Windows.Forms.Button();
            this.gridCatalog = new DazUnpacker.Form1CatalogGrid();
            this.panelProgress = new System.Windows.Forms.Panel();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.labelProgress = new System.Windows.Forms.Label();
            this.timerFilter = new System.Windows.Forms.Timer(this.components);
            this.saveFileDialogCsv = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialogCsv = new System.Windows.Forms.OpenFileDialog();
            this.tableLayoutPanel.SuspendLayout();
            this.flowLayoutPanelScan.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownThreads)).BeginInit();
            this.flowLayoutPanelFilter.SuspendLayout();
            this.flowLayoutPanelActions.SuspendLayout();
            this.panelProgress.SuspendLayout();
            this.SuspendLayout();
            //
            // tableLayoutPanel
            //
            this.tableLayoutPanel.ColumnCount = 1;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Controls.Add(this.flowLayoutPanelScan, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.flowLayoutPanelFilter, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.flowLayoutPanelActions, 0, 2);
            this.tableLayoutPanel.Controls.Add(this.gridCatalog, 0, 3);
            this.tableLayoutPanel.Controls.Add(this.panelProgress, 0, 4);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.Padding = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel.RowCount = 5;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.Size = new System.Drawing.Size(1000, 700);
            this.tableLayoutPanel.TabIndex = 0;
            //
            // flowLayoutPanelScan
            //
            this.flowLayoutPanelScan.AutoSize = true;
            this.flowLayoutPanelScan.Controls.Add(this.labelFolder);
            this.flowLayoutPanelScan.Controls.Add(this.textBoxFolder);
            this.flowLayoutPanelScan.Controls.Add(this.buttonSelectFolder);
            this.flowLayoutPanelScan.Controls.Add(this.labelThreads);
            this.flowLayoutPanelScan.Controls.Add(this.numericUpDownThreads);
            this.flowLayoutPanelScan.Controls.Add(this.buttonScan);
            this.flowLayoutPanelScan.Controls.Add(this.buttonStop);
            this.flowLayoutPanelScan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanelScan.Location = new System.Drawing.Point(7, 7);
            this.flowLayoutPanelScan.Name = "flowLayoutPanelScan";
            this.flowLayoutPanelScan.Size = new System.Drawing.Size(986, 34);
            this.flowLayoutPanelScan.TabIndex = 0;
            //
            // labelFolder
            //
            this.labelFolder.AutoSize = true;
            this.labelFolder.Location = new System.Drawing.Point(3, 8);
            this.labelFolder.Margin = new System.Windows.Forms.Padding(3, 8, 3, 0);
            this.labelFolder.Name = "labelFolder";
            this.labelFolder.Size = new System.Drawing.Size(88, 13);
            this.labelFolder.TabIndex = 0;
            this.labelFolder.Text = "Archives folder:";
            //
            // textBoxFolder
            //
            this.textBoxFolder.Location = new System.Drawing.Point(97, 5);
            this.textBoxFolder.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.textBoxFolder.Name = "textBoxFolder";
            this.textBoxFolder.Size = new System.Drawing.Size(190, 20);
            this.textBoxFolder.TabIndex = 1;
            //
            // buttonSelectFolder
            //
            this.buttonSelectFolder.AutoSize = false;
            this.buttonSelectFolder.Location = new System.Drawing.Point(483, 3);
            this.buttonSelectFolder.MinimumSize = new System.Drawing.Size(75, 28);
            this.buttonSelectFolder.Name = "buttonSelectFolder";
            this.buttonSelectFolder.Size = new System.Drawing.Size(75, 28);
            this.buttonSelectFolder.TabIndex = 2;
            this.buttonSelectFolder.Text = "Select ...";
            this.buttonSelectFolder.UseVisualStyleBackColor = true;
            this.buttonSelectFolder.Click += new System.EventHandler(this.buttonSelectFolder_Click);
            //
            // labelThreads
            //
            this.labelThreads.AutoSize = true;
            this.labelThreads.Location = new System.Drawing.Point(576, 8);
            this.labelThreads.Margin = new System.Windows.Forms.Padding(8, 8, 3, 0);
            this.labelThreads.Name = "labelThreads";
            this.labelThreads.Size = new System.Drawing.Size(46, 13);
            this.labelThreads.TabIndex = 3;
            this.labelThreads.Text = "Threads:";
            //
            // numericUpDownThreads
            //
            this.numericUpDownThreads.Location = new System.Drawing.Point(628, 5);
            this.numericUpDownThreads.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.numericUpDownThreads.Maximum = new decimal(new int[] {
            32,
            0,
            0,
            0});
            this.numericUpDownThreads.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownThreads.Name = "numericUpDownThreads";
            this.numericUpDownThreads.Size = new System.Drawing.Size(50, 20);
            this.numericUpDownThreads.TabIndex = 4;
            this.numericUpDownThreads.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            //
            // buttonScan
            //
            this.buttonScan.AutoSize = false;
            this.buttonScan.Location = new System.Drawing.Point(684, 3);
            this.buttonScan.MinimumSize = new System.Drawing.Size(75, 28);
            this.buttonScan.Name = "buttonScan";
            this.buttonScan.Size = new System.Drawing.Size(75, 28);
            this.buttonScan.TabIndex = 5;
            this.buttonScan.Text = "Scan";
            this.buttonScan.UseVisualStyleBackColor = true;
            this.buttonScan.Click += new System.EventHandler(this.buttonScan_Click);
            //
            // buttonStop
            //
            this.buttonStop.AutoSize = false;
            this.buttonStop.Enabled = false;
            this.buttonStop.Location = new System.Drawing.Point(765, 3);
            this.buttonStop.MinimumSize = new System.Drawing.Size(75, 28);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(75, 28);
            this.buttonStop.TabIndex = 6;
            this.buttonStop.Text = "Stop";
            this.buttonStop.UseVisualStyleBackColor = true;
            this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
            //
            // flowLayoutPanelFilter
            //
            this.flowLayoutPanelFilter.AutoSize = true;
            this.flowLayoutPanelFilter.Controls.Add(this.labelFilter);
            this.flowLayoutPanelFilter.Controls.Add(this.textBoxFilter);
            this.flowLayoutPanelFilter.Controls.Add(this.checkBoxIgnoreCase);
            this.flowLayoutPanelFilter.Controls.Add(this.labelCount);
            this.flowLayoutPanelFilter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanelFilter.Location = new System.Drawing.Point(7, 47);
            this.flowLayoutPanelFilter.Name = "flowLayoutPanelFilter";
            this.flowLayoutPanelFilter.Size = new System.Drawing.Size(986, 31);
            this.flowLayoutPanelFilter.TabIndex = 1;
            //
            // labelFilter
            //
            this.labelFilter.AutoSize = true;
            this.labelFilter.Location = new System.Drawing.Point(3, 8);
            this.labelFilter.Margin = new System.Windows.Forms.Padding(3, 8, 3, 0);
            this.labelFilter.Name = "labelFilter";
            this.labelFilter.Size = new System.Drawing.Size(88, 13);
            this.labelFilter.TabIndex = 0;
            this.labelFilter.Text = "Filter (FilePath):";
            //
            // textBoxFilter
            //
            this.textBoxFilter.Location = new System.Drawing.Point(97, 5);
            this.textBoxFilter.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.textBoxFilter.Name = "textBoxFilter";
            this.textBoxFilter.Size = new System.Drawing.Size(190, 20);
            this.textBoxFilter.TabIndex = 1;
            this.textBoxFilter.TextChanged += new System.EventHandler(this.FilterChanged);
            //
            // checkBoxIgnoreCase
            //
            this.checkBoxIgnoreCase.AutoSize = true;
            this.checkBoxIgnoreCase.Location = new System.Drawing.Point(483, 6);
            this.checkBoxIgnoreCase.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
            this.checkBoxIgnoreCase.Name = "checkBoxIgnoreCase";
            this.checkBoxIgnoreCase.Size = new System.Drawing.Size(80, 17);
            this.checkBoxIgnoreCase.TabIndex = 2;
            this.checkBoxIgnoreCase.Text = "Ignore case";
            this.checkBoxIgnoreCase.UseVisualStyleBackColor = true;
            this.checkBoxIgnoreCase.CheckedChanged += new System.EventHandler(this.FilterChanged);
            //
            // labelCount
            //
            this.labelCount.AutoSize = true;
            this.labelCount.Location = new System.Drawing.Point(584, 8);
            this.labelCount.Margin = new System.Windows.Forms.Padding(18, 8, 3, 0);
            this.labelCount.Name = "labelCount";
            this.labelCount.Size = new System.Drawing.Size(0, 13);
            this.labelCount.TabIndex = 3;
            //
            // flowLayoutPanelActions
            //
            this.flowLayoutPanelActions.AutoSize = true;
            this.flowLayoutPanelActions.Controls.Add(this.buttonExportCsv);
            this.flowLayoutPanelActions.Controls.Add(this.buttonImportCsv);
            this.flowLayoutPanelActions.Controls.Add(this.buttonClearDatabase);
            this.flowLayoutPanelActions.Controls.Add(this.buttonSearchInstall);
            this.flowLayoutPanelActions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanelActions.Location = new System.Drawing.Point(7, 84);
            this.flowLayoutPanelActions.Name = "flowLayoutPanelActions";
            this.flowLayoutPanelActions.Size = new System.Drawing.Size(986, 34);
            this.flowLayoutPanelActions.TabIndex = 2;
            //
            // buttonExportCsv
            //
            this.buttonExportCsv.AutoSize = false;
            this.buttonExportCsv.Location = new System.Drawing.Point(3, 3);
            this.buttonExportCsv.MinimumSize = new System.Drawing.Size(75, 28);
            this.buttonExportCsv.Name = "buttonExportCsv";
            this.buttonExportCsv.Size = new System.Drawing.Size(115, 28);
            this.buttonExportCsv.TabIndex = 0;
            this.buttonExportCsv.Text = "Export to CSV ...";
            this.buttonExportCsv.UseVisualStyleBackColor = true;
            this.buttonExportCsv.Click += new System.EventHandler(this.buttonExportCsv_Click);
            //
            // buttonImportCsv
            //
            this.buttonImportCsv.AutoSize = false;
            this.buttonImportCsv.Location = new System.Drawing.Point(109, 3);
            this.buttonImportCsv.MinimumSize = new System.Drawing.Size(75, 28);
            this.buttonImportCsv.Name = "buttonImportCsv";
            this.buttonImportCsv.Size = new System.Drawing.Size(125, 28);
            this.buttonImportCsv.TabIndex = 1;
            this.buttonImportCsv.Text = "Import from CSV ...";
            this.buttonImportCsv.UseVisualStyleBackColor = true;
            this.buttonImportCsv.Click += new System.EventHandler(this.buttonImportCsv_Click);
            //
            // buttonClearDatabase
            //
            this.buttonClearDatabase.AutoSize = false;
            this.buttonClearDatabase.Location = new System.Drawing.Point(225, 3);
            this.buttonClearDatabase.MinimumSize = new System.Drawing.Size(75, 28);
            this.buttonClearDatabase.Name = "buttonClearDatabase";
            this.buttonClearDatabase.Size = new System.Drawing.Size(115, 28);
            this.buttonClearDatabase.TabIndex = 2;
            this.buttonClearDatabase.Text = "Clear Database";
            this.buttonClearDatabase.UseVisualStyleBackColor = true;
            this.buttonClearDatabase.Click += new System.EventHandler(this.buttonClearDatabase_Click);
            //
            // buttonSearchInstall
            //
            this.buttonSearchInstall.AutoSize = false;
            this.buttonSearchInstall.Location = new System.Drawing.Point(331, 3);
            this.buttonSearchInstall.Margin = new System.Windows.Forms.Padding(20, 3, 3, 3);
            this.buttonSearchInstall.MinimumSize = new System.Drawing.Size(75, 28);
            this.buttonSearchInstall.Name = "buttonSearchInstall";
            this.buttonSearchInstall.Size = new System.Drawing.Size(235, 28);
            this.buttonSearchInstall.TabIndex = 3;
            this.buttonSearchInstall.Text = "Search && Install by file paths ...";
            this.buttonSearchInstall.UseVisualStyleBackColor = true;
            this.buttonSearchInstall.Click += new System.EventHandler(this.buttonSearchInstall_Click);
            //
            // gridCatalog
            //
            this.gridCatalog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridCatalog.Location = new System.Drawing.Point(7, 124);
            this.gridCatalog.Name = "gridCatalog";
            this.gridCatalog.Size = new System.Drawing.Size(986, 532);
            this.gridCatalog.TabIndex = 3;
            //
            // panelProgress
            //
            this.panelProgress.Controls.Add(this.progressBar);
            this.panelProgress.Controls.Add(this.labelProgress);
            this.panelProgress.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelProgress.Location = new System.Drawing.Point(7, 662);
            this.panelProgress.Name = "panelProgress";
            this.panelProgress.Size = new System.Drawing.Size(986, 31);
            this.panelProgress.TabIndex = 4;
            //
            // progressBar
            //
            this.progressBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.progressBar.Location = new System.Drawing.Point(0, 0);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(786, 31);
            this.progressBar.TabIndex = 0;
            //
            // labelProgress
            //
            this.labelProgress.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelProgress.Location = new System.Drawing.Point(786, 0);
            this.labelProgress.Name = "labelProgress";
            this.labelProgress.Size = new System.Drawing.Size(200, 31);
            this.labelProgress.TabIndex = 1;
            this.labelProgress.Text = "0 / 0";
            this.labelProgress.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // timerFilter
            //
            this.timerFilter.Interval = 1000;
            this.timerFilter.Tick += new System.EventHandler(this.timerFilter_Tick);
            //
            // saveFileDialogCsv
            //
            this.saveFileDialogCsv.DefaultExt = "csv";
            this.saveFileDialogCsv.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
            this.saveFileDialogCsv.Title = "Export the catalog";
            //
            // openFileDialogCsv
            //
            this.openFileDialogCsv.DefaultExt = "csv";
            this.openFileDialogCsv.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
            this.openFileDialogCsv.Title = "Import the catalog";
            //
            // Form1Catalog
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1000, 700);
            this.ControlBox = false;
            this.Controls.Add(this.tableLayoutPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Form1Catalog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Catalog";
            this.Load += new System.EventHandler(this.Form1Catalog_Load);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.flowLayoutPanelScan.ResumeLayout(false);
            this.flowLayoutPanelScan.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownThreads)).EndInit();
            this.flowLayoutPanelFilter.ResumeLayout(false);
            this.flowLayoutPanelFilter.PerformLayout();
            this.flowLayoutPanelActions.ResumeLayout(false);
            this.flowLayoutPanelActions.PerformLayout();
            this.panelProgress.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelScan;
        private System.Windows.Forms.Label labelFolder;
        internal System.Windows.Forms.TextBox textBoxFolder;
        private System.Windows.Forms.Button buttonSelectFolder;
        private System.Windows.Forms.Label labelThreads;
        internal System.Windows.Forms.NumericUpDown numericUpDownThreads;
        private System.Windows.Forms.Button buttonScan;
        private System.Windows.Forms.Button buttonStop;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelFilter;
        private System.Windows.Forms.Label labelFilter;
        private System.Windows.Forms.TextBox textBoxFilter;
        internal System.Windows.Forms.CheckBox checkBoxIgnoreCase;
        private System.Windows.Forms.Label labelCount;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelActions;
        private System.Windows.Forms.Button buttonExportCsv;
        private System.Windows.Forms.Button buttonImportCsv;
        private System.Windows.Forms.Button buttonClearDatabase;
        private System.Windows.Forms.Button buttonSearchInstall;
        private Form1CatalogGrid gridCatalog;
        private System.Windows.Forms.Panel panelProgress;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label labelProgress;
        private System.Windows.Forms.Timer timerFilter;
        private System.Windows.Forms.SaveFileDialog saveFileDialogCsv;
        private System.Windows.Forms.OpenFileDialog openFileDialogCsv;
    }
}
