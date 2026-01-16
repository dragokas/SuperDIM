using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DazUnpacker
{
    public static class Exclusion
    {
        static Form1 form;
        public static void Init(Form1 frm)
        {
            form = frm;
        }

        public static void Add(string item)
        {
            form.checkedListBoxSubfolderBlock.Items.Add(item, true);
        }

        public static void Remove(int item)
        {
            form.checkedListBoxSubfolderBlock.Items.RemoveAt(item);
        }

        public static void Select(int index)
        {
            form.checkedListBoxSubfolderBlock.SetItemChecked(index, true);
        }

        public static void UnSelectAll()
        {
            for (int i = 0; i < form.checkedListBoxSubfolderBlock.Items.Count; i++)
            {
                form.checkedListBoxSubfolderBlock.SetItemChecked(i, false);
            }
        }

        public static IEnumerable<string> GetList()
        {
            foreach (string item in form.checkedListBoxSubfolderBlock.Items)
            {
                yield return item;
            }
        }

        public static IEnumerable<int> GetIndeces()
        {
            for (int i = 0; i < form.checkedListBoxSubfolderBlock.Items.Count; i++)
            {
                if (form.checkedListBoxSubfolderBlock.GetItemChecked(i))
                {
                    yield return i;
                }
            }
        }

    }
}
