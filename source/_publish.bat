@echo off
cd /d "%~dp0"

:: CONSTANTS
set compressor=%ProgramFiles%\7-Zip\7z.exe
if not exist "%compressor%" set compressor=%~dp0tools\7z\7z.exe
set prj=DazUnpacker
set exename=SuperDIM
set release_dir=%~dp0bin\Release

if exist "%release_dir%" move "%release_dir%" "%release_dir%.bak"

:: Compile project if needed
if not exist "%release_dir%\%exename%.exe" (
  set DOTNET_CLI_TELEMETRY_OPTOUT=1
  dotnet build "%prj%.slnx" --configuration Release /p:Platform="Any CPU"
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
pause
