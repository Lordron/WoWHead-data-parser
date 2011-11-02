using System.Text;
using System.Text.RegularExpressions;

namespace WoWHeadParser
{
    internal class ClassTrainerParser : Parser
    {
        public override string Parse(Block block)
        {
            StringBuilder content = new StringBuilder();

            string pattern = @"data: \[.*;";
            string subPattern = "{[^}]*\"id\":(\\d+)[^}]*\"level\":(\\d+)[^}]*\"trainingcost\":(\\d+)[^}]*";

            bool print = false;

            MatchCollection find = Regex.Matches(block.Page, pattern);
            foreach (Match item in find)
            {
                MatchCollection matches = Regex.Matches(item.Value, subPattern);
                if (!print && matches.Count > 0)
                {
                    content.AppendLine();
                    content.AppendFormat(@"SET @ENTRY := {0};", block.Entry).AppendLine();
                    content.AppendLine(@"UPDATE `creature_template` SET `npcflag` = `npcflag` | 48 WHERE `entry` = @ENTRY;");
                    content.AppendLine(@"REPLACE INTO `npc_trainer` (`entry`, `spell`, `spellcost`, `reqlevel`) VALUES");
                    print = true;
                }

                for(int i = 0; i < matches.Count; ++i)
                {
                    GroupCollection groups = matches[i].Groups;
                    content.AppendFormat(@"(@ENTRY, {0}, {1}, {2}){3}", groups[1].Value, groups[3].Value, groups[2].Value, (i < matches.Count - 1 ? "," : ";")).AppendLine();
                }
            }

            return content.ToString();
        }

        public override string Address
        {
            get
            {
                return "wowhead.com/npc=";
            }
        }
    }
}
