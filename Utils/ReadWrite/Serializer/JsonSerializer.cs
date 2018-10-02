using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization.Json;
using System.IO;

namespace Utils.Serializer
{
    public class JsonSerializer : ISerializer
    {
        /// <summary>
        /// JSON Serialization
        /// </summary>
        public string Serialize<T>(T objectToSerialize) where T : Serializable
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            MemoryStream ms = new MemoryStream();
            ser.WriteObject(ms, objectToSerialize);
            string jsonString = Encoding.UTF8.GetString(ms.ToArray());
            ms.Close();
            return jsonString;
        }
        /// <summary>
        /// JSON Deserialization
        /// </summary>
        public T Deserialize<T>(string textSerialized) where T : Serializable
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(textSerialized));
            T obj = (T)ser.ReadObject(ms);
            return obj;
        }
        /// <summary>
        ///  JSON list Serialization
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public string SerializeList<T>(IEnumerable<T> objectListToSerialize) where T : Serializable
        {
            StringBuilder jsonData = new StringBuilder();
            foreach(T item in objectListToSerialize)
            {
                jsonData.Append(Serialize(item));
            }
            return jsonData.ToString();
        }
        /// <summary>
        ///  JSON list Deserialization
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="textSerialized"></param>
        /// <returns></returns>
        public IEnumerable<T> DeserializeList<T>(string textSerialized) where T : Serializable
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(List<T>));
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(textSerialized));
            List < T > objs = (List<T>)ser.ReadObject(ms);
            return objs;
        }
    }
}
