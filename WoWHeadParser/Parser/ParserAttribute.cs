using System;

namespace WoWHeadParser.Parser
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ParserAttribute : Attribute
    {
        public ParserType ParserType;
        public uint CountLimit;
        public Type Type;

        public ParserAttribute(ParserType type)
        {
            ParserType = type;
            CountLimit = 0;
        }

        public ParserAttribute(ParserType type, uint limit)
        {
            ParserType = type;
            CountLimit = limit;
        }

        public override string ToString()
        {
            return ParserType.ToString();
        }
    }
}
