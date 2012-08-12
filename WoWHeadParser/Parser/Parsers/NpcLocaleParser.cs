using System.Text.RegularExpressions;
using Newtonsoft.Json;

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

        private const string pattern = @"new Listview\({template: 'npc', id: 'npcs', data: (?<npc>.+)}\)";
        private Regex localeRegex = new Regex(pattern);

        public override void Parse(string page, uint id)
        {
            Match item = localeRegex.Match(page);
            if (!item.Success)
                return;

            string text = item.Groups["npc"].Value;
            NpcLocaleItem[] localeItems = JsonConvert.DeserializeObject<NpcLocaleItem[]>(text);
            foreach (NpcLocaleItem localeItem in localeItems)
            {
                Builder.SetKey(localeItem.Id);
                Builder.AppendValues(localeItem.Name.HTMLEscapeSumbols(), string.IsNullOrEmpty(localeItem.Tag) ? string.Empty : localeItem.Tag.HTMLEscapeSumbols());
                Builder.Flush();
            }
        }
    }
}