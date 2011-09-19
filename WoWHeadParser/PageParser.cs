using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoWHeadParser
{
    internal class PageParser : Parser
    {
        public override string Parse(Queue<string> pages)
        {
            throw new NotImplementedException("NOT IMPLEMENTED");
        }

        public override string Address
        {
            get
            {
                return "wowhead.com/object=";
            }
        }
    }
}
