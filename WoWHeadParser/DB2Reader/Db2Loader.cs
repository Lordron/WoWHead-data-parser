namespace WoWHeadParser.DB2Reader
{
    public class DB2Loader
    {
        public DB2Loader()
        {
            DB2.ExtendedCost = DBFileReader.ReadDb2<ItemExtendedCostEntry>(DB2.ItemExtendedCostEntryStrings);
        }
    }
}