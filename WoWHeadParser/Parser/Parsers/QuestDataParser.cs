using System.Collections.Generic;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WoWHeadParser
{
    internal class QuestLocaleParser : Parser
    {
        private Dictionary<Locale, string> _tables = new Dictionary<Locale, string>
        {
            {Locale.Russia, "loc8"},
            {Locale.Germany, "loc3"},
            {Locale.France, "loc2"},
            {Locale.Spain, "loc6"},
        };

        public override string Parse(Block block)
        {
            string page = block.Page.Substring("\'quests\'");

            const string pattern = @"data: \[.*;";

            if (Locale > Locale.English)
            {
                SqlBuilder.Initial("locales_quest");
                SqlBuilder.SetFieldsName(string.Format("title_{0}", _tables[Locale]));
            }
            else
            {
                SqlBuilder.Initial("quest_template", "id");
                SqlBuilder.SetFieldsName("title");
            }

            MatchCollection find = Regex.Matches(page, pattern);
            foreach (Match item in find)
            {
                string text = item.Value.Replace("data: ", "").Replace("});", "");
                JArray serialiation = (JArray)JsonConvert.DeserializeObject(text);

                for (int i = 0; i < serialiation.Count; ++i)
                {
                    JObject jobj = (JObject)serialiation[i];

                    string id = jobj["id"].ToString();
                    string name = string.Empty;

                    JToken nameToken = jobj["name"];

                    if (nameToken != null)
                        name = nameToken.ToString().HTMLEscapeSumbols();

                    SqlBuilder.AppendFieldsValue(id, name);
                }
            }

            return SqlBuilder.ToString();
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
            string page = block.Page.Substring("\'quests\'");

            const string pattern = @"data: \[.*;";

            SqlBuilder.Initial("quest_template", "id");
            SqlBuilder.SetFieldsName("level", "minlevel", "zoneOrSort", "RewardOrRequiredMoney");

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

                    SqlBuilder.AppendFieldsValue(id, level, minLevel, zoneOrSort, money);
                }
            }

            return SqlBuilder.ToString();
        }

        public override string BeforParsing()
        {
            return string.Empty;
        }

        public override string Address { get { return "wowhead.com/quests?filter=cr=30:30;crs=1:4;"; } }

        public override string Name { get { return "Quest template data parser"; } }

        public override int MaxCount { get { return 32000; } }
    }
}
