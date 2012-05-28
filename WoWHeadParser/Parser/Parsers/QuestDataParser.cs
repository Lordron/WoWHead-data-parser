using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sql;
using WoWHeadParser.Page;

namespace WoWHeadParser.Parser.Parsers
{
    internal class QuestLocaleParser : DataParser
    {
        private const string pattern = @"data: \[.*;";

        public override bool Parse(ref PageItem block)
        {
            SqlBuilder builder;
            if (HasLocales)
            {
                builder = new SqlBuilder("locales_quest");
                builder.SetFieldsName("title_{0}", LocalePosfix);
            }
            else
            {
                builder = new SqlBuilder("quest_template", "id");
                builder.SetFieldsNames("title");
            }

            string page = block.Page.Substring("\'quests\'");

            MatchCollection find = Regex.Matches(page, pattern);
            for (int i = 0; i < find.Count; ++i)
            {
                Match item = find[i];
                string text = item.Value.Replace("data: ", "").Replace("});", "");
                JArray serialiation = (JArray)JsonConvert.DeserializeObject(text);

                for (int j = 0; j < serialiation.Count; ++j)
                {
                    JObject jobj = (JObject)serialiation[j];
                    JToken nameToken = jobj["name"];

                    string id = jobj["id"].ToString();
                    string name = nameToken == null ? string.Empty : nameToken.ToString().HTMLEscapeSumbols();

                    builder.AppendFieldsValue(id, name);
                }
            }

            block.Page = builder.ToString();
            return !builder.Empty;
        }

        public override string Name { get { return "Quest locale data parser"; } }

        public override string Address { get { return "quests?filter=cr=30:30;crs=1:4;crv={0}:{1}"; } }

        public override int MaxCount { get { return 32000; } }
    }

    internal class QuestDataParser : DataParser
    {
        private const string pattern = @"data: \[.*;";

        public override bool Parse(ref PageItem block)
        {
            SqlBuilder builder = new SqlBuilder("quest_template", "id");
            builder.SetFieldsNames("level", "minlevel", "zoneOrSort", "RewardOrRequiredMoney");

            string page = block.Page.Substring("\'quests\'");

            MatchCollection find = Regex.Matches(page, pattern);
            for (int i = 0; i < find.Count; ++i)
            {
                Match item = find[i];
                string text = item.Value.Replace("data: ", "").Replace("});", "");
                JArray serialization = (JArray)JsonConvert.DeserializeObject(text);

                for (int j = 0; j < serialization.Count; ++j)
                {
                    JObject jobj = (JObject)serialization[j];

                    string id = jobj["id"].ToString();

                    JToken zoneOrSortToken = jobj["category"];
                    JToken levelToken = jobj["level"];
                    JToken minLevelToken = jobj["reqlevel"];
                    JToken moneyToken = jobj["money"];

                    string level = levelToken == null ? string.Empty : levelToken.ToString();
                    string minLevel = minLevelToken == null ? string.Empty : minLevelToken.ToString();
                    string zoneOrSort = zoneOrSortToken == null ? string.Empty : zoneOrSortToken.ToString();
                    string money = moneyToken == null ? string.Empty : moneyToken.ToString();

                    builder.AppendFieldsValue(id, level, minLevel, zoneOrSort, money);
                }
            }

            block.Page = builder.ToString();
            return !builder.Empty;
        }

        public override string Name { get { return "Quest template data parser"; } }

        public override string Address { get { return "quests?filter=cr=30:30;crs=1:4;crv={0}:{1}"; } }

        public override int MaxCount { get { return 32000; } }
    }
}