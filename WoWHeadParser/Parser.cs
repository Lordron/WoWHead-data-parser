namespace WoWHeadParser
{
    public abstract class Parser
    {
        public abstract string Parse(string page, uint entry);

        public abstract string Address { get; }
    }
}
