# SuperDIM — заметки для Claude

Windows-приложение (WinForms, .NET Framework 4.8, C#) для массовой установки контент-архивов DAZ Studio
(zip / 7z / rar) в структуру Content Library. Распаковка делегируется внешним утилитам `7z.exe` (полный 7-Zip, умеет и rar) и `UnRAR.exe`.
Лицензия GPL-3.0; репозиторий: https://github.com/dragokas/SuperDIM (issues нет; единственный TODO из README —
режим Full install может работать некорректно). Описание для пользователей — в `README.md`. Здесь — только то, что нужно для работы с кодом.
`7z.exe`+`7z.dll`/`UnRAR.exe` — сторонние официальные бинарники (7-Zip, WinRAR), их не изменяем.

## Сборка и запуск

Решение: `source/DazUnpacker.slnx` (новый XML-формат, нужен VS 2022 17.13+ / MSBuild из VS или dotnet SDK 9+), проект `source/DazUnpacker.csproj` (SDK-style: `Microsoft.NET.Sdk`, `net48`, `UseWindowsForms`).
Корневой namespace `DazUnpacker`, имя сборки `SuperDIM`, точка входа `Program.Main`.

- Рабочая команда (проверена, Debug), из каталога `source/` (`-restore` вызывается сам):

```powershell
dotnet build DazUnpacker.slnx --configuration Debug '-p:Platform=Any CPU' -nologo -v:m
```

- Полный MSBuild из VS тоже работает: `& 'C:\Program Files\Microsoft Visual Studio\18\Community\MSBuild\Current\Bin\MSBuild.exe' DazUnpacker.slnx -p:Configuration=Debug '-p:Platform=Any CPU' -restore -nologo -v:m`.
- Единственный не-строковый ресурс — иконка формы `$this.Icon` в `Form1.resx`. Чтобы её собирал `dotnet build` (иначе `MSB3822/MSB3823`), в csproj заданы
  `GenerateResourceUsePreserializedResources=true` и пакет `System.Resources.Extensions`: он нужен и в рантайме (+ его зависимости `System.Memory`, `System.Buffers`,
  `System.Numerics.Vectors`, `System.Runtime.CompilerServices.Unsafe` лежат рядом с exe и в zip; binding redirect пишется в `SuperDIM.exe.config` автоматически).

- Результат: `source/bin/Debug/SuperDIM.exe` (`AppendTargetFrameworkToOutputPath=false` — без подпапки `net48`). Папка `tools/` копируется в выход автоматически
  (`<Content Include="tools\**">`).
