using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Sql;
using WoWHeadParser.Page;

namespace WoWHeadParser.Parser.Parsers
{
    internal class NpcDataParser : DataParser
    {
        #region Level

        private Dictionary<Locale, string> levelLocales = new Dictionary<Locale, string>
        {
            {Locale.Russia, "Уровень"},
            {Locale.English, "Level"},
            {Locale.Germany, "Stufe"},
            {Locale.France, "Dévoreur"},
            {Locale.Spain, "Nivel"},
        };

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

        #region Health

        private List<Dictionary<Locale, string>> healthDifficulty;

        private Dictionary<Locale, string> healthLocales = new Dictionary<Locale, string>
        {
            {Locale.Russia, "Здоровье"},
            {Locale.English, "Health"},
            {Locale.Germany, "Gesundheit"},
            {Locale.France, "Vie"},
            {Locale.Spain, "Salud"},
        };

        private Dictionary<Locale, string> healthNormaLocales = new Dictionary<Locale, string>
        {
            {Locale.Russia, "Здоровье.+"},
            {Locale.English, "Health (Normal)"},
            {Locale.Germany, "Gesundheit (Normal)"},
            {Locale.France, "Vie (Standard)"},
            {Locale.Spain, "Gesundheit (Normal)"},
        };

        private Dictionary<Locale, List<string>> difficulty = new Dictionary<Locale, List<string>>
        {
            {Locale.Russia, new List<string> { "Обычный", "Героический", "10 нормал.", "25 нормал.", "10 героич.", "25 героич.", "Поиск Рейда на 25 человек" }},
            {Locale.English, new List<string> { "Normal", "Heroic", "10-player Normal", "25-player Normal", "10-player Heroic", "25-player Heroic", "25-player Raid Finder" }},
            {Locale.France, new List<string> { "Standard", "Héroïque", "10-joueurs Normal", "25-joueurs Normal", "10-joueurs Héroïque", "25-joueurs Héroïque", "Recherche de Raid à 25 joueurs" }},
            {Locale.Germany, new List<string> { "Normal", "Heroisch", "10-Spieler Normal", "25-Spieler Normal", "10-Spieler Heroisch", "25-Spieler Heroisch", "25-Spieler Schlachtzugsfinder" }},
            {Locale.Spain, new List<string> { "Normal", "Heroico", "10 jugadores Normal", "25 jugadores Normal", "10 jugadores Heroico", "25 jugadores Heroico", "Buscador de bandas de 25 jugadores" }},
        };

        #endregion

        #region Constants

        private const int MaxDiffSize = 30;
        private const int MinLevelCount = 1;
        private const int MaxLevelCount = 2;
        private const int BossLevel = 9999;
        private const string BossLevelString = "??";

        #endregion

        public override void Prepare()
        {
            healthDifficulty = new List<Dictionary<Locale, string>>
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
            List<string> healths = new List<string>((int)Difficulty.Max);
            List<string> difficulties = new List<string>((int)Difficulty.Max);
            if ((healthCount = Health(page, out difficulties, out healths)) > 0)
            {
                builder = new SqlBuilder("creature_health");
                builder.SetFieldsNames(difficulties, healthCount);
                builder.AppendFieldsValue(id, healths, healthCount);
                content.Append(builder);
            }

            return new PageItem(id, content.ToString());
        }

