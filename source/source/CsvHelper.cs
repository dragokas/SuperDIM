using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace DazUnpacker
{
    // Reads and writes the catalog as a CSV file (UTF-8 with BOM, RFC 4180 quoting)
    internal static class CsvHelper
    {
        // The order of CatalogColumn
        private static readonly string[] Header =
        {
            "FilePath", "FileName", "FileType", "Modified", "ArchivePath", "ArchiveName", "ArchiveType", "ArchiveCreated"
        };

        internal static void Export(string path, IEnumerable<CatalogRow> rows, Action<long> rowsWritten, CancellationToken token)
        {
            long count = 0;
            using (StreamWriter writer = new StreamWriter(path, false, new UTF8Encoding(true)))
            {
                WriteRecord(writer, Header);
                foreach (CatalogRow row in rows)
                {
                    token.ThrowIfCancellationRequested();

                    WriteRecord(writer, new string[]
                    {
                        row.FilePath, row.FileName, row.FileType, row.Modified, row.ArchivePath, row.ArchiveName, row.ArchiveType, row.ArchiveCreated
                    });
                    count++;
                    if ((count & 0x3FFF) == 0)
                    {
                        rowsWritten(count);
                    }
                }
            }
            rowsWritten(count);
        }

        // Rows of the file. FilePath and ArchivePath are required, the names and types are derived from them;
        // Modified and ArchiveCreated are taken if the file has these columns (a file of an older version has not: the dates are empty).
        // percent(0..100) reports the progress by the position in the file.
        internal static IEnumerable<CatalogRow> Import(string path, Action<int> percent)
        {
            using (FileStream stream = File.OpenRead(path))
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8, true))
            {
                List<string> header = ReadRecord(reader);
                if (header == null)
                {
                    yield break;
                }
                int filePathIndex = header.FindIndex(h => h.Trim().Equals("FilePath", StringComparison.OrdinalIgnoreCase));
                int archivePathIndex = header.FindIndex(h => h.Trim().Equals("ArchivePath", StringComparison.OrdinalIgnoreCase));
                if (filePathIndex < 0 || archivePathIndex < 0)
                {
                    throw new InvalidDataException("The CSV file must have the columns FilePath and ArchivePath.");
                }
                int modifiedIndex = header.FindIndex(h => h.Trim().Equals("Modified", StringComparison.OrdinalIgnoreCase));
                int createdIndex = header.FindIndex(h => h.Trim().Equals("ArchiveCreated", StringComparison.OrdinalIgnoreCase));

                long lines = 0;
                List<string> record;
                while ((record = ReadRecord(reader)) != null)
                {
                    if (record.Count <= Math.Max(filePathIndex, archivePathIndex))
                    {
                        continue;
                    }
                    string filePath = record[filePathIndex];
                    string archivePath = record[archivePathIndex];
                    if (filePath.Length == 0 || archivePath.Length == 0)
                    {
                        continue;
                    }
                    lines++;
                    if ((lines & 0x3FFF) == 0 && stream.Length != 0)
                    {
                        percent((int)(stream.Position * 100 / stream.Length));
                    }
                    string modified = modifiedIndex >= 0 && modifiedIndex < record.Count ? record[modifiedIndex].Trim() : "";
                    string created = createdIndex >= 0 && createdIndex < record.Count ? record[createdIndex].Trim() : "";
                    yield return CatalogRow.Create(filePath, modified, archivePath, created);
                }
                percent(100);
            }
        }

        private static void WriteRecord(TextWriter writer, string[] fields)
        {
            for (int i = 0; i < fields.Length; i++)
            {
                if (i > 0)
                {
                    writer.Write(',');
                }
                writer.Write(Quote(fields[i]));
            }
            writer.Write("\r\n");
        }

        private static string Quote(string field)
        {
            if (field.IndexOfAny(new char[] { ',', '"', '\r', '\n' }) < 0)
            {
                return field;
            }
            return "\"" + field.Replace("\"", "\"\"") + "\"";
        }

        // One CSV record (quoted fields may contain commas and line breaks); null at the end of the file
        private static List<string> ReadRecord(TextReader reader)
        {
            if (reader.Peek() < 0)
            {
                return null;
            }
            List<string> fields = new List<string>();
            StringBuilder field = new StringBuilder();
            bool inQuotes = false;
            int ch;

            while ((ch = reader.Read()) >= 0)
            {
                char c = (char)ch;
                if (inQuotes)
                {
                    if (c == '"')
                    {
                        if (reader.Peek() == '"')
                        {
                            reader.Read();
                            field.Append('"');
                        }
                        else
                        {
                            inQuotes = false;
                        }
                    }
                    else
                    {
                        field.Append(c);
                    }
                }
                else if (c == '"')
                {
                    inQuotes = true;
                }
                else if (c == ',')
                {
                    fields.Add(field.ToString());
                    field.Clear();
                }
                else if (c == '\r')
                {
                    if (reader.Peek() == '\n')
                    {
                        reader.Read();
                    }
                    break;
                }
                else if (c == '\n')
                {
                    break;
                }
                else
                {
                    field.Append(c);
                }
            }
            fields.Add(field.ToString());
            return fields;
        }
    }
}
