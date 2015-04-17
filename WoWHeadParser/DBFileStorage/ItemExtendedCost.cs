using System.Runtime.InteropServices;
using DBFilesClient.NET;

namespace WoWHeadParser.DBFileStorage
{
    [StructLayout(LayoutKind.Sequential)]
    public class ItemExtendedCostEntry
    {
        public uint Id;

        public uint ReqArenaSlot;

        [StoragePresence(StoragePresenceOption.Include, ArraySize = 5)]
        public uint[] ReqItems;

        [StoragePresence(StoragePresenceOption.Include, ArraySize = 5)]
        public uint[] ReqItemCounts;

        public uint ReqPersonalRating;

        public uint ItemPurchaseGroup;

        [StoragePresence(StoragePresenceOption.Include, ArraySize = 5)]
        public uint[] ReqCurrences;

        [StoragePresence(StoragePresenceOption.Include, ArraySize = 5)]
        public uint[] ReqCurrencyCounts;

        public uint RequiredFactionId;

        public uint RequiredFactionStanding;

        public uint RequirementFlags;

        public uint RequiredAchievement;

        public uint RequiredMoney;
    };

    public class ItemExtendedCost : IDBFileLoader
    {
        private DBCStorage<ItemExtendedCostEntry> _storage;

        #region IDBFileLoader

        public void Load()
        {
            _storage = DBFileLoader.Load<ItemExtendedCostEntry>(false);
        }

        #endregion

        public int Count { get { return _storage == null ? -1 : _storage.Count; } } 

        public const uint MaxSize = 5;

        public uint GetExtendedCost(uint cost, uint count)
        {
            foreach (ItemExtendedCostEntry entry in _storage)
            {
                for (int i = 0; i < MaxSize; ++i)
                {
                    if (entry.ReqItems[i] == cost && entry.ReqItemCounts[i] == count)
                        return entry.Id;

                    if (entry.ReqCurrences[i] == cost && (entry.ReqCurrencyCounts[i] == count || entry.ReqCurrencyCounts[i] == count * 100))
                        return entry.Id;
                }
            }
            return 0;
        }
    }
}