using DazUnpacker.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DazUnpacker
{
    internal static class Statistics
    {
        private delegate void LabelInvoke(Label label, string value);

        private static int m_total;
        private static int m_done;

        public static int Total
        {
            get { return m_total; }
            set
            {
                m_total = value;
                UpdateFormCounter();
            }
        }
        public static int Done
        {
            get { return m_done; }
            set
            {
                m_done = value;
                UpdateFormCounter();
            }
        }

        public static void DoneIncrement()
        {
            int done = Interlocked.Increment(ref m_done);
            UpdateFormCounter(done, m_total);
        }

        private static void UpdateFormCounter(int done = -1, int total = -1)
        {
            if (done == -1)
            {
                done = m_done;
            }
            if (total == -1)
            {
                total = m_total;
            }
            SetLabelTextFramed(Content.form.labelProgress, done.ToString() + " / " + total.ToString());
        }

        private static void SetLabelTextFramed(Label label, string value)
        {
            if (label.InvokeRequired)
            {
                var invoke = new LabelInvoke(SetLabelText);
                label.BeginInvoke(invoke, label, value.ToString());
            }
            else
                SetLabelText(label, value.ToString());
        }

        private static void SetLabelText(Label label, string value)
        {
            label.Text = value;
            label.Refresh();
        }

        public static void Reset()
        {
            Done = 0;
            Total = 0;
        }

    }
}
