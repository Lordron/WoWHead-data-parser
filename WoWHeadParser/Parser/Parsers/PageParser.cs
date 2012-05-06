using System;
using System.Text.RegularExpressions;
using Sql;
using WoWHeadParser.Page;

namespace WoWHeadParser.Parser.Parsers
{
    internal class PageParser : DataParser
    {
        public override string Parse(PageItem block)
        {
            Regex regex = new Regex(@"new Book\({ parent: '.+', pages: \['(?<page>.+)'\]}\)", RegexOptions.Multiline);
            MatchCollection matches = regex.Matches(block.Page);

            SqlBuilder builder = new SqlBuilder(HasLocales ? "locales_page_text" : "page_text");
            if (HasLocales)
                builder.SetFieldsName(string.Format("Text_{0}", Locales[Locale]));
            else
                builder.SetFieldsName("text", "next_page");

            for (int i = 0; i < matches.Count; ++i)
            {
                Match item = matches[i];
                GroupCollection groups = item.Groups;
                string typeStr = groups["page"].Value;
                string[] pages = typeStr.Split(new[] {@"','"}, StringSplitOptions.RemoveEmptyEntries);

                string query = string.Format(@"SET @ENTRY := (SELECT `data0` FROM `gameobject_template` WHERE `entry` = {0});", block.Id);
                builder.AppendSqlQuery(query);

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

            return builder.ToString();
        }

        public override string Name { get { return "Book page data parser"; } }

        public override string Address { get { return "object={0}"; } }
    }
}