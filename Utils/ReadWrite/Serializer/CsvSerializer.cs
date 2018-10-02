using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Utils.Serializer;

namespace Utils.ReadWrite.Serializer
{
    public class CsvSerializer : ISerializer
    {
        /// <summary>
        /// csv separator
        /// </summary>
        private char _separator;

        /// <summary>
        /// headers to serialize
        /// </summary>
        private StringList _headers;


        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="separator"></param>
        public CsvSerializer(char separator, StringList headers = null)
        {
            _separator = separator;
            _headers = headers;
        }

        /// <summary>
        /// Deserialize Csv object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="textSerialized"></param>
        /// <returns></returns>
        public T Deserialize<T>(string textSerialized) where T : Serializable
        {
            StringList listElements = new StringList(textSerialized.Split(_separator));
            T element = (T)Activator.CreateInstance(typeof(T), new object[] { listElements });
            element.Construct();
            return element;
        }
        /// <summary>
        /// Deserialize Csv object list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="textSerialized"></param>
        /// <returns></returns>
        public IEnumerable<T> DeserializeList<T>(string textSerialized) where T : Serializable
        {
            StringList lines = new StringList(textSerialized.Split('\n'));
            List<T> elements = new List<T>();
            foreach(string text in lines)
            {
                elements.Add(Deserialize<T>(text));
            }
            return elements;
        }

        /// <summary>
        /// Serialize Csv object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objectToSerialize"></param>
        /// <returns></returns>
        public string Serialize<T>(T objectToSerialize) where T : Serializable
        {
            Type type = typeof(T);
            FieldInfo[] fields = type.GetFields();
            if (_headers != null && _headers.Count > 0)
            {
                fields = fields.Where(field => _headers.Contains(field.Name)).ToArray();
            }
            return ToCsvFields(Convert.ToString(_separator), fields, objectToSerialize);
        }

        /// <summary>
        ///  Serialize Csv object list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="listObjects"></param>
        /// <returns></returns>
        public string SerializeList<T>(IEnumerable<T> listObjects) where T : Serializable
        {
            Type type = typeof(T);
            FieldInfo[] fields = type.GetFields();
            StringBuilder csvdata = new StringBuilder();
            if (_headers != null && _headers.Count > 0)
            {
                fields = fields.Where(field => _headers.Contains(field.Name)).ToArray();
            }
            string header = string.Join(Convert.ToString(_separator), fields.Select(f => f.Name).ToArray());
            csvdata.AppendLine(header);
            foreach (var obj in listObjects)
                csvdata.AppendLine(ToCsvFields(Convert.ToString(_separator), fields, obj));
            return csvdata.ToString();
        }

        /// <summary>
        /// get fields of object
        /// </summary>
        /// <param name="separator"></param>
        /// <param name="fields"></param>
        /// <param name="o"></param>
        /// <returns></returns>
        public string ToCsvFields(string separator, FieldInfo[] fields, object o)
        {
            StringBuilder linie = new StringBuilder();
            
            foreach (var f in fields)
            {
                if (linie.Length > 0)
                    linie.Append(separator);

                var x = f.GetValue(o);

                if (x != null)
                    linie.Append(x.ToString());
            }

            return linie.ToString();
        }
    }
}
