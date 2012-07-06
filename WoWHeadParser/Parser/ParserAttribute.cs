using System;

namespace WoWHeadParser.Parser
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class ParserAttribute : Attribute
    {
        public ParserType Type { get; private set; }

        public ParserAttribute(ParserType type)
        {
            Type = type;
        }
    }
}
