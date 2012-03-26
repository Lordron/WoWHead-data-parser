using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WoWHeadParser
{
    internal class NpcLocaleParser : Parser
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

            string page = block.Page.Substring("\'npcs\'");

            const string pattern = @"data: \[.*;";

            MatchCollection find = Regex.Matches(page, pattern);
            foreach (Match item in find)
            {
                string text = item.Value.Replace("data: ", "").Replace("});", "");
                JArray serialization = (JArray)JsonConvert.DeserializeObject(text);

                if (serialization.Count > 0)
                {
                    if (Locale > Locale.English)
                        content.AppendFormat(@"INSERT IGNORE INTO `locales_creature` (`entry`, `name_{0}`, `subname_{0}`) VALUES", _tables[Locale]).AppendLine();
                }

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

                    if (string.IsNullOrEmpty(name))
                        continue;

                    if (Locale == Locale.English)
                        content.AppendFormat("UPDATE `creature_template` SET `name` = '{0}', `subname` = '{1}' WHERE `entry` = {2};", name, subName,  id).AppendLine();
                    else
                        content.AppendFormat("({0}, '{1}', '{2}'){3}", id, name, subName, (i < serialization.Count - 1 ? "," : ";")).AppendLine();
                }
            }

            return content.AppendLine().ToString();
        }

        public override string BeforParsing()
        {
            return string.Empty;
        }

        public override string Address { get { return "wowhead.com/npcs?filter=cr=37:37;crs=1:4;"; } }

        public override string Name { get { return "Npc locale data parser"; } }

        public override int MaxCount { get { return 59000; } }
    }

    internal class NpcDataParser : Parser
    {
        public override string Parse(Block block)
        {
            StringBuilder content = new StringBuilder();

            string page = block.Page.Substring("\'npcs\'");

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

                    if (string.IsNullOrEmpty(minLevel))
                        minLevel = "`minlevel`";

                    if (string.IsNullOrEmpty(maxLevel))
                        maxLevel = "`maxlevel`";

                    content.AppendFormat("UPDATE `creature_template` SET `minlevel` = {0}, `maxlevel` = {1}, `type` = {2} WHERE `id` = {3};", minLevel, maxLevel, type, id).AppendLine();
                }
            }

            return content.AppendLine().ToString();
        }

        public override string BeforParsing()
        {
            StringBuilder content = new StringBuilder();
            content.AppendLine(@"SET @BOSS_LEVEL := 9999;");
            return content.AppendLine().ToString();
        }

        public override string Address { get { return "wowhead.com/npcs?filter=cr=37:37;crs=1:4;"; } }

        public override string Name { get { return "Npc data (level, type) parser"; } }

        public override int MaxCount { get { return 59000; } }
    }
}
