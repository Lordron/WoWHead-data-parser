using System;

namespace WoWHeadParser.Parser
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ParserAttribute : Attribute
    {
        public ParserType Type;
        public int CountLimit;

        public ParserAttribute(ParserType type)
        {
            Type = type;
            CountLimit = 0;
        }

        public ParserAttribute(ParserType type, int limit)
        {
            Type = type;
            CountLimit = limit;
        }

        public override string ToString()
        {
            return Type.ToString();
        }
    }
}
