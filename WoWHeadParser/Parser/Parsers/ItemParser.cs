using System;
using System.Collections.Generic;

namespace WoWHeadParser.Parser.Parsers
{
    [Parser(ParserType.Item)]
    internal class ItemParser : PageParser
    {
        public ItemParser(Locale locale, int flags)
            : base(locale, flags)
        {
            Builder.Setup("item_template", "entry", false, "Durability");
        }

        private Dictionary<Locale, string> _durabiliy = new Dictionary<Locale, string>
        {
            {Locale.English, @"Durability"},
            {Locale.Russia, @"Прочность"},
            {Locale.Germany, @"Haltbarkeit"},
            {Locale.Spain, @"Durabilidad"},
            {Locale.France, @"Durabilité"},
        };

        public override void Parse(string page, uint id)
        {
            int startIndex = page.FastIndexOf(_durabiliy[Locale]);
            if (startIndex == -1)
                return;

            int endIndex = page.FastIndexOf("<br />", startIndex);
            if (endIndex == -1)
                return;

            string subsString = page.Substring(startIndex, endIndex - startIndex);
            {
                string[] values = subsString.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < values.Length; ++i)
                {
                    int durability;
                    if (!int.TryParse(values[i].Trim(), out durability))
                        continue;

                    Builder.SetKey(id);
                    Builder.AppendValue(durability);
                    Builder.Flush();
                    break;
                }
            }
        }
    }
}