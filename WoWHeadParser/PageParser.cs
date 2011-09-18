using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoWHeadParser
{
    internal class PageParser : Parser
    {
        public override string Parse(List<string> datas)
        {
            throw new NotImplementedException();
        }

        public override string Adress
        {
            get
            {
                return "wowhead.com/object=";
            }
        }
    }
}
