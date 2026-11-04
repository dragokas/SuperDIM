using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using DazUnpacker.Properties;
using static DazUnpacker.Unpacker;
using DazUnpacker.source;

namespace DazUnpacker
{
    public partial class Form1 : Form
    {
        private Stopwatch m_stopWatcher = new Stopwatch();
        private ListViewColumnSorter lvwColumnSorter;
        private TabPage _previousTab;
        private bool _isFormLoaded = false;
        private volatile bool m_installRunning = false;

        internal Form1Catalog CatalogForm { get; private set; }

        // The Installer tab exists only while at least one DAZ library is added
        internal bool IsInstallerAvailable
        {
            get { return tabControl.TabPages.Contains(tabPageInstaller); }
        }

        public Form1()
        {
            InitializeComponent();
            MinimumSize = Size; // the layout is designed for this size
            Globals.Init();
            Archive.Init();
            Library.Init(this);
            Content.Init(this);

            CatalogForm = new Form1Catalog(this);
            CatalogForm.Dock = DockStyle.Fill;
            tabPageCatalog.Controls.Add(CatalogForm);
            CatalogForm.Show();

            Config.Init(this);
            panelLanguage.Visible = false;
            UpdateTabs();

            RegisterDropHandler(tabPageInstaller);
        }

        private void RegisterDropHandler(Control ctl)
        {
            if (ctl.Controls.Count > 0)
            {
                foreach (Control c in ctl.Controls)
                {
                    RegisterDropHandler(c);
                }
            }
            ctl.AllowDrop = true;
            ctl.DragDrop += new DragEventHandler(Event_CommonDragDrop);
            ctl.DragEnter += new DragEventHandler(Event_CommonDragEnter);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            lvwColumnSorter = new ListViewColumnSorter();
            listViewSkippedContent.ListViewItemSorter = lvwColumnSorter;
            lvwColumnSorter.SortColumn = 1;
            listViewSkippedContent.Sort();
            _previousTab = tabControl.SelectedTab;
            _isFormLoaded = true;

            Log.Init(checkBoxDeleteLogOnStart.Checked);
            Log.SetControlForReport(listBoxLog);
            Log.SetLogNoiseLevel(Log.LOG_NOISE_LEVEL.Noisy);
            Log.SetFormMaxLines(500);
            Log.SetLogLocation(Log.LOG_LOCATION.LOG_LOCATION_FORM | Log.LOG_LOCATION.LOG_LOCATION_FILE);
            Log.DisableCache(true);
            Log.Info("Daz unpacker is started.");
            if (Config.LoadWarning != null)
            {
                Log.Warning(Config.LoadWarning);
            }
        }

        #region Settings

