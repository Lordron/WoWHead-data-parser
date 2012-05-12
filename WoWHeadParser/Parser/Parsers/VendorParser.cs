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

        public override bool Parse(ref PageItem block)
        {
            string page = block.Page.Substring("\'sells\'");

            const string pattern = @"data: \[.*;";
            char[] anyOf = new[] { '[', ']', '{', '}' };

            uint blockId = block.Id;

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

                        if (costBlock.IndexOfAny(anyOf) != -1)
                        {
                            MatchCollection matches = Regex.Matches(costBlock, @"\[(\d+),(\d+)\]");
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
                    string incrTime = maxcount.Equals("0") ? "0" : "3600";

                    uint extendedCost = 0;
                    if (hasExtendedCost && cost > 0 && count > 0)
                        extendedCost = _itemExtendedCost.GetExtendedCost(cost, count);
                    else if (!hasExtendedCost)
                        extendedCost = 9999999;

                    builder.AppendFieldsValue(blockId, id, maxcount, incrTime, (extendedCost != 9999999) ? extendedCost.ToString() : "@UNK_COST");
                }
            }

            block.Page = builder.ToString();
            return !builder.Empty;
        }

        public override string PreParse()
        {
            return @"SET @UNK_COST := 9999999;" + Environment.NewLine;
        }

        public override string Name { get { return "Vendor data parser"; } }

        public override string Address { get { return "npc={0}"; } }
    }
}