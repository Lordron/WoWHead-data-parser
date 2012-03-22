using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace WoWHeadParser
{
    internal class TrainerParser : Parser
    {
        private Dictionary<TrainerType, string> _patterns = new Dictionary<TrainerType, string>
        {
            {TrainerType.TypeNone, ""},
            {TrainerType.TypeClass, "{[^}]*\"id\":(\\d+)[^}]*\"level\":(\\d+)[^}]*\"skill\":\\[(\\d+)?\\][^}]*\"trainingcost\":(\\d+)[^}]*"},
            {TrainerType.TypeTradeskills, "{[^}]*\"id\":(\\d+)[^}]*\"learnedat\":(\\d+)[^}]*\"level\":(\\d+)[^}]*\"skill\":\\[(\\d+)?\\][^}]*\"trainingcost\":(\\d+)[^}]*"},
        };

        public override string Parse(Block block)
        {
            StringBuilder content = new StringBuilder();

            string page = block.Page;

            int npcflag = 16;
            TrainerType type = TrainerType.TypeNone;

            Regex regex = new Regex("template: 'spell', id: ('[a-z\\-]+'), name: ", RegexOptions.Multiline);
            {
                MatchCollection matches = regex.Matches(page);
                foreach (Match item in matches)
                {
                    switch (item.Groups[1].Value)
                    {
                        case "\'teaches-ability\'":
                            npcflag = 48;
                            type = TrainerType.TypeClass;
                            break;
                        case "\'teaches-recipe\'":
                            npcflag = 80;
                            type = TrainerType.TypeTradeskills;
                            break;
                        default:
                            continue;
                    }

                    int start = item.Index;
                    int end = page.IndexOf("});", start);

                    page = page.Substring(start, end - start + 3);
                }
            }

            if (type == TrainerType.TypeNone)
            {
                Console.WriteLine("-- Unknown trainer type, Id {0}", block.Id);
                return string.Empty;
            }

            const string pattern = @"data: \[.*;";
            string subPattern = _patterns[type];

            MatchCollection find = Regex.Matches(page, pattern);

            if (find.Count > 0)
            {
                content.AppendFormat(@"SET @ENTRY := {0};", block.Id).AppendLine();
                content.AppendFormat(@"UPDATE `creature_template` SET `npcflag` = `npcflag` | {0}, `trainer_type` = {1} WHERE `entry` = @ENTRY;", npcflag, (int)type).AppendLine();
                content.AppendLine(@"REPLACE INTO `npc_trainer` (`entry`, `spell`, `spellcost`, `reqlevel`, `reqSkill`, `reqSkillValue`) VALUES");
            }

            foreach (Match item in find)
            {
                MatchCollection matches = Regex.Matches(item.Value, subPattern);

                for (int i = 0; i < matches.Count; ++i)
                {
                    GroupCollection groups = matches[i].Groups;

                    string spell = groups[1].Value;
                    string end = (i < matches.Count - 1 ? "," : ";");
                    switch (type)
                    {
                        case TrainerType.TypeTradeskills:
                            {
                                string reqSkill = (string.IsNullOrEmpty(groups[4].Value) ? "0" : groups[4].Value);
                                string reqSkillValue = (string.IsNullOrEmpty(groups[2].Value) ? "0" : groups[2].Value);

                                content.AppendFormat(@"(@ENTRY, {0}, {1}, {2}, {3}, {4}){5}", spell, groups[5].Value, groups[3].Value, reqSkill, reqSkillValue, end).AppendLine();
                            }
                            break;
                        case TrainerType.TypeClass:
                            {
                                string reqSkill = (string.IsNullOrEmpty(groups[3].Value) ? "0" : groups[3].Value);

                                content.AppendFormat(@"(@ENTRY, {0}, {1}, {2}, {3}, {4}){5}", spell, groups[4].Value, groups[2].Value, reqSkill, "0", end).AppendLine();
                            }
                            break;
                    }
                }
            }

            content.AppendLine();
            return content.ToString();
        }

        public override string BeforParsing()
        {
            StringBuilder content = new StringBuilder();

            content.AppendLine("-- Uncomment");
            content.AppendLine("-- DELETE FROM `npc_trainer`; -- Delete all data");

            return content.AppendLine().ToString();
        }

        public override string Address { get { return "wowhead.com/npc="; } }

        public override string Name { get { return "Professions & Class Trainer data parser"; } }

        public enum TrainerType
        {
            TypeNone = -1,
            TypeClass = 0,
            TypeMounts = 1,
            TypeTradeskills = 2,
            TypePets = 3,
        }
    }
}