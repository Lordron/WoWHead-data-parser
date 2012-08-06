using System;
using System.Text.RegularExpressions;

namespace WoWHeadParser.Parser.Parsers
{
    [Parser(ParserType.Page)]
    internal class BookPageParser : PageParser
    {
        public BookPageParser(Locale locale, int flags)
            : base(locale, flags)
        {
            this.Address = "object={0}";

            if (HasLocales)
                Builder.Setup("locales_page_text", "entry", true, string.Format("Text_{0}", LocalePosfix));
            else
                Builder.Setup("page_text", "entry", true, "text", "next_page");
        }

        private const string bookPattern = @"new Book\({ parent: '.+', pages: \['(?<page>.+)'\]}\)";
        private Regex bookRegex = new Regex(bookPattern);

        public override void Parse(string page, uint id)
        {
            MatchCollection matches = bookRegex.Matches(page);
            for (int i = 0; i < matches.Count; ++i)
            {
                Match item = matches[i];
                GroupCollection groups = item.Groups;
                string typeStr = groups["page"].Value;
                string[] pages = typeStr.Split(new[] { @"','" }, StringSplitOptions.RemoveEmptyEntries);

                for (int j = 0; j < pages.Length; ++j)
                {
                    string key = string.Format("@ENTRY_{0} + {1}", id, j);

                    Builder.SetKey(key);
                    Builder.AppendSqlQuery(key, @"SET @ENTRY_{0}:= (SELECT `data0` FROM `gameobject_template` WHERE `entry` = {0});", id);

                    if (HasLocales)
                        Builder.AppendValue(pages[j].HTMLEscapeSumbols());
                    else
                    {
                        string nextPage = (j < pages.Length - 1) ? string.Format("@ENTRY_{0} + {1}", id, j + 1) : "0";
                        Builder.AppendValues(pages[j].HTMLEscapeSumbols(), nextPage);
                    }
                    Builder.Flush();
                }
            }
        }
    }
}