- NuGet (`PackageReference` в csproj, `packages.config` нет): `EWSoftware.ListControls` (кастомный список-радиокнопки `radioButtonInstallLibs`), `Newtonsoft.Json` (конфиг),
  `System.Data.SQLite.Core` (БД каталога; нативные `x86\`/`x64\SQLite.Interop.dll` копирует `.targets` транзитивного пакета `Stub.System.Data.SQLite.Core.NetFramework`).
  Пакеты восстанавливает `-restore` в глобальный кэш NuGet (нужен доступ к nuget.org). Старая папка `source/packages/` не используется (исключена из сборки
  через `DefaultItemExcludes`, в git не лежит) — её можно удалить.
- Версия и описание сборки (`Version`, `AssemblyTitle`, `Product`, `Copyright`) заданы в csproj; `AssemblyInfo.cs` нет.
- Тестов в проекте нет. `source/_publish.bat` собирает Release (`dotnet build`) и пакует zip (без `config.txt`, `catalog.db*` и `app.log`);
  существующий `bin\Release` на время сборки откладывается в `bin\Release.bak` и возвращается после. Проверять bat нужно на копии репозитория, не на рабочем каталоге.
- Релизная сборка блокируется запущенным Release-процессом пользователя — всегда собирай Debug (см. глобальные правила).

## Структура репозитория

```
README.md, LICENSE, .gitignore
source/
  DazUnpacker.slnx / .csproj    проект SDK-style: все *.cs и *.resx подхватываются по маске, в csproj их перечислять не нужно
  Properties/                   Resources, Settings (Settings пустой)
  source/                       весь код приложения (папка называется "source" внутри "source")
  tools/7z/{7z.exe,7z.dll}, tools/WinRar/UnRAR.exe   внешние распаковщики
  dci.ico, _publish.bat
```

Артефакты рядом с exe при работе: `config.txt` (настройки, JSON), `app.log`, `catalog.db` (SQLite, + `-wal`/`-shm`).

## Архитектура

Один главный `Form1` (partial: `Form1.cs` + `Form1.Designer.cs`, вкладки Settings, Installer, Catalog, Logs) и набор **статических классов-фасадов**,
каждый из которых получает ссылку на форму через `Init(Form1)` и работает с её контролами напрямую.
Слоёв «модель / представление» нет: состояние живёт в контролах формы и в `Config.Storage`.

### Поток установки (главный сценарий)

```
Drag&Drop / "Select Folder"  ->  Content.AddObject/AddFolder/AddFile  ->  listViewContent (Library.Content)
    (фильтр по дате применяется уже на добавлении; перед добавлением `Form1` вызывает `Config.SaveFiltersFromForm()`, иначе берётся устаревшее сохранённое состояние)
buttonStart_Click  ->  собирает Unpacker.UnpackInfo из контролов  ->  новый Thread -> Unpacker.UnpackAll
    Parallel.ForEach(архивы, MaxDegreeOfParallelism = threadsCount):
      1. определить ContentType (по имени архива: DetectContentType, либо общий тип из UI)
      2. ранний фильтр по типу (FilterManager.IsPassed(ContentTypes)) -> Skipped(CriteriaNotMatch)
      3. Archive.ExtractSmart -> сначала отсев по имени без распаковки (NotArchive, BlackName, фильтр поколений),
         затем распаковка во временную папку <диск библиотеки>\Temp\Daz<N> + рекурсивно вложенные архивы
         + поиск "top-директорий" с *.duf + определение Genesis-версии/пола
      4. если тип всё ещё Unknown — определить по top-директории; повторный фильтр по типу
      5. для каждой версии Genesis (3/8/9) из фильтра: InstallDuf -> целевой путь в библиотеке
      6. Full install: InstallRoot — копирует остальные корневые папки (data, Runtime…) в библиотеку
      7. Statistics.DoneIncrement() в finally -> обновление прогресса на форме
```

Целевой путь для .duf: `<Library>\People\<Genesis X [Male|Female]>\<Clothing|Hair|Poses|…>\[rootFolderName]\<dirGroup>\<имя top-папки>`
(`Unpacker.GetInstallDirByContentType`; у Genesis 9 папка без Male/Female). `dirGroup` — относительный путь архива от «Root folder» (зеркалирование иерархии),
к каждому компоненту может добавляться префикс. Environment/Light/Prop/Script — «version-independent»
(`Content.IsVersionIndependentType`): цикл по версиям Genesis прерывается после первой версии, разрешённой фильтром (ставится один раз). Environment/Light/Script
кладутся вне папки Genesis (`Environments`, `Light Presets`, `scripts`); Prop — в `People\<Genesis …>\Props` первой разрешённой фильтром версии (порядок 3, 8, 9).

### Модули (`source/source/`)

| Файл | Роль |
|---|---|
| `Program.cs` | Именованный Mutex `SuperDIM_mtx` (защита от двойного запуска), `Application.Run(new Form1())` |
| `Form1.cs` / `.Designer.cs` | UI и обработчики; вкладки: Settings, Installer, Catalog, Logs. Сборка `UnpackInfo`, запуск потока, drag&drop (рекурсивно навешивается на всё `tabPageInstaller`), контекстное меню копирования (Copy / Copy All для `listBoxLog` и `listViewSkippedContent`), «Re-install access denied» (после добавления архивов переходит на вкладку Logs и сам жмёт Install) |
| `Unpacker.cs` | Оркестрация установки (`UnpackAll`), `UnpackInfo`, выбор целевой папки, эвристики: `DetectContentType` (подстроки в пути), `IsGenesisX`/`GetGenesisVersionsByName` (версия/пол по имени), `IsGenesisFilterAllowed` (учёт фильтра поколений) |
| `Archive.cs` | Работа с архивами: `ExtractSmart` (ядро), `ExtractSingle(Zip/Rar)` — запуск `7z.exe`/`UnRAR.exe` с проверкой кода возврата, `GetDufTopFolders`/`ConcatenateSibilFolders` (какие папки считать «корнем контента»), `FindRootDir` по списку `DazRootDirs`, `IsBlackName` (отсев Poser/UVM/…Ps), `PrepareTempDir`. Enum `FindFailReason`, класс `DufRootInfo` |
| `Content.cs` | Enum'ы `GenesisVersion` ([Flags]), `GenesisGenders`, `ContentTypes`; добавление файлов в список (`AddFile/AddFolder/AddObject`), `GetParentFolders` для combobox «Root folder» |
| `Library.cs` | Статические вложенные фасады над контролами: `Content` (список архивов и колонка Status), `Basic` (DAZ-библиотеки), `Install` (выбранная целевая библиотека), `Skipped` (список пропущенных + причина; потокобезопасное добавление через `BeginInvoke`) |
| `Config.cs` | `Config.Storage` (`ConfigStorage` → `_TabSettings/_TabInstaller/_TabCatalog/_TabLog`) — сериализуемые настройки; `Config.Data` — пути к `7z.exe`/`UnRAR.exe` и `%Temp%\Daz`; `Init` (`LoadCommonData`, `LoadDefaults` — заполняет комбобоксы, `LoadStorage`, `ApplyStorage` форма←конфиг), `SaveConfig` форма→конфиг (вызывается в `FormClosing`), `LoadFiltersOnForm`/`SaveFiltersFromForm`/`GetGenesisVersionFilter` |
| `FilterManager.cs` | Фильтры установки: `IFilter`, `DateFilter` (Before/After/Equal по CreationTime архива, сравнивается только дата без времени), `ContentFilter` (один выбранный `ContentTypes`), `FilterManager` (флаги включения + список). Полиморфно сериализуется Json.NET (`TypeNameHandling.Auto`) |
| `FileSys.cs` | Файловые хелперы: `CopyFolderRecursively` (+`FsCopyStatus`), `FileDelete` (снимает ReadOnly), `DirectoryDelete` (не бросает исключений), `EnumDirectory`, `GetParentDir`, `PathInsertPrefixToAllComponents`, `PathInsertPostfix`, `StripFilename`, `Unquote`, `FindFileNameAvailable`/`FindDirNameAvailable` |
| `Statistics.cs` | Счётчики `Total/Done` (Interlocked) и вывод в `labelProgress` через `BeginInvoke` |
| `Logger.cs` | Статический `Log` (Info/Warning/Error/Trace), вывод в `app.log` и `listBoxLog`; потокобезопасный, `DisableCache(true)` => запись сразу |
| `Globals.cs` | `AppDir`, `AppName`, `ReportError(ex, interactive, context)` (лог; MessageBox только при `interactive = true`), `FormatException`, `StringContains` (ignore-case), `OpenTextFile`, `ClearTempFolder` (не вызывается) |
| `ArchiveLister.cs` | Чтение содержимого архива через `7z.exe`: `ListFiles` (`7z l -slt -ba`, только оглавление, без распаковки), `ExtractNestedArchives` (`7z x -r -i!*.zip -i!*.rar -i!*.7z` — только вложенные архивы), `FindArchives` |
| `CatalogDb.cs` | SQLite-слой каталога (`catalog.db`): таблица `Files`, `ReplaceBundle` (атомарная замена строк бандла), `QueryIds` (фильтр/сортировка), `GetRows`, `FindByTails`, `ImportRows`, `Clear`. Класс `CatalogRow` (6 полей), `CatalogColumn` |
| `CatalogScanner.cs` | `CatalogScanner` — Scan папки (потоки `Parallel.ForEach`, вложенные архивы рекурсивно); `CatalogCollector` — сбор листинга во время установки (хук в `Archive.ExtractSmart`) |
| `CatalogRowSource.cs` | `ICatalogRowSource` для виртуального грида: `DbRowSource` (Id из БД + страницы по 100 строк, фоновые запросы) и `ListRowSource` (список в памяти) |
| `CsvHelper.cs` | Экспорт/импорт каталога в CSV (UTF-8 BOM, RFC 4180) |
| `Form1Catalog*.cs` | Вкладка Catalog: `Form1Catalog` (форма `TopLevel=false`, встроена в `tabPageCatalog`), `Form1CatalogGrid` (UserControl: виртуальный DataGridView, Copy, сортировка по заголовку), `Form1CatalogSearch` (ввод путей), `Form1CatalogResult` (3 списка результата) |
| `FolderPicker.cs` | Диалог выбора папки через COM (IFileOpenDialog) |
| `Geometry.cs` | Поиск контрола под курсором (для контекстного меню Copy) |
| `ListViewColumnSorter.cs` | `IComparer` для сортировки `listViewSkippedContent` (глобальный класс без namespace) |

### Каталог файлов в архивах (вкладка Catalog)

- **Бандл** — архив на диске (его путь `ArchivePath` хранится в БД и именно он добавляется в список установки). Внутри могут лежать другие архивы
  (любой глубины, `MaxNestingDepth = 8`). В БД попадают файлы всех уровней; вложенные архивы-контейнеры как строки не пишутся, кроме случая,
  когда сканер не смог их открыть (или превышена глубина) — тогда они остаются обычными строками. Хук установщика вложенный архив в строки не пишет никогда
  (его содержимое листится отдельным вызовом `catalog.Add`).
- `FilePath` — путь внутри (листового) архива **как есть**, с `/` и ведущим `/` (например, `/Content/data/Daz 3D/...`, `/Pinme-for-.../Studio/My Library/Presets/Poses/...`).
  Корень библиотеки DAZ не вычисляется. Массовый поиск (Search & Install) — по **хвосту** пути; фильтр на вкладке — по **вхождению**.
- **Ignore case** (чекбокс, по умолчанию выкл): выкл — всё регистрозависимо; вкл — полностью регистронезависимо, включая кириллицу.
  Для этого в таблице есть технические колонки `PathKey`/`NameKey` (нижний регистр, `ToLowerInvariant`) — в UI и CSV не показываются, при импорте CSV пересчитываются.
- Листинг бандла не распаковывает его целиком: `7z l -slt -ba`; на диск (в `<temp>\Catalog\<guid>`) попадают только вложенные архивы и только если они есть;
  папка удаляется сразу после листинга. Temp — «Specific folder» из groupBox6 (если режим включён и папка существует), иначе `Config.Data.TempFolder`.
  `7z.exe` читает rar только в полной версии 7-Zip (`7za.exe` формата RAR не содержит).
- **Повторный Scan пропускает неизменённые бандлы:** таблица `Bundles` (`ArchivePath`, `Size`, `ModifiedTicks` = `LastWriteTimeUtc`) пишется в той же транзакции, что и строки
  (`ReplaceBundle(path, rows, BundleStamp)`; штамп снимается **до** чтения архива). Совпали размер и время — бандл не открывается, статус показывает число пропущенных.
  Штамп пишет и хук установщика. Импорт CSV штамп бандла удаляет (бандл пересканируется), Clear очищает обе таблицы. Принудительного «Rescan all» нет (по решению пользователя).
- Хук установщика: `Archive.ExtractSmart(..., CatalogCollector)` листит внешний и каждый вложенный архив **перед** распаковкой (вложенный затем удаляется);
  `Unpacker.UnpackAll` пишет результат в БД, ошибки каталога установку не ломают. Так список файлов полный и в режиме Partial (где `data`/`Runtime` не распаковываются).
- БД: один писатель (общий `lock`), WAL; чтение — отдельными соединениями. Грид виртуальный: `DbRowSource` держит `long[] ids` (результат фильтра/сортировки)
  и подгружает строки страницами по Id. Фильтр запускается через 1 с после ввода (`Timer`), устаревшие запросы отбрасываются по номеру.
- **Unique archive** (чекбокс): в гриде только первая (по `Id`) подходящая строка каждого бандла — `WHERE Id IN (SELECT MIN(Id) ... GROUP BY ArchivePath)`; работает вместе с фильтром и сортировкой.
- Search & Install: `Form1.ReplaceInstallList` (очистка списка установки + `AddFile` только для существующих на диске бандлов, прошедших фильтр даты, + переход на вкладку Installer).
  Если на диске нет ни одного бандла — список установки не трогается (проверка в `Form1Catalog` до вызова: `ReplaceInstallList` сам список всегда очищает).

### Конфигурация

- `config.txt` рядом с exe, Json.NET, `Formatting.Indented`. Читается в `Config.Init` (конструктор формы), пишется в `Form1_FormClosing` через `config.txt.tmp` + `File.Replace`.
  Пустой или повреждённый файл не мешает запуску: копия остаётся в `config.txt.bad`, берутся значения по умолчанию, причина в `Config.LoadWarning` (в лог попадает в `Form1_Load`).
  Секции `null` заменяются пустыми (`FillMissingSections`), значения вне диапазона (потоки, номер вкладки, дата фильтра) приводятся к допустимым.
- Пути к `7z.exe` и `UnRAR.exe` вычисляются от каталога сборки: `tools\7z\7z.exe`, `tools\WinRAR\UnRAR.exe`
  (в git каталог называется `WinRar` — регистр не важен на Windows).
- Режимы установки: **Partial** (`InstallDufOnly = true`, по умолчанию — при распаковке пропускаются `data`, `Runtime`, `Documentation`, `Documents`, `Readme`, `ReadMe's`,
  `Templates`, `Dokumentation`, список в `ExtractSmart`) и **Full** (копируются и остальные корневые папки из `DazRootDirs`).

