using System.Text;
using System.Runtime.Serialization.Json;
using System.IO;

namespace Utils.FileReaderWriter.Serialization.StandardSerializer
{
    public class JsonSerializer<T> : IStandardSerializer<T> where T : Serializable
    {
        /// <summary>
        /// JSON Serialization
        /// </summary>
        public string Serialize(T objectToSerialize)
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
        public T Deserialize(string textSerialized)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(textSerialized));
            return (T)ser.ReadObject(ms);
        }
        /// <summary>
        ///  JSON list Serialization
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public string SerializeList<Y>(Y listObjects) where Y : ListSerializable<T>
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Y));
            MemoryStream ms = new MemoryStream();
            ser.WriteObject(ms, listObjects);
            string jsonString = Encoding.UTF8.GetString(ms.ToArray());
            ms.Close();
            return jsonString;
        }
        /// <summary>
        ///  JSON list Deserialization
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="textSerialized"></param>
        /// <returns></returns>
        public Y DeserializeList<Y>(string textListSerialized) where Y: ListSerializable<T>
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Y));
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(textListSerialized));
            return (Y)ser.ReadObject(ms);
        }

        public string SerializeList<Y>(ListSerializable<T> listObjects) where Y : ListSerializable<T>
        {
            return SerializeList((Y)listObjects);
        }
    }
}
