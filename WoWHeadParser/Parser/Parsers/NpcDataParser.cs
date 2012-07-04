using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Sql;
using WoWHeadParser.Page;
using WoWHeadParser.Properties;

namespace WoWHeadParser.Parser.Parsers
{
    internal class NpcDataParser : DataParser
    {
        #region Level

        private Dictionary<Locale, Regex> levelLocales = new Dictionary<Locale, Regex>
        {
            {Locale.Russia, new Regex("Уровень: (.*?)\\[/li\\]")},
            {Locale.English, new Regex("Level: (.*?)\\[/li\\]")},
            {Locale.Germany, new Regex("Stufe: (.*?)\\[/li\\]")},
            {Locale.France, new Regex("Niveau : (.*?)\\[/li\\]")},
            {Locale.Spain, new Regex("Nivel: (.*?)\\[/li\\]")},
        };

        private const int MinLevelCount = 1;
        private const int MaxLevelCount = 2;
        private const int BossLevel = 255;
        private const string BossLevelString = "??";

        #endregion

        #region Text

        private Dictionary<string, int> textType = new Dictionary<string, int>
        {
            {"S1", 14},
            {"S2", 12},
            {"S3", 15},
        };

        private Dictionary<Locale, string> textFields = new Dictionary<Locale, string>
        {
            {Locale.English, "text"},
            {Locale.Russia, "text_loc1"},
            {Locale.Germany, "text_loc2"},
            {Locale.France, "text_loc3"},
            {Locale.Spain, "text_loc4"},
        };

        #endregion

        #region Power

        private List<Dictionary<Locale, Regex>> healthDifficulty;

        private Dictionary<Locale, Regex> manaLocales = new Dictionary<Locale, Regex>
        {
            {Locale.Russia, new Regex("Мана: (.*?)\\[/li\\]")},
            {Locale.English, new Regex("Mana: (.*?)\\[/li\\]")},
            {Locale.Germany, new Regex("Mana: (.*?)\\[/li\\]")},
            {Locale.France, new Regex("Mana : (.*?)\\[/li\\]")},
            {Locale.Spain, new Regex("Maná: (.*?)\\[/li\\]")},
        };

        private Dictionary<Locale, Regex> healthLocales = new Dictionary<Locale, Regex>
        {
            {Locale.Russia, new Regex("Здоровье: ([^<]+)</div></li>")},
            {Locale.English, new Regex("Health: ([^<]+)</div></li>")},
            {Locale.Germany, new Regex("Gesundheit: ([^<]+)</div></li>")},
            {Locale.France, new Regex("Vie: ([^<]+)</div></li>")},
            {Locale.Spain, new Regex("Salud: ([^<]+)</div></li>")},
        };

        private Dictionary<Locale, Regex> healthNormaLocales = new Dictionary<Locale, Regex>
        {
            {Locale.Russia, new Regex("Здоровье.+: ([^<]+)</div></li>")},
            {Locale.English, new Regex("Health.+: ([^<]+)</div></li>")},
            {Locale.Germany, new Regex("Gesundheit.+: ([^<]+)</div></li>")},
            {Locale.France, new Regex("Vie.+: ([^<]+)</div></li>")},
            {Locale.Spain, new Regex("Salud.+: ([^<]+)</div></li>")},
        };

        private Dictionary<Locale, List<string>> difficultiesLocale = new Dictionary<Locale, List<string>>
        {
            {Locale.Russia, new List<string> { "Обычный", "Героический", "10 нормал.", "25 нормал.", "10 героич.", "25 героич.", "Поиск Рейда на 25 человек" }},
            {Locale.English, new List<string> { "Normal", "Heroic", "10-player Normal", "25-player Normal", "10-player Heroic", "25-player Heroic", "25-player Raid Finder" }},
            {Locale.France, new List<string> { "Standard", "Héroïque", "10-joueurs Normal", "25-joueurs Normal", "10-joueurs Héroïque", "25-joueurs Héroïque", "Recherche de Raid à 25 joueurs" }},
            {Locale.Germany, new List<string> { "Normal", "Heroisch", "10-Spieler Normal", "25-Spieler Normal", "10-Spieler Heroisch", "25-Spieler Heroisch", "25-Spieler Schlachtzugsfinder" }},
            {Locale.Spain, new List<string> { "Normal", "Heroico", "10 jugadores Normal", "25 jugadores Normal", "10 jugadores Heroico", "25 jugadores Heroico", "Buscador de bandas de 25 jugadores" }},
        };

