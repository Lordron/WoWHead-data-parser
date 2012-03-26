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
            StringBuilder content = new StringBuilder();

            string page = block.Page.Substring("\'sells\'");

            const string pattern = @"data: \[.*;";

            char[] anyOf = new[] {'[', ']', '{', '}'};
            string[] subPatterns = new[] {@"\[(\d+),(\d+)\]", @"\[\[(\d+),(\d+)\]\]"};

            MatchCollection find = Regex.Matches(page, pattern);

            if (find.Count > 0)
            {
                content.AppendFormat(@"SET @ENTRY := {0};", block.Id).AppendLine();
                content.AppendLine(@"REPLACE INTO `npc_vendor` (`entry`, `item`, `maxcount`, `incrtime`, `ExtendedCost`) VALUES");
            }

            foreach (Match item in find)
            {
                string text = item.Value.Replace("data: ", "").Replace("});", "");
                JArray serialization = (JArray)JsonConvert.DeserializeObject(text);

                for (int i = 0; i < serialization.Count; ++i)
                {
                    JObject jobj = (JObject)serialization[i];

                    string scost = string.Empty;
                    string scount = string.Empty;
                    string id = jobj["id"].ToString();
                    string maxcount = jobj["avail"].ToString();

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
                    int incrTime = maxcount.Equals("0") ? 0 : 3600;

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

                    if (extendedCostEntry == 9999999)
                        content.AppendFormat(@"(@ENTRY, {0}, {1}, {2}, @UNK_COST){3}", id, maxcount, incrTime, (i < serialization.Count - 1 ? "," : ";")).AppendLine();
                    else
                        content.AppendFormat(@"(@ENTRY, {0}, {1}, {2}, {3}){4}", id, maxcount, incrTime, extendedCostEntry, (i < serialization.Count - 1 ? "," : ";")).AppendLine();
                }
            }

            return content.AppendLine().ToString();
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