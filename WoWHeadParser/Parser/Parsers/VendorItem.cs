
namespace WoWHeadParser.Parser.Parsers
{
    public class VendorItem
    {
        public uint Id;
        public int Avail;
        public dynamic[] Cost;

        public override bool Equals(object obj)
        {
            if (!(obj is VendorItem))
                return false;

            VendorItem item = (VendorItem)obj;
            return item.Id == Id && item.Avail == Avail;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
