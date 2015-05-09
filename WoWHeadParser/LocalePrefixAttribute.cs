using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoWHeadParser
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class LocalePrefixAttribute : Attribute
    {
        public string Prefix;

        public LocalePrefixAttribute(string prefix)
        {
            Prefix = prefix;
        }
    }
}
