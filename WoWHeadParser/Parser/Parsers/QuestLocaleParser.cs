using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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

            if (HasLocales)
                Builder.Setup("locales_quest", "entry", false, string.Format("title_{0}", LocalePosfix));
            else
                Builder.Setup("quest_template", "id", false, "title");
        }

        private const string pattern = @"data: \[.*;";
        private Regex localeRegex = new Regex(pattern);

        public override void Parse(string page, uint id)
        {
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

                    Builder.SetKey(entry);
                    Builder.AppendValue(name);
                    Builder.Flush();
                }
            }
        }
    }
}
