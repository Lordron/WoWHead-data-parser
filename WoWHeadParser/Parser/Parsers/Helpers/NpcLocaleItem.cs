
namespace WoWHeadParser.Parser.Parsers
{
    public class NpcLocaleItem
    {
        public uint Id;
        public string Name;
        public string Tag;

        public override bool Equals(object obj)
        {
            if (!(obj is NpcLocaleItem))
                return false;

            QuestLocaleItem item = (QuestLocaleItem)obj;
            return item.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
