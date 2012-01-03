
namespace WoWHeadParser
{
    public class Block
    {
        public Block(string page, uint entry)
        {
            Page = page;
            Entry = entry;
        }

        public string Page { get; set; }

        public uint Entry { get; set; }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return Entry.GetHashCode();
        }

        public override string ToString()
        {
            return Page;
        }

        public static bool operator ==(Block block1, Block block2)
        {
            return block1.Entry == block2.Entry;
        }

        public static bool operator !=(Block block1, Block block2)
        {
            return block1.Entry != block2.Entry;
        }
    }
}
