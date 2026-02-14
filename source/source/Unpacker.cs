using DazUnpacker.source;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DazUnpacker
{
    public static class Unpacker
    {
        public class UnpackInfo
        {
            public ContentTypes contentType;
            public bool contentTypeDetectIndividually;
            public List<string> contentFiles;
            public string contentLibDir;
            public string userSelectedRootDir;
            public bool ignoreRootDir;
            public string subfolderPrefix;
            public string rootFolderName;
            public GenesisVersion suggestedVersion;
            public GenesisGenders suggestedGender;
            public bool overwriteExisting;
            public int threadsCount;
        }

        public static void UnpackAll(UnpackInfo unpackInfo)
        {
            string baseTempDir;
            int indexTmpDir = 0;
            bool isFullInstall = !Config.Storage.TabInstaller.InstallDufOnly;

            ParallelOptions parallelOptions = new ParallelOptions();
            parallelOptions.MaxDegreeOfParallelism = unpackInfo.threadsCount;

            if (Archive.PrepareTempDir(unpackInfo.contentLibDir, out baseTempDir))
            {
                Parallel.ForEach(unpackInfo.contentFiles, parallelOptions, path =>
                {
                    try
                    {
                        if (File.Exists(path))
                        {
                            Log.Info("Extracting: " + path);

                            List<Archive.DufRootInfo> dufRootInfo;
                            List<string> rootDirList;
                            bool atLeastOneInstalled = false;

                            FindFailReason failReason;
                            int index = Interlocked.Increment(ref indexTmpDir);
                            string tempDir = baseTempDir + index.ToString();

                            FileSys.DirectoryDelete(tempDir);

                            ContentTypes contentType;
                            if (unpackInfo.contentTypeDetectIndividually)
                            {
                                contentType = Unpacker.DetectContentType(path);
                            }
                            else
                            {
                                contentType = unpackInfo.contentType;
                            }

                            if (contentType != ContentTypes.Unknown) // Earlier content type filter check to skip unpacking stage
                            {
                                if (!Config.Storage.TabInstaller.Filters.IsPassed(contentType))
                                {
                                    Library.Skipped.Add(path, FindFailReason.CriteriaNotMatch);
                                    return;
                                }
                            }

                            if (Archive.ExtractSmart(path, tempDir, true, !isFullInstall, out dufRootInfo, out rootDirList, out failReason))
                            {
                                if (contentType == ContentTypes.Unknown)
                                {
                                    foreach (Archive.DufRootInfo dufInfo in dufRootInfo)
                                    {
                                        contentType = Unpacker.DetectContentType(dufInfo.topDirectory);

                                        if (contentType != ContentTypes.Unknown)
                                        {
                                            break;
                                        }
                                    }
                                }

                                if (contentType == ContentTypes.Unknown)
                                {
                                    Library.Skipped.Add(path, FindFailReason.ContentTypeUndefined);
                                    return;
                                }

                                if (!Config.Storage.TabInstaller.Filters.IsPassed(contentType)) // second content type filter check
                                {
                                    Library.Skipped.Add(path, FindFailReason.CriteriaNotMatch);
                                    return;
                                }

                                bool bVersionIndependentType = Content.IsVersionIndependentType(contentType);

                                foreach (Archive.DufRootInfo dufInfo in dufRootInfo)
                                {
                                    dufInfo.contentType = contentType;

                                    if (bVersionIndependentType)
                                    {
                                        dufInfo.genesisVersion = GenesisVersion.Genesis_Any;
                                    }
                                    if (dufInfo.genesisVersion == GenesisVersion.Unknown)
                                    {
                                        dufInfo.genesisVersion = unpackInfo.suggestedVersion;
                                    }
                                    if (dufInfo.gender == GenesisGenders.Unknown)
                                    {
                                        dufInfo.gender = unpackInfo.suggestedGender;
                                    }
                                    string dirGroup;
                                    if (unpackInfo.ignoreRootDir)
                                    {
                                        dirGroup = "";
                                    }
                                    else
                                    {
                                        dirGroup = FileSys.StripFilename(path.Substring(unpackInfo.userSelectedRootDir.Length + 1));
                                    }

                                    dirGroup = FileSys.PathInsertPrefixToAllComponents(dirGroup, unpackInfo.subfolderPrefix);

                                    GenesisVersion genVersionsFiltered = dufInfo.genesisVersion & Config.Storage.TabSettings.genesisVersionFilter;
                                    bool hasSingleGenesisVersion = IsSingleGenesisVersion(dufInfo.genesisVersion);

                                    foreach (GenesisVersion genVersion in new GenesisVersion[] {
                                        GenesisVersion.Genesis_3, GenesisVersion.Genesis_8, GenesisVersion.Genesis_9
                                    })
                                    {
                                        if ((genVersionsFiltered & genVersion) != 0)
                                        {
                                            dufInfo.genesisVersion = genVersion;

                                            if (InstallDuf(unpackInfo.contentLibDir, dufInfo, unpackInfo.rootFolderName,
                                                dirGroup, unpackInfo.overwriteExisting, hasSingleGenesisVersion, out failReason))
                                            {
                                                atLeastOneInstalled = true;
                                            }
                                            else {
                                                Library.Skipped.Add(path, failReason);

                                                if (failReason == FindFailReason.AlreadyInstalled)
                                                {
                                                    atLeastOneInstalled = true;
                                                }
                                            }

                                            if (bVersionIndependentType)
                                            {
                                                break;
                                            }
                                        }
                                    }
                                }
                                if (atLeastOneInstalled && isFullInstall)
                                {
                                    List<string> topDirs = dufRootInfo.Select(x => x.topDirectory).ToList();

                                    if (!InstallRoot(unpackInfo.contentLibDir, rootDirList, topDirs, unpackInfo.overwriteExisting, out failReason))
                                    {
                                        Library.Skipped.Add(path, failReason);
                                    }
                                }
                            }
                            else
                            {
                                Library.Skipped.Add(path, failReason);
                            }
                            FileSys.DirectoryDelete(tempDir);
                        }
                    }
                    catch (Exception ex)
                    {
                        Globals.ReportError(ex, false);
                    }
                    finally
                    {
                        Statistics.DoneIncrement();
                    }
                });
            }
        }

        private static bool IsNodeOfTopDir(string dir, List<string> topDirs)
        {
            foreach (string topDir in topDirs)
            {
                if (topDir.StartsWith(dir, StringComparison.CurrentCultureIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

        private static bool InstallRoot(string contentLibDir, List<string> rootDirList, List<string> topDirs, bool overwrite, out FindFailReason failReason)
        {
            foreach (string dir in rootDirList)
            {
                foreach (string subDir in Directory.EnumerateDirectories(dir, "*.*", SearchOption.TopDirectoryOnly))
                {
                    if (!IsNodeOfTopDir(subDir, topDirs)) // exclude dir of duf
                    {
                        string fullInstDir = Path.Combine(contentLibDir, Path.GetFileName(subDir));

                        if (Directory.Exists(fullInstDir))
                        {
                            var status = FileSys.CopyFolderRecursively(subDir, fullInstDir, overwrite);
                            if (status == FileSys.FsCopyStatus.CopyFailed)
                            {
                                failReason = FindFailReason.AccessDenied;
                                return false;
                            }
                        }
                        else
                        {
                            Directory.Move(subDir, fullInstDir);
                        }
                    }
                }
            }
            failReason = FindFailReason.Unknown;
            return true;
        }

        private static bool InstallDuf(
            string contentLibDir,
            Archive.DufRootInfo dufInfo, 
            string rootDirName, 
            string dirGroup, 
            bool overwrite,
            bool hasSingleGenesisVersion,
            out FindFailReason failReason)
        {
            failReason = FindFailReason.Unknown;
            try
            {
                string targetInstDir = GetInstallDirByContentType(contentLibDir, dufInfo.contentType, dufInfo.genesisVersion, dufInfo.gender);
                if (!String.IsNullOrEmpty(rootDirName))
                {
                    targetInstDir += "\\" + rootDirName;
                }
                targetInstDir += "\\" + dirGroup;

                string dufParentName = Path.GetFileName(dufInfo.topDirectory);

                // For weird structure such as:
                // People\Genesis 9\Characters\CJ Anya.duf
                // People\Genesis 9\Characters\Daz Originals\CJ Anya\...
                if (String.Compare(dufParentName, "Characters", true) == 0 )
                {
                    dufParentName = "";
                }

                string fullInstDir = Path.Combine(targetInstDir, dufParentName);
                string parentInstDir = FileSys.GetParentDir(fullInstDir);

                if (!Directory.Exists(parentInstDir))
                {
                    Directory.CreateDirectory(parentInstDir);
                }
                if (Directory.Exists(fullInstDir) || !hasSingleGenesisVersion)
                {
                    var status = FileSys.CopyFolderRecursively(dufInfo.topDirectory, fullInstDir, overwrite);
                    if (status == FileSys.FsCopyStatus.CopySuccess)
                    {
                        return true;
                    }
                    if (status == FileSys.FsCopyStatus.CopyFailed)
                    {
                        failReason = FindFailReason.AccessDenied;
                        return false;
                    }
                    if (status == FileSys.FsCopyStatus.CopyNoChanges)
                    {
                        failReason = FindFailReason.AlreadyInstalled;
                    }
                    return false;
                }
                else
                {
                    Directory.Move(dufInfo.topDirectory, fullInstDir);
                    return true;
                }
            }
            catch (Exception ex)
            {
                failReason = FindFailReason.AccessDenied;
                Globals.ReportError(ex, false);
            }
            return false;
        }

        private static string GetInstallDirByContentType(string contentLibDir, ContentTypes contentType, GenesisVersion genesisVersion, GenesisGenders gender)
        {
            string middle = "";
            string genesisStr = "";
            string genesisIndexStr = "";

            if (genesisVersion == GenesisVersion.Genesis_3)
            {
                genesisIndexStr = " 3";
            }
            else if (genesisVersion == GenesisVersion.Genesis_8)
            {
                genesisIndexStr = " 8";
            }
            else if (genesisVersion == GenesisVersion.Genesis_8_1)
            {
                genesisIndexStr = " 8.1";
            }
            else if (genesisVersion == GenesisVersion.Genesis_9)
            {
                genesisIndexStr = " 9";
            }
            else
            {
                genesisIndexStr = "";
            }

            if (genesisVersion == GenesisVersion.Genesis_9)
            {
                genesisStr = "Genesis 9";
            }
            else {
                if (gender == GenesisGenders.Female)
                {
                    genesisStr = String.Format("Genesis{0} Female", genesisIndexStr);
                }
                else if (gender == GenesisGenders.Male)
                {
                    genesisStr = String.Format("Genesis{0} Male", genesisIndexStr);
                }
                else
                {
                    genesisStr = String.Format("Genesis{0}", genesisIndexStr);
                }
            }

            switch (contentType)
            {
                case ContentTypes.Accessory: { middle = String.Format("People\\{0}\\Accessories", genesisStr); break; }
                case ContentTypes.Anatomy: { middle = String.Format("People\\{0}\\Anatomy", genesisStr); break; }
                case ContentTypes.Animation: { middle = String.Format("People\\{0}\\Animations", genesisStr); break; }
                case ContentTypes.Character: { middle = String.Format("People\\{0}\\Characters", genesisStr); break; }
                case ContentTypes.Dress: { middle = String.Format("People\\{0}\\Clothing", genesisStr); break; }
                case ContentTypes.Environment: { middle = String.Format("Environments"); break; }
                case ContentTypes.Expression: { middle = String.Format("People\\{0}\\Expressions", genesisStr); break; }
                case ContentTypes.Hair: { middle = String.Format("People\\{0}\\Hair", genesisStr); break; }
                case ContentTypes.Light: { middle = String.Format("Light Presets"); break; }
                case ContentTypes.Material: { middle = String.Format("People\\{0}\\Materials", genesisStr); break; }
                case ContentTypes.Morph: { middle = String.Format("People\\{0}\\Shapes", genesisStr); break; }
                case ContentTypes.Pose: { middle = String.Format("People\\{0}\\Poses", genesisStr); break; }
                case ContentTypes.Prop: { middle = String.Format("People\\{0}\\Props", genesisStr); break; }
                case ContentTypes.Script: { middle = String.Format("scripts"); break; }
                default: { middle = "_Unknown"; break; }
            }

            return contentLibDir + "\\" + middle;
        }

        public static ContentTypes DetectContentType(string path)
        {
            ContentTypes contentType = ContentTypes.Unknown;

            if (Globals.StringContains(path, "Accessor") || Globals.StringContains(path, "Jewel"))
            {
                contentType = ContentTypes.Accessory;
            }
            else if (Globals.StringContains(path, "Anatom"))
            {
                contentType = ContentTypes.Anatomy;
            }
            else if (Globals.StringContains(path, "Animat"))
            {
                contentType = ContentTypes.Animation;
            }
            else if (Globals.StringContains(path, "Charact"))
            {
                contentType = ContentTypes.Character;
            }
            else if (Globals.StringContains(path, "Outfit") || Globals.StringContains(path, "Cloth") 
                || Globals.StringContains(path, "Dress") || Globals.StringContains(path, "Suit"))
            {
                contentType = ContentTypes.Dress;
            }
            else if (Globals.StringContains(path, "Environ") || Globals.StringContains(path, "Buildi"))
            {
                contentType = ContentTypes.Environment;
            }
            else if (Globals.StringContains(path, "Express"))
            {
                contentType = ContentTypes.Expression;
            }
            else if (Globals.StringContains(path, "Hair"))
            {
                if (Globals.StringContains(path, "Chair"))
                {
                    contentType = ContentTypes.Prop;
                }
                else
                {
                    contentType = ContentTypes.Hair;
                }
            }
            else if (Globals.StringContains(path, "Light") || Globals.StringContains(path, "IRay"))
            {
                contentType = ContentTypes.Light;
            }
            else if (Globals.StringContains(path, "Materia"))
            {
                contentType = ContentTypes.Material;
            }
            else if (Globals.StringContains(path, "Shape") || Globals.StringContains(path, "Morph"))
            {
                contentType = ContentTypes.Morph;
            }
            else if (Globals.StringContains(path, "Pose"))
            {
                contentType = ContentTypes.Pose;
            }
            else if (Globals.StringContains(path, "Prop"))
            {
                contentType = ContentTypes.Prop;
            }
            else if (Globals.StringContains(path, "Script"))
            {
                contentType = ContentTypes.Script;
            }
            return contentType;
        }

        private static bool IsGenesisX(string name, string version, out GenesisGenders gender)
        {
            gender = GenesisGenders.Unknown;

            if (name.IndexOf("female", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                gender = GenesisGenders.Female;
            }
            else if (name.IndexOf("male", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                gender = GenesisGenders.Male;
            }

            name = name.ToLower();
            if (name.Contains("g" + version + "f"))
            {
                gender = GenesisGenders.Female;
                return true;
            }
            if (name.Contains("g" + version + "m"))
            {
                gender = GenesisGenders.Male;
                return true;
            }
            if (name.Contains("gf" + version))
            {
                gender = GenesisGenders.Female;
                return true;
            }
            if (name.Contains("gm" + version))
            {
                gender = GenesisGenders.Male;
                return true;
            }
            if (name.Contains("genesis" + version))
            {
                return true;
            }
            if (name.Contains("genesis " + version))
            {
                return true;
            }
            if (name.Contains("genesis-" + version))
            {
                return true;
            }
            if (name.Contains("and " + version))
            {
                return true;
            }
            if (name.Contains(version + " fema"))
            {
                return true;
            }
            if (name.Contains(version + " mal"))
            {
                return true;
            }
            if (name.Contains("and-" + version))
            {
                return true;
            }
            if (name.Contains(version + "-fema"))
            {
                return true;
            }
            if (name.Contains(version + "-mal"))
            {
                return true;
            }
            if (name.Contains("and" + version))
            {
                return true;
            }
            if (name.Contains(version + "fema"))
            {
                return true;
            }
            if (name.Contains(version + "mal"))
            {
                return true;
            }
            if (version == "8")
            {
                if (IsGenesisX(name, "8.1", out gender))
                {
                    return true;
                }
                if (IsGenesisX(name, "81", out gender))
                {
                    return true;
                }
                if (IsGenesisX(name, "8_1", out gender))
                {
                    return true;
                }
            }
            return false;
        }

        public static GenesisVersion GetGenesisVersion(string name)
        {
            if (IsGenesisX(name, "8", out _))
            {
                return GenesisVersion.Genesis_8;
            }
            if (IsGenesisX(name, "9", out _))
            {
                return GenesisVersion.Genesis_9;
            }
            if (IsGenesisX(name, "3", out _))
            {
                return GenesisVersion.Genesis_3;
            }
            return GenesisVersion.Unknown;
        }

        public static bool IsSingleGenesisVersion(GenesisVersion version)
        {
            if (version == GenesisVersion.Unknown ||
                version == GenesisVersion.Genesis_Any)
                return false;

            int v = (int)version;
            return (v & (v - 1)) == 0;
        }

        public static bool IsGenesisFilterAllowed(GenesisVersion genesisVersion)
        {
            if (genesisVersion == GenesisVersion.Unknown)
            {
                return true;
            }
            if (0 != (genesisVersion & Config.Storage.TabSettings.genesisVersionFilter))
            {
                return true;
            }
            return false;
        }

        public static void GetGenesisVersionsByName(string name, out GenesisVersion genesisVersion, out GenesisGenders gender)
        {
            genesisVersion = GenesisVersion.Unknown;
            gender = GenesisGenders.Unknown;

            if (IsGenesisX(name, "8", out gender))
            {
                genesisVersion |= GenesisVersion.Genesis_8;
            }
            if (IsGenesisX(name, "9", out gender))
            {
                genesisVersion |= GenesisVersion.Genesis_9;
            }
            if (IsGenesisX(name, "3", out gender))
            {
                genesisVersion |= GenesisVersion.Genesis_3;
            }
        }

        public static bool IsGenesisFilterAllowed(string name, out GenesisVersion genesisVersion, out GenesisGenders gender)
        {
            genesisVersion = GenesisVersion.Unknown;
            gender = GenesisGenders.Unknown;

            if ((0 != (Config.Storage.TabSettings.genesisVersionFilter & GenesisVersion.Genesis_8)) && IsGenesisX(name, "8", out gender))
            {
                genesisVersion = GenesisVersion.Genesis_8;
                return true;
            }
            if ((0 != (Config.Storage.TabSettings.genesisVersionFilter & GenesisVersion.Genesis_9)) && IsGenesisX(name, "9", out gender))
            {
                genesisVersion = GenesisVersion.Genesis_9;
                return true;
            }
            if ((0 != (Config.Storage.TabSettings.genesisVersionFilter & GenesisVersion.Genesis_3)) && IsGenesisX(name, "3", out gender))
            {
                genesisVersion = GenesisVersion.Genesis_3;
                return true;
            }
            if (IsGenesisX(name, "8", out gender))
            {
                genesisVersion = GenesisVersion.Genesis_8;
                return false;
            }
            if (IsGenesisX(name, "9", out gender))
            {
                genesisVersion = GenesisVersion.Genesis_9;
                return false;
            }
            if (IsGenesisX(name, "3", out gender))
            {
                genesisVersion = GenesisVersion.Genesis_3;
                return false;
            }
            return true;
        }

    }
}