        #endregion

        #region Faction

        private const string FactionAlliance = "A";
        private const int MinFactionCount = 1;
        private const int MaxFactionCount = 2;

        private Dictionary<string, FactionColor> factionColors = new Dictionary<string, FactionColor>
        {
            {"q", FactionColor.Yellow},
            {"q2", FactionColor.Green},
            {"q10", FactionColor.Red},
        };

        private Dictionary<Locale, Regex> reactLocales = new Dictionary<Locale, Regex>
        {
            {Locale.Russia, new Regex("Реакция: (.*?)\\[/li\\]")},
            {Locale.English, new Regex("React: (.*?)\\[/li\\]")},
            {Locale.Germany, new Regex("Einstellung: (.*?)\\[/li\\]")},
            {Locale.France, new Regex("Réaction : (.*?)\\[/li\\]")},
            {Locale.Spain, new Regex("Reacción: (.*?)\\[/li\\]")},
        };

        #endregion

        #region Regex

        private const string moneyPattern = "money=(.*?)]";
        private const string currencyPattern = "currency=(.*?) amount=(.*?)]";
        private const string quotesPattern = "<li><div><span class=\"(.*?)\">(.*?)</span></div></li>";
        private const string healthPattern = @"<tr><td>([^<]+)</td><td style=&quot;text-align:right&quot;>([^<]+)</td>";
        private const string factionPattern = "color=(.*?)](.*?)\\[/color\\]";
        private const string questPattern = "\"id\":(.*?),";

        private Regex moneyRegex = new Regex(moneyPattern);
        private Regex currencyRegex = new Regex(currencyPattern);
        private Regex quotesRegex = new Regex(quotesPattern);
        private Regex healthRegex = new Regex(healthPattern);
        private Regex factionRegex = new Regex(factionPattern);
        private Regex questRegex = new Regex(questPattern);

        #endregion

        public override void Prepare()
        {
            healthDifficulty = new List<Dictionary<Locale, Regex>>
            {
                healthLocales,
                healthNormaLocales,
            };
        }

        public override PageItem Parse(string page, uint id)
        {
            SqlBuilder builder;
            StringBuilder content = new StringBuilder(1024);

            Tuple<int, int> levels;
            if (Levels(page, out levels))
            {
                builder = new SqlBuilder("creature_template");
                builder.SetFieldsNames("minlevel", "maxlevel");
                builder.AppendFieldsValue(id, levels.Item1, levels.Item2);
                content.Append(builder);
            }

            int money;
            if (Money(page, out money))
            {
                builder = new SqlBuilder("creature_template");
                builder.SetFieldsNames("mingold", "maxgold");
                builder.AppendFieldsValue(id, money, money);
                content.Append(builder);
            }

            Tuple<int, int> currency;
            if (Currency(page, out currency))
            {
                builder = new SqlBuilder("creature_currency");
                builder.SetFieldsNames("currencyId", "currencyAmount");
                builder.AppendFieldsValue(id, currency.Item1, currency.Item2);
                content.Append(builder);
            }

            List<Tuple<string, int>> quotes;
            if (Quotes(page, out quotes))
            {
                builder = new SqlBuilder("creature_quotes");
                builder.SetFieldsNames("id", "type", textFields[Locale]);
                for (int i = 0; i < quotes.Count; ++i)
                {
                    Tuple<string, int> quote = quotes[i];
                    builder.AppendFieldsValue(id, i, quote.Item2, quote.Item1);
                }
                content.Append(builder);
            }

            int healthCount;
            List<string> healths;
            List<string> difficulties;
            if ((healthCount = Health(page, out difficulties, out healths)) > 0)
            {
                builder = new SqlBuilder("creature_power");
                builder.SetFieldsNames(difficulties, healthCount);
                builder.AppendFieldsValue(id, healths, healthCount);
                content.Append(builder);
            }

            int mana;
            if (Mana(page, out mana))
            {
                builder = new SqlBuilder("creature_power");
                builder.SetFieldsNames("mana");
                builder.AppendFieldsValue(id, mana);
                content.Append(builder);
            }

            int factionA, factionH;
            if (Faction(page, out factionA, out factionH))
            {
                builder = new SqlBuilder("creature_faction");
                builder.SetFieldsNames("faction_a", "faction_h");
                builder.AppendFieldsValue(id, factionA, factionH);
                content.Append(builder);
            }

            List<string> questIds;
            if (QuestStart(page, out questIds))
            {
                builder = new SqlBuilder("creature_questrelation");
                builder.SetFieldsNames("quest");
                foreach (string questId in questIds)
                {
                    builder.AppendFieldsValue(id, questId);
                }
                content.Append(builder);
            }

            if (QuestEnd(page, out questIds))
            {
                builder = new SqlBuilder("creature_involvedrelation");
                builder.SetFieldsNames("quest");
                foreach (string questId in questIds)
                {
                    builder.AppendFieldsValue(id, questId);
                }
                content.Append(builder);
            }

            return new PageItem(id, content.ToString());
        }

