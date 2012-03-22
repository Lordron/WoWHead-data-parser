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

        public override string Parse(Block block)
        {
            StringBuilder content = new StringBuilder();

            Regex regex = new Regex(@"new Book\({ parent: '.+', pages: \['(?<page>.+)'\]}\)", RegexOptions.Multiline);
            MatchCollection matches = regex.Matches(block.Page);

            foreach (Match match in matches)
            {
                GroupCollection groups = match.Groups;
                string typeStr = groups["page"].Value ?? string.Empty;
                string[] pages = typeStr.Split(new[] {@"','"}, StringSplitOptions.RemoveEmptyEntries);

                content.AppendFormat(@"SET @ENTRY := (SELECT `data0` FROM `gameobject_template` WHERE `entry` = {0});", block.Id).AppendLine();

                if (Locale == Locale.English || Locale == Locale.Portugal)
                {
                    content.AppendLine(@"INSERT IGNORE INTO `page_text` (`entry`, `text`, `next_page`) VALUES");

                    for (int i = 0; i < pages.Length; ++i)
                    {
                        content.AppendFormat(@"({0}, '{1}', {2}){3}",
                                             (i == 0 ? "@ENTRY" : string.Format("@ENTRY + {0}", i)), pages[i].HTMLEscapeSumbols(),
                                             (i < pages.Length - 1) ? string.Format("@ENTRY + {0}", i + 1) : "0", (i < pages.Length - 1 ? "," : ";")).AppendLine();
                    }
                }
                else
                {
                    string locale = _tables[Locale];
                    content.AppendFormat(@"INSERT IGNORE INTO `locales_page_text` (`entry`, `{0}`) VALUES", locale);
                    for (int i = 0; i < pages.Length; ++i)
                    {
                        content.AppendFormat(@"({0}, '{1}'){2}",
                                             (i == 0 ? "@ENTRY" : string.Format("@ENTRY + {0}", i)), pages[i].HTMLEscapeSumbols(),
                                             (i < pages.Length - 1 ? "," : ";")).AppendLine();
                    }
                }
                content.AppendLine();
            }

            return content.ToString();
        }

        public override string BeforParsing()
        {
            return string.Empty;
        }

        public override string Address { get { return "wowhead.com/object="; } }

        public override string Name { get { return "Page data parser"; } }
    }
}