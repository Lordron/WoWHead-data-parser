using System.Text;
using System.Text.RegularExpressions;

namespace WoWHeadParser
{
    internal class ClassTrainerParser : Parser
    {
        public override string Parse(Block block)
        {
            StringBuilder content = new StringBuilder();

            string page = block.Page;
            TrainerType type = GetTrainerType(page);

            string pattern = @"data: \[.*;";
            string subPattern = string.Empty;

            switch (type)
            {
                case TrainerType.ClassTrainer:
                    subPattern = "{[^}]*\"id\":(\\d+)[^}]*\"level\":(\\d+)[^}]*\"skill\":\\[(\\d+)?\\][^}]*\"trainingcost\":(\\d+)[^}]*";
                    break;
                case TrainerType.RecipeTrainer:
                    subPattern = "{[^}]*\"id\":(\\d+)[^}]*\"learnedat\":(\\d+)[^}]*\"level\":(\\d+)[^}]*\"skill\":\\[(\\d+)?\\][^}]*\"trainingcost\":(\\d+)[^}]*";
                    break;
                default:
                    return string.Format("-- Unknown trainer type: {0}", type);
            }

            bool print = false;

            MatchCollection find = Regex.Matches(page, pattern);
            foreach (Match item in find)
            {
                MatchCollection matches = Regex.Matches(item.Value, subPattern);
                if (!print && matches.Count > 0)
                {
                    content.AppendLine();
                    content.AppendFormat(@"SET @ENTRY := {0};", block.Entry).AppendLine();
                    content.AppendLine(@"UPDATE `creature_template` SET `npcflag` = `npcflag` | 48 WHERE `entry` = @ENTRY;");
                    content.AppendLine(@"REPLACE INTO `npc_trainer` (`entry`, `spell`, `spellcost`, `reqlevel`, `reqSkill`, `reqSkillValue`) VALUES");
                    print = true;
                }

                for (int i = 0; i < matches.Count; ++i)
                {
                    GroupCollection groups = matches[i].Groups;

                    string spell = groups[1].Value;
                    string end = (i < matches.Count - 1 ? "," : ";");
                    if (type == TrainerType.RecipeTrainer)
                    {
                        string reqSkill = (string.IsNullOrEmpty(groups[4].Value) ? "0" : groups[4].Value);
                        string reqSkillValue = (string.IsNullOrEmpty(groups[2].Value) ? "0" : groups[2].Value);

                        content.AppendFormat(@"(@ENTRY, {0}, {1}, {2}, {3}, {4}){5}", spell, groups[5].Value, groups[3].Value, reqSkill, reqSkillValue, end).AppendLine();
                    }
                    else if (type == TrainerType.ClassTrainer)
                    {
                        string reqSkill = (string.IsNullOrEmpty(groups[3].Value) ? "0" : groups[3].Value);

                        content.AppendFormat(@"(@ENTRY, {0}, {1}, {2}, {3}, {4}){5}", spell, groups[4].Value, groups[2].Value, reqSkill, "0", end).AppendLine();
                    }
                }
            }

            return content.ToString();
        }

        public TrainerType GetTrainerType(string page)
        {
            Regex regex = new Regex("template: 'spell', id: ('[a-z\\-]+'), name: ", RegexOptions.Multiline);
            {
                MatchCollection matches = regex.Matches(page);
                foreach (Match item in matches)
                {
                    switch (item.Groups[1].Value)
                    {
                        case "\'teaches-ability\'": 
                            return TrainerType.ClassTrainer;
                        case "\'teaches-recipe\'":
                            return TrainerType.RecipeTrainer;
                    }
                }
            }
            return TrainerType.TrainerNone;
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
