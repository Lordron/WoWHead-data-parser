using System.Linq;
using System.Runtime.InteropServices;

namespace WoWHeadParser
{
    public struct Db2Header
    {
        public int Signature;
        public int RecordsCount;
        public int FieldsCount;
        public int RecordSize;
        public int StringTableSize;

        public bool IsDb2
        {
            get { return Signature == 0x32424457; }
        }

        public long DataSize
        {
            get { return RecordsCount * RecordSize; }
        }

        public long StartStringPosition
        {
            get { return DataSize + Marshal.SizeOf(typeof(Db2Header)); }
        }
    };

    public struct ItemExtendedCostEntry
    {
        public uint Id;
        public uint ReqHonorPoints;
        public uint ReqArenaPoints;
        public uint ReqArenaSlot;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public uint[] ReqItems;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public uint[] ReqItemCounts;
        public uint ReqPersonalRating;
        public uint ItemPurchaseGroup;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public uint[] ReqCurrences;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public uint[] ReqCurrencyCounts;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public uint[] Unks;

        public bool HaveCurrency(uint currency, uint count)
        {
            for (int i = 0; i < 5; ++i)
            {
                if (ReqCurrences[i] == currency && ReqCurrencyCounts[i] == count)
                    return true;
            }
            return false;
        }

        public bool HaveItem(uint item, uint count)
        {
            for (int i = 0; i < 5; ++i)
            {
                if (ReqItems[i] == item && ReqItemCounts[i] == count)
                    return true;
            }
            return false;
        }
    };
}