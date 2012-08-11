using System;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WoWHeadParser.DBFileStorage;

namespace WoWHeadParser.Parser.Parsers
{
    [Parser(ParserType.Vendor)]
    internal class VendorParser : PageParser
    {
        public VendorParser(Locale locale, int flags)
            : base(locale, flags)
        {
            this.Address = "npc={0}";

            _itemExtendedCost = DBFileLoader.GetLoader<ItemExtendedCost>();
            if (_itemExtendedCost == null)
                throw new ArgumentNullException("_itemExtendedCost");

            Builder.Setup("npc_vendor", "entry", false, "item", "maxcount", "incrtime", "ExtendedCost");
        }

        private const string pattern = @"new Listview\({template: 'item', id: 'sells', name: .+, data: (?<vendor>.+)}\)";
        private Regex templateRegex = new Regex(pattern);

        private ItemExtendedCost _itemExtendedCost = null;

        public override void Parse(string page, uint id)
        {
            Match item = templateRegex.Match(page);
            if (!item.Success)
                return;

            string text = item.Groups["vendor"].Value;
            VendorItem[] vendorItems = JsonConvert.DeserializeObject<VendorItem[]>(text);
            foreach (VendorItem vendorItem in vendorItems)
            {
                int maxCount = vendorItem.Avail == -1 ? 0 : vendorItem.Avail;
                int incrTime = maxCount == 0 ? 0 : 3600;

                uint price = 0, count = 0;

                dynamic[] costArray = vendorItem.Cost;
                foreach (dynamic cost in costArray)
                {
                    if (cost is JArray)
                    {
                        foreach (dynamic token in cost)
                        {
                            JArray extendedCosts = token as JArray;
                            {
                                price = uint.Parse(extendedCosts[0].ToString());
                                count = uint.Parse(extendedCosts[1].ToString());
                            }
                        }
                    }
                    else
                        price = (uint)cost;
                }

                uint extendedCost = 0;
                if (price > 0 && count > 0)
                    extendedCost = _itemExtendedCost.GetExtendedCost(price, count);
                else if (price == 0)
                    extendedCost = 9999999;

                Builder.SetKey(id);
                Builder.AppendValues(vendorItem.Id, maxCount, incrTime, extendedCost);
                Builder.Flush();
            }
        }
    }
}