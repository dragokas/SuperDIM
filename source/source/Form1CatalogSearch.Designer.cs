namespace DazUnpacker
{
    partial class Form1CatalogSearch
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
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.labelHint = new System.Windows.Forms.Label();
            this.textBoxPaths = new System.Windows.Forms.TextBox();
            this.labelCase = new System.Windows.Forms.Label();
            this.flowLayoutPanelButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonSearch = new System.Windows.Forms.Button();
            this.tableLayoutPanel.SuspendLayout();
            this.flowLayoutPanelButtons.SuspendLayout();
            this.SuspendLayout();
            //
            // tableLayoutPanel
            //
            this.tableLayoutPanel.ColumnCount = 1;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Controls.Add(this.labelHint, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.textBoxPaths, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.labelCase, 0, 2);
            this.tableLayoutPanel.Controls.Add(this.flowLayoutPanelButtons, 0, 3);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.Padding = new System.Windows.Forms.Padding(8);
            this.tableLayoutPanel.RowCount = 4;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.Size = new System.Drawing.Size(760, 480);
            this.tableLayoutPanel.TabIndex = 0;
            //
            // labelHint
            //
            this.labelHint.AutoSize = true;
            this.labelHint.Location = new System.Drawing.Point(11, 8);
            this.labelHint.Name = "labelHint";
            this.labelHint.Padding = new System.Windows.Forms.Padding(0, 0, 0, 6);
            this.labelHint.Size = new System.Drawing.Size(500, 19);
            this.labelHint.TabIndex = 0;
            this.labelHint.Text = "One relative path per line, e.g. /Poses/Name/file.duf  (matched by the end of the path):";
            //
            // textBoxPaths
            //
            this.textBoxPaths.AcceptsReturn = true;
            this.textBoxPaths.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxPaths.Location = new System.Drawing.Point(11, 30);
            this.textBoxPaths.Multiline = true;
            this.textBoxPaths.Name = "textBoxPaths";
            this.textBoxPaths.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxPaths.Size = new System.Drawing.Size(738, 380);
            this.textBoxPaths.TabIndex = 1;
            this.textBoxPaths.WordWrap = false;
            //
            // labelCase
            //
            this.labelCase.AutoSize = true;
            this.labelCase.Location = new System.Drawing.Point(11, 413);
            this.labelCase.Name = "labelCase";
            this.labelCase.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.labelCase.Size = new System.Drawing.Size(100, 19);
            this.labelCase.TabIndex = 2;
            //
            // flowLayoutPanelButtons
            //
            this.flowLayoutPanelButtons.AutoSize = true;
            this.flowLayoutPanelButtons.Controls.Add(this.buttonCancel);
            this.flowLayoutPanelButtons.Controls.Add(this.buttonSearch);
            this.flowLayoutPanelButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanelButtons.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanelButtons.Location = new System.Drawing.Point(11, 435);
            this.flowLayoutPanelButtons.Name = "flowLayoutPanelButtons";
            this.flowLayoutPanelButtons.Size = new System.Drawing.Size(738, 34);
            this.flowLayoutPanelButtons.TabIndex = 3;
            //
            // buttonCancel
            //
            this.buttonCancel.AutoSize = true;
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(660, 3);
            this.buttonCancel.MinimumSize = new System.Drawing.Size(75, 28);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 28);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            //
            // buttonSearch
            //
            this.buttonSearch.AutoSize = true;
            this.buttonSearch.Location = new System.Drawing.Point(540, 3);
            this.buttonSearch.MinimumSize = new System.Drawing.Size(75, 28);
            this.buttonSearch.Name = "buttonSearch";
            this.buttonSearch.Size = new System.Drawing.Size(114, 28);
            this.buttonSearch.TabIndex = 0;
            this.buttonSearch.Text = "Search && Install";
            this.buttonSearch.UseVisualStyleBackColor = true;
            this.buttonSearch.Click += new System.EventHandler(this.buttonSearch_Click);
            //
            // Form1CatalogSearch
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(760, 480);
            this.Controls.Add(this.tableLayoutPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(500, 300);
            this.Name = "Form1CatalogSearch";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Search & Install by file paths";
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.flowLayoutPanelButtons.ResumeLayout(false);
            this.flowLayoutPanelButtons.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Label labelHint;
        private System.Windows.Forms.TextBox textBoxPaths;
        private System.Windows.Forms.Label labelCase;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelButtons;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonSearch;
    }
}
