using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.Reader;

namespace Utils.Serializer
{
    public interface ISerializer
    {
        /// <summary>
        /// serialize generic object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        string Serialize<T>(T t) where T :Serializable;

        /// <summary>
        /// serialize generic object list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        string SerializeList<T>(IEnumerable<T> t) where T : Serializable;

        /// <summary>
        /// deserialize generic object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="textSerialized"></param>
        /// <returns></returns>
        T Deserialize<T>(string textSerialized) where T : Serializable;

        /// <summary>
        /// deserialize generic object list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="textSerialized"></param>
        /// <returns></returns>
        IEnumerable<T> DeserializeList<T>(string textListSerialized) where T : Serializable;

    }
}
