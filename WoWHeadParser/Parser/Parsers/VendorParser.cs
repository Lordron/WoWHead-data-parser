using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WoWHeadParser
{
    internal class VendorParser : Parser
    {
        public override string Parse(Block block)
        {
            string page = block.Page.Substring("\'sells\'");

            const string pattern = @"data: \[.*;";

            char[] anyOf = new[] {'[', ']', '{', '}'};
            string[] subPatterns = new[] {@"\[(\d+),(\d+)\]", @"\[\[(\d+),(\d+)\]\]"};

            SqlBuilder.Initial("npc_vendor");
            SqlBuilder.SetFieldsName("item", "maxcount", "incrtime", "ExtendedCost");

            MatchCollection find = Regex.Matches(page, pattern);

            foreach (Match item in find)
            {
                string text = item.Value.Replace("data: ", "").Replace("});", "");
                JArray serialization = (JArray)JsonConvert.DeserializeObject(text);

                for (int i = 0; i < serialization.Count; ++i)
                {
                    JObject jobj = (JObject)serialization[i];

                    string id = jobj["id"].ToString();

                    string scost = string.Empty;
                    string scount = string.Empty;
                    string maxcount = string.Empty;

                    JToken maxcountToken = jobj["avail"];
                    if (maxcountToken != null)
                        maxcount = maxcountToken.ToString();

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
                            extendedCostEntry = DB2Reader.GetExtendedCost(cost, count);
                        }
                    }
                    else
                        extendedCostEntry = 9999999;

                    SqlBuilder.AppendFieldsValue(block.Id, id, maxcount, incrTime, (extendedCostEntry != 9999999) ? extendedCostEntry.ToString() : "@UNK_COST");
                }
            }

            return SqlBuilder.ToString();
        }

        public override string BeforParsing()
        {
            StringBuilder content = new StringBuilder();

            content.AppendLine("-- Uncomment");
            content.AppendLine("-- DELETE FROM `npc_vendor`; -- Delete all data");
            content.AppendLine();
            content.AppendLine(@"SET @UNK_COST := 9999999;");

            return content.AppendLine().ToString();
        }

        public override string Address { get { return "wowhead.com/npc="; } }

        public override string Name { get { return "Vendor data parser"; } }

        public override int MaxCount{ get { return 0; } }
    }
}