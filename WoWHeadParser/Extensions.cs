using System;
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
            MatchCollection matches = Regex.Matches(input, pattern, RegexOptions.Multiline);
            foreach (Match item in matches)
            {
                string type = item.Groups[1].Value;

                if (!type.Equals(check))
                    continue;

                int start = item.Index;
                int end = input.FastIndexOf("});", start);

                input = input.Substring(start, end - start + 3);
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
                bool found = true;
                for (int j = 2; j < pattern.Length; j++)
                {
                    if (source[first + j] == pattern[j])
                        continue;

                    found = false;
                    break;
                }
                // If the whole word was found, return its index, otherwise try again
                if (found)
                    return first;

                first = source.IndexOf(c0, ++first, limit - first);
            }

            return -1;
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