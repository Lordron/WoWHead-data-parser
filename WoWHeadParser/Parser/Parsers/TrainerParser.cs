using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace WoWHeadParser.Parser.Parsers
{
    [Parser(ParserType.Trainer)]
    internal class TrainerParser : PageParser
    {
        public TrainerParser(Locale locale, int flags)
            : base(locale, flags)
        {
            this.Address = "npc={0}";

            Builder.Setup("npc_trainer", "entry", true, "spell", "spellcost", "reqlevel", "reqSkill", "reqSkillValue");
        }

        private Dictionary<TrainerType, Regex> _patterns = new Dictionary<TrainerType, Regex>
        {
            {TrainerType.TypeNone, null},
            {TrainerType.TypeOther, new Regex("{[^}]*\"id\":(\\d+)[^}]*\"level\":(\\d+)[^}]*\"skill\":\\[(\\d+)?\\][^}]*")},
            {TrainerType.TypeClass, new Regex("{[^}]*\"id\":(\\d+)[^}]*\"level\":(\\d+)[^}]*\"skill\":\\[(\\d+)?\\][^}]*\"trainingcost\":(\\d+)[^}]*")},
            {TrainerType.TypeTradeskills, new Regex("{[^}]*\"id\":(\\d+)[^}]*\"learnedat\":(\\d+)[^}]*\"skill\":\\[(\\d+)?\\][^}]*\"trainingcost\":(\\d+)[^}]*")},
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

        private const string Zero = "0";

        private const string pattern = @"data: \[.*;";
        private Regex dataRegex = new Regex(pattern);

        private const string trainerTypePattern = @"template: 'spell', id: ('[a-z\\-]+'), name: ";
        private Regex trainerTypeRegex = new Regex(trainerTypePattern);

        public override void Parse(string page, uint id)
        {
            MatchCollection items = trainerTypeRegex.Matches(page);
            foreach (Match item in items)
            {
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
                    return;

                int startIndex = item.Index;
                int endIndex = page.FastIndexOf("});", startIndex);

                string template = page.Substring(startIndex, endIndex - startIndex + 3);

                Match find = dataRegex.Match(template);
                if (!find.Success)
                    continue;

                Builder.SetKey(id);
                Builder.AppendSqlQuery(id, @"UPDATE `creature_template` SET `npcflag` = `npcflag` | 0x{0:X4}, `trainer_type` = '{1}' WHERE `entry` = '{2}';", npcFlags[type], trainerTypes[type], id);

                MatchCollection matches = _patterns[type].Matches(find.Value);
                foreach (Match match in matches)
                {
                    GroupCollection groups = match.Groups;
                    switch (type)
                    {
                        case TrainerType.TypeOther:
                            {
                                string reqSkill = (string.IsNullOrEmpty(groups[3].Value) ? Zero : groups[3].Value);
                                Builder.AppendValues(groups[1].Value, Zero, groups[2].Value, reqSkill, Zero);
                            }
                            break;
                        case TrainerType.TypeClass:
                            {
                                string reqSkill = (string.IsNullOrEmpty(groups[3].Value) ? Zero : groups[3].Value);
                                string spellCost = (string.IsNullOrEmpty(groups[4].Value) ? Zero : groups[4].Value);
                                Builder.AppendValues(groups[1].Value, spellCost, groups[2].Value, reqSkill, Zero);
                            }
                            break;
                        case TrainerType.TypeTradeskills:
                            {
                                string reqSkill = (string.IsNullOrEmpty(groups[3].Value) ? Zero : groups[3].Value);
                                string reqSkillValue = (string.IsNullOrEmpty(groups[2].Value) ? Zero : groups[2].Value);
                                string spellCost = (string.IsNullOrEmpty(groups[4].Value) ? Zero : groups[4].Value);
                                Builder.AppendValues(groups[1].Value, spellCost, Zero, reqSkill, reqSkillValue);
                            }
                            break;
                    }

                    Builder.Flush();
                }
            }
        }

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