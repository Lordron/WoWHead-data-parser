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

        private const string pattern = @"data: \[.*;";
        private const string costPattern = @"\[(\d+),(\d+)\]";
        private Regex costRegex = new Regex(costPattern);

        private ItemExtendedCost _itemExtendedCost = null;

        private char[] _anyOf = new[] { '[', ']', '{', '}' };

        public override void Parse(string page, uint id)
        {
            MatchCollection find = Regex.Matches(page, pattern);
            for (int i = 0; i < find.Count; ++i)
            {
                Match item = find[i];
                string text = item.Value.Replace("data: ", "").Replace("});", "");
                JArray serialization = JsonConvert.DeserializeObject<JArray>(text);

                for (int j = 0; j < serialization.Count; ++j)
                {
                    JObject jobj = (JObject)serialization[j];
                    JToken maxcountToken = jobj["avail"];

                    string entry = jobj["id"].ToString();
                    string maxcount = maxcountToken == null ? string.Empty : maxcountToken.ToString();

                    object obj = jobj["cost"];
                    if (!(obj is JArray))
                        continue;

                    uint cost = 0;
                    uint count = 0;
                    bool hasExtendedCost = false;

                    JArray array = obj as JArray;
                    foreach (JToken token in array)
                    {
                        string costBlock = token.ToString().Replace("\r\n", "").Replace(" ", "");

                        if (costBlock.Equals("0"))
                            continue;

                        if (costBlock.IndexOfAny(_anyOf) != -1)
                        {
                            MatchCollection matches = costRegex.Matches(costBlock);
                            foreach (Match match in matches)
                            {
                                cost = uint.Parse(match.Groups[1].Value);
                                count = uint.Parse(match.Groups[2].Value);
                                hasExtendedCost = true;
                            }
                        }
                        else
                            hasExtendedCost = true;
                    }

                    maxcount = maxcount.Equals("-1") ? "0" : maxcount;
                    int incrTime = maxcount.Equals("0") ? 0 : 3600;

                    uint extendedCost = 0;
                    if (hasExtendedCost && cost > 0 && count > 0)
                        extendedCost = _itemExtendedCost.GetExtendedCost(cost, count);
                    else if (!hasExtendedCost)
                        extendedCost = 9999999;

                    Builder.SetKey(id);
                    Builder.AppendValues(entry, maxcount, incrTime, extendedCost);
                    Builder.Flush();
                }
            }
        }
    }
}