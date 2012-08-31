using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Sql;
using System.Linq;

namespace WoWHeadParser.Parser.Parsers
{
    [Parser(ParserType.Npc)]
    internal class NpcDataParser : PageParser
    {
        private SubParsers parsers;

        #region Sql Builder

        private SqlBuilder _moneyBuilder = new SqlBuilder(_builderSettings);
        private SqlBuilder _currencyBuilder = new SqlBuilder(_builderSettings);
        private SqlBuilder _quoteBuilder = new SqlBuilder(_builderSettings);
        private SqlBuilder _healthBuilder = new SqlBuilder(_builderSettings);
        private SqlBuilder _manaBuilder = new SqlBuilder(_builderSettings);
        private SqlBuilder _factionBuilder = new SqlBuilder(_builderSettings);
        private SqlBuilder _questStartBuilder = new SqlBuilder(_builderSettings);
        private SqlBuilder _questEndBuilder = new SqlBuilder(_builderSettings);

        #endregion

        public NpcDataParser(Locale locale, int flags)
            : base(locale, flags)
        {
            this.Address = "npc={0}";

            parsers = (SubParsers)flags;

            if (parsers.HasFlag(SubParsers.Level))
                Builder.Setup("creature_template", "entry", false, "minlevel", "maxlevel");

            if (parsers.HasFlag(SubParsers.Money))
                _moneyBuilder.Setup("creature_template", "entry", false, "mingold", "maxgold");

            if (parsers.HasFlag(SubParsers.Currency))
                _currencyBuilder.Setup("creature_currency", "entry", false, "currencyId", "currencyAmount");

            if (parsers.HasFlag(SubParsers.Quotes))
                _quoteBuilder.Setup("creature_quotes", "entry", false, "id", "type", textFields[Locale]);

            if (parsers.HasFlag(SubParsers.Health))
                _healthBuilder.Setup("creature_power", "entry", false, "Normal", "Heroic", "Normal10", "Normal25", "Heroic10", "Heroic25", "RaidFinder25");

            if (parsers.HasFlag(SubParsers.Mana))
                _manaBuilder.Setup("creature_power", "entry", false, "Mana");

            if (parsers.HasFlag(SubParsers.Faction))
                _factionBuilder.Setup("creature_faction", "entry", false, "faction_a", "faction_h");

            if (parsers.HasFlag(SubParsers.QuestStart))
                _questStartBuilder.Setup("creature_questrelation", "id", false, "quest");

            if (parsers.HasFlag(SubParsers.QuestEnd))
                _questEndBuilder.Setup("creature_involvedrelation", "id", false, "quest");

            #region Creature_currency

            Content.AppendLine(
 @"DROP TABLE IF EXISTS `creature_currency`;
CREATE TABLE `creature_currency` (
  `entry` int(10) NOT NULL,
  `currencyId` int(10) NOT NULL default '0',
  `currencyAmount` int(10) NOT NULL default '0',
  PRIMARY KEY (`entry`)
  ) ENGINE=MyISAM DEFAULT CHARSET=utf8;"
 );

            #endregion

            Content.AppendLine();

            #region Creature_quotes

            Content.AppendLine(
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

            Content.AppendLine();

            #region Creature_power

            Content.AppendLine(
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

            Content.AppendLine();

            #region Creature_faction

            Content.AppendLine(
 @"DROP TABLE IF EXISTS `creature_faction`;
CREATE TABLE `creature_faction` (
  `entry` int(10) NOT NULL,
  `faction_a` int(10) NOT NULL default '0',
  `faction_h` int(10) NOT NULL default '0',
  PRIMARY KEY (`entry`)
  ) ENGINE=MyISAM DEFAULT CHARSET=utf8;"
 );

            #endregion

            Content.AppendLine();
        }


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

        private Dictionary<Locale, Regex> manaLocales = new Dictionary<Locale, Regex>
        {
            {Locale.Russia, new Regex("Мана: (.*?)\\[/li\\]")},
            {Locale.English, new Regex("Mana: (.*?)\\[/li\\]")},
            {Locale.Germany, new Regex("Mana: (.*?)\\[/li\\]")},
            {Locale.France, new Regex("Mana : (.*?)\\[/li\\]")},
            {Locale.Spain, new Regex("Maná: (.*?)\\[/li\\]")},
        };

        private Dictionary<Locale, Regex> healthNormaLocales = new Dictionary<Locale, Regex>
        {
            {Locale.Russia, new Regex("Здоровье(.*?): ([^<]+)</div></li>")},
            {Locale.English, new Regex("Health(.*?): ([^<]+)</div></li>")},
            {Locale.Germany, new Regex("Gesundheit(.*?): ([^<]+)</div></li>")},
            {Locale.France, new Regex("Vie(.*?): ([^<]+)</div></li>")},
            {Locale.Spain, new Regex("Salud(.*?): ([^<]+)</div></li>")},
        };

        private Dictionary<Locale, string[]> difficultiesLocale = new Dictionary<Locale, string[]>
        {
            {Locale.Russia, new [] { "Обычный", "Героический", "10 нормал.", "25 нормал.", "10 героич.", "25 героич.", "Поиск Рейда на 25 человек" }},
            {Locale.English, new [] { "Normal", "Heroic", "10-player Normal", "25-player Normal", "10-player Heroic", "25-player Heroic", "25-player Raid Finder" }},
            {Locale.France, new [] { "Standard", "Héroïque", "10-joueurs Normal", "25-joueurs Normal", "10-joueurs Héroïque", "25-joueurs Héroïque", "Recherche de Raid à 25 joueurs" }},
            {Locale.Germany, new [] { "Normal", "Heroisch", "10-Spieler Normal", "25-Spieler Normal", "10-Spieler Heroisch", "25-Spieler Heroisch", "25-Spieler Schlachtzugsfinder" }},
            {Locale.Spain, new [] { "Normal", "Heroico", "10 jugadores Normal", "25 jugadores Normal", "10 jugadores Heroico", "25 jugadores Heroico", "Buscador de bandas de 25 jugadores" }},
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

        public override void Parse(string page, uint id)
        {
            if (parsers.HasFlag(SubParsers.Level))
            {
                Tuple<int, int> levels;
                if (Levels(page, out levels))
                {
                    Builder.SetKey(id);
                    Builder.AppendValues(levels.Item1, levels.Item2);
                    Builder.Flush();
                }
            }

            if (parsers.HasFlag(SubParsers.Money))
            {
                int money;
                if (Money(page, out money))
                {
                    _moneyBuilder.SetKey(id);
                    _moneyBuilder.AppendValues(money, money);
                    _moneyBuilder.Flush();
                }
            }

            if (parsers.HasFlag(SubParsers.Currency))
            {
                Tuple<int, int> currency;
                if (Currency(page, out currency))
                {
                    _currencyBuilder.SetKey(id);
                    _currencyBuilder.AppendValues(currency.Item1, currency.Item2);
                    _currencyBuilder.Flush();
                }
            }

            if (parsers.HasFlag(SubParsers.Quotes))
            {
                List<Tuple<int, string>> quotes;
                if (Quotes(page, out quotes))
                {
                    _quoteBuilder.SetKey(id);
                    for (int i = 0; i < quotes.Count; ++i)
                    {
                        Tuple<int, string> quote = quotes[i];
                        _quoteBuilder.AppendValues(i, quote.Item1, quote.Item2);
                        _quoteBuilder.Flush();
                    }
                }
            }

            /*if (parsers.HasFlag(SubParsers.Health))
            {
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
            }*/

            if (parsers.HasFlag(SubParsers.Mana))
            {
                int mana;
                if (Mana(page, out mana))
                {
                    _manaBuilder.SetKey(id);
                    _manaBuilder.AppendValue(mana);
                    _manaBuilder.Flush();
                }
            }

            if (parsers.HasFlag(SubParsers.Faction))
            {
                int factionA, factionH;
                if (Faction(page, out factionA, out factionH))
                {
                    _factionBuilder.SetKey(id);
                    _factionBuilder.AppendValues(factionA, factionH);
                    _factionBuilder.Flush();
                }
            }

            if (parsers.HasFlag(SubParsers.QuestStart))
            {
                List<string> questIds;
                if (QuestStart(page, out questIds))
                {
                    _questStartBuilder.SetKey(id);
                    foreach (string questId in questIds)
                    {
                        _questStartBuilder.AppendValue(questId);
                        _questStartBuilder.Flush();
                    }
                }
            }

            if (parsers.HasFlag(SubParsers.QuestEnd))
            {
                List<string> questIds;
                if (QuestEnd(page, out questIds))
                {
                    _questEndBuilder.SetKey(id);
                    foreach (string questId in questIds)
                    {
                        _questEndBuilder.AppendValue(questId);
                        _questEndBuilder.Flush();
                    }
                }
            }
        }

        public override void PutStringData()
        {
            if (parsers.HasFlag(SubParsers.Level))
                Content.Append(Builder);

            if (parsers.HasFlag(SubParsers.Money))
                Content.Append(_moneyBuilder);

            if (parsers.HasFlag(SubParsers.Currency))
                Content.Append(_currencyBuilder);

            if (parsers.HasFlag(SubParsers.Quotes))
                Content.Append(_quoteBuilder);

            if (parsers.HasFlag(SubParsers.Health))
                Content.Append(_healthBuilder);

            if (parsers.HasFlag(SubParsers.Mana))
                Content.Append(_manaBuilder);

            if (parsers.HasFlag(SubParsers.Faction))
                Content.Append(_factionBuilder);

            if (parsers.HasFlag(SubParsers.QuestStart))
                Content.Append(_questStartBuilder);

            if (parsers.HasFlag(SubParsers.QuestEnd))
                Content.Append(_questEndBuilder);
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

        private bool Quotes(string page, out List<Tuple<int, string>> tuple)
        {
            MatchCollection matches = quotesRegex.Matches(page);

            tuple = new List<Tuple<int, string>>(matches.Count);
            foreach (Match item in matches)
            {
                GroupCollection groups = item.Groups;
                string type = groups[1].Value;
                string text = groups[2].Value;

                tuple.Add(new Tuple<int, string>(textType[type.ToUpper()], text.HTMLEscapeSumbols()));
            }

            return true;
        }

        private int Health(string page, out List<string> difficulties, out List<string> healths)
        {
            int count;

            MatchCollection matches = healthRegex.Matches(page);
            if ((count = matches.Count) > 0)
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

            Match match = healthNormaLocales[Locale].Match(page);
            if (!match.Success)
            {
                healths = difficulties = null;
                return -1;
            }

            healths = new List<string>(1);
            difficulties = new List<string>(1);
            {
                string health = match.Groups[2].Value.Replace(",", "");
                string difficulty = "Normal";

                healths.Add(health);
                difficulties.Add(difficulty);
            }

            return 1;
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

            string[] difficulty = difficultiesLocale[Locale];
            int difficultyIndex = difficulty.ToList().IndexOf(stringDifficulty);
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

        [Flags]
        public enum SubParsers : uint
        {
            Mana = 0x0001,
            Health = 0x0002,
            Money = 0x0004,
            Currency = 0x0008,
            Level = 0x0010,
            Quotes = 0x0020,
            Faction = 0x0040,
            QuestStart = 0x0080,
            QuestEnd = 0x0100,
        }
    }
}
