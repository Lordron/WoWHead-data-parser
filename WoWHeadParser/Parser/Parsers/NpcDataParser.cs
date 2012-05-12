using System;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sql;
using WoWHeadParser.Page;

namespace WoWHeadParser.Parser.Parsers
{
    internal class NpcLocaleParser : DataParser
    {
        public override bool Parse(ref PageItem block)
        {
            string page = block.Page.Substring("\'npcs\'");

            const string pattern = @"data: \[.*;";

            SqlBuilder builder = new SqlBuilder(HasLocales ? "locales_creature" : "creature_template");

            if (HasLocales)
                builder.SetFieldsName(string.Format("name_{0}", Locales[Locale]), string.Format("subname_{0}", Locales[Locale]));
            else
                builder.SetFieldsName("name", "subname");

            MatchCollection find = Regex.Matches(page, pattern);
            for (int i = 0; i < find.Count; ++i)
            {
                Match item = find[i];

                string text = item.Value.Replace("data: ", "").Replace("});", "");
                JArray serialization = (JArray)JsonConvert.DeserializeObject(text);

                for (int j = 0; j < serialization.Count; ++j)
                {
                    JObject jobj = (JObject)serialization[j];

                    JToken nameToken = jobj["name"];
                    JToken subNameToken = jobj["tag"];

                    string id = jobj["id"].ToString();
                    string name = nameToken == null ? string.Empty : nameToken.ToString().HTMLEscapeSumbols();
                    string subName = subNameToken == null ? string.Empty : subNameToken.ToString().HTMLEscapeSumbols();

                    builder.AppendFieldsValue(id, name, subName);
                }
            }
            block.Page = builder.ToString();
            return !builder.Empty;
        }

        public override string Name { get { return "NPC locale data parser"; } }

        public override string Address { get { return "npcs?filter=cr=37:37;crs=1:4;crv={0}:{1}"; } }

        public override int MaxCount { get { return 59000; } }
    }

    internal class NpcDataParser : DataParser
    {
        public override bool Parse(ref PageItem block)
        {
            string page = block.Page.Substring("\'npcs\'");

            const string pattern = @"data: \[.*;";

            SqlBuilder builder = new SqlBuilder("creature_template");
            builder.SetFieldsName("minlevel", "maxlevel", "type");

            MatchCollection find = Regex.Matches(page, pattern);
            for (int i = 0; i < find.Count; ++i)
            {
                Match item = find[i];

                string text = item.Value.Replace("data: ", "").Replace("});", "");
                JArray serialization = (JArray)JsonConvert.DeserializeObject(text);

                for (int j = 0; j < serialization.Count; ++j)
                {
                    JObject jobj = (JObject)serialization[j];

                    JToken maxLevelToken = jobj["maxlevel"];
                    JToken minLevelToken = jobj["minlevel"];

                    string id = jobj["id"].ToString();
                    string type = jobj["type"].ToString();

                    string maxLevel = maxLevelToken == null ? string.Empty : maxLevelToken.ToString();
                    string minLevel = minLevelToken == null ? string.Empty : minLevelToken.ToString();

                    if (minLevel.Equals("9999"))
                        minLevel = "@BOSS_LEVEL";
                    if (maxLevel.Equals("9999"))
                        maxLevel = "@BOSS_LEVEL";

                    builder.AppendFieldsValue(id, minLevel, maxLevel, type);
                }
            }

            block.Page = builder.ToString();
            return !builder.Empty;
        }

        public override string PreParse()
        {
            return @"SET @BOSS_LEVEL := 9999;" + Environment.NewLine;
        }

        public override string Name { get { return "NPC template data parser"; } }

        public override string Address { get { return "npcs?filter=cr=37:37;crs=1:4;crv={0}:{1}"; } }

        public override int MaxCount { get { return 59000; } }
    }
}