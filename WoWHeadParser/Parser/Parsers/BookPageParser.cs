using System;
using System.Text.RegularExpressions;
using Sql;
using WoWHeadParser.Page;
using WoWHeadParser.Properties;

namespace WoWHeadParser.Parser.Parsers
{
    internal class BookPageParser : PageParser
    {
        private const string bookPattern = @"new Book\({ parent: '.+', pages: \['(?<page>.+)'\]}\)";
        private Regex bookRegex = new Regex(bookPattern);

        public override PageItem Parse(string page, uint id)
        {
            SqlBuilder builder = new SqlBuilder(HasLocales ? "locales_page_text" : "page_text");
            if (HasLocales)
                builder.SetFieldsName("Text_{0}", LocalePosfix);
            else
                builder.SetFieldsNames("text", "next_page");

            MatchCollection matches = bookRegex.Matches(page);

            for (int i = 0; i < matches.Count; ++i)
            {
                Match item = matches[i];
                GroupCollection groups = item.Groups;
                string typeStr = groups["page"].Value;
                string[] pages = typeStr.Split(new[] {@"','"}, StringSplitOptions.RemoveEmptyEntries);

                builder.AppendSqlQuery(@"SET @ENTRY := (SELECT `data0` FROM `gameobject_template` WHERE `entry` = {0});", id);

                if (HasLocales)
                {
                    for (int j = 0; j < pages.Length; ++j)
                    {
                        string key = string.Format("@ENTRY + {0}", j);
                        builder.AppendFieldsValue(key, pages[j].HTMLEscapeSumbols());
                    }
                }
                else
                {
                    for (int j = 0; j < pages.Length; ++j)
                    {
                        string key = string.Format("@ENTRY + {0}", j);
                        string nextPage = (j < pages.Length - 1) ? string.Format("@ENTRY + {0}", j + 1) : "0";

                        builder.AppendFieldsValue(key, pages[j].HTMLEscapeSumbols(), nextPage);
                    }
                }
            }

            return new PageItem(id, builder.ToString());
        }

        public override string Name { get { return Resources.PageParser; } }

        public override string Address { get { return "object={0}"; } }

        public override string WelfName { get { return "page"; } }
    }
}