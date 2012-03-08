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

            string page = block.Page;

            const string pattern = @"data: \[.*;";

            char[] items = new char[] { '[', ']', '{', '}' };
            string[] subPatterns = new string[] { @"\[(\d+),(\d+)\]", @"\[\[(\d+),(\d+)\]\]" };

            Regex regex = new Regex("template: 'item', id: ('[a-z\\-]+'), name: ", RegexOptions.Multiline);
            {
                MatchCollection matches = regex.Matches(page);
                foreach (Match item in matches)
                {
                    string type = item.Groups[1].Value;

                    if (!type.Equals("\'sells\'"))
                        continue;

                    int start = item.Index;
                    int end = page.IndexOf("});", start);

                    page = page.Substring(start, end - start + 3);
                }
            }
            MatchCollection find = Regex.Matches(page, pattern);

            if (find.Count > 0)
            {
                content.AppendFormat(@"SET @ENTRY := {0};", block.Id).AppendLine();
                content.AppendLine(@"REPLACE INTO `npc_vendor` (`entry`, `item`, `maxcount`, `incrtime`, `ExtendedCost`) VALUES");
            }

            foreach (Match item in find)
            {
                string text = item.Value.Replace("data: ", "").Replace("});", "");
                JArray ser = (JArray)JsonConvert.DeserializeObject(text);

                for (int i = 0; i < ser.Count; ++i)
                {
                    JObject obj = (JObject)ser[i];

                    string cost = string.Empty;
                    string id = obj["id"].ToString();
                    string avail = obj["avail"].ToString();

                    if (!(obj["cost"] is JArray))
                        continue;

                    JArray array = obj["cost"] as JArray;
                    foreach (JToken costBlock in array)
                    {
                        string scost = costBlock.ToString().Trim();
                        scost = scost.Replace("\r\n", "").Replace(" ", "");

                        if (scost.Equals("0"))
                            continue;

                        if (scost.IndexOfAny(items) != -1)
                        {
                            foreach (string subpattern in subPatterns)
                            {
                                MatchCollection matches = Regex.Matches(scost, subpattern);
                                foreach (Match match in matches)
                                {
                                    cost = match.Groups[1].Value;
                                }
                            }
                        }
                        else
                            cost = scost;
                    }

                    avail = avail.Equals("-1") ? "0" : avail;
                    int incrTime = avail.Equals("0") ? 0 : 3600;

                    if (string.IsNullOrWhiteSpace(cost) || cost.Equals("0"))
                        cost = "9999999";

                    content.AppendFormat(@"(@ENTRY, {0}, {1}, {2}, {3}){4}", id, avail, incrTime, cost, (i < ser.Count - 1 ? "," : ";")).AppendLine();
                }
            }
            content.AppendLine();
            return content.ToString();
        }

        public override string Address
        {
            get
            {
                return "wowhead.com/npc=";
            }
        }

        public override string Name
        {
            get
            {
                return "Vendor data parser";
            }
        }
    }
}
