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
    }
}