namespace WoWHeadParser
{
    public abstract class Parser
    {
        public abstract string Parse(Block block);

        public abstract string Address { get; }
    }
}
