using System.Runtime.Serialization;

namespace WoWHeadParser.Serialization.Structures
{
    [DataContract]
    public class Header
    {
        [DataMember(IsRequired=true)]
        public int Version;

        [IgnoreDataMember]
        public static int CurrentVersion = 1;
    }
}