## Подводные камни и особенности (важно при доработке)

- **Нет разделения UI и логики.** Статические классы напрямую читают контролы (`Library.*`, `Config.SaveConfig`, `Content.AddFile`).
  Новые настройки добавляются в три места: контрол в дизайнере → поле в `Config.Storage` → `ApplyStorage` + `SaveConfig`.
- **Статический глобальный `Config.Storage` читается из рабочих потоков** `Parallel.ForEach` (фильтры, `genesisVersionFilter`) —
  значения для установки перед стартом синхронизируются из формы (`UpdateGenesisTypeFilter`, `SaveFiltersFromForm` в `buttonStart_Click`).
- **Завершение установки:** поток установки (`Form1.RunInstall`) в `finally` сам включает кнопку Install через `BeginInvoke(OnInstallFinished)`, что бы ни случилось
  (пустой список, сбой `PrepareTempDir`, исключение). `labelProgress_TextChanged` только показывает время, скорость и прогресс (по `Statistics.Done`/`Total`) и останавливает
  секундомер. Пропущенные на этапе `FilterFilesList` архивы тоже считаются через `DoneIncrement`.
- **Временные папки:** установщик (`Unpacker.UnpackAll` → `Archive.PrepareTempDir`) всегда распаковывает в `<корень диска целевой библиотеки>\Temp\Daz<N>`
  (N — номер архива в этом запуске; пустая папка `Temp\Daz` тоже создаётся; вложенные архивы — в `Daz<N>\Daz<M>`; так `Directory.Move` остаётся переименованием,
  а не копированием). Папка `Daz<N>` удаляется в `finally` лямбды `UnpackAll` при любом исходе (в том числе `ContentTypeUndefined`, `CriteriaNotMatch`, исключение);
  `FileSys.DirectoryDelete` при ошибке снимает ReadOnly со всего дерева и повторяет удаление. Установщик настройку временной папки не читает. Поэтому «Specific folder» на вкладке Settings
  (`groupBox6`, `TabSettings.UseSpecificTempFolder/TempFolder`) на установку не влияет, хотя форма проверяет путь при уходе с вкладки (`CheckTempFolderForValidInput`).
  Эта настройка действует только в сканировании каталога (`CatalogScanner`, ~строка 84): указанная папка, если режим включён и она существует,
  иначе `Config.Data.TempFolder` (`%Temp%\Daz`). Не вызываются нигде (заготовки очистки): `Globals.ClearTempFolder`, `Archive.ClearTemp`. По решению пользователя код и интерфейс не менялись.
