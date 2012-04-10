using System;
using System.Collections.Generic;
using System.Text;
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
            StringBuilder content = new StringBuilder();

            Regex regex = new Regex(@"new Book\({ parent: '.+', pages: \['(?<page>.+)'\]}\)", RegexOptions.Multiline);
            MatchCollection matches = regex.Matches(block.Page);

            if (Locale > Locale.English)
            {
                SqlBuilder.Initial("locales_page_text");
                SqlBuilder.SetFieldsName(_tables[Locale]);
            }
            else
            {
                SqlBuilder.Initial("page_text");
                SqlBuilder.SetFieldsName("text", "next_page");
            }

            foreach (Match match in matches)
            {
                GroupCollection groups = match.Groups;
                string typeStr = groups["page"].Value ?? string.Empty;
                string[] pages = typeStr.Split(new[] {@"','"}, StringSplitOptions.RemoveEmptyEntries);

                content.AppendFormat(@"SET @ENTRY := (SELECT `data0` FROM `gameobject_template` WHERE `entry` = {0});", block.Id).AppendLine();

                if (Locale == Locale.English)
                {
                    for (int i = 0; i < pages.Length; ++i)
                    {
                        string key = (i == 0 ? "@ENTRY" : string.Format("@ENTRY + {0}", i));
                        string next_page = (i < pages.Length - 1) ? string.Format("@ENTRY + {0}", i + 1) : "0";

                        SqlBuilder.AppendFieldsValue(key, pages[i].HTMLEscapeSumbols(), next_page);
                    }
                }
                else
                {
                    for (int i = 0; i < pages.Length; ++i)
                    {
                        string key = (i == 0 ? "@ENTRY" : string.Format("@ENTRY + {0}", i));
                        SqlBuilder.AppendFieldsValue(key, pages[i].HTMLEscapeSumbols());
                    }
                }
            }

            content.Append(SqlBuilder.ToString());
            return content.ToString();
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