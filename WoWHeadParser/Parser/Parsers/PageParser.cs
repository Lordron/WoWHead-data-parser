using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace WoWHeadParser
{
    internal class PageParser : Parser
    {
        private Dictionary<Locale, string> _tables = new Dictionary<Locale, string>
        {
            {Locale.Russia, "Text_loc8"},
            {Locale.Germany, "Text_loc3"},
            {Locale.France, "Text_loc2"},
            {Locale.Spain, "Text_loc6"},
        };

        public override string Parse(PageItem block)
        {
            Regex regex = new Regex(@"new Book\({ parent: '.+', pages: \['(?<page>.+)'\]}\)", RegexOptions.Multiline);
            MatchCollection matches = regex.Matches(block.Page);

            SqlBuilder builder = new SqlBuilder(Locale > Locale.English ? "locales_page_text" : "page_text");
            if (Locale > Locale.English)
                builder.SetFieldsName(_tables[Locale]);
            else
                builder.SetFieldsName("text", "next_page");

            for (int i = 0; i < matches.Count; ++i)
            {
                Match item = matches[i];
                GroupCollection groups = item.Groups;
                string typeStr = groups["page"].Value ?? string.Empty;
                string[] pages = typeStr.Split(new[] {@"','"}, StringSplitOptions.RemoveEmptyEntries);

                string query = string.Format(@"SET @ENTRY := (SELECT `data0` FROM `gameobject_template` WHERE `entry` = {0});", block.Id);
                builder.AppendSqlQuery(query);

                if (Locale > Locale.English)
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
                        string next_page = (j < pages.Length - 1) ? string.Format("@ENTRY + {0}", j + 1) : "0";

                        builder.AppendFieldsValue(key, pages[j].HTMLEscapeSumbols(), next_page);
                    }
                }
            }

            return builder.ToString();
        }

        public override string BeforParsing()
        {
            return string.Empty;
        }

        public override string Address { get { return "wowhead.com/object="; } }

        public override string Name { get { return "Book page data parser"; } }

        public override int MaxCount { get { return 0; } }
    }
}