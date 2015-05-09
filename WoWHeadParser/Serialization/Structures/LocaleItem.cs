using System.Runtime.Serialization;

namespace WoWHeadParser.Serialization.Structures
{
    [DataContract]
    public class LocaleItem
    {
        [DataMember(Name = "id", IsRequired = true)]
        public int Id;

        [DataMember(Name = "name", IsRequired = true)]
        public string Name;

        [DataMember(Name = "tag")]
        public string Tag;
    }
}
