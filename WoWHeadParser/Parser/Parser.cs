namespace WoWHeadParser
{
    public abstract class Parser
    {
        public Locale Locale = Locale.English;

        public abstract string BeforParsing();

        public abstract string Parse(Block block);

        public abstract string Address { get; }

        public abstract string Name { get; }

        public abstract int MaxCount { get; }
    }
}