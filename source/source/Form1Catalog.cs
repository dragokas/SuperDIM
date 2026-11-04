using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DazUnpacker
{
    // "Catalog" tab: the cache of file lists of archives (SQLite). The form is embedded into the tab page.
    internal partial class Form1Catalog : Form
    {
        private readonly Form1 owner;
        private readonly DbRowSource rows;
        private CancellationTokenSource workCts;
        private bool dbReady;
        private bool busy;

        public Form1Catalog(Form1 owner)
        {
            this.owner = owner;
            InitializeComponent();
            TopLevel = false;

            rows = new DbRowSource(this);
            rows.Loading += Rows_Loading;
            rows.Changed += Rows_Changed;
            gridCatalog.SetSource(rows);

            try
            {
                CatalogDb.Init(Path.Combine(Globals.AppDir, "catalog.db"));
                dbReady = true;
            }
            catch (Exception ex)
            {
                Globals.ReportError(ex, true, "Cannot open the catalog database");
                tableLayoutPanel.Enabled = false;
            }
        }

        #region Settings

        internal void LoadSettings(_TabCatalog settings)
        {
            textBoxFolder.Text = settings.ScanFolder;
            numericUpDownThreads.Value = Math.Max(numericUpDownThreads.Minimum, Math.Min(numericUpDownThreads.Maximum, settings.ThreadsCount));
            checkBoxIgnoreCase.Checked = settings.IgnoreCase;
            checkBoxUniqueArchive.Checked = settings.UniqueArchive;
        }

        internal void SaveSettings(_TabCatalog settings)
        {
            settings.ScanFolder = textBoxFolder.Text;
            settings.ThreadsCount = (int)numericUpDownThreads.Value;
            settings.IgnoreCase = checkBoxIgnoreCase.Checked;
            settings.UniqueArchive = checkBoxUniqueArchive.Checked;
        }

        // Stops the running work (the application is closing)
        internal void Shutdown()
        {
            if (workCts != null)
            {
                workCts.Cancel();
            }
        }

        #endregion

        #region Grid and filter

        private void Form1Catalog_Load(object sender, EventArgs e)
        {
            if (dbReady)
            {
                ReloadGrid();
            }
        }

        private void ReloadGrid()
        {
            rows.Reload(textBoxFilter.Text, checkBoxIgnoreCase.Checked, checkBoxUniqueArchive.Checked);
        }

        // The search starts one second after the last change of the filter
        private void FilterChanged(object sender, EventArgs e)
        {
            timerFilter.Stop();
            timerFilter.Start();
        }

        private void timerFilter_Tick(object sender, EventArgs e)
        {
            timerFilter.Stop();
            if (dbReady)
            {
                ReloadGrid();
            }
        }

        private void Rows_Loading(bool loading)
        {
            UseWaitCursor = loading;
            if (loading)
            {
                labelCount.Text = "Loading ...";
            }
        }

        private void Rows_Changed(object sender, EventArgs e)
        {
            labelCount.Text = rows.Count.ToString("N0") + " row(s)";
        }

        #endregion

        #region Background work

        // Disables the controls while a long operation is running
        private void SetBusy(bool isBusy)
        {
            busy = isBusy;
            buttonScan.Enabled = !isBusy;
            buttonSelectFolder.Enabled = !isBusy;
            textBoxFolder.Enabled = !isBusy;
            numericUpDownThreads.Enabled = !isBusy;
            buttonExportCsv.Enabled = !isBusy;
            buttonImportCsv.Enabled = !isBusy;
            buttonClearDatabase.Enabled = !isBusy;
            buttonSearchInstall.Enabled = !isBusy;
            buttonStop.Enabled = isBusy;
        }

        // Runs 'work' on a pool thread. 'work' returns the final status text.
        // 'onSuccess' runs on the UI thread after a successful completion.
        private void StartWork(Func<CancellationToken, string> work, Action onSuccess)
        {
            if (!dbReady || busy)
            {
                return;
            }
            SetBusy(true);
            SetProgress(0, 0);
            CancellationTokenSource cts = new CancellationTokenSource();
            workCts = cts;

            Task.Run(() =>
            {
                string status;
                bool success = false;
                try
                {
                    status = work(cts.Token);
                    success = true;
                }
                catch (OperationCanceledException)
                {
                    status = "Cancelled";
                }
                catch (Exception ex)
                {
                    Globals.ReportError(ex, false, "Catalog");
                    status = "Error: " + ex.Message;
                }
                PostToUi(() => FinishWork(status, success ? onSuccess : null));
            });
        }

        private void FinishWork(string status, Action onSuccess)
        {
            SetBusy(false);
            labelProgress.Text = status;
            ReloadGrid();
            if (onSuccess != null)
            {
                onSuccess();
            }
        }

        private void PostToUi(Action action)
        {
            if (!IsDisposed && IsHandleCreated)
            {
                BeginInvoke(action);
            }
        }

        private void SetProgress(int done, int total)
        {
            progressBar.Maximum = Math.Max(total, 1);
            progressBar.Value = Math.Min(done, progressBar.Maximum);
            labelProgress.Text = done + " / " + total;
        }

        // May be called from any thread
        private void ReportProgress(int done, int total)
        {
            PostToUi(() => SetProgress(done, total));
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            if (workCts != null)
            {
                workCts.Cancel();
            }
        }

        #endregion

        #region Scan

        private void buttonSelectFolder_Click(object sender, EventArgs e)
        {
            FolderPicker dlg = new FolderPicker();
            dlg.InputPath = Directory.Exists(textBoxFolder.Text) ? textBoxFolder.Text : @"c:\";
            if (dlg.ShowDialog(Handle) == true)
            {
                textBoxFolder.Text = dlg.ResultPath;
            }
        }

        private void buttonScan_Click(object sender, EventArgs e)
        {
            string folder = textBoxFolder.Text.Trim();
            if (!Directory.Exists(folder))
            {
                MessageBox.Show(this, "Select an existing folder with archives.", Globals.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            int threads = (int)numericUpDownThreads.Value;

            StartWork(token =>
            {
                CatalogScanner scanner = new CatalogScanner(threads, ReportProgress);
                int total = scanner.Run(folder, token);
                return "Scanned archives: " + total;
            }, null);
        }

        #endregion

        #region CSV and database

        private void buttonExportCsv_Click(object sender, EventArgs e)
        {
            if (saveFileDialogCsv.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }
            string path = saveFileDialogCsv.FileName;

            StartWork(token =>
            {
                int total = (int)Math.Min(CatalogDb.Count(), int.MaxValue);
                CsvHelper.Export(path, CatalogDb.EnumerateAll(), written => ReportProgress((int)Math.Min(written, int.MaxValue), total), token);
                return "Exported rows: " + total;
            }, null);
        }

        private void buttonImportCsv_Click(object sender, EventArgs e)
        {
            if (openFileDialogCsv.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }
            string path = openFileDialogCsv.FileName;

            StartWork(token =>
            {
                int count = CatalogDb.ImportRows(CsvHelper.Import(path, percent => ReportProgress(percent, 100)), token);
                return "Imported rows: " + count;
            }, null);
        }

        private void buttonClearDatabase_Click(object sender, EventArgs e)
        {
            DialogResult answer = MessageBox.Show(this, "Delete everything from the catalog database?", Globals.AppName,
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            if (answer != DialogResult.Yes)
            {
                return;
            }
            StartWork(token =>
            {
                CatalogDb.Clear();
                return "Database cleared";
            }, null);
        }

        #endregion

        #region Search and install

        private void buttonSearchInstall_Click(object sender, EventArgs e)
        {
            if (!owner.IsInstallerAvailable)
            {
                MessageBox.Show(this, "Add at least one DAZ library on the Settings tab first.", Globals.AppName,
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            bool ignoreCase = checkBoxIgnoreCase.Checked;
            List<string> paths;
            using (Form1CatalogSearch dlg = new Form1CatalogSearch(ignoreCase))
            {
                if (dlg.ShowDialog(this) != DialogResult.OK)
                {
                    return;
                }
                paths = dlg.Paths;
            }

            List<CatalogTailMatch> matches = null;
            StartWork(token =>
            {
                matches = CatalogDb.FindByTails(paths, ignoreCase, token);
                return "Search completed";
            }, () => ShowSearchResult(matches));
        }

        // Puts the found archives which exist on disk to the install list and shows the report
        private void ShowSearchResult(List<CatalogTailMatch> matches)
        {
            List<CatalogRow> onDisk = new List<CatalogRow>();
            List<CatalogRow> missing = new List<CatalogRow>();
            List<string> notFound = new List<string>();
            List<string> archivesToInstall = new List<string>();
            HashSet<string> seenRows = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            HashSet<string> seenArchives = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            int foundPaths = 0;

            foreach (CatalogTailMatch match in matches)
            {
                if (match.Rows.Count == 0)
                {
                    notFound.Add(match.Tail);
                    continue;
                }
                foundPaths++;
                foreach (CatalogRow row in match.Rows)
                {
                    if (!seenRows.Add(row.ArchivePath + "|" + row.FilePath))
                    {
                        continue;
                    }
                    if (File.Exists(row.ArchivePath))
                    {
                        onDisk.Add(row);
                        if (seenArchives.Add(row.ArchivePath))
                        {
                            archivesToInstall.Add(row.ArchivePath);
                        }
                    }
                    else
                    {
                        missing.Add(row);
                    }
                }
            }

            string summary = "Found " + foundPaths + " of " + matches.Count + " file path(s) in the catalog.";
            if (archivesToInstall.Count > 0)
            {
                int added = owner.ReplaceInstallList(archivesToInstall);
                summary += "  Archives added to the install list: " + added + " of " + archivesToInstall.Count + ".";
            }
            using (Form1CatalogResult dlg = new Form1CatalogResult(summary, onDisk, missing, notFound))
            {
                dlg.ShowDialog(owner);
            }
        }

        #endregion
    }
}
