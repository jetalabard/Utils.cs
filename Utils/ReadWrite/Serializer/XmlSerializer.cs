using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.Serialization;
using System.IO;

namespace Utils.Serializer
{
    public class XmlSerializer : ISerializer
    {
        /// <summary>
        /// Deserialize XML object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="textSerialized"></param>
        /// <returns></returns>
        public T Deserialize<T>(string textSerialized) where T : Serializable
        {
            DataContractSerializer ser = new DataContractSerializer(typeof(T));
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(textSerialized));
            T obj = (T)ser.ReadObject(ms);
            return obj;
        }

        /// <summary>
        /// Deserialize XML object list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="textSerialized"></param>
        /// <returns></returns>
        public IEnumerable<T> DeserializeList<T>(string xmlFileContent) where T : Serializable
        {
            DataContractSerializer ser = new DataContractSerializer(typeof(List<T>));
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(xmlFileContent));
            List<T> objs = (List<T>)ser.ReadObject(ms);
            return objs;
        }

        /// <summary>
        /// serialize Xml object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objectToSerialize"></param>
        /// <returns></returns>
        public string Serialize<T>(T objectToSerialize) where T : Serializable
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
        public string SerializeList<T>(IEnumerable<T> listObject) where T : Serializable
        {
            StringBuilder xmlData = new StringBuilder();
            foreach (T item in listObject)
            {
                xmlData.Append(Serialize(item));
            }
            return xmlData.ToString();
        }
    }
}
