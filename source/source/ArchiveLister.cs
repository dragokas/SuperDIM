using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace DazUnpacker
{
    // A file inside an archive as 7z lists it
    internal class ArchiveEntry
    {
        internal string Path = "";
        internal string Modified = ""; // "yyyy-MM-dd HH:mm:ss"; empty if 7z did not tell
    }

    // Reads archive contents with 7z.exe: lists files without unpacking, unpacks only nested archives
    internal static class ArchiveLister
    {
        private const string KeyPath = "Path = ";
        private const string KeyFolder = "Folder = ";
        private const string KeyAttributes = "Attributes = ";
        private const string KeyModified = "Modified = ";

        // Fills 'files' with all files (no directories) inside the archive. Returns false on a fatal error.
        internal static bool ListFiles(string archivePath, List<ArchiveEntry> files)
        {
            try
            {
                string args = String.Format("l -slt -ba -sccUTF-8 -y \"{0}\"", archivePath);
                List<string> lines = new List<string>();
                int exitCode = RunTool(args, lines);
                if (exitCode >= 2)
                {
                    Log.Warning("Failed to list archive (7z exit code " + exitCode + "): " + archivePath);
                    return false;
                }
                ParseListing(lines, files);
                return true;
            }
            catch (Exception ex)
            {
                Globals.ReportError(ex, false, "ListFiles: " + archivePath);
            }
            return false;
        }

        // Parses "7z l -slt -ba" output: blocks of "Key = Value" lines, the block starts with "Path = "
        private static void ParseListing(List<string> lines, List<ArchiveEntry> files)
        {
            string path = null;
            string modified = "";
            bool isDirectory = false;

            foreach (string line in lines)
            {
                if (line.StartsWith(KeyPath))
                {
                    AddFile(files, path, modified, isDirectory);
                    path = line.Substring(KeyPath.Length);
                    modified = "";
                    isDirectory = false;
                }
                else if (line.StartsWith(KeyFolder))
                {
                    isDirectory = line.Substring(KeyFolder.Length).Trim() == "+";
                }
                else if (line.StartsWith(KeyAttributes))
                {
                    if (line.Substring(KeyAttributes.Length).TrimStart().StartsWith("D"))
                    {
                        isDirectory = true;
                    }
                }
                else if (line.StartsWith(KeyModified))
                {
                    modified = NormalizeDate(line.Substring(KeyModified.Length));
                }
            }
            AddFile(files, path, modified, isDirectory);
        }

        // 7z prints "2025-06-16 16:09:46" for zip and "2023-07-09 18:08:07.0000000" for rar and 7z: keep the seconds only
        internal static string NormalizeDate(string value)
        {
            value = value.Trim();
            if (value.Length >= 19 && value[10] == ' ')
            {
                return value.Substring(0, 19);
            }
            return value;
        }

        private static void AddFile(List<ArchiveEntry> files, string path, string modified, bool isDirectory)
        {
            if (path != null && !isDirectory && path.Length != 0)
            {
                files.Add(new ArchiveEntry { Path = path, Modified = modified });
            }
        }

        // Unpacks only the nested archives (any depth) keeping their relative paths. Returns false on a fatal error.
        internal static bool ExtractNestedArchives(string archivePath, string destinationDir)
        {
            try
            {
                string args = String.Format("x -y -r -o\"{0}\" -i!*.zip -i!*.rar -i!*.7z \"{1}\"", destinationDir, archivePath);
                int exitCode = RunTool(args, null);
                if (exitCode >= 2)
                {
                    Log.Warning("Failed to extract nested archives (7z exit code " + exitCode + "): " + archivePath);
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                Globals.ReportError(ex, false, "ExtractNestedArchives: " + archivePath);
            }
            return false;
        }

        private static int RunTool(string arguments, List<string> outputLines)
        {
            return RunTool(Config.Data.UnpackerTool7z, arguments, Encoding.UTF8, outputLines, null);
        }

        // Runs an archiver hidden; collects stdout / stderr lines (if requested, may be the same list); returns the exit code
        internal static int RunTool(string exePath, string arguments, Encoding outputEncoding, List<string> outputLines, List<string> errorLines)
        {
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = exePath,
                Arguments = arguments,
                UseShellExecute = false,
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                RedirectStandardInput = true, // closed at once, so a password prompt fails instead of hanging
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                StandardOutputEncoding = outputEncoding,
                StandardErrorEncoding = outputEncoding
            };

            using (Process p = new Process())
            {
                p.StartInfo = psi;
                p.ErrorDataReceived += (sender, e) =>
                {
                    // stderr is always drained to prevent a deadlock
                    if (errorLines != null && e.Data != null)
                    {
                        lock (errorLines)
                        {
                            errorLines.Add(e.Data);
                        }
                    }
                };
                p.Start();
                p.StandardInput.Close();
                p.BeginErrorReadLine();

                string line;
                while ((line = p.StandardOutput.ReadLine()) != null)
                {
                    if (outputLines != null)
                    {
                        lock (outputLines)
                        {
                            outputLines.Add(line);
                        }
                    }
                }
                p.WaitForExit();
                return p.ExitCode;
            }
        }

        // All archive files below the directory; unreadable subfolders are skipped
        internal static List<string> FindArchives(string directory)
        {
            List<string> archives = new List<string>();
            Stack<string> pending = new Stack<string>();
            if (Directory.Exists(directory))
            {
                pending.Push(directory);
            }
            while (pending.Count > 0)
            {
                string dir = pending.Pop();
                try
                {
                    foreach (string file in Directory.EnumerateFiles(dir))
                    {
                        if (Archive.IsArchiveExtension(file))
                        {
                            archives.Add(file);
                        }
                    }
                    foreach (string subDir in Directory.EnumerateDirectories(dir))
                    {
                        pending.Push(subDir);
                    }
                }
                catch (Exception ex)
                {
                    Log.Warning("Cannot read folder: " + dir + " (" + ex.Message + ")");
                }
            }
            archives.Sort(StringComparer.OrdinalIgnoreCase);
            return archives;
        }
    }
}
