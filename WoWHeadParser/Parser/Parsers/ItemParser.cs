using System;
using Sql;
using WoWHeadParser.Page;

namespace WoWHeadParser.Parser.Parsers
{
    internal class ItemParser : DataParser
    {
        public override bool Parse(ref PageItem block)
        {
            string page = block.Page;
            int startIndex = page.FastIndexOf("Durability");
            if (startIndex == -1)
                return false;

            SqlBuilder builder = new SqlBuilder("item_template");
            builder.SetFieldsName("Durability");

            int endIndex = page.FastIndexOf("<br />", startIndex);
            string subsString = page.Substring(startIndex, (endIndex - startIndex));
            {
                string[] values = subsString.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < values.Length; ++i)
                {
                    int durability;
                    if (!int.TryParse(values[i].Trim(), out durability))
                        continue;

                    builder.AppendFieldsValue(block.Id, durability);
                    break;
                }
            }

            block.Page = builder.ToString();
            return !builder.Empty;
        }

        public override string Name { get { return "Item data parser"; } }

        public override string Address { get { return "item={0}?power"; } }
    }
}