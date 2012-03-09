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
            string fileName =
                Path.Combine(Application.StartupPath + "\\" + "db2", typeof(T).Name + ".db2").Replace("Entry",
                                                                                                             string.
                                                                                                                 Empty);

            using (FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            using (BinaryReader reader = new BinaryReader(stream, Encoding.UTF8))
            {
                if (!File.Exists(fileName))
                    throw new FileNotFoundException();

                Db2Header header = reader.ReadStruct<Db2Header>();
                int size = Marshal.SizeOf(typeof (T));

                if (!header.IsDb2)
                    throw new Exception(fileName + " is not DBC files");

                if (header.RecordSize != size)
                    throw new Exception(
                        string.Format("Size of row in DB2 file ({0}) != size of DB2 struct ({1}) in DB2: {2}",
                                      header.RecordSize, size, fileName));

                // WDB2 specific fields
                uint tableHash = reader.ReadUInt32(); // new field in WDB2
                uint build = reader.ReadUInt32();     // new field in WDB2

                int unk1 = reader.ReadInt32();        // new field in WDB2
                int unk2 = reader.ReadInt32();        // new field in WDB2
                int unk3 = reader.ReadInt32();        // new field in WDB2 (index table?)
                int locale = reader.ReadInt32();      // new field in WDB2
                int unk5 = reader.ReadInt32();        // new field in WDB2

                if (unk3 != 0)
                {
                    reader.ReadBytes(unk3 * 4 - 48);     // an index for rows
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
                        uint offset = (uint) (reader.BaseStream.Position - header.StartStringPosition);
                        string str = reader.ReadCString();
                        strDict.Add(offset, str);
                    }
                }
            }
            return dict;
        }

        public static T GetValue<T>(this Dictionary<uint, T> dictionary, uint key)
        {
            T value;
            dictionary.TryGetValue(key, out value);
            return value;
        }

        public static uint GetExtendedCost(uint cost, uint count)
        {
            foreach (KeyValuePair<uint, ItemExtendedCostEntry> kvp in DB2.ExtendedCost)
            {
                ItemExtendedCostEntry entry = kvp.Value;
                if (entry.HaveCurrency(cost, count))
                    return entry.Id;

                if (entry.HaveItem(cost, count))
                    return entry.Id;
            }
            return 0;
        }
    }
}