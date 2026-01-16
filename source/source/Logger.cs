using System;
using System.Text;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Threading;
using System.IO;
using System.Windows.Forms;
using System.Linq;

namespace DazUnpacker
{
    public static class Log
    {
        public enum LOG_NOISE_LEVEL : int
        {
            Minimum = 1,
            Average = 2,
            Noisy = 3
        }

        [Flags]
        public enum LOG_LOCATION : int
        {
            LOG_LOCATION_NONE = 0,
            LOG_LOCATION_CONSOLE = 1,
            LOG_LOCATION_FILE = 2,
            LOG_LOCATION_DEBUG_OUTPUT_STRING = 4,
            LOG_LOCATION_FORM = 8,
            LOG_LOCATION_FORM_RETRIEVE_MANUALLY = 16,
            LOG_LOCATION_ALL = -1
        }

        public enum LogTag
        {
            Info,
            Warning,
            Error
        }

        private const int m_SizeFlushCache = 16 * 1024;
        private const int m_SizeMaxCache = 32 * 1024;

        private static LOG_LOCATION LogLocation =
            LOG_LOCATION.LOG_LOCATION_ALL;
        private static LOG_NOISE_LEVEL LogNoiseLevel =
            LOG_NOISE_LEVEL.Noisy;

        public static string LogPath { get; set; } = "";

        private static ReaderWriterLock locker = new ReaderWriterLock();
        private static FileStream fs;
        private static UTF8Encoding utf8 = new UTF8Encoding();
        private static byte[] crlf = new byte[] { 13, 10 };
        private static ListBox listboxReport = new ListBox();
        private static string TagInfo = "| INFO |";
        private static string TagError = "| ERROR |";
        private static string TagWarning = "| WARNING |";
        private static StringBuilder sbCache = new StringBuilder(m_SizeFlushCache, m_SizeMaxCache);
        private static List<string> m_ListFormLog = new List<string>();
        private static int m_FormMaxLines = 300;
        private static readonly object m_lockCache = new object();
        private static readonly object m_lockUI = new object();
        private static bool m_DisableCache = false;

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        static extern void OutputDebugString(string lpOutputString);

        public static void Init(bool deleteOld)
        {
            string appdir = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory);
            LogPath = Path.Combine((appdir != null ? appdir : "").Replace("file:\\", ""), "app.log");

            try
            {
                locker.AcquireWriterLock(Timeout.Infinite);

                if (deleteOld)
                {
                    if (File.Exists(LogPath))
                    {
                        File.Delete(LogPath);
                    }
                }
                fs = File.Open(LogPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read);
                fs?.Seek(0, SeekOrigin.End);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
            }
            finally
            {
                if (locker.IsWriterLockHeld)
                {
                    locker.ReleaseWriterLock();
                }
            }
        }

        public static void SetLogLocation(LOG_LOCATION logLocation)
        {
            try
            {
                LogLocation = logLocation;
            }
            finally { }
        }

        public static void SetLogNoiseLevel(LOG_NOISE_LEVEL level)
        {
            try
            {
                LogNoiseLevel = level;
            }
            finally { }
        }

        public static void SetFormMaxLines(int maxLines)
        {
            try
            {
                m_FormMaxLines = maxLines;
            }
            finally { }
        }

        public static void SetControlForReport(ListBox listbox)
        {
            listboxReport = listbox;
        }

        internal static void FlushReportUI()
        {
            lock (m_lockUI)
            {
                if (listboxReport.Items.Count > m_FormMaxLines)
                {
                    listboxReport.Items.Clear();
                }
            }
        }

        internal static void DisableCache(bool disable)
        {
            m_DisableCache = disable;
        }

        delegate void FormDelegate(string data);
        private static void WriteOnFormThread(string data)
        {
            if (m_FormMaxLines == 0)
            {
                return;
            }
            FlushReportUI();
            listboxReport.Items.Add(data);
            listboxReport.SelectedIndex = listboxReport.Items.Count - 1;
        }