        private bool Levels(string page, out Tuple<int, int> tuple)
        {
            tuple = new Tuple<int, int>(-1, -1);

            string pattern = string.Format("[li]{0}:", levelLocales[Locale]);

            int startIndex = page.FastIndexOf(pattern) + pattern.Length;
            if (startIndex == -1)
                return false;

            int endIndex = page.FastIndexOf("[/li]", startIndex);
            if (endIndex == -1)
                return false;

            int diff = endIndex - startIndex;
            if (diff > MaxDiffSize)
                return false;

            string levelString = page.Substring(startIndex, diff);

            if (levelString.FastIndexOf(BossLevelString) > -1)
            {
                tuple = new Tuple<int, int>(BossLevel, BossLevel);
                return true;
            }

            string[] values = levelString.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            List<int> levels = new List<int>(MaxLevelCount);
            foreach (string value in values)
            {
                int level = 0;
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

            const string pattern = "money=(.*?)]";

            MatchCollection matches = Regex.Matches(page, pattern, RegexOptions.Multiline);
            foreach (Match item in matches)
            {
                string stringMoney = item.Groups[1].Value;

                if (!int.TryParse(stringMoney.Trim(), out val))
                    continue;

                return true;
            }

            return false;
        }

        private bool Currency(string page, out Tuple<int, int> tuple)
        {
            tuple = new Tuple<int, int>(-1, -1);

            const string pattern = "currency=(.*?) amount=(.*?)]";

            MatchCollection matches = Regex.Matches(page, pattern, RegexOptions.Multiline);
            foreach (Match item in matches)
            {
                string currency = item.Groups[1].Value;
                string count = item.Groups[2].Value;

                int currencyId = 0;
                if (!int.TryParse(currency.Trim(), out currencyId))
                    continue;

                int currencyAmount = 0;
                if (!int.TryParse(count.Trim(), out currencyAmount))
                    continue;

                tuple = new Tuple<int, int>(currencyId, currencyAmount);
                return true;
            }

            return false;
        }

        private bool Quotes(string page, out List<Tuple<string, int>> tuple)
        {
            const string pattern = "<li><div><span class=\"(.*?)\">(.*?)</span></div></li>";

            MatchCollection matches = Regex.Matches(page, pattern, RegexOptions.Multiline);

            tuple = new List<Tuple<string, int>>(matches.Count);
            foreach (Match item in matches)
            {
                string type = item.Groups[1].Value;
                string text = item.Groups[2].Value;

                tuple.Add(new Tuple<string, int>(text.HTMLEscapeSumbols(), textType[type.ToUpper()]));
            }

            return true;
        }

        private int Health(string page, out List<string> difficulties, out List<string> healths)
        {
            string pattern = @"<tr><td>([^<]+)</td><td style=&quot;text-align:right&quot;>([^<]+)</td>";

            MatchCollection matches = Regex.Matches(page, pattern, RegexOptions.Multiline);

            int count = -1;

            if ((count = matches.Count) > 0)
            {
                healths = new List<string>(count);
                difficulties = new List<string>(count);

                foreach (Match item in matches)
                {
                    string stringDifficulty = item.Groups[1].Value.HTMLEscapeSumbols().Trim();
                    string health = item.Groups[2].Value.Replace(",", "");
                    Difficulty difficulty = GetDifficulty(stringDifficulty);

                    healths.Add(health);
                    difficulties.Add(difficulty.ToString());
                }
                return count;
            }

            foreach (Dictionary<Locale, string> kvp in healthDifficulty)
            {
                pattern = string.Format("{0}: ([^<]+)</div></li>", kvp[Locale]);
                matches = Regex.Matches(page, pattern, RegexOptions.Multiline);
                count = matches.Count;

                if (count > 0)
                    break;
            }

            if (count == 0)
            {
                healths = difficulties = null;
                return -1;
            }

            healths = new List<string>(count);
            difficulties = new List<string>(count);
            foreach (Match item in matches)
            {
                string health = item.Groups[1].Value.Replace(",", "");

                healths.Add(health);
                difficulties.Add("Normal");
            }

            return count;
        }

        private Difficulty GetDifficulty(string stringDifficulty)
        {
            if (string.IsNullOrEmpty(stringDifficulty))
                return Difficulty.Normal;

            List<string> difficulties = difficulty[Locale];
            int difficultyIndex = difficulties.IndexOf(stringDifficulty);
            if (difficultyIndex == -1)
                throw new ArgumentOutOfRangeException("difficultyIndex");

            return (Difficulty)difficultyIndex;
        }

        public override string PreParse()
        {
            StringBuilder content = new StringBuilder(1024);

            #region Creature_currency_template

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

            #region Creature_health

            content.AppendLine(
 @"DROP TABLE IF EXISTS `creature_health`;
CREATE TABLE `creature_health` (
  `entry` int(10) NOT NULL,
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

            return content.AppendLine().ToString();
        }

        public override string Name { get { return "NPC data parser"; } }

        public override string Address { get { return "npc={0}"; } }

        public override string WelfName { get { return "npc"; } }

        public enum Difficulty : byte
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
    }
}
