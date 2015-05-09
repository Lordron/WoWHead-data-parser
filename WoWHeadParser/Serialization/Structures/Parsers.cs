using System;
using System.Runtime.Serialization;
using WoWHeadParser.Parser;

namespace WoWHeadParser.Serialization.Structures
{
    [DataContract]
    public class ParserData : Header
    {
        [DataContract]
        public class Parser
        {
            [DataMember]
            public ParserType ParserType;

            [DataMember]
            public string Address;

            [DataMember]
            public uint CountLimit;

            [IgnoreDataMember]
            public Type Type;
        }

        [DataMember]
        public Parser[] Data;
    }
}
