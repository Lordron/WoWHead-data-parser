using System.Collections.Generic;

namespace WoWHeadParser
{
    public struct PageItemComparer : IComparer<PageItem>
    {
        private bool _sortDown;

        public PageItemComparer(bool sortDown)
        {
            _sortDown = sortDown;
        }

        public int Compare(PageItem x, PageItem y)
        {
            if (_sortDown)
                return x.Id.CompareTo(y.Id);
            else
                return y.Id.CompareTo(x.Id);
        }
    }
}
