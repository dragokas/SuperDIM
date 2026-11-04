using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DazUnpacker
{
    // Input window: a list of relative file paths to look for in the catalog
    internal partial class Form1CatalogSearch : Form
    {
        // Normalized paths (forward slashes, leading slash, no duplicates)
        internal List<string> Paths { get; private set; } = new List<string>();

        public Form1CatalogSearch(bool ignoreCase)
        {
            InitializeComponent();
            labelCase.Text = "Case: " + (ignoreCase ? "ignored" : "sensitive") + " (see the \"Ignore case\" option on the Catalog tab)";
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            HashSet<string> unique = new HashSet<string>(StringComparer.Ordinal);
            List<string> paths = new List<string>();

            foreach (string line in textBoxPaths.Lines)
            {
                if (line.Trim().Length == 0)
                {
                    continue;
                }
                string path = CatalogRow.NormalizePath(line);
                if (unique.Add(path))
                {
                    paths.Add(path);
                }
            }
            if (paths.Count == 0)
            {
                MessageBox.Show(this, "Enter at least one file path.", Globals.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            Paths = paths;
            DialogResult = DialogResult.OK;
        }
    }
}
