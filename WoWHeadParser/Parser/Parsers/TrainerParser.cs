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

        public override string Parse(PageItem block)
        {
            StringBuilder content = new StringBuilder();

            //string bpage = item.Page;
            string page = block.Page;

            int npcflag = 0x10;
            TrainerType type = TrainerType.TypeNone;

            Regex regex = new Regex("template: 'spell', id: ('[a-z\\-]+'), name: ", RegexOptions.Multiline);
            {
                MatchCollection matches = regex.Matches(page);
                foreach (Match item in matches)
                {
                    switch (item.Groups[1].Value)
                    {
                        //case "\'teaches-other\'":
                        case "\'teaches-ability\'":
                            npcflag = 0x30;
                            type = TrainerType.TypeClass;
                            break;
                        case "\'teaches-recipe\'":
                            npcflag = 0x50;
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
                Console.WriteLine("-- Unsupported trainer type, Id {0}", block.Id);
                return string.Empty;
            }

            const string pattern = @"data: \[.*;";
            string subPattern = _patterns[type];

            MatchCollection find = Regex.Matches(page, pattern);

            if (find.Count > 0)
                content.AppendFormat(@"UPDATE `creature_template` SET `npcflag` = `npcflag` | '{0}', `trainer_type` = '{1}' WHERE `entry` = '{2}';", npcflag, (int)type, block.Id).AppendLine();

            SqlBuilder builder = new SqlBuilder("npc_trainer");
            builder.SetFieldsName("spell", "spellcost", "reqlevel", "reqSkill", "reqSkillValue");

            foreach (Match item in find)
            {
                MatchCollection matches = Regex.Matches(item.Value, subPattern);

                for (int i = 0; i < matches.Count; ++i)
                {
                    GroupCollection groups = matches[i].Groups;

                    string spellCost = (type == TrainerType.TypeTradeskills) ? groups[5].Value : groups[4].Value; // TODO: support zero cost
                    switch (type)
                    {
                        case TrainerType.TypeTradeskills:
                            {
                                string reqSkill = (string.IsNullOrEmpty(groups[4].Value) ? "0" : groups[4].Value);
                                string reqSkillValue = (string.IsNullOrEmpty(groups[2].Value) ? "0" : groups[2].Value);
                                builder.AppendFieldsValue(block.Id, groups[1].Value, spellCost, groups[3].Value, reqSkill, reqSkillValue);
                            }
                            break;
                        case TrainerType.TypeClass:
                            {
                                string reqSkill = (string.IsNullOrEmpty(groups[3].Value) ? "0" : groups[3].Value);
                                builder.AppendFieldsValue(block.Id, groups[1].Value, spellCost, groups[2].Value, reqSkill, "0");
                            }
                            break;
                    }
                }
            }

            content.Append(builder.ToString());
            return content.AppendLine().ToString();
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

        public override int MaxCount { get { return 0; } }

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