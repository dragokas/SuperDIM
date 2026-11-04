using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Text;
using System.Threading;

namespace DazUnpacker
{
    // Columns of the Files table which are visible to the user (order matches the grid columns)
    public enum CatalogColumn
    {
        FilePath,
        FileName,
        FileType,
        ArchivePath,
        ArchiveName,
        ArchiveType
    }

    // One file inside a bundle (the bundle is an archive file on disk)
    public class CatalogRow
    {
        public string FilePath = "";
        public string FileName = "";
        public string FileType = "";
        public string ArchivePath = "";
        public string ArchiveName = "";
        public string ArchiveType = "";

        // Builds the row from a path inside the archive (any slashes) and the bundle path
        public static CatalogRow Create(string pathInArchive, string bundlePath)
        {
            string filePath = NormalizePath(pathInArchive);
            CatalogRow row = new CatalogRow();
            row.FilePath = filePath;
            row.FileName = filePath.Substring(filePath.LastIndexOf('/') + 1);
            row.FileType = GetExtension(row.FileName);
            row.ArchivePath = bundlePath;
            row.ArchiveName = Path.GetFileName(bundlePath);
            row.ArchiveType = GetExtension(row.ArchiveName);
            return row;
        }

        // Forward slashes and the leading slash, e.g. "/Poses/Name/file.duf"
        public static string NormalizePath(string path)
        {
            path = path.Trim().Replace('\\', '/');
            if (!path.StartsWith("/"))
            {
                path = "/" + path;
            }
            return path;
        }

        // Lower-case extension without dot
        private static string GetExtension(string name)
        {
            int pos = name.LastIndexOf('.');
            if (pos < 0 || pos == name.Length - 1)
            {
                return "";
            }
            return name.Substring(pos + 1).ToLowerInvariant();
        }

        public string GetValue(CatalogColumn column)
        {
            switch (column)
            {
                case CatalogColumn.FilePath: return FilePath;
                case CatalogColumn.FileName: return FileName;
                case CatalogColumn.FileType: return FileType;
                case CatalogColumn.ArchivePath: return ArchivePath;
                case CatalogColumn.ArchiveName: return ArchiveName;
                default: return ArchiveType;
            }
        }
    }

    // Size and modification time of a bundle at the moment its file list was read
    public class BundleStamp
    {
        public long Size;
        public long ModifiedTicks;

        // null if the file does not exist
        public static BundleStamp FromFile(string path)
        {
            FileInfo info = new FileInfo(path);
            if (!info.Exists)
            {
                return null;
            }
            BundleStamp stamp = new BundleStamp();
            stamp.Size = info.Length;
            stamp.ModifiedTicks = info.LastWriteTimeUtc.Ticks;
            return stamp;
        }

        public bool IsSameAs(BundleStamp other)
        {
            return other != null && Size == other.Size && ModifiedTicks == other.ModifiedTicks;
        }
    }

    // Rows found in the catalog for one line typed by the user
    public class CatalogTailMatch
    {
        public string Tail = "";
        public List<CatalogRow> Rows = new List<CatalogRow>();
    }

    public static class CatalogDb
    {
        private const string SelectColumns = "FilePath, FileName, FileType, ArchivePath, ArchiveName, ArchiveType";

        private static readonly object writeLock = new object();
        private static string connectionString = "";

        public static string DbPath { get; private set; } = "";

