using System.Collections.Generic;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WoWHeadParser
{
    internal class NpcLocaleParser : Parser
    {
        private Dictionary<Locale, string> _tables = new Dictionary<Locale, string>
        {
            {Locale.Russia, "loc8"},
            {Locale.Germany, "loc3"},
            {Locale.France, "loc2"},
            {Locale.Spain, "loc6"},
        };

        public override string Parse(PageItem block)
        {
            string page = block.Page.Substring("\'npcs\'");

            const string pattern = @"data: \[.*;";

            SqlBuilder builder = new SqlBuilder(Locale > Locale.English ? "locales_creature" : "creature_template");

            if (Locale > Locale.English)
                builder.SetFieldsName(string.Format("name_{0}", _tables[Locale]), string.Format("subname_{0}", _tables[Locale]));
            else
                builder.SetFieldsName("name", "subname");

            MatchCollection find = Regex.Matches(page, pattern);
            foreach (Match item in find)
            {
                string text = item.Value.Replace("data: ", "").Replace("});", "");
                JArray serialization = (JArray)JsonConvert.DeserializeObject(text);

                for (int i = 0; i < serialization.Count; ++i)
                {
                    JObject jobj = (JObject)serialization[i];

                    string id = jobj["id"].ToString();
                    string name = string.Empty;
                    string subName = string.Empty;

                    JToken nameToken = jobj["name"];
                    JToken subNameToken = jobj["tag"];

                    if (nameToken != null)
                        name = nameToken.ToString().HTMLEscapeSumbols();

                    if (subNameToken != null)
                        subName = subNameToken.ToString().HTMLEscapeSumbols();

                    builder.AppendFieldsValue(id, name, subName);
                }
            }

            return builder.ToString();
        }

        public override string BeforParsing()
        {
            return string.Empty;
        }

        public override string Address { get { return "wowhead.com/npcs?filter=cr=37:37;crs=1:4;"; } }

        public override string Name { get { return "NPC locale data parser"; } }

        public override int MaxCount { get { return 59000; } }
    }

    internal class NpcDataParser : Parser
    {
        public override string Parse(PageItem block)
        {
            string page = block.Page.Substring("\'npcs\'");

            const string pattern = @"data: \[.*;";

            SqlBuilder builder = new SqlBuilder("creature_template");
            builder.SetFieldsName("minlevel", "maxlevel", "type");

            MatchCollection find = Regex.Matches(page, pattern);
            foreach (Match item in find)
            {
                string text = item.Value.Replace("data: ", "").Replace("});", "");
                JArray serialization = (JArray)JsonConvert.DeserializeObject(text);

                for (int i = 0; i < serialization.Count; ++i)
                {
                    JObject jobj = (JObject)serialization[i];

                    string id = jobj["id"].ToString();
                    string maxLevel = string.Empty;
                    string minLevel = string.Empty;
                    string type = jobj["type"].ToString();

                    JToken maxLevelToken = jobj["maxlevel"];
                    JToken minLevelToken = jobj["minlevel"];

                    if (maxLevelToken != null)
                        maxLevel = maxLevelToken.ToString();

                    if (minLevelToken != null)
                        minLevel = minLevelToken.ToString();

                    if (minLevel.Equals("9999"))
                        minLevel = "@BOSS_LEVEL";
                    if (maxLevel.Equals("9999"))
                        maxLevel = "@BOSS_LEVEL";

                    builder.AppendFieldsValue(id, minLevel, maxLevel, type);
                }
            }

            return builder.ToString();
        }

        public override string BeforParsing()
        {
            return @"SET @BOSS_LEVEL := 9999;";
        }

        public override string Address { get { return "wowhead.com/npcs?filter=cr=37:37;crs=1:4;"; } }

        public override string Name { get { return "NPC template data parser"; } }

        public override int MaxCount { get { return 59000; } }
    }
}