        private bool Levels(string page, out Tuple<int, int> tuple)
        {
            tuple = new Tuple<int, int>(BossLevel, BossLevel);

            Match item = levelLocales[Locale].Match(page);
            if (!item.Success)
                return false;

            string levelString = item.Groups[1].Value;
            if (levelString.FastIndexOf(BossLevelString) > -1)
                return true;

            List<int> levels = new List<int>(MaxLevelCount);

            string[] values = levelString.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string value in values)
            {
                int level;
                if (!int.TryParse(value.Trim(), out level))
                    continue;

                levels.Add(level);
            }

            if (levels.Count <= 0)
                return false;

            tuple = new Tuple<int, int>(levels[0], levels.Count == MinLevelCount ? levels[0] : levels[1]);
            return true;
        }

        private bool Money(string page, out int val)
        {
            val = -1;

            Match item = moneyRegex.Match(page);
            if (!item.Success)
                return false;

            string stringMoney = item.Groups[1].Value;
            if (!int.TryParse(stringMoney.Trim(), out val))
                return false;

            return val > 0;
        }

        private bool Currency(string page, out Tuple<int, int> tuple)
        {
            tuple = new Tuple<int, int>(-1, -1);

            Match item = currencyRegex.Match(page);
            if (!item.Success)
                return false;

            GroupCollection groups = item.Groups;
            string currency = groups[1].Value;
            string count = groups[2].Value;

            int currencyId;
            if (!int.TryParse(currency.Trim(), out currencyId))
                return false;

            int currencyAmount;
            if (!int.TryParse(count.Trim(), out currencyAmount))
                return false;

            tuple = new Tuple<int, int>(currencyId, currencyAmount);
            return true;
        }

        private bool Quotes(string page, out List<Tuple<string, int>> tuple)
        {
            MatchCollection matches = quotesRegex.Matches(page);

            tuple = new List<Tuple<string, int>>(matches.Count);
            foreach (Match item in matches)
            {
                GroupCollection groups = item.Groups;
                string type = groups[1].Value;
                string text = groups[2].Value;

                tuple.Add(new Tuple<string, int>(text.HTMLEscapeSumbols(), textType[type.ToUpper()]));
            }

            return true;
        }

        private int Health(string page, out List<string> difficulties, out List<string> healths)
        {
            MatchCollection matches = healthRegex.Matches(page);

            int count = matches.Count;
            if (count > 0)
            {
                healths = new List<string>(count);
                difficulties = new List<string>(count);

                foreach (Match item in matches)
                {
                    GroupCollection groups = item.Groups;
                    string stringDifficulty = groups[1].Value.HTMLEscapeSumbols().Trim();
                    string health = groups[2].Value.Replace(",", "");
                    Difficulty difficulty = GetDifficulty(stringDifficulty);

                    healths.Add(health);
                    if (difficulties.Contains(difficulty.ToString()))
                        difficulties.Add((difficulty + 1).ToString());
                    else
                        difficulties.Add(difficulty.ToString());
                }
                return count;
            }

            foreach (Dictionary<Locale, Regex> kvp in healthDifficulty)
            {
                matches = kvp[Locale].Matches(page);
                if ((count = matches.Count) > 0)
                    break;
            }

            if (count == 0)
            {
                healths = difficulties = null;
                return -1;
            }

            healths = new List<string>(count);
            difficulties = new List<string>(count);
            for (int i = 0; i < count; ++i)
            {
                Match item = matches[i];
                string health = item.Groups[1].Value.Replace(",", "");
                string difficulty = ((Difficulty)i).ToString();

                healths.Add(health);
                difficulties.Add(difficulty);
            }

            return count;
        }

