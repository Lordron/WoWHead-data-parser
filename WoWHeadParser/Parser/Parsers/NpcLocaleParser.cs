using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WoWHeadParser.Parser.Parsers
{
    [Parser(ParserType.NpcLocale)]
    internal class NpcLocaleParser : PageParser
    {
        public NpcLocaleParser(Locale locale, int flags)
            : base(locale, flags)
        {
            this.Address = "npcs?filter=cr=37:37;crs=1:4;crv={0}:{1}";
            this.MaxCount = 59000;

            if (HasLocales)
                Builder.Setup("locales_creature", "entry", false, string.Format("name_{0}", LocalePosfix), string.Format("subname_{0}", LocalePosfix));
            else
                Builder.Setup("creature_template", "entry", false, "name", "subname");
        }

        private const string pattern = @"data: \[.*;";
        private Regex localeRegex = new Regex(pattern);

        public override void Parse(string page, uint id)
        {
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

                    Builder.SetKey(entry);
                    Builder.AppendValues(name, subName);
                    Builder.Flush();
                }
            }
        }
    }
}