namespace WoWHeadParser
{
    public class DB2Loader
    {
        public DB2Loader()
        {
            DB2.ExtendedCost = DB2Reader.ReadDb2<ItemExtendedCostEntry>(DB2.ItemExtendedCostEntryStrings);
        }
    }
}