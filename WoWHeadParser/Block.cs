
namespace WoWHeadParser
{
    public struct Block
    {
        public string Page;
        public uint Entry;

        public Block(string page, uint entry)
        {
            Page = page;
            Entry = entry;
        }
    }
}
