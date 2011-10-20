
namespace WoWHeadParser
{
    public struct Block
    {
        private string _page;
        private uint _entry;

        public string Page
        {
            get { return _page; }
        }

        public uint Entry
        {
            get { return _entry; }
        }

        public Block(string page, uint entry)
        {
            _page = page;
            _entry = entry;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return _entry.GetHashCode();
        }

        public override string ToString()
        {
            return _page;
        }

        public static bool operator ==(Block block1, Block block2)
        {
            return block1._entry == block2._entry;
        }

        public static bool operator !=(Block block1, Block block2)
        {
            return block1._entry != block2._entry;
        }
    }
}
