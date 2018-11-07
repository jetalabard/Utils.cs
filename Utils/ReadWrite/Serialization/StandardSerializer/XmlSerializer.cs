using System.Text;
using System.Runtime.Serialization;
using System.IO;

namespace Utils.ReadWrite.Serialization.StandardSerializer
{
    public class XmlSerializer<T> : IStandardSerializer<T> where T : Serializable
    {
        /// <summary>
        /// Deserialize XML object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="textSerialized"></param>
        /// <returns></returns>
        public T Deserialize(string textSerialized)
        {
            DataContractSerializer ser = new DataContractSerializer(typeof(T));
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(textSerialized));
            return (T)ser.ReadObject(ms);
        }

        /// <summary>
        /// Deserialize XML object list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="textSerialized"></param>
        /// <returns></returns>
        public Y DeserializeList<Y>(string textListSerialized) where Y : ListSerializable<T>
        {
            DataContractSerializer ser = new DataContractSerializer(typeof(Y));
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(textListSerialized));
            return(Y)ser.ReadObject(ms);
        }

        /// <summary>
        /// serialize Xml object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objectToSerialize"></param>
        /// <returns></returns>
        public string Serialize(T objectToSerialize)
        {
            DataContractSerializer ser = new DataContractSerializer(typeof(T));
            MemoryStream ms = new MemoryStream();
            ser.WriteObject(ms, objectToSerialize);
            string xmlString = Encoding.UTF8.GetString(ms.ToArray());
            ms.Close();
            return xmlString;
        }
        /// <summary>
        /// serialize Xml object list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="listObject"></param>
        /// <returns></returns>
        public string SerializeList<Y>(Y listObjects) where Y : ListSerializable<T>
        {
            DataContractSerializer ser = new DataContractSerializer(typeof(Y));
            MemoryStream ms = new MemoryStream();
            ser.WriteObject(ms, listObjects);
            string xmlString = Encoding.UTF8.GetString(ms.ToArray());
            ms.Close();
            return xmlString;
        }

        public string SerializeList<Y>(ListSerializable<T> listObjects) where Y : ListSerializable<T>
        {
            return SerializeList((Y)listObjects);
        }
    }
}
