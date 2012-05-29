namespace WoWHeadParser.Page
{
    public class PageItem
    {
        public PageItem(uint id, string page)
        {
            Id = id;
            Page = page;
        }

        public PageItem()
            : this(0, string.Empty)
        {
        }

        public uint Id { get; private set; }

        public string Page { get; set; }

        public override string ToString()
        {
            return Page;
        }
    }
}