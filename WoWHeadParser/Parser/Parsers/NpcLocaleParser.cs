using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sql;
using WoWHeadParser.Page;
using WoWHeadParser.Properties;

namespace WoWHeadParser.Parser.Parsers
{
    internal class NpcLocaleParser : PageParser
    {
        private const string pattern = @"data: \[.*;";
        private Regex localeRegex = new Regex(pattern);

        public override PageItem Parse(string page, uint id)
        {
            SqlBuilder builder = new SqlBuilder(HasLocales ? "locales_creature" : "creature_template");

            if (HasLocales) 
                builder.SetFieldsNames(string.Format("name_{0}", LocalePosfix), string.Format("subname_{0}", LocalePosfix));
            else
                builder.SetFieldsNames("name", "subname");

            page = page.Substring("\'npcs\'");

            MatchCollection find = localeRegex.Matches(page);
            for (int i = 0; i < find.Count; ++i)
            {
                Match item = find[i];

                string text = item.Value.Replace("data: ", string.Empty).Replace("});", string.Empty);
                JArray serialization = (JArray)JsonConvert.DeserializeObject(text);

                for (int j = 0; j < serialization.Count; ++j)
                {
                    JObject jobj = (JObject)serialization[j];

                    JToken nameToken = jobj["name"];
                    JToken subNameToken = jobj["tag"];

                    string entry = jobj["id"].ToString();
                    string name = nameToken == null ? string.Empty : nameToken.ToString().HTMLEscapeSumbols();
                    string subName = subNameToken == null ? string.Empty : subNameToken.ToString().HTMLEscapeSumbols();

                    builder.AppendFieldsValue(entry, name, subName);
                }
            }

            return new PageItem(id, builder.ToString());
        }

        public override string Name { get { return Resources.NpcLocaleParser; } }

        public override string Address { get { return "npcs?filter=cr=37:37;crs=1:4;crv={0}:{1}"; } }

        public override int MaxCount { get { return 59000; } }
    }
}