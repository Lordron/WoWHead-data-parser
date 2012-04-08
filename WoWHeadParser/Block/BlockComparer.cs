using System.Collections.Generic;

namespace WoWHeadParser
{
    public struct BlockComparer : IComparer<Block>
    {
        private bool _sortDown;

        public BlockComparer(bool sortDown)
        {
            _sortDown = sortDown;
        }

        public int Compare(Block x, Block y)
        {
            if (_sortDown)
                return x.Id.CompareTo(y.Id);
            else
                return y.Id.CompareTo(x.Id);
        }
    }
}
