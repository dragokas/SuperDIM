using DazUnpacker.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DazUnpacker
{
    internal static class Globals
    {
        internal static string AppDir = GetAppDir();
        internal static string AppName = "SuperDIM";

        internal static void Init()
        {
        }

        private static string GetAppDir()
        {
            string appdir = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory);
            return (appdir != null ? appdir : "").Replace("file:\\", "");
        }

        public static string FormatException(Exception ex)
        {
            string s;
            s = "Error: " + ex.ToString() + "\n";
            if (ex.InnerException != null)
            {
                s += "Inner error: " + ex.InnerException.Message.ToString() + "\n";
            }
            if (ex.StackTrace != null)
            {
                s += "Stack: " + ex.StackTrace.ToString();
            }
            return s;
        }

        public static void ReportError(Exception ex, bool interactive = true, string additionalContext = "")
        {
            string s = FormatException(ex);
            if (!string.IsNullOrEmpty(additionalContext))
            {
                s = additionalContext + ", " + s;
            }
            Log.Error(s);
            if (interactive)
            {
                MessageBox.Show(s, Globals.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        internal static bool StringContains(string str, string value)
        {
            return str.IndexOf(value, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        internal static void ClearTempFolder()
        {
            try
            {
                if (Directory.Exists(Config.Data.TempFolder))
                {
                    Directory.Delete(Config.Data.TempFolder, true);
                }
                if (!Directory.Exists(Config.Data.TempFolder))
                {
                    Directory.CreateDirectory(Config.Data.TempFolder);
                }
            }
            catch (Exception ex)
            {
                Globals.ReportError(ex, false);
            }
        }

        public static void OpenTextFile(string path)
        {
            try
            {
                Process.Start(new ProcessStartInfo() { FileName = path, UseShellExecute = true });
            }
            catch (Exception ex) { Globals.ReportError(ex); }
        }
    }
}
