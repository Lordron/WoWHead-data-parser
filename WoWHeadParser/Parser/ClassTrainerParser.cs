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
            string subPattern = "{[^}]*\"id\":(\\d+)[^}]*\"level\":(\\d+)[^}]*\"skill\":\\[(\\d+)?\\][^}]*\"trainingcost\":(\\d+)[^}]*";

            //[{"cat":9,"id":33095,"learnedat":275,"level":1,"name":"@Рыбная ловля","nskillup":1,"schools":1,"skill":[356],"source":[6],"trainingcost":100000}

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
                    content.AppendLine(@"REPLACE INTO `npc_trainer` (`entry`, `spell`, `spellcost`, `reqlevel`, `reqSkill`) VALUES");
                    print = true;
                }

                for (int i = 0; i < matches.Count; ++i)
                {
                    GroupCollection groups = matches[i].Groups;
                    content.AppendFormat(@"(@ENTRY, {0}, {1}, {2}, {3}){4}", groups[1].Value, groups[4].Value, groups[2].Value, string.IsNullOrEmpty(groups[3].Value) ? "0" : groups[3].Value, (i < matches.Count - 1 ? "," : ";")).AppendLine();
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
