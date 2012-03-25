using System;

namespace WoWHeadParser
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ParserAttribute : Attribute
    {
        public ParserAttribute(string name, int max = 0)
        {
            Name = name;
            MaxValue = max;
        }

        internal string Name { get; private set; }

        internal int MaxValue { get; private set; }

        internal Parser Parser { get; set; }
    }
}
