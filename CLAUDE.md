# SuperDIM — заметки для Claude

Windows-приложение (WinForms, .NET Framework 4.8, C#) для массовой установки контент-архивов DAZ Studio
(zip / 7z / rar) в структуру Content Library. Распаковка делегируется внешним утилитам `7z.exe` (полный 7-Zip, умеет и rar) и `UnRAR.exe`.
Лицензия GPL-3.0; репозиторий: https://github.com/dragokas/SuperDIM (открытых issues нет; единственный TODO из README —
режим Full install может работать некорректно). Описание для пользователей — в `README.md`. Здесь — только то, что нужно для работы с кодом.
`7z.exe`+`7z.dll`/`UnRAR.exe` — сторонние официальные бинарники (7-Zip, WinRAR), их не изменяем.

## Сборка и запуск

Решение: `source/DazUnpacker.sln`, проект `source/DazUnpacker.csproj` (старый формат, не SDK-style).
Корневой namespace `DazUnpacker`, имя сборки `SuperDIM`, точка входа `Program.Main`.

- `dotnet build` **не работает**: ресурсы `.resx` с не-строковыми данными требуют полноценного MSBuild.
- Рабочая команда (проверена, Debug), из каталога `source/`:

```powershell
& 'C:\Program Files\Microsoft Visual Studio\18\Community\MSBuild\Current\Bin\MSBuild.exe' DazUnpacker.sln -p:Configuration=Debug '-p:Platform=Any CPU' -restore -nologo -v:q
```

