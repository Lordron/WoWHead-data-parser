using System.Collections.Generic;
using System.Windows.Forms;

namespace Sql
{
    public struct SqlItemComparer : IEqualityComparer<object>
    {
        private SortOrder _order;

        public SqlItemComparer(SortOrder order)
        {
            _order = order;
        }

        public bool Equals(object x, object y)
        {
            switch (_order)
            {
                case SortOrder.Ascending:
                    return x.Equals(y);
                case SortOrder.Descending:
                    return y.Equals(x);
            }
            return false;
        }

        public int GetHashCode(object obj)
        {
            return obj.GetHashCode();
        }
    }
}
