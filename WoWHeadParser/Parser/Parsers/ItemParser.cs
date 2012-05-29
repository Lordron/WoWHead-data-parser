using System;
using System.Collections.Generic;
using Sql;
using WoWHeadParser.Page;

namespace WoWHeadParser.Parser.Parsers
{
    internal class ItemParser : DataParser
    {
        private Dictionary<Locale, string> _durabiliy = new Dictionary<Locale, string>
        {
            {Locale.Old, @"Durability"},
            {Locale.English, @"Durability"},
            {Locale.Russia, @"Прочность"},
            {Locale.Germany, @"Haltbarkeit"},
            {Locale.Spain, @"Durabilidad"},
            {Locale.France, @"Durabilité"},
            {Locale.Portugal, @"Durabilidade"},
        };

        public override PageItem Parse(string page, uint id)
        {
            int startIndex = page.FastIndexOf(_durabiliy[Locale]);
            if (startIndex == -1)
                return new PageItem();

            SqlBuilder builder = new SqlBuilder("item_template");
            builder.SetFieldsNames("Durability");

            int endIndex = page.FastIndexOf("<br />", startIndex);
            string subsString = page.Substring(startIndex, (endIndex - startIndex));
            {
                string[] values = subsString.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < values.Length; ++i)
                {
                    int durability;
                    if (!int.TryParse(values[i].Trim(), out durability))
                        continue;

                    builder.AppendFieldsValue(id, durability);
                    break;
                }
            }

            return new PageItem(id, builder.ToString());
        }

        public override string Name { get { return "Item data parser"; } }

        public override string Address { get { return "item={0}?power"; } }
    }
}