        // Use it to queue on thread
        private static void WriteOnForm(string data)
        {
            if (listboxReport.InvokeRequired)
            {
                var invoke = new FormDelegate(WriteOnFormThread);
                listboxReport.BeginInvoke(invoke, data); // "BeginInvoke" doesn't block UI thread, unlike "Invoke"
            }
            else
                WriteOnFormThread(data);
        }

        public static void Flush()
        {
            try
            {
                locker.AcquireWriterLock(Timeout.Infinite);

                if (fs != null)
                {
                    var bytes = utf8.GetBytes(sbCache.ToString());
                    fs.Write(bytes, 0, bytes.Length);
                    fs.Flush();
                    sbCache.Clear();
                }
            }
            finally
            {
                if (locker.IsWriterLockHeld)
                {
                    locker.ReleaseWriterLock();
                }
            }
        }

        private static void WriteCache(string text)
        {
            try
            {
                lock (m_lockCache)
                {
                    sbCache.AppendLine(text);

                    if (sbCache.Length > m_SizeFlushCache || m_DisableCache)
                    {
                        Flush();
                    }
                }
            }
            finally { }
        }

        public static void WriteLog(string data)
        {
            if ((LogLocation & LOG_LOCATION.LOG_LOCATION_FILE) != 0)
            {
                WriteCache(data);
            }
            if ((LogLocation & LOG_LOCATION.LOG_LOCATION_CONSOLE) != 0)
            {
                Console.WriteLine(data);
            }
            if ((LogLocation & LOG_LOCATION.LOG_LOCATION_DEBUG_OUTPUT_STRING) != 0)
            {
                System.Diagnostics.Debug.WriteLine(data);
            }
            if ((LogLocation & LOG_LOCATION.LOG_LOCATION_FORM) != 0)
            {
                if ((LogLocation & LOG_LOCATION.LOG_LOCATION_FORM_RETRIEVE_MANUALLY) != 0)
                {
                    m_ListFormLog.Add(data);
                }
                else
                {
                    WriteOnForm(data);
                }
            }
        }

        public static void Info(string text)
        {
            try
            {
                locker.AcquireWriterLock(Timeout.Infinite);
                string data = String.Format("> {0}.{1:D3} {2} {3}", DateTime.Now, DateTime.Now.Millisecond, TagInfo, text);
                WriteLog(data);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
            }
            finally
            {
                if (locker.IsWriterLockHeld)
                {
                    locker.ReleaseWriterLock();
                }
            }
        }

        public static void Info(string text, string prefix)
        {
            try
            {
                locker.AcquireWriterLock(Timeout.Infinite);
                string data = String.Format("> {0}{1}.{2:D3} {3} {4}", prefix, DateTime.Now, DateTime.Now.Millisecond, TagInfo, text);
                WriteLog(data);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
            }
            finally
            {
                if (locker.IsWriterLockHeld)
                {
                    locker.ReleaseWriterLock();
                }
            }
        }
        public static void Info(LOG_NOISE_LEVEL noiseLevel, string text)
        {
            if (LogNoiseLevel < noiseLevel)
            {
                return;
            }
            try
            {
                locker.AcquireWriterLock(Timeout.Infinite);
                string data = String.Format("> {0}.{1:D3} {2} {3}", DateTime.Now, DateTime.Now.Millisecond, TagInfo, text);
                WriteLog(data);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
            }
            finally
            {
                if (locker.IsWriterLockHeld)
                {
                    locker.ReleaseWriterLock();
                }
            }
        }

        public static void Warning(string text)
        {
            try
            {
                locker.AcquireWriterLock(Timeout.Infinite);
                string data = String.Format("> {0}.{1:D3} {2} {3}", DateTime.Now, DateTime.Now.Millisecond, TagWarning, text);
                WriteLog(data);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
            }
            finally
            {
                if (locker.IsWriterLockHeld)
                {
                    locker.ReleaseWriterLock();
                }
            }
        }