- **Вкладка Sort удалена** (вместе с `_TabSort`, `Exclusion`, `Library.Search/Exclude`). Её настройки установка не использовала. Живые поля «Prefix to append to subfolders»
  и «Create root subfolder» стоят на вкладке Installer (панели `panel6`/`panel4`) и хранятся в `_TabInstaller` (`UsePrefix/PrefixName/CreateRootSubfolder/RootSubfolder`).
  Старый ключ `TabSort` в `config.txt` Json.NET игнорирует. Остальные наследия: `UseLanguageRu` (переключатель языка скрыт `panelLanguage.Visible = false`), `GarbageLibraries`.
- **Эвристики определения типа/версии хрупкие** (подстроки в пути архива/папки): `DetectContentType` (порядок `else if` важен —
  например, «Hair» vs «Chair», «Light» ловит «IRay»), `IsGenesisX`, `IsBlackName`, `GetDufTopFolders` (костыли для `Iray` и `\data\`), `InstallDuf` (папка `Characters`).
  Любая правка должна учитывать порядок проверок и регрессии на реальных архивах — автотестов нет.
  `GenesisVersion.Genesis_8_1` есть в enum и в `GetInstallDirByContentType`, но определение версии его не выставляет: имена с «8.1» / «81» / «8_1» считаются Genesis 8.
- **Вложенные архивы:** `ExtractSmart` в цикле распаковывает найденные внутри архивы в `destDir\Daz<N>`, удаляя исходный вложенный архив,
  и собирает общий список `dufRootInfo`/`rootDirs`.
- **Коды возврата распаковщиков проверяются** (`Archive.RunExtractor`, запуск через общий `ArchiveLister.RunTool`, stdin закрыт — запрос пароля не вешает поток):
  0 — успех, 1 — предупреждение (`Log.Warning`, установка идёт дальше), 10 у UnRAR — «нечего распаковывать» (при Partial всё могло быть исключено), остальное — ошибка
  (`Log.Error` с кодом и хвостом вывода). Ошибка внешнего архива → `ExtractSmart` возвращает `FindFailReason.ArchiverError`; ошибка вложенного → он пропускается,
  остальное ставится, `archiverFailed = true`. Вывод архиваторов урезан ключами `-bso0 -bsp0` (7z) и `-idq` (UnRAR).
- **Колонка Status** списка архивов (`listViewContent`, 3-я колонка): итог по бандлу пишет `Unpacker.UnpackAll` через `Library.Content.SetStatus` (потокобезопасно; перед стартом
  `PrepareStatuses` очищает колонку и индексирует элементы по пути). Значения: `Success`; `FAIL` (`ArchiverError`, `Unknown`, исключение); иначе имя `FindFailReason`
  (`AccessDenied`, `AlreadyInstalled`, `BlackName`, `NoDufFiles`, `CriteriaNotMatch`, `Genesis9` …). Приоритет: FAIL > AccessDenied > Success > AlreadyInstalled.
  Распакованные файлы нередко имеют атрибут ReadOnly — удалять их через `FileSys.FileDelete`, а не `File.Delete`.
- **Ошибки в рабочих потоках** гасятся: `Globals.ReportError(ex, false)` только пишет в лог. `AccessDenied` помечается в `Library.Skipped`
  и позволяет кнопку «Re-install access denied» (повторно добавляет такие архивы и жмёт Install).
- **Размер окна:** `Form1` изменяемого размера (`MinimumSize` = размер дизайна), контролы вкладок Installer/Logs расставлены якорями (Anchor) в дизайнере,
  Catalog растягивается через Dock/TableLayoutPanel. Разворачивание на весь экран — только системной кнопкой окна (собственного full screen нет, по просьбе пользователя).
- **Вкладки:** порядок Settings, Installer, Catalog, Logs. Installer и Logs добавляются/убираются в `UpdateTabs` (когда есть хотя бы одна библиотека),
  Catalog есть всегда. Вкладки выбирай по ссылке (`tabControl.SelectedTab = tabPageLogs`), а не по индексу; `Config.TabNumber` хранит индекс,
  поэтому после добавления Catalog старый сохранённый индекс может один раз открыть соседнюю вкладку.
- **Локализация:** UI на английском, русский вариант не реализован. Комментарии в коде — английские.
- **Кодировка исходников:** `*.cs` и `.csproj` — UTF-8 с BOM, CRLF; `.slnx` — UTF-8 без BOM; `_publish.bat` — без BOM, CRLF (перед правкой полагаться на хуки File Guard, см. глобальные правила).

## Рабочие соглашения для этого проекта

- Общие правила стиля/git/сборки берутся из глобального `~/.claude/CLAUDE.md` (фигурные скобки везде, комментарии на английском,
  один оператор на строку, коммит локально после каждой правки, push только по явной просьбе).
- Новый `.cs` файл в csproj добавлять не нужно (SDK-style подхватывает `**/*.cs`). Исключение — `DependentUpon` для вложенности в Solution Explorer и
  `.resx` формы: у `Form1.resx` оно задано в csproj явно (от него зависит имя ресурса `DazUnpacker.Form1.resources`; у новой формы с `.resx` проверяй имя ресурса).
- Проверка UI без ручного запуска: копию `bin\Debug` запускать из scratchpad (чужие/пользовательские процессы SuperDIM не трогать — завершать только по своему PID),
  управлять через UI Automation (элементы ищутся по `Name`, перед кликом окно нужно активировать, `Microsoft.VisualBasic.Interaction.AppActivate`).
- Если логика новой функции > ~500 строк — выносить в отдельную форму/класс с именем, начинающимся на `Form1…` (правило из глобальных инструкций).
- Прежде чем добавлять код, проверяй существующие хелперы: `FileSys`, `Globals`, `Library.*`, `FilterManager`.
