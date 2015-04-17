using System;

namespace WoWHeadParser.Parser
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ParserAttribute : Attribute
    {
        public ParserType ParserType;
        public uint CountLimit;
        public Type Type;

        public ParserAttribute(ParserType type, uint limit)
        {
            ParserType = type;
            CountLimit = limit;
        }

        public ParserAttribute(ParserType type)
            : this(type, 0)
        {
        }

        public override string ToString()
        {
            return ParserType.ToString();
        }
    }
}
