using System;
using System.Collections.Generic;

namespace WoWHeadParser
{
    public class Core : Parser
    {
        public Core()
        {
        }

        public override string Parse(List<string> datas)
        {
            throw new NotImplementedException();
        }

        public override string Adress
        {
            get
            {
                return "wowhead.com/npc=";
            }
            set
            {
                Adress = value;
            }
        }
    }
}
