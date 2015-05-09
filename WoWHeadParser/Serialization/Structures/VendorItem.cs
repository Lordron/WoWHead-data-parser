using System.Runtime.Serialization;

namespace WoWHeadParser.Serialization.Structures
{
    [DataContract]
    public class VendorItem
    {
        [DataMember(Name = "id", IsRequired = true)]
        public int Id;

        [DataMember(Name = "avail")]
        public int Available = -1;

        [DataMember(Name = "cost")]
        public dynamic[] Cost;
    }
}
