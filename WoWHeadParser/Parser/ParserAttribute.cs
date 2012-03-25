using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WoWHeadParser
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ParserAttribute : Attribute
    {
        public ParserAttribute(string name = "", int max = 0)
        {
            Name = name;
            MaxValue = max;
        }

        internal string Name { get; private set; }

        public int MaxValue { get; private set; }
    }
}
