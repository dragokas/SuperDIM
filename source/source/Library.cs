using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DazUnpacker
{
    public static class Library
    {
        private delegate void ListViewInvoke(ListView listView, string value, string reason);

        static Form1 form;
        public static void Init(Form1 frm)
        {
            form = frm;
        }

        public static class Content
        {
            public static int Count
            {
                get
                {
                    return form.listViewContent.Items.Count;
                }
            }

            public static void RemoveAll()
            {
                form.listViewContent.Items.Clear();
            }

            public const string StatusSuccess = "Success";
            public const string StatusFail = "FAIL";
            private const int StatusColumnIndex = 2;

            // Items of listViewContent by archive path; built (and used) in the UI thread only
            private static Dictionary<string, List<ListViewItem>> itemsByPath = new Dictionary<string, List<ListViewItem>>();

            public static int Add(string filename, string folder, string note, bool isChecked)
            {
                var item = form.listViewContent.Items.Add(new ListViewItem(new string[] {
                filename, folder, "", note }));
                if (isChecked)
                {
                    item.Checked = true;
                }
                return item.Index;
            }

            // Text of the Status column for an archive which is not installed
            public static string GetStatusText(FindFailReason reason)
            {
                if (reason == FindFailReason.ArchiverError || reason == FindFailReason.Unknown)
                {
                    return StatusFail;
                }
                return reason.ToString();
            }

            // Call in the UI thread before the installation: clears the Status column and indexes the items by path
            public static void PrepareStatuses()
            {
                itemsByPath = new Dictionary<string, List<ListViewItem>>(StringComparer.OrdinalIgnoreCase);
                foreach (ListViewItem item in form.listViewContent.Items)
                {
                    item.SubItems[StatusColumnIndex].Text = "";
                    string path = Path.Combine(item.SubItems[1].Text, item.Text);
                    List<ListViewItem> items;
                    if (!itemsByPath.TryGetValue(path, out items))
                    {
                        items = new List<ListViewItem>();
                        itemsByPath.Add(path, items);
                    }
                    items.Add(item);
                }
            }

            // May be called from any thread
            public static void SetStatus(string path, string status)
            {
                if (form.listViewContent.InvokeRequired)
                {
                    form.listViewContent.BeginInvoke(new Action(() => SetStatus(path, status)));
                    return;
                }
                List<ListViewItem> items;
                if (itemsByPath.TryGetValue(path, out items))
                {
                    foreach (ListViewItem item in items)
                    {
                        item.SubItems[StatusColumnIndex].Text = status;
                    }
                }
            }

            public static void UpdateNote(int index, string note)
            {
                form.listViewContent.Items[index].SubItems[3].Text = note;
            }

            public static IEnumerable<string> GetFiles()
            {
                foreach (ListViewItem item in form.listViewContent.Items)
                {
                    yield return Path.Combine(item.SubItems[1].Text, item.Text);
                }
            }

            public static List<string> GetFilesList()
            {
                List<string> list = new List<string>();
                foreach (ListViewItem item in form.listViewContent.Items)
                {
                    list.Add(Path.Combine(item.SubItems[1].Text, item.Text));
                }
                return list;
            }

            public static string GetFirstFile()
            {
                if (form.listViewContent.Items.Count > 0)
                {
                    var item = form.listViewContent.Items[0];
                    return Path.Combine(item.SubItems[1].Text, item.Text);
                }
                return String.Empty;
            }

            public static string GetFolder(int index)
            {
                if (form.listViewContent.Items.Count <= index)
                {
                    return string.Empty;
                }
                return form.listViewContent.Items[index].SubItems[1].Text;
            }
        }

        public static class Basic
        {
            public static Dictionary<string, string> DirToNameMap = new Dictionary<string, string>();

            public static string GetPathByShortName(string shortName)
            {
                if (DirToNameMap.ContainsKey(shortName))
                {
                    return DirToNameMap[shortName];
                }
                return String.Empty;
            }

            public static bool Add(string path)
            {
                if (Directory.Exists(path))
                {
                    if (!form.checkedListBoxLibraryDirs.Items.Contains(path))
                    {
                        string shortName = Path.GetFileName(path);
                        form.checkedListBoxLibraryDirs.Items.Add(path, false);
                        form.radioButtonInstallLibs.Items.Add(shortName);

                        if (!DirToNameMap.ContainsKey(shortName))
                        {
                            DirToNameMap.Add(shortName, path);
                        }
                    }
                    return true;
                }
                return false;
            }

            public static void Remove(int index)
            {
                if (index != -1)
                {
                    form.checkedListBoxLibraryDirs.Items.RemoveAt(index);
                    form.radioButtonInstallLibs.Items.RemoveAt(index);
                }
            }
            public static IEnumerable<string> GetLibraries()
            {
                foreach (string item in form.checkedListBoxLibraryDirs.Items)
                {
                    yield return item;
                }
            }
            public static int Count()
            {
                return form.checkedListBoxLibraryDirs.Items.Count;
            }

            public static string GetLibrary(int index)
            {
                if (index < form.checkedListBoxLibraryDirs.Items.Count)
                {
                    return form.checkedListBoxLibraryDirs.Items[index].ToString();
                }
                return string.Empty;
            }
        }

        public static class Install
        {
            public static int GetIndex()
            {
                return form.radioButtonInstallLibs.SelectedIndex;
            }
            public static void SetIndex(int index)
            {
                if (index != -1 && form.radioButtonInstallLibs.Items.Count >= (index + 1))
                {
                    form.radioButtonInstallLibs.SelectedIndex = index;
                }
            }
            public static string GetPath()
            {
                int index = GetIndex();
                if (index != -1)
                {
                    return Basic.GetPathByShortName(form.radioButtonInstallLibs.Items[index].ToString());
                }
                return "";
            }

        }

        public static class Skipped
        {
            public static void Add(string zipPath, FindFailReason reason = FindFailReason.Unknown)
            {
                AddSkippedTextFramed(form.listViewSkippedContent, zipPath, reason.ToString());
            }

            public static List<string>GetFiles(FindFailReason reason)
            {
                List<string> list = new List<string>();

                foreach (ListViewItem item in form.listViewSkippedContent.Items)
                {
                    FindFailReason itemReason = (FindFailReason)Enum.Parse(typeof(FindFailReason), item.SubItems[1].Text);
                    if (reason == itemReason)
                    {
                        list.Add(item.Text);
                    }
                }
                return list;
            }

            public static void RemoveAll()
            {
                form.listViewSkippedContent.Items.Clear();
            }

            public static void RemoveAll(FindFailReason reason)
            {
                for (int i = Count - 1; i >= 0; i--)
                {
                    ListViewItem item = form.listViewSkippedContent.Items[i];
                    FindFailReason itemReason = (FindFailReason)Enum.Parse(typeof(FindFailReason), item.SubItems[1].Text);
                    if (reason == itemReason)
                    {
                        form.listViewSkippedContent.Items.RemoveAt(i);
                    }
                }
            }

            public static int Count
            {
                get
                {
                    return form.listViewSkippedContent.Items.Count;
                }
            }

            private static void AddSkippedTextFramed(ListView listView, string value, string reason = "")
            {
                if (listView.InvokeRequired)
                {
                    var invoke = new ListViewInvoke(SetListViewText);
                    listView.BeginInvoke(invoke, listView, value, reason);
                }
                else
                    SetListViewText(listView, value, reason);
            }

            private static void SetListViewText(ListView listView, string value, string reason)
            {
                listView.Items.Add(new ListViewItem(new[] { value, reason }));
            }
        }

        

    }
}
