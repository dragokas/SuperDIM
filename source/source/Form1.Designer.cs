namespace DazUnpacker
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.label1 = new System.Windows.Forms.Label();
            this.buttonAddLibraryPath = new System.Windows.Forms.Button();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPageSettings = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panelLanguage = new System.Windows.Forms.Panel();
            this.radioButtonLangRussian = new System.Windows.Forms.RadioButton();
            this.radioButtonLangEnglish = new System.Windows.Forms.RadioButton();
            this.label19 = new System.Windows.Forms.Label();
            this.panel7 = new System.Windows.Forms.Panel();
            this.checkedListBoxLibraryDirs = new System.Windows.Forms.CheckedListBox();
            this.label23 = new System.Windows.Forms.Label();
            this.buttonRemoveLibraryPath = new System.Windows.Forms.Button();
            this.label22 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label20 = new System.Windows.Forms.Label();
            this.buttonSelectTempFolder = new System.Windows.Forms.Button();
            this.textBoxTempFolder = new System.Windows.Forms.TextBox();
            this.radioButtonTempIsSpecific = new System.Windows.Forms.RadioButton();
            this.radioButtonTempSameAsInstall = new System.Windows.Forms.RadioButton();
            this.label8 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label21 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label28 = new System.Windows.Forms.Label();
            this.comboBoxGender = new System.Windows.Forms.ComboBox();
            this.label29 = new System.Windows.Forms.Label();
            this.comboBoxGeneration = new System.Windows.Forms.ComboBox();
            this.label24 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.checkBoxGenesis9 = new System.Windows.Forms.CheckBox();
            this.checkBoxGenesis3 = new System.Windows.Forms.CheckBox();
            this.checkBoxGenesis8 = new System.Windows.Forms.CheckBox();
            this.tabPageSort = new System.Windows.Forms.TabPage();
            this.label15 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.checkBoxUseSubfolder = new System.Windows.Forms.CheckBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.panelSubfolder = new System.Windows.Forms.Panel();
            this.panelSubfolderAutoDetect = new System.Windows.Forms.Panel();
            this.label33 = new System.Windows.Forms.Label();
            this.checkBoxSearchSubfolder = new System.Windows.Forms.CheckBox();
            this.label17 = new System.Windows.Forms.Label();
            this.checkBoxSubfolderAsZipLocation = new System.Windows.Forms.CheckBox();
            this.label16 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.checkedListBoxSearchLibs = new System.Windows.Forms.CheckedListBox();
            this.panel5 = new System.Windows.Forms.Panel();
            this.label18 = new System.Windows.Forms.Label();
            this.checkedListBoxSubfolderBlock = new System.Windows.Forms.CheckedListBox();
            this.label4 = new System.Windows.Forms.Label();
            this.panel9 = new System.Windows.Forms.Panel();
            this.radioButtonPrioritySubfolderZipLocation = new System.Windows.Forms.RadioButton();
            this.radioButtonPrioritySubfolderOldLibrary = new System.Windows.Forms.RadioButton();
            this.radioButtonAutoDetectSubfolder = new System.Windows.Forms.RadioButton();
            this.label9 = new System.Windows.Forms.Label();
            this.radioButtonSpecificSubfolder = new System.Windows.Forms.RadioButton();
            this.textBoxSubfolderForced = new System.Windows.Forms.TextBox();
            this.tabPageInstaller = new System.Windows.Forms.TabPage();
            this.checkBoxIgnoreRootFolder = new System.Windows.Forms.CheckBox();
            this.checkBoxOverwrite = new System.Windows.Forms.CheckBox();
            this.panel8 = new System.Windows.Forms.Panel();
            this.buttonClear = new System.Windows.Forms.Button();
            this.labelProgress = new System.Windows.Forms.Label();
            this.listViewContent = new System.Windows.Forms.ListView();
            this.columnName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnPath = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnProgress = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnSubfolder = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.buttonAddContentFolder = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.radioButtonInstallLibs = new EWSoftware.ListControls.RadioButtonList();
            this.checkBoxDetectContentTypePerFile = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.checkBoxDateFilterCreated = new System.Windows.Forms.CheckBox();
            this.dateTimePickerFilterCreated = new System.Windows.Forms.DateTimePicker();
            this.comboBoxDateFilter = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.radioButtonFullInstall = new System.Windows.Forms.RadioButton();
            this.checkBoxInstallDufOnly = new System.Windows.Forms.RadioButton();
            this.panel6 = new System.Windows.Forms.Panel();
            this.textBoxSubfolderPrefix = new System.Windows.Forms.TextBox();
            this.checkBoxSubfolderPrefix = new System.Windows.Forms.CheckBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.textBoxRootSubfolder = new System.Windows.Forms.TextBox();
            this.checkBoxCreateRootSubfolder = new System.Windows.Forms.CheckBox();
            this.labelSpeed = new System.Windows.Forms.Label();
            this.labelTimeElapsed = new System.Windows.Forms.Label();
            this.label32 = new System.Windows.Forms.Label();
            this.label31 = new System.Windows.Forms.Label();
            this.label30 = new System.Windows.Forms.Label();
            this.numericUpDownThreadsCount = new System.Windows.Forms.NumericUpDown();
            this.comboBoxContentType = new System.Windows.Forms.ComboBox();
            this.label27 = new System.Windows.Forms.Label();
            this.comboBoxContentRootFolder = new System.Windows.Forms.ComboBox();
            this.label26 = new System.Windows.Forms.Label();
            this.buttonStart = new System.Windows.Forms.Button();
            this.tabPageLogs = new System.Windows.Forms.TabPage();
            this.buttonReInstallAccessDenied = new System.Windows.Forms.Button();
            this.listViewSkippedContent = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.buttonDeleteLog = new System.Windows.Forms.Button();
            this.buttonOpenLog = new System.Windows.Forms.Button();
            this.checkBoxDeleteLogOnStart = new System.Windows.Forms.CheckBox();
            this.checkBoxLogDetails = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.listBoxLog = new System.Windows.Forms.ListBox();
            this.textBoxLogDetails = new System.Windows.Forms.TextBox();
            this.contextMenuCommon = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl.SuspendLayout();
            this.tabPageSettings.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panelLanguage.SuspendLayout();
            this.panel7.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabPageSort.SuspendLayout();
            this.panelSubfolder.SuspendLayout();
            this.panelSubfolderAutoDetect.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel9.SuspendLayout();
            this.tabPageInstaller.SuspendLayout();
            this.panel8.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radioButtonInstallLibs)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownThreadsCount)).BeginInit();
            this.tabPageLogs.SuspendLayout();
            this.contextMenuCommon.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(4, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(143, 24);
            this.label1.TabIndex = 1;
            this.label1.Text = "DAZ Libraries:";
            // 
            // buttonAddLibraryPath
            // 
            this.buttonAddLibraryPath.Location = new System.Drawing.Point(9, 402);
            this.buttonAddLibraryPath.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonAddLibraryPath.Name = "buttonAddLibraryPath";
            this.buttonAddLibraryPath.Size = new System.Drawing.Size(200, 48);
            this.buttonAddLibraryPath.TabIndex = 2;
            this.buttonAddLibraryPath.Text = "Add ...";
            this.buttonAddLibraryPath.UseVisualStyleBackColor = true;
            this.buttonAddLibraryPath.Click += new System.EventHandler(this.buttonAddLibraryPath_Click);
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPageSettings);
            this.tabControl.Controls.Add(this.tabPageSort);
            this.tabControl.Controls.Add(this.tabPageInstaller);
            this.tabControl.Controls.Add(this.tabPageLogs);
            this.tabControl.Location = new System.Drawing.Point(17, 18);
            this.tabControl.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(1166, 1117);
            this.tabControl.TabIndex = 3;
            // 
            // tabPageSettings
            // 
            this.tabPageSettings.Controls.Add(this.panel1);
            this.tabPageSettings.Location = new System.Drawing.Point(4, 33);
            this.tabPageSettings.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPageSettings.Name = "tabPageSettings";
            this.tabPageSettings.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPageSettings.Size = new System.Drawing.Size(1158, 1080);
            this.tabPageSettings.TabIndex = 0;
            this.tabPageSettings.Text = "Settings";
            this.tabPageSettings.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panelLanguage);
            this.panel1.Controls.Add(this.panel7);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Location = new System.Drawing.Point(9, 10);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1111, 1058);
            this.panel1.TabIndex = 0;
            // 
            // panelLanguage
            // 
            this.panelLanguage.Controls.Add(this.radioButtonLangRussian);
            this.panelLanguage.Controls.Add(this.radioButtonLangEnglish);
            this.panelLanguage.Controls.Add(this.label19);
            this.panelLanguage.Location = new System.Drawing.Point(617, 5);
            this.panelLanguage.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panelLanguage.Name = "panelLanguage";
            this.panelLanguage.Size = new System.Drawing.Size(490, 32);
            this.panelLanguage.TabIndex = 13;
            // 
            // radioButtonLangRussian
            // 
            this.radioButtonLangRussian.AutoSize = true;
            this.radioButtonLangRussian.Location = new System.Drawing.Point(357, 0);
            this.radioButtonLangRussian.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.radioButtonLangRussian.Name = "radioButtonLangRussian";
            this.radioButtonLangRussian.Size = new System.Drawing.Size(107, 29);
            this.radioButtonLangRussian.TabIndex = 2;
            this.radioButtonLangRussian.Text = "Russian";
            this.radioButtonLangRussian.UseVisualStyleBackColor = true;
            this.radioButtonLangRussian.CheckedChanged += new System.EventHandler(this.radioButtonLangRussian_CheckedChanged);
            // 
            // radioButtonLangEnglish
            // 
            this.radioButtonLangEnglish.AutoSize = true;
            this.radioButtonLangEnglish.Checked = true;
            this.radioButtonLangEnglish.Location = new System.Drawing.Point(244, 2);
            this.radioButtonLangEnglish.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.radioButtonLangEnglish.Name = "radioButtonLangEnglish";
            this.radioButtonLangEnglish.Size = new System.Drawing.Size(101, 29);
            this.radioButtonLangEnglish.TabIndex = 1;
            this.radioButtonLangEnglish.TabStop = true;
            this.radioButtonLangEnglish.Text = "English";
            this.radioButtonLangEnglish.UseVisualStyleBackColor = true;
            this.radioButtonLangEnglish.CheckedChanged += new System.EventHandler(this.radioButtonLangEnglish_CheckedChanged);
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label19.Location = new System.Drawing.Point(4, 2);
            this.label19.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(218, 24);
            this.label19.TabIndex = 0;
            this.label19.Text = "Language of interface:";
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.checkedListBoxLibraryDirs);
            this.panel7.Controls.Add(this.label23);
            this.panel7.Controls.Add(this.buttonRemoveLibraryPath);
            this.panel7.Controls.Add(this.label22);
            this.panel7.Controls.Add(this.buttonAddLibraryPath);
            this.panel7.Controls.Add(this.label1);
            this.panel7.Location = new System.Drawing.Point(21, 47);
            this.panel7.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(1045, 455);
            this.panel7.TabIndex = 12;
            // 
            // checkedListBoxLibraryDirs
            // 
            this.checkedListBoxLibraryDirs.AllowDrop = true;
            this.checkedListBoxLibraryDirs.FormattingEnabled = true;
            this.checkedListBoxLibraryDirs.Location = new System.Drawing.Point(24, 35);
            this.checkedListBoxLibraryDirs.Margin = new System.Windows.Forms.Padding(4);
            this.checkedListBoxLibraryDirs.Name = "checkedListBoxLibraryDirs";
            this.checkedListBoxLibraryDirs.Size = new System.Drawing.Size(651, 238);
            this.checkedListBoxLibraryDirs.TabIndex = 16;
            this.checkedListBoxLibraryDirs.DragDrop += new System.Windows.Forms.DragEventHandler(this.checkedListBoxLibraryDirs_DragDrop);
            this.checkedListBoxLibraryDirs.DragEnter += new System.Windows.Forms.DragEventHandler(this.checkedListBoxLibraryDirs_DragEnter);
            this.checkedListBoxLibraryDirs.MouseEnter += new System.EventHandler(this.checkedListBoxLibraryDirs_MouseEnter);
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(20, 353);
            this.label23.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(552, 25);
            this.label23.TabIndex = 15;
            this.label23.Text = "You can include empty folders as well, to install new content to.";
            // 
            // buttonRemoveLibraryPath
            // 
            this.buttonRemoveLibraryPath.Location = new System.Drawing.Point(403, 402);
            this.buttonRemoveLibraryPath.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonRemoveLibraryPath.Name = "buttonRemoveLibraryPath";
            this.buttonRemoveLibraryPath.Size = new System.Drawing.Size(207, 48);
            this.buttonRemoveLibraryPath.TabIndex = 3;
            this.buttonRemoveLibraryPath.Text = "Remove from the list";
            this.buttonRemoveLibraryPath.UseVisualStyleBackColor = true;
            this.buttonRemoveLibraryPath.Click += new System.EventHandler(this.buttonRemoveLibraryPath_Click);
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(20, 312);
            this.label22.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(612, 25);
            this.label22.TabIndex = 14;
            this.label22.Text = "Please, add here all DAZ content library folders, you plan to work with.";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.label20);
            this.panel3.Controls.Add(this.buttonSelectTempFolder);
            this.panel3.Controls.Add(this.textBoxTempFolder);
            this.panel3.Controls.Add(this.radioButtonTempIsSpecific);
            this.panel3.Controls.Add(this.radioButtonTempSameAsInstall);
            this.panel3.Controls.Add(this.label8);
            this.panel3.Location = new System.Drawing.Point(21, 532);
            this.panel3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1045, 178);
            this.panel3.TabIndex = 11;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.ForeColor = System.Drawing.Color.Gray;
            this.label20.Location = new System.Drawing.Point(304, 74);
            this.label20.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(360, 25);
            this.label20.TabIndex = 5;
            this.label20.Text = "( usually: ?:\\Temp\\Daz - it\'s fastest way)";
            // 
            // buttonSelectTempFolder
            // 
            this.buttonSelectTempFolder.Enabled = false;
            this.buttonSelectTempFolder.Location = new System.Drawing.Point(738, 108);
            this.buttonSelectTempFolder.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonSelectTempFolder.Name = "buttonSelectTempFolder";
            this.buttonSelectTempFolder.Size = new System.Drawing.Size(114, 43);
            this.buttonSelectTempFolder.TabIndex = 4;
            this.buttonSelectTempFolder.Text = "Select ...";
            this.buttonSelectTempFolder.UseVisualStyleBackColor = true;
            this.buttonSelectTempFolder.Click += new System.EventHandler(this.buttonSelectTempFolder_Click);
            // 
            // textBoxTempFolder
            // 
            this.textBoxTempFolder.Enabled = false;
            this.textBoxTempFolder.Location = new System.Drawing.Point(196, 119);
            this.textBoxTempFolder.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBoxTempFolder.Name = "textBoxTempFolder";
            this.textBoxTempFolder.Size = new System.Drawing.Size(532, 29);
            this.textBoxTempFolder.TabIndex = 3;
            this.textBoxTempFolder.TextChanged += new System.EventHandler(this.textBoxTempFolder_TextChanged);
            // 
            // radioButtonTempIsSpecific
            // 
            this.radioButtonTempIsSpecific.AutoSize = true;
            this.radioButtonTempIsSpecific.Location = new System.Drawing.Point(20, 120);
            this.radioButtonTempIsSpecific.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.radioButtonTempIsSpecific.Name = "radioButtonTempIsSpecific";
            this.radioButtonTempIsSpecific.Size = new System.Drawing.Size(165, 29);
            this.radioButtonTempIsSpecific.TabIndex = 2;
            this.radioButtonTempIsSpecific.Text = "Specific folder:";
            this.radioButtonTempIsSpecific.UseVisualStyleBackColor = true;
            this.radioButtonTempIsSpecific.CheckedChanged += new System.EventHandler(this.radioButtonTempIsSpecific_CheckedChanged);
            // 
            // radioButtonTempSameAsInstall
            // 
            this.radioButtonTempSameAsInstall.AutoSize = true;
            this.radioButtonTempSameAsInstall.Checked = true;
            this.radioButtonTempSameAsInstall.Location = new System.Drawing.Point(20, 72);
            this.radioButtonTempSameAsInstall.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.radioButtonTempSameAsInstall.Name = "radioButtonTempSameAsInstall";
            this.radioButtonTempSameAsInstall.Size = new System.Drawing.Size(278, 29);
            this.radioButtonTempSameAsInstall.TabIndex = 1;
            this.radioButtonTempSameAsInstall.TabStop = true;
            this.radioButtonTempSameAsInstall.Text = "Same as target library drive ";
            this.radioButtonTempSameAsInstall.UseVisualStyleBackColor = true;
            this.radioButtonTempSameAsInstall.CheckedChanged += new System.EventHandler(this.radioButtonTempSameAsInstall_CheckedChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label8.Location = new System.Drawing.Point(27, 25);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(186, 24);
            this.label8.TabIndex = 0;
            this.label8.Text = "Temporarily folder:";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label21);
            this.panel2.Controls.Add(this.groupBox2);
            this.panel2.Controls.Add(this.label24);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.checkBoxGenesis9);
            this.panel2.Controls.Add(this.checkBoxGenesis3);
            this.panel2.Controls.Add(this.checkBoxGenesis8);
            this.panel2.Location = new System.Drawing.Point(21, 745);
            this.panel2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1045, 293);
            this.panel2.TabIndex = 10;
            // 
            // label21
            // 
            this.label21.ForeColor = System.Drawing.Color.Gray;
            this.label21.Location = new System.Drawing.Point(40, 230);
            this.label21.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(732, 49);
            this.label21.TabIndex = 49;
            this.label21.Text = "* Content types: \"Environment\", \"Props\", \"Scripts\" are mostly version-independent" +
    ". They get installed at first selected Genesis.";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label28);
            this.groupBox2.Controls.Add(this.comboBoxGender);
            this.groupBox2.Controls.Add(this.label29);
            this.groupBox2.Controls.Add(this.comboBoxGeneration);
            this.groupBox2.Location = new System.Drawing.Point(738, 40);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox2.Size = new System.Drawing.Size(298, 136);
            this.groupBox2.TabIndex = 48;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "On fail detection (fallback):";
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(39, 52);
            this.label28.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(83, 25);
            this.label28.TabIndex = 31;
            this.label28.Text = "Gender:";
            // 
            // comboBoxGender
            // 
            this.comboBoxGender.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxGender.FormattingEnabled = true;
            this.comboBoxGender.Location = new System.Drawing.Point(128, 48);
            this.comboBoxGender.Margin = new System.Windows.Forms.Padding(4);
            this.comboBoxGender.Name = "comboBoxGender";
            this.comboBoxGender.Size = new System.Drawing.Size(162, 32);
            this.comboBoxGender.TabIndex = 32;
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Location = new System.Drawing.Point(7, 95);
            this.label29.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(114, 25);
            this.label29.TabIndex = 33;
            this.label29.Text = "Generation:";
            // 
            // comboBoxGeneration
            // 
            this.comboBoxGeneration.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxGeneration.FormattingEnabled = true;
            this.comboBoxGeneration.Location = new System.Drawing.Point(128, 89);
            this.comboBoxGeneration.Margin = new System.Windows.Forms.Padding(4);
            this.comboBoxGeneration.Name = "comboBoxGeneration";
            this.comboBoxGeneration.Size = new System.Drawing.Size(162, 32);
            this.comboBoxGeneration.TabIndex = 34;
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.ForeColor = System.Drawing.Color.Gray;
            this.label24.Location = new System.Drawing.Point(40, 193);
            this.label24.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(687, 25);
            this.label24.TabIndex = 10;
            this.label24.Text = "* Note: if archive contains both generations, all selected above will be installe" +
    "d.";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.Location = new System.Drawing.Point(27, 19);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(287, 24);
            this.label5.TabIndex = 6;
            this.label5.Text = "Install these generations only:";
            // 
            // checkBoxGenesis9
            // 
            this.checkBoxGenesis9.AutoSize = true;
            this.checkBoxGenesis9.Checked = true;
            this.checkBoxGenesis9.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxGenesis9.Location = new System.Drawing.Point(20, 146);
            this.checkBoxGenesis9.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.checkBoxGenesis9.Name = "checkBoxGenesis9";
            this.checkBoxGenesis9.Size = new System.Drawing.Size(126, 29);
            this.checkBoxGenesis9.TabIndex = 9;
            this.checkBoxGenesis9.Text = "Genesis 9";
            this.checkBoxGenesis9.UseVisualStyleBackColor = true;
            this.checkBoxGenesis9.CheckedChanged += new System.EventHandler(this.checkBoxGenesis9_CheckedChanged);
            // 
            // checkBoxGenesis3
            // 
            this.checkBoxGenesis3.AutoSize = true;
            this.checkBoxGenesis3.Checked = true;
            this.checkBoxGenesis3.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxGenesis3.Location = new System.Drawing.Point(20, 66);
            this.checkBoxGenesis3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.checkBoxGenesis3.Name = "checkBoxGenesis3";
            this.checkBoxGenesis3.Size = new System.Drawing.Size(126, 29);
            this.checkBoxGenesis3.TabIndex = 7;
            this.checkBoxGenesis3.Text = "Genesis 3";
            this.checkBoxGenesis3.UseVisualStyleBackColor = true;
            this.checkBoxGenesis3.CheckedChanged += new System.EventHandler(this.checkBoxGenesis3_CheckedChanged);
            // 
            // checkBoxGenesis8
            // 
            this.checkBoxGenesis8.AutoSize = true;
            this.checkBoxGenesis8.Checked = true;
            this.checkBoxGenesis8.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxGenesis8.Location = new System.Drawing.Point(20, 108);
            this.checkBoxGenesis8.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.checkBoxGenesis8.Name = "checkBoxGenesis8";
            this.checkBoxGenesis8.Size = new System.Drawing.Size(169, 29);
            this.checkBoxGenesis8.TabIndex = 8;
            this.checkBoxGenesis8.Text = "Genesis 8 / 8.1";
            this.checkBoxGenesis8.UseVisualStyleBackColor = true;
            this.checkBoxGenesis8.CheckedChanged += new System.EventHandler(this.checkBoxGenesis8_CheckedChanged);
            // 
            // tabPageSort
            // 
            this.tabPageSort.Controls.Add(this.label15);
            this.tabPageSort.Controls.Add(this.label13);
            this.tabPageSort.Controls.Add(this.label12);
            this.tabPageSort.Controls.Add(this.checkBoxUseSubfolder);
            this.tabPageSort.Controls.Add(this.label11);
            this.tabPageSort.Controls.Add(this.label10);
            this.tabPageSort.Controls.Add(this.panelSubfolder);
            this.tabPageSort.Location = new System.Drawing.Point(4, 33);
            this.tabPageSort.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPageSort.Name = "tabPageSort";
            this.tabPageSort.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPageSort.Size = new System.Drawing.Size(1158, 1080);
            this.tabPageSort.TabIndex = 3;
            this.tabPageSort.Text = "Sort";
            this.tabPageSort.UseVisualStyleBackColor = true;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.ForeColor = System.Drawing.Color.Gray;
            this.label15.Location = new System.Drawing.Point(396, 230);
            this.label15.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(368, 25);
            this.label15.TabIndex = 23;
            this.label15.Text = "(*.duf contents folder will be placed there)";
            // 
            // label13
            // 
            this.label13.Location = new System.Drawing.Point(33, 155);
            this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(1095, 70);
            this.label13.TabIndex = 22;
            this.label13.Text = "You can also use the folder name, where zip-file (to be installed) is located, as" +
    " a subfolder name. Exclusions can be defined.";
            // 
            // label12
            // 
            this.label12.Location = new System.Drawing.Point(33, 102);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(1095, 53);
            this.label12.TabIndex = 21;
            this.label12.Text = resources.GetString("label12.Text");
            // 
            // checkBoxUseSubfolder
            // 
            this.checkBoxUseSubfolder.AutoSize = true;
            this.checkBoxUseSubfolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.checkBoxUseSubfolder.Location = new System.Drawing.Point(54, 228);
            this.checkBoxUseSubfolder.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.checkBoxUseSubfolder.Name = "checkBoxUseSubfolder";
            this.checkBoxUseSubfolder.Size = new System.Drawing.Size(320, 28);
            this.checkBoxUseSubfolder.TabIndex = 17;
            this.checkBoxUseSubfolder.Text = "Which subfolder name to use?";
            this.checkBoxUseSubfolder.UseVisualStyleBackColor = true;
            this.checkBoxUseSubfolder.CheckedChanged += new System.EventHandler(this.checkBoxUseSubfolder_CheckedChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(33, 59);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(824, 25);
            this.label11.TabIndex = 20;
            this.label11.Text = "It is convenient to store them in subfolders by groups, like \"Props/Sport\", \"Prop" +
    "s/Learning\", etc.";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(33, 22);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(477, 25);
            this.label10.TabIndex = 19;
            this.label10.Text = "You can define rules, where you want to put *.duf files.";
            // 
            // panelSubfolder
            // 
            this.panelSubfolder.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelSubfolder.Controls.Add(this.panelSubfolderAutoDetect);
            this.panelSubfolder.Controls.Add(this.radioButtonAutoDetectSubfolder);
            this.panelSubfolder.Controls.Add(this.label9);
            this.panelSubfolder.Controls.Add(this.radioButtonSpecificSubfolder);
            this.panelSubfolder.Controls.Add(this.textBoxSubfolderForced);
            this.panelSubfolder.Enabled = false;
            this.panelSubfolder.Location = new System.Drawing.Point(37, 266);
            this.panelSubfolder.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panelSubfolder.Name = "panelSubfolder";
            this.panelSubfolder.Size = new System.Drawing.Size(1109, 801);
            this.panelSubfolder.TabIndex = 18;
            // 
            // panelSubfolderAutoDetect
            // 
            this.panelSubfolderAutoDetect.Controls.Add(this.label33);
            this.panelSubfolderAutoDetect.Controls.Add(this.checkBoxSearchSubfolder);
            this.panelSubfolderAutoDetect.Controls.Add(this.label17);
            this.panelSubfolderAutoDetect.Controls.Add(this.checkBoxSubfolderAsZipLocation);
            this.panelSubfolderAutoDetect.Controls.Add(this.label16);
            this.panelSubfolderAutoDetect.Controls.Add(this.label14);
            this.panelSubfolderAutoDetect.Controls.Add(this.checkedListBoxSearchLibs);
            this.panelSubfolderAutoDetect.Controls.Add(this.panel5);
            this.panelSubfolderAutoDetect.Controls.Add(this.panel9);
            this.panelSubfolderAutoDetect.Location = new System.Drawing.Point(23, 119);
            this.panelSubfolderAutoDetect.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panelSubfolderAutoDetect.Name = "panelSubfolderAutoDetect";
            this.panelSubfolderAutoDetect.Size = new System.Drawing.Size(1091, 676);
            this.panelSubfolderAutoDetect.TabIndex = 29;
            // 
            // label33
            // 
            this.label33.ForeColor = System.Drawing.Color.Gray;
            this.label33.Location = new System.Drawing.Point(458, 578);
            this.label33.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(605, 28);
            this.label33.TabIndex = 30;
            this.label33.Text = "* e.g.: C:/.../DazContent/Outfit/<RootSubfolderName>/Night/.../*";
            // 
            // checkBoxSearchSubfolder
            // 
            this.checkBoxSearchSubfolder.AutoSize = true;
            this.checkBoxSearchSubfolder.Location = new System.Drawing.Point(34, 19);
            this.checkBoxSearchSubfolder.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.checkBoxSearchSubfolder.Name = "checkBoxSearchSubfolder";
            this.checkBoxSearchSubfolder.Size = new System.Drawing.Size(438, 29);
            this.checkBoxSearchSubfolder.TabIndex = 28;
            this.checkBoxSearchSubfolder.Text = "find previous name in the \"Old libraries\" below:";
            this.checkBoxSearchSubfolder.UseVisualStyleBackColor = true;
            this.checkBoxSearchSubfolder.CheckedChanged += new System.EventHandler(this.checkBoxSearchSubfolder_CheckedChanged);
            // 
            // label17
            // 
            this.label17.ForeColor = System.Drawing.Color.Gray;
            this.label17.Location = new System.Drawing.Point(131, 278);
            this.label17.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(937, 62);
            this.label17.TabIndex = 24;
            this.label17.Text = "(e.g.: if you\'re installing C:/.../DazBackup/Dress/Night/PrettyShortsG8F.zip, thi" +
    "s content will be installed at \"Night\" subfolder as: <DazLibrary>/People/Genesis" +
    " 8 Female/Outfit/Night/<ContentName>/*)";
            // 
            // checkBoxSubfolderAsZipLocation
            // 
            this.checkBoxSubfolderAsZipLocation.AutoSize = true;
            this.checkBoxSubfolderAsZipLocation.Checked = true;
            this.checkBoxSubfolderAsZipLocation.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxSubfolderAsZipLocation.Location = new System.Drawing.Point(34, 245);
            this.checkBoxSubfolderAsZipLocation.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.checkBoxSubfolderAsZipLocation.Name = "checkBoxSubfolderAsZipLocation";
            this.checkBoxSubfolderAsZipLocation.Size = new System.Drawing.Size(548, 29);
            this.checkBoxSubfolderAsZipLocation.TabIndex = 23;
            this.checkBoxSubfolderAsZipLocation.Text = "Use folder name where zip-file located as a subfolder name";
            this.checkBoxSubfolderAsZipLocation.UseVisualStyleBackColor = true;
            this.checkBoxSubfolderAsZipLocation.CheckedChanged += new System.EventHandler(this.checkBoxSubfolderAsZipLocation_CheckedChanged);
            // 
            // label16
            // 
            this.label16.ForeColor = System.Drawing.Color.Gray;
            this.label16.Location = new System.Drawing.Point(571, 545);
            this.label16.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(496, 22);
            this.label16.TabIndex = 22;
            this.label16.Text = "* If none of rules matched, the subfolder is not used.";
            // 
            // label14
            // 
            this.label14.ForeColor = System.Drawing.Color.Gray;
            this.label14.Location = new System.Drawing.Point(131, 178);
            this.label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(937, 62);
            this.label14.TabIndex = 21;
            this.label14.Text = "(e.g.: if /ContentName/*.duf will be found in /OldLibrary/Props/Sport/ContentName" +
    "/*.duf, it will be installed in \"Sport\" subfolder: /<NewLibrary>/Props/Sport/Con" +
    "tentName/*.duf)";
            // 
            // checkedListBoxSearchLibs
            // 
            this.checkedListBoxSearchLibs.CheckOnClick = true;
            this.checkedListBoxSearchLibs.ColumnWidth = 200;
            this.checkedListBoxSearchLibs.Enabled = false;
            this.checkedListBoxSearchLibs.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.checkedListBoxSearchLibs.FormattingEnabled = true;
            this.checkedListBoxSearchLibs.Location = new System.Drawing.Point(134, 62);
            this.checkedListBoxSearchLibs.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.checkedListBoxSearchLibs.MultiColumn = true;
            this.checkedListBoxSearchLibs.Name = "checkedListBoxSearchLibs";
            this.checkedListBoxSearchLibs.Size = new System.Drawing.Size(928, 60);
            this.checkedListBoxSearchLibs.TabIndex = 20;
            this.checkedListBoxSearchLibs.SelectedIndexChanged += new System.EventHandler(this.checkedListBoxSearchLibraries_SelectedIndexChanged);
            this.checkedListBoxSearchLibs.MouseEnter += new System.EventHandler(this.checkedListBoxSearchLibraries_MouseEnter);
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.label18);
            this.panel5.Controls.Add(this.checkedListBoxSubfolderBlock);
            this.panel5.Controls.Add(this.label4);
            this.panel5.Location = new System.Drawing.Point(21, 347);
            this.panel5.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(1047, 193);
            this.panel5.TabIndex = 19;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.ForeColor = System.Drawing.Color.Gray;
            this.label18.Location = new System.Drawing.Point(224, 13);
            this.label18.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(481, 25);
            this.label18.TabIndex = 19;
            this.label18.Text = "(if planned subfolder matches this name, it is not used)";
            // 
            // checkedListBoxSubfolderBlock
            // 
            this.checkedListBoxSubfolderBlock.ColumnWidth = 200;
            this.checkedListBoxSubfolderBlock.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.checkedListBoxSubfolderBlock.FormattingEnabled = true;
            this.checkedListBoxSubfolderBlock.Location = new System.Drawing.Point(112, 42);
            this.checkedListBoxSubfolderBlock.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.checkedListBoxSubfolderBlock.MultiColumn = true;
            this.checkedListBoxSubfolderBlock.Name = "checkedListBoxSubfolderBlock";
            this.checkedListBoxSubfolderBlock.Size = new System.Drawing.Size(928, 88);
            this.checkedListBoxSubfolderBlock.TabIndex = 15;
            this.checkedListBoxSubfolderBlock.SelectedIndexChanged += new System.EventHandler(this.checkedListBoxSubfolderExclude_SelectedIndexChanged);
            this.checkedListBoxSubfolderBlock.MouseEnter += new System.EventHandler(this.checkedListBoxSubfolderExclude_MouseEnter);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(11, 13);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(187, 24);
            this.label4.TabIndex = 14;
            this.label4.Text = "Subfolder blocklist:";
            // 
            // panel9
            // 
            this.panel9.Controls.Add(this.radioButtonPrioritySubfolderZipLocation);
            this.panel9.Controls.Add(this.radioButtonPrioritySubfolderOldLibrary);
            this.panel9.Location = new System.Drawing.Point(777, 19);
            this.panel9.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(291, 322);
            this.panel9.TabIndex = 26;
            // 
            // radioButtonPrioritySubfolderZipLocation
            // 
            this.radioButtonPrioritySubfolderZipLocation.AutoSize = true;
            this.radioButtonPrioritySubfolderZipLocation.Location = new System.Drawing.Point(7, 226);
            this.radioButtonPrioritySubfolderZipLocation.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.radioButtonPrioritySubfolderZipLocation.Name = "radioButtonPrioritySubfolderZipLocation";
            this.radioButtonPrioritySubfolderZipLocation.Size = new System.Drawing.Size(168, 29);
            this.radioButtonPrioritySubfolderZipLocation.TabIndex = 1;
            this.radioButtonPrioritySubfolderZipLocation.Text = "Priority this rule";
            this.radioButtonPrioritySubfolderZipLocation.UseVisualStyleBackColor = true;
            this.radioButtonPrioritySubfolderZipLocation.CheckedChanged += new System.EventHandler(this.radioButtonPrioritySubfolderZipLocation_CheckedChanged);
            // 
            // radioButtonPrioritySubfolderOldLibrary
            // 
            this.radioButtonPrioritySubfolderOldLibrary.AutoSize = true;
            this.radioButtonPrioritySubfolderOldLibrary.Checked = true;
            this.radioButtonPrioritySubfolderOldLibrary.Location = new System.Drawing.Point(7, 5);
            this.radioButtonPrioritySubfolderOldLibrary.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.radioButtonPrioritySubfolderOldLibrary.Name = "radioButtonPrioritySubfolderOldLibrary";
            this.radioButtonPrioritySubfolderOldLibrary.Size = new System.Drawing.Size(168, 29);
            this.radioButtonPrioritySubfolderOldLibrary.TabIndex = 0;
            this.radioButtonPrioritySubfolderOldLibrary.TabStop = true;
            this.radioButtonPrioritySubfolderOldLibrary.Text = "Priority this rule";
            this.radioButtonPrioritySubfolderOldLibrary.UseVisualStyleBackColor = true;
            this.radioButtonPrioritySubfolderOldLibrary.CheckedChanged += new System.EventHandler(this.radioButtonPrioritySubfolderOldLibrary_CheckedChanged);
            // 
            // radioButtonAutoDetectSubfolder
            // 
            this.radioButtonAutoDetectSubfolder.AutoSize = true;
            this.radioButtonAutoDetectSubfolder.Checked = true;
            this.radioButtonAutoDetectSubfolder.Location = new System.Drawing.Point(15, 90);
            this.radioButtonAutoDetectSubfolder.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.radioButtonAutoDetectSubfolder.Name = "radioButtonAutoDetectSubfolder";
            this.radioButtonAutoDetectSubfolder.Size = new System.Drawing.Size(195, 29);
            this.radioButtonAutoDetectSubfolder.TabIndex = 27;
            this.radioButtonAutoDetectSubfolder.TabStop = true;
            this.radioButtonAutoDetectSubfolder.Text = "auto-detect name:";
            this.radioButtonAutoDetectSubfolder.UseVisualStyleBackColor = true;
            this.radioButtonAutoDetectSubfolder.CheckedChanged += new System.EventHandler(this.radioButtonAutoDetectSubfolder_CheckedChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ForeColor = System.Drawing.Color.Gray;
            this.label9.Location = new System.Drawing.Point(154, 61);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(921, 25);
            this.label9.TabIndex = 18;
            this.label9.Text = "(e.g.: \"Sport\" => will be created as \"<LibraryPath>/People/Genesis X/Outfit/Sport" +
    "/<ContentName>/*.duf\")";
            // 
            // radioButtonSpecificSubfolder
            // 
            this.radioButtonSpecificSubfolder.AutoSize = true;
            this.radioButtonSpecificSubfolder.Location = new System.Drawing.Point(15, 25);
            this.radioButtonSpecificSubfolder.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.radioButtonSpecificSubfolder.Name = "radioButtonSpecificSubfolder";
            this.radioButtonSpecificSubfolder.Size = new System.Drawing.Size(162, 29);
            this.radioButtonSpecificSubfolder.TabIndex = 16;
            this.radioButtonSpecificSubfolder.Text = "specific name:";
            this.radioButtonSpecificSubfolder.UseVisualStyleBackColor = true;
            this.radioButtonSpecificSubfolder.CheckedChanged += new System.EventHandler(this.radioButtonSpecificSubfolder_CheckedChanged_1);
            // 
            // textBoxSubfolderForced
            // 
            this.textBoxSubfolderForced.Enabled = false;
            this.textBoxSubfolderForced.Location = new System.Drawing.Point(187, 23);
            this.textBoxSubfolderForced.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBoxSubfolderForced.Name = "textBoxSubfolderForced";
            this.textBoxSubfolderForced.Size = new System.Drawing.Size(214, 29);
            this.textBoxSubfolderForced.TabIndex = 12;
            this.textBoxSubfolderForced.TextChanged += new System.EventHandler(this.textBoxSubfolderForced_TextChanged);
            // 
            // tabPageInstaller
            // 
            this.tabPageInstaller.AllowDrop = true;
            this.tabPageInstaller.Controls.Add(this.checkBoxIgnoreRootFolder);
            this.tabPageInstaller.Controls.Add(this.checkBoxOverwrite);
            this.tabPageInstaller.Controls.Add(this.panel8);
            this.tabPageInstaller.Controls.Add(this.groupBox4);
            this.tabPageInstaller.Controls.Add(this.checkBoxDetectContentTypePerFile);
            this.tabPageInstaller.Controls.Add(this.groupBox3);
            this.tabPageInstaller.Controls.Add(this.groupBox1);
            this.tabPageInstaller.Controls.Add(this.panel6);
            this.tabPageInstaller.Controls.Add(this.panel4);
            this.tabPageInstaller.Controls.Add(this.labelSpeed);
            this.tabPageInstaller.Controls.Add(this.labelTimeElapsed);
            this.tabPageInstaller.Controls.Add(this.label32);
            this.tabPageInstaller.Controls.Add(this.label31);
            this.tabPageInstaller.Controls.Add(this.label30);
            this.tabPageInstaller.Controls.Add(this.numericUpDownThreadsCount);
            this.tabPageInstaller.Controls.Add(this.comboBoxContentType);
            this.tabPageInstaller.Controls.Add(this.label27);
            this.tabPageInstaller.Controls.Add(this.comboBoxContentRootFolder);
            this.tabPageInstaller.Controls.Add(this.label26);
            this.tabPageInstaller.Controls.Add(this.buttonStart);
            this.tabPageInstaller.Location = new System.Drawing.Point(4, 33);
            this.tabPageInstaller.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPageInstaller.Name = "tabPageInstaller";
            this.tabPageInstaller.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPageInstaller.Size = new System.Drawing.Size(1158, 1080);
            this.tabPageInstaller.TabIndex = 1;
            this.tabPageInstaller.Text = "Installer";
            this.tabPageInstaller.UseVisualStyleBackColor = true;
            // 
            // checkBoxIgnoreRootFolder
            // 
            this.checkBoxIgnoreRootFolder.AutoSize = true;
            this.checkBoxIgnoreRootFolder.Location = new System.Drawing.Point(1043, 823);
            this.checkBoxIgnoreRootFolder.Margin = new System.Windows.Forms.Padding(4);
            this.checkBoxIgnoreRootFolder.Name = "checkBoxIgnoreRootFolder";
            this.checkBoxIgnoreRootFolder.Size = new System.Drawing.Size(92, 29);
            this.checkBoxIgnoreRootFolder.TabIndex = 52;
            this.checkBoxIgnoreRootFolder.Text = "ignore";
            this.checkBoxIgnoreRootFolder.UseVisualStyleBackColor = true;
            this.checkBoxIgnoreRootFolder.CheckedChanged += new System.EventHandler(this.checkBoxIgnoreRootFolder_CheckedChanged);
            // 
            // checkBoxOverwrite
            // 
            this.checkBoxOverwrite.AutoSize = true;
            this.checkBoxOverwrite.Location = new System.Drawing.Point(43, 923);
            this.checkBoxOverwrite.Margin = new System.Windows.Forms.Padding(4);
            this.checkBoxOverwrite.Name = "checkBoxOverwrite";
            this.checkBoxOverwrite.Size = new System.Drawing.Size(231, 29);
            this.checkBoxOverwrite.TabIndex = 51;
            this.checkBoxOverwrite.Text = "Overwrite existing files";
            this.checkBoxOverwrite.UseVisualStyleBackColor = true;
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.buttonClear);
            this.panel8.Controls.Add(this.labelProgress);
            this.panel8.Controls.Add(this.listViewContent);
            this.panel8.Controls.Add(this.progressBar1);
            this.panel8.Controls.Add(this.buttonAddContentFolder);
            this.panel8.Controls.Add(this.label3);
            this.panel8.Location = new System.Drawing.Point(31, 335);
            this.panel8.Margin = new System.Windows.Forms.Padding(4);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(1122, 479);
            this.panel8.TabIndex = 50;
            // 
            // buttonClear
            // 
            this.buttonClear.Location = new System.Drawing.Point(970, 422);
            this.buttonClear.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new System.Drawing.Size(100, 40);
            this.buttonClear.TabIndex = 42;
            this.buttonClear.Text = "Clear";
            this.buttonClear.UseVisualStyleBackColor = true;
            this.buttonClear.Click += new System.EventHandler(this.buttonClear_Click);
            // 
            // labelProgress
            // 
            this.labelProgress.AutoSize = true;
            this.labelProgress.Location = new System.Drawing.Point(385, 70);
            this.labelProgress.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelProgress.Name = "labelProgress";
            this.labelProgress.Size = new System.Drawing.Size(50, 25);
            this.labelProgress.TabIndex = 35;
            this.labelProgress.Text = "0 / 0";
            this.labelProgress.TextChanged += new System.EventHandler(this.labelProgress_TextChanged);
            // 
            // listViewContent
            // 
            this.listViewContent.AllowDrop = true;
            this.listViewContent.CheckBoxes = true;
            this.listViewContent.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnName,
            this.columnPath,
            this.columnProgress,
            this.columnSubfolder});
            this.listViewContent.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.listViewContent.FullRowSelect = true;
            this.listViewContent.GridLines = true;
            this.listViewContent.HideSelection = false;
            this.listViewContent.Location = new System.Drawing.Point(6, 98);
            this.listViewContent.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.listViewContent.MultiSelect = false;
            this.listViewContent.Name = "listViewContent";
            this.listViewContent.Size = new System.Drawing.Size(1109, 363);
            this.listViewContent.TabIndex = 16;
            this.listViewContent.UseCompatibleStateImageBehavior = false;
            this.listViewContent.View = System.Windows.Forms.View.Details;
            // 
            // columnName
            // 
            this.columnName.Text = "Name";
            this.columnName.Width = 300;
            // 
            // columnPath
            // 
            this.columnPath.Text = "Path";
            this.columnPath.Width = 200;
            // 
            // columnProgress
            // 
            this.columnProgress.Text = "Progress";
            this.columnProgress.Width = 70;
            // 
            // columnSubfolder
            // 
            this.columnSubfolder.Text = "Subfolder";
            this.columnSubfolder.Width = 220;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(565, 60);
            this.progressBar1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(551, 29);
            this.progressBar1.TabIndex = 10;
            // 
            // buttonAddContentFolder
            // 
            this.buttonAddContentFolder.Location = new System.Drawing.Point(10, 7);
            this.buttonAddContentFolder.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonAddContentFolder.Name = "buttonAddContentFolder";
            this.buttonAddContentFolder.Size = new System.Drawing.Size(169, 46);
            this.buttonAddContentFolder.TabIndex = 7;
            this.buttonAddContentFolder.Text = "Select Folder ...";
            this.buttonAddContentFolder.UseVisualStyleBackColor = true;
            this.buttonAddContentFolder.Click += new System.EventHandler(this.buttonAddContentFolder_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(13, 70);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(339, 24);
            this.label3.TabIndex = 5;
            this.label3.Text = "Drag && Drop content archives here:";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.radioButtonInstallLibs);
            this.groupBox4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox4.Location = new System.Drawing.Point(39, 30);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox4.Size = new System.Drawing.Size(512, 281);
            this.groupBox4.TabIndex = 49;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Where to install";
            // 
            // radioButtonInstallLibs
            // 
            this.radioButtonInstallLibs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radioButtonInstallLibs.ListBackColor = System.Drawing.Color.White;
            this.radioButtonInstallLibs.Location = new System.Drawing.Point(4, 26);
            this.radioButtonInstallLibs.Margin = new System.Windows.Forms.Padding(4);
            this.radioButtonInstallLibs.Name = "radioButtonInstallLibs";
            this.radioButtonInstallLibs.Size = new System.Drawing.Size(504, 251);
            this.radioButtonInstallLibs.TabIndex = 0;
            this.radioButtonInstallLibs.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.radioButtonInstallLibs.MouseEnter += new System.EventHandler(this.radioButtonInstallLibs_MouseEnter);
            // 
            // checkBoxDetectContentTypePerFile
            // 
            this.checkBoxDetectContentTypePerFile.AutoSize = true;
            this.checkBoxDetectContentTypePerFile.Checked = true;
            this.checkBoxDetectContentTypePerFile.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxDetectContentTypePerFile.Location = new System.Drawing.Point(534, 864);
            this.checkBoxDetectContentTypePerFile.Margin = new System.Windows.Forms.Padding(4);
            this.checkBoxDetectContentTypePerFile.Name = "checkBoxDetectContentTypePerFile";
            this.checkBoxDetectContentTypePerFile.Size = new System.Drawing.Size(356, 29);
            this.checkBoxDetectContentTypePerFile.TabIndex = 48;
            this.checkBoxDetectContentTypePerFile.Text = "Auto-detect content type (may glitch)";
            this.checkBoxDetectContentTypePerFile.UseVisualStyleBackColor = true;
            this.checkBoxDetectContentTypePerFile.CheckedChanged += new System.EventHandler(this.checkBoxDetectContentTypePerFile_CheckedChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.checkBoxDateFilterCreated);
            this.groupBox3.Controls.Add(this.dateTimePickerFilterCreated);
            this.groupBox3.Controls.Add(this.comboBoxDateFilter);
            this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox3.Location = new System.Drawing.Point(576, 180);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox3.Size = new System.Drawing.Size(573, 131);
            this.groupBox3.TabIndex = 47;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Filter by date";
            // 
            // checkBoxDateFilterCreated
            // 
            this.checkBoxDateFilterCreated.AutoSize = true;
            this.checkBoxDateFilterCreated.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.checkBoxDateFilterCreated.Location = new System.Drawing.Point(17, 61);
            this.checkBoxDateFilterCreated.Margin = new System.Windows.Forms.Padding(4);
            this.checkBoxDateFilterCreated.Name = "checkBoxDateFilterCreated";
            this.checkBoxDateFilterCreated.Size = new System.Drawing.Size(102, 28);
            this.checkBoxDateFilterCreated.TabIndex = 3;
            this.checkBoxDateFilterCreated.Text = "Created";
            this.checkBoxDateFilterCreated.UseVisualStyleBackColor = true;
            this.checkBoxDateFilterCreated.CheckedChanged += new System.EventHandler(this.checkBoxDateFilterCreated_CheckedChanged);
            // 
            // dateTimePickerFilterCreated
            // 
            this.dateTimePickerFilterCreated.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.dateTimePickerFilterCreated.Location = new System.Drawing.Point(304, 56);
            this.dateTimePickerFilterCreated.Margin = new System.Windows.Forms.Padding(4);
            this.dateTimePickerFilterCreated.Name = "dateTimePickerFilterCreated";
            this.dateTimePickerFilterCreated.Size = new System.Drawing.Size(258, 29);
            this.dateTimePickerFilterCreated.TabIndex = 2;
            this.dateTimePickerFilterCreated.ValueChanged += new System.EventHandler(this.dateTimePickerFilterCreated_ValueChanged);
            // 
            // comboBoxDateFilter
            // 
            this.comboBoxDateFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxDateFilter.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.comboBoxDateFilter.FormattingEnabled = true;
            this.comboBoxDateFilter.Location = new System.Drawing.Point(143, 56);
            this.comboBoxDateFilter.Margin = new System.Windows.Forms.Padding(4);
            this.comboBoxDateFilter.Name = "comboBoxDateFilter";
            this.comboBoxDateFilter.Size = new System.Drawing.Size(143, 32);
            this.comboBoxDateFilter.TabIndex = 1;
            this.comboBoxDateFilter.SelectedIndexChanged += new System.EventHandler(this.comboBoxDateFilter_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.radioButtonFullInstall);
            this.groupBox1.Controls.Add(this.checkBoxInstallDufOnly);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox1.Location = new System.Drawing.Point(576, 30);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(573, 143);
            this.groupBox1.TabIndex = 46;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Installation method";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.ForeColor = System.Drawing.Color.Gray;
            this.label2.Location = new System.Drawing.Point(192, 95);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(286, 24);
            this.label2.TabIndex = 2;
            this.label2.Text = "*.duf only (skip data, runtime etc.)";
            // 
            // radioButtonFullInstall
            // 
            this.radioButtonFullInstall.AutoSize = true;
            this.radioButtonFullInstall.Checked = true;
            this.radioButtonFullInstall.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.radioButtonFullInstall.Location = new System.Drawing.Point(35, 43);
            this.radioButtonFullInstall.Margin = new System.Windows.Forms.Padding(4);
            this.radioButtonFullInstall.Name = "radioButtonFullInstall";
            this.radioButtonFullInstall.Size = new System.Drawing.Size(117, 28);
            this.radioButtonFullInstall.TabIndex = 0;
            this.radioButtonFullInstall.TabStop = true;
            this.radioButtonFullInstall.Text = "Full install";
            this.radioButtonFullInstall.UseVisualStyleBackColor = true;
            // 
            // checkBoxInstallDufOnly
            // 
            this.checkBoxInstallDufOnly.AutoSize = true;
            this.checkBoxInstallDufOnly.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.checkBoxInstallDufOnly.Location = new System.Drawing.Point(35, 92);
            this.checkBoxInstallDufOnly.Margin = new System.Windows.Forms.Padding(4);
            this.checkBoxInstallDufOnly.Name = "checkBoxInstallDufOnly";
            this.checkBoxInstallDufOnly.Size = new System.Drawing.Size(136, 28);
            this.checkBoxInstallDufOnly.TabIndex = 1;
            this.checkBoxInstallDufOnly.Text = "Partial install";
            this.checkBoxInstallDufOnly.UseVisualStyleBackColor = true;
            this.checkBoxInstallDufOnly.CheckedChanged += new System.EventHandler(this.checkBoxInstallDufOnly_CheckedChanged);
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.textBoxSubfolderPrefix);
            this.panel6.Controls.Add(this.checkBoxSubfolderPrefix);
            this.panel6.Location = new System.Drawing.Point(39, 1013);
            this.panel6.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(430, 47);
            this.panel6.TabIndex = 44;
            // 
            // textBoxSubfolderPrefix
            // 
            this.textBoxSubfolderPrefix.Location = new System.Drawing.Point(325, 1);
            this.textBoxSubfolderPrefix.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBoxSubfolderPrefix.Name = "textBoxSubfolderPrefix";
            this.textBoxSubfolderPrefix.Size = new System.Drawing.Size(82, 29);
            this.textBoxSubfolderPrefix.TabIndex = 5;
            this.textBoxSubfolderPrefix.Text = "_";
            this.textBoxSubfolderPrefix.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBoxSubfolderPrefix.TextChanged += new System.EventHandler(this.textBoxSubfolderPrefix_TextChanged);
            // 
            // checkBoxSubfolderPrefix
            // 
            this.checkBoxSubfolderPrefix.AutoSize = true;
            this.checkBoxSubfolderPrefix.Checked = true;
            this.checkBoxSubfolderPrefix.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxSubfolderPrefix.Location = new System.Drawing.Point(4, 5);
            this.checkBoxSubfolderPrefix.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.checkBoxSubfolderPrefix.Name = "checkBoxSubfolderPrefix";
            this.checkBoxSubfolderPrefix.Size = new System.Drawing.Size(301, 29);
            this.checkBoxSubfolderPrefix.TabIndex = 4;
            this.checkBoxSubfolderPrefix.Text = "Prefix to append to subfolders:";
            this.checkBoxSubfolderPrefix.UseVisualStyleBackColor = true;
            this.checkBoxSubfolderPrefix.CheckedChanged += new System.EventHandler(this.checkBoxSubfolderPrefix_CheckedChanged);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.textBoxRootSubfolder);
            this.panel4.Controls.Add(this.checkBoxCreateRootSubfolder);
            this.panel4.Location = new System.Drawing.Point(39, 960);
            this.panel4.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(430, 47);
            this.panel4.TabIndex = 43;
            // 
            // textBoxRootSubfolder
            // 
            this.textBoxRootSubfolder.Enabled = false;
            this.textBoxRootSubfolder.Location = new System.Drawing.Point(242, 2);
            this.textBoxRootSubfolder.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBoxRootSubfolder.Name = "textBoxRootSubfolder";
            this.textBoxRootSubfolder.Size = new System.Drawing.Size(176, 29);
            this.textBoxRootSubfolder.TabIndex = 5;
            this.textBoxRootSubfolder.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // checkBoxCreateRootSubfolder
            // 
            this.checkBoxCreateRootSubfolder.AutoSize = true;
            this.checkBoxCreateRootSubfolder.Location = new System.Drawing.Point(4, 5);
            this.checkBoxCreateRootSubfolder.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.checkBoxCreateRootSubfolder.Name = "checkBoxCreateRootSubfolder";
            this.checkBoxCreateRootSubfolder.Size = new System.Drawing.Size(226, 29);
            this.checkBoxCreateRootSubfolder.TabIndex = 4;
            this.checkBoxCreateRootSubfolder.Text = "Create root subfolder:";
            this.checkBoxCreateRootSubfolder.UseVisualStyleBackColor = true;
            this.checkBoxCreateRootSubfolder.CheckedChanged += new System.EventHandler(this.checkBoxCreateRootSubfolder_CheckedChanged);
            // 
            // labelSpeed
            // 
            this.labelSpeed.AutoSize = true;
            this.labelSpeed.Location = new System.Drawing.Point(801, 1049);
            this.labelSpeed.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelSpeed.Name = "labelSpeed";
            this.labelSpeed.Size = new System.Drawing.Size(32, 25);
            this.labelSpeed.TabIndex = 41;
            this.labelSpeed.Text = "-/-";
            // 
            // labelTimeElapsed
            // 
            this.labelTimeElapsed.AutoSize = true;
            this.labelTimeElapsed.Location = new System.Drawing.Point(801, 1009);
            this.labelTimeElapsed.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelTimeElapsed.Name = "labelTimeElapsed";
            this.labelTimeElapsed.Size = new System.Drawing.Size(32, 25);
            this.labelTimeElapsed.TabIndex = 40;
            this.labelTimeElapsed.Text = "-/-";
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.Location = new System.Drawing.Point(716, 1049);
            this.label32.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(76, 25);
            this.label32.TabIndex = 39;
            this.label32.Text = "Speed:";
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.Location = new System.Drawing.Point(659, 1009);
            this.label31.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(136, 25);
            this.label31.TabIndex = 38;
            this.label31.Text = "Time elapsed:";
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Location = new System.Drawing.Point(973, 1033);
            this.label30.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(91, 25);
            this.label30.TabIndex = 37;
            this.label30.Text = "Threads:";
            // 
            // numericUpDownThreadsCount
            // 
            this.numericUpDownThreadsCount.Location = new System.Drawing.Point(1069, 1031);
            this.numericUpDownThreadsCount.Margin = new System.Windows.Forms.Padding(4);
            this.numericUpDownThreadsCount.Maximum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.numericUpDownThreadsCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownThreadsCount.Name = "numericUpDownThreadsCount";
            this.numericUpDownThreadsCount.Size = new System.Drawing.Size(79, 29);
            this.numericUpDownThreadsCount.TabIndex = 36;
            this.numericUpDownThreadsCount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // comboBoxContentType
            // 
            this.comboBoxContentType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxContentType.FormattingEnabled = true;
            this.comboBoxContentType.Location = new System.Drawing.Point(242, 862);
            this.comboBoxContentType.Margin = new System.Windows.Forms.Padding(4);
            this.comboBoxContentType.Name = "comboBoxContentType";
            this.comboBoxContentType.Size = new System.Drawing.Size(267, 32);
            this.comboBoxContentType.TabIndex = 30;
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(44, 868);
            this.label27.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(194, 25);
            this.label27.TabIndex = 29;
            this.label27.Text = "Content type (for all):";
            // 
            // comboBoxContentRootFolder
            // 
            this.comboBoxContentRootFolder.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxContentRootFolder.FormattingEnabled = true;
            this.comboBoxContentRootFolder.Location = new System.Drawing.Point(242, 821);
            this.comboBoxContentRootFolder.Margin = new System.Windows.Forms.Padding(4);
            this.comboBoxContentRootFolder.Name = "comboBoxContentRootFolder";
            this.comboBoxContentRootFolder.Size = new System.Drawing.Size(792, 32);
            this.comboBoxContentRootFolder.TabIndex = 28;
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(44, 824);
            this.label26.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(111, 25);
            this.label26.TabIndex = 27;
            this.label26.Text = "Root folder:";
            // 
            // buttonStart
            // 
            this.buttonStart.Location = new System.Drawing.Point(980, 940);
            this.buttonStart.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(169, 66);
            this.buttonStart.TabIndex = 8;
            this.buttonStart.Text = "Install";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // tabPageLogs
            // 
            this.tabPageLogs.Controls.Add(this.buttonReInstallAccessDenied);
            this.tabPageLogs.Controls.Add(this.listViewSkippedContent);
            this.tabPageLogs.Controls.Add(this.buttonDeleteLog);
            this.tabPageLogs.Controls.Add(this.buttonOpenLog);
            this.tabPageLogs.Controls.Add(this.checkBoxDeleteLogOnStart);
            this.tabPageLogs.Controls.Add(this.checkBoxLogDetails);
            this.tabPageLogs.Controls.Add(this.label7);
            this.tabPageLogs.Controls.Add(this.label6);
            this.tabPageLogs.Controls.Add(this.listBoxLog);
            this.tabPageLogs.Controls.Add(this.textBoxLogDetails);
            this.tabPageLogs.Location = new System.Drawing.Point(4, 33);
            this.tabPageLogs.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPageLogs.Name = "tabPageLogs";
            this.tabPageLogs.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPageLogs.Size = new System.Drawing.Size(1158, 1080);
            this.tabPageLogs.TabIndex = 2;
            this.tabPageLogs.Text = "Logs";
            this.tabPageLogs.UseVisualStyleBackColor = true;
            // 
            // buttonReInstallAccessDenied
            // 
            this.buttonReInstallAccessDenied.Location = new System.Drawing.Point(832, 4);
            this.buttonReInstallAccessDenied.Margin = new System.Windows.Forms.Padding(4);
            this.buttonReInstallAccessDenied.Name = "buttonReInstallAccessDenied";
            this.buttonReInstallAccessDenied.Size = new System.Drawing.Size(282, 44);
            this.buttonReInstallAccessDenied.TabIndex = 18;
            this.buttonReInstallAccessDenied.Text = "Re-install \"access denied\"";
            this.buttonReInstallAccessDenied.UseVisualStyleBackColor = true;
            this.buttonReInstallAccessDenied.Click += new System.EventHandler(this.buttonReInstallAccessDenied_Click);
            // 
            // listViewSkippedContent
            // 
            this.listViewSkippedContent.AllowDrop = true;
            this.listViewSkippedContent.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.listViewSkippedContent.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.listViewSkippedContent.FullRowSelect = true;
            this.listViewSkippedContent.GridLines = true;
            this.listViewSkippedContent.HideSelection = false;
            this.listViewSkippedContent.Location = new System.Drawing.Point(24, 53);
            this.listViewSkippedContent.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.listViewSkippedContent.MultiSelect = false;
            this.listViewSkippedContent.Name = "listViewSkippedContent";
            this.listViewSkippedContent.Size = new System.Drawing.Size(1089, 339);
            this.listViewSkippedContent.TabIndex = 17;
            this.listViewSkippedContent.UseCompatibleStateImageBehavior = false;
            this.listViewSkippedContent.View = System.Windows.Forms.View.Details;
            this.listViewSkippedContent.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listViewSkippedContent_ColumnClick);
            this.listViewSkippedContent.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listViewSkippedContent_MouseDoubleClick);
            this.listViewSkippedContent.MouseUp += new System.Windows.Forms.MouseEventHandler(this.listViewSkippedContent_MouseUp);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Archive";
            this.columnHeader1.Width = 700;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Reason";
            this.columnHeader2.Width = 150;
            // 
            // buttonDeleteLog
            // 
            this.buttonDeleteLog.Location = new System.Drawing.Point(966, 415);
            this.buttonDeleteLog.Margin = new System.Windows.Forms.Padding(4);
            this.buttonDeleteLog.Name = "buttonDeleteLog";
            this.buttonDeleteLog.Size = new System.Drawing.Size(163, 44);
            this.buttonDeleteLog.TabIndex = 8;
            this.buttonDeleteLog.Text = "Clear log now";
            this.buttonDeleteLog.UseVisualStyleBackColor = true;
            this.buttonDeleteLog.Click += new System.EventHandler(this.buttonDeleteLog_Click);
            // 
            // buttonOpenLog
            // 
            this.buttonOpenLog.Location = new System.Drawing.Point(111, 413);
            this.buttonOpenLog.Margin = new System.Windows.Forms.Padding(4);
            this.buttonOpenLog.Name = "buttonOpenLog";
            this.buttonOpenLog.Size = new System.Drawing.Size(163, 44);
            this.buttonOpenLog.TabIndex = 7;
            this.buttonOpenLog.Text = "Open log";
            this.buttonOpenLog.UseVisualStyleBackColor = true;
            this.buttonOpenLog.Click += new System.EventHandler(this.buttonOpenLog_Click);
            // 
            // checkBoxDeleteLogOnStart
            // 
            this.checkBoxDeleteLogOnStart.AutoSize = true;
            this.checkBoxDeleteLogOnStart.Location = new System.Drawing.Point(705, 424);
            this.checkBoxDeleteLogOnStart.Margin = new System.Windows.Forms.Padding(4);
            this.checkBoxDeleteLogOnStart.Name = "checkBoxDeleteLogOnStart";
            this.checkBoxDeleteLogOnStart.Size = new System.Drawing.Size(223, 29);
            this.checkBoxDeleteLogOnStart.TabIndex = 6;
            this.checkBoxDeleteLogOnStart.Text = "Clear log on app start";
            this.checkBoxDeleteLogOnStart.UseVisualStyleBackColor = true;
            // 
            // checkBoxLogDetails
            // 
            this.checkBoxLogDetails.AutoSize = true;
            this.checkBoxLogDetails.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.checkBoxLogDetails.Location = new System.Drawing.Point(24, 960);
            this.checkBoxLogDetails.Margin = new System.Windows.Forms.Padding(4);
            this.checkBoxLogDetails.Name = "checkBoxLogDetails";
            this.checkBoxLogDetails.Size = new System.Drawing.Size(178, 34);
            this.checkBoxLogDetails.TabIndex = 4;
            this.checkBoxLogDetails.Text = "Log line details";
            this.checkBoxLogDetails.UseVisualStyleBackColor = true;
            this.checkBoxLogDetails.CheckedChanged += new System.EventHandler(this.checkBoxLogDetails_CheckedChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label7.Location = new System.Drawing.Point(18, 418);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(50, 30);
            this.label7.TabIndex = 2;
            this.label7.Text = "Log";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label6.Location = new System.Drawing.Point(18, 18);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(178, 30);
            this.label6.TabIndex = 1;
            this.label6.Text = "Skipped archives";
            // 
            // listBoxLog
            // 
            this.listBoxLog.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.listBoxLog.FormattingEnabled = true;
            this.listBoxLog.HorizontalScrollbar = true;
            this.listBoxLog.ItemHeight = 25;
            this.listBoxLog.Location = new System.Drawing.Point(24, 468);
            this.listBoxLog.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.listBoxLog.Name = "listBoxLog";
            this.listBoxLog.ScrollAlwaysVisible = true;
            this.listBoxLog.Size = new System.Drawing.Size(1103, 479);
            this.listBoxLog.TabIndex = 0;
            this.listBoxLog.MouseUp += new System.Windows.Forms.MouseEventHandler(this.listBoxLog_MouseUp);
            // 
            // textBoxLogDetails
            // 
            this.textBoxLogDetails.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.textBoxLogDetails.Location = new System.Drawing.Point(24, 468);
            this.textBoxLogDetails.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxLogDetails.Multiline = true;
            this.textBoxLogDetails.Name = "textBoxLogDetails";
            this.textBoxLogDetails.Size = new System.Drawing.Size(1103, 484);
            this.textBoxLogDetails.TabIndex = 5;
            this.textBoxLogDetails.Visible = false;
            // 
            // contextMenuCommon
            // 
            this.contextMenuCommon.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.contextMenuCommon.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToolStripMenuItem,
            this.copyAllToolStripMenuItem});
            this.contextMenuCommon.Name = "contextMenuStrip1";
            this.contextMenuCommon.Size = new System.Drawing.Size(164, 76);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(163, 36);
            this.copyToolStripMenuItem.Text = "Copy";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // copyAllToolStripMenuItem
            // 
            this.copyAllToolStripMenuItem.Name = "copyAllToolStripMenuItem";
            this.copyAllToolStripMenuItem.Size = new System.Drawing.Size(163, 36);
            this.copyAllToolStripMenuItem.Text = "Copy All";
            this.copyAllToolStripMenuItem.Click += new System.EventHandler(this.copyAllToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1192, 1151);
            this.Controls.Add(this.tabControl);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Super DAZ Content Install Manager";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabControl.ResumeLayout(false);
            this.tabPageSettings.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panelLanguage.ResumeLayout(false);
            this.panelLanguage.PerformLayout();
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tabPageSort.ResumeLayout(false);
            this.tabPageSort.PerformLayout();
            this.panelSubfolder.ResumeLayout(false);
            this.panelSubfolder.PerformLayout();
            this.panelSubfolderAutoDetect.ResumeLayout(false);
            this.panelSubfolderAutoDetect.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel9.ResumeLayout(false);
            this.panel9.PerformLayout();
            this.tabPageInstaller.ResumeLayout(false);
            this.tabPageInstaller.PerformLayout();
            this.panel8.ResumeLayout(false);
            this.panel8.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radioButtonInstallLibs)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownThreadsCount)).EndInit();
            this.tabPageLogs.ResumeLayout(false);
            this.tabPageLogs.PerformLayout();
            this.contextMenuCommon.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonAddLibraryPath;
        private System.Windows.Forms.TabPage tabPageSettings;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TabPage tabPageInstaller;
        private System.Windows.Forms.Button buttonAddContentFolder;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.Button buttonRemoveLibraryPath;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.ColumnHeader columnName;
        private System.Windows.Forms.ColumnHeader columnPath;
        private System.Windows.Forms.ColumnHeader columnProgress;
        private System.Windows.Forms.ColumnHeader columnSubfolder;
        public System.Windows.Forms.ListView listViewContent;
        private System.Windows.Forms.Label label5;
        public System.Windows.Forms.CheckBox checkBoxGenesis9;
        public System.Windows.Forms.CheckBox checkBoxGenesis8;
        public System.Windows.Forms.CheckBox checkBoxGenesis3;
        private System.Windows.Forms.TabPage tabPageLogs;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.RadioButton radioButtonTempSameAsInstall;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Panel panel2;
        public System.Windows.Forms.TextBox textBoxTempFolder;
        public System.Windows.Forms.RadioButton radioButtonTempIsSpecific;
        private System.Windows.Forms.Button buttonSelectTempFolder;
        private System.Windows.Forms.TabPage tabPageSort;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Panel panelSubfolder;
        private System.Windows.Forms.Panel panel5;
        public System.Windows.Forms.CheckedListBox checkedListBoxSubfolderBlock;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label9;
        public System.Windows.Forms.RadioButton radioButtonSpecificSubfolder;
        public System.Windows.Forms.TextBox textBoxSubfolderForced;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label14;
        public System.Windows.Forms.CheckedListBox checkedListBoxSearchLibs;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Panel panelLanguage;
        private System.Windows.Forms.RadioButton radioButtonLangEnglish;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Label label20;
        public System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.Panel panel9;
        public System.Windows.Forms.RadioButton radioButtonAutoDetectSubfolder;
        public System.Windows.Forms.CheckBox checkBoxSearchSubfolder;
        private System.Windows.Forms.Panel panelSubfolderAutoDetect;
        public System.Windows.Forms.CheckBox checkBoxUseSubfolder;
        public System.Windows.Forms.CheckBox checkBoxSubfolderAsZipLocation;
        public System.Windows.Forms.RadioButton radioButtonPrioritySubfolderZipLocation;
        public System.Windows.Forms.RadioButton radioButtonPrioritySubfolderOldLibrary;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label24;
        public System.Windows.Forms.RadioButton radioButtonLangRussian;
        public System.Windows.Forms.ListBox listBoxLog;
        private System.Windows.Forms.ComboBox comboBoxContentRootFolder;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.ContextMenuStrip contextMenuCommon;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.TextBox textBoxLogDetails;
        private System.Windows.Forms.CheckBox checkBoxLogDetails;
        private System.Windows.Forms.ComboBox comboBoxContentType;
        private System.Windows.Forms.Label label27;
        internal System.Windows.Forms.Label labelProgress;
        internal System.Windows.Forms.CheckBox checkBoxDeleteLogOnStart;
        private System.Windows.Forms.Button buttonOpenLog;
        private System.Windows.Forms.Label label30;
        internal System.Windows.Forms.NumericUpDown numericUpDownThreadsCount;
        private System.Windows.Forms.Label labelSpeed;
        private System.Windows.Forms.Label labelTimeElapsed;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.Button buttonDeleteLog;
        internal System.Windows.Forms.ListView listViewSkippedContent;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ToolStripMenuItem copyAllToolStripMenuItem;
        private System.Windows.Forms.Button buttonClear;
        private System.Windows.Forms.Button buttonReInstallAccessDenied;
        private System.Windows.Forms.Panel panel6;
        public System.Windows.Forms.TextBox textBoxSubfolderPrefix;
        public System.Windows.Forms.CheckBox checkBoxSubfolderPrefix;
        private System.Windows.Forms.Panel panel4;
        public System.Windows.Forms.TextBox textBoxRootSubfolder;
        public System.Windows.Forms.CheckBox checkBoxCreateRootSubfolder;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButtonFullInstall;
        internal System.Windows.Forms.RadioButton checkBoxInstallDufOnly;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.ComboBox comboBoxGender;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.ComboBox comboBoxGeneration;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox checkBoxDateFilterCreated;
        internal System.Windows.Forms.ComboBox comboBoxDateFilter;
        internal System.Windows.Forms.DateTimePicker dateTimePickerFilterCreated;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label2;
        internal System.Windows.Forms.CheckBox checkBoxOverwrite;
        internal System.Windows.Forms.CheckBox checkBoxDetectContentTypePerFile;
        internal EWSoftware.ListControls.RadioButtonList radioButtonInstallLibs;
        internal System.Windows.Forms.CheckedListBox checkedListBoxLibraryDirs;
        private System.Windows.Forms.CheckBox checkBoxIgnoreRootFolder;
    }
}

