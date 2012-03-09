using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace WoWHeadParser
{
    public static class Extensions
    {
        public static string HTMLEscapeSumbols(this string input)
        {
            return input.Replace(@"""", @"\""")
                    .Replace(@"'", @"\'")
                    .Replace(@"&quot;", @"\""")
                    .Replace(@"&apos;", @"\'")
                    .Replace(@"&amp;", @"&")
                    .Replace(@"&lt;", @"<")
                    .Replace(@"&gt;", @">")
                    .Replace(@"&nbsp;", @" ")
                    .Replace(@"&curren;", @"¤")
                    .Replace(@"&brvbar;", @"¦")
                    .Replace(@"&sect;", @"§")
                    .Replace(@"&copy;", @"©")
                    .Replace(@"&laquo;", @"«")
                    .Replace(@"&not;", @"¬")
                    .Replace(@"<name>", @"$N")
                    .Replace(@"<class>", @"$C")
                    .Replace(@"<race>", @"$R")
                    .Replace(@"<lad:lass>", @"$G")
                    .Replace(@"<br />", @"$B");
        }

        public static string ReadCString(this BinaryReader reader)
        {
            return reader.ReadCString(Encoding.UTF8);
        }

        public static string ReadCString(this BinaryReader reader, Encoding encoding)
        {
            List<byte> bytes = new List<byte>();
            byte b;
            while ((b = reader.ReadByte()) != 0)
                bytes.Add(b);

            return encoding.GetString(bytes.ToArray());
        }

        public static T ReadStruct<T>(this BinaryReader reader) where T : struct
        {
            byte[] rawData = reader.ReadBytes(Marshal.SizeOf(typeof(T)));
            GCHandle handle = GCHandle.Alloc(rawData, GCHandleType.Pinned);
            T returnObject = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
            handle.Free();
            return returnObject;
        }
    }
}