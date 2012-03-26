using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WoWHeadParser
{
    internal class QuestLocaleParser : Parser
    {
        private Dictionary<Locale, string> _tables = new Dictionary<Locale, string>
        {
            {Locale.Russia, "Text_loc8"},
            {Locale.Germany, "Text_loc3"},
            {Locale.France, "Text_loc2"},
            {Locale.Spain, "Text_loc6"},
        };

        public override string Parse(Block block)
        {
            StringBuilder content = new StringBuilder();

            string page = block.Page.Substring("\'quests\'");

            const string pattern = @"data: \[.*;";

            MatchCollection find = Regex.Matches(page, pattern);
            foreach (Match item in find)
            {
                string text = item.Value.Replace("data: ", "").Replace("});", "");
                JArray serialiation = (JArray)JsonConvert.DeserializeObject(text);

                if (serialiation.Count > 0)
                {
                    if (Locale > Locale.English)
                        content.AppendFormat(@"INSERT IGNORE INTO `locales_quest` (`entry`, `title_{0}``) VALUES", _tables[Locale]).AppendLine();
                }

                for (int i = 0; i < serialiation.Count; ++i)
                {
                    JObject jobj = (JObject)serialiation[i];

                    string id = jobj["id"].ToString();
                    string name = string.Empty;

                    JToken nameToken = jobj["name"];

                    if (nameToken != null)
                        name = nameToken.ToString().HTMLEscapeSumbols();

                    if (string.IsNullOrEmpty(name))
                        continue;

                    if (Locale == Locale.English)
                        content.AppendFormat("UPDATE `quest_template` SET `title` = '{0}' WHERE `entry` = {1};", name, id).AppendLine();
                    else
                        content.AppendFormat("({0}, '{1}'){2}", id, name, (i < serialiation.Count - 1 ? "," : ";")).AppendLine();
                }
            }

            return content.AppendLine().ToString();
        }

        public override string BeforParsing()
        {
            return string.Empty;
        }

        public override string Address { get { return "wowhead.com/quests?filter=cr=30:30;crs=1:4;"; } }

        public override string Name { get { return "Quest locale data parser"; } }

        public override int MaxCount { get { return 32000; } }
    }

    internal class QuestDataParser : Parser
    {
        public override string Parse(Block block)
        {
            StringBuilder content = new StringBuilder();

            string page = block.Page.Substring("\'quests\'");

            const string pattern = @"data: \[.*;";

            MatchCollection find = Regex.Matches(page, pattern);
            foreach (Match item in find)
            {
                string text = item.Value.Replace("data: ", "").Replace("});", "");
                JArray serialization = (JArray)JsonConvert.DeserializeObject(text);

                for (int i = 0; i < serialization.Count; ++i)
                {
                    JObject jobj = (JObject)serialization[i];

                    string id = jobj["id"].ToString();

                    JToken zoneOrSortToken = jobj["category"];
                    JToken levelToken = jobj["level"];
                    JToken minLevelToken = jobj["reqlevel"];
                    JToken moneyToken = jobj["money"];

                    string level = string.Empty;
                    string minLevel = string.Empty;
                    string zoneOrSort = string.Empty;
                    string money = string.Empty;

                    if (zoneOrSortToken != null)
                        zoneOrSort = zoneOrSortToken.ToString();

                    if (levelToken != null)
                        level = levelToken.ToString();

                    if (minLevelToken != null)
                        minLevel = minLevelToken.ToString();

                    if (moneyToken != null)
                        money = moneyToken.ToString();

                    if (string.IsNullOrEmpty(level))
                        level = "`level`";

                    if (string.IsNullOrEmpty(minLevel))
                        minLevel = "`minlevel`";

                    if (string.IsNullOrEmpty(zoneOrSort))
                        zoneOrSort = "`zoneOrSort`";

                    if (string.IsNullOrEmpty(money))
                        money = "`RewardOrRequiredMoney`";

                    content.AppendFormat("UPDATE `quest_template` SET `level` = {0}, `minlevel` = {1}, `zoneOrSort` = {2}, `RewardOrRequiredMoney` = {3} WHERE `id` = {4};", level, minLevel, zoneOrSort, money, id).AppendLine();
                }
            }

            return content.AppendLine().ToString();
        }

        public override string BeforParsing()
        {
            return string.Empty;
        }

        public override string Address { get { return "wowhead.com/quests?filter=cr=30:30;crs=1:4;"; } }

        public override string Name { get { return "Quest data (level, type and etc.) parser"; } }

        public override int MaxCount { get { return 32000; } }
    }
}
