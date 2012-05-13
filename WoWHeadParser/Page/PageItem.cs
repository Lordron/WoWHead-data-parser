namespace WoWHeadParser.Page
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

        public PageItem(Requests requests)
                : this(requests.Id, requests.ToString())
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