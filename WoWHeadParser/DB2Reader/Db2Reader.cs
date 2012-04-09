using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace WoWHeadParser
{
    internal static class DB2Reader
    {
        public static Dictionary<uint, T> ReadDb2<T>(Dictionary<uint, string> strDict) where T : struct
        {
            Dictionary<uint, T> dict = new Dictionary<uint, T>();

            string fileName = typeof(T).Name.Replace("Entry", string.Empty) + ".db2";
            string path = Path.Combine(Application.StartupPath, "db2", fileName);

            if (!File.Exists(path))
                return new Dictionary<uint, T>();

            using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            using (BinaryReader reader = new BinaryReader(stream, Encoding.UTF8))
            {
                Db2Header header = reader.ReadStruct<Db2Header>();
                int size = Marshal.SizeOf(typeof(T));

                if (!header.IsDb2)
                {
                    Console.WriteLine("{0} is not db2 file, skip", fileName);
                    return new Dictionary<uint, T>();
                }

                if (header.RecordSize != size)
                {
                    Console.WriteLine("File {0}, size of row in db2 file != size of struct ({1}, {2}), skip", fileName, header.RecordSize, size);
                    return new Dictionary<uint, T>();
                }

                // WDB2 specific fields
                uint tableHash = reader.ReadUInt32(); // new field in WDB2
                uint build = reader.ReadUInt32(); // new field in WDB2

                int unk1 = reader.ReadInt32(); // new field in WDB2
                int unk2 = reader.ReadInt32(); // new field in WDB2
                int unk3 = reader.ReadInt32(); // new field in WDB2 (index table?)
                int locale = reader.ReadInt32(); // new field in WDB2
                int unk5 = reader.ReadInt32(); // new field in WDB2

                if (unk3 != 0)
                {
                    reader.ReadBytes(unk3 * 4 - 48); // an index for rows
                    reader.ReadBytes(unk3 * 2 - 48 * 2); // a memory allocation bank
                }

                // read data
                for (int r = 0; r < header.RecordsCount; ++r)
                {
                    uint key = reader.ReadUInt32();
                    reader.BaseStream.Position -= 4;
                    T entry = reader.ReadStruct<T>();
                    dict.Add(key, entry);
                }

                // read strings
                if (strDict != null)
                {
                    while (reader.BaseStream.Position != reader.BaseStream.Length)
                    {
                        uint offset = (uint)(reader.BaseStream.Position - header.StartStringPosition);
                        string str = reader.ReadCString();
                        strDict.Add(offset, str);
                    }
                }
            }
            return dict;
        }

        public static uint GetExtendedCost(uint cost, uint count)
        {
            uint currencyCount = count * 100;
            foreach (KeyValuePair<uint, ItemExtendedCostEntry> kvp in DB2.ExtendedCost)
            {
                ItemExtendedCostEntry entry = kvp.Value;
                if (entry.HaveItem(cost, count))
                    return entry.Id;

                if (entry.HaveCurrency(cost, count))
                    return entry.Id;

                if (entry.HaveCurrency(cost, currencyCount))
                    return entry.Id;
            }
            return 0;
        }
    }
}