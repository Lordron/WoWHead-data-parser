using System.Collections.Generic;
using System.Text.RegularExpressions;
using Sql;
using WoWHeadParser.Page;

namespace WoWHeadParser.Parser.Parsers
{
    internal class TrainerParser : DataParser
    {
        private Dictionary<TrainerType, string> _patterns = new Dictionary<TrainerType, string>
        {
            {TrainerType.TypeNone, ""},
            {TrainerType.TypeClass, "{[^}]*\"id\":(\\d+)[^}]*\"level\":(\\d+)[^}]*\"skill\":\\[(\\d+)?\\][^}]*\"trainingcost\":(\\d+)[^}]*"},
            {TrainerType.TypeTradeskills, "{[^}]*\"id\":(\\d+)[^}]*\"learnedat\":(\\d+)[^}]*\"level\":(\\d+)[^}]*\"skill\":\\[(\\d+)?\\][^}]*\"trainingcost\":(\\d+)[^}]*"},
        };

        private const string pattern = @"data: \[.*;";

        private const string trainerTypePattern = @"template: 'spell', id: ('[a-z\\-]+'), name: ";

        public override PageItem Parse(string page, uint id)
        {
            int npcflag = 0x10;
            TrainerType type = TrainerType.TypeNone;

            MatchCollection items = Regex.Matches(page, trainerTypePattern, RegexOptions.Multiline);
            foreach (Match item in items)
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

                int startIndex = item.Index;
                int endIndex = page.FastIndexOf("});", startIndex);

                page = page.Substring(startIndex, endIndex - startIndex + 3);
            }

            if (type == TrainerType.TypeNone)
                return new PageItem();

            string subPattern = _patterns[type];

            SqlBuilder builder = new SqlBuilder("npc_trainer");
            builder.SetFieldsNames("spell", "spellcost", "reqlevel", "reqSkill", "reqSkillValue");

            MatchCollection find = Regex.Matches(page, pattern);

            int count = find.Count;
            if (count > 0)
                builder.AppendSqlQuery(@"UPDATE `creature_template` SET `npcflag` = `npcflag` | '{0}', `trainer_type` = '{1}' WHERE `entry` = '{2}';", npcflag, (int)type, id);

            for (int i = 0; i < count; ++i)
            {
                MatchCollection matches = Regex.Matches(find[i].Value, subPattern);

                int matchesCount = matches.Count;
                for (int j = 0; j < matchesCount; ++j)
                {
                    GroupCollection groups = matches[j].Groups;

                    string spellCost = (type == TrainerType.TypeTradeskills) ? groups[5].Value : groups[4].Value; // TODO: support zero cost
                    switch (type)
                    {
                        case TrainerType.TypeTradeskills:
                            {
                                string reqSkill = (string.IsNullOrEmpty(groups[4].Value) ? "0" : groups[4].Value);
                                string reqSkillValue = (string.IsNullOrEmpty(groups[2].Value) ? "0" : groups[2].Value);
                                builder.AppendFieldsValue(id, groups[1].Value, spellCost, groups[3].Value, reqSkill, reqSkillValue);
                            }
                            break;
                        case TrainerType.TypeClass:
                            {
                                string reqSkill = (string.IsNullOrEmpty(groups[3].Value) ? "0" : groups[3].Value);
                                builder.AppendFieldsValue(id, groups[1].Value, spellCost, groups[2].Value, reqSkill, "0");
                            }
                            break;
                    }
                }
            }

            return new PageItem(id, builder.ToString());
        }

        public override string Name { get { return "Professions & Class Trainer data parser"; } }

        public override string Address { get { return "npc={0}"; } }

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