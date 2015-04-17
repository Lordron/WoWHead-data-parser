using System.Runtime.Serialization;

namespace WoWHeadParser.Serialization
{
    [DataContract]
    public class Entries : Header
    {
        [DataMember]
        public uint Count;

        [DataMember]
        public uint[] Data;
    }
}