        private bool Mana(string page, out int mana)
        {
            mana = -1;

            Match item = manaLocales[Locale].Match(page);
            if (!item.Success)
                return false;

            string manaString = item.Groups[1].Value.Replace(",", "");
            return int.TryParse(manaString, out mana);
        }

        private Difficulty GetDifficulty(string stringDifficulty)
        {
            if (string.IsNullOrEmpty(stringDifficulty))
                return Difficulty.Normal;

            List<string> difficulty = difficultiesLocale[Locale];
            int difficultyIndex = difficulty.IndexOf(stringDifficulty);
            if (difficultyIndex == -1)
                throw new ArgumentOutOfRangeException("difficultyIndex");

            return (Difficulty)difficultyIndex;
        }

        private bool Faction(string page, out int factionA, out int factionH)
        {
            factionA = factionH = -1;

            Match blockItem = reactLocales[Locale].Match(page);
            if (!blockItem.Success)
                return false;

            string colorString = blockItem.Groups[1].Value;

            List<Tuple<FactionColor, string>> factions = new List<Tuple<FactionColor, string>>(MaxFactionCount);

            MatchCollection matches = factionRegex.Matches(colorString);
            foreach (Match item in matches)
            {
                GroupCollection groups = item.Groups;
                string color = groups[1].Value;
                string faction = groups[2].Value;

                factions.Add(new Tuple<FactionColor, string>(factionColors[color], faction));
            }

            int count = factions.Count;
            if (count < MinFactionCount)
                return false;

            switch (count)
            {
                case MinFactionCount:
                    {
                        Tuple<FactionColor, string> faction = factions[0];
                        switch (faction.Item1)
                        {
                            case FactionColor.Yellow:
                                factionA = factionH = 7;
                                break;
                            case FactionColor.Red:
                                factionA = factionH = 14;
                                break;
                            case FactionColor.Green:
                                factionA = factionH = (faction.Item2.Equals(FactionAlliance) ? 1802 : 1801);
                                break;
                        }
                    }
                    break;
                case MaxFactionCount:
                    {
                        FactionColor color1 = factions[0].Item1;
                        FactionColor color2 = factions[1].Item1;
                        if (color1.Equals(color2))
                        {
                            switch (color1)
                            {
                                case FactionColor.Yellow:
                                    factionA = factionH = 7;
                                    break;
                                case FactionColor.Red:
                                    factionA = factionH = 14;
                                    break;
                                case FactionColor.Green:
                                    factionA = factionH = 35;
                                    break;
                            }
                        }
                        else
                        {
                            switch (color1)
                            {
                                case FactionColor.Red:
                                    {
                                        switch (color2)
                                        {
                                            case FactionColor.Green:
                                                factionA = factionH = 1801;
                                                break;
                                            case FactionColor.Yellow:
                                                factionA = 14;
                                                factionH = 7;
                                                break;
                                        }
                                        break;
                                    }
                                case FactionColor.Green:
                                    {
                                        switch (color2)
                                        {
                                            case FactionColor.Red:
                                                factionA = factionH = 1802;
                                                break;
                                            case FactionColor.Yellow:
                                                factionA = 35;
                                                factionH = 7;
                                                break;
                                        }
                                        break;
                                    }
                                case FactionColor.Yellow:
                                    {
                                        factionA = 7;
                                        switch (color2)
                                        {
                                            case FactionColor.Red:
                                                factionH = 14;
                                                break;
                                            case FactionColor.Green:
                                                factionH = 35;
                                                break;
                                        }
                                        break;
                                    }
                            }
                        }
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException("Faction count", count, "Bad value");
            }

            return factionA > -1 && factionH > -1;
        }

        private bool QuestStart(string page, out List<string> val)
        {
            val = new List<string>(16);

            const string pattern = "template: 'quest', id: 'starts', name";
            int startIndex = page.FastIndexOf(pattern);
            if (startIndex == -1)
                return false;

            int endIndex = page.FastIndexOf("});", startIndex);
            string template = page.Substring(startIndex, endIndex - startIndex);

            MatchCollection matches = questRegex.Matches(template);
            foreach (Match item in matches)
            {
                string questId = item.Groups[1].Value;
                val.Add(questId);
            }
            return true;
        }

        private bool QuestEnd(string page, out List<string> val)
        {
            val = new List<string>(16);

            const string pattern = "template: 'quest', id: 'ends', name";
            int startIndex = page.FastIndexOf(pattern);
            if (startIndex == -1)
                return false;

            int endIndex = page.FastIndexOf("});", startIndex);
            string template = page.Substring(startIndex, endIndex - startIndex);

            MatchCollection matches = questRegex.Matches(template);
            foreach (Match item in matches)
            {
                string questId = item.Groups[1].Value;
                val.Add(questId);
            }
            return true;
        }

        public override string PreParse()
        {
            StringBuilder content = new StringBuilder(1024);

            #region Creature_currency

            content.AppendLine(
 @"DROP TABLE IF EXISTS `creature_currency`;
CREATE TABLE `creature_currency` (
  `entry` int(10) NOT NULL,
  `currencyId` int(10) NOT NULL default '0',
  `currencyAmount` int(10) NOT NULL default '0',
  PRIMARY KEY (`entry`)
  ) ENGINE=MyISAM DEFAULT CHARSET=utf8;"
 );

            #endregion

            content.AppendLine();

            #region Creature_quotes

            content.AppendLine(
@"DROP TABLE IF EXISTS `creature_quotes`;
CREATE TABLE `creature_quotes` (
  `entry` mediumint(8) unsigned NOT NULL DEFAULT '0',
  `id` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `type` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `text` longtext,
  `text_loc1` longtext,
  `text_loc2` longtext,
  `text_loc3` longtext,
  `text_loc4` longtext,
  PRIMARY KEY (`entry`,`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;"
);

            #endregion

            content.AppendLine();

            #region Creature_power

            content.AppendLine(
 @"DROP TABLE IF EXISTS `creature_power`;
CREATE TABLE `creature_power` (
  `entry` int(10) NOT NULL,
  `Mana` int(10) NOT NULL default '0',
  `Normal` int(10) NOT NULL default '0',
  `Heroic` int(10) NOT NULL default '0',
  `Normal10` int(10) NOT NULL default '0',
  `Normal25` int(10) NOT NULL default '0',
  `Heroic10` int(10) NOT NULL default '0',
  `Heroic25` int(10) NOT NULL default '0',
  `RaidFinder25` int(10) NOT NULL default '0',
  PRIMARY KEY (`entry`)
  ) ENGINE=MyISAM DEFAULT CHARSET=utf8;"
 );

            #endregion

            content.AppendLine();

            #region Creature_faction

            content.AppendLine(
 @"DROP TABLE IF EXISTS `creature_faction`;
CREATE TABLE `creature_faction` (
  `entry` int(10) NOT NULL,
  `faction_a` int(10) NOT NULL default '0',
  `faction_h` int(10) NOT NULL default '0',
  PRIMARY KEY (`entry`)
  ) ENGINE=MyISAM DEFAULT CHARSET=utf8;"
 );

            #endregion

            return content.AppendLine().ToString();
        }

        public override string Name { get { return Resources.NpcDataParser; } }

        public override string Address { get { return "npc={0}"; } }

        public override string WelfName { get { return "npc"; } }

        private enum Difficulty : byte
        {
            Normal,
            Heroic,
            Normal10,
            Normal25,
            Heroic10,
            Heroic25,
            RaidFinder25,
            Max,
        }

        private enum FactionColor : byte
        {
            Red,
            Yellow,
            Green,
        }
    }
}
