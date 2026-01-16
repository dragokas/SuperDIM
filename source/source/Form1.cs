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

        public Form1()
        {
            InitializeComponent();
            tabControl.TabPages.Remove(tabPageSort);
            Globals.Init();
            Archive.Init();
            Library.Init(this);
            Content.Init(this);
            Exclusion.Init(this);
            Config.Init(this);
            panelLanguage.Visible = false;
            UpdateGenesisTypeFilter();
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

            foreach (string name in Enum.GetNames(typeof(ContentType)))
            {
                comboBoxContentType.Items.Add(name);
            }
            comboBoxContentType.SelectedIndex = 0;

            foreach (string name in Enum.GetNames(typeof(GenesisGender)))
            {
                comboBoxGender.Items.Add(name);
            }
            comboBoxGender.SelectedIndex = 0;
            comboBoxGender.SelectedIndex = comboBoxGender.FindStringExact(GenesisGender.Female.ToString());

            foreach (string name in Enum.GetNames(typeof(GenesisVersion)))
            {
                comboBoxGeneration.Items.Add(name);
            }
            comboBoxGeneration.SelectedIndex = 2;

            Log.Init(checkBoxDeleteLogOnStart.Checked);
            Log.SetControlForReport(listBoxLog);
            Log.SetLogNoiseLevel(Log.LOG_NOISE_LEVEL.Noisy);
            Log.SetFormMaxLines(500);
            Log.SetLogLocation(Log.LOG_LOCATION.LOG_LOCATION_FORM | Log.LOG_LOCATION.LOG_LOCATION_FILE);
            Log.DisableCache(true);
            Log.Info("Daz unpacker is started.");
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
                tabControl.TabPages.Add(tabPageInstaller);
                tabControl.TabPages.Add(tabPageLogs);
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
        private void checkBoxGenesis3_CheckedChanged(object sender, EventArgs e)
        {
            UpdateGenesisTypeFilter();
        }
        private void checkBoxGenesis8_CheckedChanged(object sender, EventArgs e)
        {
            UpdateGenesisTypeFilter();
        }
        private void checkBoxGenesis9_CheckedChanged(object sender, EventArgs e)
        {
            UpdateGenesisTypeFilter();
        }
        private void UpdateGenesisTypeFilter()
        {
            Config.Storage.TabSettings.genesisVersionFilter = Config.GetGenesisVersionFilter();
        }

        #endregion


        #region Sort
        private void checkBoxUseSubfolder_CheckedChanged(object sender, EventArgs e)
        {
            panelSubfolder.Enabled = checkBoxUseSubfolder.Checked;
            Config.Storage.TabSort.UseSubfolder = checkBoxUseSubfolder.Checked;
        }
        private void radioButtonSpecificSubfolder_CheckedChanged_1(object sender, EventArgs e)
        {
            textBoxSubfolderForced.Enabled = radioButtonSpecificSubfolder.Checked;
            Config.Storage.TabSort.UseForcedSubfolder = radioButtonSpecificSubfolder.Checked;
        }
        private void textBoxSubfolderForced_TextChanged(object sender, EventArgs e)
        {
            Config.Storage.TabSort.ForcedSubfolderName = textBoxSubfolderForced.Text;
        }
        private void radioButtonAutoDetectSubfolder_CheckedChanged(object sender, EventArgs e)
        {
            panelSubfolderAutoDetect.Enabled = radioButtonAutoDetectSubfolder.Checked;
            Config.Storage.TabSort.UseForcedSubfolder = !radioButtonAutoDetectSubfolder.Checked;
        }
        private void checkBoxSearchSubfolder_CheckedChanged(object sender, EventArgs e)
        {
            checkedListBoxSearchLibs.Enabled = checkBoxSearchSubfolder.Checked;
            Config.Storage.TabSort.UseSubfolderOldLibrary = checkBoxSearchSubfolder.Checked;
        }
        private void checkedListBoxSearchLibraries_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void checkedListBoxSearchLibraries_MouseEnter(object sender, EventArgs e)
        {
            this.ActiveControl = checkedListBoxSearchLibs;
        }
        private void radioButtonPrioritySubfolderOldLibrary_CheckedChanged(object sender, EventArgs e)
        {
            Config.Storage.TabSort.UsePriorityZipLocation = !radioButtonPrioritySubfolderOldLibrary.Checked;
        }
        private void radioButtonPrioritySubfolderZipLocation_CheckedChanged(object sender, EventArgs e)
        {
            Config.Storage.TabSort.UsePriorityZipLocation = radioButtonPrioritySubfolderZipLocation.Checked;
        }
        private void checkBoxSubfolderAsZipLocation_CheckedChanged(object sender, EventArgs e)
        {
            Config.Storage.TabSort.UseSubfolderZipLocation = checkBoxSubfolderAsZipLocation.Checked;
        }
        private void checkedListBoxSubfolderExclude_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void checkedListBoxSubfolderExclude_MouseEnter(object sender, EventArgs e)
        {
            this.ActiveControl = checkedListBoxSubfolderBlock;
        }
        private void checkBoxSubfolderPrefix_CheckedChanged(object sender, EventArgs e)
        {
            textBoxSubfolderPrefix.Enabled = checkBoxSubfolderPrefix.Checked;
            Config.Storage.TabSort.UsePrefix = checkBoxSubfolderPrefix.Checked;
        }
        private void textBoxSubfolderPrefix_TextChanged(object sender, EventArgs e)
        {
            Config.Storage.TabSort.PrefixName = textBoxSubfolderPrefix.Text;
        }
        #endregion


        #region Installer

        private void radioButtonInstallLibs_MouseEnter(object sender, EventArgs e)
        {
            this.ActiveControl = radioButtonInstallLibs;
        }

        private void AcceptDropEvent(DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (string file in files)
            {
                Content.AddObject(file);
            }
            Statistics.Total = Library.Content.Count;
            DetectAndShowContentType();
        }

        private void DetectAndShowContentType()
        {
            comboBoxContentRootFolder.Items.Clear();
            comboBoxContentRootFolder.Items.AddRange(Content.GetParentFolders().ToArray());

            string file = Library.Content.GetFirstFile();
            string grandParentDir = FileSys.GetParentDir(FileSys.GetParentDir(file));
            comboBoxContentRootFolder.SelectedIndex = comboBoxContentRootFolder.FindStringExact(grandParentDir);

            ContentType contentType = Unpacker.DetectContentType(file);
            comboBoxContentType.SelectedIndex = comboBoxContentType.FindStringExact(contentType.ToString());
        }

        private void AddFile(string path, bool dDetectContentType = true)
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
            Content.AddFolder(path);
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

                buttonStart.Enabled = false;
                progressBar1.Value = 0;
                m_stopWatcher.Restart();

                UnpackInfo unpackInfo = new UnpackInfo();

                unpackInfo.contentType = (ContentType)Enum.Parse(typeof(ContentType), comboBoxContentType.Text);
                unpackInfo.contentTypeDetectIndividually = checkBoxDetectContentTypePerFile.Checked;
                unpackInfo.suggestedVersion = (GenesisVersion)Enum.Parse(typeof(GenesisVersion), comboBoxGeneration.Text);
                unpackInfo.suggestedGender = (GenesisGender)Enum.Parse(typeof(GenesisGender), comboBoxGender.Text);
                unpackInfo.contentLibDir = Library.Install.GetPath();
                unpackInfo.userSelectedRootDir = comboBoxContentRootFolder.Text;
                unpackInfo.ignoreRootDir = checkBoxIgnoreRootFolder.Checked;
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
                    Thread t = new Thread(() =>
                        Unpacker.UnpackAll(unpackInfo)
                    );
                    t.Start();
                }
            }
            catch (Exception ex)
            {
                Globals.ReportError(ex);
            }
        }

        private List<string> FilterFilesList(List<string> list)
        {
            if (!FilterManager.HasFilters())
            {
                return list;
            }

            List<string> filteredList = new List<string>();
            foreach (string file in list)
            {
                if (FilterManager.IsPassed(new FileInfo(file).CreationTime))
                {
                    filteredList.Add(file);
                }
                else
                {
                    Library.Skipped.Add(file, FindFailReason.CriteriaNotMatch);
                    Statistics.DoneIncrement();
                }
            }
            return filteredList;
        }

        #endregion


        #region Form

        

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
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
                labelTimeElapsed.Text = elapsed.TotalSeconds.ToString() + " s.";
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
                    buttonStart.Enabled = true;
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
            Library.Content.RemoveAll();
            Library.Skipped.RemoveAll();
            Statistics.Reset();
        }

        private void buttonReInstallAccessDenied_Click(object sender, EventArgs e)
        {
            Library.Content.RemoveAll();
            Statistics.Reset();

            List<string> paths = Library.Skipped.GetFiles(FindFailReason.AccessDenied);

            foreach (var path in paths.Distinct())
            {
                AddFile(path);
            }

            Library.Skipped.RemoveAll(FindFailReason.AccessDenied);

            tabControl.SelectedIndex = 2;

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

        private void RefreshFilters()
        {
            FilterManager.ClearFilters();

            if (checkBoxDateFilterCreated.Checked)
            {
                DateFilterDirection direction = (DateFilterDirection)Enum.Parse(typeof(DateFilterDirection), comboBoxDateFilter.Text);
                FilterManager.AddDateFilter(DateFilterAction.Created, direction, dateTimePickerFilterCreated.Value);
            }
        }

        private void checkBoxDateFilterCreated_CheckedChanged(object sender, EventArgs e)
        {
            RefreshFilters();
        }

        private void comboBoxDateFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshFilters();
        }

        private void dateTimePickerFilterCreated_ValueChanged(object sender, EventArgs e)
        {
            RefreshFilters();
        }

        private void checkBoxDetectContentTypePerFile_CheckedChanged(object sender, EventArgs e)
        {
            comboBoxContentType.Enabled = !checkBoxDetectContentTypePerFile.Checked;
        }

        private void checkBoxInstallDufOnly_CheckedChanged(object sender, EventArgs e)
        {
            Config.Storage.TabInstaller.InstallDufOnly = checkBoxInstallDufOnly.Checked;
        }

        private void checkBoxIgnoreRootFolder_CheckedChanged(object sender, EventArgs e)
        {
            comboBoxContentRootFolder.Enabled = !checkBoxIgnoreRootFolder.Checked;
        }

        private void checkBoxCreateRootSubfolder_CheckedChanged(object sender, EventArgs e)
        {
            textBoxRootSubfolder.Enabled = checkBoxCreateRootSubfolder.Checked;
        }
    }
}
