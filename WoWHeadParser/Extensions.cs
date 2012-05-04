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
            const string pattern = "template: '.+', id: ('[a-z\\-]+'), data: ";
            Regex regex = new Regex(pattern, RegexOptions.Multiline);
            {
                MatchCollection matches = regex.Matches(input);
                foreach (Match item in matches)
                {
                    string type = item.Groups[1].Value;

                    if (!type.Equals(check))
                        continue;

                    int start = item.Index;
                    int end = input.FastIndexOf("});", start);

                    input = input.Substring(start, end - start + 3);
                }
            }

            return input;
        }

        public static int FastIndexOf(this string source, string pattern, int startIndex = 0)
        {
            if (pattern == null) 
                throw new ArgumentNullException();

            if (pattern.Length == 0) 
                return 0;

            if (pattern.Length == 1) 
                return source.IndexOf(pattern[0], startIndex);

            bool found;
            int limit = source.Length - pattern.Length + 1;
            if (limit < 1) 
                return -1;

            // Store the first 2 characters of "pattern"
            char c0 = pattern[0];
            char c1 = pattern[1];
            // Find the first occurrence of the first character
            int first = source.IndexOf(c0, startIndex, limit - startIndex);
            while (first != -1)
            {
                // Check if the following character is the same like
                // the 2nd character of "pattern"
                if (source[first + 1] != c1)
                {
                    first = source.IndexOf(c0, ++first, limit - first);
                    continue;
                }
                // Check the rest of "pattern" (starting with the 3rd character)
                found = true;
                for (int j = 2; j < pattern.Length; j++)
                {
                    if (source[first + j] != pattern[j])
                    {
                        found = false;
                        break;
                    }
                }
                // If the whole word was found, return its index, otherwise try again
                if (found) 
                    return first;

                first = source.IndexOf(c0, ++first, limit - first);
            }

            return -1;
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