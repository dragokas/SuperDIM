using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Collections;
using System.Windows.Forms;
using DazUnpacker.source;

namespace DazUnpacker
{
    public class _TabSettings
    {
        public List<string> BasicLibraries = new List<string>();
        public GenesisVersion genesisVersionFilter = GenesisVersion.Unknown;
        public bool UseSpecificTempFolder = false;
        public string TempFolder = "";
        public bool UseLanguageRu;
    }

    public class _TabSort
    {
        public List<string> BlockNames = new List<string>();
        public List<int> BlockNamesIdx = new List<int>();
        public bool UsePrefix = false;
        public string PrefixName = "";
        public bool UseForcedSubfolder = false;
        public string ForcedSubfolderName = "";
        public bool UseSubfolder;
        public bool UseSubfolderZipLocation;
        public bool UsePriorityZipLocation;
        public bool UseSubfolderOldLibrary;
        public string RootSubfolder;
        public bool CreateRootSubfolder;
    }

    public class _TabInstaller
    {
        public int IntallLibrary = -1;
        public List<int> GarbageLibraries = new List<int>();
        public bool InstallDufOnly = true;
        public bool OverwriteExisting = false;
        public bool DetectContentTypePerFile = true;
        public int ThreadsCount = 1;
    }
    public class _TabLog
    {
        public bool DeleteLogOnStart = false;
    }

    public class ConfigStorage
    {
        public _TabSettings TabSettings = new _TabSettings();
        public _TabSort TabSort = new _TabSort();
        public _TabInstaller TabInstaller = new _TabInstaller();
        public _TabLog TabLog = new _TabLog();
        public int TabNumber = 0;
    }

    public class ConfigData
    {
        public string UnpackerTool7z = "";
        public string UnpackerToolRar = "";
        public string TempFolder = "";
    }

    public static class Config
    {
        public static ConfigStorage Storage = new ConfigStorage();
        public static ConfigData Data = new ConfigData();

        private static Form1 form;
        private static string configPath = 
            Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) 
            + "\\config.txt";
        
        public static void Init(Form1 frm)
        {
            form = frm;
            LoadCommonData();
            if (LoadStorage())
            {
                ApplyStorage();
            }
        }

