using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;

namespace WoWHeadParser.Serialization
{
    public static class SerializationHelper
    {
        public static T Serialize<T>(string path) where T : Header
        {
            using (StreamReader stream = new StreamReader(path))
            {
                return Serialize<T>(stream.BaseStream);
            }
        }

        public static T Serialize<T>(Stream stream) where T : Header
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
            T result = (T)serializer.ReadObject(stream);
            if (result.Version != Header.CurrentVersion)
            {
                // print error message
                return null;
            }
            return result;
        }
    }
}
