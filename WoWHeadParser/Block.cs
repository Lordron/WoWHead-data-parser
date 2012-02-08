
namespace WoWHeadParser
{
    public class Block
    {
        public Block(string page, uint id)
        {
            Id = id;
            Page = page;
        }

        public string Page { get; set; }

        public uint Id { get; set; }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override string ToString()
        {
            return Page;
        }

        public static bool operator ==(Block block1, Block block2)
        {
            return block1.Id == block2.Id;
        }

        public static bool operator !=(Block block1, Block block2)
        {
            return block1.Id != block2.Id;
        }
    }
}
