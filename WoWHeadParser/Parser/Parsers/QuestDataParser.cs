using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sql;
using WoWHeadParser.Page;

namespace WoWHeadParser.Parser.Parsers
{
    [Parser(ParserType.QuestLocale)]
    internal class QuestLocaleParser : PageParser
    {
        public QuestLocaleParser(Locale locale, int flags)
            : base(locale, flags)
        {
            this.Address = "quests?filter=cr=30:30;crs=1:4;crv={0}:{1}";
            this.MaxCount = 32000;
        }

        private const string pattern = @"data: \[.*;";
        private Regex localeRegex = new Regex(pattern);

        public override PageItem Parse(string page, uint id)
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

            page = page.Substring("\'quests\'");

            MatchCollection find = localeRegex.Matches(page);
            for (int i = 0; i < find.Count; ++i)
            {
                Match item = find[i];
                string text = item.Value.Replace("data: ", "").Replace("});", "");
                JArray serialiation = (JArray)JsonConvert.DeserializeObject(text);

                for (int j = 0; j < serialiation.Count; ++j)
                {
                    JObject jobj = (JObject)serialiation[j];
                    JToken nameToken = jobj["name"];

                    string entry = jobj["id"].ToString();
                    string name = nameToken == null ? string.Empty : nameToken.ToString().HTMLEscapeSumbols();

                    builder.AppendFieldsValue(entry, name);
                }
            }

            return new PageItem(id, builder.ToString());
        }
    }

    [Parser(ParserType.QuestData)]
    internal class QuestDataParser : PageParser
    {
        public QuestDataParser(Locale locale, int flags)
            : base(locale, flags)
        {
            this.Address = "quests?filter=cr=30:30;crs=1:4;crv={0}:{1}";
            this.MaxCount = 32000;
        }

        private const string pattern = @"data: \[.*;";
        private Regex dataRegex = new Regex(pattern);

        public override PageItem Parse(string page, uint id)
        {
            SqlBuilder builder = new SqlBuilder("quest_template", "id");
            builder.SetFieldsNames("level", "minlevel", "zoneOrSort", "RewardOrRequiredMoney");

            page = page.Substring("\'quests\'");

            MatchCollection find = dataRegex.Matches(page);
            for (int i = 0; i < find.Count; ++i)
            {
                Match item = find[i];
                string text = item.Value.Replace("data: ", "").Replace("});", "");
                JArray serialization = (JArray)JsonConvert.DeserializeObject(text);

                for (int j = 0; j < serialization.Count; ++j)
                {
                    JObject jobj = (JObject)serialization[j];

                    string entry = jobj["id"].ToString();

                    JToken zoneOrSortToken = jobj["category"];
                    JToken levelToken = jobj["level"];
                    JToken minLevelToken = jobj["reqlevel"];
                    JToken moneyToken = jobj["money"];

                    string level = levelToken == null ? string.Empty : levelToken.ToString();
                    string minLevel = minLevelToken == null ? string.Empty : minLevelToken.ToString();
                    string zoneOrSort = zoneOrSortToken == null ? string.Empty : zoneOrSortToken.ToString();
                    string money = moneyToken == null ? string.Empty : moneyToken.ToString();

                    builder.AppendFieldsValue(entry, level, minLevel, zoneOrSort, money);
                }
            }

            return new PageItem(id, builder.ToString());
        }
    }
}