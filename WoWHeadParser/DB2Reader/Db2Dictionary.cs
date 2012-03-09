using System.Collections.Generic;

namespace WoWHeadParser
{
    public static class DB2
    {
        public static Dictionary<uint, ItemExtendedCostEntry> ExtendedCost = new Dictionary<uint, ItemExtendedCostEntry>();
        public static Dictionary<uint, string> ItemExtendedCostEntryStrings = new Dictionary<uint, string>();
    }
}