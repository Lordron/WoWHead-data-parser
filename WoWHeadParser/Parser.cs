using System.Collections.Generic;
namespace WoWHeadParser
{
    public abstract class Parser
    {
        public abstract string Parse(Queue<string> pages);

        public abstract string Address { get; }
    }
}
