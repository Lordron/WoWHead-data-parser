
namespace WoWHeadParser.Parser.Parsers
{
    public class QuestLocaleItem
    {
        public uint Id;
        public string Name;

        public override bool Equals(object obj)
        {
            if (!(obj is QuestLocaleItem))
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
