
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using WoWHeadParser.Serialization.Structures;

namespace WoWHeadParser.Parser.Parsers
{
    [Parser(ParserType.QuestLocale)]
    internal class QuestLocaleParser : PageParser
    {
        public QuestLocaleParser(Locale locale, int flags)
            : base(locale, flags)
        {
            if (HasLocales)
                Builder.Setup("locales_quest", "entry", false, string.Format("title_{0}", LocalePosfix));
            else
                Builder.Setup("quest_template", "id", false, "title");
        }

        private const string s_Pattern = @"new Listview\({template: 'quest', id: 'quests', data: (?<quest>.+)}\)";
        private Regex m_regex = new Regex(s_Pattern);

        public override void Parse(string page, uint id)
        {
            Match match = m_regex.Match(page);
            if (!match.Success)
                return;

            string json = match.Groups["quest"].Value;

            LocaleItem[] items = JsonConvert.DeserializeObject<LocaleItem[]>(json);
            foreach (LocaleItem item in items)
            {
                Builder.SetKey(item.Id);
                Builder.AppendValue(item.Name.HTMLEscapeSumbols());
                Builder.Flush();
            }
        }
    }
}
