@echo off
cd /d "%~dp0"

:: CONSTANTS
set compressor=%ProgramFiles%\7-Zip\7z.exe
set prj=DazUnpacker
set exename=SuperDIM
set release_dir=%~dp0bin\Release

:: Compile project if needed
if not exist "%release_dir%\%exename%.exe" (
  set DOTNET_CLI_TELEMETRY_OPTOUT=1
  dotnet build "%prj%.sln" --configuration Release /p:Platform="Any CPU"
)

if not exist "%release_dir%\%exename%.exe" (
  echo Failed to build the solution!
  pause>NUL & exit /b
)

cd /d "%release_dir%"

:: Remove logfile
del "app.log"

:: Prepare release archive
if exist "%exename%.zip" del "%exename%.zip"
"%compressor%" a -tzip -mx=9 -r -x!config.txt "%exename%.zip" .\*
move "%exename%.zip" "%~dp0"

pause