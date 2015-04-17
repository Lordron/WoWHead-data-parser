using System.Runtime.Serialization;

namespace WoWHeadParser.Serialization
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
