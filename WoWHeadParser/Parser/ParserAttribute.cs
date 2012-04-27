using System;

namespace WoWHeadParser
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ParserAttribute : Attribute
    {
        public ParserAttribute(string name, string address)
        {
            Name = name;
            Address = address;
        }

        public string Name { get; private set; }

        public string Address { get; private set; }

        public Parser Parser { get; set; }
    }
}
