using System;
using System.Text;
using System.Text.RegularExpressions;

namespace WoWHeadParser
{
    //Entry = SELECT data0 FROM gameobject_template WHERE entry = entry;
    internal class PageParser : Parser
    {
        public override string Parse(string page, uint entry)
        {
            StringBuilder content = new StringBuilder();
            Regex reg = new Regex(@"new Book\({ parent: '.+', pages: \['(?<test>.+)'\]}\)", RegexOptions.Multiline);
            MatchCollection matches = reg.Matches(page);

            foreach (Match match in matches)
            {
                GroupCollection groups = match.Groups;
                string typeStr = groups["test"].Value ?? string.Empty;
                string[] pages = typeStr.Split(new[] { @"','" }, StringSplitOptions.RemoveEmptyEntries);

                content.AppendLine(@"INSERT IGNORE INTO `page_text` VALUES");
                for (int i = 0; i < pages.Length; ++i)
                {
                    content.AppendFormat(@"({0}, '{1}', {2}){3}",
                        entry + i, pages[i].HTMLEscapeSumbols(),
                        i < pages.Length - 1 ? (entry + i + 1) : 0, (i < pages.Length - 1 ? "," : ";")).AppendLine();
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
