using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace DazUnpacker
{
    // Reads archive contents with 7z.exe: lists files without unpacking, unpacks only nested archives
    internal static class ArchiveLister
    {
        private const string KeyPath = "Path = ";
        private const string KeyFolder = "Folder = ";
        private const string KeyAttributes = "Attributes = ";

        // Fills 'files' with paths of all files (no directories) inside the archive. Returns false on a fatal error.
        internal static bool ListFiles(string archivePath, List<string> files)
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
        private static void ParseListing(List<string> lines, List<string> files)
        {
            string path = null;
            bool isDirectory = false;

            foreach (string line in lines)
            {
                if (line.StartsWith(KeyPath))
                {
                    AddFile(files, path, isDirectory);
                    path = line.Substring(KeyPath.Length);
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
            }
            AddFile(files, path, isDirectory);
        }

        private static void AddFile(List<string> files, string path, bool isDirectory)
        {
            if (path != null && !isDirectory && path.Length != 0)
            {
                files.Add(path);
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

        // Runs 7z.exe hidden; collects stdout lines (if requested); returns the exit code
        private static int RunTool(string arguments, List<string> outputLines)
        {
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = Config.Data.UnpackerTool7z,
                Arguments = arguments,
                UseShellExecute = false,
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                RedirectStandardInput = true, // closed at once, so a password prompt fails instead of hanging
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                StandardOutputEncoding = Encoding.UTF8
            };

            using (Process p = new Process())
            {
                p.StartInfo = psi;
                p.ErrorDataReceived += (sender, e) => { }; // drain stderr to prevent a deadlock
                p.Start();
                p.StandardInput.Close();
                p.BeginErrorReadLine();

                string line;
                while ((line = p.StandardOutput.ReadLine()) != null)
                {
                    if (outputLines != null)
                    {
                        outputLines.Add(line);
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
