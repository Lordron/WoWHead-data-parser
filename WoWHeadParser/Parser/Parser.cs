namespace WoWHeadParser
{
    public abstract class Parser
    {
        public Locale Locale = Locale.English;

        public abstract string Parse(Block block);

        public abstract string Address { get; }

        public abstract string Name { get; }
    }
}