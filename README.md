# SuperDIM
Advanced Daz Studio Install Manager, allowing to unpack content archives of any complexity

<img width="1564" height="1346" alt="Image" src="https://github.com/user-attachments/assets/d9d46864-2592-4a0e-a6d1-04af15589a8c" />

## Purpose
This tool is intended for DAZ Studio, the well-known 3D content managment and rendering software.
It provides the advanced fetures to install content archives of any type in convenient directories hierarhy.

## Features
 - Bulk multithreading install
 - Install zip, 7z, rar archive types
 - Supports multi-nested and multiple archives in a single pack
 - Mirroring directory hierarhy
   - The relative folders structure where content archives located initially will be mirrored in DAZ Studio UI "Content library" window (see the screenshot)
 - Partial or Full install
   - Partial install copies only \*.duf files (those you see in DAZ Studio), it is helpful when content is already installed.
 - Filter by date
   - options: before, after, exact - e.g. only newest content will be installed
 - Filter by Genesis version and gender

## Quick start

 - Prepare your content archives with a convenient folders hierarhy
 - Run the SuperDIM
 - On "Settings" tab press Add... (or Drag & Drop) your DAZ Library root dirs, or select the empty folder
 - On "Install" tab select where to install to
 - Add or drag and drop your content archives.
   - You may put the entire folder.
   - It is highly suggested to install only one type of content per session (only Dress, only Poses etc). In this case, please unselect option "Auto-detect content type" and select appropriate type if autodetection failed.
 - Select the desired "Root folder"
   - The folder hierarhy will be counted relative to this folder during mirroring to the DAZ Studio UI.
 - Select number of threads (for HDD maximum 2 suggested, for SSD you may choose more).
 - Press "Install"
 - Look the "Logs" tab
   - In rare cases there are "Access Denied" errors, if one present press the "Re-install access-denied" button
 - For the newly created Library dir, add it to DAZ
   - Through DAZ Studio, press "Content library" side tab, right click on "DAZ Studio Formats", select "Add a Base directory" and select the newly created root Library folder, see the step 3

## Compatibility

 - Windows 7 and newer
 
## Requirements

 - [DAZ Studio](https://www.daz3d.com/) of any version
 - [.NET Framework 4.8 Runtime](https://dotnet.microsoft.com/en-us/download/dotnet-framework/net48)

## Compilation from source
 
 - Visual Studio 2019 or newer
 - Copy "tools" dir to the release dir
   - tools\7z must have the official [7zip Extra](https://www.7-zip.org/download.html) package files: 7za.dll, 7za.exe, 7zxa.dll
   - tools\WinRar must have the official [WinRar](https://www.rarlab.com/download.htm) UnRAR.exe file

## TODO
 - "Full installation" method may be glitchy, please open Issue and provide your Content archive if you think the installation goes wrong
