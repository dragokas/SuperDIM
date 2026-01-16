using DazUnpacker.source;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DazUnpacker
{
    [Flags]
    public enum GenesisVersion
    {
        Unknown = 0,
        Genesis_3 = 1,
        Genesis_8 = 2,
        Genesis_8_1 = 4,
        Genesis_9 = 8,
        Genesis_Any = -1
    }

    public enum GenesisGender
    {
        Unknown,
        Male,
        Female
    }

    public enum ContentType
    {
        Unknown,
        Accessory, // Jewelry
        Anatomy,
        Animation,
        Character,
        Dress, // Outfit
        Expression,
        Environment,
        Hair,
        Light,
        Material,
        Morph, // Shape
        Pose,
        Prop,
        Script
    }

    public static class Content
    {
        public static Form1 form;
        public static void Init(Form1 frm)
        {
            form = frm;
        }

        public static void AddFile(string file)
        {
            if (!Archive.IsArchiveExtension(file))
            {
                return;
            }
            FileInfo fi = new FileInfo(file);
            if (FilterManager.IsPassed(fi.CreationTime))
            {
                Library.Content.Add(Path.GetFileName(file), Path.GetDirectoryName(file), 0, "", true);
            }
        }

        public static void AddObject(string obj)
        {
            if (File.Exists(obj))
            {
                Content.AddFile(obj);
            }
            else if (Directory.Exists(obj))
            {
                Content.AddFolder(obj);
            }
        }

        public static void AddFolder(string folder)
        {
            var list = FileSys.EnumDirectory(folder + "\\*.*").ToList();
            foreach (string file in list)
            {
                AddFile(file);
            }
        }

        public static List<string> GetParentFolders()
        {
            List<string> list = new List<string>();
            foreach (var file in Library.Content.GetFiles())
            {
                string parentDir = FileSys.GetParentDir(file);
                if (!list.Contains(parentDir) && Directory.Exists(parentDir))
                {
                    list.Add(parentDir);
                }
            }
            List<string> grandParentList = new List<string>();
            foreach (string dir in list)
            {
                bool hasEntry = false;
                string parentDir = dir;
                do
                {
                    parentDir = FileSys.GetParentDir(parentDir);
                    if (!Directory.Exists(parentDir))
                    {
                        parentDir = "";
                    }
                    hasEntry = grandParentList.Contains(parentDir);
                    if (!hasEntry)
                    {
                        grandParentList.Add(parentDir);
                    }
                }
                while (!hasEntry);
            }
            list.AddRange(grandParentList.ToArray());
            return list;
        }
        public static bool IsVersionIndependentType(ContentType type)
        {
            switch (type)
            {
                case ContentType.Environment:
                case ContentType.Light:
                case ContentType.Prop:
                case ContentType.Script:
                    return true;
            }
            return false;
        }

        
    }
}
