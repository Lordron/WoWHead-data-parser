using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

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

        public static string Substring(this string input, string check)
        {
            string pattern = string.Format("template: '.+', id: ('[a-z\\-]+'), data: ");
            Regex regex = new Regex(pattern, RegexOptions.Multiline);
            {
                MatchCollection matches = regex.Matches(input);
                foreach (Match item in matches)
                {
                    string type = item.Groups[1].Value;

                    if (!type.Equals(check))
                        continue;

                    int start = item.Index;
                    int end = input.IndexOf("});", start);

                    input = input.Substring(start, end - start + 3);
                }
            }

            return input;
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

        public static void ThreadSafe<T>(this T control, Action<T> action) where T : Control
        {
            if (control == null)
                throw new ArgumentNullException("control");

            if (action == null)
                throw new ArgumentNullException("action");

            if (control.InvokeRequired)
                control.Invoke(action, control);
            else
                action(control);
        }

        public static void ThreadSafeBegin<T>(this T control, Action<T> action) where T : Control
        {
            if (control == null)
                throw new ArgumentNullException("control");

            if (action == null)
                throw new ArgumentNullException("action");

            if (control.InvokeRequired)
                control.BeginInvoke(action, control);
            else
                action(control);
        }
    }
}