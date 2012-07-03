using System.Collections.Generic;
using System.Text.RegularExpressions;
using Sql;
using WoWHeadParser.Page;
using WoWHeadParser.Properties;

namespace WoWHeadParser.Parser.Parsers
{
    internal class TrainerParser : DataParser
    {
        private Dictionary<TrainerType, Regex> _patterns = new Dictionary<TrainerType, Regex>
        {
            {TrainerType.TypeNone, null},
            {TrainerType.TypeClass, new Regex("{[^}]*\"id\":(\\d+)[^}]*\"level\":(\\d+)[^}]*\"skill\":\\[(\\d+)?\\][^}]*\"trainingcost\":(\\d+)[^}]*")},
            {TrainerType.TypeTradeskills, new Regex("{[^}]*\"id\":(\\d+)[^}]*\"learnedat\":(\\d+)[^}]*\"level\":(\\d+)[^}]*\"skill\":\\[(\\d+)?\\][^}]*\"trainingcost\":(\\d+)[^}]*")},
        };

        private const string pattern = @"data: \[.*;";
        private Regex dataRegex = new Regex(pattern);

        private const string trainerTypePattern = @"template: 'spell', id: ('[a-z\\-]+'), name: ";
        private Regex trainerTypeRegex = new Regex(trainerTypePattern);

        public override PageItem Parse(string page, uint id)
        {
            int npcflag = 0x10;
            TrainerType type = TrainerType.TypeNone;

            MatchCollection items = trainerTypeRegex.Matches(page);
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

            Regex subPattern = _patterns[type];

            SqlBuilder builder = new SqlBuilder("npc_trainer");
            builder.SetFieldsNames("spell", "spellcost", "reqlevel", "reqSkill", "reqSkillValue");

            MatchCollection find = dataRegex.Matches(page);

            int count = find.Count;
            if (count > 0)
                builder.AppendSqlQuery(@"UPDATE `creature_template` SET `npcflag` = `npcflag` | '{0}', `trainer_type` = '{1}' WHERE `entry` = '{2}';", npcflag, (int)type, id);

            for (int i = 0; i < count; ++i)
            {
                MatchCollection matches = subPattern.Matches(find[i].Value);

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

        public override string Name { get { return Resources.TrainerParser; } }

        public override string Address { get { return "npc={0}"; } }

        public override string WelfName { get { return "trainer"; } }

        private enum TrainerType : sbyte
        {
            TypeNone = -1,
            TypeClass = 0,
            TypeMounts = 1,
            TypeTradeskills = 2,
            TypePets = 3,
        }
    }
}