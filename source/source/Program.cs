using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DazUnpacker
{
    internal static class Program
    {
        private static Mutex mutex = null;
        private const string MutexName = "SuperDIM_mtx";

        [STAThread]
        static void Main()
        {
            bool createdNew;
            mutex = new Mutex(true, MutexName, out createdNew);

            if (!createdNew)
            {
                MessageBox.Show("Application is already running!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);

            // The error goes to the log and to a message box instead of the standard "Unhandled exception" dialog
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += (sender, e) => ReportUnhandled(e.Exception);
            AppDomain.CurrentDomain.UnhandledException += (sender, e) => ReportUnhandled(e.ExceptionObject as Exception);

            Application.Run(new Form1());

            mutex.ReleaseMutex();
        }

        private static void ReportUnhandled(Exception ex)
        {
            if (ex != null)
            {
                Globals.ReportError(ex, true, "Unhandled exception");
            }
        }
    }
}
