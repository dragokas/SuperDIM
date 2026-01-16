using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DazUnpacker
{
    public static class FileSys
    {
        public static string GetParentDir(string folder)
        {
            if (!string.IsNullOrEmpty(folder))
            {
                folder = Path.GetDirectoryName(folder);
                if (folder != null)
                {
                    return folder;
                }
            }
            return string.Empty;
        }

        public static string Unquote(string path)
        {
            if (path.Length >= 2)
            {
                if (path[0] == '\"' && path[path.Length - 1] == '\"')
                {
                    return path.Substring(1, path.Length - 2);
                }
                return path;
            }
            return String.Empty;
        }

        public static string PathInsertPostfix(string path, string filenamePostfix)
        {
            return String.Format("{0}\\{1}{2}{3}", FileSys.GetParentDir(path),
                Path.GetFileNameWithoutExtension(path), filenamePostfix, Path.GetExtension(path));
        }

        public static string PathInsertPrefixToAllComponents(string path, string filenamePrefix)
        {
            string result = String.Empty;
            bool first = true;

            foreach (string component in path.Split('\\'))
            {
                if (first && component.EndsWith(":"))
                {
                    result += component;
                }
                else
                {
                    if (!first)
                    {
                        result += "\\";
                    }
                    if (component.Length != 0)
                    {
                        result += filenamePrefix + component;
                    }
                }
                first = false;
            }
            return result;
        }

        public static string FindFileNameAvailable(string busyPath)
        {
            string dir = FileSys.GetParentDir(busyPath);
            string file = Path.GetFileNameWithoutExtension(busyPath);
            string ext = Path.GetExtension(busyPath);
            string path = String.Empty;
            int i = 1;
            do
            {
                i++;
                path = Path.Combine(dir, String.Format("{0}({1}){2}", file, i, ext));
            }
            while (File.Exists(path));
            return path;
        }

        public static string FindDirNameAvailable(string busyPath)
        {
            string path = String.Empty;
            int i = 1;
            do
            {
                i++;
                path = String.Format("{0}({1})", busyPath, i);
            }
            while (Directory.Exists(path));
            return path;
        }

        public static bool DirectoryDelete(string dir)
        {
            try
            {
                if (Directory.Exists(dir))
                {
                    Directory.Delete(dir, true);
                    return true;
                }
            }
            catch (Exception) // shitty C# IO Access denied Exception on delete dir
            {
                return false;
            } 
            return false;
        }

        public enum FsCopyStatus
        {
            CopySuccess,
            CopyFailed,
            CopyNoChanges
        }

        public static FsCopyStatus CopyFolderRecursively(string sourceDir, string targetDir, bool overwrite)
        {
            bool hasReplaces = false;
            try
            {
                if (!Directory.Exists(targetDir))
                {
                    Directory.CreateDirectory(targetDir);
                }
                foreach (string dirPath in Directory.EnumerateDirectories(sourceDir, "*", SearchOption.AllDirectories))
                {
                    string dir = Path.Combine(targetDir, dirPath.Substring(sourceDir.Length + 1));
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }
                }
                foreach (string sourceFile in Directory.EnumerateFiles(sourceDir, "*.*", SearchOption.AllDirectories))
                {
                    string targetFile = Path.Combine(targetDir, sourceFile.Substring(sourceDir.Length + 1));
                    if (overwrite)
                    {
                        File.Copy(sourceFile, targetFile, true);
                        hasReplaces = true;
                    }
                    else
                    {
                        if (!File.Exists(targetFile))
                        {
                            File.Copy(sourceFile, targetFile, true);
                            hasReplaces = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Globals.ReportError(ex, false);
                return FsCopyStatus.CopyFailed;
            }
            return hasReplaces ? FsCopyStatus.CopySuccess : FsCopyStatus.CopyNoChanges;
        }

        public static IEnumerable<string> EnumDirectory(string path, Func<FileInfo, bool> checkFile = null)
        {
            if (string.IsNullOrEmpty(path)) yield break;
            string mask = Path.GetFileName(path);
            if (string.IsNullOrEmpty(mask)) mask = "*.*";
            path = Path.GetDirectoryName(path);

            foreach (string file in Directory.EnumerateFiles(path, mask, SearchOption.AllDirectories))
            {
                if (checkFile == null || checkFile(new FileInfo(file)))
                    yield return file;
            }
        }

        public static string StripFilename(string path)
        {
            int pos = path.LastIndexOf('\\');
            if (pos >= 0)
            {
                return path.Substring(0, pos);
            }
            else
            {
                return "";
            }
        }

    }
}
