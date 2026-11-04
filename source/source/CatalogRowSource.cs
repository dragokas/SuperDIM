using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DazUnpacker
{
    // Rows for the virtual grid
    internal interface ICatalogRowSource
    {
        int Count { get; }

        // Raised on the UI thread when Count or the order of rows has been changed
        event EventHandler Changed;

        CatalogRow GetRow(int index);

        // Changes the order of rows; may complete asynchronously (Changed is raised then)
        void Sort(CatalogColumn column, bool ascending);
    }

    // Rows which are already in memory (search result windows)
    internal class ListRowSource : ICatalogRowSource
    {
        private List<CatalogRow> rows;

        public event EventHandler Changed;

        public ListRowSource(IEnumerable<CatalogRow> rows)
        {
            this.rows = rows.ToList();
        }

        public int Count
        {
            get { return rows.Count; }
        }

        public CatalogRow GetRow(int index)
        {
            return rows[index];
        }

        public void Sort(CatalogColumn column, bool ascending)
        {
            if (ascending)
            {
                rows = rows.OrderBy(r => r.GetValue(column), StringComparer.Ordinal).ToList();
            }
            else
            {
                rows = rows.OrderByDescending(r => r.GetValue(column), StringComparer.Ordinal).ToList();
            }
            if (Changed != null)
            {
                Changed(this, EventArgs.Empty);
            }
        }
    }

    // Rows which are read from the catalog database page by page. The filter and the order are applied by the database.
    internal class DbRowSource : ICatalogRowSource
    {
        private const int PageSize = 100;
        private const int MaxCachedPages = 40;

        private readonly Control uiControl;
        private readonly object cacheLock = new object();
        private readonly Dictionary<int, List<CatalogRow>> pages = new Dictionary<int, List<CatalogRow>>();
        private readonly Queue<int> pageOrder = new Queue<int>();

        private long[] ids = new long[0];
        private string filter = "";
        private bool ignoreCase;
        private CatalogColumn? sortColumn;
        private bool sortAscending = true;
        private int requestVersion;
        private CancellationTokenSource cts;

        public event EventHandler Changed;

        // Raised on the UI thread: true when a query starts, false when it ends
        public event Action<bool> Loading;

        public DbRowSource(Control uiControl)
        {
            this.uiControl = uiControl;
        }

        public int Count
        {
            get { return ids.Length; }
        }

        public CatalogRow GetRow(int index)
        {
            int page = index / PageSize;
            List<CatalogRow> rows = null;

            lock (cacheLock)
            {
                pages.TryGetValue(page, out rows);
            }
            if (rows == null)
            {
                rows = CatalogDb.GetRows(ids, page * PageSize, PageSize);
                lock (cacheLock)
                {
                    pages[page] = rows;
                    pageOrder.Enqueue(page);
                    while (pageOrder.Count > MaxCachedPages)
                    {
                        pages.Remove(pageOrder.Dequeue());
                    }
                }
            }
            int offset = index - page * PageSize;
            return offset < rows.Count ? rows[offset] : new CatalogRow();
        }

        public void Sort(CatalogColumn column, bool ascending)
        {
            sortColumn = column;
            sortAscending = ascending;
            Reload(filter, ignoreCase);
        }

        // Runs the query in the background; the result of an outdated query is dropped
        public void Reload(string filter, bool ignoreCase)
        {
            this.filter = filter;
            this.ignoreCase = ignoreCase;

            if (cts != null)
            {
                cts.Cancel();
            }
            CancellationTokenSource current = new CancellationTokenSource();
            cts = current;
            int version = ++requestVersion;
            CatalogColumn? column = sortColumn;
            bool ascending = sortAscending;

            RaiseLoading(true);
            Task.Run(() =>
            {
                try
                {
                    long[] result = CatalogDb.QueryIds(filter, ignoreCase, column, ascending, current.Token);
                    PostToUi(() => Apply(version, result));
                }
                catch (OperationCanceledException)
                {
                    // a newer query has been started
                }
                catch (Exception ex)
                {
                    Globals.ReportError(ex, false, "Catalog query");
                    PostToUi(() => Apply(version, new long[0]));
                }
            });
        }

        private void Apply(int version, long[] result)
        {
            if (version != requestVersion)
            {
                return;
            }
            lock (cacheLock)
            {
                pages.Clear();
                pageOrder.Clear();
            }
            ids = result;
            RaiseLoading(false);
            if (Changed != null)
            {
                Changed(this, EventArgs.Empty);
            }
        }

        private void RaiseLoading(bool loading)
        {
            if (Loading != null)
            {
                Loading(loading);
            }
        }

        private void PostToUi(Action action)
        {
            if (!uiControl.IsDisposed && uiControl.IsHandleCreated)
            {
                uiControl.BeginInvoke(action);
            }
        }
    }
}
