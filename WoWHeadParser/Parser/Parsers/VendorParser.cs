using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Text.RegularExpressions;
using WoWHeadParser.DBFileStorage;
using WoWHeadParser.Serialization.Structures;

namespace WoWHeadParser.Parser.Parsers
{
    [Parser(ParserType.Vendor)]
    internal class VendorParser : PageParser
    {
        public VendorParser(Locale locale, int flags)
            : base(locale, flags)
        {
            m_extendedCostStorage = DBFileLoader.GetLoader<ItemExtendedCost>();
            if (m_extendedCostStorage == null)
                throw new ArgumentNullException("_itemExtendedCost");

            Builder.Setup("npc_vendor", "entry", false, "item", "maxcount", "incrtime", "ExtendedCost");
        }

        private const string s_Pattern = @"new Listview\({template: 'item', id: 'sells', name: .+, data: (?<vendor>.+)}\)";
        private Regex m_regex = new Regex(s_Pattern);

        private ItemExtendedCost m_extendedCostStorage = null;

        public override void Parse(string page, uint id)
        {
            Match match = m_regex.Match(page);
            if (!match.Success)
                return;

            string json = match.Groups["vendor"].Value;

            VendorItem[] items = JsonConvert.DeserializeObject<VendorItem[]>(json);
            foreach (VendorItem item in items)
            {
                int maxCount = item.Available == -1 ? 0 : item.Available;
                int incrTime = maxCount == 0 ? 0 : 3600;

                uint price = 0, count = 0;

                foreach (dynamic array in item.Cost)
                {
                    if (array is JArray)
                    {
                        foreach (dynamic token in array)
                        {
                            JArray data = token as JArray;
                            {
                                price = data[0].Value<uint>();
                                count = data[1].Value<uint>();
                            }
                        }
                    }
                    else
                        price = (uint)array;
                }

                uint extendedCost = 0;
                if (price > 0 && count > 0)
                    extendedCost = m_extendedCostStorage.GetExtendedCost(price, count);
                else if (price == 0)
                    extendedCost = 9999999;

                Builder.SetKey(id);
                Builder.AppendValues(item.Id, maxCount, incrTime, extendedCost);
                Builder.Flush();
            }
        }
    }
}