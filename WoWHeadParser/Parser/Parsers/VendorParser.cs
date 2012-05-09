using System;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sql;
using WoWHeadParser.DBFileStorage;
using WoWHeadParser.Page;

namespace WoWHeadParser.Parser.Parsers
{
    internal class VendorParser : DataParser
    {
        private ItemExtendedCost _itemExtendedCost = null;

        public override void Prepare()
        {
            _itemExtendedCost = DBFileLoader.GetLoader<ItemExtendedCost>();
        }

        public override string Parse(PageItem block)
        {
            string page = block.Page.Substring("\'sells\'");

            const string pattern = @"data: \[.*;";

            char[] anyOf = new[] {'[', ']', '{', '}'};
            string[] subPatterns = new[] {@"\[(\d+),(\d+)\]", @"\[\[(\d+),(\d+)\]\]"};

            SqlBuilder builder = new SqlBuilder("npc_vendor");
            builder.SetFieldsName("item", "maxcount", "incrtime", "ExtendedCost");

            MatchCollection find = Regex.Matches(page, pattern);

            for (int i = 0; i < find.Count; ++i)
            {
                Match item = find[i];

                string text = item.Value.Replace("data: ", "").Replace("});", "");
                JArray serialization = (JArray)JsonConvert.DeserializeObject(text);

                for (int j = 0; j < serialization.Count; ++j)
                {
                    JObject jobj = (JObject)serialization[j];
                    JToken maxcountToken = jobj["avail"];

                    string id = jobj["id"].ToString();

                    string scost = string.Empty;
                    string scount = string.Empty;
                    string maxcount = maxcountToken == null ? string.Empty : maxcountToken.ToString();

                    uint extendedCostEntry = 0;

                    object obj = jobj["cost"];
                    if (!(obj is JArray))
                        continue;

                    JArray array = obj as JArray;
                    foreach (JToken token in array)
                    {
                        string costBlock = token.ToString().Replace("\r\n", "").Replace(" ", "");

                        if (costBlock.Equals("0"))
                            continue;

                        if (costBlock.IndexOfAny(anyOf) != -1)
                        {
                            foreach (string subpattern in subPatterns)
                            {
                                MatchCollection matches = Regex.Matches(costBlock, subpattern);
                                foreach (Match match in matches)
                                {
                                    scost = match.Groups[1].Value;
                                    scount = match.Groups[2].Value;
                                }
                            }
                        }
                        else
                            scost = costBlock;
                    }

                    maxcount = maxcount.Equals("-1") ? "0" : maxcount;
                    string incrTime = maxcount.Equals("0") ? "0" : "3600";

                    if (!string.IsNullOrWhiteSpace(scost) && !scost.Equals("0"))
                    {
                        if (!string.IsNullOrEmpty(scount))
                        {
                            uint cost = uint.Parse(scost);
                            uint count = uint.Parse(scount);
                            extendedCostEntry = _itemExtendedCost.GetExtendedCost(cost, count);
                        }
                    }
                    else
                        extendedCostEntry = 9999999;

                    builder.AppendFieldsValue(block.Id, id, maxcount, incrTime, (extendedCostEntry != 9999999) ? extendedCostEntry.ToString() : "@UNK_COST");
                }
            }

            return builder.ToString();
        }

        public override string PreParse()
        {
            return @"SET @UNK_COST := 9999999;" + Environment.NewLine;
        }

        public override string Name { get { return "Vendor data parser"; } }

        public override string Address { get { return "npc={0}"; } }
    }
}