namespace WoWHeadParser
{
    public class PageItem
    {
        public PageItem(uint id, string page)
        {
            Id = id;
            Page = page;
        }

        public PageItem(uint id)
            : this(id, string.Empty)
        {
        }

        public string Page { get; private set; }

        public uint Id { get; private set; }

        public override string ToString()
        {
            return Page;
        }
    }
}