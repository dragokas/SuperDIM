using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace DazUnpacker
{
    // Collects the file list of a bundle while the installer unpacks it (no second unpacking is needed)
    internal class CatalogCollector
    {
        internal readonly string BundlePath;
        internal readonly List<CatalogRow> Rows = new List<CatalogRow>();
        private readonly BundleStamp stamp; // the state of the file before it is read

        internal CatalogCollector(string bundlePath)
        {
            BundlePath = bundlePath;
            stamp = BundleStamp.FromFile(bundlePath);
        }

        // Lists the archive (outer or nested) right before it is unpacked.
        // Nested archives are skipped, because the installer unpacks and lists them separately.
        internal void Add(string archivePath)
        {
            List<string> entries = new List<string>();
            if (!ArchiveLister.ListFiles(archivePath, entries))
            {
                return;
            }
            lock (Rows)
            {
                foreach (string entry in entries)
                {
                    if (!Archive.IsArchiveExtension(entry))
                    {
                        Rows.Add(CatalogRow.Create(entry, BundlePath));
                    }
                }
            }
        }

        // Writes the collected rows to the catalog database
        internal void Save()
        {
            lock (Rows)
            {
                if (Rows.Count != 0)
                {
                    CatalogDb.ReplaceBundle(BundlePath, Rows, stamp);
                }
            }
        }
    }

    // Scans a folder with bundles and stores the list of their files in the catalog database
    internal class CatalogScanner
    {
        private const int MaxNestingDepth = 8;

        private readonly string workRoot;
        private readonly int threadsCount;
        private readonly Action<int, int> progress;
        private int doneCount;
        private int skippedCount;

        // Number of bundles of the last Run which were not read, because they had not been changed since the previous scan
        internal int SkippedCount
        {
            get { return skippedCount; }
        }

        // progress(done, total) is called from worker threads
        internal CatalogScanner(int threadsCount, Action<int, int> progress)
        {
            this.threadsCount = Math.Max(1, threadsCount);
            this.progress = progress;
            workRoot = Path.Combine(GetTempRoot(), "Catalog");
        }

        // Temp folder from the "Temporarily folder" group, otherwise the default one
        internal static string GetTempRoot()
        {
            if (Config.Storage.TabSettings.UseSpecificTempFolder && Directory.Exists(Config.Storage.TabSettings.TempFolder))
            {
                return Config.Storage.TabSettings.TempFolder;
            }
            return Config.Data.TempFolder;
        }

        // Returns the number of bundles found; throws OperationCanceledException when cancelled
        internal int Run(string folder, CancellationToken token)
        {
            List<string> bundles = ArchiveLister.FindArchives(folder);
            int total = bundles.Count;
            doneCount = 0;
            skippedCount = 0;
            Dictionary<string, BundleStamp> knownStamps = CatalogDb.LoadBundleStamps();
            progress(0, total);

            ParallelOptions options = new ParallelOptions();
            options.MaxDegreeOfParallelism = threadsCount;
            options.CancellationToken = token;

            Parallel.ForEach(bundles, options, bundle =>
            {
                try
                {
                    BundleStamp stamp = BundleStamp.FromFile(bundle);
                    BundleStamp knownStamp;
                    if (knownStamps.TryGetValue(bundle, out knownStamp) && knownStamp.IsSameAs(stamp))
                    {
                        Interlocked.Increment(ref skippedCount);
                        return;
                    }
                    List<CatalogRow> rows = ScanBundle(bundle, token);
                    if (rows != null)
                    {
                        CatalogDb.ReplaceBundle(bundle, rows, stamp);
                    }
                }
                catch (OperationCanceledException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    Globals.ReportError(ex, false, "Catalog scan: " + bundle);
                }
                finally
                {
                    progress(Interlocked.Increment(ref doneCount), total);
                }
            });
            return total;
        }

        // Rows for all files of the bundle, null if the bundle cannot be read
        private List<CatalogRow> ScanBundle(string bundlePath, CancellationToken token)
        {
            List<CatalogRow> rows = new List<CatalogRow>();
            if (!ScanArchive(bundlePath, bundlePath, rows, 0, token))
            {
                return null;
            }
            return rows;
        }

        // Lists the archive without unpacking; only nested archives (if any) are unpacked and scanned recursively
        private bool ScanArchive(string archivePath, string bundlePath, List<CatalogRow> rows, int depth, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            List<string> entries = new List<string>();
            if (!ArchiveLister.ListFiles(archivePath, entries))
            {
                return false;
            }

            List<string> nestedEntries = new List<string>();
            foreach (string entry in entries)
            {
                if (Archive.IsArchiveExtension(entry))
                {
                    nestedEntries.Add(entry);
                }
                else
                {
                    rows.Add(CatalogRow.Create(entry, bundlePath));
                }
            }
            if (nestedEntries.Count == 0)
            {
                return true; // a leaf archive
            }
            if (depth >= MaxNestingDepth || !ScanNestedArchives(archivePath, bundlePath, rows, depth, token))
            {
                // Nested archives cannot be opened: keep them as ordinary files
                foreach (string entry in nestedEntries)
                {
                    rows.Add(CatalogRow.Create(entry, bundlePath));
                }
            }
            return true;
        }

        private bool ScanNestedArchives(string archivePath, string bundlePath, List<CatalogRow> rows, int depth, CancellationToken token)
        {
            string dir = Path.Combine(workRoot, Guid.NewGuid().ToString("N"));
            try
            {
                Directory.CreateDirectory(dir);
                if (!ArchiveLister.ExtractNestedArchives(archivePath, dir))
                {
                    return false;
                }
                List<string> nestedFiles = ArchiveLister.FindArchives(dir);
                if (nestedFiles.Count == 0)
                {
                    return false;
                }
                foreach (string nestedFile in nestedFiles)
                {
                    List<CatalogRow> nestedRows = new List<CatalogRow>();
                    if (ScanArchive(nestedFile, bundlePath, nestedRows, depth + 1, token))
                    {
                        rows.AddRange(nestedRows);
                    }
                    else
                    {
                        // keep the unreadable archive itself as a file
                        rows.Add(CatalogRow.Create(nestedFile.Substring(dir.Length), bundlePath));
                    }
                    FileSys.FileDelete(nestedFile); // free the disk space as early as possible
                }
                return true;
            }
            finally
            {
                FileSys.DirectoryDelete(dir);
            }
        }
    }
}
