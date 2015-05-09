
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using WoWHeadParser.Serialization.Structures;

namespace WoWHeadParser.Parser.Parsers
{
    [Parser(ParserType.NpcLocale)]
    internal class NpcLocaleParser : PageParser
    {
        public NpcLocaleParser(Locale locale, int flags)
            : base(locale, flags)
        {
            if (HasLocales)
                Builder.Setup("locales_creature", "entry", false, string.Format("name_{0}", LocalePosfix), string.Format("subname_{0}", LocalePosfix));
            else
                Builder.Setup("creature_template", "entry", false, "name", "subname");
        }

        private const string s_Pattern = @"new Listview\({template: 'npc', id: 'npcs', data: (?<npc>.+)}\)";
        private Regex m_regex = new Regex(s_Pattern);

        public override void Parse(string page, uint id)
        {
            Match match = m_regex.Match(page);
            if (!match.Success)
                return;

            string json = match.Groups["npc"].Value;

            LocaleItem[] items = JsonConvert.DeserializeObject<LocaleItem[]>(json);
            foreach (LocaleItem item in items)
            {
                Builder.SetKey(item.Id);
                Builder.AppendValues(item.Name.HTMLEscapeSumbols(), string.IsNullOrEmpty(item.Tag) ? string.Empty : item.Tag.HTMLEscapeSumbols());
                Builder.Flush();
            }
        }
    }
}