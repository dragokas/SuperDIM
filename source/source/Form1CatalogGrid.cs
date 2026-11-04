using System;
using System.Windows.Forms;

namespace DazUnpacker
{
    // Virtual grid with the catalog columns: cell selection, Copy menu, sorting by a column header click
    internal partial class Form1CatalogGrid : UserControl
    {
        private ICatalogRowSource source;
        private int sortColumnIndex = -1;
        private bool sortAscending = true;

        public Form1CatalogGrid()
        {
            InitializeComponent();
        }

        internal void SetSource(ICatalogRowSource newSource)
        {
            if (source != null)
            {
                source.Changed -= Source_Changed;
            }
            source = newSource;
            source.Changed += Source_Changed;
            ReloadRows();
        }

        // Reads the row count again and repaints the cells
        internal void ReloadRows()
        {
            dataGridView.RowCount = 0; // drop the selection and the scroll position of the previous content
            dataGridView.RowCount = source != null ? source.Count : 0;
            dataGridView.Invalidate();
        }

        private void Source_Changed(object sender, EventArgs e)
        {
            ReloadRows();
        }

        private void dataGridView_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            if (source != null && e.RowIndex < source.Count)
            {
                e.Value = source.GetRow(e.RowIndex).GetValue((CatalogColumn)e.ColumnIndex);
            }
        }

        private void dataGridView_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (source == null || e.Button != MouseButtons.Left)
            {
                return;
            }
            sortAscending = (e.ColumnIndex != sortColumnIndex) || !sortAscending;

            if (sortColumnIndex >= 0)
            {
                dataGridView.Columns[sortColumnIndex].HeaderCell.SortGlyphDirection = SortOrder.None;
            }
            sortColumnIndex = e.ColumnIndex;
            dataGridView.Columns[sortColumnIndex].HeaderCell.SortGlyphDirection = sortAscending ? SortOrder.Ascending : SortOrder.Descending;
            source.Sort((CatalogColumn)sortColumnIndex, sortAscending);
        }

        // The right click selects the cell under the cursor, unless it is a part of the selection already
        private void dataGridView_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right || e.RowIndex < 0 || e.ColumnIndex < 0)
            {
                return;
            }
            DataGridViewCell cell = dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex];
            if (!cell.Selected)
            {
                dataGridView.ClearSelection();
                cell.Selected = true;
                dataGridView.CurrentCell = cell;
            }
        }

        private void contextMenuGrid_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = dataGridView.SelectedCells.Count == 0;
        }

        private void toolStripMenuItemCopy_Click(object sender, EventArgs e)
        {
            DataObject data = dataGridView.GetClipboardContent();
            if (data != null)
            {
                Clipboard.SetDataObject(data);
            }
        }
    }
}
