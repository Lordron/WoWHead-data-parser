using System;
using System.Text;
using System.Text.RegularExpressions;

namespace WoWHeadParser
{
    internal class PageParser : Parser
    {
        public override string Parse(Block block)
        {
            StringBuilder content = new StringBuilder();
            Regex reg = new Regex(@"new Book\({ parent: '.+', pages: \['(?<page>.+)'\]}\)", RegexOptions.Multiline);
            MatchCollection matches = reg.Matches(block.Page);

            foreach (Match match in matches)
            {
                GroupCollection groups = match.Groups;
                string typeStr = groups["page"].Value ?? string.Empty;
                string[] pages = typeStr.Split(new[] { @"','" }, StringSplitOptions.RemoveEmptyEntries);
                content.AppendFormat("SET @ENTRY := SELECT `data0` FROM `gameobject_template` WHERE `entry` = {0};", block.Entry).AppendLine();
                content.AppendLine(@"INSERT IGNORE INTO `page_text` (`entry`, `text`, `next_page`) VALUES");
                for (int i = 0; i < pages.Length; ++i)
                {
                    content.AppendFormat(@"({0}, '{1}', {2}){3}",
                        (i == 0 ? "@ENTRY" : string.Format("@ENTRY + {0}", i)), pages[i].HTMLEscapeSumbols(),
                        (i < pages.Length - 1) ? string.Format("@ENTRY + {0}", i + 1) : "0", (i < pages.Length - 1 ? "," : ";")).AppendLine();
                }
            }

            return content.ToString();
        }

        public override string Address
        {
            get
            {
                return "wowhead.com/object=";
            }
        }
    }
}
