@echo off
cd /d "%~dp0"

:: CONSTANTS
set compressor=%ProgramFiles%\7-Zip\7z.exe
if not exist "%compressor%" set compressor=%~dp0tools\7z\7z.exe
set vswhere=%ProgramFiles(x86)%\Microsoft Visual Studio\Installer\vswhere.exe
set prj=DazUnpacker
set exename=SuperDIM
set release_dir=%~dp0bin\Release

:: MSBuild from Visual Studio is required: "dotnet build" cannot compile non-string .resx resources
set msbuild=
if exist "%vswhere%" (
  for /f "usebackq delims=" %%i in (`"%vswhere%" -latest -products * -requires Microsoft.Component.MSBuild -find MSBuild\**\Bin\MSBuild.exe`) do set msbuild=%%i
)
if not defined msbuild (
  echo MSBuild of Visual Studio is not found!
  goto done
)

if exist "%release_dir%" move "%release_dir%" "%release_dir%.bak"

:: Compile project if needed
if not exist "%release_dir%\%exename%.exe" (
  "%msbuild%" "%prj%.slnx" -restore -p:Configuration=Release "-p:Platform=Any CPU" -nologo -v:m
)

if not exist "%release_dir%\%exename%.exe" (
  echo Failed to build the solution!
  goto finale
)

cd /d "%release_dir%"

:: Remove logfile
del "app.log" 2>NUL

:: Prepare release archive
if exist "%exename%.zip" del "%exename%.zip"
"%compressor%" a -tzip -mx=9 -r -x!config.txt -x!catalog.db* "%exename%.zip" .\*
move "%exename%.zip" "%~dp0"

:finale
:: Leave the release folder, otherwise it cannot be deleted
cd /d "%~dp0"
rd /s /q "%release_dir%"
if exist "%release_dir%.bak" move "%release_dir%.bak" "%release_dir%"

:done
pause
