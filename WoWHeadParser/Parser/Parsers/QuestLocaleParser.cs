using System.Text.RegularExpressions;
using Newtonsoft.Json;

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

        private const string pattern = @"new Listview\({template: 'quest', id: 'quests', data: (?<quest>.+)}\)";
        private Regex localeRegex = new Regex(pattern);

        public override void Parse(string page, uint id)
        {
            Match item = localeRegex.Match(page);
            if (!item.Success)
                return;

            string text = item.Groups["quest"].Value;
            QuestLocaleItem[] questLocaleItems = JsonConvert.DeserializeObject<QuestLocaleItem[]>(text);

            foreach (QuestLocaleItem localeItem in questLocaleItems)
            {
                Builder.SetKey(localeItem.Id);
                Builder.AppendValue(localeItem.Name.HTMLEscapeSumbols());
                Builder.Flush();
            }
        }
    }
}
