using System.Collections.Generic;
using System.Windows.Forms;

namespace WoWHeadParser.Page
{
    public struct PageItemComparer : IComparer<PageItem>
    {
        private SortOrder _order;

        public PageItemComparer(SortOrder order)
        {
            _order = order;
        }

        public int Compare(PageItem x, PageItem y)
        {
            switch (_order)
            {
                case SortOrder.Ascending:
                    return x.Id.CompareTo(y.Id);
                case SortOrder.Descending:
                    return y.Id.CompareTo(x.Id);
            }

            return 0;
        }
    }
}