        public static void Init(string dbPath)
        {
            DbPath = dbPath;
            connectionString = new SQLiteConnectionStringBuilder
            {
                DataSource = dbPath,
                Version = 3,
                Pooling = true,
                JournalMode = SQLiteJournalModeEnum.Wal,
                SyncMode = SynchronizationModes.Normal,
                DefaultTimeout = 60 // seconds, also the busy timeout
            }.ToString();

            lock (writeLock)
            {
                using (SQLiteConnection con = OpenConnection())
                {
                    Execute(con,
                        "CREATE TABLE IF NOT EXISTS Files(" +
                        "Id INTEGER PRIMARY KEY, " +
                        "FilePath TEXT NOT NULL, " +
                        "FileName TEXT NOT NULL, " +
                        "FileType TEXT NOT NULL, " +
                        "ArchivePath TEXT NOT NULL, " +
                        "ArchiveName TEXT NOT NULL, " +
                        "ArchiveType TEXT NOT NULL, " +
                        "PathKey TEXT NOT NULL, " + // lower-cased FilePath, for "Ignore case" search
                        "NameKey TEXT NOT NULL)"); // lower-cased FileName, for the lookup by name
                    Execute(con, "CREATE INDEX IF NOT EXISTS ix_files_namekey ON Files(NameKey)");
                    Execute(con, "CREATE INDEX IF NOT EXISTS ix_files_archive ON Files(ArchivePath)");
                    // bundles whose file list is in the catalog; used to skip unchanged archives on the next scan
                    Execute(con,
                        "CREATE TABLE IF NOT EXISTS Bundles(" +
                        "ArchivePath TEXT PRIMARY KEY, " +
                        "Size INTEGER NOT NULL, " +
                        "ModifiedTicks INTEGER NOT NULL)");
                }
            }
        }

        private static SQLiteConnection OpenConnection()
        {
            SQLiteConnection con = new SQLiteConnection(connectionString);
            con.Open();
            return con;
        }

        private static void Execute(SQLiteConnection con, string sql)
        {
            using (SQLiteCommand cmd = new SQLiteCommand(sql, con))
            {
                cmd.ExecuteNonQuery();
            }
        }

        private static string ColumnSql(CatalogColumn column)
        {
            return column.ToString(); // enum names are the column names, so no injection is possible
        }

        public static long Count()
        {
            using (SQLiteConnection con = OpenConnection())
            using (SQLiteCommand cmd = new SQLiteCommand("SELECT COUNT(*) FROM Files", con))
            {
                return Convert.ToInt64(cmd.ExecuteScalar());
            }
        }

        // Atomically replaces everything known about the bundle.
        // stamp is the state of the bundle file when its content was read (null: not remembered, the bundle is rescanned next time).
        public static void ReplaceBundle(string bundlePath, IList<CatalogRow> rows, BundleStamp stamp)
        {
            lock (writeLock)
            {
                using (SQLiteConnection con = OpenConnection())
                using (SQLiteTransaction tran = con.BeginTransaction())
                {
                    DeleteBundle(con, bundlePath);
                    InsertRows(con, rows);
                    SaveStamp(con, bundlePath, stamp);
                    tran.Commit();
                }
            }
        }

        private static void SaveStamp(SQLiteConnection con, string bundlePath, BundleStamp stamp)
        {
            if (stamp == null)
            {
                return;
            }
            using (SQLiteCommand cmd = new SQLiteCommand("INSERT OR REPLACE INTO Bundles(ArchivePath, Size, ModifiedTicks) VALUES(@p, @s, @m)", con))
            {
                cmd.Parameters.AddWithValue("@p", bundlePath);
                cmd.Parameters.AddWithValue("@s", stamp.Size);
                cmd.Parameters.AddWithValue("@m", stamp.ModifiedTicks);
                cmd.ExecuteNonQuery();
            }
        }

