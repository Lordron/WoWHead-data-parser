
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
    }
}