- Результат: `source/bin/Debug/SuperDIM.exe`. Папка `tools/` копируется в выход автоматически (`<Content Include=".\tools\**">`).
- NuGet: `EWSoftware.ListControls` (кастомный список-радиокнопки `radioButtonInstallLibs`), `Newtonsoft.Json` (конфиг),
  `System.Data.SQLite.Core` (БД каталога; нативные `x86\`/`x64\SQLite.Interop.dll` копирует `.targets` пакета `Stub.System.Data.SQLite.Core.NetFramework`).
  Пакеты лежат в `source/packages/`. На чистом клоне (нет `source/packages/`) первую сборку делай с `-p:RestorePackagesConfig=true`
  (иначе `-restore` не восстанавливает `packages.config`) и затем повтори сборку обычной командой.
- Тестов в проекте нет. `source/_publish.bat` собирает Release и пакует zip (без `config.txt`, `catalog.db*` и `app.log`).
- Релизная сборка блокируется запущенным Release-процессом пользователя — всегда собирай Debug (см. глобальные правила).

## Структура репозитория

```
README.md, LICENSE, .gitignore
source/
  DazUnpacker.sln / .csproj     проект; файлы перечислены в csproj явно (новый .cs надо добавлять в <Compile>)
  Properties/                   AssemblyInfo, Resources, Settings (Settings пустой)
  source/                       весь код приложения (папка называется "source" внутри "source")
  tools/7z/{7z.exe,7z.dll}, tools/WinRar/UnRAR.exe   внешние распаковщики
  dci.ico, _publish.bat, packages.config
```

Артефакты рядом с exe при работе: `config.txt` (настройки, JSON), `app.log`, `catalog.db` (SQLite, + `-wal`/`-shm`).

## Архитектура

Один главный `Form1` (partial: `Form1.cs` + `Form1.Designer.cs`, вкладки Settings, Installer, Catalog, Logs) и набор **статических классов-фасадов**,
каждый из которых получает ссылку на форму через `Init(Form1)` и работает с её контролами напрямую.
Слоёв «модель / представление» нет: состояние живёт в контролах формы и в `Config.Storage`.

### Поток установки (главный сценарий)

```
Drag&Drop / "Select Folder"  ->  Content.AddObject/AddFolder/AddFile  ->  listViewContent (Library.Content)
    (фильтр по дате применяется уже на добавлении)
buttonStart_Click  ->  собирает Unpacker.UnpackInfo из контролов  ->  новый Thread -> Unpacker.UnpackAll
    Parallel.ForEach(архивы, MaxDegreeOfParallelism = threadsCount):
      1. определить ContentType (по имени архива: DetectContentType, либо общий тип из UI)
      2. ранний фильтр по типу (FilterManager.IsPassed(ContentTypes)) -> Skipped(CriteriaNotMatch)
      3. Archive.ExtractSmart -> распаковка во временную папку + рекурсивно вложенные архивы
         + поиск "top-директорий" с *.duf + определение Genesis-версии/пола
      4. если тип всё ещё Unknown — определить по top-директории; повторный фильтр по типу
      5. для каждой версии Genesis (3/8/9) из фильтра: InstallDuf -> целевой путь в библиотеке
      6. Full install: InstallRoot — копирует остальные корневые папки (data, Runtime…) в библиотеку
      7. Statistics.DoneIncrement() в finally -> обновление прогресса на форме
```

Целевой путь для .duf: `<Library>\People\<Genesis X [Male|Female]>\<Clothing|Hair|Poses|…>\[rootFolderName]\<dirGroup>\<имя top-папки>`
(`Unpacker.GetInstallDirByContentType`). `dirGroup` — относительный путь архива от «Root folder» (зеркалирование иерархии),
к каждому компоненту может добавляться префикс. Environment/Light/Prop/Script — «version-independent»
(`Content.IsVersionIndependentType`): кладутся вне папки Genesis (`Environments`, `Light Presets`, `scripts`)
или один раз, без перебора версий.

### Модули (`source/source/`)

| Файл | Роль |
|---|---|
| `Program.cs` | Именованный Mutex `SuperDIM_mtx` (защита от двойного запуска), `Application.Run(new Form1())` |
| `Form1.cs` / `.Designer.cs` | UI и обработчики; вкладки: Settings, Installer, Catalog, Logs. Сборка `UnpackInfo`, запуск потока, drag&drop (рекурсивно навешивается на всё `tabPageInstaller`), контекстное меню копирования, «Re-install access denied» |
| `Unpacker.cs` | Оркестрация установки (`UnpackAll`), `UnpackInfo`, выбор целевой папки, эвристики: `DetectContentType` (подстроки в пути), `IsGenesisX`/`GetGenesisVersionsByName` (версия/пол по имени), `IsGenesisFilterAllowed` (учёт фильтра поколений) |
| `Archive.cs` | Работа с архивами: `ExtractSmart` (ядро), `ExtractSingle(Zip/Rar)` — запуск `7z.exe`/`UnRAR.exe` через `Process`, `GetDufTopFolders`/`ConcatenateSibilFolders` (какие папки считать «корнем контента»), `FindRootDir` по списку `DazRootDirs`, `IsBlackName` (отсев Poser/UVM/…Ps), `PrepareTempDir`. Enum `FindFailReason`, класс `DufRootInfo` |
| `Content.cs` | Enum'ы `GenesisVersion` ([Flags]), `GenesisGenders`, `ContentTypes`; добавление файлов в список (`AddFile/AddFolder/AddObject`), `GetParentFolders` для combobox «Root folder» |
| `Library.cs` | Статические вложенные фасады над контролами: `Content` (список архивов), `Basic` (DAZ-библиотеки), `Search`, `Exclude`, `Install` (выбранная целевая библиотека), `Skipped` (список пропущенных + причина; потокобезопасное добавление через `BeginInvoke`) |
| `Config.cs` | `Config.Storage` (`ConfigStorage` → `_TabSettings/_TabInstaller/_TabCatalog/_TabLog`) — сериализуемые настройки; `Config.Data` — пути к утилитам; `Init` заполняет комбобоксы, `ApplyStorage` форма←конфиг, `SaveConfig` форма→конфиг (вызывается в `FormClosing`) |
| `FilterManager.cs` | Фильтры установки: `IFilter`, `DateFilter` (Before/After/Equal по CreationTime архива), `ContentFilter` (один выбранный `ContentTypes`), `FilterManager` (флаги включения + список). Полиморфно сериализуется Json.NET (`TypeNameHandling.Auto`) |
| `FileSys.cs` | Файловые хелперы: `CopyFolderRecursively` (+`FsCopyStatus`), `DirectoryDelete`, `EnumDirectory`, `GetParentDir`, `PathInsertPrefixToAllComponents`, `StripFilename` |
| `Statistics.cs` | Счётчики `Total/Done` (Interlocked) и вывод в `labelProgress` через `BeginInvoke` |
| `Logger.cs` | Статический `Log` (Info/Warning/Error/Trace), вывод в `app.log` и `listBoxLog`; потокобезопасный, `DisableCache(true)` => запись сразу |
| `Globals.cs` | `AppDir`, `AppName`, `ReportError` (лог + MessageBox), `StringContains` (ignore-case), `OpenTextFile` |
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
  (любой глубины, `MaxNestingDepth = 8`). В БД попадают файлы всех уровней; вложенные архивы-контейнеры как строки не пишутся.
- `FilePath` — путь внутри (листового) архива **как есть**, с `/` и ведущим `/` (например, `/Content/data/Daz 3D/...`, `/Pinme-for-.../Studio/My Library/Presets/Poses/...`).
  Корень библиотеки DAZ не вычисляется. Массовый поиск (Search & Install) — по **хвосту** пути; фильтр на вкладке — по **вхождению**.
- **Ignore case** (чекбокс, по умолчанию выкл): выкл — всё регистрозависимо; вкл — полностью регистронезависимо, включая кириллицу.
  Для этого в таблице есть технические колонки `PathKey`/`NameKey` (нижний регистр, `ToLowerInvariant`) — в UI и CSV не показываются, при импорте CSV пересчитываются.
- Листинг бандла не распаковывает его целиком: `7z l -slt -ba`; на диск (в `<temp>\Catalog\<guid>`) попадают только вложенные архивы и только если они есть;
  папка удаляется сразу после листинга. Temp — «Specific folder» из groupBox6, иначе `Config.Data.TempFolder`.
  `7z.exe` читает rar только в полной версии 7-Zip (`7za.exe` формата RAR не содержит).
- **Повторный Scan пропускает неизменённые бандлы:** таблица `Bundles` (`ArchivePath`, `Size`, `ModifiedTicks` = `LastWriteTimeUtc`) пишется в той же транзакции, что и строки
  (`ReplaceBundle(path, rows, BundleStamp)`; штамп снимается **до** чтения архива). Совпали размер и время — бандл не открывается, статус показывает число пропущенных.
  Штамп пишет и хук установщика. Импорт CSV штамп бандла удаляет (бандл пересканируется), Clear очищает обе таблицы. Принудительного «Rescan all» нет (по решению пользователя).
- Хук установщика: `Archive.ExtractSmart(..., CatalogCollector)` листит внешний и каждый вложенный архив **перед** распаковкой (вложенный затем удаляется);
  `Unpacker.UnpackAll` пишет результат в БД, ошибки каталога установку не ломают. Так список файлов полный и в режиме Partial (где `data`/`Runtime` не распаковываются).
- БД: один писатель (общий `lock`), WAL; чтение — отдельными соединениями. Грид виртуальный: `DbRowSource` держит `long[] ids` (результат фильтра/сортировки)
  и подгружает строки страницами по Id. Фильтр запускается через 1 с после ввода (`Timer`), устаревшие запросы отбрасываются по номеру.
- **Unique archive** (чекбокс): в гриде только первая (по `Id`) подходящая строка каждого бандла — `WHERE Id IN (SELECT MIN(Id) ... GROUP BY ArchivePath)`; работает вместе с фильтром и сортировкой.
- Search & Install: `Form1.ReplaceInstallList` (очистка списка установки + `AddFile` только для существующих на диске бандлов + переход на вкладку Installer).
  Если на диске нет ни одного бандла — список установки не трогается.

### Конфигурация

- `config.txt` рядом с exe, Json.NET, `Formatting.Indented`. Читается в `Config.Init` (конструктор формы), пишется в `Form1_FormClosing`.
- Пути к `7z.exe` и `UnRAR.exe` вычисляются от каталога сборки: `tools\7z\7z.exe`, `tools\WinRAR\UnRAR.exe`
  (в git каталог называется `WinRar` — регистр не важен на Windows).
- Режимы установки: **Partial** (`InstallDufOnly = true`, по умолчанию — при распаковке пропускаются `data`, `Runtime`, `Documentation` и т.п.)
  и **Full** (копируются и остальные корневые папки).

## Подводные камни и особенности (важно при доработке)

- **Нет разделения UI и логики.** Статические классы напрямую читают контролы (`Library.*`, `Config.SaveConfig`, `Content.AddFile`).
  Новые настройки добавляются в три места: контрол в дизайнере → поле в `Config.Storage` → `ApplyStorage` + `SaveConfig`.
- **Статический глобальный `Config.Storage` читается из рабочих потоков** `Parallel.ForEach` (фильтры, `genesisVersionFilter`) —
  значения для установки перед стартом синхронизируются из формы (`UpdateGenesisTypeFilter`, `SaveFiltersFromForm` в `buttonStart_Click`).
- **Завершение установки определяется косвенно:** `labelProgress_TextChanged` сравнивает `Statistics.Done == Total`,
  останавливает секундомер и включает кнопку Install. Пропущенные на этапе `FilterFilesList` архивы тоже считаются через `DoneIncrement`.
- **Временные папки:** `Archive.PrepareTempDir` всегда использует `<корень диска целевой библиотеки>\Temp\Daz` (чтобы `Directory.Move`
  был переименованием, а не копированием). Настройка «Specific temp folder» (`TabSettings.TempFolder/UseSpecificTempFolder`),
  `Config.Data.TempFolder`, `Globals.ClearTempFolder` и `Archive.ClearTemp` — **не используются при распаковке / нигде не вызываются** (заготовки или недоделки).
- **Вкладка Sort удалена** (вместе с `_TabSort`, `Exclusion`, `Library.Search/Exclude`). Её настройки установка не использовала. Живые поля «Prefix to append to subfolders»
  и «Create root subfolder» стоят на вкладке Installer (панели `panel6`/`panel4`) и хранятся в `_TabInstaller` (`UsePrefix/PrefixName/CreateRootSubfolder/RootSubfolder`).
  Старый ключ `TabSort` в `config.txt` Json.NET игнорирует. Остальные наследия: `UseLanguageRu` (переключатель языка скрыт `panelLanguage.Visible = false`), `GarbageLibraries`.
- **Эвристики определения типа/версии хрупкие** (подстроки в пути архива/папки): `DetectContentType` (порядок `else if` важен —
  например, «Hair» vs «Chair», «Light» ловит «IRay»), `IsGenesisX`, `IsBlackName`, `GetDufTopFolders` (костыли для `Iray`, `\data\`, `Characters`).
  Любая правка должна учитывать порядок проверок и регрессии на реальных архивах — автотестов нет.
- **Вложенные архивы:** `ExtractSmart` в цикле распаковывает найденные внутри архивы в `destDir\Daz<N>`, удаляя исходный вложенный архив,
  и собирает общий список `dufRootInfo`/`rootDirs`. Код ошибки/exit code внешних утилит не проверяется (`ExtractSingle*` возвращает true, если процесс стартовал).
- **Ошибки в рабочих потоках** гасятся: `Globals.ReportError(ex, false)` только пишет в лог. `AccessDenied` помечается в `Library.Skipped`
  и позволяет кнопку «Re-install access denied» (повторно добавляет такие архивы и жмёт Install).
- **Размер окна:** `Form1` изменяемого размера (`MinimumSize` = размер дизайна), контролы вкладок Installer/Logs расставлены якорями (Anchor) в дизайнере,
  Catalog растягивается через Dock/TableLayoutPanel. Разворачивание на весь экран — только системной кнопкой окна (собственного full screen нет, по просьбе пользователя).
- **Вкладки:** порядок Settings, Installer, Catalog, Logs. Installer и Logs добавляются/убираются в `UpdateTabs` (когда есть хотя бы одна библиотека),
  Catalog есть всегда. Вкладки выбирай по ссылке (`tabControl.SelectedTab = tabPageLogs`), а не по индексу; `Config.TabNumber` хранит индекс,
  поэтому после добавления Catalog старый сохранённый индекс может один раз открыть соседнюю вкладку.
- **Локализация:** UI на английском, русский вариант не реализован. Комментарии в коде — английские.
- **Кодировка исходников:** файлы в UTF-8 с BOM, CRLF (перед правкой полагаться на хуки File Guard, см. глобальные правила).
- Несоответствие в зависимостях: `packages.config` в `source/` ссылается только на `EWSoftware.ListControls`,
  `Newtonsoft.Json` описан в `source/source/packages.config` (net472); в csproj обе ссылки идут через `packages\...` (restore работает).

## Рабочие соглашения для этого проекта

- Общие правила стиля/git/сборки берутся из глобального `~/.claude/CLAUDE.md` (фигурные скобки везде, комментарии на английском,
  один оператор на строку, коммит локально после каждой правки, push только по явной просьбе).
- Новый `.cs` файл нужно вручную добавить в `<Compile Include=...>` в `source/DazUnpacker.csproj`.
- Проверка UI без ручного запуска: копию `bin\Debug` запускать из scratchpad (чужие/пользовательские процессы SuperDIM не трогать — завершать только по своему PID),
  управлять через UI Automation (элементы ищутся по `Name`, перед кликом окно нужно активировать, `Microsoft.VisualBasic.Interaction.AppActivate`).
- Если логика новой функции > ~500 строк — выносить в отдельную форму/класс с именем, начинающимся на `Form1…` (правило из глобальных инструкций).
- Прежде чем добавлять код, проверяй существующие хелперы: `FileSys`, `Globals`, `Library.*`, `FilterManager`.