        public static void Error(string text)
        {
            try
            {
                locker.AcquireWriterLock(Timeout.Infinite);
                string data = String.Format("> {0}.{1:D3} {2} {3}", DateTime.Now, DateTime.Now.Millisecond, TagError, text);
                WriteLog(data);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
            }
            finally
            {
                if (locker.IsWriterLockHeld)
                {
                    locker.ReleaseWriterLock();
                }
            }
        }

        

        public static void Trace(string text)
        {
            if ((LogLocation & LOG_LOCATION.LOG_LOCATION_DEBUG_OUTPUT_STRING) == 0)
            {
                return;
            }
            try
            {
                locker.AcquireWriterLock(Timeout.Infinite);
                //OutputDebugString(text);
                System.Diagnostics.Debug.WriteLine(String.Format("{0}.{1:D3} | TRACE | {2}", DateTime.Now, DateTime.Now.Millisecond, text));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
            }
            finally
            {
                if (locker.IsWriterLockHeld)
                {
                    locker.ReleaseWriterLock();
                }
            }
        }

        public static string ReadLogFile()
        {
            try
            {
                Flush();
                locker.AcquireReaderLock(Timeout.Infinite);

                if (fs != null)
                {
                    Byte[] buffer = new Byte[fs.Length];
                    long position = fs.Position;
                    fs.Seek(0, SeekOrigin.Begin);
                    fs.Read(buffer, 0, buffer.Length);
                    fs.Seek(position, SeekOrigin.Begin);
                    return Encoding.UTF8.GetString(buffer);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
            }
            finally
            {
                if (locker.IsReaderLockHeld)
                {
                    locker.ReleaseReaderLock();
                }
            }
            return String.Empty;
        }

        // must be called on form's thread!
        public static void FlushFormLog()
        {
            try
            {
                bool bShouldClearForm = false;
                int linesToWrite = 0;
                List<string> listLogLines = new List<string>();
                try
                {
                    locker.AcquireReaderLock(Timeout.Infinite);

                    int countInLog = m_ListFormLog.Count;
                    if (countInLog == 0)
                    {
                        return;
                    }
                    int countOnForm = listboxReport.Items.Count;
                    int totalLines = countOnForm + countInLog;
                    if (totalLines > m_FormMaxLines)
                    {
                        bShouldClearForm = true;
                        linesToWrite = totalLines % m_FormMaxLines;
                    }
                    else
                    {
                        linesToWrite = countInLog;
                    }
                    //listLogLines = TakeLast(m_ListFormLog, linesToWrite).ToList();
                    listLogLines = m_ListFormLog;
                    m_ListFormLog = new List<string>();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message + ex.StackTrace);
                }
                finally
                {
                    if (locker.IsReaderLockHeld)
                    {
                        locker.ReleaseReaderLock();
                    }
                }
                if (bShouldClearForm)
                {
                    listboxReport.Items.Clear();
                }
                listboxReport.Items.AddRange(TakeLast(listLogLines, linesToWrite).ToArray());
                listboxReport.SelectedIndex = listboxReport.Items.Count - 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
            }
        }

        public static IEnumerable<T> TakeLast<T>(this IEnumerable<T> source, int N)
        {
            return source.Skip(Math.Max(0, source.Count() - N));
        }

        public static string GetTag(LogTag logTag)
        {
            if (logTag == LogTag.Info)
            {
                return TagInfo;
            }
            else if (logTag == LogTag.Warning)
            {
                return TagWarning;
            }
            else if (logTag == LogTag.Error)
            {
                return TagError;
            }
            return String.Empty;
        }

        public static void Clear()
        {
            Release();
            lock (m_lockUI)
            {
                listboxReport.Items.Clear();
            }
            Init(true);
        }

        public static void Release()
        {
            try
            {
                if (fs != null)
                {
                    Flush();
                    fs.Close();
                    fs.Dispose();
                    fs = null;
                }
                m_ListFormLog.Clear();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
            }
        }
    }
}
