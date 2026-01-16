using DazUnpacker.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DazUnpacker
{
    public enum FindFailReason
    {
        Unknown,
        Genesis3,
        Genesis8,
        Genesis9,
        BlackName,
        AccessDenied,
        NotArchive,
        EmptyArchive,
        AlreadyInstalled,
        CriteriaNotMatch,
        ContentTypeUndefined
    }

    internal static class Archive
    {
        internal class DufRootInfo
        {
            internal string topDirectory = "";
            internal GenesisVersion genesisVersion = GenesisVersion.Unknown;
            internal ContentType contentType = ContentType.Unknown;
            internal GenesisGender gender = GenesisGender.Unknown;

            internal bool isSingleGenesisVersion()
            {
                return Enum.GetValues(typeof(GenesisVersion)).Cast<GenesisVersion>()
                    .Any(v => v.Equals(genesisVersion));
            }
        }

        private static List<string> m_TempDirs = new List<string>();
        internal static HashSet<string> DazRootDirs = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        internal static void Init()
        {
            DazRootDirs.Add("Accessories");
            DazRootDirs.Add("data");
            DazRootDirs.Add("aniBlocks");
            DazRootDirs.Add("Animals");
            DazRootDirs.Add("Animation");
            DazRootDirs.Add("Creatures");
            DazRootDirs.Add("Document");
            DazRootDirs.Add("Documentation");
            DazRootDirs.Add("Documents");
            DazRootDirs.Add("Environment");
            DazRootDirs.Add("Environments");
            DazRootDirs.Add("Figures");
            DazRootDirs.Add("Light Presets");
            DazRootDirs.Add("Materials");
            DazRootDirs.Add("People");
            DazRootDirs.Add("Poses");
            DazRootDirs.Add("Presets");
            DazRootDirs.Add("Props");
            DazRootDirs.Add("Readme");
            DazRootDirs.Add("ReadMes");
            DazRootDirs.Add("ReadMe's");
            DazRootDirs.Add("Runtime");
            DazRootDirs.Add("Scene");
            DazRootDirs.Add("Scenes");
            DazRootDirs.Add("scripts");
            DazRootDirs.Add("Shader Presets");
            DazRootDirs.Add("Shaders");
            DazRootDirs.Add("Templates");
            DazRootDirs.Add("Vehicles");
        }

        // Temp dir should be on the same drive as target to improve performance (prevent double-copy)
        public static bool PrepareTempDir(string zipTargetPath, out string tempDir)
        {
            try
            {
                string rootDrive = Path.GetPathRoot(zipTargetPath);
                string dir = Path.Combine(rootDrive, "Temp\\Daz");
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                if (!m_TempDirs.Contains(dir))
                {
                    m_TempDirs.Add(dir);
                }
                tempDir = dir;
                return true;
            }
            catch (Exception ex)
            {
                Globals.ReportError(ex);
            }
            tempDir = String.Empty;
            return false;
        }

        public static bool IsArchiveExtension(string path)
        {
            string e = Path.GetExtension(path).ToLower();
            return (e == ".zip" ||
                    e == ".rar" ||
                    e == ".7z");
        }

        private static bool IsBlackName(string name)
        {
            string filename = Path.GetFileNameWithoutExtension(name);
            if (filename.EndsWith("Ps"))
            {
                return true;
            }
            name = name.ToLower();
            // Sometimes authors confuse names
            /*
            if (name.Contains("templ"))
            {
                return true;
            }
            */
            if (name.Contains("poser"))
            {
                return true;
            }
            if (name.Contains("uvm"))
            {
                return true;
            }
            return false;
        }

        internal static bool ExtractSingle(string zipPath, string destinationDir)
        {
            return ExtractSingle(zipPath, destinationDir, new List<string>());
        }



        internal static bool ExtractSingleZip(string zipPath, string destinationDir, List<string> skipObjects)
        {
            try
            {
                string skipList = "";
                foreach (string name in skipObjects)
                {
                    skipList += " -xr!\"" + name + "\"";
                }
                Process p = new Process();
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = Config.Data.UnpackerTool7z,
                    Arguments = String.Format("x -y -o\"{0}\"{1} \"{2}\"", destinationDir, skipList, zipPath),
                    UseShellExecute = false,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    CreateNoWindow = true
                };
                p.StartInfo = psi;
                if (p.Start())
                {
                    p.WaitForExit();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Globals.ReportError(ex, false);
            }
            return false;
        }

        internal static bool ExtractSingleRar(string zipPath, string destinationDir, List<string> skipObjects)
        {
            try
            {
                string skipList = "";
                foreach (string name in skipObjects)
                {
                    skipList += " -x\"" + name + "\"";
                }
                Process p = new Process();
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = Config.Data.UnpackerToolRar,
                    Arguments = String.Format("x -y -o- -op\"{0}\"{1} \"{2}\"", destinationDir, skipList, zipPath),
                    UseShellExecute = false,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    CreateNoWindow = true
                };
                p.StartInfo = psi;
                if (p.Start())
                {
                    p.WaitForExit();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Globals.ReportError(ex, false);
            }
            return false;
        }

        internal static bool ExtractSingle(string zipPath, string destinationDir, List<string> skipObjects)
        {
            if (Path.GetExtension(zipPath).IndexOf(".rar", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return ExtractSingleRar(zipPath, destinationDir, skipObjects);
            }
            else
            {
                return ExtractSingleZip(zipPath, destinationDir, skipObjects);
            }
        }

        // Concat:
        // G8/Dress/Name/Materials | *.duf
        // G8/Dress/Name/Wetness | *.duf
        // to:
        // G8/Dress/Name
        private static List<string> ConcatenateSibilFolders(List<string> dufFolders)
        {
            List<string> fixedTopFolders = new List<string>();
            try
            {
                List<string> commonParent = new List<string>();
                List<string> listDeletion = new List<string>();
                Dictionary<string, int> uniqueParent = new Dictionary<string, int>();

                for (int i = dufFolders.Count - 1; i >= 0; i--)
                {
                    string parentDir = FileSys.GetParentDir(dufFolders[i]);

                    // found common parent
                    if (uniqueParent.ContainsKey(parentDir))
                    {
                        int indexPrev = uniqueParent[parentDir];
                        if (indexPrev != -1)
                        {
                            //dufFolders.RemoveAt(indexPrev);
                            listDeletion.Add(dufFolders[indexPrev]);
                            commonParent.Add(parentDir);
                        }
                        uniqueParent[parentDir] = -1;
                        //dufFolders.RemoveAt(i);
                        listDeletion.Add(dufFolders[i]);
                    }
                    else
                    {
                        uniqueParent.Add(parentDir, i);
                    }
                }
                foreach (string dir in dufFolders)
                {
                    if (!listDeletion.Contains(dir))
                    {
                        fixedTopFolders.Add(dir);
                    }
                }
                fixedTopFolders.AddRange(commonParent.ToArray());
            }
            catch (Exception ex)
            {
                Globals.ReportError(ex, false);
            }
            return fixedTopFolders.Distinct().ToList();
        }

        private static bool GetDufTopFolders(string dir, out List<string> topDirs)
        {
            topDirs = new List<string>();
            string dufDir;
            foreach (string file in Directory.EnumerateFiles(dir, "*.duf", SearchOption.AllDirectories))
            {
                dufDir = FileSys.GetParentDir(file);
                if (!topDirs.Contains(dufDir))
                {
                    string name = Path.GetFileName(dufDir);
                    // Add:
                    // G8\Clothing\Short Dress\A97-Diverse
                    // instead of:
                    // G8\Clothing\Short Dress\A97-Diverse\Iray
                    if (Globals.StringContains(name, "Iray"))
                    {
                        dufDir = FileSys.GetParentDir(dufDir);
                        if (!topDirs.Contains(dufDir))
                        {
                            topDirs.Add(dufDir);
                        }
                    }
                    else
                    {
                        topDirs.Add(dufDir);
                    }
                }
            }
            //topDirs = topDirs.Distinct().ToList();

            // remove subfolders of top-folders, e.g.:
            // G8/Dress/Name/ <== leave only this one
            // G8/Dress/Name/Materials
            // G8/Dress/Name/Wetness
            if (topDirs.Count != 0)
            {
                for (int i = topDirs.Count - 1; i >= 0; i--)
                {
                    for (int k = 0; k < i; k++)
                    {
                        if (topDirs[i].StartsWith(topDirs[k]))
                        {
                            topDirs.RemoveAt(i);
                            break;
                        }
                    }
                }
            }
            if (topDirs.Count != 0)
            {
                topDirs = ConcatenateSibilFolders(topDirs);
                topDirs = ConcatenateSibilFolders(topDirs);
            }
            foreach (string topDir in topDirs)
            {
                Log.Info("TOP: " + topDir);
            }
            return topDirs.Count != 0;
        }

        private static FindFailReason GetFailReasonByGenesisVersion(GenesisVersion genesisVersion)
        {
            if (genesisVersion == GenesisVersion.Genesis_3)
            {
                return FindFailReason.Genesis3;
            }
            else if (genesisVersion == GenesisVersion.Genesis_8 || genesisVersion == GenesisVersion.Genesis_8_1)
            {
                return FindFailReason.Genesis8;
            }
            else if (genesisVersion == GenesisVersion.Genesis_9)
            {
                return FindFailReason.Genesis9;
            }
            else
            {
                return FindFailReason.Unknown;
            }
        }

        internal static bool ExtractSmart(string zipPath, string destinationDir, bool recursively, bool skipDataDir, 
            out List<DufRootInfo> dufRootInfo,
            out List<string> rootDirs,
            out FindFailReason failReason)
        {
            failReason = FindFailReason.Unknown;
            dufRootInfo = new List<DufRootInfo>();
            rootDirs = new List<string>();

            GenesisVersion genesisVersionCommon = GenesisVersion.Unknown;
            GenesisGender genderCommon = GenesisGender.Unknown;

            if (!IsArchiveExtension(zipPath))
            {
                failReason = FindFailReason.NotArchive;
                return false;
            }
            if (IsBlackName(zipPath))
            {
                failReason = FindFailReason.BlackName;
                return false;
            }
            if (!Unpacker.IsGenesisFilterAllowed(zipPath, out genesisVersionCommon, out genderCommon))
            {
                failReason = GetFailReasonByGenesisVersion(genesisVersionCommon);
                return false;
            }

            List<string> skipObjects = new List<string>();
            if (skipDataDir)
            {
                skipObjects.Add("data");
                skipObjects.Add("Runtime");
                skipObjects.Add("Documentation");
                skipObjects.Add("Documents");
                skipObjects.Add("Readme");
                skipObjects.Add("ReadMe's");
                skipObjects.Add("Templates");
                skipObjects.Add("Dokumentation");
            }
            ExtractSingle(zipPath, destinationDir, skipObjects);

            List<string> DirsUnpacked = new List<string>();
            DirsUnpacked.Add(destinationDir);

            if (!Directory.Exists(destinationDir))
            {
                failReason = FindFailReason.EmptyArchive;
                return false;
            }

            string rootDir = FindRootDir(destinationDir);
            if (!String.IsNullOrEmpty(rootDir))
            {
                rootDirs.Add(rootDir);
            }

            int dirIndex = 0;
            {
                foreach (string dir in Directory.EnumerateDirectories(destinationDir, "*.*", SearchOption.TopDirectoryOnly))
                {
                    List<string> topDirs;
                    if (GetDufTopFolders(dir, out topDirs))
                    {
                        foreach (string topDir in topDirs)
                        {
                            GenesisVersion genesisVersion = GenesisVersion.Unknown;
                            GenesisGender gender = GenesisGender.Unknown;

                            //Unpacker.IsGenesisFilterAllowed(topDir, out genesisVersion, out gender);

                            Unpacker.GetGenesisVersionsByName(topDir, out genesisVersion, out gender);

                            if (genesisVersion == GenesisVersion.Unknown)
                            {
                                genesisVersion = genesisVersionCommon;
                            }
                            if (gender == GenesisGender.Unknown)
                            {
                                gender = genderCommon;
                            }

                            if (Unpacker.IsGenesisFilterAllowed(genesisVersion))
                            {
                                dufRootInfo.Add(new DufRootInfo
                                {
                                    topDirectory = topDir,
                                    genesisVersion = genesisVersion,
                                    gender = gender
                                });
                            }
                        }
                    }
                }
            }

            // searching for nested archives
            do
            {
                List<string> nestedDirs = new List<string>();

                foreach (string dir in DirsUnpacked)
                {
                    if (Directory.Exists(dir))
                    {
                        foreach (string file in Directory.EnumerateFiles(dir, "*.*", SearchOption.AllDirectories))
                        {
                            GenesisVersion genesisVersion2 = GenesisVersion.Unknown;
                            GenesisGender gender2 = GenesisGender.Unknown;

                            if (!IsArchiveExtension(file))
                            {
                                continue;
                            }
                            if (IsBlackName(file))
                            {
                                failReason = FindFailReason.BlackName;
                                continue;
                            }

                            Unpacker.GetGenesisVersionsByName(file, out genesisVersion2, out gender2);

                            //if (!Unpacker.IsGenesisFilterAllowed(file, out genesisVersion2, out gender2))
                            if (!Unpacker.IsGenesisFilterAllowed(genesisVersion2))
                            {
                                failReason = GetFailReasonByGenesisVersion(genesisVersion2);
                                continue;
                            }

                            ++dirIndex;
                            string tempDir = destinationDir + "\\Daz" + dirIndex.ToString();
                            ExtractSingle(file, tempDir, skipObjects);
                            File.Delete(file);
                            nestedDirs.Add(tempDir);

                            if (!Directory.Exists(tempDir))
                            {
                                continue;
                            }

                            rootDir = FindRootDir(tempDir);
                            if (!String.IsNullOrEmpty(rootDir))
                            {
                                rootDirs.Add(rootDir);
                            }

                            List<string> topDirs;
                            if (GetDufTopFolders(tempDir, out topDirs))
                            {
                                foreach (string topDir in topDirs)
                                {
                                    GenesisVersion genesisVersion = GenesisVersion.Unknown;
                                    GenesisGender gender = GenesisGender.Unknown;

                                    string relativeTopDir = topDir.Substring(rootDir.Length);

                                    Unpacker.GetGenesisVersionsByName(relativeTopDir, out genesisVersion, out gender);

                                    if (genesisVersion == GenesisVersion.Unknown)
                                    {
                                        Unpacker.GetGenesisVersionsByName(topDir, out genesisVersion, out gender);
                                    }

                                    if (genesisVersion == GenesisVersion.Unknown)
                                    {
                                        genesisVersion = genesisVersion2;
                                    }
                                    if (gender == GenesisGender.Unknown)
                                    {
                                        gender = gender2;
                                    }
                                    if (genesisVersion == GenesisVersion.Unknown)
                                    {
                                        genesisVersion = genesisVersionCommon;
                                    }
                                    if (gender == GenesisGender.Unknown)
                                    {
                                        gender = genderCommon;
                                    }

                                    if (Unpacker.IsGenesisFilterAllowed(genesisVersion))
                                    {
                                        dufRootInfo.Add(new DufRootInfo
                                        {
                                            topDirectory = topDir,
                                            genesisVersion = genesisVersion,
                                            gender = gender
                                        });

                                        //ensure we didn't miss root dir
                                        rootDir = FindRootDirFromTopDir(topDir, tempDir);
                                        if (!String.IsNullOrEmpty(rootDir))
                                        {
                                            rootDirs.Add(rootDir);
                                        }
                                    }
                                    else
                                    {
                                        failReason = GetFailReasonByGenesisVersion(genesisVersion);
                                    }
                                }
                            }
                            
                        }
                    }
                }
                DirsUnpacked = nestedDirs;
            }
            while (DirsUnpacked.Count != 0);

            rootDirs = rootDirs.Distinct().ToList();

            return dufRootInfo.Count > 0;
        }

        private static string FindRootDir(string tempDir)
        {
            // Recurse algorithm: with top directories first
            string[] dirs = Directory.GetDirectories(tempDir, "*.*", SearchOption.TopDirectoryOnly);
            foreach (string dir in dirs)
            {
                string dirName = Path.GetFileName(dir);
                if (DazRootDirs.Contains(dirName))
                {
                    return FileSys.GetParentDir(dir);
                }
            }
            foreach (string dir in dirs)
            {
                string nestedRootDir = FindRootDir(dir);
                if (!String.IsNullOrEmpty(nestedRootDir))
                {
                    return nestedRootDir;
                }
            }
            return String.Empty;
        }

        private static string FindRootDirFromTopDir(string topDir, string tempDir)
        {
            // Sequent check: from tempDir to topDir
            List<string> dirs = new List<string>();
            dirs.Add(topDir);
            string currentDir = topDir;
            while (true)
            {
                string parentDir = FileSys.GetParentDir(currentDir);
                if (parentDir.Length <= tempDir.Length)
                {
                    break;
                }
                dirs.Add(parentDir);
                currentDir = parentDir;
            }
            for (int i = dirs.Count - 1; i >= 0; i--)
            {
                if (DazRootDirs.Contains(Path.GetFileName(dirs[i])))
                {
                    return FileSys.GetParentDir(dirs[i]);
                }
            }
            return String.Empty;
        }

        internal static void ClearTemp()
        {
            try
            {
                foreach (string dir in m_TempDirs)
                {
                    if (Directory.Exists(dir))
                    {
                        Directory.Delete(dir, true);
                    }
                }
                m_TempDirs.Clear();
            }
            catch (Exception ex)
            {
                Globals.ReportError(ex);
            }
        }

    }
}
