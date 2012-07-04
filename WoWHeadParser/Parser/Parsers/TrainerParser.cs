using System.Collections.Generic;
using System.Text;
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
            {TrainerType.TypeOther, new Regex("{[^}]*\"id\":(\\d+)[^}]*\"level\":(\\d+)[^}]*\"skill\":\\[(\\d+)?\\][^}]*")},
            {TrainerType.TypeClass, new Regex("{[^}]*\"id\":(\\d+)[^}]*\"level\":(\\d+)[^}]*\"skill\":\\[(\\d+)?\\][^}]*\"trainingcost\":(\\d+)[^}]*")},
            {TrainerType.TypeTradeskills, new Regex("{[^}]*\"id\":(\\d+)[^}]*\"learnedat\":(\\d+)[^}]*\"level\":(\\d+)[^}]*\"skill\":\\[(\\d+)?\\][^}]*\"trainingcost\":(\\d+)[^}]*")},
        };

        private Dictionary<TrainerType, int> npcFlags = new Dictionary<TrainerType, int>
        {
            {TrainerType.TypeOther, 0x30},
            {TrainerType.TypeClass, 0x30},
            {TrainerType.TypeTradeskills, 0x50},
        };

        private Dictionary<TrainerType, int> trainerTypes = new Dictionary<TrainerType, int>
        {
            {TrainerType.TypeClass, 0},
            {TrainerType.TypeTradeskills, 2},
            {TrainerType.TypeOther, 0},
        };

        private const string pattern = @"data: \[.*;";
        private Regex dataRegex = new Regex(pattern);

        private const string trainerTypePattern = @"template: 'spell', id: ('[a-z\\-]+'), name: ";
        private Regex trainerTypeRegex = new Regex(trainerTypePattern);

        public override PageItem Parse(string page, uint id)
        {
            StringBuilder content = new StringBuilder(4096);

            MatchCollection items = trainerTypeRegex.Matches(page);
            foreach (Match item in items)
            {
                string template = string.Empty;
                TrainerType type = TrainerType.TypeNone;

                switch (item.Groups[1].Value)
                {
                    case "\'teaches-other\'":
                        type = TrainerType.TypeOther;
                        break;
                    case "\'teaches-ability\'":
                        type = TrainerType.TypeClass;
                        break;
                    case "\'teaches-recipe\'":
                        type = TrainerType.TypeTradeskills;
                        break;
                    default:
                        continue;
                }

                if (type == TrainerType.TypeNone)
                    return new PageItem();

                int startIndex = item.Index;
                int endIndex = page.FastIndexOf("});", startIndex);

                template = page.Substring(startIndex, endIndex - startIndex + 3);

                SqlBuilder builder = new SqlBuilder("npc_trainer");
                builder.SetFieldsNames("spell", "spellcost", "reqlevel", "reqSkill", "reqSkillValue");

                MatchCollection find = dataRegex.Matches(template);

                int count = find.Count;
                if (count > 0)
                    builder.AppendSqlQuery(@"UPDATE `creature_template` SET `npcflag` = `npcflag` | '0x{0:X4}', `trainer_type` = '{1}' WHERE `entry` = '{2}';", npcFlags[type], trainerTypes[type], id);

                Regex subPattern = _patterns[type];

                for (int i = 0; i < count; ++i)
                {
                    MatchCollection matches = subPattern.Matches(find[i].Value);

                    int matchesCount = matches.Count;
                    for (int j = 0; j < matchesCount; ++j)
                    {
                        GroupCollection groups = matches[j].Groups;
                        switch (type)
                        {
                            case TrainerType.TypeOther:
                                {
                                    string reqSkill = (string.IsNullOrEmpty(groups[3].Value) ? "0" : groups[3].Value);
                                    builder.AppendFieldsValue(id, groups[1].Value, "0", groups[2].Value, reqSkill, "0");
                                }
                                break;
                            case TrainerType.TypeClass:
                                {
                                    string reqSkill = (string.IsNullOrEmpty(groups[3].Value) ? "0" : groups[3].Value);
                                    string spellCost = (string.IsNullOrEmpty(groups[4].Value) ? "0" : groups[4].Value);
                                    builder.AppendFieldsValue(id, groups[1].Value, spellCost, groups[2].Value, reqSkill, "0");
                                }
                                break;
                            case TrainerType.TypeTradeskills:
                                {
                                    string reqSkill = (string.IsNullOrEmpty(groups[4].Value) ? "0" : groups[4].Value);
                                    string reqSkillValue = (string.IsNullOrEmpty(groups[2].Value) ? "0" : groups[2].Value);
                                    string spellCost = (string.IsNullOrEmpty(groups[5].Value) ? "0" : groups[5].Value);
                                    builder.AppendFieldsValue(id, groups[1].Value, spellCost, groups[3].Value, reqSkill, reqSkillValue);
                                }
                                break;
                        }
                    }
                }

                content.Append(builder.ToString());
            }

            return new PageItem(id, content.ToString());
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
            TypeOther = 4,
        }
    }
}