        // Stamps of all bundles which are in the catalog (the key ignores case, as Windows paths do)
        public static Dictionary<string, BundleStamp> LoadBundleStamps()
        {
            Dictionary<string, BundleStamp> stamps = new Dictionary<string, BundleStamp>(StringComparer.OrdinalIgnoreCase);
            using (SQLiteConnection con = OpenConnection())
            using (SQLiteCommand cmd = new SQLiteCommand("SELECT ArchivePath, Size, ModifiedTicks FROM Bundles", con))
            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    BundleStamp stamp = new BundleStamp();
                    stamp.Size = reader.GetInt64(1);
                    stamp.ModifiedTicks = reader.GetInt64(2);
                    stamps[reader.GetString(0)] = stamp;
                }
            }
            return stamps;
        }

        // Replaces the rows of every bundle mentioned in the stream, other bundles stay untouched
        public static int ImportRows(IEnumerable<CatalogRow> rows, CancellationToken token)
        {
            int count = 0;
            HashSet<string> cleaned = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            lock (writeLock)
            {
                using (SQLiteConnection con = OpenConnection())
                using (SQLiteTransaction tran = con.BeginTransaction())
                using (SQLiteCommand insert = CreateInsertCommand(con))
                {
                    foreach (CatalogRow row in rows)
                    {
                        token.ThrowIfCancellationRequested();

                        if (cleaned.Add(row.ArchivePath))
                        {
                            DeleteBundle(con, row.ArchivePath);
                        }
                        InsertRow(insert, row);
                        count++;
                    }
                    tran.Commit();
                }
            }
            return count;
        }

        // Removes the rows and the stamp of the bundle (a bundle without a stamp is scanned again)
        private static void DeleteBundle(SQLiteConnection con, string bundlePath)
        {
            using (SQLiteCommand cmd = new SQLiteCommand("DELETE FROM Files WHERE ArchivePath = @p; DELETE FROM Bundles WHERE ArchivePath = @p", con))
            {
                cmd.Parameters.AddWithValue("@p", bundlePath);
                cmd.ExecuteNonQuery();
            }
        }

        private static void InsertRows(SQLiteConnection con, IList<CatalogRow> rows)
        {
            using (SQLiteCommand insert = CreateInsertCommand(con))
            {
                foreach (CatalogRow row in rows)
                {
                    InsertRow(insert, row);
                }
            }
        }

        private static SQLiteCommand CreateInsertCommand(SQLiteConnection con)
        {
            SQLiteCommand cmd = new SQLiteCommand(
                "INSERT INTO Files(FilePath, FileName, FileType, ArchivePath, ArchiveName, ArchiveType, PathKey, NameKey) " +
                "VALUES(@fp, @fn, @ft, @ap, @an, @at, @pk, @nk)", con);
            cmd.Parameters.Add(new SQLiteParameter("@fp"));
            cmd.Parameters.Add(new SQLiteParameter("@fn"));
            cmd.Parameters.Add(new SQLiteParameter("@ft"));
            cmd.Parameters.Add(new SQLiteParameter("@ap"));
            cmd.Parameters.Add(new SQLiteParameter("@an"));
            cmd.Parameters.Add(new SQLiteParameter("@at"));
            cmd.Parameters.Add(new SQLiteParameter("@pk"));
            cmd.Parameters.Add(new SQLiteParameter("@nk"));
            return cmd;
        }

        private static void InsertRow(SQLiteCommand insert, CatalogRow row)
        {
            insert.Parameters["@fp"].Value = row.FilePath;
            insert.Parameters["@fn"].Value = row.FileName;
            insert.Parameters["@ft"].Value = row.FileType;
            insert.Parameters["@ap"].Value = row.ArchivePath;
            insert.Parameters["@an"].Value = row.ArchiveName;
            insert.Parameters["@at"].Value = row.ArchiveType;
            insert.Parameters["@pk"].Value = row.FilePath.ToLowerInvariant();
            insert.Parameters["@nk"].Value = row.FileName.ToLowerInvariant();
            insert.ExecuteNonQuery();
        }

        public static void Clear()
        {
            lock (writeLock)
            {
                using (SQLiteConnection con = OpenConnection())
                {
                    Execute(con, "DELETE FROM Files");
                    Execute(con, "DELETE FROM Bundles");
                    Execute(con, "VACUUM");
                }
            }
        }

        // Ids of the rows whose FilePath contains the filter, ordered as requested.
        // sortColumn == null means the order of insertion.
        // uniqueArchive: only the first (by Id) matching row of every archive is returned.
        public static long[] QueryIds(string filter, bool ignoreCase, bool uniqueArchive, CatalogColumn? sortColumn, bool ascending, CancellationToken token)
        {
            StringBuilder sql = new StringBuilder();
            string query = (filter ?? "").Replace('\\', '/');
            string where = "";

            if (query.Length > 0)
            {
                where = ignoreCase ? " WHERE instr(PathKey, @q) > 0" : " WHERE instr(FilePath, @q) > 0";
            }
            if (uniqueArchive)
            {
                sql.Append("SELECT Id FROM Files WHERE Id IN (SELECT MIN(Id) FROM Files").Append(where).Append(" GROUP BY ArchivePath)");
            }
            else
            {
                sql.Append("SELECT Id FROM Files").Append(where);
            }
            if (sortColumn.HasValue)
            {
                sql.Append(" ORDER BY ").Append(ColumnSql(sortColumn.Value)).Append(ascending ? " ASC" : " DESC").Append(", Id");
            }
            else
            {
                sql.Append(" ORDER BY Id");
            }

            List<long> ids = new List<long>();
            using (SQLiteConnection con = OpenConnection())
            using (SQLiteCommand cmd = new SQLiteCommand(sql.ToString(), con))
            {
                if (query.Length > 0)
                {
                    cmd.Parameters.AddWithValue("@q", ignoreCase ? query.ToLowerInvariant() : query);
                }
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ids.Add(reader.GetInt64(0));
                        if ((ids.Count & 0xFFF) == 0)
                        {
                            token.ThrowIfCancellationRequested();
                        }
                    }
                }
            }
            return ids.ToArray();
        }

        // Rows for ids[start .. start + count), in the same order as the ids
        public static List<CatalogRow> GetRows(long[] ids, int start, int count)
        {
            List<CatalogRow> result = new List<CatalogRow>();
            if (count <= 0 || start >= ids.Length)
            {
                return result;
            }
            int end = Math.Min(ids.Length, start + count);

            StringBuilder idList = new StringBuilder();
            for (int i = start; i < end; i++)
            {
                if (i > start)
                {
                    idList.Append(',');
                }
                idList.Append(ids[i]); // numbers only, safe to inline
            }

            Dictionary<long, CatalogRow> byId = new Dictionary<long, CatalogRow>();
            using (SQLiteConnection con = OpenConnection())
            using (SQLiteCommand cmd = new SQLiteCommand("SELECT Id, " + SelectColumns + " FROM Files WHERE Id IN (" + idList + ")", con))
            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    byId[reader.GetInt64(0)] = ReadRow(reader, 1);
                }
            }
            for (int i = start; i < end; i++)
            {
                CatalogRow row;
                if (byId.TryGetValue(ids[i], out row))
                {
                    result.Add(row);
                }
                else
                {
                    result.Add(new CatalogRow()); // the row was deleted meanwhile
                }
            }
            return result;
        }

        private static CatalogRow ReadRow(SQLiteDataReader reader, int offset)
        {
            CatalogRow row = new CatalogRow();
            row.FilePath = reader.GetString(offset);
            row.FileName = reader.GetString(offset + 1);
            row.FileType = reader.GetString(offset + 2);
            row.ArchivePath = reader.GetString(offset + 3);
            row.ArchiveName = reader.GetString(offset + 4);
            row.ArchiveType = reader.GetString(offset + 5);
            return row;
        }

        // Streams every row (used by the CSV export)
        public static IEnumerable<CatalogRow> EnumerateAll()
        {
            using (SQLiteConnection con = OpenConnection())
            using (SQLiteCommand cmd = new SQLiteCommand("SELECT " + SelectColumns + " FROM Files ORDER BY Id", con))
            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    yield return ReadRow(reader, 0);
                }
            }
        }

        // For every line (normalized, with the leading slash) finds the rows whose FilePath ends with it.
        // Candidates are taken by the indexed lower-cased file name, then the tail is verified in .NET.
        public static List<CatalogTailMatch> FindByTails(IList<string> tails, bool ignoreCase, CancellationToken token)
        {
            List<CatalogTailMatch> result = new List<CatalogTailMatch>();
            StringComparison comparison = ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;

            using (SQLiteConnection con = OpenConnection())
            using (SQLiteCommand cmd = new SQLiteCommand("SELECT " + SelectColumns + " FROM Files WHERE NameKey = @n", con))
            {
                cmd.Parameters.Add(new SQLiteParameter("@n"));

                foreach (string tail in tails)
                {
                    token.ThrowIfCancellationRequested();

                    CatalogTailMatch match = new CatalogTailMatch();
                    match.Tail = tail;
                    cmd.Parameters["@n"].Value = tail.Substring(tail.LastIndexOf('/') + 1).ToLowerInvariant();

                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            CatalogRow row = ReadRow(reader, 0);
                            if (row.FilePath.EndsWith(tail, comparison))
                            {
                                match.Rows.Add(row);
                            }
                        }
                    }
                    result.Add(match);
                }
            }
            return result;
        }
    }
}
