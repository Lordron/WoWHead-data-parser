using System.Collections.Generic;
using System.Windows.Forms;

namespace WelfEditor
{
    public struct IdComparer : IComparer<uint>
    {
        private SortOrder _order;

        public IdComparer(SortOrder order)
        {
            _order = order;
        }

        public int Compare(uint x, uint y)
        {
            switch (_order)
            {
                case SortOrder.Ascending:
                    return x.CompareTo(y);
                case SortOrder.Descending:
                    return y.CompareTo(x);
            }

            return 0;
        }
    }
}
