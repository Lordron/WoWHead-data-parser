using System;
using System.Runtime.InteropServices;
using DBFilesClient.NET;

namespace WoWHeadParser.DBFileStorage
{
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct ItemExtendedCostEntry : IStructureFmt
    {
        public uint Id;
        public uint ReqHonorPoints;
        public uint ReqArenaPoints;
        public uint ReqArenaSlot;
        public fixed uint ReqItems[5];
        public fixed uint ReqItemCounts[5];
        public uint ReqPersonalRating;
        public uint ItemPurchaseGroup;
        public fixed uint ReqCurrences[5];
        public fixed uint ReqCurrencyCounts[5];
        public fixed uint Unks[5];

        public string Fmt
        {
            get { return "iiiiiiiiiiiiiiiiiiiiiiiiiiiiiii"; }
        }
    };

    public class ItemExtendedCost : IDBFileLoader
    {
        private DBCStorage _storage;

        #region IDBFileLoader

        public void Load()
        {
            _storage = DBFileLoader.Load<ItemExtendedCostEntry>(false);
        }

        #endregion

        public int Count { get { return _storage == null ? -1 : _storage.EntriesCount; } } 

        public const uint MaxSize = 5;

        public unsafe uint GetExtendedCost(uint cost, uint count)
        {
            uint maxId = _storage.MaxId;
            for (uint id = _storage.MinId; id <= maxId; ++id)
            {
                IntPtr entryPtr = _storage.GetEntry(id);
                if (entryPtr == IntPtr.Zero)
                    continue;

                ItemExtendedCostEntry* entry = (ItemExtendedCostEntry*)entryPtr.ToPointer();
                for (int i = 0; i < MaxSize; ++i)
                {
                    if (entry->ReqItems[i] == cost && entry->ReqItemCounts[i] == count)
                        return entry->Id;

                    if (entry->ReqCurrences[i] == cost && (entry->ReqCurrencyCounts[i] == count || entry->ReqCurrencyCounts[i] == count * 100))
                        return entry->Id;
                }
            }
            return 0;
        }
    }
}