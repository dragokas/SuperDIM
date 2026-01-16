using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DazUnpacker
{
    internal class Geometry
    {
        internal static Control FindControlAtPoint(Control container, Point pos)
        {
            Control child;
            foreach (Control c in container.Controls)
            {
                if (c.Visible && c.Bounds.Contains(pos))
                {
                    child = FindControlAtPoint(c, new Point(pos.X - c.Left, pos.Y - c.Top));
                    if (child == null) return c;
                    else return child;
                }
            }
            return null;
        }

        internal static Control FindControlAtCursor(Form form)
        {
            Point pos = Cursor.Position;
            if (form.Bounds.Contains(pos))
                return FindControlAtPoint(form, form.PointToClient(pos));
            return null;
        }

        internal static bool IsContainerAtCursor(Form form, Control container)
        {
            Control c = FindControlAtCursor(form);
            if (c != null)
            {
                do
                {
                    if (Control.ReferenceEquals(c, container))
                    {
                        return true;
                    }
                    c = c.Parent;
                }
                while (c != null);
            }
            return false;
        }
    }
}
