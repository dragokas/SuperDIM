using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DazUnpacker
{
    // Result of "Search & Install": found files whose archives exist / are missing on disk, and paths not found at all
    internal partial class Form1CatalogResult : Form
    {
        public Form1CatalogResult(string summary, IList<CatalogRow> onDisk, IList<CatalogRow> missing, IList<string> notFound)
        {
            InitializeComponent();

            labelSummary.Text = summary;
            labelOnDisk.Text = "Found, archive is on disk (" + onDisk.Count + "):";
            labelMissing.Text = "Found, archive is missing on disk (" + missing.Count + "):";
            labelNotFound.Text = "Not found (" + notFound.Count + "):";

            gridOnDisk.SetSource(new ListRowSource(onDisk));
            gridMissing.SetSource(new ListRowSource(missing));
            foreach (string path in notFound)
            {
                listBoxNotFound.Items.Add(path);
            }
        }

        // The right click selects the line under the cursor, unless it is a part of the selection already
        private void listBoxNotFound_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
            {
                return;
            }
            int index = listBoxNotFound.IndexFromPoint(e.Location);
            if (index != ListBox.NoMatches && !listBoxNotFound.GetSelected(index))
            {
                listBoxNotFound.ClearSelected();
                listBoxNotFound.SetSelected(index, true);
            }
        }

        private void contextMenuNotFound_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = listBoxNotFound.SelectedItems.Count == 0;
        }

        private void toolStripMenuItemCopy_Click(object sender, EventArgs e)
        {
            string[] lines = new string[listBoxNotFound.SelectedItems.Count];
            for (int i = 0; i < lines.Length; i++)
            {
                lines[i] = listBoxNotFound.SelectedItems[i].ToString();
            }
            if (lines.Length != 0)
            {
                Clipboard.SetText(string.Join(Environment.NewLine, lines));
            }
        }
    }
}
