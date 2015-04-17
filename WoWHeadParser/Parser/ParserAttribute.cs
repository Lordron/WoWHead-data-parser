using System;

namespace WoWHeadParser.Parser
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ParserAttribute : Attribute
    {
        public ParserType ParserType;

        public ParserAttribute(ParserType type)
        {
            ParserType = type;
        }

        public override string ToString()
        {
            return ParserType.ToString();
        }
    }
}
