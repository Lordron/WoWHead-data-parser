using System.Collections.Generic;
namespace WoWHeadParser
{
    public abstract class Parser
    {
        public abstract string Parse(List<string> datas);

        public abstract string Adress { get; }
    }
}
