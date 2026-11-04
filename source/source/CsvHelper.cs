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
        private static readonly string[] Header =
        {
            "FilePath", "FileName", "FileType", "ArchivePath", "ArchiveName", "ArchiveType"
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
                        row.FilePath, row.FileName, row.FileType, row.ArchivePath, row.ArchiveName, row.ArchiveType
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

        // Rows of the file. Only FilePath and ArchivePath are taken, the other columns are derived from them.
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
                    yield return CatalogRow.Create(filePath, archivePath);
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
