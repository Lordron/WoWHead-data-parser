using Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace WoWHeadParser.Parser.Parsers
{
    [Parser(ParserType.Npc)]
    internal class NpcDataParser : PageParser
    {
        private SubParsers parsers;

        public NpcDataParser(Locale locale, int flags)
            : base(locale, flags)
        {
            this.Address = "npc={0}";

            parsers = (SubParsers)flags;

            for (int i = 0; i < ((int)SubParsersType.Max - 1); ++i)
            {
                Builders.Add(new SqlBuilder(_builderSettings));
            }

            if (parsers.HasFlag(SubParsers.Level))
                Builders[(int)SubParsersType.Level].Setup("creature_template", "entry", false, "minlevel", "maxlevel");

            if (parsers.HasFlag(SubParsers.Money))
                Builders[(int)SubParsersType.Money].Setup("creature_template", "entry", false, "mingold", "maxgold");

            if (parsers.HasFlag(SubParsers.Currency))
                Builders[(int)SubParsersType.Currency].Setup("creature_currency", "entry", false, "currencyId", "currencyAmount");

            if (parsers.HasFlag(SubParsers.Quotes))
                Builders[(int)SubParsersType.Quotes].Setup("creature_quotes", "entry", false, "id", "type", textFields[Locale]);

            if (parsers.HasFlag(SubParsers.Health))
                Builders[(int)SubParsersType.Health].Setup("creature_power", "entry", false, "Normal", "Heroic", "Normal10", "Normal25", "Heroic10", "Heroic25", "RaidFinder25");

            if (parsers.HasFlag(SubParsers.Mana))
                Builders[(int)SubParsersType.Mana].Setup("creature_power", "entry", false, "Mana");

            if (parsers.HasFlag(SubParsers.Faction))
                Builders[(int)SubParsersType.Faction].Setup("creature_faction", "entry", false, "faction_a", "faction_h");

            if (parsers.HasFlag(SubParsers.QuestStart))
                Builders[(int)SubParsersType.QuestStart].Setup("creature_questrelation", "id", false, "quest");

            if (parsers.HasFlag(SubParsers.QuestEnd))
                Builders[(int)SubParsersType.QuestEnd].Setup("creature_involvedrelation", "id", false, "quest");

            #region Creature_currency

            if (parsers.HasFlag(SubParsers.Currency))
            {
                Content.AppendLine(
     @"DROP TABLE IF EXISTS `creature_currency`;
CREATE TABLE `creature_currency` (
  `entry` int(10) NOT NULL,
  `currencyId` int(10) NOT NULL default '0',
  `currencyAmount` int(10) NOT NULL default '0',
  PRIMARY KEY (`entry`)
  ) ENGINE=MyISAM DEFAULT CHARSET=utf8;"
     );
            }

            #endregion

            Content.AppendLine();

            #region Creature_quotes

            if (parsers.HasFlag(SubParsers.Quotes))
            {
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
            }

            #endregion

            Content.AppendLine();

            #region Creature_power

            if (parsers.HasFlag(SubParsers.Health | SubParsers.Mana))
            {
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
            }

            #endregion

            Content.AppendLine();

            #region Creature_faction

            if (parsers.HasFlag(SubParsers.Faction))
            {
                Content.AppendLine(
     @"DROP TABLE IF EXISTS `creature_faction`;
CREATE TABLE `creature_faction` (
  `entry` int(10) NOT NULL,
  `faction_a` int(10) NOT NULL default '0',
  `faction_h` int(10) NOT NULL default '0',
  PRIMARY KEY (`entry`)
  ) ENGINE=MyISAM DEFAULT CHARSET=utf8;"
     );
            }

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

        private Dictionary<Locale, Regex> healthNormalLocales = new Dictionary<Locale, Regex>
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
        private const string questStartPattern = "template: 'quest', id: 'starts', name";
        private const string questEndPattern = "template: 'quest', id: 'ends', name";

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
                    Builders[(int)SubParsersType.Level].SetKey(id);
                    Builders[(int)SubParsersType.Level].AppendValues(levels.Item1, levels.Item2);
                    Builders[(int)SubParsersType.Level].Flush();
                }
            }

            if (parsers.HasFlag(SubParsers.Money))
            {
                int money;
                if (Money(page, out money))
                {
                    Builders[(int)SubParsersType.Money].SetKey(id);
                    Builders[(int)SubParsersType.Money].AppendValues(money, money);
                    Builders[(int)SubParsersType.Money].Flush();
                }
            }

            if (parsers.HasFlag(SubParsers.Currency))
            {
                Tuple<int, int> currency;
                if (Currency(page, out currency))
                {
                    Builders[(int)SubParsersType.Currency].SetKey(id);
                    Builders[(int)SubParsersType.Currency].AppendValues(currency.Item1, currency.Item2);
                    Builders[(int)SubParsersType.Currency].Flush();
                }
            }

            if (parsers.HasFlag(SubParsers.Quotes))
            {
                Tuple<int, string>[] quotes;
                if (Quotes(page, out quotes))
                {
                    Builders[(int)SubParsersType.Quotes].SetKey(id);
                    for (int i = 0; i < quotes.Length; ++i)
                    {
                        Tuple<int, string> quote = quotes[i];
                        Builders[(int)SubParsersType.Quotes].AppendValues(i, quote.Item1, quote.Item2);
                        Builders[(int)SubParsersType.Quotes].Flush();
                    }
                }
            }

            if (parsers.HasFlag(SubParsers.Health))
            {
                object[] health;
                if (Health(page, out health))
                {
                    Builders[(int)SubParsersType.Health].SetKey(id);
                    Builders[(int)SubParsersType.Health].AppendValues(health);
                    Builders[(int)SubParsersType.Health].Flush();
                }
            }

            if (parsers.HasFlag(SubParsers.Mana))
            {
                int mana;
                if (Mana(page, out mana))
                {
                    Builders[(int)SubParsersType.Mana].SetKey(id);
                    Builders[(int)SubParsersType.Mana].AppendValue(mana);
                    Builders[(int)SubParsersType.Mana].Flush();
                }
            }

            if (parsers.HasFlag(SubParsers.Faction))
            {
                int factionA, factionH;
                if (Faction(page, out factionA, out factionH))
                {
                    Builders[(int)SubParsersType.Faction].SetKey(id);
                    Builders[(int)SubParsersType.Faction].AppendValues(factionA, factionH);
                    Builders[(int)SubParsersType.Faction].Flush();
                }
            }

            if (parsers.HasFlag(SubParsers.QuestStart))
            {
                int[] questIds;
                if (QuestStart(page, out questIds))
                {
                    Builders[(int)SubParsersType.QuestStart].SetKey(id);
                    foreach (int questId in questIds)
                    {
                        Builders[(int)SubParsersType.QuestStart].AppendValue(questId);
                        Builders[(int)SubParsersType.QuestStart].Flush();
                    }
                }
            }

            if (parsers.HasFlag(SubParsers.QuestEnd))
            {
                int[] questIds;
                if (QuestEnd(page, out questIds))
                {
                    Builders[(int)SubParsersType.QuestEnd].SetKey(id);
                    foreach (int questId in questIds)
                    {
                        Builders[(int)SubParsersType.QuestEnd].AppendValue(questId);
                        Builders[(int)SubParsersType.QuestEnd].Flush();
                    }
                }
            }
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

            int count = 0;
            int[] levels = new int[MaxLevelCount];

            string[] values = levelString.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < values.Length; ++i)
            {
                int level;
                if (!int.TryParse(values[i].Trim(), out level))
                    continue;

                levels[count++] = level;
            }

            if (count == 0)
                return false;

            tuple = new Tuple<int, int>(levels[0], count == MinLevelCount ? levels[0] : levels[1]);
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

        private bool Quotes(string page, out Tuple<int, string>[] tuple)
        {
            MatchCollection matches = quotesRegex.Matches(page);

            tuple = new Tuple<int, string>[matches.Count];

            for (int i = 0; i < matches.Count; ++i)
            {
                GroupCollection groups = matches[i].Groups;
                string type = groups[1].Value;
                string text = groups[2].Value;

                tuple[i] = new Tuple<int, string>(textType[type.ToUpper()], text.HTMLEscapeSumbols());
            }

            return true;
        }

        private bool Health(string page, out object[] healths)
        {
            healths = new object[7] { 0, 0, 0, 0, 0, 0, 0 };

            MatchCollection matches = healthRegex.Matches(page);
            if (matches.Count > 0)
            {
                foreach (Match item in matches)
                {
                    GroupCollection groups = item.Groups;

                    string stringDifficulty = groups[1].Value.HTMLEscapeSumbols().Trim();
                    string health = groups[2].Value.Replace(",", "");

                    int difficultyIndex = -1;
                    if (string.IsNullOrEmpty(stringDifficulty))
                        difficultyIndex = 0;

                    difficultyIndex = difficultiesLocale[Locale].ToList().IndexOf(stringDifficulty);
                    if (difficultyIndex == -1)
                        throw new ArgumentOutOfRangeException("difficultyIndex");

                    healths[difficultyIndex] = int.Parse(health);
                }

                return true;
            }

            Match match = healthNormalLocales[Locale].Match(page);
            if (match.Success)
            {
                string health = match.Groups[2].Value.Replace(",", "");
                healths[0] = int.Parse(health);
            }

            return match.Success;
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

        private bool QuestStart(string page, out int[] val)
        {
            val = null;

            int startIndex = page.FastIndexOf(questStartPattern);
            if (startIndex == -1)
                return false;

            int endIndex = page.FastIndexOf("});", startIndex);
            string template = page.Substring(startIndex, endIndex - startIndex);

            MatchCollection matches = questRegex.Matches(template);

            val = new int[matches.Count];
            for (int i = 0; i < val.Length; ++i)
            {
                string questId = matches[i].Groups[1].Value;
                val[i] = int.Parse(questId);
            }
            return true;
        }

        private bool QuestEnd(string page, out int[] val)
        {
            val = null;

            int startIndex = page.FastIndexOf(questEndPattern);
            if (startIndex == -1)
                return false;

            int endIndex = page.FastIndexOf("});", startIndex);
            string template = page.Substring(startIndex, endIndex - startIndex);

            MatchCollection matches = questRegex.Matches(template);

            val = new int[matches.Count];
            for (int i = 0; i < val.Length; ++i)
            {
                string questId = matches[i].Groups[1].Value;
                val[i] = int.Parse(questId);
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

        public enum SubParsersType : int
        {
            Mana = 0,
            Health = 1,
            Money = 2,
            Currency = 3,
            Level = 4,
            Quotes = 5,
            Faction = 6,
            QuestStart = 7,
            QuestEnd = 8,
            Max,
        }

        public enum SubParsers : uint
        {
            Mana = 1 << SubParsersType.Mana,
            Health = 1 << SubParsersType.Health,
            Money = 1 << SubParsersType.Money,
            Currency = 1 << SubParsersType.Currency,
            Level = 1 << SubParsersType.Level,
            Quotes = 1 << SubParsersType.Quotes,
            Faction = 1 << SubParsersType.Faction,
            QuestStart = 1 << SubParsersType.QuestStart,
            QuestEnd = 1 << SubParsersType.QuestEnd,
        }
    }
}
