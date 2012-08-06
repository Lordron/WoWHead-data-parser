using System.Collections.Generic;
using System.Windows.Forms;

namespace Sql
{
    public struct SqlItemComparer : IComparer<object>
    {
        private SortOrder _order;

        public SqlItemComparer(SortOrder order)
        {
            _order = order;
        }

        public int Compare(object x, object y)
        {
            switch (_order)
            {
                case SortOrder.Ascending:
                    return x.ToString().CompareTo(y);
                case SortOrder.Descending:
                    return y.ToString().CompareTo(x);
            }

            return 0;
        }
    }
}
