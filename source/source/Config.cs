using DazUnpacker.source;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DazUnpacker
{
    public class _TabSettings
    {
        public List<string> BasicLibraries = new List<string>();
        public GenesisVersion genesisVersionFilter = GenesisVersion.Unknown;
        public GenesisVersion genesisVersionFallback = GenesisVersion.Genesis_9;
        public GenesisGenders genesisGenderFallback = GenesisGenders.Female;
        public bool UseSpecificTempFolder = false;
        public string TempFolder = "";
        public bool UseLanguageRu;
    }

    public class _TabInstaller
    {
        public int IntallLibrary = -1;
        public List<int> GarbageLibraries = new List<int>();
        public bool InstallDufOnly = true;
        public bool OverwriteExisting = false;
        public bool DetectContentTypePerFile = true;
        public int ThreadsCount = 1;
        public bool IgnoreRootFolder = false;
        public bool UsePrefix = false;
        public string PrefixName = "";
        public bool CreateRootSubfolder = false;
        public string RootSubfolder = "";
        public FilterManager Filters = new FilterManager();
    }
    public class _TabLog
    {
        public bool DeleteLogOnStart = false;
    }

    public class _TabCatalog
    {
        public string ScanFolder = "";
        public int ThreadsCount = 2;
        public bool IgnoreCase = false;
        public bool UniqueArchive = false;
    }

    public class ConfigStorage
    {
        public _TabSettings TabSettings = new _TabSettings();
        public _TabInstaller TabInstaller = new _TabInstaller();
        public _TabCatalog TabCatalog = new _TabCatalog();
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

        // Set when config.txt was damaged or could not be applied completely; written to the log when the log is ready
        public static string LoadWarning { get; private set; }

        private static Form1 form;
        private static string configPath =
            Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) 
            + "\\config.txt";
        
        public static void Init(Form1 frm)
        {
            form = frm;

            LoadCommonData();
            LoadDefaults();

            if (LoadStorage())
            {
                try
                {
                    ApplyStorage();
                }
                catch (Exception ex)
                {
                    LoadWarning = "Some settings of config.txt cannot be applied: " + ex.Message;
                }
            }
        }

        private static void LoadCommonData()
        {
            Data.UnpackerTool7z = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) +
                @"\tools\7z\7z.exe";
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
            try
            {
                string str = File.ReadAllText(configPath);
                var settings = new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto // Interface support
                };
                ConfigStorage loaded = JsonConvert.DeserializeObject<ConfigStorage>(str, settings);
                if (loaded == null)
                {
                    throw new InvalidDataException("the file is empty");
                }
                FillMissingSections(loaded);
                Storage = loaded;
                return true;
            }
            catch (Exception ex)
            {
                BackupBrokenConfig(ex);
                return false;
            }
        }

        // A damaged or hand-edited config may have "null" instead of a section
        private static void FillMissingSections(ConfigStorage storage)
        {
            if (storage.TabSettings == null)
            {
                storage.TabSettings = new _TabSettings();
            }
            if (storage.TabSettings.BasicLibraries == null)
            {
                storage.TabSettings.BasicLibraries = new List<string>();
            }
            if (storage.TabInstaller == null)
            {
                storage.TabInstaller = new _TabInstaller();
            }
            if (storage.TabInstaller.GarbageLibraries == null)
            {
                storage.TabInstaller.GarbageLibraries = new List<int>();
            }
            if (storage.TabInstaller.Filters == null)
            {
                storage.TabInstaller.Filters = new FilterManager();
            }
            if (storage.TabInstaller.Filters.FilterList == null)
            {
                storage.TabInstaller.Filters.FilterList = new List<IFilter>();
            }
            if (storage.TabCatalog == null)
            {
                storage.TabCatalog = new _TabCatalog();
            }
            if (storage.TabLog == null)
            {
                storage.TabLog = new _TabLog();
            }
        }

        // The damaged file is copied to config.txt.bad (it is overwritten on exit); the application starts with the defaults
        private static void BackupBrokenConfig(Exception reason)
        {
            LoadWarning = "config.txt cannot be read (" + reason.Message + "), the defaults are used";
            try
            {
                File.Copy(configPath, configPath + ".bad", true);
            }
            catch (Exception ex)
            {
                LoadWarning += "; the copy config.txt.bad failed: " + ex.Message;
            }
        }

        private static decimal ClampToRange(NumericUpDown control, int value)
        {
            return Math.Max(control.Minimum, Math.Min(control.Maximum, value));
        }

        private static void LoadDefaults()
        {
            // Tab Settings
            foreach (string name in Enum.GetNames(typeof(GenesisGenders)))
            {
                form.comboBoxFallbackGender.Items.Add(name);
            }
            form.comboBoxFallbackGender.Text = GenesisGenders.Female.ToString();

            foreach (string name in Enum.GetNames(typeof(GenesisVersion)))
            {
                form.comboBoxFallbackGeneration.Items.Add(name);
            }
            form.comboBoxFallbackGeneration.Text = GenesisVersion.Genesis_9.ToString();

            // Tab Installer
            {
                // Filters
                foreach (string name in Enum.GetNames(typeof(DateFilterDirection)))
                {
                    form.comboBoxDateFilterDirection.Items.Add(name);
                }
                form.comboBoxDateFilterDirection.SelectedIndex = 0;
                form.dateTimePickerFilterCreated.Value = DateTime.Now;

                form.comboBoxFilterContentTypes.Items.Clear();

                foreach (string contentType in Enum.GetNames(typeof(ContentTypes)))
                {
                    form.comboBoxFilterContentTypes.Items.Add(contentType);
                }
                form.comboBoxFilterContentTypes.SelectedIndex = 0;
            }

            foreach (string name in Enum.GetNames(typeof(ContentTypes)))
            {
                form.comboBoxContentType.Items.Add(name);
            }
            form.comboBoxContentType.SelectedIndex = 0;
        }

        private static void ApplyStorage()
        {
            // General
            if (Storage.TabNumber >= 0 && Storage.TabNumber < form.tabControl.TabPages.Count)
            {
                form.tabControl.SelectedIndex = Storage.TabNumber;
            }

            // Tab Settings
            foreach (string item in Storage.TabSettings.BasicLibraries)
            {
                Library.Basic.Add(item);
            }
            Library.Install.SetIndex(Storage.TabInstaller.IntallLibrary);
            form.comboBoxFallbackGender.SelectedIndex = form.comboBoxFallbackGender.FindStringExact(GenesisGenders.Female.ToString());
            form.radioButtonLangRussian.Checked = Storage.TabSettings.UseLanguageRu;
            form.radioButtonTempIsSpecific.Checked = Storage.TabSettings.UseSpecificTempFolder;
            form.textBoxTempFolder.Text = Storage.TabSettings.TempFolder;
            form.checkBoxGenesis3.Checked = (Storage.TabSettings.genesisVersionFilter & GenesisVersion.Genesis_3) != 0;
            form.checkBoxGenesis8.Checked = (Storage.TabSettings.genesisVersionFilter & GenesisVersion.Genesis_8) != 0;
            form.checkBoxGenesis9.Checked = (Storage.TabSettings.genesisVersionFilter & GenesisVersion.Genesis_9) != 0;
            form.comboBoxFallbackGeneration.Text = Storage.TabSettings.genesisVersionFallback.ToString();
            form.comboBoxFallbackGender.Text = Storage.TabSettings.genesisGenderFallback.ToString();

            // Tab Installer
            form.checkBoxSubfolderPrefix.Checked = Storage.TabInstaller.UsePrefix;
            form.textBoxSubfolderPrefix.Text = Storage.TabInstaller.PrefixName;
            form.textBoxRootSubfolder.Text = Storage.TabInstaller.RootSubfolder;
            form.checkBoxCreateRootSubfolder.Checked = Storage.TabInstaller.CreateRootSubfolder;
            form.checkBoxDetectContentTypePerFile.Checked = Storage.TabInstaller.DetectContentTypePerFile;
            form.checkBoxInstallDufOnly.Checked = Storage.TabInstaller.InstallDufOnly;
            form.numericUpDownThreadsCount.Value = ClampToRange(form.numericUpDownThreadsCount, Storage.TabInstaller.ThreadsCount);
            LoadFiltersOnForm();
            form.checkBoxOverwrite.Checked = Storage.TabInstaller.OverwriteExisting;
            form.checkBoxHierarhyByRootFolder.Checked = !Storage.TabInstaller.IgnoreRootFolder;

            // Tab Catalog
            form.CatalogForm.LoadSettings(Storage.TabCatalog);

            // Tab Log
            form.checkBoxDeleteLogOnStart.Checked = Storage.TabLog.DeleteLogOnStart;
        }

        public static void SaveConfig()
        {
            // General
            Storage.TabNumber = form.tabControl.SelectedIndex;
            Storage.TabSettings.BasicLibraries.Clear();
            Storage.TabInstaller.GarbageLibraries.Clear();

            // Tab Settings
            foreach (string item in Library.Basic.GetLibraries())
            {
                Storage.TabSettings.BasicLibraries.Add(item);
            }
            Storage.TabSettings.genesisVersionFilter = GetGenesisVersionFilter();
            Storage.TabSettings.genesisVersionFallback = (GenesisVersion)Enum.Parse(typeof(GenesisVersion), form.comboBoxFallbackGeneration.Text);
            Storage.TabSettings.genesisGenderFallback = (GenesisGenders)Enum.Parse(typeof(GenesisGenders), form.comboBoxFallbackGender.Text);
            Storage.TabSettings.TempFolder = form.textBoxTempFolder.Text;

            // Tab Installer
            Storage.TabInstaller.IntallLibrary = Library.Install.GetIndex();
            Storage.TabInstaller.DetectContentTypePerFile = form.checkBoxDetectContentTypePerFile.Checked;
            Storage.TabInstaller.OverwriteExisting = form.checkBoxOverwrite.Checked;
            Storage.TabInstaller.ThreadsCount = (int)form.numericUpDownThreadsCount.Value;
            Storage.TabInstaller.IgnoreRootFolder = !form.checkBoxHierarhyByRootFolder.Checked;
            Storage.TabInstaller.UsePrefix = form.checkBoxSubfolderPrefix.Checked;
            Storage.TabInstaller.PrefixName = form.textBoxSubfolderPrefix.Text;
            Storage.TabInstaller.CreateRootSubfolder = form.checkBoxCreateRootSubfolder.Checked;
            Storage.TabInstaller.RootSubfolder = form.textBoxRootSubfolder.Text;
            SaveFiltersFromForm();

            // Tab Catalog
            form.CatalogForm.SaveSettings(Storage.TabCatalog);

            // Tab Log
            Storage.TabLog.DeleteLogOnStart = form.checkBoxDeleteLogOnStart.Checked;

            var settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto, // Interface support
                Formatting = Formatting.Indented
            };
            string str = JsonConvert.SerializeObject(Storage, settings);
            try
            {
                // write to a temporary file first, so that an interrupted write cannot leave a half-written config.txt
                string tempPath = configPath + ".tmp";
                File.WriteAllText(tempPath, str);
                if (File.Exists(configPath))
                {
                    File.Replace(tempPath, configPath, null);
                }
                else
                {
                    File.Move(tempPath, configPath);
                }
            }
            catch (Exception ex)
            {
                Globals.ReportError(ex, false, "Save config");
            }
        }

        internal static void LoadFiltersOnForm()
        {
            foreach (IFilter filter in Config.Storage.TabInstaller.Filters.FilterList)
            {
                if (filter is DateFilter dateFilter)
                {
                    form.comboBoxDateFilterDirection.Text = dateFilter.Direction.ToString();
                    DateTime date = dateFilter.Date;
                    if (date < form.dateTimePickerFilterCreated.MinDate || date > form.dateTimePickerFilterCreated.MaxDate)
                    {
                        date = DateTime.Now; // a missing or invalid date in the config
                    }
                    form.dateTimePickerFilterCreated.Value = date;
                }
                else if (filter is ContentFilter contentFilter)
                {
                    form.comboBoxFilterContentTypes.Text = contentFilter.ContentType.ToString();
                }
            }
            form.checkBoxDateFilterCreated.Checked = Storage.TabInstaller.Filters.IsDateFilterEnabled;
            form.checkBoxFilterByType.Checked = Storage.TabInstaller.Filters.IsContentTypeFilterEnabled;
        }

        internal static void SaveFiltersFromForm()
        {
            Storage.TabInstaller.Filters.ClearFilters();

            if (form.checkBoxDateFilterCreated.Checked)
            {
                DateFilterDirection direction = (DateFilterDirection)Enum.Parse(typeof(DateFilterDirection), form.comboBoxDateFilterDirection.Text);
                Storage.TabInstaller.Filters.AddDateFilter(DateFilterAction.Created, direction, form.dateTimePickerFilterCreated.Value);
            }
            if (form.checkBoxFilterByType.Checked)
            {
                if (Enum.TryParse<ContentTypes>(form.comboBoxFilterContentTypes.Text, out var contentType))
                {
                    Storage.TabInstaller.Filters.AddContentTypeFilter(contentType);
                }
            }
            Storage.TabInstaller.Filters.IsDateFilterEnabled = form.checkBoxDateFilterCreated.Checked;
            Storage.TabInstaller.Filters.IsContentTypeFilterEnabled = form.checkBoxFilterByType.Checked;
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
