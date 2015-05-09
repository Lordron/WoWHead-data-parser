using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace WoWHeadParser.Serialization
{
    public static class SerializationHelper
    {
        public static T SerializeFile<T>(string path)
        {
            using (StreamReader stream = new StreamReader(path))
            {
                return Serialize<T>(stream.BaseStream);
            }
        }

        public static T SerializeString<T>(string str, Encoding encoding)
        {
            using (MemoryStream stream = new MemoryStream(encoding.GetBytes(str)))
            {
                return Serialize<T>(stream);
            }
        }

        public static T Serialize<T>(Stream stream)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
            T result = (T)serializer.ReadObject(stream);
            return result;
        }
    }
}