        private static void LoadCommonData()
        {
            Data.UnpackerTool7z = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) +
                @"\tools\7z\7za.exe";
            if (!File.Exists(Data.UnpackerTool7z))
            {
                MessageBox.Show("Failed to find unpacker: " + Data.UnpackerTool7z);
            }
            Data.UnpackerToolRar = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) +
                @"\tools\WinRAR\UnRAR.exe";
            if (!File.Exists(Data.UnpackerToolRar))
            {
                MessageBox.Show("Failed to find unpacker: " + Data.UnpackerToolRar);
            }
            Data.TempFolder = Environment.ExpandEnvironmentVariables("%Temp%\\Daz");
        }

        private static bool LoadStorage()
        {
            if (!File.Exists(configPath))
            {
                return false;
            }
            string str = File.ReadAllText(configPath);
            Storage = JsonConvert.DeserializeObject<ConfigStorage>(str);
            return true;
        }

        private static void ApplyStorage()
        {
            foreach (string item in Storage.TabSettings.BasicLibraries)
            {
                Library.Basic.Add(item);
            }

            // TODO:
            // Replace index by name
            
            Library.Install.SetIndex(Storage.TabInstaller.IntallLibrary);

            foreach (string item in Storage.TabSort.BlockNames)
            {
                Exclusion.Add(item);
            }
            Exclusion.UnSelectAll();
            foreach (int item in Storage.TabSort.BlockNamesIdx)
            {
                Exclusion.Select(item);
            }

            form.tabControl.SelectedIndex = Storage.TabNumber;

            // Tab Settings
            form.radioButtonLangRussian.Checked = Storage.TabSettings.UseLanguageRu;
            form.radioButtonTempIsSpecific.Checked = Storage.TabSettings.UseSpecificTempFolder;
            form.textBoxTempFolder.Text = Storage.TabSettings.TempFolder;
            form.checkBoxGenesis3.Checked = (Storage.TabSettings.genesisVersionFilter & GenesisVersion.Genesis_3) != 0;
            form.checkBoxGenesis8.Checked = (Storage.TabSettings.genesisVersionFilter & GenesisVersion.Genesis_8) != 0;
            form.checkBoxGenesis9.Checked = (Storage.TabSettings.genesisVersionFilter & GenesisVersion.Genesis_9) != 0;

            // Tab Sort
            form.checkBoxUseSubfolder.Checked = Storage.TabSort.UseSubfolder;
            form.radioButtonSpecificSubfolder.Checked = Storage.TabSort.UseForcedSubfolder;
            form.textBoxSubfolderForced.Text = Storage.TabSort.ForcedSubfolderName;
            form.checkBoxSubfolderAsZipLocation.Checked = Storage.TabSort.UseSubfolderZipLocation;
            form.radioButtonPrioritySubfolderZipLocation.Checked = Storage.TabSort.UsePriorityZipLocation;
            form.checkBoxSearchSubfolder.Checked = Storage.TabSort.UseSubfolderOldLibrary;
            form.checkBoxSubfolderPrefix.Checked = Storage.TabSort.UsePrefix;
            form.textBoxSubfolderPrefix.Text = Storage.TabSort.PrefixName;
            form.textBoxRootSubfolder.Text = Storage.TabSort.RootSubfolder;
            form.checkBoxCreateRootSubfolder.Checked = Storage.TabSort.CreateRootSubfolder;

            // Tab Installer
            form.checkBoxDetectContentTypePerFile.Checked = Storage.TabInstaller.DetectContentTypePerFile;
            form.checkBoxInstallDufOnly.Checked = Storage.TabInstaller.InstallDufOnly;
            form.numericUpDownThreadsCount.Value = Storage.TabInstaller.ThreadsCount;
            foreach (string name in Enum.GetNames(typeof(DateFilterDirection)))
            {
                form.comboBoxDateFilter.Items.Add(name);
            }
            form.comboBoxDateFilter.SelectedIndex = 0;
            form.dateTimePickerFilterCreated.Value = DateTime.Now;
            form.checkBoxOverwrite.Checked = Storage.TabInstaller.OverwriteExisting;

            // Tab Log
            form.checkBoxDeleteLogOnStart.Checked = Storage.TabLog.DeleteLogOnStart;
        }

        public static void SaveConfig()
        {
            Storage.TabNumber = form.tabControl.SelectedIndex;
            Storage.TabSettings.BasicLibraries.Clear();
            Storage.TabInstaller.GarbageLibraries.Clear();

            foreach (string item in Library.Basic.GetLibraries())
            {
                Storage.TabSettings.BasicLibraries.Add(item);
            }
            Storage.TabInstaller.IntallLibrary = Library.Install.GetIndex();
            foreach (string item in Exclusion.GetList())
            {
                Storage.TabSort.BlockNames.Add(item);
            }
            foreach (int item in Exclusion.GetIndeces())
            {
                Storage.TabSort.BlockNamesIdx.Add(item);
            }

            Storage.TabInstaller.DetectContentTypePerFile = form.checkBoxDetectContentTypePerFile.Checked;
            Storage.TabInstaller.OverwriteExisting = form.checkBoxOverwrite.Checked;
            Storage.TabInstaller.ThreadsCount = (int)form.numericUpDownThreadsCount.Value;

            Storage.TabSort.RootSubfolder = form.textBoxRootSubfolder.Text;
            Storage.TabSort.CreateRootSubfolder = form.checkBoxCreateRootSubfolder.Checked;
            Storage.TabLog.DeleteLogOnStart = form.checkBoxDeleteLogOnStart.Checked;

            string str = JsonConvert.SerializeObject(Storage, Formatting.Indented);
            File.WriteAllText(configPath, str);
        }

        public static GenesisVersion GetGenesisVersionFilter()
        {
            GenesisVersion version = GenesisVersion.Unknown;

            if (form.checkBoxGenesis9.Checked)
            {
                version |= GenesisVersion.Genesis_9;
            }
            if (form.checkBoxGenesis8.Checked)
            {
                version |= GenesisVersion.Genesis_8;
            }
            if (form.checkBoxGenesis3.Checked)
            {
                version |= GenesisVersion.Genesis_3;
            }
            return version;
        }

    }
}