        private void radioButtonLangEnglish_CheckedChanged(object sender, EventArgs e)
        {
            Config.Storage.TabSettings.UseLanguageRu = !radioButtonLangEnglish.Checked;
        }
        private void radioButtonLangRussian_CheckedChanged(object sender, EventArgs e)
        {
            Config.Storage.TabSettings.UseLanguageRu = radioButtonLangRussian.Checked;
        }
        private void checkedListBoxLibraryDirs_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
            else e.Effect = DragDropEffects.None;
        }
        private void checkedListBoxLibraryDirs_MouseEnter(object sender, EventArgs e)
        {
            this.ActiveControl = checkedListBoxLibraryDirs;
        }
        private void checkedListBoxLibraryDirs_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (string file in files)
            {
                if (Library.Basic.Add(file))
                {
                    UpdateTabs();
                }
            }
        }
        private void buttonAddLibraryPath_Click(object sender, EventArgs e)
        {
            var dlg = new FolderPicker();
            string folder = FileSys.GetParentDir(Library.Basic.GetLibrary(0));
            dlg.InputPath = string.IsNullOrEmpty(folder) ? @"c:\" : folder;
            if (dlg.ShowDialog() == true)
            {
                if (Library.Basic.Add(dlg.ResultPath))
                {
                    UpdateTabs();
                }
            }
        }
        private void buttonRemoveLibraryPath_Click(object sender, EventArgs e)
        {
            for (int i = checkedListBoxLibraryDirs.Items.Count - 1; i >= 0; i--)
            {
                if (checkedListBoxLibraryDirs.GetItemChecked(i))
                {
                    Library.Basic.Remove(i);
                    UpdateTabs();
                }
            }
        }
        private void UpdateTabs()
        {
            if (Library.Basic.Count() == 0 && tabControl.TabPages.Contains(tabPageInstaller))
            {
                tabControl.TabPages.Remove(tabPageInstaller);
                tabControl.TabPages.Remove(tabPageLogs);
            }
            else if (!tabControl.TabPages.Contains(tabPageInstaller))
            {
                // Order of tabs: Settings, Installer, Catalog, Logs
                tabControl.TabPages.Insert(tabControl.TabPages.IndexOf(tabPageCatalog), tabPageInstaller);
                tabControl.TabPages.Add(tabPageLogs);
            }
        }

        // Selects the Installer tab (if it is available)
        internal void ShowInstallerTab()
        {
            if (IsInstallerAvailable)
            {
                tabControl.SelectedTab = tabPageInstaller;
            }
        }
        private void radioButtonTempSameAsInstall_CheckedChanged(object sender, EventArgs e)
        {
            Config.Storage.TabSettings.UseSpecificTempFolder = !radioButtonTempSameAsInstall.Checked;
        }
        private void radioButtonTempIsSpecific_CheckedChanged(object sender, EventArgs e)
        {
            textBoxTempFolder.Enabled = radioButtonTempIsSpecific.Checked;
            buttonSelectTempFolder.Enabled = radioButtonTempIsSpecific.Checked;
            Config.Storage.TabSettings.UseSpecificTempFolder = radioButtonTempIsSpecific.Checked;
        }
        private void buttonSelectTempFolder_Click(object sender, EventArgs e)
        {
            var dlg = new FolderPicker();
            string folder = FileSys.GetParentDir(textBoxTempFolder.Text);
            dlg.InputPath = string.IsNullOrEmpty(folder) ? @"c:\" : folder;
            if (dlg.ShowDialog() == true)
            {
                textBoxTempFolder.Text = dlg.ResultPath;
            }
        }
        private void textBoxTempFolder_TextChanged(object sender, EventArgs e)
        {
            Config.Storage.TabSettings.TempFolder = textBoxTempFolder.Text;
        }
        private void UpdateGenesisTypeFilter()
        {
            Config.Storage.TabSettings.genesisVersionFilter = Config.GetGenesisVersionFilter();
        }

        #endregion


        #region Installer

        private void checkBoxSubfolderPrefix_CheckedChanged(object sender, EventArgs e)
        {
            textBoxSubfolderPrefix.Enabled = checkBoxSubfolderPrefix.Checked;
        }

        private void radioButtonInstallLibs_MouseEnter(object sender, EventArgs e)
        {
            this.ActiveControl = radioButtonInstallLibs;
        }

        private void AcceptDropEvent(DragEventArgs e)
        {
            Config.SaveFiltersFromForm(); // the filters of the form (not the saved ones) are applied to the added archives
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            WithoutListRedraw(() =>
            {
                foreach (string file in files)
                {
                    Content.AddObject(file);
                }
            });
            Statistics.Total = Library.Content.Count;
            DetectAndShowContentType();
        }

        // Adds many archives without redrawing the list after each one
        private void WithoutListRedraw(Action action)
        {
            listViewContent.BeginUpdate();
            try
            {
                action();
            }
            finally
            {
                listViewContent.EndUpdate();
            }
        }

        private void DetectAndShowContentType()
        {
            comboBoxContentRootFolder.Items.Clear();
            comboBoxContentRootFolder.Items.AddRange(Content.GetParentFolders().ToArray());

            string file = Library.Content.GetFirstFile();
            string grandParentDir = FileSys.GetParentDir(FileSys.GetParentDir(file));
            comboBoxContentRootFolder.SelectedIndex = comboBoxContentRootFolder.FindStringExact(grandParentDir);

            ContentTypes contentType = Unpacker.DetectContentType(file);
            comboBoxContentType.SelectedIndex = comboBoxContentType.FindStringExact(contentType.ToString());
        }

        internal void AddFile(string path, bool dDetectContentType = true)
        {
            Content.AddFile(path);
            Statistics.Total = Library.Content.Count;
            if (dDetectContentType)
            {
                DetectAndShowContentType();
            }
        }

        private void AddFolder(string path, bool dDetectContentType = true)
        {
            Config.SaveFiltersFromForm();
            WithoutListRedraw(() => Content.AddFolder(path));
            Statistics.Total = Library.Content.Count;
            if (dDetectContentType)
            {
                DetectAndShowContentType();
            }
        }

        private void buttonAddContentFolder_Click(object sender, EventArgs e)
        {
            var dlg = new FolderPicker();
            string folder = Library.Content.GetFolder(0);
            dlg.InputPath = string.IsNullOrEmpty(folder) ? @"c:\" : folder;
            if (dlg.ShowDialog() == true)
            {
                AddFolder(dlg.ResultPath);
            }
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            try
            {
                Statistics.Done = 0;
                Library.Skipped.RemoveAll();
                Library.Content.PrepareStatuses();

                UpdateGenesisTypeFilter();
                Config.SaveFiltersFromForm();

                buttonStart.Enabled = false;
                progressBar1.Value = 0;
                m_stopWatcher.Restart();

                UnpackInfo unpackInfo = new UnpackInfo();

                unpackInfo.contentType = (ContentTypes)Enum.Parse(typeof(ContentTypes), comboBoxContentType.Text);
                unpackInfo.contentTypeDetectIndividually = checkBoxDetectContentTypePerFile.Checked;
                unpackInfo.suggestedVersion = (GenesisVersion)Enum.Parse(typeof(GenesisVersion), comboBoxFallbackGeneration.Text);
                unpackInfo.suggestedGender = (GenesisGenders)Enum.Parse(typeof(GenesisGenders), comboBoxFallbackGender.Text);
                unpackInfo.contentLibDir = Library.Install.GetPath();
                unpackInfo.userSelectedRootDir = comboBoxContentRootFolder.Text;
                unpackInfo.ignoreRootDir = !checkBoxHierarhyByRootFolder.Checked;
                unpackInfo.overwriteExisting = checkBoxOverwrite.Checked;
                unpackInfo.subfolderPrefix = "";
                if (checkBoxCreateRootSubfolder.Checked)
                {
                    unpackInfo.rootFolderName = textBoxRootSubfolder.Text;
                }
                if (checkBoxSubfolderPrefix.Checked)
                {
                    unpackInfo.subfolderPrefix = textBoxSubfolderPrefix.Text;
                }
                unpackInfo.contentFiles = FilterFilesList(Library.Content.GetFilesList());
                unpackInfo.threadsCount = (int)numericUpDownThreadsCount.Value;

                if (unpackInfo.contentFiles.Count > 0)
                {
                    Thread t = new Thread(() => RunInstall(unpackInfo));
                    t.IsBackground = true; // the work must not keep the process alive after the window is closed
                    m_installRunning = true;
                    t.Start();
                }
                else
                {
                    buttonStart.Enabled = true; // nothing to install
                }
            }
            catch (Exception ex)
            {
                buttonStart.Enabled = true;
                Globals.ReportError(ex);
            }
        }

        // Worker thread: the Install button is enabled again when the work is over, whatever its result
        private void RunInstall(UnpackInfo unpackInfo)
        {
            try
            {
                Unpacker.UnpackAll(unpackInfo);
            }
            catch (Exception ex)
            {
                Globals.ReportError(ex, false, "Install");
            }
            finally
            {
                if (!IsDisposed && IsHandleCreated)
                {
                    BeginInvoke(new MethodInvoker(OnInstallFinished));
                }
            }
        }

        private void OnInstallFinished()
        {
            m_installRunning = false;
            m_stopWatcher.Stop();
            buttonStart.Enabled = true;
        }

        private List<string> FilterFilesList(List<string> list)
        {
            if (!Config.Storage.TabInstaller.Filters.HasFilters())
            {
                return list;
            }

            List<string> filteredList = new List<string>();
            foreach (string file in list)
            {
                if (Config.Storage.TabInstaller.Filters.IsArchivePassed(file))
                {
                    filteredList.Add(file);
                }
                else
                {
                    Library.Skipped.Add(file, FindFailReason.CriteriaNotMatch);
                    Library.Content.SetStatus(file, Library.Content.GetStatusText(FindFailReason.CriteriaNotMatch));
                    Statistics.DoneIncrement();
                }
            }
            return filteredList;
        }

        #endregion


        #region Form

        

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (m_installRunning && e.CloseReason == CloseReason.UserClosing)
            {
                // the worker thread is a background one: closing the window stops the installation at once
                DialogResult answer = MessageBox.Show(this, "The installation is in progress. Stop it and exit?", Globals.AppName,
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (answer != DialogResult.Yes)
                {
                    e.Cancel = true;
                    return;
                }
            }
            CatalogForm.Shutdown();
            Config.SaveConfig();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Control c = Geometry.FindControlAtCursor(this);
            if (c != null)
            {
                if (Control.ReferenceEquals(c, listBoxLog))
                {
                    if (listBoxLog.SelectedIndex != -1)
                    {
                        Clipboard.SetText(listBoxLog.Text);
                    }
                }
                else if (Control.ReferenceEquals(c, listViewSkippedContent))
                {
                    if (listViewSkippedContent.SelectedItems.Count != 0)
                    {
                        Clipboard.SetText(listViewSkippedContent.SelectedItems[0].Text);
                    }
                }
            }
        }

        private void copyAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Control c = Geometry.FindControlAtCursor(this);
            if (c != null)
            {
                if (Control.ReferenceEquals(c, listBoxLog))
                {
                    if (listBoxLog.SelectedIndex != -1)
                    {
                        //Clipboard.SetText(listBoxLog.Text);
                    }
                }
                else if (Control.ReferenceEquals(c, listViewSkippedContent))
                {
                    StringBuilder all = new StringBuilder();
                    foreach(ListViewItem item in listViewSkippedContent.Items)
                    {
                        all.Append(String.Format("|{0}| {1}\n", item.SubItems[1].Text, item.Text));
                    }
                    Clipboard.SetText(all.ToString());
                }
            }
        }

        private void listBoxLog_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                int index = listBoxLog.IndexFromPoint(e.Location);
                if (index != ListBox.NoMatches)
                {
                    listBoxLog.SelectedIndex = index;
                    contextMenuCommon.Show(Cursor.Position);
                }
            }
        }

        private void checkBoxLogDetails_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxLogDetails.Checked)
            {
                textBoxLogDetails.Visible = true;
                listBoxLog.Visible = false;
                if (listBoxLog.SelectedIndex != -1)
                {
                    textBoxLogDetails.Text = "Details:\r\n\r\n" + listBoxLog.Text;
                }
            }
            else
            {
                textBoxLogDetails.Visible = false;
                listBoxLog.Visible = true;
            }
        }

        #endregion

        private void buttonOpenLog_Click(object sender, EventArgs e)
        {
            Globals.OpenTextFile(Log.LogPath);
        }

        private void labelProgress_TextChanged(object sender, EventArgs e)
        {
            try
            {
                TimeSpan elapsed = m_stopWatcher.Elapsed;
                labelTimeElapsed.Text = ((int)elapsed.TotalSeconds).ToString() + " s.";
                if (elapsed.TotalSeconds != 0)
                {
                    int countDone = Statistics.Done;
                    double speed = countDone / elapsed.TotalSeconds;
                    labelSpeed.Text = String.Format("{0:#.#} arc/s", speed); ;
                    if (Statistics.Total != 0)
                    {
                        int percent = countDone * 100 / Statistics.Total;
                        progressBar1.Value = percent;
                    }
                }
                if (Statistics.Done == Statistics.Total)
                {
                    m_stopWatcher.Stop();
                    progressBar1.Value = 100;
                }
            }
            catch { }
        }

        private void buttonDeleteLog_Click(object sender, EventArgs e)
        {
            Log.Clear();
        }

        private void listViewSkippedContent_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var item = listViewSkippedContent.GetItemAt(e.Location.X, e.Location.Y);
                if (item != null)
                {
                    item.Selected = true;
                    contextMenuCommon.Show(Cursor.Position);
                }
            }
        }

        private void listViewSkippedContent_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            SortOrder sortOrder;
            if (e.Column != lvwColumnSorter.SortColumn)
            {
                sortOrder = SortOrder.Ascending;
            }
            else
            {
                // invert
                sortOrder = lvwColumnSorter.Order == SortOrder.Ascending ?
                    SortOrder.Descending : SortOrder.Ascending;
            }
            ListView_SortColumn(e.Column, sortOrder == SortOrder.Ascending);
        }

        private void ListView_SortColumn(int columnIndex, bool isAscending)
        {
            if (columnIndex != lvwColumnSorter.SortColumn)
            {
                lvwColumnSorter.SortColumn = columnIndex;
            }
            lvwColumnSorter.Order = isAscending ? SortOrder.Ascending : SortOrder.Descending;
            listViewSkippedContent.Sort();
        }

        private void listViewSkippedContent_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listViewSkippedContent.SelectedItems.Count != 0)
            {
                string file = listViewSkippedContent.SelectedItems[0].Text;
                if (File.Exists(file))
                {
                    Globals.OpenTextFile(file);
                }
            }
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            ClearInstallList();
        }

        private void ClearInstallList()
        {
            Library.Content.RemoveAll();
            Library.Skipped.RemoveAll();
            Statistics.Reset();
        }

        // Replaces the install list with the given archives (those which exist on disk and pass the filters),
        // opens the Installer tab, returns the number of archives in the list
        internal int ReplaceInstallList(IEnumerable<string> archives)
        {
            ClearInstallList();
            Config.SaveFiltersFromForm();
            WithoutListRedraw(() =>
            {
                foreach (string path in archives)
                {
                    if (File.Exists(path))
                    {
                        AddFile(path, false);
                    }
                }
            });
            if (Library.Content.Count > 0)
            {
                DetectAndShowContentType();
            }
            ShowInstallerTab();
            return Library.Content.Count;
        }

        private void buttonReInstallAccessDenied_Click(object sender, EventArgs e)
        {
            Library.Content.RemoveAll();
            Statistics.Reset();

            List<string> paths = Library.Skipped.GetFiles(FindFailReason.AccessDenied);

            Config.SaveFiltersFromForm();
            foreach (var path in paths.Distinct())
            {
                AddFile(path);
            }

            Library.Skipped.RemoveAll(FindFailReason.AccessDenied);

            tabControl.SelectedTab = tabPageLogs;

            buttonStart.Enabled = true;
            buttonStart.PerformClick();
        }
        private void Event_CommonDragDrop(object sender, DragEventArgs e)
        {
            AcceptDropEvent(e);
        }

        private void Event_CommonDragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
            else e.Effect = DragDropEffects.None;
        }

        private void checkBoxDetectContentTypePerFile_CheckedChanged(object sender, EventArgs e)
        {
            comboBoxContentType.Enabled = !checkBoxDetectContentTypePerFile.Checked;
        }

        private void checkBoxInstallDufOnly_CheckedChanged(object sender, EventArgs e)
        {
            Config.Storage.TabInstaller.InstallDufOnly = checkBoxInstallDufOnly.Checked;
        }

        private void checkBoxCreateRootSubfolder_CheckedChanged(object sender, EventArgs e)
        {
            textBoxRootSubfolder.Enabled = checkBoxCreateRootSubfolder.Checked;
        }

        private void checkBoxFilterByType_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxFilterByType.Checked)
            {
                checkBoxDetectContentTypePerFile.Checked = true;
            }
            comboBoxFilterContentTypes.Enabled = checkBoxFilterByType.Checked;
        }

        private void checkBoxHierarhyByRootFolder_CheckedChanged(object sender, EventArgs e)
        {
            comboBoxContentRootFolder.Enabled = checkBoxHierarhyByRootFolder.Checked;
        }

        private void tabControl_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (_isFormLoaded)
            {
                if (_previousTab == tabPageSettings)
                {
                    if (!CheckTempFolderForValidInput())
                    {
                        e.Cancel = true;
                        return;
                    }
                }
            }
            _previousTab = tabControl.SelectedTab;
        }

        private bool CheckTempFolderForValidInput()
        {
            if (radioButtonTempIsSpecific.Checked)
            {
                string dir = textBoxTempFolder.Text;
                if (String.IsNullOrEmpty(dir) || !Directory.Exists(dir))
                {
                    MessageBox.Show("Incorrect temporarily folder specified!", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    this.BeginInvoke((MethodInvoker)(() => textBoxTempFolder.Focus()));
                    return false;
                }
            }
            return true;
        }

        private void checkBoxDateFilterCreated_CheckedChanged(object sender, EventArgs e)
        {
            comboBoxDateFilterDirection.Enabled = checkBoxDateFilterCreated.Checked;
            dateTimePickerFilterCreated.Enabled = checkBoxDateFilterCreated.Checked;
        }

    }
}
